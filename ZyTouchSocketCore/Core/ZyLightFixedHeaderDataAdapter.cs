using System;
using System.Buffers;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Core.Enum;
using ZySocketCore.Manager;

namespace ZyLightTouchSocketCore.Core
{
    /// <summary>
    /// 轻量级协议数据处理适配器
    /// </summary>
    public class ZyLightFixedHeaderDataAdapter : CustomFixedHeaderDataHandlingAdapter<ZyLightFixedHeaderPackageInfo>
    {
        public override int HeaderLength => 13;

        protected override ZyLightFixedHeaderPackageInfo GetInstance()
        {
            return new ZyLightFixedHeaderPackageInfo();
        }

        public override bool CanSendRequestInfo => true;

        protected override void PreviewSend(IRequestInfo requestInfo)
        {
            if (requestInfo is ZyLightFixedHeaderPackageInfo zyFixedHeaderPackageInfo)
            {
                var data = zyFixedHeaderPackageInfo.ToByteArray();
                GoSend(data, 0, data.Length);
                return;
            }
            base.PreviewSend(requestInfo);
        }
    }

    /// <summary>
    /// 轻量级协议数据包信息
    /// </summary>
    public class ZyLightFixedHeaderPackageInfo : IFixedHeaderRequestInfo
    {
        public ZyLightFixedHeaderPackageInfo()
        {
            isLittleEndian = CheckLittleEndian();
        }

        public ZyLightFixedHeaderPackageInfo(int messageType, byte[]? body)
        {
            MessageID = IDGenerator.Instance.GetNextID();
            MessageType = messageType;
            Body = body;
            isLittleEndian = CheckLittleEndian();
        }

        public ClinetType ClinetType { get; private set; } = ClinetType.Win; //第3

        /// <summary>
        /// 消息号自增  
        /// </summary>
        public uint MessageID { get; private set; } = 0; //第1入列

        /// <summary>
        /// 消息类型
        /// </summary>
        public int MessageType { get; private set; }  //第2

        /// <summary>
        /// 消息内容
        /// </summary>
        public byte[] Body { get => body?? new byte[0]; private set => body = value; } //为解决Body 为null时，接收方不会触发该消息的问题
        public int BodyLength { get; private set; }

        private bool isLittleEndian = false;
        private byte[] body = new byte[0];

        private bool CheckLittleEndian()
        {
            //将int 转换到数组中，然后根据数组的地址进行判断
            byte[] buf = BitConverter.GetBytes(1);
            return buf[0] == 1;
        }

        public override string ToString()
        {
            int bodyLength = Body == null ? -1 : Body.Length;
            return $"MessageID:{MessageID},MessageType:{MessageType},BodyLength:{bodyLength}";
        }

        public byte[] ToByteArray()
        {
            int bodyLength = Body == null ? -1 : Body.Length;
            byte[] buff = bodyLength > -1 ? new byte[13 + bodyLength] : new byte[13];
            byte[] messageIDBuff = BitConverter.GetBytes(MessageID);
            byte[] messageTypeBuff = BitConverter.GetBytes(MessageType);
            byte[] clinetTypeBuff = BitConverter.GetBytes((byte)ClinetType);
            byte[] bodyLengthBuff = BitConverter.GetBytes(bodyLength);
            Buffer.BlockCopy(messageIDBuff, 0, buff, 0, messageIDBuff.Length);
            Buffer.BlockCopy(messageTypeBuff, 0, buff, 4, messageTypeBuff.Length);
            Buffer.BlockCopy(clinetTypeBuff, 0, buff, 8, clinetTypeBuff.Length);
            Buffer.BlockCopy(bodyLengthBuff, 0, buff, 9, bodyLengthBuff.Length);
            if (bodyLength > -1)
            {
                Buffer.BlockCopy(Body, 0, buff, 13, bodyLength);
            }
            return buff;
        }

        public bool OnParsingHeader(byte[] header)
        {
            using (ByteBlock byteBlock = new ByteBlock(header))
            {
                EndianType endianType = isLittleEndian ? EndianType.Little : EndianType.Big;
                MessageID = byteBlock.ReadUInt32(endianType);
                MessageType = byteBlock.ReadInt32(endianType);
                ClinetType = (ClinetType)byteBlock.ReadByte();
                BodyLength = byteBlock.ReadInt32(endianType);
            }
            return true;
        }

        public bool OnParsingBody(byte[] body)
        {
            //if (this.BodyLength == 0 && body.Length ==0) {
            //    this.Body = null;
            //    return true;
            //}
            if (body?.Length == BodyLength)
            {
                Body = body;
                return true;
            }
            return false;
        }
    }
}
