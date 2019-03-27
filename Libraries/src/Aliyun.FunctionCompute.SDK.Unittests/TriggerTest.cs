using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aliyun.FunctionCompute.SDK.model;
using Aliyun.FunctionCompute.SDK.Request;
using Aliyun.FunctionCompute.SDK.Response;
using Newtonsoft.Json;
using Xunit;

namespace Aliyun.FunctionCompute.SDK.Unittests
{
    [Collection("fcDotnet.Unittests")]
    public class TriggerUnitTests : IDisposable
    {
        readonly TestConfig tf;

        public string Service;
        public string Function;

        public List<string> Triggers = new List<string>();

        public TriggerUnitTests()
        {
            Console.WriteLine("TriggerUnitTests Setup .....");
            Service = "test-charp-" + TestConfig.RandomString(8);
            Function = Service;
            tf = new TestConfig();
        }

        public void Dispose()
        {
            Console.WriteLine("TriggerUnitTests TearDownBase .....");
            foreach (string name in Triggers.ToArray())
            {
                Console.WriteLine(string.Format("delete trigger {0} .....", name));
                try
                {
                    tf.Client.DeleteTrigger(new DeleteTriggerRequest(Service, Function, name));
                }
                catch (Exception)
                {
                    //
                }

            }

            try
            {
                Console.WriteLine(string.Format("delete service/function {0}/{1} .....", Service, Function));
                tf.Client.DeleteFunction(new DeleteFunctionRequest(Service, Function));
                tf.Client.DeleteService(new DeleteServiceRequest(Service));
            }
            catch (Exception)
            {
                //
            }
        }

        [Fact]
        public void TestTriggerCRUD()
        {

            tf.Client.CreateService(new CreateServiceRequest(Service));
               
            byte[] contents = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/hello.zip");
            var code = new Code(Convert.ToBase64String(contents));
            tf.Client.CreateFunction(new CreateFunctionRequest(Service, Function, "python3", "index.handler", code, "desc"));

            string triggerName = "my-http-trigger";
            var httpTriggerConfig = new HttpTriggerConfig(HttpAuthType.ANONYMOUS, new HttpMethod[] { HttpMethod.GET, HttpMethod.POST });
            var response1 = tf.Client.CreateTrigger(new CreateTriggerRequest(Service, Function, triggerName, "http", "dummy_arn", "", httpTriggerConfig));

            this.Triggers.Add(triggerName);

            Assert.Equal(triggerName, response1.Data.TriggerName);
            Assert.Equal("", response1.Data.Description);
            Assert.Equal("http", response1.Data.TriggerType);
            Assert.Equal(JsonConvert.DeserializeObject<HttpTriggerConfig>(response1.Data.TriggerConfig.ToString()), httpTriggerConfig);

            var newHttpTriggerConfig = new HttpTriggerConfig(HttpAuthType.ANONYMOUS, new HttpMethod[] { HttpMethod.GET });
            var response2 = tf.Client.UpdateTrigger(new UpdateTriggerRequest(Service, Function, triggerName, newHttpTriggerConfig));
            Assert.Equal(triggerName, response2.Data.TriggerName);
            Assert.Equal("", response2.Data.Description);
            Assert.Equal("http", response2.Data.TriggerType);
            Assert.Equal(JsonConvert.DeserializeObject<HttpTriggerConfig>(response2.Data.TriggerConfig.ToString()), newHttpTriggerConfig);


            var response3 = tf.Client.GetTrigger(new GetTriggerRequest(Service, Function, triggerName));
            Assert.Equal(triggerName, response3.Data.TriggerName);
            Assert.Equal("", response3.Data.Description);
            Assert.Equal("http", response3.Data.TriggerType);
            Assert.Equal(JsonConvert.DeserializeObject<HttpTriggerConfig>(response3.Data.TriggerConfig.ToString()), newHttpTriggerConfig);


            var response4 = tf.Client.ListTriggers(new ListTriggersRequest(Service, Function));
            Console.WriteLine(response4.Content);
            Assert.Equal(1, response4.Data.Triggers.GetLength(0));
            Assert.Equal(triggerName, response4.Data.Triggers[0].TriggerName);
            Assert.Equal("", response4.Data.Triggers[0].Description);
            Assert.Equal("http", response4.Data.Triggers[0].TriggerType);
            Assert.Equal(JsonConvert.DeserializeObject<HttpTriggerConfig>(response4.Data.Triggers[0].TriggerConfig.ToString()), newHttpTriggerConfig);


            var response5 = tf.Client.DeleteTrigger(new DeleteTriggerRequest(Service, Function, triggerName));
            Assert.Equal(204, response5.StatusCode);
            this.Triggers.Remove(triggerName);
        }


