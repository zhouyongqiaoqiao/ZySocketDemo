using System;
using System.Text;
using TouchSocket.Core;
using ZySocketCore.Core.Enum;
using ZySocketCore.Extension;
using ZySocketCore.Manager;
using ZySocketCore.Utils;

namespace ZySocketCore.Core
{
    /// <summary>
    /// 轻量级协议数据处理适配器
    /// </summary>
    internal class ZyLightFixedHeaderDataAdapter : CustomFixedHeaderDataHandlingAdapter<ZyLightFixedHeaderPackageInfo>
    {
        public override int HeaderLength => ZyLightFixedHeaderPackageInfo.HeaderLength;

        protected override ZyLightFixedHeaderPackageInfo GetInstance()
        {
            return new ZyLightFixedHeaderPackageInfo(this.Reset);
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
    internal class ZyLightFixedHeaderPackageInfo : IFixedHeaderRequestInfo
    {

        private readonly Action m_actionForReset;

        public ZyLightFixedHeaderPackageInfo(Action _actionForReset)
        {
            isLittleEndian = CheckLittleEndian();
            this.m_actionForReset = _actionForReset;
        }

        public ZyLightFixedHeaderPackageInfo(string userID, string destUserID, int messageType, byte[]? body)
        {
            UserID = userID;
            MessageID = IDGenerator.Instance.GetNextID();
            DestUserID = destUserID;
            MessageType = messageType;
            Body = body;
            isLittleEndian = CheckLittleEndian();
            TouchSocketBitConverter.DefaultEndianType = isLittleEndian ? EndianType.Little : EndianType.Big;
        }
        public byte StartToken { get; set; } = 0x00; //第1


        /// <summary>
        /// 当前用户ID
        /// </summary>
        public string UserID { get => userID ?? ""; set => userID = value; }
        /// <summary>
        /// 目标用户ID
        /// </summary>
        public string DestUserID { get => destUserID ?? ""; set => destUserID = value; }

        public ClientType ClientType { get; private set; } = ClientType.Win; //第6

        /// <summary>
        /// 消息号自增  
        /// </summary>
        public uint MessageID { get; private set; } = 0; //第4入列

        /// <summary>
        /// 消息类型
        /// </summary>
        public int MessageType { get; private set; }  //第5

        /// <summary>
        /// 消息内容
        /// </summary>
        public byte[] Body { get => body ?? new byte[0]; private set => body = value; }



        public int BodyLength { get; private set; }


        private bool isLittleEndian = false;
        private string userID = "";
        private string destUserID = "";
        private byte[] body = new byte[0];

        private bool CheckLittleEndian()
        {
            //将int 转换到数组中，然后根据数组的地址进行判断
            byte[] buf = BitConverter.GetBytes(1);
            return buf[0] == 1;
        }

        public static int HeaderLength => 22 + 2 * GlobalUtil.MaxLengthOfUserID;

        /// <summary>
        /// 完整的用户ID
        /// </summary>
        public string FullUserID => IdUtil.BuildFullUserId(this.ClientType, UserID);

        public override string ToString()
        {
            int bodyLength = Body == null ? -1 : Body.Length;
            return $"StartToken:{StartToken},UserID:{UserID},DestUserID:{DestUserID},MessageID:{MessageID},MessageType:{MessageType},BodyLength:{bodyLength}";
        }

        public byte[] ToByteArray()
        {
            int bodyLength = Body == null ? -1 : Body.Length;
            byte[] buff = bodyLength > -1 ? new byte[HeaderLength + bodyLength] : new byte[HeaderLength];
            byte[] messageIDBuff = BitConverter.GetBytes(MessageID);
            byte[] messageTypeBuff = BitConverter.GetBytes(MessageType);
            byte[] bodyLengthBuff = BitConverter.GetBytes(bodyLength);

            using (ByteBlock byteBlock = new ByteBlock(buff))
            {
                byteBlock.Write(StartToken);
                byteBlock.Write(UserID.PadRight(GlobalUtil.MaxLengthOfUserID));
                byteBlock.Write(DestUserID.PadRight(GlobalUtil.MaxLengthOfUserID));
                byteBlock.Write(messageIDBuff);
                byteBlock.Write(messageTypeBuff);
                byteBlock.Write((byte)ClientType);
                byteBlock.Write(bodyLengthBuff);
                if (bodyLength > -1)
                {
                    byteBlock.Write(Body);
                }
            }


            //Buffer.BlockCopy(messageIDBuff, 0, buff, 0, messageIDBuff.Length);
            //Buffer.BlockCopy(messageTypeBuff, 0, buff, 4, messageTypeBuff.Length);
            //Buffer.BlockCopy(clinetTypeBuff, 0, buff, 8, clinetTypeBuff.Length);
            //Buffer.BlockCopy(bodyLengthBuff, 0, buff, 9, bodyLengthBuff.Length);
            //if (bodyLength > -1)
            //{
            //    Buffer.BlockCopy(Body, 0, buff, 13, bodyLength);
            //}
            return buff;
        }

        public bool OnParsingHeader(byte[] header)
        {
            if (header.Length != HeaderLength)
            {
                this.m_actionForReset.Invoke();
                return false;
            }
            using (ByteBlock byteBlock = new ByteBlock(header))
            {
                EndianType endianType = isLittleEndian ? EndianType.Little : EndianType.Big;
                StartToken = (byte)byteBlock.ReadByte();
                UserID = byteBlock.ReadString(endianType).Trim();
                DestUserID = byteBlock.ReadString(endianType).Trim();
                MessageID = byteBlock.ReadUInt32(endianType);
                MessageType = byteBlock.ReadInt32(endianType);
                ClientType = (ClientType)byteBlock.ReadByte();
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
            this.m_actionForReset.Invoke();
            return false;
        }
    }
}
