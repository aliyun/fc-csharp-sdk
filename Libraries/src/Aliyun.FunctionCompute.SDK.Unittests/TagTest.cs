using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aliyun.FunctionCompute.SDK.model;
using Aliyun.FunctionCompute.SDK.Request;
using Aliyun.FunctionCompute.SDK.Response;
using Xunit;

namespace Aliyun.FunctionCompute.SDK.Unittests
{
    [Collection("fcDotnet.Unittests")]
    public class TagUnitTests : IDisposable
    {
        readonly TestConfig tf;

        public List<string> Services = new List<string>();

        public TagUnitTests()
        {
            Console.WriteLine("TagUnitTests Setup .....");

            tf = new TestConfig();
            for(int i =0; i < 3; i++)
            {
                string SvrName = "test-csharp-" + TestConfig.RandomString(8);
                var resp = tf.Client.CreateService(new CreateServiceRequest(SvrName));
                Assert.Equal(200, resp.StatusCode);
                Services.Add(SvrName);
            }
        }

        public void Dispose()
        {
            Console.WriteLine("TagUnitTests TearDownBase .....");
            try
            {
                foreach (var s in Services)
                {
                    string[] keys = { };
                    string resArn = String.Format("services/{0}", s);
                    tf.Client.UnTagResource(new UntagResourceRequest(resArn, keys, true));
                    tf.Client.DeleteService(new DeleteServiceRequest(s));
                }

            }
            catch (Exception)
            {
                //
            }
        }

        [Fact]
        public void TestTagOperation()
        {
            int i = 0;
            foreach (var s in Services)
            {
                string resArn = String.Format("services/{0}", s);
                string fullArn = String.Format("acs:fc:{0}:{1}:services/{2}", tf.Region, tf.AccountID, s);
                Dictionary<string, string> tags = new Dictionary<string, string> {
                    {"k3","v3"},
                };
                if (i % 2 == 0)
                {
                    tags["k1"] = "v1";
                }
                else
                {
                    tags["k2"] = "v2";
                }
                var resp = tf.Client.TagResource(new TagResourceRequest(resArn, tags));
                Assert.Equal(200, resp.StatusCode);
                Assert.NotNull(resp.GetRequestID());
               
                var gResp = tf.Client.GetResourceTags(new GetResourceTagsRequest(resArn));
                Assert.Equal(200, gResp.StatusCode);
                Assert.NotNull(gResp.GetRequestID());
                Assert.Equal(fullArn, gResp.Data.ResourceArn);
                Assert.Equal("v3", gResp.Data.Tags["k3"]);
                if (i % 2 == 0)
                {
                    Assert.Equal("v1", gResp.Data.Tags["k1"]);
                    Assert.False(gResp.Data.Tags.ContainsKey("k2"));
                } 
                else
                {
                    Assert.Equal("v2", gResp.Data.Tags["k2"]);
                    Assert.False(gResp.Data.Tags.ContainsKey("k1"));
                }

                string[] keys = { "k3" };
                var uresp = tf.Client.UnTagResource(new UntagResourceRequest(resArn, keys, false));
                Assert.Equal(200, uresp.StatusCode);
                Assert.NotNull(uresp.GetRequestID());

                gResp = tf.Client.GetResourceTags(new GetResourceTagsRequest(resArn));
                Assert.Equal(200, gResp.StatusCode);
                Assert.NotNull(gResp.GetRequestID());
                Assert.Equal(fullArn, gResp.Data.ResourceArn);
                Assert.False(gResp.Data.Tags.ContainsKey("k3"));

                string[]  emptyKeys =  { };
                uresp = tf.Client.UnTagResource(new UntagResourceRequest(resArn, emptyKeys, true));
                Assert.Equal(200, uresp.StatusCode);
                Assert.NotNull(uresp.GetRequestID());

                gResp = tf.Client.GetResourceTags(new GetResourceTagsRequest(resArn));
                Assert.Equal(200, gResp.StatusCode);
                Assert.NotNull(gResp.GetRequestID());
                Assert.Equal(fullArn, gResp.Data.ResourceArn);
                Assert.False(gResp.Data.Tags.ContainsKey("k1"));
                Assert.False(gResp.Data.Tags.ContainsKey("k2"));
                Assert.False(gResp.Data.Tags.ContainsKey("k3"));

                i++;
            }
        }
    }
}

