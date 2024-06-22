using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Client;
using ZySocketCore.Core.Contacts;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Core.DynamicGroup.Client
{
    internal class DynamicGroupOutter : IDynamicGroupOutter
    {
        Dictionary<string, GroupInfo> dic = new Dictionary<string, GroupInfo>();
        private readonly ZyClientEngine clientEngine;
        public DynamicGroupOutter(ZyClientEngine engine)
        {
            this.clientEngine = engine;
            this.clientEngine.Received += DataReceived;
        }

        private Task DataReceived(TcpClient client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.Broadcast)
                {
                    BroadcastContract broadcastContract = SerializeConvert.JsonDeserializeFromBytes<BroadcastContract>(packageInfo.Body);
                    string sourceID = packageInfo.UserID;
                    if (sourceID == SystemSettings.ServerDefaultId)
                    {
                        sourceID = null;
                    }
                    this.BroadcastReceived?.Invoke(sourceID, broadcastContract.GroupID, broadcastContract.InformatinType, broadcastContract.Content, broadcastContract.Tag);
                }
                else if (packageInfo.MessageType == (int)MessageType.SomeoneJoinGroupNotify)
                {
                    GroupNotifyContract groupNotifyContract = SerializeConvert.JsonDeserializeFromBytes<GroupNotifyContract>(packageInfo.Body);
                    if (this.dic.ContainsKey(groupNotifyContract.GroupID))
                    {
                        this.dic[groupNotifyContract.GroupID].AddMember(groupNotifyContract.UserID);
                    }
                    this.SomeoneJoinGroup?.Invoke(groupNotifyContract.GroupID, groupNotifyContract.UserID);
                }
                else if (packageInfo.MessageType == (int)MessageType.SomeoneQuitGroupNotify)
                {
                    GroupNotifyContract groupNotifyContract = SerializeConvert.JsonDeserializeFromBytes<GroupNotifyContract>(packageInfo.Body);
                    if (this.dic.ContainsKey(groupNotifyContract.GroupID))
                    {
                        this.dic[groupNotifyContract.GroupID].RemoveMember(groupNotifyContract.UserID);
                    }
                    this.SomeoneQuitGroup?.Invoke(groupNotifyContract.GroupID, groupNotifyContract.UserID);
                }
                else if (packageInfo.MessageType == (int)MessageType.SomeonePullIntoGroupNotify)
                {
                    RecruitOrFireContract recruitOrFireContract = SerializeConvert.JsonDeserializeFromBytes<RecruitOrFireContract>(packageInfo.Body);
                    if (this.dic.ContainsKey(recruitOrFireContract.GroupID))
                    {
                        this.dic[recruitOrFireContract.GroupID].AddMembers(recruitOrFireContract.Members);
                    }
                    this.SomeoneBePulledIntoGroup?.Invoke(recruitOrFireContract.GroupID, recruitOrFireContract.Members, recruitOrFireContract.OperatorID);
                }
                else if (packageInfo.MessageType == (int)MessageType.SomeoneRemoveFromGroupNotify)
                {
                    RecruitOrFireContract recruitOrFireContract = SerializeConvert.JsonDeserializeFromBytes<RecruitOrFireContract>(packageInfo.Body);
                    if (this.dic.ContainsKey(recruitOrFireContract.GroupID))
                    {
                        this.dic[recruitOrFireContract.GroupID].RemoveMembers(recruitOrFireContract.Members);
                    }
                    this.SomeoneBeRemovedFromGroup?.Invoke(recruitOrFireContract.GroupID, recruitOrFireContract.Members, recruitOrFireContract.OperatorID);

                }
                else if (packageInfo.MessageType == (int)MessageType.PullIntoGroupNotify)
                {
                    GroupContract2 groupNotifyContract2 = SerializeConvert.JsonDeserializeFromBytes<GroupContract2>(packageInfo.Body);
                    if (!this.dic.ContainsKey(groupNotifyContract2.GroupInfo.ID))
                    {
                        this.dic.Add(groupNotifyContract2.GroupInfo.ID, groupNotifyContract2.GroupInfo);
                    }
                    this.BePulledIntoGroup?.Invoke(groupNotifyContract2.GroupInfo, groupNotifyContract2.OperatorID);
                }
                else if (packageInfo.MessageType == (int)MessageType.RemoveFromGroupNotify)
                {
                    RecruitOrFireContract groupNotifyContract = SerializeConvert.JsonDeserializeFromBytes<RecruitOrFireContract>(packageInfo.Body);
                    this.dic.Remove(groupNotifyContract.GroupID);
                    this.BeRemovedFromGroup?.Invoke(groupNotifyContract.GroupID, groupNotifyContract.OperatorID);
                }
                else if (packageInfo.MessageType == (int)MessageType.GroupmateOfflineNotify) { }
                else if (packageInfo.MessageType == (int)MessageType.InvitedJoinGroupNotify) {
                    GroupContract2 groupNotifyContract2 = SerializeConvert.JsonDeserializeFromBytes<GroupContract2>(packageInfo.Body);
                    if (!this.dic.ContainsKey(groupNotifyContract2.GroupInfo.ID))
                    {
                        this.dic.Add(groupNotifyContract2.GroupInfo.ID, groupNotifyContract2.GroupInfo);
                    }
                    this.BeInvitedJoinGroup?.Invoke(groupNotifyContract2.GroupInfo, groupNotifyContract2.OperatorID);
                }
                else if (packageInfo.MessageType == (int)MessageType.GroupTagChangedNotify)
                {
                    SetGroupTagContract setGroupTagContract = SerializeConvert.JsonDeserializeFromBytes<SetGroupTagContract>(packageInfo.Body);
                    if (this.dic.ContainsKey(setGroupTagContract.GroupID))

                    {
                        this.dic[setGroupTagContract.GroupID].Tag = setGroupTagContract.Tag;
                    }
                    this.GroupTagChanged?.Invoke(setGroupTagContract.GroupID, setGroupTagContract.Tag, setGroupTagContract.OperatorID);
                }              
           
                e.Handled = false;//表示该数据是否已经被本插件处理，无需再投递到其他插件。不影响 Received 的事件触发
            }
            return EasyTask.CompletedTask;
        }

        #region IDynamicGroupOutter
        public event Action<string, string, string> GroupTagChanged;
        public event Action<string, string> BeRemovedFromGroup;
        public event Action<GroupInfo, string> BePulledIntoGroup;
        public event Action<string, List<string>, string> SomeoneBeRemovedFromGroup;
        public event Action<string, List<string>, string> SomeoneBePulledIntoGroup;
        public event Action<string, string> SomeoneQuitGroup;
        public event Action<string, string> SomeoneJoinGroup;
        public event Action<string> GroupmateOnline;
        public event Action<string> GroupmateOffline;
        public event Action<string, string, int, byte[], string> BroadcastReceived;
        public event Action<GroupInfo, string> BeInvitedJoinGroup;

        public void Broadcast(string groupID, int broadcastType, byte[] broadcastContent, string comment)
        {
            BroadcastContract broadcastContract = new BroadcastContract(groupID, broadcastType, broadcastContent, comment);

            this.clientEngine.SendMessageAsync(this.clientEngine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.Broadcast, SerializeConvert.JsonSerializeToBytes(broadcastContract));
        }

        public void ClearLocalCache()
        {
            this.dic.Clear();
        }

        public GroupInfo GetGroupInformation(string groupID, bool useLocalCache)
        {
            if (useLocalCache && this.dic.ContainsKey(groupID))
                return this.dic[groupID];
            var group = this.GetGroupInfoFromServer(groupID);
            if (group != null && this.dic.ContainsKey(groupID))
            {
                this.dic.Remove(groupID);
                this.dic.Add(groupID, group);
            }
            return group;
        }

        public List<string> GetMyGroups()
        {
           return this.clientEngine.QueryMessage<List<string>>( SystemSettings.ServerDefaultId, (int)MessageType.GetMyGroups, null); 
        }

        public List<string> GetOnlineGroupmates()
        {
            return this.clientEngine.QueryMessage<List<string>>(SystemSettings.ServerDefaultId, (int)MessageType.GetOnlineGroupmates, null);
        }

        public void InviteJoinGroup(string groupID, List<string> users)
        {
            RecruitOrFireContract recruitOrFireContract = new RecruitOrFireContract(groupID, users, this.clientEngine.CurrentUserID);
            this.clientEngine.SendMessageAsync(this.clientEngine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.InviteJoinGroup, SerializeConvert.JsonSerializeToBytes(recruitOrFireContract));
        }

        public GroupInfo JoinGroup(string groupID)
        {
            GroupContract groupContract = new GroupContract() { GroupID = groupID };
            GroupInfo? group = this.clientEngine.QueryMessage<GroupInfo>(SystemSettings.ServerDefaultId, (int)MessageType.JoinGroup, SerializeConvert.JsonSerializeToBytes(groupContract));
            if (!this.dic.ContainsKey(groupID))
            {
                this.dic.Add(groupID, group);
            }
            return group;
        }

        public void PullIntoGroup(string groupID, List<string> members)
        {
            RecruitOrFireContract contract = new RecruitOrFireContract(groupID, members, this.clientEngine.CurrentUserID);
            this.clientEngine.SendMessageAsync(this.clientEngine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.PullIntoGroup, SerializeConvert.JsonSerializeToBytes(contract));
            var group = this.dic.GetValue(groupID);
            group?.Members.AddRange(members);
        }

        public void QuitGroup(string groupID)
        {
            GroupContract groupContract = new GroupContract() { GroupID = groupID };
            this.clientEngine.SendMessageAsync(this.clientEngine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.QuitGroup, SerializeConvert.JsonSerializeToBytes(groupContract));
            this.dic.Remove(groupID);
        }

        public void RemoveFromGroup(string groupID, List<string> members)
        {
            RecruitOrFireContract contract  = new RecruitOrFireContract(groupID, members, this.clientEngine.CurrentUserID);
            this.clientEngine.SendMessageAsync(this.clientEngine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.RemoveFromGroup, SerializeConvert.JsonSerializeToBytes(contract));
            var group = this.dic.GetValue(groupID);
            group?.Members.RemoveAll(x => members.Contains(x));
        }

        public void SetGroupTag(string groupID, string tag)
        {
            SetGroupTagContract contract = new SetGroupTagContract() {  GroupID = groupID, Tag = tag };
            this.clientEngine.SendMessageAsync(this.clientEngine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.SetGroupTag, SerializeConvert.JsonSerializeToBytes(contract));
            var group = this.dic.GetValue(groupID);
            if(group!= null)
                group.Tag = tag;        
        }

        public void SyncGroupMembers(string groupID)
        {
            var group = this.GetGroupInformation(groupID, false);
            if(dic.ContainsKey(groupID))
                dic.Remove(groupID);
            dic.Add(groupID, group);
        }
        #endregion


        #region Metohods
        private GroupInfo GetGroupInfoFromServer(string groupID)
        {
            GroupContract groupContract = new GroupContract() { GroupID = groupID };
            return this.clientEngine.QueryMessage<GroupInfo>(SystemSettings.ServerDefaultId, (int)MessageType.GetGroupInfomation, SerializeConvert.JsonSerializeToBytes(groupContract));
        }

        #endregion
    }
}
