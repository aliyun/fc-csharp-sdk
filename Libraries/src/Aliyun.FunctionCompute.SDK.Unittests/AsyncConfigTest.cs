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
    public class AsyncConfigUnitTests : IDisposable
    {
        readonly TestConfig tf;

        public string Service;
        public string Function;

        public AsyncConfigUnitTests()
        {
            Console.WriteLine("AsyncConfigUnitTests Setup .....");
            Service = "destination-suite" + TestConfig.RandomString(8);
            Function = "function-py" + TestConfig.RandomString(8);
            tf = new TestConfig();
        }

        public void Dispose()
        {
            Console.WriteLine("AsyncConfigUnitTests TearDownBase .....");
            var aResp = tf.Client.ListFunctionAsyncConfigs(new ListFunctionAsyncConfigsRequest(Service, Function));

            if(aResp.Data != null && aResp.Data.AsyncConfigs != null)
            {
                foreach (var c in aResp.Data.AsyncConfigs)
                {
                    Console.WriteLine("asyncConfig leak, now deleting.....");
                    try
                    {
                        var resp2 = tf.Client.DeleteFunctionAsyncConfig(
                            new DeleteFunctionAsyncConfigRequest(c.Service, c.Qualifier, c.Function));
                    }
                    catch (Exception)
                    {
                        //
                    }
                }
            }

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
        public void TestAsyncConfigCRUD()
        {
            var resp = tf.Client.CreateService(new CreateServiceRequest(Service));
            Console.WriteLine(resp.Content);
            Assert.Equal(200, resp.StatusCode);

            byte[] contents = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/hello.zip");
            var code = new Code(Convert.ToBase64String(contents));
            var response = tf.Client.CreateFunction(new CreateFunctionRequest(Service, Function, "python3", "index.handler", code, "desc"));
            Console.WriteLine(response.Content);
            Assert.NotEmpty(response.GetRequestID());
            var pReq = new PutFunctionAsyncConfigRequest(Service, "", Function)
            {
                AsyncConfig = new FunctionAsyncConfigMeta
                {
                    // Avoid being set to zero values by default, please set MaxAsyncEventAgeInSeconds and MaxAsyncRetryAttempts manually.
                    MaxAsyncEventAgeInSeconds = 100,
                    MaxAsyncRetryAttempts = 1
                }
            };
            var response2 = tf.Client.PutFunctionAsyncConfig(pReq);
            Assert.Equal(200, response2.StatusCode);
            Assert.NotEmpty(response2.Data.CreatedTime);
            Assert.NotEmpty(response2.Data.LastModifiedTime);
            Assert.Equal(1, response2.Data.MaxAsyncRetryAttempts);
            Assert.Equal("LATEST", response2.Data.Qualifier);


            var response3 = tf.Client.GetFunctionAsyncConfig(new GetFunctionAsyncConfigRequest(Service, "", Function));
            Assert.Equal(response2.Data.MaxAsyncEventAgeInSeconds, response3.Data.MaxAsyncEventAgeInSeconds);

            var resp1 = tf.Client.ListFunctionAsyncConfigs(new ListFunctionAsyncConfigsRequest(Service, Function));
            Assert.Equal(200, resp1.StatusCode);
            Assert.Empty(resp1.Data.NextToken);
            Assert.Single(resp1.Data.AsyncConfigs);
            Assert.Equal(response2.Data.MaxAsyncEventAgeInSeconds, resp1.Data.AsyncConfigs[0].MaxAsyncEventAgeInSeconds);

            var resp2 = tf.Client.DeleteFunctionAsyncConfig(new DeleteFunctionAsyncConfigRequest(Service, "", Function));
            Assert.Equal(204, resp2.StatusCode);

            var response4 = tf.Client.GetFunctionAsyncConfig(new GetFunctionAsyncConfigRequest(Service, "", Function));
            Assert.Equal(404, response4.StatusCode);
        }
    }
}