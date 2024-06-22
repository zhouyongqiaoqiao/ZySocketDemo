using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Core;

namespace ZySocketCore.Extension
{
    public static class ByteBlockExtension
    {

        public static string ReadString(this ByteBlock block, EndianType endianType)
        {
            var len = block.ReadInt32(endianType);
            if (len < 0)
            {
                return null;
            }
            else
            {
                var str = Encoding.UTF8.GetString(block.Buffer, block.Pos, len);
                block.Position += len;
                return str;
            }
        }
    }
}
