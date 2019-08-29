﻿using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.model;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{
    public class GetResourceTagsResponse : IResponseBase
    {
        public string Content { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, object> Headers { get; set; }
        public GetTagMetaWithRequestID Data { get; set; }

        public GetResourceTagsResponse()
        {
            this.Data = new GetTagMetaWithRequestID();
            this.Headers = new Dictionary<string, object> { };
        }

        virtual public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            if (status < 300)
                this.Data = JsonConvert.DeserializeObject<GetTagMetaWithRequestID>(this.Content);
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }
    }

}