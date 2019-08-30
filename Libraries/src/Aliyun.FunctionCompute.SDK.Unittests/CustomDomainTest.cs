using System;
using System.Collections.Generic;
using Aliyun.FunctionCompute.SDK.model;
using Aliyun.FunctionCompute.SDK.Request;
using Xunit;

namespace Aliyun.FunctionCompute.SDK.Unittests
{
    [Collection("fcDotnet.Unittests")]
    public class CustomDomainTestUnitTests : IDisposable
    {
        readonly TestConfig tf;

        public List<string> Domains = new List<string>();


        readonly string SERVER_CERT = @"-----BEGIN CERTIFICATE-----
MIID3zCCAsegAwIBAgIBATANBgkqhkiG9w0BAQsFADAxMSAwHgYDVQQDDBdUTFNH
ZW5TZWxmU2lnbmVkdFJvb3RDQTENMAsGA1UEBwwEJCQkJDAgFw0xOTA1MDcxMTU3
MzZaGA8yMTE5MDQxMzExNTczNlowVzFEMEIGA1UEAww7Ki5jbi1ob25na29uZy4x
MjIxOTY4Mjg3NjQ2MjI3LmNuYW1lLXRlc3QuZmMuYWxpeXVuLWluYy5jb20xDzAN
BgNVBAoMBnNlcnZlcjCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBANLB
yRAr2XeX35HCtMBnBi1aiip4Aw410GfMeRDKEY4bnrWvAcqXDMCa/6YSafc1Qc42
BuuoWHaW50LxfvhJQLGyvrqf/utKK4CKG/6FctE0RZqOl1mfaqg41bf0/CN6u0U8
TFNKsMbnf4dHX6ijoBbN85qO+Eem1nfrHJ49fd2U2kpm5RiR1wf8RoOhNBkRp6sB
YZIwhL0d1NcV4NDtznvpMgyymZ19fmKzmfT09Pfl8W/1WP333wlTKvUDwfwVI7Cn
CF2FbgeZyuPXhstapN0OPCLwpc/LEMU3d1pPlSuDCcr/bM7bJjiTcChCM8PPy9cY
Y35wWXs1/BMV0DqvGC8CAwEAAaOB2TCB1jAJBgNVHRMEAjAAMAsGA1UdDwQEAwIF
oDATBgNVHSUEDDAKBggrBgEFBQcDATBnBgNVHREEYDBegjsqLmNuLWhvbmdrb25n
LjEyMjE5NjgyODc2NDYyMjcuY25hbWUtdGVzdC5mYy5hbGl5dW4taW5jLmNvbYIU
YWxpLWx1b3NvbmdNYWMubG9jYWyCCWxvY2FsaG9zdDAdBgNVHQ4EFgQU9SBQBxy4
8i/yffJr7BgTGDv8YfcwHwYDVR0jBBgwFoAUwuBRRQqdUHVruV0qV86WN6SN04Iw
DQYJKoZIhvcNAQELBQADggEBAETlFsUgTEVj88Fg7/AhQK31846z8v1vWvtI8aiK
Z50m7gH7neCE3pKhU+K/GC85ft0TZvlM1EDRFKyduQiVc2pNdAjtR3FsUtkjkk5H
/WJwojKw1kU0B4BO2z7QYzAb7ns3Err9hHSciCY5hYel9YDMNpfjWjQYsKeAL6gL
sO1ac18LcDkuIVi1lOHqEgqJY9odMkmfDzBd1mLMc7QTbHUx7hp1vt56CqKHVt3N
CI5+nkOs34a0ZoHZG5HlpO41x9VPRQ84wRUbu053PWQzwoGaSmKRQziNiopID9yQ
MTEgXpjRsvTvqGRY56hGqHFhQueFxAMc+C6rbOiMewQ/bsU=
-----END CERTIFICATE-----
";

