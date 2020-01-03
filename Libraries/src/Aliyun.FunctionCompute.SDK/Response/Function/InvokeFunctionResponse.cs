using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{
    public class InvokeFunctionResponse : IResponseBase
    {
        /// <summary>
        /// Gets or sets the content.
        /// Invoke function string result
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, object> Headers { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// Invoke function byte[] result
        /// </summary>
        /// <value>The data.</value>
        public byte[] Data { get; set; }

        public InvokeFunctionResponse()
        {
            this.Data = new byte[] { };
            this.Headers = new Dictionary<string, object> { };
        }

        public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            this.Data = rawBytes;
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }

        public string GetRequestID()
        {
            return this.Headers[Constants.HeaderKeys.REQUEST_ID].ToString();
        }
    }
}
