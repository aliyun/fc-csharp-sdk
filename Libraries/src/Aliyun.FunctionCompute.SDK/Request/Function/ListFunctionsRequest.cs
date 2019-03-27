using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class ListFunctionsRequest : RequestBase
    {
        public string ServiceName { get; set; }
        public string Qualifier { get; set; }
        public string Prefix { get; set; }
        public string StartKey { get; set; }
        public string NextToken { get; set; }
        public int Limit { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public ListFunctionsRequest(string serviceName, string qualifier = null, int limit = 0, string prefix = null, string startKey = null, string nextToken = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.Qualifier = qualifier;
            this.Limit = limit;
            this.Prefix = prefix;
            this.StartKey = startKey;
            this.NextToken = nextToken;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            if (string.IsNullOrEmpty(this.Qualifier))
            {
                return string.Format(Const.FUNCTION_PATH, Const.API_VERSION, this.ServiceName);
            }
            else
            {
                return string.Format(Const.FUNCTION_WITH_QUALIFIER_PATH, Const.API_VERSION, this.ServiceName, this.Qualifier);
            }
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("GET", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.GET);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            if (this.Limit > 0) request.AddQueryParameter("limit", this.Limit.ToString());
            if (!string.IsNullOrEmpty(this.Prefix)) request.AddQueryParameter("prefix", this.Prefix);
            if (!string.IsNullOrEmpty(this.StartKey)) request.AddQueryParameter("startKey", this.StartKey);
            if (!string.IsNullOrEmpty(this.NextToken)) request.AddQueryParameter("nextToken", this.NextToken);

            return request;
        }

    }
}
