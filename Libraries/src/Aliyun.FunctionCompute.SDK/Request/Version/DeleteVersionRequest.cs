using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Auth;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class DeleteVersionRequest : RequestBase, IRequestBase
    {
        public string ServiceName { get; set; }
        public string VersionID { get; set; }

        public Dictionary<string, string> Headers { get; set; }


        public DeleteVersionRequest(string serviceName, string versionID, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(versionID) == false);
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.VersionID = versionID;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            return string.Format(Const.SINGLE_VERSION_PATH, Const.API_VERSION, this.ServiceName, this.VersionID);
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
