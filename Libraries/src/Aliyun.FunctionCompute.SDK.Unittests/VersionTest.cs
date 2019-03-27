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
    public class VersionUnitTests : IDisposable
    {
        readonly TestConfig tf;

        public string Service;

        public VersionUnitTests()
        {
            Console.WriteLine("VersionUnitTests Setup .....");
            Service = "test-charp-" + TestConfig.RandomString(8);
            tf = new TestConfig();
        }

        public void Dispose()
        {
            Console.WriteLine("VersionUnitTests TearDownBase .....");

            var response = tf.Client.ListFunctions(new ListFunctionsRequest(Service));

            foreach (var r in response.Data.Functions)
            {
                string name = r.FunctionName;
                Console.WriteLine(string.Format("delete function {0} .....", name));
                try
                {
                    tf.Client.DeleteFunction(new DeleteFunctionRequest(Service, name));
                }
                catch (Exception)
                {
                    //
                }
            }

            try
            {
                Console.WriteLine(string.Format("delete service {0} .....", Service));

                var response2 = tf.Client.ListAliases(new ListAliasesRequest(Service));
                foreach (var r in response2.Data.Aliases)
                {
                    tf.Client.DeleteAlias(new DeleteAliasRequest(Service, r.AliasName));
                }

                var response3 = tf.Client.ListVersions(new ListVersionsRequest(Service));
                foreach (var r in response3.Data.Versions)
                {
                    tf.Client.DeleteVersion(new DeleteVersionRequest(Service, r.VersionId));
                }

                tf.Client.DeleteService(new DeleteServiceRequest(Service));
            }
            catch (Exception)
            {
                //
            }
        }

        [Fact]
        public void TestVersionCRUD()
        {
            var resp = tf.Client.CreateService(new CreateServiceRequest(Service));
            Assert.Equal(200, resp.StatusCode);

            var response2 = tf.Client.PublishVersion(new PublishVersionRequest(Service, "C# fc sdk 1"));
            Assert.Equal("C# fc sdk 1", response2.Data.Description);
            Assert.True(response2.Data.VersionId != "");
            Assert.True(response2.Data.CreatedTime != "");
            Assert.True(response2.Data.LastModifiedTime != "");


            string newDesc = "create by c# sdk new update";
            tf.Client.UpdateService(new UpdateServiceRequest(Service, newDesc));
            var response3 = tf.Client.PublishVersion(new PublishVersionRequest(Service, "C# fc sdk 2"));
            Assert.Equal(200, response3.StatusCode);
            Assert.Equal("C# fc sdk 2", response3.Data.Description);

            var response4 = tf.Client.ListVersions(new ListVersionsRequest(Service));
            Assert.True(2 == response4.Data.Versions.Length);

            tf.Client.DeleteVersion(new DeleteVersionRequest(Service, response2.Data.VersionId));

            var response5 = tf.Client.ListVersions(new ListVersionsRequest(Service));
            Assert.True(1 == response5.Data.Versions.Length);
        }


        [Fact]
        public void TestAliasCRUD()
        {
            var resp = tf.Client.CreateService(new CreateServiceRequest(Service));
            Assert.Equal(200, resp.StatusCode);

            var response2 = tf.Client.PublishVersion(new PublishVersionRequest(Service, "C# fc sdk 1"));
            Assert.Equal(200, response2.StatusCode);
            string v = response2.Data.VersionId;

            Dictionary<string, float> additionalVersionWeight = new Dictionary<string, float>
            {
                {v,  0.8f}
            };
            var response3 = tf.Client.CreateAlias(new CreateAliasRequest(Service, "staging", v, "alias desc", additionalVersionWeight));
            Assert.Equal("staging", response3.Data.AliasName);
            Assert.Equal("alias desc", response3.Data.Description);
            Assert.True(response3.Data.VersionId == v);
            Assert.True(additionalVersionWeight.Count == response3.Data.AdditionalVersionWeight.Count && !additionalVersionWeight.Except(response3.Data.AdditionalVersionWeight).Any());
            Assert.True(response3.Data.CreatedTime != "");
            Assert.True(response3.Data.LastModifiedTime != "");


            var response4 = tf.Client.GetAlias(new GetAliasRequest(Service, "staging"));
            Assert.Equal("staging", response4.Data.AliasName);
            Assert.Equal("alias desc", response4.Data.Description);
            Assert.True(response4.Data.VersionId == v);
            Assert.True(additionalVersionWeight.Count == response4.Data.AdditionalVersionWeight.Count && !additionalVersionWeight.Except(response4.Data.AdditionalVersionWeight).Any());
            Assert.True(response4.Data.CreatedTime != "");
            Assert.True(response4.Data.LastModifiedTime != "");

            var response5 = tf.Client.UpdateALias(new UpdateAliasRequest(Service, "staging", v, "new alias desc"));
            Assert.Equal("new alias desc", response5.Data.Description);

            string newDesc = "create by c# sdk new update";
            tf.Client.UpdateService(new UpdateServiceRequest(Service, newDesc));
            var response6 = tf.Client.PublishVersion(new PublishVersionRequest(Service, "C# fc sdk 2"));
            Assert.Equal(200, response6.StatusCode);
            string v2 = response6.Data.VersionId;
            tf.Client.CreateAlias(new CreateAliasRequest(Service, "prod", v2, "alias desc 2"));

            var response7 = tf.Client.ListAliases(new ListAliasesRequest(Service));
            Assert.True(2 == response7.Data.Aliases.Length);

            var response8 = tf.Client.DeleteAlias(new DeleteAliasRequest(Service, "staging"));
            Assert.Equal(204, response8.StatusCode);

            var response9 = tf.Client.ListAliases(new ListAliasesRequest(Service));
            Assert.True(1 == response9.Data.Aliases.Length);
        }

        [Fact]
        public void TestGetService()
        {
            var resp = tf.Client.CreateService(new CreateServiceRequest(Service));
            Assert.Equal(200, resp.StatusCode);

            var response2 = tf.Client.PublishVersion(new PublishVersionRequest(Service, "C# fc sdk 1"));
            Assert.Equal(200, response2.StatusCode);
            string v = response2.Data.VersionId;

            GetServiceResponse response3 = tf.Client.GetService(new GetServiceRequest(Service));
            Assert.Equal("", response3.Data.Description);

            GetServiceResponse response4 = tf.Client.GetService(new GetServiceRequest(Service, v));
            Assert.Equal("C# fc sdk 1", response4.Data.Description);

            tf.Client.CreateAlias(new CreateAliasRequest(Service, "staging", v, "alias desc"));
            GetServiceResponse response5 = tf.Client.GetService(new GetServiceRequest(Service, "staging"));
            Assert.Equal("C# fc sdk 1", response5.Data.Description);

        }

        [Fact]
        public void TestGetFunction()
        {
            tf.Client.CreateService(new CreateServiceRequest(Service));

            string name = "test-charp-func" + TestConfig.RandomString(8);
            byte[] contents = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/hello.zip");
            var code = new Code(Convert.ToBase64String(contents));
            var response = tf.Client.CreateFunction(new CreateFunctionRequest(Service, name, "python3", "index.handler", code, "desc"));
            Assert.Equal(200, response.StatusCode);
            var response2 = tf.Client.PublishVersion(new PublishVersionRequest(Service, "C# fc sdk 1"));
            Assert.Equal(200, response2.StatusCode);
            var response3 = tf.Client.CreateAlias(new CreateAliasRequest(Service, "staging", response2.Data.VersionId, "alias desc"));
            Assert.Equal(200, response3.StatusCode);

            var response4 = tf.Client.GetFunction(new GetFunctionRequest(Service, name));
            Assert.Equal("desc", response4.Data.Description);

            var response5 = tf.Client.GetFunction(new GetFunctionRequest(Service, name, response2.Data.VersionId));
            Assert.Equal("desc", response5.Data.Description);

            var response6 = tf.Client.GetFunction(new GetFunctionRequest(Service, name, "staging"));
            Assert.Equal("desc", response6.Data.Description);

            tf.Client.UpdateFunction(new UpdateFunctionRequest(Service, name, "python3", "index.handler", code, "new-desc"));
            var response7 = tf.Client.PublishVersion(new PublishVersionRequest(Service, "C# fc sdk 2"));
            tf.Client.CreateAlias(new CreateAliasRequest(Service, "prod", response7.Data.VersionId, "alias desc 2"));

            var response8 = tf.Client.GetFunction(new GetFunctionRequest(Service, name, response7.Data.VersionId));
            Assert.Equal("new-desc", response8.Data.Description);

            var response9 = tf.Client.GetFunction(new GetFunctionRequest(Service, name, "prod"));
            Assert.Equal("new-desc", response9.Data.Description);

        }

        [Fact]
        public void TestInvokeFunction()
        {
            tf.Client.CreateService(new CreateServiceRequest(Service));

            string name = "test-charp-func" + TestConfig.RandomString(8);
            byte[] contents = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/hello.zip");
            var code = new Code(Convert.ToBase64String(contents));
            tf.Client.CreateFunction(new CreateFunctionRequest(Service, name, "python3", "index.handler", code, "desc"));
            byte[] hello = Encoding.UTF8.GetBytes("hello csharp world");
            var response = tf.Client.InvokeFunction(new InvokeFunctionRequest(Service, name, null, hello));
            Assert.Equal("hello csharp world", response.Content);
            Assert.Equal(hello, response.Data);

            var response2 = tf.Client.PublishVersion(new PublishVersionRequest(Service, "C# fc sdk 1"));
            Assert.Equal(200, response2.StatusCode);
            var response3 = tf.Client.CreateAlias(new CreateAliasRequest(Service, "staging", response2.Data.VersionId, "alias desc"));
            Assert.Equal(200, response3.StatusCode);

            var response4 = tf.Client.InvokeFunction(new InvokeFunctionRequest(Service, name, "staging", hello));
            Assert.Equal("hello csharp world", response4.Content);
            Assert.Equal(hello, response4.Data);

            var response5 = tf.Client.InvokeFunction(new InvokeFunctionRequest(Service, name, response2.Data.VersionId, hello));
            Assert.Equal("hello csharp world", response5.Content);
            Assert.Equal(hello, response5.Data);
        }
    }
}