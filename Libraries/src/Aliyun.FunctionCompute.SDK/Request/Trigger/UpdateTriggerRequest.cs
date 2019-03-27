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

    public class UpdateTriggerRequest : RequestBase, IRequestBase
    {
        public string ServiceName { get; set; }
        public string FunctionName { get; set; }
        public string TriggerName { get; set; }

        public UpdateTriggerMeta UpdateTriggerMeta { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public UpdateTriggerRequest(string serviceName, string functionName, string triggerName,
            object triggerConfig=null, string invocationRole=null, string description = null, string qualifier = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(triggerName) == false);
            Contract.Requires(string.IsNullOrEmpty(functionName) == false);
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.FunctionName = functionName;
            this.TriggerName = triggerName;
            this.UpdateTriggerMeta = new UpdateTriggerMeta(invocationRole, triggerConfig, description, qualifier);
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.SINGLE_TRIGGER_PATH, Const.API_VERSION, this.ServiceName,this.FunctionName, this.TriggerName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("PUT", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.PUT);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.UpdateTriggerMeta));

            return request;

        }


    }
}

