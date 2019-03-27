using System;
using System.Collections.Generic;

namespace Aliyun.FunctionCompute.SDK.Response
{
    public class DeleteResponseBase : IResponseBase
    {
        public Dictionary<string, object> Headers { get; set; }
        public string Content { get; set; }
        public int StatusCode { get; set; }

        public DeleteResponseBase()
        {
            this.Headers = new Dictionary<string, object> { };
        }

        public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }
    }
}
