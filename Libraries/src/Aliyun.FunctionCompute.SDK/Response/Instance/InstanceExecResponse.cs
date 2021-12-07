using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.IO;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using Aliyun.FunctionCompute.SDK.Request;
using Newtonsoft.Json;

namespace Aliyun.FunctionCompute.SDK.Response
{
    public class InstanceExecResponse : ExecWebSocket
    {
        const byte STDIN = 0;
        const byte STDOUT = 1;
        const byte STDERR = 2;
        const byte SYSERR = 3;

        public string Content { get; set; }
        public Dictionary<string, object> Headers { get; set; }
        public int StatusCode { get; set; }

        public InstancesResponseData Data { get; set; }

        private Uri uri { get; set; }
        private ClientWebSocket websocket { get; }
        private ExecCallback callback { get; }

        public InstanceExecResponse(ClientWebSocket websocket, string uri, ExecCallback callback)
        {
            this.Data = new InstancesResponseData();
            this.Headers = new Dictionary<string, object> { };

            this.websocket = websocket;
            this.callback = callback;
            this.uri = new Uri(uri);
        }

        public void Start()
        {
            this.websocket.ConnectAsync(this.uri, CancellationToken.None).Wait();

            var task = new Task(() =>
             {
                 this.callback.OnOpen(this);
             });
            task.Start();

            using (var ms = new MemoryStream())
            {
                while (this.websocket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result;
                    do
                    {
                        var messageBuffer = WebSocket.CreateClientBuffer(16, 16);
                        var recv = this.websocket.ReceiveAsync(messageBuffer, CancellationToken.None);
                        recv.Wait();
                        result = recv.Result;
                        ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
                    }
                    while (!result.EndOfMessage);

                    var data = ms.ToArray();
                    if (data.Length > 0)
                    {
                        var messageType = data[0];
                        var msg = new byte[data.Length - 1];
                        for (var i = 1; i < data.Length; i++)
                            msg[i - 1] = data[i];

                        Array.Copy(data, 1, msg, 0, data.Length - 1);
                        switch (messageType)
                        {
                            case STDOUT:
                                this.callback.OnStdout(this, msg);
                                break;
                            case STDERR:
                                this.callback.OnStderr(this, msg);
                                break;
                            case SYSERR:
                                this.callback.OnServerErr(this, msg);
                                break;
                        }
                    }

                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Position = 0;
                    ms.SetLength(0);
                }
                this.callback.OnClose(this);
            }
        }

        public void Send(byte[] msg)
        {
            byte[] buf = new byte[msg.Length + 1];
            buf[0] = STDIN;
            msg.CopyTo(buf, 1);
            this.websocket.SendAsync(new ArraySegment<byte>(buf), WebSocketMessageType.Text, true, System.Threading.CancellationToken.None).Wait();
        }

        public void Close()
        {
            this.websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).Wait();
        }

        public void SetStatusContent(string content, int status, byte[] rawBytes)
        {
            this.StatusCode = status;
            this.Content = content;
            if (status < 300)
                this.Data = JsonConvert.DeserializeObject<InstancesResponseData>(this.Content);
        }

        public void SetHeaders(Dictionary<string, object> headers)
        {
            this.Headers = headers;
        }

        public string GetRequestID()
        {
            return this.Headers[Constants.HeaderKeys.REQUEST_ID].ToString();
        }
    }
}
