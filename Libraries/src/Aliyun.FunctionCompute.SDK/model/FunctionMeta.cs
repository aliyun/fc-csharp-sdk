using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.model
{
    public class Code
    {

        [JsonProperty("ossBucketName")]
        public string OssBucketName;

        [JsonProperty("ossObjectName")]
        public string OssObjectName;

        [JsonProperty("zipFile")]
        public string ZipFile;

        public Code(string zipFile, string ossBucketName=null, string ossObjectName=null)
        {
            if (ossBucketName != null && ossBucketName.Trim() == "")
            {
                throw new ArgumentException("message", nameof(ossBucketName));
            }

            if (ossObjectName != null && ossObjectName.Trim() == "")
            {
                throw new ArgumentException("message", nameof(ossObjectName));
            }

            this.OssBucketName = ossBucketName;
            this.OssObjectName = ossObjectName;
            this.ZipFile = zipFile;
            if (string.IsNullOrEmpty(this.ZipFile))
            {
                if (string.IsNullOrEmpty(this.OssBucketName) || string.IsNullOrEmpty(this.OssObjectName))
                {
                    throw new Exception("OssBucketName and OssObjectName must exist together");
                }
            }
        }
    }

    public class CustomContainerConfig
    {

        [JsonProperty("image")]
        public string Image;

        [JsonProperty("command")]
        public string Command;

        [JsonProperty("args")]
        public string Args;

        public CustomContainerConfig(string image, string cmd = null, string args = null)
        {
            this.Image = image;
            this.Command = cmd;
            this.Args = args;
        }
    }

    public class FunctionMetaBase
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("runtime")]
        public string Runtime { get; set; }

        [JsonProperty("handler")]
        public string Handler { get; set; }

        [JsonProperty("timeout")]
        public int Timeout { get; set; }

        [JsonProperty("initializer")]
        public string Initializer { get; set; }

        [JsonProperty("initializationTimeout")]
        public int? InitializationTimeout { get; set; }

        [JsonProperty("memorySize")]
        public int MemorySize { get; set; }

        //[JsonProperty("reservedContainerCount")]
        //public int? ReservedContainerCount { get; set; }

        [JsonProperty("customContainerConfig")]
        public CustomContainerConfig CustomContainerConfig { get; set; }

        [JsonProperty("caPort")]
        public int? CAPort { get; set; }

        [JsonProperty("environmentVariables")]
        public Dictionary<string, string> EnvironmentVariables { get; set; }

        public FunctionMetaBase() { }

        public FunctionMetaBase(string runtime, string handler, string desc = null,
            int memorySize = 256, int timeout = 60, Dictionary<string, string> env = null,
            string initializer = null, int initializationTimeout = 30)
        {
            this.Runtime = runtime;
            this.Handler = handler;
            this.Description = desc;
            this.MemorySize = memorySize;
            this.Timeout = timeout;
            this.EnvironmentVariables = env;
            this.Initializer = initializer;
            this.InitializationTimeout = initializationTimeout;
        }
    }

    public class UpdateFunctionMeta : FunctionMetaBase
    {
        [JsonProperty("code")]
        public Code Code;

        public UpdateFunctionMeta() { }

        public UpdateFunctionMeta(string runtime, string handler, Code code, string desc = null,
                int memorySize = 256, int timeout = 60, Dictionary<string, string> env = null,
                string initializer = null, int initializationTimeout = 30)
            :base(runtime, handler, desc, memorySize, timeout, env, initializer, initializationTimeout)
        {
            this.Code = code;
        }
    }

    public class CreateFunctionMeta : UpdateFunctionMeta
    {
        [JsonProperty("functionName")]
        public string FunctionName { get; set; }

        public CreateFunctionMeta() { }

        public CreateFunctionMeta(string functionName,string runtime, string handler, Code code, string desc = null,
                int memorySize = 256, int timeout = 60, Dictionary<string, string> env = null,
                string initializer = null, int initializationTimeout = 30)
            : base(runtime, handler,code, desc, memorySize, timeout, env, initializer, initializationTimeout)
        {
            this.FunctionName = functionName;
        }
    }


    public class FunctionMeta : FunctionMetaBase
    {
        [JsonProperty("functionId")]
        public string FunctionId { get; set; }

        [JsonProperty("functionName")]
        public string FunctionName { get; set; }

        [JsonProperty("codeSize")]
        public int CodeSize { get; set; }

        [JsonProperty("codeChecksum")]
        public string CodeChecksum { get; set; }

        [JsonProperty("createdTime")]
        public string CreatedTime { get; set; }

        [JsonProperty("lastModifiedTime")]
        public string LastModifiedTime { get; set; }

        public FunctionMeta() { }
    }

}
