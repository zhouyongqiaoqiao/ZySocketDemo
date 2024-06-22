using System.Collections.Generic;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Server;
using ZySocketCore.Core.Enum;
using ZySocketCore.Extension;
using ZySocketCore.Utils;

namespace ZySocketCore.Core.CustomizeInfo.Server
{
    public class CustomizeController : ICustomizeController
    {
        private ITcpServiceBase serviceBase;
        public CustomizeController(ITcpServiceBase service )
        {
            this.serviceBase = service;
        }

        public Task<byte[]> QueryAsyncToClient(string userID, int informationType, byte[] info, ClientType clientType)
        {
            ZySocketClient client = this.GetLocalClient(userID, clientType);

            if (client == null)
                throw new ClientNotFindException("client not found");

            return client.Queryer.QueryCustomMessageAsync(SystemSettings.ServerDefaultId, userID, informationType, info);
        }

        public Task SendAsync(string userID, int informationType, byte[] info, ClientType? clientType = null)
        {
            List<ZySocketClient>? list = this.serviceBase.GetSocketClients(IdUtil.BuildFullUserId(clientType, userID));
            foreach (ZySocketClient client in list)
            {
                client.SendCustomMessageAsync(SystemSettings.ServerDefaultId, userID, informationType, info);
            }
            return EasyTask.CompletedTask;
        }

        public Task SendCertainlyToClient(string userID, int informationType, byte[] info, ClientType clientType)
        {
            ZySocketClient client = this.GetLocalClient(userID, clientType);

            if (client == null)
                throw new ClientNotFindException("client not found");

            client.Queryer.SendCertainlyCustomMessage(SystemSettings.ServerDefaultId, userID, informationType, info);
            return EasyTask.CompletedTask;
        }

        private ZySocketClient GetLocalClient(string userID, ClientType clientType)
        {
             this.serviceBase.SocketClients.TryGetSocketClient(IdUtil.BuildFullUserId(clientType, userID), out ZySocketClient client);
            return client;
        }
    }
}
