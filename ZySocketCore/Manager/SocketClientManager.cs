using System.Collections.Generic;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Utils;

namespace ZySocketCore.Manager
{
    //internal class SocketClientManager
    //{
    //    /// <summary>
    //    /// 客户端连接集合 key:用户名  value:所有该用户名的集合（带ClientType 的FullUserID）
    //    /// </summary>
    //    Dictionary<string, List<string>> clientDic =new Dictionary<string, List<string>>();

    //    private SocketClientManager()
    //    {
                
    //    }
    //    private static SocketClientManager _instance ;
    //    public static SocketClientManager Instance
    //    {
    //        get
    //        {
    //            if (_instance == null)
    //            {
    //                _instance = new SocketClientManager();
    //            }
    //            return _instance;
    //        }
    //    }        
        
    //    public bool IsEmpty() { return this.clientDic.Count == 0; }

    //    /// <summary>
    //    /// 获取所有客户端的FullUserID
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<string> GetAllFullUserIds()
    //    {
    //        List<string> allFullUserIds = new List<string>();
    //        foreach (var clientList in this.clientDic.Values)
    //        {
    //            allFullUserIds.AddRange(clientList);
    //        }
    //        return allFullUserIds;
    //    }

    //    public bool Contains(string userID) {

    //       return this.clientDic.ContainsKey(userID);
    //    }

    //    public void AddClient(string fullUserID)
    //    {
    //        string userID = IdUtil.GetUserId(fullUserID);
    //        if (!this.clientDic.ContainsKey(userID))
    //        {
    //            this.clientDic.Add(userID, new List<string>());
    //        }
    //        if (!this.clientDic[userID].Contains(fullUserID))
    //        {
    //            this.clientDic[userID].Add(fullUserID);
    //        }
    //    }

    //    public void AddClient(SocketClient socketClient)
    //    {
    //        this.AddClient(socketClient.Id);
    //    }

    //    public void RemoveClient(string fullUserID) {
    //        var userID = IdUtil.GetUserId(fullUserID);
    //        List<string>? list= this.GetUserIdList(userID);
    //        list?.Remove(fullUserID);
    //    }

    //    public List<string> GetUserIdList(string userID)
    //    {
    //        if (IdUtil.IsFullUserId(userID)) { 
    //            return new List<string>() { userID };            
    //        }
    //        List<string> clientList;
    //        this.clientDic.TryGetValue(userID, out clientList);
    //        return clientList;
    //    }

    //    public void Clear() { this.clientDic.Clear(); }

    //}
}
