using System;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.model
{
    public class DestinationMeta
    {
        [JsonProperty("destination")]
        public string Destination { get; set; }

        public DestinationMeta() { }
    }

    public class DestinationConfigMeta
    {
        [JsonProperty("onSuccess")]
        public DestinationMeta OnSuccess { get; set; }

        [JsonProperty("onFailure")]
        public DestinationMeta OnFailure { get; set; }

        public DestinationConfigMeta() { }
    }

    public class FunctionAsyncConfigMeta
    {
        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("function")]
        public string Function { get; set; }

        [JsonProperty("qualifier")]
        public string Qualifier { get; set; }

        [JsonProperty("lastModifiedTime")]
        public string LastModifiedTime { get; set; }

        [JsonProperty("createdTime")]
        public string CreatedTime { get; set; }

        [JsonProperty("destinationConfig")]
        public DestinationConfigMeta DestinationConfig { get; set; }

        [JsonProperty("maxAsyncEventAgeInSeconds")]
        public int MaxAsyncEventAgeInSeconds { get; set; }

        [JsonProperty("maxAsyncRetryAttempts")]
        public int MaxAsyncRetryAttempts { get; set; }

        public FunctionAsyncConfigMeta(){}
    }


    public class ListFunctionAsyncConfigsMeta
    {
        [JsonProperty("nextToken")]
        public string NextToken { get; set; }

        [JsonProperty("configs")]
        public FunctionAsyncConfigMeta[] AsyncConfigs { get; set; }
    }
}
