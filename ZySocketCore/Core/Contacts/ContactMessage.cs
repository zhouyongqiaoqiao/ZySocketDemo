using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.Contacts
{
    /// <summary>
    /// 联系人消息
    /// </summary>
    internal class ContactMessage
    {
        public ContactMessage() { }
        public ContactMessage(string groupID, int informationType, byte[] content, string tag)
        {
            InformationType = informationType;
            Content = content;
            Tag = tag;
            GroupID = groupID;
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        public int InformationType { get; set; }

        /// <summary>
        /// 消息体内容
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// 附加消息标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 群组ID
        /// </summary>
        public string GroupID { get; set; }
    }
}
