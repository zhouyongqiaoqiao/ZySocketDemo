using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Core;
using ZySocketCore.Core.Enum;

namespace ZyLightTouchSocketCore.Plugin
{
    internal class LoginPlugin : PluginBase, ITcpReceivedPlugin
    {
        public Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.REQ_OR_RESP_LOGON) {


                    return Task.CompletedTask;
                }
                
            }
            return e.InvokeNext();
        }
    }
}
