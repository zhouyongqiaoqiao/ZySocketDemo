using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Core;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;
using ZySocketCore.Server.User;
using ZySocketCore.Utils;

namespace ZySocketCore.Server.Plugin
{
    internal class UserDevicePlugin : PluginBase, ITcpReceivedPlugin
    {
        private IZyServerEngine _engine;
        public UserDevicePlugin(IZyServerEngine engine) {
            this._engine = engine;
        }

        public Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.GetMyOnlineDevice)
                {
                    List<ClientType> types = UserManager.Instance.GetUserClientType(packageInfo.UserID);       
                    client.SendMessage(SystemSettings.ServerDefaultId, packageInfo.UserID, (int)MessageType.QUERY_RESPONSE, SerializeConvert.JsonSerializeToBytes(types));
                    return EasyTask.CompletedTask;
                }
            }
            return e.InvokeNext();
        }
    }
}
