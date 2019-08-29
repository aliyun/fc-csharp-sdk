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

    public class GetTagMetaWithRequestID : TagMeta
    {
        [JsonProperty("requestId")]
        public string RequestID { get; set; }

        public GetTagMetaWithRequestID()
        {
        }

        public GetTagMetaWithRequestID(string rid, string resourceArn, Dictionary<string, string> tags) :
            base(resourceArn,tags)
        {
            this.RequestID = rid;
        }
    }
}
