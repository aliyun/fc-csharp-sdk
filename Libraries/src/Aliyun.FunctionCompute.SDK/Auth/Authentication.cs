using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using Aliyun.FunctionCompute.SDK.Config;

namespace Aliyun.FunctionCompute.SDK.Auth
{
    public class Authentication
    {
        public FCConfig Config { get; set; }

        public Authentication(FCConfig cfg)
        {
            this.Config = cfg;
        }
        /// <summary>
        /// Signs the request.
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="method">Method.</param>
        /// <param name="unescapedPath">Unescaped path.</param>
        /// <param name="headers">Headers.</param>
        /// <param name="unescapedQueries">Unescaped queries.</param>
        public string SignRequest(string method, string unescapedPath, Dictionary<string, string> headers, Dictionary<string, string[]> unescapedQueries)
        {
            if (headers.TryGetValue("content-md5", out string contentMd5)) { }

            if (headers.TryGetValue("content-type", out string contentType)) { }

            if (headers.TryGetValue("date", out string date)) { }

            string canonicalHeaders = this.BuildCanonicalHeaders(headers);
            string canonicalResource = unescapedPath;

            if (unescapedQueries != null)
            {
                canonicalResource = this.GetSignResource(unescapedPath, unescapedQueries);
            }

            string stringToSign = method.ToUpper() + "\n" + contentMd5 + "\n" + contentType + "\n" + date + "\n" + canonicalHeaders + canonicalResource;

            return "FC " + this.Config.AccessKeyID + ':' + this.HmacSHA256ToBase64(stringToSign, this.Config.AccessKeySecret);
        }

        /// <summary>
        /// Gets the sign resource.
        /// </summary>
        /// <returns>The sign resource.</returns>
        /// <param name="unescapedPath">Unescaped path.</param>
        /// <param name="unescapedQueries">Unescaped queries.</param>
        internal string GetSignResource(string unescapedPath, Dictionary<string, string[]>  unescapedQueries)
        {
            Contract.Requires(unescapedQueries != null);
            List<string> param = new List<string>();

            // http trigger special sign
            if (unescapedQueries != null)
            {
                foreach(var item in unescapedQueries)
                {
                    if(item.Value.Length > 0)
                    {
                        foreach(string v in item.Value)
                        {
                            param.Add(string.Format("{0}={1}", item.Key, v));
                        }
                    }
                    else
                    {
                        param.Add(item.Key);
                    }
                }
            }
            string resource = unescapedPath + "\n" + string.Join("\n", param.ToArray());
            return resource;
        }

        /// <summary>
        /// Hmacs the SHA 256 to base64.
        /// </summary>
        /// <returns>The SHA 256 to base64.</returns>
        /// <param name="message">Message.</param>
        /// <param name="secret">Secret.</param>
        internal string HmacSHA256ToBase64(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        /// <summary>
        /// Builds the canonical headers.
        /// </summary>
        /// <returns>The canonical headers.</returns>
        /// <param name="headers">Headers.</param>
        internal string BuildCanonicalHeaders(Dictionary<string, string> headers)
        {

            Dictionary<string, string> fcHeaders = new Dictionary<string, string> { };

            foreach (var item in headers)
            {
                //Console.WriteLine(item.Key + item.Value);
                string key = item.Key.ToLower();
                if (key.StartsWith("x-fc-", StringComparison.Ordinal))
                {
                    fcHeaders.Add(key, item.Value);
                }
            }

            Dictionary<string, string> canonicalHeaders = fcHeaders.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);

            if(canonicalHeaders.Count > 0)
            {
                string[] headersArr = new string[canonicalHeaders.Count];
                int i = 0;
                foreach (var item in canonicalHeaders)
                {
                    headersArr[i++] = item.Key + ":" + item.Value;
                }
                return string.Join("\n", headersArr) + "\n";
            }

            return "";
        }
    }
}
