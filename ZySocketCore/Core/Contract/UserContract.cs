using System;
using System.Collections.Generic;
using System.Text;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Core.Contract
{
    internal class UserContract
    {
        public UserContract()
        {
            
        }
        public UserContract(string userID)
        {
            UserID = userID;
        }

        public UserContract(string userID , ClientType clientType)
        {
            UserID = userID;
            ClientType = clientType;
            IsAllClientType = false;
        }

        public string UserID { get; set; }

        public ClientType ClientType { get; set; }

        public bool IsAllClientType { get; set; } = true;
    }
}
