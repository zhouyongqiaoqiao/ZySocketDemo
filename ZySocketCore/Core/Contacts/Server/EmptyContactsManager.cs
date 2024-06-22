using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.Contacts.Server
{
    internal class EmptyContactsManager : IContactsManager
    {
        public List<string> GetContacts(string userID)
        {
            return new List<string>();
        }

        public List<string> GetGroupMemberList(string groupID)
        {
            return new List<string>();
        }

        public void OnUserOffline(string userID)
        {
            
        }
    }
}
