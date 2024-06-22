using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.DynamicGroup.Client
{
    //
    // 摘要:
    //     用于客户端向服务器发送与动态组相关的信息，或从服务器获取动态组（自己参与其中的）的相关通知。 1.动态组分为两种类型：组员掉线就从组中移除，或者不移除（默认值）。服务端通过IDynamicGroupController.SetGroupActionType方法可以设定这个类型。
    //     2.巧妙使用Tag。通过SetGroupTag方法，可以设定目标组携带的Tag（比如，群名称、群公告等），并会同步到所有组员。
    public interface IDynamicGroupOutter
    {

        /// <summary>
        /// 当目标组的Tag发生变化时，触发该事件。GroupID - newTag - OperatorID
        /// </summary>
        event Action<string, string, string> GroupTagChanged;

        /// <summary>
        /// 当自己被移除组时，触发该事件。参数：groupID - operatorID 当OperatorID为null时，表示操作者是服务端。
        /// </summary>
        event Action<string, string> BeRemovedFromGroup;

        /// <summary>
        /// 当自己被拉入组时，触发该事件。参数：groupInformation - operatorID。 当OperatorID为null时，表示操作者是服务端。
        /// </summary>
        event Action<GroupInfo, string> BePulledIntoGroup;

        /// <summary>
        /// 当某人被移除组时，触发该事件。参数：groupID - memberIDList - operatorID 当OperatorID为null时，表示操作者是服务端。
        /// </summary>
        event Action<string, List<string>, string> SomeoneBeRemovedFromGroup;

        /// <summary>
        /// 当某人被拉入组时，触发该事件。参数：groupID - memberIDList - operatorID。 当OperatorID为null时，表示操作者是服务端。
        /// </summary>
        event Action<string, List<string>, string> SomeoneBePulledIntoGroup;

        /// <summary>
        /// 当成员退出组时，触发该事件。参数：groupID - memberID
        /// </summary>
        event Action<string, string> SomeoneQuitGroup;

        /// <summary>
        /// 当新成员加入组时，触发该事件。参数：groupID - memberID
        /// </summary>
        event Action<string, string> SomeoneJoinGroup;

        /// <summary>
        /// 当某个动态组的组友上线时，触发此事件。
        /// </summary>
        event Action<string> GroupmateOnline;

        /// <summary>
        /// 当某个动态组的组友掉线时，触发此事件。
        /// </summary>
        event Action<string> GroupmateOffline;

        /// <summary>
        /// 当接收到某个组内的广播消息时，触发此事件。参数：broadcasterID - groupID - broadcastType - broadcastContent - tag。 如果broadcasterID为null，表示是服务端发送的广播。
        /// </summary>
        event Action<string, string, int, byte[], string> BroadcastReceived;

        /// <summary>
        /// 当收到加入组的邀请时，触发该事件。参数：GroupInformation - OperatorID。 注意：收到邀请时，自己尚未加入组。若接受邀请，则可以调用JoinGroup加入目标组。
        /// </summary>
        event Action<GroupInfo, string> BeInvitedJoinGroup;

        /// <summary>
        /// 在目标组内广播消息。
        /// </summary>
        /// <param name="groupID">目标组ID</param>
        /// <param name="broadcastType">广播消息类型</param>
        /// <param name="broadcastContent">广播消息内容</param>
        /// <param name="tag">附加信息</param>
        void Broadcast(string groupID, int broadcastType, byte[] broadcastContent, string tag);

        /// <summary>
        /// 清空本地缓存的所有组信息。（在掉线重连成功后，本地缓存有可能已经过时）
        /// </summary>
        void ClearLocalCache();

        /// <summary>
        /// 获取组信息。
        /// </summary>
        /// <param name="groupID">目标组ID</param>
        /// <param name="useLocalCache">是否使用本地缓存？如果为true且本地有缓存，则直接从本地缓存获取组信息。</param>
        /// <returns></returns>
        GroupInfo GetGroupInformation(string groupID, bool useLocalCache);

        /// <summary>
        /// 获取我的所有组
        /// </summary>
        /// <returns>组ID列表</returns>
        List<string> GetMyGroups();

        /// <summary>
        /// 获取所有在线组友的列表
        /// </summary>
        /// <returns></returns>
        List<string> GetOnlineGroupmates();

        /// <summary>
        /// 邀请用户加入动态组。被邀请的用户将会触发BeInvitedJoinGroup事件。
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="users"></param>
        void InviteJoinGroup(string groupID, List<string> users);

        /// <summary>
        /// 加入组
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        GroupInfo JoinGroup(string groupID);

        /// <summary>
        /// 将用户拉入组。如果操作成功，将收到SomeoneBePulledIntoGroup事件通知。
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="members"></param>
        void PullIntoGroup(string groupID, List<string> members);

        /// <summary>
        /// 退出组。
        /// </summary>
        /// <param name="groupID"></param>
        void QuitGroup(string groupID);

        /// <summary>
        /// 将用户从动态组中移除。如果操作成功，将收到SomeoneBeRemovedFromGroup事件通知。
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="members"></param>
        void RemoveFromGroup(string groupID, List<string> members);

        /// <summary>
        /// 设置目标组的Tag。其它组成员将收到GroupTagChanged事件通知。 前提：目标组必须存在，否则将忽略该调用。
        /// </summary>
        /// <param name="groupID">目标组ID</param>
        /// <param name="tag">新的tag</param>
        void SetGroupTag(string groupID, string tag);

        /// <summary>
        /// 从服务器获取最新的组成员列表同步到本地
        /// </summary>
        /// <param name="groupID">目标组ID</param>
        void SyncGroupMembers(string groupID);
    }
}
