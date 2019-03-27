using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.model
{
    public class UpdateAliasMeta
    {
        [JsonProperty("versionId")]
        public string VersionId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("additionalVersionWeight")]
        public Dictionary<string, float> AdditionalVersionWeight { get; set; }


        public UpdateAliasMeta(string versionId, string description = null, Dictionary<string, float> additionalVersionWeight = null)
        {
            this.VersionId = versionId;
            this.Description = description;
            this.AdditionalVersionWeight = additionalVersionWeight;
        }

        public UpdateAliasMeta() { }
    }

    public class CreateAliasMeta : UpdateAliasMeta
    { 

        [JsonProperty("aliasName")]
        public string AliasName { get; set; }

        public CreateAliasMeta(string aliasName, string versionId, string description=null, Dictionary<string, float> additionalVersionWeight=null)
        : base(versionId, description, additionalVersionWeight)
        {
            this.AliasName = aliasName;
        }

        public CreateAliasMeta() { }
    }

    public class AliasMeta: CreateAliasMeta
    {
        [JsonProperty("createdTime")]
        public string CreatedTime { get; set; }

        [JsonProperty("lastModifiedTime")]
        public string LastModifiedTime { get; set; }

        public AliasMeta() { }
    }
}
