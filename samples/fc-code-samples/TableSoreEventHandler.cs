using System.IO;
using Aliyun.Serverless.Core;
using Microsoft.Extensions.Logging;
using PeterO.Cbor;
using System.Text;

namespace fc_code_samples
{
    public class TableSoreEventHandler
    {
        public Stream Handler(Stream input, IFcContext context)
        {
            ILogger logger = context.Logger;
            logger.LogDebug(string.Format("Handle request {0}", context.RequestId));
            var cbor = CBORObject.Read(input);


            // do your things with cbor


            byte[] hello = Encoding.UTF8.GetBytes("hello world");
            MemoryStream output = new MemoryStream();
            output.Write(hello, 0, hello.Length);
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }
    }
}
