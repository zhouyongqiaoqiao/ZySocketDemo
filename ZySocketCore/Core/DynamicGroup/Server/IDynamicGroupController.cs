using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.DynamicGroup.Server
{
    //
    // 摘要:
    //     动态组控制器。 1.动态组分为两种类型：组员掉线就从组中移除，或者不移除（默认值）。服务端通过IDynamicGroupController.SetGroupActionType方法可以设定这个类型。
    //     2.巧妙使用PresetGroup方法，可以在服务端启动的时候，就预设一些已经存在的组（比如从DB中加载的群）。
    public interface IDynamicGroupController
    {
        /// <summary>
        /// 当目标组的Tag发生变化时，触发该事件。groupID - newTag - operatorID
        /// </summary>        
        event Action<string, string, string> GroupTagChanged;

        /// <summary>
        /// 当某些人被拉入组时，触发该事件。参数：groupID - memberIDs - operatorID。 当OperatorID为null时，表示操作者是服务端。
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
        /// 最后一个成员退出组将导致动态组被销毁，触发该事件。参数：groupID
        /// </summary>
        event Action<string> GroupDestroyed;

        /// <summary>
        /// 第一个成员加入组将导致动态组被创建，触发该事件。参数：groupID
        /// </summary>
        event Action<string> GroupCreated;

        /// <summary>
        /// 当服务端接收到要在组内转发的广播消息时，触发此事件。参数：broadcasterID - groupID - broadcastType - broadcastContent- comment
        /// </summary>
        event Action<string, string, int, byte[], string> BroadcastReceived;

        /// <summary>
        /// 当某些人被移除组时，触发该事件。参数：groupID - memberIDs - operatorID 当OperatorID为null时，表示操作者是服务端。
        /// </summary>
        event Action<string, List<string>, string> SomeoneBeRemovedFromGroup;

        /// <summary>
        /// 在组内广播信息。
        /// </summary>
        /// <param name="groupID">接收广播信息的组ID</param>
        /// <param name="broadcastType">广播信息的类型</param>
        /// <param name="broadcastContent">广播的内容</param>
        /// <param name="comment">附加信息</param>
        void Broadcast(string groupID, int broadcastType, byte[] broadcastContent, string comment);

        /// <summary>
        /// 获取组信息
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        GroupInfo GetGroup(string groupID);

        /// <summary>
        /// 获取所有动态组的ID列表
        /// </summary>
        /// <returns></returns>
        List<string> GetGroupList();

        /// <summary>
        /// 获取所有组友列表。
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<string> GetGroupmates(string userID);

        /// <summary>
        /// 获取目标组的Tag。
        /// </summary>
        /// <param name="groupID">指定组ID</param>
        /// <returns></returns>
        string GetGroupTag(string groupID);

        /// <summary>
        /// 预设组。 如果目标组ID已经存在，将抛出异常。
        /// </summary>
        /// <param name="groupID">组ID</param>
        /// <param name="members">组成员列表</param>
        void PresetGroup(string groupID, List<string> members);

        /// <summary>
        /// 将用户拉入组。
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="members"></param>
        void PullIntoGroup(string groupID, List<string> members);

        /// <summary>
        /// 将用户从动态组中移除。
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="members"></param>
        void RemoveFromGroup(string groupID, List<string> members);

        /// <summary>
        /// 设置组反应类型（当组成员掉线时）。 注意：最好是在GroupCreated事件中，即组被创建时就立即设置该组的ActionType。 如果不设置，则默认为：当目标组中的成员掉线时，不会从该组中移除成员。
        /// </summary>
        /// <param name="groupID">目标组ID</param>
        /// <param name="quitGroupWhenOffline">当目标组中的成员掉线时，是否从该组中移除？默认值：false。</param>
        void SetGroupActionType(string groupID, bool quitGroupWhenOffline);

        /// <summary>
        /// 设置目标组的Tag。组成员将收到GroupTagChanged事件通知。 前提：目标组必须存在，否则将忽略该调用。
        /// </summary>
        /// <param name="groupID">目标组ID</param>
        /// <param name="tag">新的tag</param>
        void SetGroupTag(string groupID, string tag);
    }
}
