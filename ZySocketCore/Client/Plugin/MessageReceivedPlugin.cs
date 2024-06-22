using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Core;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Client.Plugin
{
    internal class MessageReceivedPlugin : PluginBase, ITcpReceivedPlugin
    {
        private readonly ZyClientEngine tcpClient;
        private ILoggerObject _logger;
        public MessageReceivedPlugin(ZyClientEngine client)
        {
            this.tcpClient = client;
            
        }

        public Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.BE_FORCED_OUT_NOTIFY || packageInfo.MessageType == (int)MessageType.BE_KICKED_OUT_NOTIFY)
                {
                    
                    //TODO:处理被踢下线的逻辑
                    client.Close();
                    return EasyTask.CompletedTask;
                }
            }
            return e.InvokeNext();
        }
    }
}
