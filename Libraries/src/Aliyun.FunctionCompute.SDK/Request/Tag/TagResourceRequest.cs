using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using Newtonsoft.Json;
using RestSharp;
using Aliyun.FunctionCompute.SDK.model;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class TagResourceRequest : RequestBase, IRequestBase
    {
        public Dictionary<string, string> Headers { get; set; }
        public TagMeta TagMeta { get; set; }

        public TagResourceRequest(string resourceArn, Dictionary<string, string> tags, Dictionary<string, string> customHeaders = null)
        {
            this.Headers = customHeaders;
            this.TagMeta = new TagMeta(resourceArn, tags);
        }

        public string GetPath()
        {
            return string.Format(Const.TAG_PATH, Const.API_VERSION);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("POST", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.POST);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.TagMeta));

            return request;
        }
    }
}
