using System;
using System.Linq;
using Aliyun.FunctionCompute.SDK.Client;
using Aliyun.FunctionCompute.SDK.model;

namespace Aliyun.FunctionCompute.SDK.Unittests
{
    public class TestConfig
    {
        public TestConfig()
        {
            this.VpcId = Environment.GetEnvironmentVariable("VPC_ID");
            this.VSwitchIds = Environment.GetEnvironmentVariable("VSWITCH_IDS");
            this.SecurityGroupId = Environment.GetEnvironmentVariable("SECURITY_GROUP_ID");
            this.VpcRole = Environment.GetEnvironmentVariable("VPC_ROLE");
            this.UserId = Environment.GetEnvironmentVariable("USER_ID");
            this.GroupId = Environment.GetEnvironmentVariable("GROUP_ID");
            this.NasServerAddr = Environment.GetEnvironmentVariable("NAS_SERVER_ADDR");
            this.NasMountDir = Environment.GetEnvironmentVariable("NAS_MOUNT_DIR");
            this.ServiceRole = Environment.GetEnvironmentVariable("SERVICE_ROLE");
            this.LogProject = Environment.GetEnvironmentVariable("LOG_PROJECT");
            this.LogStore = Environment.GetEnvironmentVariable("LOG_STORE");
            this.AccountID = Environment.GetEnvironmentVariable("ACCOUNT_ID");
            this.DomainName = "pythonSDK.cn-hongkong.1221968287646227.cname-test.fc.aliyun-inc.com";
            this.InvocationRole = Environment.GetEnvironmentVariable("INVOCATION_ROLE");
            this.Region = Environment.GetEnvironmentVariable("REGION");
            this.CodeBucket = Environment.GetEnvironmentVariable("CODE_BUCKET");
            this.RdsInstanceId = "rm-j6c2938h95da19vmi";

            this.Client = new FCClient(
                Environment.GetEnvironmentVariable("REGION"),
                this.AccountID,
                Environment.GetEnvironmentVariable("ACCESS_KEY_ID"),
                Environment.GetEnvironmentVariable("ACCESS_KEY_SECRET")
            );

            this.NasConfig = new NasConfig
            {
                UserId = int.Parse(this.UserId),
                GroupId = int.Parse(this.GroupId),
                MountPoints = new MountPointsItem[] { 
                    new MountPointsItem { ServerAddr= this.NasServerAddr, MountDir=this.NasMountDir } 
                }
            };

            this.LogConfig = new LogConfig
            {
                Project = this.LogProject,
                Logstore = this.LogStore
            };

            this.VpcConfig = new VpcConfig
            {
                VpcId = this.VpcId,
                VSwitchIds = new string[] { this.VSwitchIds },
                SecurityGroupId= this.SecurityGroupId
            };
       
        }

        private static Random myRandom = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[myRandom.Next(s.Length)]).ToArray());
        }

        public string VpcId { get; }
        public string VSwitchIds { get; }
        public string SecurityGroupId { get;}
        public string VpcRole { get; }
        public string UserId { get; }
        public string GroupId { get;}
        public string NasServerAddr { get;}
        public string NasMountDir { get; }
        public string ServiceRole { get; }
        public string LogProject { get; }
        public string LogStore { get; }
        public string AccountID { get; }
        public string DomainName { get; }
        public string InvocationRole { get; }
        public string Region { get; }
        public string CodeBucket { get; }
        public string RdsInstanceId { get; }
        public FCClient Client { get; private set; }
        public NasConfig NasConfig { get; }
        public LogConfig LogConfig { get; }
        public VpcConfig VpcConfig { get; }
    }
}
