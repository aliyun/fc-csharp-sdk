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

    public class CreateCustomDomainRequest : RequestBase, IRequestBase
    {
        public CreateCustomDomainMeta CreateCustomDomainMeta { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public CreateCustomDomainRequest(string domainName, string protocal = null, string apiVersion = null, RouteConfig routeConfig = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(domainName) == false);

            this.CreateCustomDomainMeta = new CreateCustomDomainMeta(domainName, protocal, apiVersion, routeConfig);
            this.Headers = customHeaders;
        }


        public string GetPath()
        {
            return string.Format(Const.CUSTOM_DOMAIN_PATH, Const.API_VERSION);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("POST", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.POST);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.CreateCustomDomainMeta));

            return request;

        }

    }
}

