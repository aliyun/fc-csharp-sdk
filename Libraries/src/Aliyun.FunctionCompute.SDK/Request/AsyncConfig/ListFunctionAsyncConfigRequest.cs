using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using Newtonsoft.Json;
using RestSharp;
using Aliyun.FunctionCompute.SDK.model;

namespace Aliyun.FunctionCompute.SDK.Request
{

    public class ListFunctionAsyncConfigsRequest : RequestBase, IRequestBase
    {
        public Dictionary<string, string> Headers { get; set; }
        public string ServiceName { get; set; }
        public string FunctionName { get; set; }
        public string NextToken { get; set; }
        public int Limit { get; set; }


        public ListFunctionAsyncConfigsRequest(string serviceName, string functionName, int limit= 0, string nextToken=null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            Contract.Requires(string.IsNullOrEmpty(functionName) == false);
            this.ServiceName = serviceName;
            this.FunctionName = functionName;
            this.Limit = limit;
            this.NextToken = nextToken;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.LIST_ASYNC_CONFIGS_PATH, Const.API_VERSION, this.ServiceName, this.FunctionName);
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