        [Fact]
        public void TestListTriggers()
        {
            tf.Client.CreateService(new CreateServiceRequest(Service));

            byte[] contents = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/hello.zip");
            var code = new Code(Convert.ToBase64String(contents));
            tf.Client.CreateFunction(new CreateFunctionRequest(Service, Function, "python3", "index.handler", code, "desc"));

            string triggerName = "my-oss-trigger";
            string[] events = { "oss:ObjectCreated:PostObject", "oss:ObjectCreated:PutObject" };
            var ossTriggerConfig = new OSSTriggerConfig(events, "pre", ".zip");


            string sourceArn = string.Format("acs:oss:{0}:{1}:{2}", tf.Region, tf.AccountID, tf.CodeBucket);
            tf.Client.CreateTrigger(new CreateTriggerRequest(Service, Function, triggerName, "oss",
                                     sourceArn, tf.InvocationRole, ossTriggerConfig, "oss trigger desc"));

            this.Triggers.Add(triggerName);

            triggerName = "my-log-trigger";
            SouceConfig source = new SouceConfig(tf.LogStore + "_source");
            LogConfig logConfig = new LogConfig(tf.LogProject, tf.LogStore);
            JobConfig jobConfig = new JobConfig(60, 10);
            var functionParameter = new Dictionary<string, object> { };

            var logTriggerConfig = new LogTriggerConfig(source, jobConfig, functionParameter, logConfig, false);

            sourceArn = string.Format("acs:log:{0}:{1}:project/{2}", tf.Region, tf.AccountID, tf.LogProject);
            tf.Client.CreateTrigger(new CreateTriggerRequest(Service, Function, triggerName, "log",
                                     sourceArn, tf.InvocationRole, logTriggerConfig, "log trigger desc"));

            this.Triggers.Add(triggerName);

            var response = tf.Client.ListTriggers(new ListTriggersRequest(Service, Function));
            Console.WriteLine(response.Content);
            Assert.Equal(2, response.Data.Triggers.GetLength(0));

            var response2 = tf.Client.ListTriggers(new ListTriggersRequest(Service, Function, 10, "my-log-"));
            Console.WriteLine(response2.Content);
            Assert.Equal(1, response2.Data.Triggers.GetLength(0));
            Assert.Equal(triggerName, response2.Data.Triggers[0].TriggerName);
            Assert.Equal("log trigger desc", response2.Data.Triggers[0].Description);
            Assert.Equal("log", response2.Data.Triggers[0].TriggerType);
            Assert.Equal(sourceArn, response2.Data.Triggers[0].SourceArn);
            Assert.Equal(tf.InvocationRole, response2.Data.Triggers[0].InvocationRole);
            Assert.Equal(JsonConvert.DeserializeObject<LogTriggerConfig>(response2.Data.Triggers[0].TriggerConfig.ToString()), logTriggerConfig);

        }

