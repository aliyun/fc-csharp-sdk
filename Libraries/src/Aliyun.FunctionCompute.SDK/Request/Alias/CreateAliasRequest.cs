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

    public class CreateAliasRequest : RequestBase, IRequestBase
    {
        public string ServiceName { get; private set; }

        public CreateAliasMeta CreateAliasMeta { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public CreateAliasRequest(string serviceName, string aliasName, string versionId, string description = null, 
                Dictionary<string, float> additionalVersionWeight = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(versionId) == false);
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            Contract.Requires(string.IsNullOrEmpty(aliasName) == false);
            this.ServiceName = serviceName;
            this.CreateAliasMeta = new CreateAliasMeta(aliasName, versionId, description, additionalVersionWeight);
            this.Headers = customHeaders;
        }


        public string GetPath()
        {
            return string.Format(Const.ALIAS_PATH, Const.API_VERSION, this.ServiceName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("POST", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.POST);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.CreateAliasMeta));

            return request;

        }

    }
}

