using System;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.model
{
    public class UpdateTriggerMeta
    {
        [JsonProperty("invocationRole")]
        public string InvocationRole { get; set; }

        [JsonProperty("triggerConfig")]
        public object TriggerConfig { get; set; }

        [JsonProperty("qualifier")]
        public string Qualifier { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public UpdateTriggerMeta() { }

        public UpdateTriggerMeta(string invocationRole, object triggerConfig, string description = null, string qualifier = null)
        {
            this.InvocationRole = invocationRole;
            this.TriggerConfig = triggerConfig;
            this.Description = description;
            this.Qualifier = qualifier;
        }
    }

    public class CreateTriggerMeta: UpdateTriggerMeta
    {
        [JsonProperty("triggerName")]
        public string TriggerName { get; set; }

        [JsonProperty("sourceArn")]
        public string SourceArn { get; set; }

        [JsonProperty("triggerType")]
        public string TriggerType { get; set; }

        public CreateTriggerMeta() { }

        public CreateTriggerMeta(string triggerName, string triggerType,
           string sourceArn, string invocationRole, object triggerConfig, string description = null, string qualifier = null)
            :base(invocationRole, triggerConfig, description, qualifier)
        {
            this.TriggerName = triggerName;
            this.TriggerType = triggerType;
            this.SourceArn = sourceArn;
        }
    }


    public class TriggerMeta: CreateTriggerMeta
    {
        [JsonProperty("createdTime")]
        public string CreatedTime { get; set; }

        [JsonProperty("lastModifiedTime")]
        public string LastModifiedTime { get; set; }

        public TriggerMeta() { }
        public TriggerMeta(string triggerName, string triggerType, string sourceArn, string invocationRole, object triggerConfig, string description = null, string qualifier = null) : base(triggerName, triggerType, sourceArn, invocationRole, triggerConfig, description, qualifier)
        {
        }
    }

}