        [Fact]
        public void TestOSSTrigger()
        {
            tf.Client.CreateService(new CreateServiceRequest(Service));

            byte[] contents = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/hello.zip");
            var code = new Code(Convert.ToBase64String(contents));
            tf.Client.CreateFunction(new CreateFunctionRequest(Service, Function, "python3", "index.handler", code, "desc"));

            string triggerName = "my-oss-trigger";
            string[] events = { "oss:ObjectCreated:PostObject", "oss:ObjectCreated:PutObject" };
            var ossTriggerConfig = new OSSTriggerConfig(events, "pre", ".zip");


            string sourceArn = string.Format("acs:oss:{0}:{1}:{2}", tf.Region, tf.AccountID, tf.CodeBucket);
            var response = tf.Client.CreateTrigger(new CreateTriggerRequest(Service, Function, triggerName, "oss",
                                     sourceArn, tf.InvocationRole, ossTriggerConfig, "oss trigger desc"));

            this.Triggers.Add(triggerName);

            Assert.Equal(triggerName, response.Data.TriggerName);
            Assert.Equal("oss trigger desc", response.Data.Description);
            Assert.Equal("oss", response.Data.TriggerType);
            Assert.Equal(sourceArn, response.Data.SourceArn);
            Assert.Equal(tf.InvocationRole, response.Data.InvocationRole);
            Assert.Equal(JsonConvert.DeserializeObject<OSSTriggerConfig>(response.Data.TriggerConfig.ToString()), ossTriggerConfig);

        }

        [Fact]
        public void TestLogTrigger()
        {
            tf.Client.CreateService(new CreateServiceRequest(Service));

            byte[] contents = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/hello.zip");
            var code = new Code(Convert.ToBase64String(contents));
            tf.Client.CreateFunction(new CreateFunctionRequest(Service, Function, "python3", "index.handler", code, "desc"));

            string triggerName = "my-log-trigger";

            SouceConfig source = new SouceConfig(tf.LogStore + "_source");
            LogConfig logConfig = new LogConfig(tf.LogProject, tf.LogStore);
            JobConfig jobConfig = new JobConfig(60, 10);
            var functionParameter = new Dictionary<string, object> { };

            var logTriggerConfig = new LogTriggerConfig(source, jobConfig, functionParameter, logConfig, false);

            string sourceArn = string.Format("acs:log:{0}:{1}:project/{2}", tf.Region, tf.AccountID, tf.LogProject);
            var response = tf.Client.CreateTrigger(new CreateTriggerRequest(Service, Function, triggerName, "log",
                                     sourceArn, tf.InvocationRole, logTriggerConfig, "log trigger desc"));

            this.Triggers.Add(triggerName);

            Assert.Equal(triggerName, response.Data.TriggerName);
            Assert.Equal("log trigger desc", response.Data.Description);
            Assert.Equal("log", response.Data.TriggerType);
            Assert.Equal(sourceArn, response.Data.SourceArn);
            Assert.Equal(tf.InvocationRole, response.Data.InvocationRole);
            Assert.Equal(JsonConvert.DeserializeObject<LogTriggerConfig>(response.Data.TriggerConfig.ToString()), logTriggerConfig);
        }

        [Fact]
        public void TestRDSTrigger()
        {
            var client = tf.Client;
            client.CreateService(new CreateServiceRequest(Service));

            byte[] contents = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/hello.zip");
            var code = new Code(Convert.ToBase64String(contents));
            client.CreateFunction(new CreateFunctionRequest(Service, Function, "python3", "index.handler", code, "desc"));

            string triggerName = "my-rds-trigger";
            var rdsTriggerConfig = new RdsTriggerConfig(new string[] { "db.stu"}, 3, 1, "json");
            string sourceArn = string.Format("acs:rds:{0}:{1}:dbinstance/{2}", tf.Region, tf.AccountID, tf.RdsInstanceId);
            var response = client.CreateTrigger(new CreateTriggerRequest(Service, Function, triggerName, "rds",
                                     sourceArn, tf.InvocationRole, rdsTriggerConfig, "rds trigger desc"));

            this.Triggers.Add(triggerName);

            Assert.Equal(triggerName, response.Data.TriggerName);
            Assert.Equal("rds trigger desc", response.Data.Description);
            Assert.Equal("rds", response.Data.TriggerType);
            Assert.Equal(sourceArn, response.Data.SourceArn);
            Assert.Equal(tf.InvocationRole, response.Data.InvocationRole);
            Assert.Equal(JsonConvert.DeserializeObject<RdsTriggerConfig>(response.Data.TriggerConfig.ToString()), rdsTriggerConfig);
        }

