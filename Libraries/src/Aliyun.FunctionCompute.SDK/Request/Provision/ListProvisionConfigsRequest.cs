using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using Newtonsoft.Json;
using RestSharp;
using Aliyun.FunctionCompute.SDK.model;

namespace Aliyun.FunctionCompute.SDK.Request
{

    public class ListProvisionConfigsRequest : RequestBase, IRequestBase
    {
        public Dictionary<string, string> Headers { get; set; }
        public string ServiceName { get; set; }
        public string Qualifier { get; set; }
        public string NextToken { get; set; }
        public int Limit { get; set; }


        public ListProvisionConfigsRequest(string serviceName, string qualifier, int limit= 0, string nextToken=null, Dictionary<string, string> customHeaders = null)
        {
            if (string.IsNullOrEmpty(serviceName) && !string.IsNullOrEmpty(qualifier))
            {
                throw new ArgumentException("serviceName is required when qualifier is not empty", nameof(serviceName));
            }

            this.ServiceName = serviceName;
            this.Qualifier = qualifier;
            this.Limit = limit;
            this.NextToken = nextToken;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.PROVISION_CONFIGS_PATH, Const.API_VERSION);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("GET", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.GET);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddQueryParameter("serviceName", this.ServiceName);
            request.AddQueryParameter("qualifier", this.Qualifier);

            if (this.Limit > 0) request.AddQueryParameter("limit", this.Limit.ToString());
            if (!string.IsNullOrEmpty(this.NextToken)) request.AddQueryParameter("nextToken", this.NextToken);


            return request;
        }
    }
}
