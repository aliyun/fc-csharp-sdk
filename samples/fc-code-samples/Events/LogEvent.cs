using System;
using Newtonsoft.Json;

namespace fc_code_samples.Events
{
    public class Source
    {
        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("logstoreName")]
        public string LogstoreName { get; set; }

        [JsonProperty("shardId")]
        public int ShardId { get; set; }

        [JsonProperty("beginCursor")]
        public string BeginCursor { get; set; }

        [JsonProperty("endCursor")]
        public string EndCursor { get; set; }
    }

    public class LogEvent
    {

        [JsonProperty("parameter")]
        public object Parameter { get; set; }

        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("jsonName")]
        public string JobName { get; set; }

        [JsonProperty("taskId")]
        public string TaskId { get; set; }

        [JsonProperty("cursorTime ")]
        public int CursorTime { get; set; }
    }
}
