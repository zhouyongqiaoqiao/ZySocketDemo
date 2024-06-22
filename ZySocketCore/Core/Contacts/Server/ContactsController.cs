using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Server;
using ZySocketCore.Core.Contract;
using ZySocketCore.Core.Enum;
using ZySocketCore.Extension;
using ZySocketCore.Server.User;

namespace ZySocketCore.Core.Contacts.Server
{
    internal class ContactsController : PluginBase, ITcpReceivedPlugin, IContactsController
    {
        private readonly ITcpServiceBase _service;
        private readonly IContactsManager _contactsManager;
        private readonly IUserManager _userManager;
        public ContactsController(ITcpServiceBase service, IContactsManager contactsManager, IUserManager userManager)
        {
            this._service = service;
            this._contactsManager = contactsManager;
            this._userManager = userManager;
            this._userManager.UserConnected += _userManager_UserConnected;
            this._userManager.UserDisconnected += _userManager_UserDisconnected;
            this._userManager.ClientDeviceConnected += _userManager_ClientDeviceConnected;
            this._userManager.ClientDeviceDisconnected += _userManager_ClientDeviceDisconnected;        
        }

        private void _userManager_ClientDeviceDisconnected(LoginDeviceData data, DisconnectedType type)
        {
            if (!this.ContactsDisconnectedNotifyEnabled) return;
            this.SendContactOnlineOrOfflineNotify(data.UserID, false,data.ClientType);
        }

        private void _userManager_ClientDeviceConnected(LoginDeviceData data)
        {
            if (!this.ContactsDisconnectedNotifyEnabled) return;
            this.SendContactOnlineOrOfflineNotify(data.UserID, true,data.ClientType);
        }

        private void _userManager_UserDisconnected(string userID)
        {
            // 如果联系人断开通知未启用，则直接返回
           if (!this.ContactsDisconnectedNotifyEnabled) return;
            this.SendContactOnlineOrOfflineNotify(userID, false);

        }

        private void _userManager_UserConnected(string userID)
        {
            if (!this.ContactsDisconnectedNotifyEnabled) return;
            this.SendContactOnlineOrOfflineNotify(userID, true);
        }


        #region IContactsController
        public bool BroadcastBlobListened { get; set; } = false;
        public bool ContactsDisconnectedNotifyEnabled { get; set; } = true;
        public bool ContactsConnectedNotifyEnabled { get; set; } = true;
        public bool UseContactsNotifyThread { get; set; } = false;

        public event Action<string, string, int, byte[], string> BroadcastReceived;
        public event Action<string, BroadcastInformation> BroadcastFailed;

        public Task BroadcastAsync(string groupID, int broadcastType, byte[] broadcastContent, string tag)
        {
            var memberList = this._contactsManager?.GetGroupMemberList(groupID);
            List<Task> tasks = new List<Task>();
            foreach (string userID in memberList)
            {
                List<ZySocketClient>? clients = this._service.GetSocketClients(userID);
                ContactMessage message = new ContactMessage(groupID, broadcastType, broadcastContent, tag);
                foreach (var client in clients)
                {
                    try
                    {
                        var task = client.SendMessageAsync(SystemSettings.ServerDefaultId, userID, (int)MessageType.CONTACT_BROADCAST, SerializeConvert.JsonSerializeToBytes(message));
                        tasks.Add(task);
                    }
                    catch (Exception ee)
                    {
                        this.BroadcastFailed?.Invoke(userID, new BroadcastInformation(SystemSettings.ServerDefaultId, groupID, broadcastType, broadcastContent, tag));
                    }
                }
            }
            return Task.WhenAll(tasks.ToArray());
        }
    

