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
    public class UpdateServiceRequest : RequestBase, IRequestBase
    {
        public UpdateServiceMeta UpdateServiceMeta { get; set; }

        public Dictionary<string, string> Headers { get; set; }
        public string ServiceName { get; private set; }

        public UpdateServiceRequest(string serviceName, string description = "", string role = null, LogConfig logConfig = null,
            bool internetAccess = true, VpcConfig vpcConfig = null, NasConfig nasConfig = null, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.UpdateServiceMeta = new UpdateServiceMeta(description, role, logConfig, internetAccess, vpcConfig, nasConfig);

            this.Headers = customHeaders;
        }


        public string GetPath()
        {
            return string.Format(Const.SINGLE_SERVICE_PATH, Const.API_VERSION, this.ServiceName);
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("PUT", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.PUT);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddJsonBody(JsonConvert.SerializeObject(this.UpdateServiceMeta));

            return request;

        }


    }
}
