Aliyun FunctionCompute C# SDK
=================================

[![Software License](https://img.shields.io/badge/license-MIT-brightgreen.svg)](LICENSE)
[![GitHub version](https://badge.fury.io/gh/aliyun%2Ffc-csharp-sdk.svg)](https://badge.fury.io/gh/aliyun%2Ffc-csharp-sdk)
[![Build Status](https://travis-ci.org/aliyun/fc-csharp-sdk.svg?branch=master)](https://travis-ci.org/aliyun/fc-csharp-sdk)

Overview
--------

The SDK of this version is dependent on the third-party library [Json.NET](https://www.newtonsoft.com/json) and [RestSharp](https://www.nuget.org/packages/RestSharp/).

Running environment
-------------------

Applicable to .net core 2.1 or above


Installation
-------------------

#### Install the SDK through NuGet
 - If NuGet hasn't been installed for Visual Studio, install [NuGet](http://docs.nuget.org/docs/start-here/installing-nuget) first. 
 
 - After NuGet is installed, access Visual Studio to create a project or open an existing project, and then select `TOOLS` > `NuGet Package Manager` > `Manage NuGet Packages for Solution`.
 
 - Type `aliyun.fc.sdk` in the search box and click *Search*, find `Aliyun.FC.SDK` or `Aliyun.FC.SDK.NetCore` in the search results, select the latest version, and click *Install*. After installation, the SDK is added to the project.

Getting started
-------------------

```csharp
using System;
using Aliyun.FunctionCompute.SDK.Client;
using Aliyun.FunctionCompute.SDK.Request;
using Aliyun.FunctionCompute.SDK.model;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace samples
{
    class Program
    {

        static void Main(string[] args)
        {
            var fcClient = new FCClient("cn-shanghai", "<your account id>", "<your ak id>", "<your ak secret>");
            var response1 = fcClient.CreateService(new CreateServiceRequest("csharp-service", "create by c# sdk") );
            Console.WriteLine(response1.Content);
            Console.WriteLine(response1.Data.ServiceName + "---" + response1.Data.Description);

            byte[] contents = File.ReadAllBytes(@"/Users/songluo/gitpro/fc-dotnet-sdk/Libraries/samples/hello2.zip");
            var code = new Code(Convert.ToBase64String(contents));
            var response2 = fcClient.CreateFunction(new CreateFunctionRequest("csharp-service", "csharp-function", "python3", "index.handler", code));
            Console.WriteLine(response2.Content);

            byte[] payload = Encoding.UTF8.GetBytes("hello csharp world");
            var response3 = fcClient.InvokeFunction(new InvokeFunctionRequest("csharp-service", "csharp-function", null, payload));
            Console.WriteLine(response3.Content);

            var customHeaders = new Dictionary<string, string> {
                {"x-fc-invocation-type", "Async"}
            };
            var response4 = fcClient.InvokeFunction(new InvokeFunctionRequest("csharp-service", "csharp-function", null, payload, customHeaders));
            Console.WriteLine(response4.StatusCode);

            var response5 = fcClient.CreateTrigger(new CreateTriggerRequest("csharp-service", "csharp-function", "my-http-trigger", "http", "dummy_arn", "", 
                                                        new HttpTriggerConfig(HttpAuthType.ANONYMOUS, new HttpMethod[] { HttpMethod.GET, HttpMethod.POST })));
            Console.WriteLine(response5.Content);

            var response6 = fcClient.DeleteTrigger(new DeleteTriggerRequest("csharp-service", "csharp-function", "my-http-trigger"));
            Console.WriteLine(response6.StatusCode);

            var response7 = fcClient.DeleteFunction(new DeleteFunctionRequest("test", "fff2"));
            Console.WriteLine(response7.StatusCode);

            var response8 = fcClient.DeleteService(new DeleteServiceRequest("csharp"));
            Console.WriteLine(response8.StatusCode);
        }
	}
}

```


Testing
-------

To run the tests, please set the access key id/secret, endpoint as environment variables.
Take the Linux system for example:

    $ export ENDPOINT=<endpoint>
    $ export ACCESS_KEY_ID=<AccessKeyId>
    $ export ACCESS_KEY_SECRET=<AccessKeySecret>


Install [xunit](https://www.nuget.org/packages/xunit/), then run the test in the following method:

    $ cd Libraries/src/Aliyun.FunctionCompute.SDK.Unittests
    $ dotnet test


More resources
--------------
- [Aliyun FunctionCompute docs](https://help.aliyun.com/product/50980.html)

Contacting us
-------------
- [Links](https://help.aliyun.com/document_detail/53087.html)
