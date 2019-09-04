using System;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.model
{
    public class ProvisionMeta
    {
        [JsonProperty("target")]
        public int Target { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        public ProvisionMeta(){}
    }

    public class GetProvisionMeta : ProvisionMeta
    {
        [JsonProperty("current")]
        public string Current { get; set; }

        public GetProvisionMeta(){ }
    }

    public class ListProvisionMeta
    {
        [JsonProperty("nextToken")]
        public string NextToken { get; set; }

        [JsonProperty("provisionConfigs")]
        public GetProvisionMeta[] ProvisionConfigs { get; set; }
    }
}
