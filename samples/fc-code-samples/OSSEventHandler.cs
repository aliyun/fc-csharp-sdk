using System.IO;
using Aliyun.Serverless.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using fc_code_samples.Events;

namespace fc_code_samples
{
    public class OssEventHandler
    {
        public Stream Handler(Stream input, IFcContext context)
        {
            ILogger logger = context.Logger;
            logger.LogDebug(string.Format("Handle request {0}", context.RequestId));
            string data = new StreamReader(input).ReadToEnd();
            OSSEvent ossEvent = JsonConvert.DeserializeObject<OSSEvent>(data);
            return HandlePoco(ossEvent, context);
        }

        public Stream HandlePoco(OSSEvent ossEvent, IFcContext context)
        {
            MemoryStream output = new MemoryStream();
            StreamWriter writer = new StreamWriter(output);
            foreach (OSSEvent.Event evnt in ossEvent.events)
            {
                writer.Write(string.Format("received {0} from {1} @ {2}", evnt.eventName, evnt.eventSource, evnt.region));
                writer.Write(string.Format("received bucket {0}", evnt.oss.bucket.arn));
                writer.Write(string.Format("received object {0} and it's size is {1}", evnt.oss.obj.key, evnt.oss.obj.size));
            }
            writer.Flush();

            return output;
        }
    }
}
