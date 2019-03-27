using System;
using System.Linq;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.model
{
    public class LogConfig
    {
        [JsonProperty("project")]
        public string Project { get; set; }

        [JsonProperty("logstore")]
        public string Logstore { get; set; }


        public LogConfig(string project, string logstore)
        {
            this.Project = project;
            this.Logstore = logstore;
        }

        public LogConfig()
        {

        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                LogConfig p = (LogConfig)obj;
                return (Project == p.Project) && (Logstore == p.Logstore);
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}",
               Project.GetHashCode(), Logstore.GetHashCode()).GetHashCode();
        }

    }

    public class VpcConfig
    {
        [JsonProperty("vpcId")]
        public string VpcId { get; set; }

        [JsonProperty("vSwitchIds")]
        public string[] VSwitchIds { get; set; }

        [JsonProperty("securityGroupId")]
        public string SecurityGroupId { get; set; }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                VpcConfig p = (VpcConfig)obj;
                return (VpcId == p.VpcId) && (SecurityGroupId == p.SecurityGroupId) 
                    && Enumerable.SequenceEqual(VSwitchIds, p.VSwitchIds);
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}_{2}", 
                VpcId.GetHashCode().GetHashCode(), VSwitchIds.GetHashCode(), SecurityGroupId.GetHashCode()).GetHashCode();
        }

    }

    public class MountPointsItem
    {
        [JsonProperty("serverAddr")]
        public string ServerAddr { get; set; }

        [JsonProperty("mountDir")]
        public string MountDir { get; set; }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                MountPointsItem p = (MountPointsItem)obj;
                return (ServerAddr == p.ServerAddr) && (MountDir == p.MountDir);
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}",
               ServerAddr.GetHashCode().GetHashCode(), MountDir.GetHashCode()).GetHashCode();
        }
    }


    public class NasConfig
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("groupId")]
        public int GroupId { get; set; }

        [JsonProperty("mountPoints")]
        public MountPointsItem[] MountPoints { get; set; }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                NasConfig p = (NasConfig)obj;
                return (UserId == p.UserId) && (GroupId == p.GroupId) && 
                    (Enumerable.SequenceEqual(MountPoints, p.MountPoints));
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}_{2}",
                UserId.GetHashCode().GetHashCode(), GroupId.GetHashCode(), MountPoints.GetHashCode()).GetHashCode();
        }

    }

    public class UpdateServiceMeta
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("logConfig")]
        public LogConfig LogConfig { get; set; }

        [JsonProperty("vpcConfig")]
        public VpcConfig VpcConfig { get; set; }

        [JsonProperty("internetAccess")]
        public bool InternetAccess { get; set; }

        [JsonProperty("nasConfig")]
        public NasConfig NasConfig { get; set; }

        public UpdateServiceMeta() { }

        public UpdateServiceMeta(string description = "", string role = null, LogConfig logConfig = null,
            bool internetAccess = true, VpcConfig vpcConfig = null, NasConfig nasConfig = null)
        {
            this.Description = description;
            this.Role = role;
            this.LogConfig = logConfig;
            this.VpcConfig = vpcConfig;
            this.NasConfig = nasConfig;
            this.InternetAccess = internetAccess;
        }
    }


    public class CreateServiceMeta : UpdateServiceMeta
    {

        [JsonProperty("serviceName")]
        public string ServiceName { get; set; }

        public CreateServiceMeta() { }

        public CreateServiceMeta(string serviceName, string description = "", string role = null, LogConfig logConfig = null,
                bool internetAccess = true, VpcConfig vpcConfig = null, NasConfig nasConfig = null)
            :base(description, role, logConfig, internetAccess, vpcConfig, nasConfig)
        {
            this.ServiceName = serviceName;
        }
    }


    public class ServiceMeta : CreateServiceMeta
    {
        [JsonProperty("serviceId")]
        public string ServiceId { get; set; }

        [JsonProperty("createTime")]
        public string CreatedTime { get; set; }

        [JsonProperty("lastModifiedTime")]
        public string LastModifiedTime { get; set; }

        public ServiceMeta() { }
    }

}
