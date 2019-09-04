using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using Newtonsoft.Json;
using RestSharp;
using Aliyun.FunctionCompute.SDK.model;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class PutProvisionConfigInput
    {
        [JsonProperty("target")]
        public int Target { get; set; }


        public PutProvisionConfigInput() { }

        public PutProvisionConfigInput(int t)
        {
           this.Target = t;
        }
    }

    public class PutProvisionConfigRequest : RequestBase, IRequestBase
    {
        public Dictionary<string, string> Headers { get; set; }
        public string ServiceName { get; set; }
        public string Qualifier { get; set; }
        public string FunctionName { get; set; }

        public PutProvisionConfigInput Input { get; set; }

        public PutProvisionConfigRequest(string serviceName, string qualifier, string functionName, int target, Dictionary<string, string> customHeaders = null)
        { 
            this.ServiceName = serviceName;
            this.Qualifier = qualifier;
            this.FunctionName = functionName;
            this.Input = new PutProvisionConfigInput(target);
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.SINGLE_PROVISION_CONFIG_PATH, Const.API_VERSION,
                           this.ServiceName, this.Qualifier, this.FunctionName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("PUT", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.PUT);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.Input));

            return request;
        }
    }
}
