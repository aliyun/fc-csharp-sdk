using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using Newtonsoft.Json;
using RestSharp;
using Aliyun.FunctionCompute.SDK.model;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class UnTagInput
    {
        [JsonProperty("resourceArn")]
        public string ResourceArn { get; set; }

        [JsonProperty("tagKeys")]
        public string[] TagKeys { get; set; }

        [JsonProperty("all")]
        public bool All { get; set; }

        public UnTagInput(string resourceArn, string[] keys, bool all)
        {
            this.ResourceArn = resourceArn;
            this.TagKeys = keys;
            this.All = all;
        }
    }

    public class UntagResourceRequest : RequestBase, IRequestBase
    {
        public Dictionary<string, string> Headers { get; set; }
        public UnTagInput UnTagInput { get; set; }

        public UntagResourceRequest(string resourceArn, string[] keys, bool all, Dictionary<string, string> customHeaders = null)
        {
            this.Headers = customHeaders;
            this.UnTagInput = new UnTagInput(resourceArn, keys, all);
        }

        public string GetPath()
        {
            return string.Format(Const.TAG_PATH, Const.API_VERSION);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("DELETE", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.DELETE);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.UnTagInput));

            return request;
        }
    }
}

