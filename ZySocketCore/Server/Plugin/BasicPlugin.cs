using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Core;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;

namespace ZySocketCore.Server.Plugin
{
    internal class BasicPlugin : PluginBase, ITcpReceivedPlugin
    {
        private IZyServerEngine _engine;

        public BasicPlugin(IZyServerEngine engine)
        {
            _engine = engine;
        }
        public Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.REQ_KICK_OUT)
                {
                    string targetId = Encoding.UTF8.GetString(packageInfo.Body);
                    _engine.BasicController.KickOut(targetId);                                   
                    client.SendMessage(SystemSettings.ServerDefaultId, packageInfo.UserID, (int)MessageType.QUERY_RESPONSE, null);
                    return EasyTask.CompletedTask;
                }
            }
            return e.InvokeNext();
        }
    }
}
