using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Sockets;

namespace ZySocketCore.Manager
{
    internal class ClientManager
    {
        /// <summary>
        /// 客户端连接集合 key:用户名  value:SocketClient
        /// </summary>
        Dictionary<string, SocketClient> clientDic =new Dictionary<string, SocketClient>();

        public ClientManager()
        {
                
        }

        public int GetClientCount()
        {
            return this.clientDic.Count;
        }

        public bool Contains(string clientID) {

           return this.clientDic.ContainsKey(clientID);
        }

        public void AddClient(SocketClient socketClient)
        {
            this.clientDic.TryAdd(socketClient.Id, socketClient);
        }

        public void RemoveClient(string clientID) { 
            this.clientDic.Remove(clientID);
        }

        public SocketClient GetClient(string clientID)
        {
            SocketClient client;
            this.clientDic.TryGetValue(clientID, out client);
            return client;
        }

        public void Clear() { this.clientDic.Clear(); }


    }
}
