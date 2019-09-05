using System;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.model
{

    public class ReservedCapacityMeta
    {
        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }

        [JsonProperty("cu")]
        public int CU { get; set; }

        [JsonProperty("deadline")]
        public string Deadline { get; set; }

        [JsonProperty("createdTime")]
        public string CreatedTime { get; set; }

        [JsonProperty("lastModifiedTime")]
        public string LastModifiedTime { get; set; }

        [JsonProperty("isRefunded")]
        public string IsRefunded { get; set; }

        public ReservedCapacityMeta()
        {
        }
    }
}
