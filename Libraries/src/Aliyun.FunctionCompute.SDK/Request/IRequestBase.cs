using System;
using Aliyun.FunctionCompute.SDK.Config;
using RestSharp;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public interface IRequestBase
    {
        string GetPath();
        RestRequest GenHttpRequest(FCConfig cfg);
    }
}
