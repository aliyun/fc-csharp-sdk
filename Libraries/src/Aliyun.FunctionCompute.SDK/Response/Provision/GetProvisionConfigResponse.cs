using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{

    public class GetProvisionConfigResponse : IResponseBase
    {
        public string Content { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, object> Headers { get; set; }

        public GetProvisionMeta Data { get; set; }

        public GetProvisionConfigResponse()
        {
            this.Data = new GetProvisionMeta();
            this.Headers = new Dictionary<string, object> { };
        }
        public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            if (status < 300)
                this.Data = JsonConvert.DeserializeObject<GetProvisionMeta>(this.Content);
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }
    }
}