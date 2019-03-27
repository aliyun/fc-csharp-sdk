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

            //tf.Client.DeleteCustomDomain(new DeleteCustomDomainRequest(tf.DomainName));

            var response = tf.Client.CreateCustomDomain(new CreateCustomDomainRequest(tf.DomainName, null, null, routeConfig));
            if(response.StatusCode == 200)
            {
                this.Domains.Add(tf.DomainName);
            }

            //Console.WriteLine(response.Content);
            Assert.Equal(response.Data.RouteConfig.Routes.GetLength(0), routeConfig.Routes.Length);
            Assert.Equal(response.Data.DomainName, tf.DomainName);
            Assert.Equal(response.Data.RouteConfig.Routes[0].Path, pathConfig1.Path);
            Assert.Equal(response.Data.RouteConfig.Routes[1].Path, pathConfig2.Path);


            pathConfig2 = new PathConfig
            {
                ServiceName = "s2",
                FunctionName = "f2",
                Path = "/c",
            };

            routeConfig.Routes = new PathConfig[] { pathConfig1, pathConfig2 };

            var response2 = tf.Client.UpdateCustomDomain(new UpdateCustomDomainRequest(tf.DomainName, null, null, routeConfig));
            //Console.WriteLine(response2.Content);
            Assert.Equal(response2.Data.RouteConfig.Routes.GetLength(0), routeConfig.Routes.Length);
            Assert.Equal(response2.Data.DomainName, tf.DomainName);
            Assert.Equal(response2.Data.RouteConfig.Routes[0].Path, pathConfig1.Path);
            Assert.Equal(response2.Data.RouteConfig.Routes[1].Path, pathConfig2.Path);


            var response3 = tf.Client.GetCustomDomain(new GetCustomDomainRequest(tf.DomainName));
            //Console.WriteLine(response3.Content);
            Assert.Equal(response3.Data.RouteConfig.Routes.GetLength(0), routeConfig.Routes.Length);
            Assert.Equal(response3.Data.DomainName, tf.DomainName);
            Assert.Equal(response3.Data.RouteConfig.Routes[0].Path, pathConfig1.Path);
            Assert.Equal(response3.Data.RouteConfig.Routes[1].Path, pathConfig2.Path);

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
