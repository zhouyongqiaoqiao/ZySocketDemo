using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.Contract
{
    internal class FriendOnOrOfflineContract
    {
        public FriendOnOrOfflineContract() { }
        public FriendOnOrOfflineContract(string friendId, bool isOnline)
        {
            FriendId = friendId;
            IsOnline = isOnline;
        }

        public string FriendId { get; set; }
        public bool IsOnline { get; set; }
    }
}
