using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{
    public class FunctionCodeMeta
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("checksum")]
        public string Checksum { get; set; }
    }

    public class GetFunctionCodeResponse : IResponseBase
    {
        public string Content { get; set; }
        public Dictionary<string, object> Headers { get; set; }
        public int StatusCode { get; set; }

        public FunctionCodeMeta Data { get; set; }

        public GetFunctionCodeResponse()
        {
            this.Data = new FunctionCodeMeta();
            this.Headers = new Dictionary<string, object> { };
        }

        public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            if(status < 300)
                this.Data = JsonConvert.DeserializeObject<FunctionCodeMeta>(this.Content);
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }
    }
  
}
