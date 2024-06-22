using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.Contacts.Server
{
    public interface IContactsManager
    {
        /// <summary>
        /// 获取用户的相关联系人。
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<string> GetContacts(string userID);

        /// <summary>
        /// 获取目标组的所有成员。
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        List<string> GetGroupMemberList(string groupID);

        /// <summary>
        /// 当用户下线时，ESF通过此方法回调联系人管理器。 【有些动态关系可能需要当用户下线的时候解除，为了在解除之前能通知其对应的联系人，所以，ESF会在完成下线通知后，才回调此方法】
        /// </summary>
        /// <param name="userID"></param>
        void OnUserOffline(string userID);
    }
}
