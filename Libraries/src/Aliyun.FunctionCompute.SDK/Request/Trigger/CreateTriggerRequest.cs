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

    public class CreateTriggerRequest : RequestBase, IRequestBase
    {
        public string ServiceName { get; private set; }
        public string FunctionName { get; private set; }

        public CreateTriggerMeta CreateTriggerMeta { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public CreateTriggerRequest(string serviceName, string functionName, string triggerName, string triggerType, 
            string sourceArn, string invocationRole, object triggerConfig, string description =null, string qualifier = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(triggerType) == false);
            Contract.Requires(string.IsNullOrEmpty(triggerName) == false);
            Contract.Requires(string.IsNullOrEmpty(functionName) == false);
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.FunctionName = functionName;
            this.CreateTriggerMeta = new CreateTriggerMeta(triggerName, triggerType, sourceArn, invocationRole, triggerConfig, description, qualifier);
            this.Headers = customHeaders;
        }


        public string GetPath()
        {
            return string.Format(Const.TRIGGER_PATH, Const.API_VERSION, this.ServiceName, this.FunctionName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("POST", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.POST);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.CreateTriggerMeta));

            return request;

        }

    }
}

