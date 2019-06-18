using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class ListReservedCapacitiesRequest : RequestBase, IRequestBase
    {
        public string NextToken { get; set; }
        public int Limit { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public ListReservedCapacitiesRequest(int limit = 0, string nextToken = null, Dictionary<string, string> reservedCapacityHeaders = null)
        {
            this.Limit = limit;
            this.NextToken = nextToken;
            this.Headers = reservedCapacityHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.RESERVED_CAPACITY_PATH, Const.API_VERSION);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("GET", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.GET);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            if (this.Limit > 0) request.AddQueryParameter("limit", this.Limit.ToString());
            if (!string.IsNullOrEmpty(this.NextToken)) request.AddQueryParameter("nextToken", this.NextToken);

            return request;
        }

    }
}