        readonly string SERVER_KEY = @"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEA0sHJECvZd5ffkcK0wGcGLVqKKngDDjXQZ8x5EMoRjhueta8B
ypcMwJr/phJp9zVBzjYG66hYdpbnQvF++ElAsbK+up/+60orgIob/oVy0TRFmo6X
WZ9qqDjVt/T8I3q7RTxMU0qwxud/h0dfqKOgFs3zmo74R6bWd+scnj193ZTaSmbl
GJHXB/xGg6E0GRGnqwFhkjCEvR3U1xXg0O3Oe+kyDLKZnX1+YrOZ9PT09+Xxb/VY
/fffCVMq9QPB/BUjsKcIXYVuB5nK49eGy1qk3Q48IvClz8sQxTd3Wk+VK4MJyv9s
ztsmOJNwKEIzw8/L1xhjfnBZezX8ExXQOq8YLwIDAQABAoIBAG+xW4lCc+G5jzaE
ZZ6B+vgWP5r6N0RUqLoZ1x9at6sEYDwRH3XqT0vT2SB+KcJlaxaJ82j4sslGeBE2
Qv/6clq6S/JD0KmJiTx59RQSTSMp/KlIYFWS8sdsN3diUi3LYWvz5M9Pihgfn6pl
3QBWlwT+6NdEZbgzZv3ukCAcnpPK1PJLW4+Z96+Isdp9vuph6ssIwnOgfMJMIE9n
VgiZnJhHBGw/QxnH8Lx/JPgMNQBKp2DycnB6nGLlJROxmBsxvFb63tkW9AaDA3TC
HY5S8e3fgCzY+OlP3Ps71FsVlqvuMAiO26dx1BUYcB0QzjkYeQWLx340HOrI29sm
XzOK+kECgYEA8XEXG8r3eUyZj9RC7b/YtdfyhBaoi6/j9jT4sweYnr8VvcFHDUjl
SngekLtIFBlsp3uDVZh7nqMP4dKy39WYEUQWlE0IhkHjDvCZKFxGLEPNcXvz2V1E
lMH2VeZc9ReSF3mfKcN9p+++OK5tFVLjyxVL3phRO7neibb9ogCGYncCgYEA33cK
/TNhpXPqj4IdbnlLL5WH0pIujGL8Oy78Yh57YzNouJUZpkSIju1EGDzwcgoFxv6p
l9I9clUXrkETj3l4Jtm3PUkU8MD5YWjtgnz+ANVFDTFQEyj1tNzF9EN+NsD4UUqh
8r7Yngum622tw7VNZhq2oppiBPffMiznyCSt7gkCgYEA0fJhAc3wfA3bynHs8QhN
zXJGdA2v6ie8tq9BQ4xrbj8DDMowmqC0oZjqGzh/aNri3JBOl/PMKxnoCZpJ6NG3
NexbJgIGU5ifdLJKvAnhC0S6NOBwHqc3p5MyPWfflVA1vSwI9ywC7DFQ28DxIgW+
By4xlxIkMSdjF6WDz4ddU+kCgYByX5SBSbYVn8GTF+6GZ5Bi127ACM/ITLV6eS71
7VIL0PktWCrbncIjHS84FKCgTZ7tXdRhf1qx1Pmc13PygCJOCCqwNGKCYtKA19qQ
afb7aCzCdtXcKJ+xpTmwLoc/8P5nZQKEosduBNq3Lti8DWSC+PM3QDsg/dj/7lnn
Z1aaOQKBgQChmPrKD3p3TeWeH3y6pbuYjwI1boKKP2jARZMZy1IrXjfrcF77uNXH
3W9zX2fIwWHSrtMKso3DeLe7z44PSg8eB84yCiYQw94jJqZjEdqqVaJ4UIsQqHH2
JSJ6GfMeSrfQeFeXXMrlv2hA2+5RqxiT8v8R65wNukkp61rO4p/tqQ==
-----END RSA PRIVATE KEY-----
";

        public CustomDomainTestUnitTests()
        {
            Console.WriteLine("CustomDomainUnitTests Setup .....");
            tf = new TestConfig();
        }

        public void Dispose()
        {
            Console.WriteLine("CustomDomainUnitTests TearDownBase .....");
            foreach (string name in Domains.ToArray())
            {
                Console.WriteLine(string.Format("delete domain {0} .....", name));
                try
                {
                    tf.Client.DeleteCustomDomain(new DeleteCustomDomainRequest(name));
                }
                catch
                {
                    //
                }
            }
        }

