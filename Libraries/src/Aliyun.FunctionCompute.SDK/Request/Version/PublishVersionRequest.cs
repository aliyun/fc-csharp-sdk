using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class PublishVersionRequest : RequestBase, IRequestBase
    {
        public string ServiceName { get; private set; }
        public string Description { get; private set; }
        public Dictionary<string, string> Headers { get; set; }

        public PublishVersionRequest(string serviceName, string description = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.Description = description;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.SERVICE_VERSION_PATH, Const.API_VERSION, this.ServiceName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("POST", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.POST);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);


            var payLoad = new Dictionary<string, string>{
                {"description" , this.Description}
            };

            request.AddJsonBody(payLoad);

            return request;

        }

    }
}
