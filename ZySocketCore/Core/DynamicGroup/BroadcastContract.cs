using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZySocketCore.Core.DynamicGroup
{
    internal class BroadcastContract
    {
        public BroadcastContract() { }
        public BroadcastContract(string groupID, int informatinType, byte[] content, string tag)
        {
            this.GroupID = groupID;
            this.Content = content;
            this.InformatinType = informatinType;
            this.Tag = tag;
        }

        public string GroupID { get; set; }
        public byte[] Content { get; set; }
        public int InformatinType { get; set; }
        public string Tag { get; set; }
    }
}
