using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class GetAliasRequest : RequestBase, IRequestBase
    {

        public string ServiceName { get; set; }
        public string AliasName { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public GetAliasRequest(string serviceName, string aliasName,Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(aliasName) == false);
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.AliasName = aliasName;
            this.Headers = customHeaders;
        }

        public virtual string GetPath()
        {
            return string.Format(Const.SINGLE_ALIAS_PATH, Const.API_VERSION, this.ServiceName, this.AliasName);
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
