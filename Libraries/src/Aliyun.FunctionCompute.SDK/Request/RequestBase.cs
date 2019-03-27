using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.Auth;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Utils;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class RequestBase
    {
        public static Authentication Auth { get; set; }

        private FCConfig config;

        public FCConfig Config
        {
            get { return config; }
            set
            {
                if (value != config) Auth = new Authentication(value);
                config = value;
            }
        }

        protected Dictionary<string, string> BuildCommonHeaders(string method, string path, Dictionary<string, string> customHeaders = null, Dictionary<string, string[]> unescapedQueries = null)
        {
            Dictionary<string, string> headers = new Dictionary<string, string> {
                { "host", this.Config.Host},
                { "date", DateTime.Now.ToUniversalTime().ToString("r")},
                { "content-type", "application/json"},
                { "content-length", "0"},
                { "user-agent", this.Config.UserAgent},
            };

            if (this.Config.SecurityToken != "")
                headers.Add("x-fc-security-token", this.Config.SecurityToken);

            if (customHeaders != null)
                headers = Helper.MergeDictionary(headers, customHeaders);

            headers.Add("authorization", Auth.SignRequest(method, path, headers, unescapedQueries));

            return headers;
        }
    }
}
