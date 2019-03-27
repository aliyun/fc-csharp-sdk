using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Auth;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class GetServiceRequest: RequestBase, IRequestBase
    {
        public string ServiceName { get; set; }
        public string Qualifier { get; set; }
        public Dictionary<string, string> Headers { get ; set; }

        public GetServiceRequest(string serviceName, string qualifier = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.Qualifier = qualifier;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            if (string.IsNullOrEmpty(this.Qualifier))
            {
                return string.Format(Const.SINGLE_SERVICE_PATH, Const.API_VERSION, this.ServiceName);
            }
            else
            {
                return string.Format(Const.SINGLE_SERVICE_WITH_QUALIFIER_PATH, Const.API_VERSION, this.ServiceName, this.Qualifier);
            }
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("GET", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.GET);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            return request;

        }
    }
}
