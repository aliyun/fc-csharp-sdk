using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Aliyun.FunctionCompute.SDK.Constants;

namespace Aliyun.FunctionCompute.SDK.Request
{
    public class GetFunctionCodeRequest : GetFunctionRequest
    {
        public GetFunctionCodeRequest(string serviceName, string functionName, string qualifier = null,
            Dictionary<string, string> customHeaders = null) : base(serviceName, functionName, qualifier, customHeaders)
            {
            Contract.Requires(string.IsNullOrEmpty(functionName) == false);
            Contract.Requires(string.IsNullOrEmpty(serviceName) == false);

        }

        public override string GetPath()
        {
            if (string.IsNullOrEmpty(this.Qualifier))
            {
                return string.Format(Const.FUNCTION_CODE_PATH, Const.API_VERSION, this.ServiceName, this.FunctionName);
            }
            else
            {
                return string.Format(Const.FUNCTION_CODE_WITH_QUALIFIER_PATH, Const.API_VERSION, this.ServiceName, this.Qualifier, this.FunctionName);
            }
        }

    }
}
