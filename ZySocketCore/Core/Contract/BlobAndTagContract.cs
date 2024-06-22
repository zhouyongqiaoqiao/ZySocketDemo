using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.Contract
{
    internal class BlobAndTagContract
    {
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
    }
}
