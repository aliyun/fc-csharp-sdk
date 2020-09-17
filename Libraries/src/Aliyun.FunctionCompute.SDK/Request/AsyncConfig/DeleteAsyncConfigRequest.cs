using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Auth;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class DeleteFunctionAsyncConfigRequest : RequestBase, IRequestBase
    {
        public string ServiceName { get; set; }
        public string Qualifier { get; set; }
        public string FunctionName { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public DeleteFunctionAsyncConfigRequest(string serviceName, string qualifier, string functionName,  Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            Contract.Requires(string.IsNullOrEmpty(functionName) == false);
            this.ServiceName = serviceName;
            this.FunctionName = functionName;
            this.Qualifier = qualifier;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            if (string.IsNullOrEmpty(this.Qualifier)) {
                return string.Format(Const.ASYNC_CONFIG_PATH, Const.API_VERSION, this.ServiceName, this.FunctionName);

            }
            return string.Format(Const.ASYNC_CONFIG_WITH_QUALIFIER_PATH, Const.API_VERSION, this.ServiceName, this.Qualifier, this.FunctionName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("DELETE", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.DELETE);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            return request;
        }
    }
}
