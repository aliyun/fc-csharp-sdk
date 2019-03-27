using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Auth;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class DeleteTriggerRequest : RequestBase, IRequestBase
    {
        public string ServiceName { get; set; }
        public string FunctionName { get; set; }
        public string TriggerName { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public DeleteTriggerRequest(string serviceName, string functionName, string triggerName, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(triggerName) == false);
            Contract.Requires(string.IsNullOrEmpty(functionName) == false);
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.FunctionName = functionName;
            this.TriggerName = triggerName;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.SINGLE_TRIGGER_PATH, Const.API_VERSION, this.ServiceName, this.FunctionName, this.TriggerName);
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