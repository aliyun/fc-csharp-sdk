using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{

    public class FunctionsResponseData
    {
        public FunctionsResponseData()
        {
            this.Functions = new FunctionMeta[] { };
            this.NextToken = null;
        }

        [JsonProperty("functions")]
        public FunctionMeta[] Functions { get; set; }

        [JsonProperty("nextToken")]
        public string NextToken { get; set; }
    }

    public class ListFunctionsResponse : IResponseBase
    {
        public string Content { get; set; }
        public Dictionary<string, object> Headers { get; set; }
        public int StatusCode { get; set; }

        public FunctionsResponseData Data { get; set; }

        public ListFunctionsResponse()
        {
            this.Data = new FunctionsResponseData();
            this.Headers = new Dictionary<string, object> { };
        }

        public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            if(status < 300)
                this.Data = JsonConvert.DeserializeObject<FunctionsResponseData>(this.Content);
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
