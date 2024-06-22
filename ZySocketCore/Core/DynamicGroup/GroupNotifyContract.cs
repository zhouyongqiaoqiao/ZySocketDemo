using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.DynamicGroup
{
    internal class GroupNotifyContract
    {
        public string GroupID { get; set; }
        public string UserID { get; set; }
    }

    internal class GroupNotifyContract2
    {
        public string GroupID { get; set; }
        public GroupInfo GroupInfo { get; set; }
    }
}
