using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{

    public class ReservedCapacitiesResponseData
    {
        public ReservedCapacitiesResponseData()
        {
            this.ReservedCapacities = new ReservedCapacityMeta[] { };
            this.NextToken = null;
        }

        [JsonProperty("reservedCapacities")]
        public ReservedCapacityMeta[] ReservedCapacities { get; set; }

        [JsonProperty("nextToken")]
        public string NextToken { get; set; }
    }

    public class ListReservedCapacitiesResponse : IResponseBase
    {
        public string Content { get; set; }
        public Dictionary<string, object> Headers { get; set; }
        public int StatusCode { get; set; }

        public ReservedCapacitiesResponseData Data { get; set; }

        public ListReservedCapacitiesResponse()
        {
            this.Data = new ReservedCapacitiesResponseData();
            this.Headers = new Dictionary<string, object> { };
        }

        public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            if(status < 300)
                this.Data = JsonConvert.DeserializeObject<ReservedCapacitiesResponseData>(this.Content);
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }
    }
}
