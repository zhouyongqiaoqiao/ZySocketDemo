using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core
{
    public class BroadcastInformation
    {
        public BroadcastInformation() { }
        public BroadcastInformation(string _sourceID, string _groupID, int _broadcastType, byte[] _content, string _tag) { 
            this.SourceID = _sourceID;
            this.GroupID = _groupID;
            this.BroadcastType = _broadcastType;
            this.Content = _content;
            this.Tag = _tag;        
        }

        //
        // 摘要:
        //     广播信息的发送者。可以为UserID或者NetServer.SystemUserID。
        public string SourceID { get; set; }
        //
        // 摘要:
        //     广播信息的接收组的ID。
        public string GroupID { get; set; }
        //
        // 摘要:
        //     广播信息类型
        public int BroadcastType { get; set; }
        //
        // 摘要:
        //     广播信息的内容
        public byte[] Content { get; set; }
        //
        // 摘要:
        //     附加信息。
        public string Tag { get; set; }
    }
}
