using System;
using System.Collections.Generic;

namespace Aliyun.FunctionCompute.SDK.Response
{
    public interface IResponseBase
    {
        void SetStatusContent(string content, int status, byte[] rawBytes);
        void SetHeaders(Dictionary<string, object> headers);
    }
}
