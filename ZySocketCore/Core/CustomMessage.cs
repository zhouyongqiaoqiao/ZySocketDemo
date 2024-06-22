using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core
{
    /// <summary>
    /// 自定义消息类
    /// </summary>
    internal class CustomMessage
    {
        /// <summary>
        /// 自定义消息类型
        /// </summary>
        public int InformationType { get; set; }

        /// <summary>
        /// 自定义消息内容
        /// </summary>
        public byte[] Content { get; set; }

    }
}
