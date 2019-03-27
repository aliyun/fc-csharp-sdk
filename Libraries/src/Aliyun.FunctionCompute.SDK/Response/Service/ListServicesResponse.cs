using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{

    public class ServicesResponseData
    {
        public ServicesResponseData()
        {
            this.Services = new ServiceMeta[] { };
            this.NextToken = null;
        }

        [JsonProperty("services")]
        public ServiceMeta[] Services { get; set; }

        [JsonProperty("nextToken")]
        public string NextToken { get; set; }
    }

    public class ListServicesResponse : IResponseBase
    {
        public string Content { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, object> Headers { get; set; }
        public ServicesResponseData Data { get; set; }

        public ListServicesResponse()
        {
            this.Data = new ServicesResponseData();
            this.Headers = new Dictionary<string, object> { };
        }

        public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            if(status < 300)
                this.Data = JsonConvert.DeserializeObject<ServicesResponseData>(this.Content);
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }
    }
}
