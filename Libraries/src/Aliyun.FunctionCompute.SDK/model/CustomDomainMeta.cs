using System;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.model
{
    public class PathConfig
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("serviceName")]
        public string ServiceName { get; set; }

        [JsonProperty("functionName")]
        public string FunctionName { get; set; }

        [JsonProperty("qualifier")]
        public string Qualifier { get; set; }
    }

    public class RouteConfig
    {
        [JsonProperty("routes")]
        public PathConfig[] Routes { get; set; }
    }

    public class CertConfig
    {
        [JsonProperty("certName")]
        public string CertName { get; set; }

        [JsonProperty("privateKey")]
        public string PrivateKey { get; set; }

        [JsonProperty("certificate")]
        public string Certificate { get; set; }

    }

    public class UpdateCustomDomainMeta
    {
        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }

        [JsonProperty("routeConfig")]
        public RouteConfig RouteConfig { get; set; }

        [JsonProperty("certConfig")]
        public CertConfig CertConfig { get; set; }

        public UpdateCustomDomainMeta()
        {
        }

        public UpdateCustomDomainMeta(string protocal=null, string apiVersion=null, RouteConfig routeConfig =null, CertConfig certConfig = null)
        {
            this.Protocol = protocal;
            this.ApiVersion = apiVersion;
            this.RouteConfig = routeConfig;
            this.CertConfig = certConfig;
        }
    }

    public class CreateCustomDomainMeta : UpdateCustomDomainMeta
    {
        [JsonProperty("domainName")]
        public string DomainName { get; set; }


        public CreateCustomDomainMeta()
        {
        }

        public CreateCustomDomainMeta(string domainName, string protocal = null, string apiVersion = null, RouteConfig routeConfig = null, CertConfig certConfig = null) :
            base(protocal,apiVersion ,routeConfig, certConfig) 
        {
            this.DomainName = domainName;
        }
    }


    public class CustomDomainMeta: CreateCustomDomainMeta
    {
        [JsonProperty("createdTime")]
        public string CreatedTime { get; set; }

        [JsonProperty("lastModifiedTime")]
        public string LastModifiedTime { get; set; }

        public CustomDomainMeta()
        {
        }
    }
}
