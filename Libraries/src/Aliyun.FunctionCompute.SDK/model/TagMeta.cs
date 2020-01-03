using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.model
{
    public class TagMeta
    {
        [JsonProperty("resourceArn")]
        public string ResourceArn { get; set; }

        [JsonProperty("tags")]
        public Dictionary<string, string> Tags { get; set; }

        public TagMeta() { }

        public TagMeta(string resourceArn, Dictionary<string, string> tags)
        {
            this.ResourceArn = resourceArn;
            this.Tags = tags;
        }
    }

    public class GetTagMeta : TagMeta
    {

        public GetTagMeta()
        {
        }

        public GetTagMeta(string resourceArn, Dictionary<string, string> tags) :
            base(resourceArn,tags)
        {
        }
    }
}
