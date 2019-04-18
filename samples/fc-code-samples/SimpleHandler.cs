using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Aliyun.Serverless.Core;
using Microsoft.Extensions.Logging;
using Aliyun.OSS;

namespace fc_code_samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }


    public class OssFileHandlerRequest
    {
        public string Bucket;
        public string Key;
        public string Endpoint;
    }

    public class TestHandler
    {
        public async Task<Stream> AsyncEchoEvent(Stream input, IFcContext context)
        {
            context.Logger.LogInformation("Handle request: {0}", context.RequestId);
            MemoryStream copy = new MemoryStream();
            await input.CopyToAsync(copy);
            copy.Seek(0, SeekOrigin.Begin);
            return copy;
        }

        public async Task<Stream> AsyncEchoEventNoContext(Stream input)
        {
            MemoryStream copy = new MemoryStream();
            await input.CopyToAsync(copy);
            copy.Seek(0, SeekOrigin.Begin);
            return copy;
        }

        public Stream TestLogger(Stream input, IFcContext context)
        {
            context.Logger.EnabledLogLevel = LogLevel.Error;
            context.Logger.LogError("console error 1");
            context.Logger.LogInformation("console info 1");
            context.Logger.LogWarning("console warn 1");
            context.Logger.LogDebug("console debug 1");

            context.Logger.EnabledLogLevel = LogLevel.Warning;

            context.Logger.LogError("console error 2");
            context.Logger.LogInformation("console info 2");
            context.Logger.LogWarning("console warn 2");
            context.Logger.LogDebug("console debug 2");

            context.Logger.EnabledLogLevel = LogLevel.Information;
            context.Logger.LogInformation("Handle request: {0}", context.RequestId);

            byte[] hello = Encoding.UTF8.GetBytes("hello world");
            MemoryStream output = new MemoryStream();
            output.Write(hello, 0, hello.Length);
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }

        public Stream EchoEventNoContext(Stream input)
        {
            byte[] hello = Encoding.UTF8.GetBytes("hello world");
            MemoryStream output = new MemoryStream();
            output.Write(hello, 0, hello.Length);
            output.Seek(0, SeekOrigin.Begin);
            return output;
        }

        // optional serializer class, if it’s not specified, the default serializer (based on JSON.Net) will be used.  
        // [FcSerializer(typeof(MySerialization))]  
        public Product EchoPoco(Product product, IFcContext context)
        {
            int Id = product.Id;
            string Description = product.Description;
            context.Logger.LogInformation("Id {0}, Description {1}", Id, Description);
            return product;
        }

        public Stream GetOssFile(OssFileHandlerRequest req, IFcContext context)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req));
            }
            if (context == null || context.Credentials == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            OssClient ossClient = new OssClient(req.Endpoint, context.Credentials.AccessKeyId, context.Credentials.AccessKeySecret, context.Credentials.SecurityToken);
            OssObject obj = ossClient.GetObject(req.Bucket, req.Key);
            return obj.Content;
        }
    }
}