        public void BroadcastBlob(string groupID, int broadcastType, byte[] blobContent, string tag, int fragmentSize)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ITcpReceivedPlugin
        public Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.CONTACT_BROADCAST || packageInfo.MessageType == (int)MessageType.CONTACT_BROADCAST_BLOB)
                {
                    //转发广播消息对群里的成员进行广播
                    ///TO DO 是否需要对群成员进行过滤（转发给自己）
                    ContactMessage contactMessage = SerializeConvert.JsonDeserializeFromBytes<ContactMessage>(packageInfo.Body);
                    this.BroadcastReceived?.Invoke(packageInfo.UserID, contactMessage.GroupID,  contactMessage.InformationType, contactMessage.Content, contactMessage.Tag);
                    #region 转发给群组成员
                    var memberList = this._contactsManager?.GetGroupMemberList(contactMessage.GroupID);
                    foreach (var userID in memberList)
                    {
                        List<ZySocketClient>? clients = this._service.GetSocketClients(userID);
                        foreach (var socketClient in clients)
                        {
                            try
                            {
                                ///TO DO 用 SendAsync 发送时 客户端未触发Received事件，待查原因解决
                                socketClient.Send(packageInfo);
                            }
                            catch (Exception ee)
                            {
                                this.BroadcastFailed?.Invoke(userID, new BroadcastInformation(SystemSettings.ServerDefaultId, contactMessage.GroupID, contactMessage.InformationType, contactMessage.Content, contactMessage.Tag));
                            }
                        }
                    } 
                    #endregion
                    return EasyTask.CompletedTask;
                }
                else if (packageInfo.MessageType == (int)MessageType.REQ_RESP_GetGroupMembers)
                {
                    string groupID = Encoding.UTF8.GetString(packageInfo.Body);

                    // 获取群组成员列表，如果不存在则返回一个空列表
                    var memberList = this._contactsManager?.GetGroupMemberList(groupID) ?? new List<string>();
                    // 创建离线群组成员列表
                    List<string> offlineGroupmates = new List<string>();
                    // 创建在线群组成员列表
                    List<string> onlineGroupmates = new List<string>();
                    // 遍历群组成员列表，根据在线状态分类
                    foreach (string memberID in memberList)
                    {
                        if (this._userManager.IsUserOnLine(memberID))
                        {
                            onlineGroupmates.Add(memberID);
                        }
                        else
                        {
                            offlineGroupmates.Add(memberID);
                        }
                    }
                    // 创建群组成员对象，包含群组ID、在线和离线成员列表
                    Groupmates groupmates = new Groupmates()
                    {
                        GroupID = groupID,
                        OnlineGroupmates = onlineGroupmates,
                        OfflineGroupmates = offlineGroupmates
                    };

                    client.SendMessageAsync(SystemSettings.ServerDefaultId, packageInfo.UserID, (int)MessageType.QUERY_RESPONSE,
    SerializeConvert.JsonSerializeToBytes(groupmates));
                }
                else if (packageInfo.MessageType == (int)MessageType.REQ_RESP_CONTACT)
                {
                    bool onlyOnline = BitConverter.ToBoolean(packageInfo.Body, 0);
                    var contacts = this._contactsManager?.GetContacts(packageInfo.UserID);
                    List<string> memberIDs = new List<string>();
                    if (onlyOnline)
                    {
                        foreach (string memberID in contacts)
                        {
                            if (this._userManager.IsUserOnLine(memberID))
                            {
                                memberIDs.Add(memberID);
                            }
                        }
                    }
                    else
                    {
                        memberIDs.AddRange(contacts);
                    }
                    client.SendMessageAsync(SystemSettings.ServerDefaultId, packageInfo.UserID, (int)MessageType.QUERY_RESPONSE,
    SerializeConvert.JsonSerializeToBytes(memberIDs));
                } 
            }
            return e.InvokeNext();
        }
        #endregion

        #region 方法

        /// <summary>
        /// 获取要发送的所有SocketClient对象
        /// </summary>
        /// <param name="memberList"></param>
        /// <returns></returns>
        private List<ZySocketClient> GetZySocketClients(List<string> memberList)
        {
            List<ZySocketClient> allClients = new List<ZySocketClient>();
            foreach (string userID in memberList)
            {
                List<ZySocketClient>? clients = this._service.GetSocketClients(userID);

                if (clients != null)
                {
                    allClients.AddRange(clients);
                }
            }
            return allClients;
        }

        /// <summary>
        /// 向发送联系人发送上线或下线通知
        /// </summary>
        /// <param name="userID">目标用户</param>
        /// <param name="isOnline">是否上线</param>
        private void SendContactOnlineOrOfflineNotify(string userID, bool isOnline,ClientType? clientType=null)
        {
            // 创建好友上下线协议对象
            UserContract contract = clientType==null? new UserContract(userID) : new UserContract(userID,clientType.Value);
            // 将协议对象序列化为字节数组
            var data = SerializeConvert.JsonSerializeToBytes(contract);
            // 获取用户的联系人列表
            var contacts = this._contactsManager?.GetContacts(userID);
            if(contacts == null) return;
            // 遍历每个联系人
            foreach (string friendID in contacts)
            {
                // 获取联系人的客户端列表
                var friendClients = this._service.GetSocketClients(friendID);
                int messagType = isOnline? (int)MessageType.CONTACT_ONLINE : (int)MessageType.CONTACT_OFFLINE;

                // 遍历每个客户端并发送消息
                foreach (var friendClient in friendClients)
                {
                    friendClient.SendMessageAsync(SystemSettings.ServerDefaultId, friendID, messagType, data);
                }
            }
        }

        #endregion
    }
}
