using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.Contacts
{
    //
    // 摘要:
    //     某个组所有的成员。
    public class Groupmates
    {
        public Groupmates() { }

        public string GroupID { get; set; }

        public List<string> OfflineGroupmates { get; set; }

        public List<string> OnlineGroupmates { get; set; }
    }
}
