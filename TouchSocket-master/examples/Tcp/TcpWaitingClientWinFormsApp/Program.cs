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
                //client.Logger.Info($"Received�յ����ݣ�{byteBlock.ToString()}");
            };
            
            service.Setup(new TouchSocketConfig()//��������
                .SetListenIPHosts(7789)
                .ConfigureContainer(a =>
                {
                    a.AddConsoleLogger();
                })
                .ConfigurePlugins(a =>
                {
                    a.Add<MyPlugin1>();//�˴�������Ӳ��
                    a.Add<MyPlugin2>();//�˴�������Ӳ��
                }))
                .Start();//����

            service.Logger.Info("������������");
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
                this.m_logger.Info($"�յ����ݣ�{e.ByteBlock.ToString()}");
                //if (int.Parse(e.ByteBlock.ToString()) % 2 == 1) {
                //   await Task.Delay(20);
                //}
                await client.SendAsync("���أ�22");
                await client.SendAsync("���أ�Socket" );
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
                this.m_logger.Info($"�յ�����2��{e.ByteBlock.ToString()}");
                await client.SendAsync("���Ƿ��ص�����2��" + e.ByteBlock.ToString());
            }
        }
    }
}