using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Aliyun.FunctionCompute.SDK.Constants;

namespace Aliyun.FunctionCompute.SDK.Config
{
    public class FCConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aliyun.FunctionCompute.SDK.Client.Config"/> class.
        /// </summary>
        /// <param name="region">Region.</param>
        /// <param name="accountId">Account identifier.</param>
        /// <param name="accessKeyID">Access key identifier.</param>
        /// <param name="accessKeySecret">Access key secret.</param>
        /// <param name="securityToken">Security token.</param>
        /// <param name="isHttps">If set to <c>true</c> is https.</param>
        public FCConfig(string region, string accountId, string accessKeyID, string accessKeySecret, string securityToken, bool isHttps)
        {
            Contract.Requires(string.IsNullOrEmpty(accessKeyID) == false);
            Contract.Requires(string.IsNullOrEmpty(accessKeySecret) == false);
            Contract.Requires(string.IsNullOrEmpty(region) == false);
            Contract.Requires(string.IsNullOrEmpty(accountId) == false);

            string protocol = isHttps ? "https" : "http";
            this.Host = string.Format(Const.ENDPOINT_FMT, accountId, region);
            this.Endpoint = protocol + "://" + this.Host;
            this.AccountId = accountId;
            this.AccessKeyID = accessKeyID;
            this.AccessKeySecret = accessKeySecret;
            this.SecurityToken = securityToken;

            string v = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.UserAgent = "fc-dotnet-sdk-" + v;
        }

        public string AccessKeySecret { get ; set ; }
        public string AccessKeyID { get; set; }
        public string SecurityToken { get; set; }
        public string AccountId { get; set; }
        public string Endpoint { get; set; }
        public string Host { get; set; }
        public string UserAgent { get; set; }
    }
}
