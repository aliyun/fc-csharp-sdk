using System;
namespace Aliyun.FunctionCompute.SDK.Constants
{
    public static class Const
    {
        public static readonly string ENDPOINT_FMT = "{0}.{1}.fc.aliyuncs.com";

        public static readonly string ACCOUNT_SETTING_PATH = "/{0}/account-settings";

        public static readonly string SERVICE_PATH = "/{0}/services";
        public static readonly string SINGLE_SERVICE_PATH = SERVICE_PATH + "/{1}";
        public static readonly string FUNCTION_PATH = SINGLE_SERVICE_PATH + "/functions";
        public static readonly string SINGLE_FUNCTION_PATH = FUNCTION_PATH + "/{2}";
        public static readonly string CUSTOM_DOMAIN_PATH = "/{0}/custom-domains";
        public static readonly string SINGLE_CUSTOM_DOMAIN_PATH = CUSTOM_DOMAIN_PATH + "/{1}";
        public static readonly string SERVICE_VERSION_PATH = SINGLE_SERVICE_PATH + "/versions";
        public static readonly string SINGLE_VERSION_PATH = SERVICE_VERSION_PATH + "/{2}";
        public static readonly string ALIAS_PATH = SINGLE_SERVICE_PATH + "/aliases";
        public static readonly string SINGLE_ALIAS_PATH = ALIAS_PATH + "/{2}";
        public static readonly string TAG_PATH = "/{0}/tag";


        public static readonly string FUNCTION_CODE_PATH = SINGLE_FUNCTION_PATH + "/code";
        public static readonly string TRIGGER_PATH = SINGLE_FUNCTION_PATH + "/triggers";
        public static readonly string SINGLE_TRIGGER_PATH = TRIGGER_PATH + "/{3}";
        public static readonly string INVOKE_FUNCTION_PATH = SINGLE_FUNCTION_PATH + "/invocations";
        public static readonly string HTTP_INVOKE_FUNCTION_PATH = "/{0}/proxy/{1}/{2}/{3}";
        public static readonly string HTTP_INVOKE_FUNCTION_WITH_QUALIFIER_PATH = "/{0}/proxy/{1}.{2}/{3}/{4}";

        public static readonly string SINGLE_SERVICE_WITH_QUALIFIER_PATH = SERVICE_PATH + "/{1}.{2}";
        public static readonly string FUNCTION_WITH_QUALIFIER_PATH = SINGLE_SERVICE_WITH_QUALIFIER_PATH + "/functions";
        public static readonly string SINGLE_FUNCTION_WITH_QUALIFIER_PATH = FUNCTION_WITH_QUALIFIER_PATH + "/{3}";
        public static readonly string FUNCTION_CODE_WITH_QUALIFIER_PATH = SINGLE_FUNCTION_WITH_QUALIFIER_PATH + "/code";
        public static readonly string INVOKE_FUNCTION_WITH_QUALIFIER_PATH = SINGLE_FUNCTION_WITH_QUALIFIER_PATH + "/invocations";

        /*
         * 3 seconds
         *
         * Used for http request connect timeout
         */
        public static readonly int CONNECT_TIMEOUT = 60 * 1000;

        /*
         * 10 minutes 3 seconds
         *
         * Used for http request read timeout
         */
        public static readonly int READ_TIMEOUT = 10 * 60 * 1000 + 3000;

        public static readonly string API_VERSION = "2016-08-15";
        public static readonly string INVOCATION_TYPE_ASYNC = "Async";
        public static readonly string INVOCATION_TYPE_HTTP = "http";
        public static readonly string IF_MATCH_HEADER = "If-Match";

    }
}
