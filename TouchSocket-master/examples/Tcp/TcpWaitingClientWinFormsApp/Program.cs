using System.Diagnostics;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace TcpWaitingClientWinFormsApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CreateService();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        static void CreateService()
        {
            var service = new TcpService();
            service.Received += (SocketClient client, ByteBlock byteBlock, IRequestInfo requestInfo) =>
            {
                //client.Logger.Info($"Received收到数据：{byteBlock.ToString()}");
            };
            
            service.Setup(new TouchSocketConfig()//载入配置
                .SetListenIPHosts(7789)
                .ConfigureContainer(a =>
                {
                    a.AddConsoleLogger();
                })
                .ConfigurePlugins(a =>
                {
                    a.Add<MyPlugin1>();//此处可以添加插件
                    a.Add<MyPlugin2>();//此处可以添加插件
                }))
                .Start();//启动

            service.Logger.Info("服务器已启动");
        }

        class MyPlugin1 : PluginBase, ITcpReceivedPlugin,ITcpConnectedPlugin
        {
            private readonly ILog m_logger;

            public MyPlugin1(ILog logger)
            {
                this.m_logger = logger;
            }

            public Task OnTcpConnected(IClient client, ConnectedEventArgs e)
            {
                Debug.WriteLine($"MyPlugin1 OnTcpConnected");
                return Task.Delay(0);
            }

            public async Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
            {
                this.m_logger.Info($"收到数据：{e.ByteBlock.ToString()}");
                //if (int.Parse(e.ByteBlock.ToString()) % 2 == 1) {
                //   await Task.Delay(20);
                //}
                await client.SendAsync("返回：22");
                await client.SendAsync("返回：Socket" );
                e.Handled = false;
                //await e.InvokeNext();
            }
        }

        class MyPlugin2 : PluginBase, ITcpReceivedPlugin
        {
            private readonly ILog m_logger;

            public MyPlugin2(ILog logger)
            {
                this.m_logger = logger;
            }

            public async Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
            {
                this.m_logger.Info($"收到数据2：{e.ByteBlock.ToString()}");
                await client.SendAsync("我是返回的数据2：" + e.ByteBlock.ToString());
            }
        }
    }
}