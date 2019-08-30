using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{
    public class TagRespBaseMeta
    {
        [JsonProperty("requestId")]
        public string RequestId { get; set; }
    }

    public class TagResponseBase : IResponseBase
    {
        public string Content { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, object> Headers { get; set; }

        public TagRespBaseMeta Data { get; set; }

        public TagResponseBase()
        {
            this.Data = new TagRespBaseMeta();
            this.Headers = new Dictionary<string, object> { };
        }
public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            if (status < 300)
                this.Data = JsonConvert.DeserializeObject<TagRespBaseMeta>(this.Content);
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }
    }
}