        [Fact]
        public void TestCustomDomainCRUD()
        {
        
            RouteConfig routeConfig = new RouteConfig();

            PathConfig pathConfig1 = new PathConfig
            {
                ServiceName = "s1",
                FunctionName = "f1",
                Path = "/a"
            };

            PathConfig pathConfig2 = new PathConfig
            {
                ServiceName = "s2",
                FunctionName = "f2",
                Path = "/b",
            };

            routeConfig.Routes = new PathConfig[] { pathConfig1, pathConfig2 };

            CertConfig certConfig = new CertConfig { CertName = "csharp-test-ssl", Certificate = SERVER_CERT, PrivateKey = SERVER_KEY };

            //tf.Client.DeleteCustomDomain(new DeleteCustomDomainRequest(tf.DomainName));

            var response = tf.Client.CreateCustomDomain(new CreateCustomDomainRequest(tf.DomainName, "HTTP,HTTPS", null, routeConfig, null,  certConfig));
            if(response.StatusCode == 200)
            {
                this.Domains.Add(tf.DomainName);
            }

            //Console.WriteLine(response.Content);
            Assert.Equal(response.Data.RouteConfig.Routes.GetLength(0), routeConfig.Routes.Length);
            Assert.Equal(response.Data.DomainName, tf.DomainName);
            Assert.Equal(response.Data.RouteConfig.Routes[0].Path, pathConfig1.Path);
            Assert.Equal(response.Data.RouteConfig.Routes[1].Path, pathConfig2.Path);
            Assert.Equal("csharp-test-ssl", response.Data.CertConfig.CertName);
            Assert.Equal(SERVER_CERT, response.Data.CertConfig.Certificate);
            Assert.Null(response.Data.CertConfig.PrivateKey);
            Assert.Equal("HTTP,HTTPS", response.Data.Protocol);

            pathConfig2 = new PathConfig
            {
                ServiceName = "s2",
                FunctionName = "f2",
                Path = "/c",
            };

            routeConfig.Routes = new PathConfig[] { pathConfig1, pathConfig2 };

            var response2 = tf.Client.UpdateCustomDomain(new UpdateCustomDomainRequest(tf.DomainName, "HTTP", null, routeConfig, null, null));
            //Console.WriteLine(response2.Content);
            Assert.Equal(response2.Data.RouteConfig.Routes.GetLength(0), routeConfig.Routes.Length);
            Assert.Equal(response2.Data.DomainName, tf.DomainName);
            Assert.Equal(response2.Data.RouteConfig.Routes[0].Path, pathConfig1.Path);
            Assert.Equal(response2.Data.RouteConfig.Routes[1].Path, pathConfig2.Path);
            Assert.Null(response2.Data.CertConfig);
            Assert.Equal("HTTP", response2.Data.Protocol);


            var response3 = tf.Client.GetCustomDomain(new GetCustomDomainRequest(tf.DomainName));
            //Console.WriteLine(response3.Content);
            Assert.Equal(response3.Data.RouteConfig.Routes.GetLength(0), routeConfig.Routes.Length);
            Assert.Equal(response3.Data.DomainName, tf.DomainName);
            Assert.Equal(response3.Data.RouteConfig.Routes[0].Path, pathConfig1.Path);
            Assert.Equal(response3.Data.RouteConfig.Routes[1].Path, pathConfig2.Path);
            Assert.Null(response3.Data.CertConfig.CertName);
            Assert.Null(response3.Data.CertConfig.Certificate);
            Assert.Null(response3.Data.CertConfig.PrivateKey);
            Assert.Equal("HTTP", response3.Data.Protocol);

            var response4 = tf.Client.ListCustomDomains(new ListCustomDomainsRequest(100, "pythonSDK"));
            Console.WriteLine(response4.Content);
            Assert.Equal(1, response4.Data.CustomDomains.GetLength(0));
            var firstData = response4.Data.CustomDomains[0];
            Assert.Equal(firstData.RouteConfig.Routes.GetLength(0), routeConfig.Routes.Length);
            Assert.Equal(firstData.DomainName, tf.DomainName);
            Assert.Equal(firstData.RouteConfig.Routes[0].Path, pathConfig1.Path);
            Assert.Equal(firstData.RouteConfig.Routes[1].Path, pathConfig2.Path);


            var response5 = tf.Client.DeleteCustomDomain(new DeleteCustomDomainRequest(tf.DomainName));
            Console.WriteLine(response5.Content);
            if (response.StatusCode == 204)
            {
                this.Domains.Remove(tf.DomainName);
            }
        }
    }
}
