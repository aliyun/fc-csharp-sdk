using System;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.model
{
    public class VersionMeta
    {
        [JsonProperty("versionId")]
        public string VersionId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }


        [JsonProperty("createdTime")]
        public string CreatedTime { get; set; }

        [JsonProperty("lastModifiedTime")]
        public string LastModifiedTime { get; set; }
    }
}
