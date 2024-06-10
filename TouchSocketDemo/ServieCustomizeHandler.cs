using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Interface;
using ZyLightTouchSocketCore.Server;

namespace TouchSocketServer
{
    internal class ServieCustomizeHandler : IServiceCustomizeHandler
    {
        public void HandleInformation(IServerSender client, int informationType, byte[] info)
        {
            Console.WriteLine("收到消息：" + informationType + "_" + Encoding.UTF8.GetString(info) );

            client.SendMessageToClient(informationType, Encoding.UTF8.GetBytes($"服务端回复客户端ID为{client.Id}的消息：" + informationType + "消息"));
        }
        public byte[] HandleQuery(IServerSender client, int informationType, byte[] info)
        {
            if (informationType <= 100)
            {
                return Encoding.UTF8.GetBytes(informationType.ToString());
            }
            return Encoding.UTF8.GetBytes($"服务端" +
                $"回复{informationType}消息");
        }
    }
}
