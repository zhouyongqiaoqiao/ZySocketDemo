using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZySocketCore.Core.Contacts.Server;

namespace ZySocketServer
{

    internal class CustomizeContactManager : IContactsManager
    {
        public List<string> GetContacts(string userID)
        {
            if (userID == "aa01")
                return new List<string>() { "aa02", "aa03", "aa04", "aa05" };
            else if (userID == "aa02")
                return new List<string>() { "aa01", "aa03", "aa04", "aa05" };
            else if (userID == "aa03")
                return new List<string>() { "aa01", "aa02", "aa04", "aa05" };
            else if (userID == "aa04")
                return new List<string>() { "aa01", "aa02", "aa03", "aa05" };
            else if (userID == "aa05")
                return new List<string>() { "aa01", "aa02", "aa03", "aa04" };
            else
                return new List<string>();
        }

        public List<string> GetGroupMemberList(string groupID)
        {
            if(groupID == "g01")
            return new List<string>() { "aa01", "aa02" , "aa03"   };
            else if (groupID == "g02")
                return new List<string>() { "aa01", "aa04" };
            else if (groupID == "g03")
                return new List<string>() { "aa02", "aa05" };
            else if (groupID == "g04")
                return new List<string>() { "aa03", "aa04" };
            else if (groupID == "g05")
                return new List<string>() { "aa01", "aa02", "aa03", "aa04", "aa05" };
            else
                return new List<string>();
        }

        public void OnUserOffline(string userID)
        {
            return;
        }
    }
}
