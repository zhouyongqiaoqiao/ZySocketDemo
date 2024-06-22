using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Server;
using ZySocketCore.Core.Contacts;
using ZySocketCore.Core.Contract;
using ZySocketCore.Core.Enum;
using ZySocketCore.Extension;
using ZySocketCore.Server.User;
using ZySocketCore.Utils;

namespace ZySocketCore.Core.DynamicGroup.Server
{
    internal class DynamicGroupController : PluginBase, 
        IDynamicGroupController,
        ITcpReceivedPlugin<ZySocketClient>,
        ITcpDisconnectedPlugin<ZySocketClient>
    {
        Dictionary<string, GroupInfo> dic = new Dictionary<string, GroupInfo>();
        private readonly ITcpServiceBase _service;

        public DynamicGroupController(ITcpServiceBase tcpService)
        {
            _service = tcpService;
        }
        #region IDynamicGroupController
        public event Action<string, string, string> GroupTagChanged;
        public event Action<string, List<string>, string> SomeoneBePulledIntoGroup;
        public event Action<string, string> SomeoneQuitGroup;
        public event Action<string, string> SomeoneJoinGroup;
        public event Action<string> GroupDestroyed;
        public event Action<string> GroupCreated;
        public event Action<string, string, int, byte[], string> BroadcastReceived;
        public event Action<string, List<string>, string> SomeoneBeRemovedFromGroup;

        public void Broadcast(string groupID, int broadcastType, byte[] broadcastContent, string comment)
        {
            var memberList = this.GetGroup(groupID)?.Members;
            BroadcastContract broadcastContract = new BroadcastContract(groupID, broadcastType, broadcastContent, comment);

            foreach ( var userID in memberList )
            {
                List<ZySocketClient>? clients = this._service.GetSocketClients(userID);
                foreach (ZySocketClient client in clients)
                {
                    client.SendMessageAsync(SystemSettings.ServerDefaultId, userID, (int)MessageType.Broadcast, SerializeConvert.JsonSerializeToBytes(broadcastContract));
                }
            }
        }

        public GroupInfo GetGroup(string groupID)
        {
            var res = this.dic.TryGetValue(groupID, out GroupInfo group);
            if (res)
            {
                return group;
            }
            return null;
        }

        public List<string> GetGroupList()
        {
            return new List<string>(this.dic.Keys);
        }

        public List<string> GetGroupmates(string userID)
        {
            List<string> memberIDs = new List<string>();
            foreach (GroupInfo groupInfo in this.dic.Values)
            {
                if (groupInfo.Members.Contains(userID)) {
                    memberIDs.AddRange(groupInfo.Members); }
            }        
            return memberIDs;
        }

        public string GetGroupTag(string groupID)
        {
            return this.GetGroup(groupID)?.Tag;
        }

        public void PresetGroup(string groupID, List<string> members)
        {
            if(this.dic.ContainsKey(groupID))
                throw new ArgumentException("Group already exists.");
            this.dic.Add(groupID, new GroupInfo(groupID, members));
        }

        private GroupInfo JoinGroup(string groupID, string memberID)
        {
            if (!this.dic.ContainsKey(groupID))
            {
                this.dic.Add(groupID, new GroupInfo(groupID, new List<string>()));
                this.GroupCreated?.Invoke(groupID);
            }
            bool? res = this.GetGroup(groupID)?.AddMember(memberID);
            if (res.HasValue && res.Value) {
                this.SomeoneJoinGroup?.Invoke(groupID, memberID);
                this.SomeoneJoinGroupNotify(groupID, memberID);
            }
            return this.dic[groupID];
        }

        public void PullIntoGroup(string groupID, List<string> members)
        {
            this.PullIntoGroup(groupID, members, SystemSettings.ServerDefaultId);
        }

        private void PullIntoGroup(string groupID, List<string> members, string operatorID)
        {
            if (!this.dic.ContainsKey(groupID))
            {
                this.dic.Add(groupID, new GroupInfo(groupID, members));
                this.GroupCreated?.Invoke(groupID);
            }
            else
            {
                this.dic[groupID].AddMembers(members);
            }
            this.SomeoneBePulledIntoGroup?.Invoke(groupID, members, operatorID);
            this.SomeoneBePulledIntoGroupNotify(groupID, members, operatorID); //先向组成员发送某些人被拉进群组的通知
            ///再 向被拉进群的成员发送通知 客户端要触发 BePulledIntoGroup待实现    
            GroupInfo groupInfo = this.dic[groupID];
            foreach (var item in members)
            {
                this.PullIntoGroupNotify(item, groupInfo, operatorID);                
            }

        }

        private void QuitGroup(string groupID,string memberID)
        {
            var groupInfo = this.GetGroup(groupID);
            if (groupInfo == null)
                return;
            var res = groupInfo?.RemoveMember(memberID);
            if (res.HasValue && res.Value)
            {
                this.SomeoneQuitGroup?.Invoke(groupID, memberID);
                this.SomeoneQuitGroupNotify(groupID, memberID); 
            }            
            if (groupInfo.Members.Count == 0)
            {
                this.dic.Remove(groupID);
                this.GroupDestroyed?.Invoke(groupID);
            }                                      
        }

        public void RemoveFromGroup(string groupID, List<string> members)
        {
            this.RemoveFromGroup(groupID, members, SystemSettings.ServerDefaultId);
        }

        private void RemoveFromGroup(string groupID, List<string> members, string operatorID)
        {
            this.GetGroup(groupID)?.RemoveMembers(members);
            this.SomeoneBeRemovedFromGroup?.Invoke(groupID, members, operatorID);
            //先向组成员发送某些人被踢出群组的通知
            this.SomeoneBeRemovedFromGroupNotify(groupID, members, operatorID);
            //再向被踢出群的成员发送通知 客户端要触发 BeRemovedFromGroup待实现
            foreach (string memberID in members)
            {
                this.RemoveFromGroupNotify(memberID, groupID, operatorID);
            }
            if (this.GetGroup(groupID)?.Members.Count == 0)
            {
                this.dic.Remove(groupID);
                this.GroupDestroyed?.Invoke(groupID);
            }
        }

        public void SetGroupActionType(string groupID, bool quitGroupWhenOffline)
        {
            this.GetGroup(groupID) ?.SetGroupActionType(quitGroupWhenOffline);
        }

        public void SetGroupTag(string groupID, string tag)
        {
            this.SetGroupTag(groupID, tag, SystemSettings.ServerDefaultId);
        } 

        private void SetGroupTag(string groupID, string tag,string operatorID)
        {
            this.GetGroup(groupID)?.SetTag(tag);
            this.GroupTagChanged?.Invoke(groupID, tag, operatorID);
            this.GroupTagChangedNotify(groupID, tag, operatorID);
        }
        #endregion


        #region ITcpReceivedPlugin
        public Task OnTcpReceived(ZySocketClient client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.Broadcast)
                {
                    BroadcastContract broadcastContract = SerializeConvert.JsonDeserializeFromBytes<BroadcastContract>(packageInfo.Body);
                    this.BroadcastReceived?.Invoke(packageInfo.UserID, broadcastContract.GroupID, broadcastContract.InformatinType, broadcastContract.Content, broadcastContract.Tag);

                    #region 转发给群组成员
                    var memberList = this.GetGroup(broadcastContract.GroupID)?.Members ?? new List<string>();
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
                                this._service.Logger.Error(ee, $"转发给{socketClient.Id}的Broadcast消息失败");
                            }
                        }
                    }
                    #endregion
                    return EasyTask.CompletedTask;
                }
                else if (packageInfo.MessageType == (int)MessageType.GetGroupInfomation)
                {
                    GroupContract groupContract = SerializeConvert.JsonDeserializeFromBytes<GroupContract>(packageInfo.Body);
                    GroupInfo groupInfo = this.GetGroup(groupContract.GroupID) ?? new GroupInfo();
                    client.ReplyQueryResult(SerializeConvert.JsonSerializeToBytes(groupInfo));
                    return EasyTask.CompletedTask;
                }
                else if (packageInfo.MessageType == (int)MessageType.GetMyGroups)
                {
                    var list= this.GetGroupIDs4User(packageInfo.UserID);
                    client.ReplyQueryResult(SerializeConvert.JsonSerializeToBytes(list));
                    return EasyTask.CompletedTask;                
                }
                else if (packageInfo.MessageType == (int)MessageType.GetOnlineGroupmates)
                {
                    var list = this.GetOnlineGroupmates(packageInfo.UserID);
                    client.ReplyQueryResult(SerializeConvert.JsonSerializeToBytes(list));
                    return EasyTask.CompletedTask;
                }
                else if (packageInfo.MessageType == (int)MessageType.InviteJoinGroup)
                {
                    RecruitOrFireContract contract = SerializeConvert.JsonDeserializeFromBytes<RecruitOrFireContract>(packageInfo.Body);
                    this.PullIntoGroup(contract.GroupID, contract.Members, contract.OperatorID);
                    return EasyTask.CompletedTask;
                }
                else if (packageInfo.MessageType == (int)MessageType.JoinGroup)
                {
                    GroupContract joinGroupContract = SerializeConvert.JsonDeserializeFromBytes<GroupContract>(packageInfo.Body);
                   var group =  this.JoinGroup(joinGroupContract.GroupID, packageInfo.UserID);
                    client.ReplyQueryResult(SerializeConvert.JsonSerializeToBytes(group));
                    return EasyTask.CompletedTask;
                }
                else if (packageInfo.MessageType == (int)MessageType.PullIntoGroup)
                {
                    RecruitOrFireContract pullIntoGroupContract = SerializeConvert.JsonDeserializeFromBytes<RecruitOrFireContract>(packageInfo.Body);
                    this.PullIntoGroup(pullIntoGroupContract.GroupID, pullIntoGroupContract.Members, packageInfo.UserID);
                    return EasyTask.CompletedTask;
                }
                else if (packageInfo.MessageType == (int)MessageType.QuitGroup)
                {
                    GroupContract quitGroupContract = SerializeConvert.JsonDeserializeFromBytes<GroupContract>(packageInfo.Body);
                    this.QuitGroup(quitGroupContract.GroupID, packageInfo.UserID);
                    return EasyTask.CompletedTask;                
                }
                else if (packageInfo.MessageType == (int)MessageType.RemoveFromGroup)
                {
                    RecruitOrFireContract removeFromGroupContract = SerializeConvert.JsonDeserializeFromBytes<RecruitOrFireContract>(packageInfo.Body);
                    this.RemoveFromGroup(removeFromGroupContract.GroupID, removeFromGroupContract.Members, packageInfo.UserID);
                    return EasyTask.CompletedTask;                
                }
                else if (packageInfo.MessageType == (int)MessageType.SetGroupTag)
                {
                    SetGroupTagContract setGroupTagContract = SerializeConvert.JsonDeserializeFromBytes<SetGroupTagContract>(packageInfo.Body);
                    this.SetGroupTag(setGroupTagContract.GroupID, setGroupTagContract.Tag, packageInfo.UserID);
                    return EasyTask.CompletedTask;
                }
            }
            return e.InvokeNext();            
        }
        #endregion

        #region ITcpDisconnectedPlugin
        public Task OnTcpDisconnected(ZySocketClient client, DisconnectEventArgs e)
        {
            this.QuitGroupWhenOffline(IdUtil.GetUserId(client.Id));
            return EasyTask.CompletedTask;
        }
        #endregion

        #region Methods
        private void QuitGroupWhenOffline(string userID)
        {
            var groups = this.GetGroups4User(userID);
            foreach(var group in groups)
            {
                if (group.QuitGroupWhenOffline) { 
                    this.QuitGroup(group.ID, userID);
                }
            }
        }

        /// <summary>
        /// 获取用户所在的群组列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private List<GroupInfo> GetGroups4User(string userID)
        {
            List<GroupInfo> groups = new List<GroupInfo>();
            foreach (GroupInfo groupInfo in this.dic.Values)
            {
                if (groupInfo.Members.Contains(userID)) {
                    groups.Add(groupInfo); }
            }
            return groups;
        }

        /// <summary>
        /// 获取用户所在的群组列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private List<string> GetGroupIDs4User(string userID)
        {
            List<string> groupIDs = new List<string>();
            foreach (GroupInfo groupInfo in this.dic.Values)
            {
                if (groupInfo.Members.Contains(userID))
                {
                    groupIDs.Add(groupInfo.ID);
                }
            }
            return groupIDs;
        }

        /// <summary>
        /// 获取在线的群组成员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private List<string> GetOnlineGroupmates(string userID)
        {
            List<string> onlineGroupmates = new List<string>();
            var list = this.GetGroupmates(userID);
            foreach (var memberID in list)
            {
                if (UserManager.Instance.IsUserOnLine(memberID))
                {
                    onlineGroupmates.Add(memberID);
                }
            }
            return onlineGroupmates;
        }

        private void InvitedJoinGroup(string userID, GroupInfo group, string operatorID)
        {
            var clients = this._service.GetSocketClients(userID);

            GroupContract2 pullIntoGroupContract = new GroupContract2 { GroupInfo = group, OperatorID = operatorID };
            foreach (var client in clients)
            {
                client.SendMessageAsync(SystemSettings.ServerDefaultId, userID, (int)MessageType.InviteJoinGroup, SerializeConvert.JsonSerializeToBytes(pullIntoGroupContract));
            }            
        }


        /// <summary>
        /// 向对应的新成员发送被拉进群的通知
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="group"></param>
        /// <param name="operatorID"></param>
        private void PullIntoGroupNotify(string userID, GroupInfo group, string operatorID)
        {
            var clients = this._service.GetSocketClients(userID);

            GroupContract2 pullIntoGroupContract = new GroupContract2 { GroupInfo = group, OperatorID = operatorID };
            foreach (var client in clients)
            {
                client.SendMessageAsync(SystemSettings.ServerDefaultId, userID, (int)MessageType.PullIntoGroupNotify, SerializeConvert.JsonSerializeToBytes(pullIntoGroupContract));
            }
        }

        /// <summary>
        /// 向对应的成员发送被踢出群的通知
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="operatorID"></param>
        private void RemoveFromGroupNotify(string userID, string groupID, string operatorID)
        {
            var clients = this._service.GetSocketClients(userID);

            RecruitOrFireContract contract = new RecruitOrFireContract() { GroupID = groupID,OperatorID =operatorID };
            foreach (var client in clients)
            {
                client.SendMessageAsync(SystemSettings.ServerDefaultId, userID, (int)MessageType.RemoveFromGroupNotify, SerializeConvert.JsonSerializeToBytes(contract));
            }
        }

        /// <summary>
        /// 向群组成员发送新人被拉进群的通知
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="members"></param>
        /// <param name="operatorID"></param>
        private void SomeoneBePulledIntoGroupNotify(string groupID, List<string> members, string operatorID)
        {
            RecruitOrFireContract contract = new RecruitOrFireContract() { GroupID = groupID, OperatorID = operatorID, Members = members };

            this.SendMessageToGroupmates(groupID, (int)MessageType.SomeonePullIntoGroupNotify, SerializeConvert.JsonSerializeToBytes(contract));
        }

        /// <summary>
        /// 向群组成员发送成员被踢出群的通知
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="members"></param>
        /// <param name="operatorID"></param>
        private void SomeoneBeRemovedFromGroupNotify(string groupID, List<string> members, string operatorID)
        {
            RecruitOrFireContract contract = new RecruitOrFireContract() { GroupID = groupID, OperatorID = operatorID, Members = members };
            this.SendMessageToGroupmates(groupID, (int)MessageType.SomeoneRemoveFromGroupNotify, SerializeConvert.JsonSerializeToBytes(contract));
        }

        /// <summary>
        /// 向群组成员发送新人加入群的通知
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="memberID"></param>
        private void SomeoneJoinGroupNotify(string groupID, string memberID)
        {
            GroupNotifyContract contract =new GroupNotifyContract() { GroupID = groupID, UserID = memberID };
            this.SendMessageToGroupmates(groupID, (int)MessageType.SomeoneJoinGroupNotify, SerializeConvert.JsonSerializeToBytes(contract));
        }

        /// <summary>
        /// 向群组成员发送成员退出群的通知
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="memberID"></param>
        private void SomeoneQuitGroupNotify(string groupID, string memberID)
        {
            GroupNotifyContract contract = new GroupNotifyContract() { GroupID = groupID, UserID = memberID };
            this.SendMessageToGroupmates(groupID, (int)MessageType.SomeoneQuitGroupNotify, SerializeConvert.JsonSerializeToBytes(contract));
        }

        /// <summary>
        /// 向群组成员发送某群成员在线的通知
        /// </summary>
        /// <param name="memberID"></param>
        private void GroupmateOnlineNotify(string memberID)
        {
            UserContract contract = new UserContract() {  UserID = memberID };
            var list = this.GetGroupmates(memberID);
            foreach (var groupID in list)
            {
                this.GroupmateOnlineNotify(groupID, memberID,true);
            }
        }

        /// <summary>
        /// 向群组成员发送某群成员下线的通知
        /// </summary>
        /// <param name="memberID"></param>
        private void GroupmateOfflineNotify(string memberID)
        {
            UserContract contract = new UserContract() { UserID = memberID };
            var list = this.GetGroupmates(memberID);
            foreach (var groupID in list)
            {
                this.GroupmateOnlineNotify(groupID, memberID, false);
            }
        }

        private void GroupmateOnlineNotify(string groupID, string memberID,bool isOnline)
        {
            UserContract contract = new UserContract() { UserID = memberID };
            this.SendMessageToGroupmates(groupID, isOnline?(int)MessageType.GroupmateOnlineNotify:(int)MessageType.GroupmateOfflineNotify, SerializeConvert.JsonSerializeToBytes(contract));
        }
        
        /// <summary>
        /// 向群组成员发送群组信息变更的通知
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="tag"></param>
        /// <param name="operatorID"></param>
        private void GroupTagChangedNotify(string groupID, string tag, string operatorID)
        {
            SetGroupTagContract contract = new SetGroupTagContract() { GroupID = groupID, Tag = tag, OperatorID = operatorID };
            this.SendMessageToGroupmates(groupID, (int)MessageType.GroupTagChangedNotify, SerializeConvert.JsonSerializeToBytes(contract));
        }


        /// <summary>
        /// 向群组成员发送消息
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="messageType"></param>
        /// <param name="messageContent"></param>
        private void SendMessageToGroupmates(string groupID, int messageType, byte[] messageContent)
        {
            var memberList = this.GetGroup(groupID)?.Members;
            foreach (var userID in memberList)
            {
                var clients = this._service.GetSocketClients(userID);
                foreach (var client in clients)
                {
                    client.SendMessageAsync(SystemSettings.ServerDefaultId, userID, messageType, messageContent);
                }
            }
        }

        #endregion
    }
}
