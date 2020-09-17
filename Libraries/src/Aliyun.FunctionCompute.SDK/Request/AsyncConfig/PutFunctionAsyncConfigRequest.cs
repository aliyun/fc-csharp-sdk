using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using Newtonsoft.Json;
using RestSharp;
using Aliyun.FunctionCompute.SDK.model;

namespace Aliyun.FunctionCompute.SDK.Request
{

    public class PutFunctionAsyncConfigRequest : RequestBase, IRequestBase
    {
        public Dictionary<string, string> Headers { get; set; }
        public string ServiceName { get; set; }
        public string Qualifier { get; set; }
        public string FunctionName { get; set; }

        public FunctionAsyncConfigMeta AsyncConfig { get; set; }

        public PutFunctionAsyncConfigRequest(string serviceName, string qualifier, string functionName, Dictionary<string, string> customHeaders = null)
        { 
            this.ServiceName = serviceName;
            this.Qualifier = qualifier;
            this.FunctionName = functionName;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            if (string.IsNullOrEmpty(this.Qualifier))
            {
                return string.Format(Const.ASYNC_CONFIG_PATH, Const.API_VERSION, this.ServiceName, this.FunctionName);
            }
            return string.Format(Const.ASYNC_CONFIG_WITH_QUALIFIER_PATH, Const.API_VERSION, this.ServiceName, this.Qualifier, this.FunctionName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("PUT", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.PUT);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.AsyncConfig));

            return request;
        }
    }
}
