using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.model
{
    public class InstanceMeta
    {
        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }

        [JsonProperty("versionId")]
        public string VersionId { get; set; }

        public InstanceMeta() { }
    }

}
