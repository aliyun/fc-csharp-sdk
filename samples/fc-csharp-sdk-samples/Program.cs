using System;
using Aliyun.FunctionCompute.SDK.Client;
using Aliyun.FunctionCompute.SDK.Request;
using Aliyun.FunctionCompute.SDK.model;
using System.IO;
using System.Collections.Generic;
using System.Text;

// more sample code can refrence 
// https://github.com/aliyun/fc-csharp-sdk/tree/master/Libraries/src/Aliyun.FunctionCompute.SDK.Unittests  

namespace fc_csharp_sdk_samples
{
    class Program
    {

        static void Main(string[] args)
        {
            var fcClient = new FCClient("cn-shanghai", "<your account id>", "<your ak id>", "<your ak secret>");
            var response1 = fcClient.CreateService(new CreateServiceRequest("csharp-service", "create by c# sdk"));
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
