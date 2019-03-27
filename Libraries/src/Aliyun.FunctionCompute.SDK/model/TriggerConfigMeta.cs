using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Aliyun.FunctionCompute.SDK.model
{
    #region HttpTriggerConfig
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HttpMethod { GET, POST, PUT, HEAD, DELETE };

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HttpAuthType
    {
        [EnumMember(Value = "anonymous")]
        ANONYMOUS,

        [EnumMember(Value = "function")]
        FUNCTION
    }

    public class HttpTriggerConfig
    {
        [JsonProperty("authType")]
        public HttpAuthType AuthType { get; set; }

        [JsonProperty("methods")]
        public HttpMethod[] Methods { get; set; }

        public HttpTriggerConfig(HttpAuthType authType, HttpMethod[] methods)
        {
            this.AuthType = authType;
            this.Methods = methods;
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
                HttpTriggerConfig p = (HttpTriggerConfig)obj;
                bool ret = AuthType == p.AuthType && Methods.Length == p.Methods.Length;

                for (int i = 0; i < Methods.Length; i = i + 1)
                {
                    ret = ret && (Methods[i] == p.Methods[i]);
                }
                return ret;
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}",
               AuthType.GetHashCode(), Methods.GetHashCode()).GetHashCode();
        }
    }
    #endregion HttpTriggerConfig

    #region OSSTriggerConfig
    public class OSSTriggerKey
    {

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        public OSSTriggerKey(string prefix, string suffix)
        {
            this.Prefix = prefix;
            this.Suffix = suffix;
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
                OSSTriggerKey p = (OSSTriggerKey)obj;
                return Prefix == p.Prefix && Suffix == p.Suffix;
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}",
               Prefix.GetHashCode(), Suffix.GetHashCode()).GetHashCode();
        }
    }

    public class OSSTriggerFilter
    {
        [JsonProperty("key")]
        public OSSTriggerKey Key { get; set; }

        public OSSTriggerFilter(string filterPrefix, string filterSuffix)
        {
            this.Key = new OSSTriggerKey(filterPrefix, filterSuffix);
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
                OSSTriggerFilter p = (OSSTriggerFilter)obj;
                Console.WriteLine(Key.Equals(p.Key));
                return Key.Equals(p.Key);
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}",
              Key.Prefix.GetHashCode(), Key.Suffix.GetHashCode()).GetHashCode();
        }
    }

    public class OSSTriggerConfig
    {
        [JsonProperty("events")]
        public string[] Events { get; set; }

        [JsonProperty("filter")]
        public OSSTriggerFilter Filter { get; set; }

        public OSSTriggerConfig(string[] events, string filterPrefix, string filterSuffix)
        {
            this.Events = events;
            this.Filter = new OSSTriggerFilter(filterPrefix, filterSuffix);
        }

        public OSSTriggerConfig()
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
                OSSTriggerConfig p = (OSSTriggerConfig)obj;
                return Filter.Equals(p.Filter) && Enumerable.SequenceEqual(Events, p.Events);
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}",
               Events.GetHashCode(), Filter.GetHashCode()).GetHashCode();
        }
    }
    #endregion OSSTriggerConfig

    #region TimerTriggerConfig
    public class TimeTriggerConfig
    {

        [JsonProperty("cronExpression")]
        public string CronExpression { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("enable")]
        public bool Enable { get; set; }

        public TimeTriggerConfig()
        {
        }

        public TimeTriggerConfig(string cronExpression, string payload, bool enable)
        {
            this.CronExpression = cronExpression;
            this.Payload = payload;
            this.Enable = enable;
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
                TimeTriggerConfig p = (TimeTriggerConfig)obj;
                return CronExpression == p.CronExpression && Payload == p.Payload && Enable == p.Enable;
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}_{2}", CronExpression.GetHashCode(), Payload.GetHashCode(), Enable.GetHashCode()).GetHashCode();
        }
    }
    #endregion TimerTriggerConfig


    #region MnsTopicTriggerConfig
    public class MnsTopicTriggerConfig
    {

        [JsonProperty("notifyContentFormat")]
        public string NotifyContentFormat { get; set; }

        [JsonProperty("notifyStrategy")]
        public string NotifyStrategy { get; set; }

        [JsonProperty("filterTag")]
        public string FilterTag { get; set; }

        public MnsTopicTriggerConfig()
        {
        }

        public MnsTopicTriggerConfig(string notifyContentFormat="STREAM", string notifyStrategy= "BACKOFF_RETRY", string filterTag = "")
        {
            this.NotifyStrategy = notifyStrategy;
            this.NotifyContentFormat = notifyContentFormat;
            this.FilterTag = filterTag;
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
                MnsTopicTriggerConfig p = (MnsTopicTriggerConfig)obj;
                return NotifyContentFormat == p.NotifyContentFormat && NotifyStrategy == p.NotifyStrategy && FilterTag == p.FilterTag;
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}_{2}", NotifyContentFormat.GetHashCode(), NotifyStrategy.GetHashCode(), FilterTag.GetHashCode()).GetHashCode();
        }
    }
    #endregion MnsTopicTriggerConfig

    #region RdsTriggerConfig
    public class RdsTriggerConfig
    {
        [JsonProperty("subscriptionObjects")]
        public string[] SubscriptionObjects { get; set; }

        [JsonProperty("retry")]
        public int Retry { get; set; }

        [JsonProperty("concurrency")]
        public int Concurrency { get; set; }

        [JsonProperty("eventFormat")]
        private string EventFormat { get; set; }

        public RdsTriggerConfig(string[] subscriptionObjects, int retry, int concurrency, string eventFormat)
        {
            this.SubscriptionObjects = subscriptionObjects;
            this.Retry = retry;
            this.Concurrency = concurrency;
            this.EventFormat = eventFormat;
        }

        public RdsTriggerConfig()
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
                RdsTriggerConfig p = (RdsTriggerConfig)obj;
                return Enumerable.SequenceEqual(SubscriptionObjects, p.SubscriptionObjects) && Retry == p.Retry 
                            && Concurrency == p.Concurrency && EventFormat==p.EventFormat;
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}_{2}_{3}", SubscriptionObjects.GetHashCode(), Retry.GetHashCode(), 
                                    Concurrency.GetHashCode(), EventFormat.GetHashCode()).GetHashCode();
        }
    }
    #endregion RdsTriggerConfig

    #region CdnEventsTriggerConfig
    public class CdnEventsTriggerConfig
    {

        [JsonProperty("eventName")]
        public string EventName { get; set; }

        [JsonProperty("eventVersion")]
        public string EventVersion { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("filter")]
        Dictionary<string, string[]> Filter { get; set; }

        public CdnEventsTriggerConfig(string eventName, string eventVersion, string notes, Dictionary<string, string[]> filter)
        {
            this.EventName = eventName;
            this.EventVersion = eventVersion;
            this.Notes = notes;
            this.Filter = filter;
        }

        public CdnEventsTriggerConfig()
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
                CdnEventsTriggerConfig p = (CdnEventsTriggerConfig)obj;
                bool ret = EventName == p.EventName && EventVersion == p.EventVersion && Notes == p.Notes && Filter.Count == p.Filter.Count;
                foreach(var item in Filter)
                {
                    ret = ret & p.Filter.ContainsKey(item.Key) && Enumerable.SequenceEqual(item.Value, p.Filter[item.Key]);
                }

                return ret;
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}_{2}_{3}", EventName.GetHashCode(), EventVersion.GetHashCode(),
                                    Notes.GetHashCode(), Filter.GetHashCode()).GetHashCode();
        }

    }
    #endregion CdnEventsTriggerConfig

    #region LogTriggerConfig
    public class JobConfig
    {
        [JsonProperty("maxRetryTime")]
        public int MaxRetryTime { get; set; }

        [JsonProperty("triggerInterval")]
        public int TriggerInterval { get; set; }

        public JobConfig(int maxRetryTime, int triggerInterval)
        {
            this.MaxRetryTime = maxRetryTime;
            this.TriggerInterval = triggerInterval;
        }

        public JobConfig()
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
                JobConfig p = (JobConfig)obj;
                return MaxRetryTime == p.MaxRetryTime && TriggerInterval == p.TriggerInterval;
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}", MaxRetryTime.GetHashCode(), TriggerInterval.GetHashCode()).GetHashCode();
        }
    }

    public class SouceConfig
    {
        [JsonProperty("logstore")]
        public string Logstore { get; set; }


        public SouceConfig(string logstore)
        {
            this.Logstore = logstore;
        }

        public SouceConfig()
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
                SouceConfig p = (SouceConfig)obj;
                return Logstore == p.Logstore;
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}",Logstore.GetHashCode()).GetHashCode();
        }

    }


    public class LogTriggerConfig
    {
        [JsonProperty("sourceConfig")]
        public SouceConfig SourceConfig { get; set; }

        [JsonProperty("jobConfig")]
        public JobConfig JobConfig { get; set; }

        [JsonProperty("functionParameter")]
        public Dictionary<string, object> FunctionParameter { get; set; }

        [JsonProperty("LogConfig")]
        public LogConfig LogConfig { get; set; }

        [JsonProperty("enable")]
        public  bool Enable { get; set; }

        public LogTriggerConfig(SouceConfig sourceConfig, JobConfig jobConfig, Dictionary<string, object> functionParameter, LogConfig logConfig, bool enable)
        {
            this.SourceConfig = sourceConfig;
            this.JobConfig = jobConfig;
            this.FunctionParameter = functionParameter;
            this.LogConfig = logConfig;
            this.Enable = enable;
        }

        public LogTriggerConfig()
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
                LogTriggerConfig p = (LogTriggerConfig)obj;
                return SourceConfig.Equals(p.SourceConfig) && JobConfig.Equals(p.JobConfig)
                        && (FunctionParameter.Count == p.FunctionParameter.Count && !FunctionParameter.Except(p.FunctionParameter).Any()) 
                        && LogConfig.Equals(p.LogConfig) && Enable == p.Enable;
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}",
               SourceConfig.GetHashCode(), JobConfig.GetHashCode(), FunctionParameter.GetHashCode(), LogConfig.GetHashCode(), Enable.GetHashCode()).GetHashCode();
        }
    }
    #endregion LogTriggerConfig

}
