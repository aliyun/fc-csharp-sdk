using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{

    public class VersionsResponseData
    {
        public VersionsResponseData()
        {
            this.Versions = new VersionMeta[] { };
            this.NextToken = null;
        }

        [JsonProperty("versions")]
        public VersionMeta[] Versions { get; set; }

        [JsonProperty("nextToken")]
        public string NextToken { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }
    }

    public class ListVersionsResponse : IResponseBase
    {
        public string Content { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, object> Headers { get; set; }
        public VersionsResponseData Data { get; set; }

        public ListVersionsResponse()
        {
            this.Data = new VersionsResponseData();
            this.Headers = new Dictionary<string, object> { };
        }

        public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            if (status < 300)
                this.Data = JsonConvert.DeserializeObject<VersionsResponseData>(this.Content);
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }
    }
}
