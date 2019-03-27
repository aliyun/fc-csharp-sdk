using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{

    public class AliasesResponseData
    {
        public AliasesResponseData()
        {
            this.Aliases = new AliasMeta[] { };
            this.NextToken = null;
        }

        [JsonProperty("aliases")]
        public AliasMeta[] Aliases { get; set; }

        [JsonProperty("nextToken")]
        public string NextToken { get; set; }
    }

    public class ListAliasesResponse : IResponseBase
    {
        public string Content { get; set; }
        public Dictionary<string, object> Headers { get; set; }
        public int StatusCode { get; set; }

        public AliasesResponseData Data { get; set; }

        public ListAliasesResponse()
        {
            this.Data = new AliasesResponseData();
            this.Headers = new Dictionary<string, object> { };
        }

        public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            if(status < 300)
                this.Data = JsonConvert.DeserializeObject<AliasesResponseData>(this.Content);
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }
    }
}
