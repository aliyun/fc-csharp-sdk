using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{

    public class CustomDomainsResponseData
    {
        public CustomDomainsResponseData()
        {
            this.CustomDomains = new CustomDomainMeta[] { };
            this.NextToken = null;
        }

        [JsonProperty("customDomains")]
        public CustomDomainMeta[] CustomDomains { get; set; }

        [JsonProperty("nextToken")]
        public string NextToken { get; set; }
    }

    public class ListCustomDomainsResponse : IResponseBase
    {
        public string Content { get; set; }
        public Dictionary<string, object> Headers { get; set; }
        public int StatusCode { get; set; }

        public CustomDomainsResponseData Data { get; set; }

        public ListCustomDomainsResponse()
        {
            this.Data = new CustomDomainsResponseData();
            this.Headers = new Dictionary<string, object> { };
        }

        public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            if(status < 300)
                this.Data = JsonConvert.DeserializeObject<CustomDomainsResponseData>(this.Content);
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }
    }
}
