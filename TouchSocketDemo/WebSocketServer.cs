using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Http;
using TouchSocket.Sockets;

namespace TouchSocketServer
{
    internal class WebSocketServer
    {
        public WebSocketServer()
        {

        
        }

        public void Start() {
            var service = new HttpService();
            service.Connecting = (client, e) => { Console.WriteLine($"{client.Id} 正在连接！"); return EasyTask.CompletedTask; };
            service.Connected = (client, e) => {
                Console.WriteLine($"{client.Id} 已连接！"
                );

                Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    client.WebSocket.Send($"发送消息给{client.Id}");
                    Console.WriteLine($"发送消息给{client.Id} 完成！");
                });
         
                return EasyTask.CompletedTask;
            };
            service.Disconnected = (client, e) => { Console.WriteLine($"{client.Id} 断开连接"); return EasyTask.CompletedTask; };


            service.Setup(new TouchSocketConfig()//加载配置
                .SetListenIPHosts(4440)
                .ConfigureContainer(a =>
                {
                    a.AddConsoleLogger();
                })
                .ConfigurePlugins(a =>
                {
                    a.UseWebSocket()//添加WebSocket功能
                    .SetWSUrl("/ws")//设置url直接可以连接。
                    .UseAutoPong();//当收到ping报文时自动回应pong
                }));

            service.Start();
            string ports = string.Empty;
            foreach (TcpNetworkMonitor item in service.Monitors)
            {
                ports += item.Option.IpHost + " ;";
            }
            //Console.WriteLine($"Web服务器已启动，Port:{ports}");
            service.Logger.Info($"Web服务器已启动，Port:{ports}");
        }
    }
}
