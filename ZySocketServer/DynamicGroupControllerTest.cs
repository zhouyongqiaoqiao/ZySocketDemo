using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZySocketCore.Core.DynamicGroup.Server;

namespace ZySocketServer
{
    internal class DynamicGroupControllerTest
    {
        private IDynamicGroupController _dynamicGroupController;
        public DynamicGroupControllerTest( IDynamicGroupController dynamicGroupController)
        {
            this._dynamicGroupController = dynamicGroupController;
            this._dynamicGroupController.BroadcastReceived += DynamicGroupController_BroadcastReceived;

            this._dynamicGroupController.GroupCreated += _dynamicGroupController_GroupCreated;
            this._dynamicGroupController.GroupDestroyed += _dynamicGroupController_GroupDestroyed;
            this._dynamicGroupController.SomeoneJoinGroup += _dynamicGroupController_SomeoneJoinGroup;
            this._dynamicGroupController.SomeoneQuitGroup += _dynamicGroupController_SomeoneQuitGroup;
            this._dynamicGroupController.SomeoneBePulledIntoGroup += _dynamicGroupController_SomeoneBePulledIntoGroup;
            this._dynamicGroupController.SomeoneBeRemovedFromGroup += _dynamicGroupController_SomeoneBeRemovedFromGroup;
            this._dynamicGroupController.GroupTagChanged += _dynamicGroupController_GroupTagChanged;
        }

        private void _dynamicGroupController_GroupTagChanged(string groupID, string newTag, string operatorID)
        {
            Console.WriteLine($"群组{groupID}标签被{operatorID}改变为： {newTag} ");
        }

        private void _dynamicGroupController_SomeoneBeRemovedFromGroup(string groupID, List<string> memberIDs, string operatorID)
        {
            Console.WriteLine($"群组{groupID}中成员{string.Join(",", memberIDs)}被{operatorID}移出");
        }

        private void _dynamicGroupController_SomeoneBePulledIntoGroup(string groupID, List<string> memberIDs, string operatorID)
        {
            Console.WriteLine($"群组{groupID}中成员{string.Join(",", memberIDs)}被{operatorID}拉入");
        }

        private void _dynamicGroupController_SomeoneQuitGroup(string groupID, string memberID)
        {
           Console.WriteLine($"群组{groupID}中成员{memberID}退出");
        }

        private void _dynamicGroupController_SomeoneJoinGroup(string groupID, string memberID)
        {
           Console.WriteLine($"群组{groupID}中成员{memberID}加入");
        }

        private void _dynamicGroupController_GroupDestroyed(string groupID)
        {
           Console.WriteLine($"群组{groupID}被销毁");
        }

        private void _dynamicGroupController_GroupCreated(string groupID)
        {
            Console.WriteLine($"群组{groupID}被创建");
        }

        void DynamicGroupController_BroadcastReceived(string broadcasterID, string groupID, int broadcastType, byte[] broadcastContent, string tag)
        {
            Console.WriteLine($"收到群组广播消息：broadcasterID:{broadcasterID} groupID:{groupID} broadcastType:{broadcastType} broadcastContent:{Encoding.UTF8.GetString(broadcastContent)} tag:{tag}");
        }
    }
}
