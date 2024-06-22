using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Sockets;
using ZySocketCore.Core.Enum;
using ZySocketCore.Extension;

namespace ZySocketCore.Server.Basic
{
    internal class BasicController : IBasicController
    {
        private readonly ITcpServiceBase _service;
        public BasicController(ITcpServiceBase service)
        {
            this._service = service;            
        }

        /// <summary>
        /// 踢出客户端，通知客户端被踢出（客户端主动断开连接）
        /// </summary>
        /// <param name="targetUserID"></param>
        public void KickOut(string targetUserID)
        {
            var clients = this._service.GetSocketClients(targetUserID);
            foreach (var client in clients)
            {
                client.SendMessageAsync(SystemSettings.ServerDefaultId, client.Id, (int)MessageType.BE_KICKED_OUT_NOTIFY, new byte[0]);
            }
        }
    }
}
