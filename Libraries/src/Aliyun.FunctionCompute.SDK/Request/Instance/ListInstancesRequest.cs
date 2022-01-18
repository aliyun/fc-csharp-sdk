using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using Newtonsoft.Json;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class ListInstancesRequest : RequestBase
    {
        public string ServiceName { get; set; }
        public string Qualifier { get; set; }
        public string FunctionName { get; set; }

        public int Limit { get; set; }
        public string[] InstanceIds { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public ListInstancesRequest(string serviceName, string functionName, string qualifier = null, int limit = 0, string[] instanceIds = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.FunctionName = functionName;
            this.Qualifier = qualifier;
            this.Limit = limit;
            this.InstanceIds = instanceIds;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            if (string.IsNullOrEmpty(this.Qualifier))
            {
                return string.Format(Const.SINGLE_INSTANCE_PATH, Const.API_VERSION, this.ServiceName, this.FunctionName);
            }
            else
            {
                return string.Format(Const.SINGLE_INSTANCE_WITH_QUALIFIER_PATH, Const.API_VERSION, this.ServiceName, this.Qualifier, this.FunctionName);
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
            if (this.InstanceIds != null && this.InstanceIds.Length > 0)
                request.AddQueryParameter("instanceIds", JsonConvert.SerializeObject(this.InstanceIds));

            return request;
        }

    }
}
