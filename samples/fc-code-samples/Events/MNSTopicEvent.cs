using System;
namespace fc_code_samples.Events
{
    // notifyContentFormat 为 JSON
    public class MNSTopicEvent
    { 
        public string Context { get; set; }
       
        public string TopicOwner { get; set; }
      
        public string Message { get; set; }
      
        public string Subscriber { get; set; }
       
        public int PublishTime { get; set; }
       
        public string SubscriptionName { get; set; }
       
        public string MessageMD5 { get; set; }
       
        public string TopicName { get; set; }
       
        public string MessageId { get; set; }
    }
}
