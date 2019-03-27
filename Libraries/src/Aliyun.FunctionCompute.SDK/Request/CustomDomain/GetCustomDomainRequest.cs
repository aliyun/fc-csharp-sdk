using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class GetCustomDomainRequest : RequestBase, IRequestBase
    {

        public object DomainName { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public GetCustomDomainRequest(string domainName, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(domainName) == false);
            this.DomainName = domainName;
            this.Headers = customHeaders;
        }

        public virtual string GetPath()
        {
            return string.Format(Const.SINGLE_CUSTOM_DOMAIN_PATH, Const.API_VERSION, this.DomainName);
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
