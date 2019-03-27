using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Auth;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class DeleteCustomDomainRequest : RequestBase, IRequestBase
    {
        public Dictionary<string, string> Headers { get; set; }
        public object DomainName { get; set; }

        public DeleteCustomDomainRequest(string domainName, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(domainName) == false);
            this.DomainName = domainName;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.SINGLE_CUSTOM_DOMAIN_PATH, Const.API_VERSION, this.DomainName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("DELETE", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.DELETE);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            return request;

        }
    }
}