        [Fact]
        public void TestMNSTopicTrigger()
        {
            var client = tf.Client;
            client.CreateService(new CreateServiceRequest(Service));

            byte[] contents = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/hello.zip");
            var code = new Code(Convert.ToBase64String(contents));
            client.CreateFunction(new CreateFunctionRequest(Service, Function, "python3", "index.handler", code, "desc"));

            string triggerName = "my-mns-trigger";

            var mnsTriggerConfig = new MnsTopicTriggerConfig("JSON", "BACKOFF_RETRY", "");
            string sourceArn = string.Format("acs:mns:{0}:{1}:/topics/testTopic", tf.Region, tf.AccountID);
            var response = client.CreateTrigger(new CreateTriggerRequest(Service, Function, triggerName, "mns_topic",
                                     sourceArn, tf.InvocationRole, mnsTriggerConfig, "mns trigger desc"));

            this.Triggers.Add(triggerName);

            // cn-hongkong don't support mns topic trigger now
            Assert.Equal(400, response.StatusCode);

            //Assert.Equal(triggerName, response.Data.TriggerName);
            //Assert.Equal("mns trigger desc", response.Data.Description);
            //Assert.Equal("mns_topic", response.Data.TriggerType);
            //Assert.Equal(sourceArn, response.Data.SourceArn);
            //Assert.Equal(tf.InvocationRole, response.Data.InvocationRole);
            //Assert.Equal(JsonConvert.DeserializeObject<MnsTopicTriggerConfig>(response.Data.TriggerConfig.ToString()), mnsTriggerConfig);
        }

        [Fact]
        public void TestTimerTrigger()
        {
            tf.Client.CreateService(new CreateServiceRequest(Service));

            byte[] contents = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/hello.zip");
            var code = new Code(Convert.ToBase64String(contents));
            tf.Client.CreateFunction(new CreateFunctionRequest(Service, Function, "python3", "index.handler", code, "desc"));

            string triggerName = "my-timer-trigger";
            var timerTriggerConfig = new TimeTriggerConfig("@every 5m", "awesome-fc", true);
            var response1 = tf.Client.CreateTrigger(new CreateTriggerRequest(Service, Function, triggerName, "timer", "dummy_arn", "", timerTriggerConfig));

            this.Triggers.Add(triggerName);

            Assert.Equal(triggerName, response1.Data.TriggerName);
            Assert.Equal("", response1.Data.Description);
            Assert.Equal("timer", response1.Data.TriggerType);
            Assert.Equal(JsonConvert.DeserializeObject<TimeTriggerConfig>(response1.Data.TriggerConfig.ToString()), timerTriggerConfig);
        }

        [Fact]
        public void TestCDNEventsTrigger()
        {
            var client = tf.Client;
            client.CreateService(new CreateServiceRequest(Service));

            byte[] contents = File.ReadAllBytes(Directory.GetCurrentDirectory() + "/hello.zip");
            var code = new Code(Convert.ToBase64String(contents));
            client.CreateFunction(new CreateFunctionRequest(Service, Function, "python3", "index.handler", code, "desc"));

            string triggerName = "my-cdn-trigger";

            var filter = new Dictionary<string, string[]>
            {
                { "filter",  new string[] { "www.taobao.com”,”www.tmall.com" }},
            };

            var cdnTriggerConfig = new CdnEventsTriggerConfig("LogFileCreated", "1.0.0", "cdn events trigger test", filter);
            string sourceArn = string.Format("acs:cdn:*:{0}", tf.AccountID);
            var response = client.CreateTrigger(new CreateTriggerRequest(Service, Function, triggerName, "cdn_events",
                                     sourceArn, tf.InvocationRole, cdnTriggerConfig, "cdn events trigger desc"));

            this.Triggers.Add(triggerName);

            Assert.Equal(triggerName, response.Data.TriggerName);
            Assert.Equal("cdn events trigger desc", response.Data.Description);
            Assert.Equal("cdn_events", response.Data.TriggerType);
            Assert.Equal(sourceArn, response.Data.SourceArn);
            Assert.Equal(tf.InvocationRole, response.Data.InvocationRole);
            Assert.Equal(JsonConvert.DeserializeObject<CdnEventsTriggerConfig>(response.Data.TriggerConfig.ToString()), cdnTriggerConfig);
        }
    }
}
