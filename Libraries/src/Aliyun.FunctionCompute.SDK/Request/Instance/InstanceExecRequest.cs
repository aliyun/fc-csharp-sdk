using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Constants;
using Aliyun.FunctionCompute.SDK.Utils;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{

    public interface ExecWebSocket
    {
        void Send(byte[] msg);
        void Close();
    }

    public interface ExecCallback
    {
        void OnOpen(ExecWebSocket sender);
        void OnStdout(ExecWebSocket sender, byte[] data);
        void OnStderr(ExecWebSocket sender, byte[] data);
        void OnServerErr(ExecWebSocket sender, byte[] data);
        void OnClose(ExecWebSocket sender);
    }

    public class InstanceExecRequest : RequestBase
    {
        public string ServiceName { get; set; }
        public string Qualifier { get; set; }
        public string InstanceId { get; set; }
        public string FunctionName { get; set; }
        public bool Stdin { get; set; }
        public bool Stdout { get; set; }
        public bool Stderr { get; set; }
        public bool Tty { get; set; }
        public int IdleTimeout { get; set; }
        public string[] Command
        { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public InstanceExecRequest(string serviceName, string functionName, string instanceId, string[] command, string qualifier = null, bool stdin = true, bool stdout = true, bool stderr = true, bool tty = true, int idleTimeout = 120, Dictionary<string, string> customHeaders = null)
        {
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);
            this.ServiceName = serviceName;
            this.FunctionName = functionName;
            this.InstanceId = instanceId;
            this.Qualifier = qualifier;
            this.Command = command;
            this.Stdin = stdin;
            this.Stdout = stdout;
            this.Stderr = stderr;
            this.Tty = tty;
            this.IdleTimeout = idleTimeout;
            this.Headers = customHeaders;
        }

        public string GetPath()
        {
            if (string.IsNullOrEmpty(this.Qualifier))
            {
                return string.Format(Const.SINGLE_INSTANCE_EXEC_PATH, Const.API_VERSION, this.ServiceName, this.FunctionName, this.InstanceId);
            }
            else
            {
                return string.Format(Const.SINGLE_INSTANCE_EXEC_WITH_QUALIFIER_PATH, Const.API_VERSION, this.ServiceName, this.Qualifier, this.FunctionName, this.InstanceId);
            }
        }


        private string encodeQuery(string key, string value)
        {
            return string.Format("{0}={1}", System.Web.HttpUtility.UrlEncode(key), System.Web.HttpUtility.UrlEncode(value));
        }

        public string GetQueries()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(encodeQuery("stdin", this.Stdin ? "true" : "false"));
            sb.Append("&");
            sb.Append(encodeQuery("stdout", this.Stdout ? "true" : "false"));
            sb.Append("&");
            sb.Append(encodeQuery("stderr", this.Stderr ? "true" : "false"));
            sb.Append("&");
            sb.Append(encodeQuery("tty", this.Tty ? "true" : "false"));
            sb.Append("&");
            sb.Append(encodeQuery("idleTimeout", this.IdleTimeout.ToString()));
            sb.Append("&");
            if (this.Command != null && this.Command.Length > 0)
            {
                foreach (var cmd in this.Command)
                {
                    sb.Append(encodeQuery("command", cmd));
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        public RestRequest GenHttpRequest(FCConfig cfg)
        {
            this.Config = cfg;
            this.Headers = this.BuildCommonHeaders("GET", this.GetPath(), this.Headers);
            var request = new RestRequest(this.GetPath(), Method.GET);

            foreach (var item in this.Headers)
                request.AddHeader(item.Key, item.Value);

            request.AddQueryParameter("stdin", this.Stdin ? "true" : "false");
            request.AddQueryParameter("stdout", this.Stdout ? "true" : "false");
            request.AddQueryParameter("stderr", this.Stderr ? "true" : "false");
            request.AddQueryParameter("tty", this.Tty ? "true" : "false");
            request.AddQueryParameter("idleTimeout", this.IdleTimeout.ToString());
            if (this.Command != null && this.Command.Length > 0)
            {
                foreach (var cmd in this.Command)
                {
                    request.AddQueryParameter("command", cmd);
                }
            }



            return request;
        }


        public string GetSign(Dictionary<string, string> headers)
        {
            var h = new Dictionary<string, string>();
            if (headers != null)
                h = Helper.MergeDictionary(h, headers);
            if (this.Headers != null)
                h = Helper.MergeDictionary(h, this.Headers);
            return Auth.SignRequest("GET", this.GetPath(), h, null);
        }
    }
}
