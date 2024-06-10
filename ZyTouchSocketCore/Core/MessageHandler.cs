using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Interface;
using ZyLightTouchSocketCore.Server;
using ZyTouchSocketCore;
using ZyTouchSocketCore.Core.Enum;

namespace ZyLightTouchSocketCore.Core
{
    internal class MessageHandler: IMessageHandler
    {
        private readonly bool _isClientHandler;
        public MessageHandler(bool isClientHandler = true)
        {
            this._isClientHandler = isClientHandler;
        }

        /// <summary>
        /// 服务端自定义消息处理器
        /// </summary>
        public IServiceCustomizeHandler ServiceCustomizeHandler {private get; set; }

        /// <summary>
        /// 客户端自定义消息处理器
        /// </summary>
        public ICustomizeHandler CustomizeHandler {private get; set; }
        public void HandleMessage(ISender sender,ZyLightFixedHeaderPackageInfo packageInfo)
        {
            switch (packageInfo.MessageType)
            {

                #region 接收自定义消息
                case (int)MessageType.NORMAL_MESSAGE_ASYNC:
                case (int)MessageType.NORMAL_MESSAGE:
                    // SerializeConvert.FastBinarySerialize(obj) 序列化
                    // SerializeConvert.FastBinaryDeserialize<T>(data) 反序列化
                    // 这里可以根据消息类型进行不同的处理，比如异步消息可以直接处理，同步消息可以返回处理结果。
                    {
                        CustomMessage customizeMessage = SerializeConvert.FastBinaryDeserialize<CustomMessage>(packageInfo.Body);
                        this.HandleInformation(sender,customizeMessage.InformationType, customizeMessage.Content);
                        break;
                    }
                #endregion

                #region 接收自定义请求回复消息
                case (int)MessageType.QUERY_ASYNC:
                case (int)MessageType.QUERY:
                    {
                        CustomMessage customizeMessage = SerializeConvert.FastBinaryDeserialize<CustomMessage>(packageInfo.Body);
                        byte[]? resData = this.HandleQuery(sender, customizeMessage.InformationType, customizeMessage.Content);
                        sender.SendMessageAsync((int)MessageType.NORMAL_MESSAGE_ASYNC, resData);
                        break;
                    }
                #endregion
                default:
                    break;
            }
        }

        public void HandleMessage(ISender sender,ByteBlock byteBlock)
        {
            if (byteBlock.Len == 0) { return; }

            string[] strs = byteBlock.ToString().Split(ZyTouchSocketCore.SenderExtension.Separator_Msg_Str);
            int msgType = -1;
            if (strs.Length > 1)
            {
                int.TryParse(strs[0], out msgType);
            }
            string customMsg = strs[1];
            var customizeMessage = customMsg.Split(ZyTouchSocketCore.SenderExtension.Separator_CustomMsg_Str);
            int informationType = int.Parse(customizeMessage[0]);
            string content = customizeMessage[1];

            switch (msgType)
            {

                #region 接收自定义消息
                case (int)MessageType.NORMAL_MESSAGE_ASYNC:
                case (int)MessageType.NORMAL_MESSAGE:
                    // SerializeConvert.FastBinarySerialize(obj) 序列化
                    // SerializeConvert.FastBinaryDeserialize<T>(data) 反序列化
                    // 这里可以根据消息类型进行不同的处理，比如异步消息可以直接处理，同步消息可以返回处理结果。
                    {
                        this.HandleInformation(sender,informationType, Encoding.UTF8.GetBytes(content));
                        break;
                    }
                #endregion

                #region 接收自定义请求回复消息
                case (int)MessageType.QUERY_ASYNC:
                case (int)MessageType.QUERY:
                    {
                        byte[]? resData = this.HandleQuery(sender,informationType, Encoding.UTF8.GetBytes(content));
                        sender.SendAsync(Encoding.UTF8.GetString( resData));
                        break;
                    }
                #endregion
                default:
                    break;
            }
        }

        private void HandleInformation(ISender sender, int informationType, byte[] info)
        {
            if (!this._isClientHandler)
            {
                this.ServiceCustomizeHandler?.HandleInformation((IServerSender)sender, informationType, info);
            }
            else {
                this.CustomizeHandler?.HandleInformation(informationType, info);
            }
        }

        private byte[] HandleQuery(ISender sender, int informationType, byte[] info)
        {
            if (!this._isClientHandler)
            {
              return  this.ServiceCustomizeHandler?.HandleQuery((IServerSender)sender, informationType, info);
            }
            else
            {
              return  this.CustomizeHandler?.HandleQuery(informationType, info);
            }
        }

    }

    internal interface IMessageHandler
    {
        IServiceCustomizeHandler ServiceCustomizeHandler { set; }
        ICustomizeHandler CustomizeHandler { set; }
        void HandleMessage(ISender sender,ZyLightFixedHeaderPackageInfo packageInfo);
        void HandleMessage(ISender sender, ByteBlock byteBlock);
    }
}
