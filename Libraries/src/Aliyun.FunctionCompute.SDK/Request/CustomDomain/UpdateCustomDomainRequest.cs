using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{

    public class UpdateCustomDomainRequest : RequestBase, IRequestBase
    {
        public UpdateCustomDomainMeta UpdateCustomDomainMeta { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public object DomainName { get; private set; }

        public UpdateCustomDomainRequest(string domainName, string protocal = null, string apiVersion = null, RouteConfig routeConfig = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(domainName) == false);
            this.DomainName = domainName;
            this.UpdateCustomDomainMeta = new UpdateCustomDomainMeta(protocal, apiVersion, routeConfig);
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.SINGLE_CUSTOM_DOMAIN_PATH, Const.API_VERSION, this.DomainName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("PUT", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.PUT);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.UpdateCustomDomainMeta));

            return request;

        }


    }
}

