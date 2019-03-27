using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class ListVersionsRequest : RequestBase, IRequestBase
    {
        public string ServiceName { get; set; }
        public string Direction { get; set; }
        public string StartKey { get; set; }
        public string NextToken { get; set; }
        public int Limit { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public const string LIST_DIRECTION_BACKWARD = "BACKWARD";
        public const string LIST_DIRECTION_FORWARD = "FORWARD";

        public ListVersionsRequest(string serviceName, int limit = 0, string startKey = null, string nextToken = null, string direction = LIST_DIRECTION_BACKWARD, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.Limit = limit;
            this.Direction = direction;
            this.StartKey = startKey;
            this.NextToken = nextToken;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.SERVICE_VERSION_PATH, Const.API_VERSION, this.ServiceName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("GET", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.GET);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            if (this.Limit > 0) request.AddQueryParameter("limit", this.Limit.ToString());
            if (!string.IsNullOrEmpty(this.Direction)) request.AddQueryParameter("direction", this.Direction);
            if (!string.IsNullOrEmpty(this.StartKey)) request.AddQueryParameter("startKey", this.StartKey);
            if (!string.IsNullOrEmpty(this.NextToken)) request.AddQueryParameter("nextToken", this.NextToken);

            return request;
        }

    }
}
