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

    public class UpdateAliasRequest : RequestBase, IRequestBase
    {
        public string ServiceName { get; set; }
        public string AliasName { get; set; }

        public UpdateAliasMeta UpdateAliasMeta { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public UpdateAliasRequest(string serviceName, string aliasName, string versionId, string description = null, 
            Dictionary<string, float> additionalVersionWeight = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(versionId) == false);
            Contract.Requires(string.IsNullOrEmpty(aliasName) == false);
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.AliasName = aliasName;
            this.UpdateAliasMeta = new UpdateAliasMeta(versionId, description, additionalVersionWeight);
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.SINGLE_ALIAS_PATH, Const.API_VERSION, this.ServiceName, this.AliasName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("PUT", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.PUT);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.UpdateAliasMeta));

            return request;

        }


    }
}

