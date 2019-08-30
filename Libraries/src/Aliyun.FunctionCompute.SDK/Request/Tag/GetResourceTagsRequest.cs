using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class GetResourceTagsRequest : RequestBase, IRequestBase
    {

        public string ResourceArn { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public GetResourceTagsRequest(string resourceArn, Dictionary<string, string> customHeaders = null)
        {
            if (string.IsNullOrWhiteSpace(resourceArn))
            {
                throw new ArgumentException("message", nameof(resourceArn));
            }

            this.ResourceArn = resourceArn;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.TAG_PATH, Const.API_VERSION);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("GET", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.GET);

            request.AddQueryParameter("resourceArn", this.ResourceArn);
            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            return request;

        }
    }
}

