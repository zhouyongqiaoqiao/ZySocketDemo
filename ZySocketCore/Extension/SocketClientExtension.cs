using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Server;
using ZySocketCore.Manager;
using ZySocketCore.Server.User;

namespace ZySocketCore.Extension
{
    internal static class SocketClientExtension
    {
        /// <summary>
        /// 获取要发送的SocketClinet列表
        /// </summary>
        /// <param name="targetUserID"></param>
        /// <returns></returns>
        internal static List<ZySocketClient> GetSocketClients(this ITcpServiceBase tcpService, string targetUserID)
        {
            List<ZySocketClient> clients = new List<ZySocketClient>();
            List<string> clientList = UserManager.Instance.GetLoginIdList(targetUserID);
            foreach (string fullUserID in clientList)
            {
                bool exist = tcpService.SocketClients.TryGetSocketClient(fullUserID, out ZySocketClient client);
                if (exist)
                {
                    clients.Add(client);
                }
            }
            return clients;
        }
    }
}
