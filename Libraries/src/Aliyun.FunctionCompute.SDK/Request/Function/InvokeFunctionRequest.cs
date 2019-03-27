using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class InvokeFunctionRequest : RequestBase
    {

        public string ServiceName { get; set; }
        public string FunctionName { get; set; }
        public string Qualifier { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public byte[] Payload { get; set; }

        public InvokeFunctionRequest(string serviceName, string functionName, string qualifier = null, byte[] payload = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(functionName) == false);
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.FunctionName = functionName;
            this.Qualifier = qualifier;
            this.Payload = payload;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            if (string.IsNullOrEmpty(this.Qualifier))
            {
                return string.Format(Const.INVOKE_FUNCTION_PATH, Const.API_VERSION, this.ServiceName, this.FunctionName);
            }
            else
            {
                return string.Format(Const.INVOKE_FUNCTION_WITH_QUALIFIER_PATH, Const.API_VERSION, this.ServiceName, this.Qualifier, this.FunctionName);
            }
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("POST", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.POST);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);
              
            request.AddParameter("application/octet-stream", this.Payload, ParameterType.RequestBody);

            return request;

        }
    }
}
