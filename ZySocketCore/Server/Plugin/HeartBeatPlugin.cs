using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Core;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Server.Plugin
{
    internal class HeartBeatPlugin : PluginBase, ITcpReceivedPlugin
    {
        public Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.REQ_HEART_BEAT)
                {
                    return EasyTask.CompletedTask;
                }
            }
            return e.InvokeNext();
        }
    }
}
