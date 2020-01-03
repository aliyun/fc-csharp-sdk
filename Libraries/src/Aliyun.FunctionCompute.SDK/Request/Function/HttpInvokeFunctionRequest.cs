using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class HttpInvokeFunctionRequest : RequestBase
    {

        public string ServiceName { get; set; }
        public string FunctionName { get; set; }
        public string Qualifier { get; set; }
        public string RequestMethod { get; set; }
        private string path;
        public Dictionary<string, string> Headers { get; set; }
        public byte[] Payload { get; set; }
        public Dictionary<string, string[]> UnescapedQueries { get; set; }
        public string Path { get => path ?? "/";  set => path = value; }

        public HttpInvokeFunctionRequest(string serviceName, string functionName, string method, string path = null,  string qualifier = null, byte[] payload = null,
                    Dictionary<string, string[]> unescapedQueries =null , Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(method) == false);
            Contract.Requires(string.IsNullOrEmpty(functionName) == false);
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.FunctionName = functionName;
            this.Qualifier = qualifier;
            this.RequestMethod = method;
            this.Path = path;
            this.Payload = payload;
            this.UnescapedQueries = unescapedQueries;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            string _path = this.Path;
            if (this.Path.StartsWith("/", StringComparison.Ordinal))
            {
                _path = this.Path.Substring(1);
            }
            if (string.IsNullOrEmpty(this.Qualifier))
            {
                return string.Format(Const.HTTP_INVOKE_FUNCTION_PATH, Const.API_VERSION, this.ServiceName,
                    this.FunctionName, _path);
            }
            else
            {
                return string.Format(Const.HTTP_INVOKE_FUNCTION_WITH_QUALIFIER_PATH, Const.API_VERSION,
                    this,ServiceName, this.Qualifier, this.FunctionName, _path);
            }
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders(this.RequestMethod, Regex.Unescape(this.GetPath()), this.Headers, UnescapedQueries);
            var request = new RestRequest(this.GetPath(), (Method)Enum.Parse(typeof(Method), this.RequestMethod));

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            if (this.UnescapedQueries != null)
            {
                foreach (var item in this.UnescapedQueries)
                    foreach (var query in item.Value)
                        request.AddQueryParameter(item.Key, query);
            }

            request.AddParameter("application/octet-stream", this.Payload, ParameterType.RequestBody);

            return request;
        }
    }
}

