using System;
using Newtonsoft.Json;

namespace fc_code_samples.Events
{
    public class TimerEvent
    {
        [JsonProperty("triggerTime")]
        public string TriggerTime { get; set; }

        [JsonProperty("triggerName")]
        public string TriggerName { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }
    }
}
