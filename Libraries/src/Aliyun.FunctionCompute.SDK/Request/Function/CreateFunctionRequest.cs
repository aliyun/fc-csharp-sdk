using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{

    public class CreateFunctionRequest : RequestBase
    {
        public string ServiceName { get; set; }

        public CreateFunctionMeta CreateFunctionMeta { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public CreateFunctionRequest(string serviceName, string functionName, string runtime, 
            string handler, Code code,  string desc = null,
            int memorySize=256, int timeout = 60,
            Dictionary<string, string> env = null,
            string initializer = null, int initializationTimeout=30, 
            Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(code != null);
            Contract.Requires(string.IsNullOrEmpty(handler) == false);
            Contract.Requires(string.IsNullOrEmpty(runtime) == false);
            Contract.Requires(string.IsNullOrEmpty(functionName) == false);
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.CreateFunctionMeta = new CreateFunctionMeta(functionName, runtime, handler, code, desc, memorySize, timeout, env, initializer, initializationTimeout);
            this.Headers = customHeaders;
        }


        public string GetPath()
        {
            return string.Format(Const.FUNCTION_PATH, Const.API_VERSION, this.ServiceName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("POST", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.POST);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.CreateFunctionMeta));

            return request;

        }


    }
}
