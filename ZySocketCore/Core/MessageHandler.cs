using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Client;
using ZySocketCore.Core.Contract;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;
using ZySocketCore.Server.User;
using ZySocketCore.Utils;

namespace ZySocketCore.Core
{
    internal class MessageHandler: IMessageHandler
    {
        private readonly bool _isClientHandler;
        public MessageHandler(bool isClientHandler = true)
        {
            this._isClientHandler = isClientHandler;
        }
        public IBlobAndTagMessageHandler BlobAndTagMessageHandler { private get; set; }

        /// <summary>
        /// 客户端自定义消息处理器
        /// </summary>
        public ICustomizeHandler CustomizeHandler {private get; set; }
        public void HandleMessage(ISenderById sender,ZyLightFixedHeaderPackageInfo packageInfo)
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
                        if (this.CheckRelayMessage(sender, packageInfo,packageInfo.MessageType == (int)MessageType.NORMAL_MESSAGE_ASYNC))
                        {
                            break;
                        }
                        CustomMessage customizeMessage = SerializeConvert.FastBinaryDeserialize<CustomMessage>(packageInfo.Body);
                        this.HandleInformation(packageInfo.UserID,packageInfo.ClientType, customizeMessage.InformationType, customizeMessage.Content);
                        break;
                    }
                #endregion

                #region 接收自定义请求回复消息
                case (int)MessageType.ACK_REQ:
                case (int)MessageType.ACK_RESP:
                case (int)MessageType.QUERY_ASYNC:
                case (int)MessageType.QUERY:
                    {
                        CustomMessage customizeMessage = SerializeConvert.FastBinaryDeserialize<CustomMessage>(packageInfo.Body);
                        // 这里可以根据消息类型进行不同的处理，若是Ack请求直接回复new byte[0]，其他的同步消息可以返回处理结果。
                        byte[]? resData = packageInfo.MessageType == (int)MessageType.ACK_REQ ? new byte[0] : this.HandleQuery(packageInfo.UserID, packageInfo.ClientType, customizeMessage.InformationType, customizeMessage.Content);
                        sender.SendMessage(SystemSettings.ServerDefaultId, packageInfo.UserID, (int)MessageType.QUERY_RESPONSE, resData);
                        break;
                    }
                #endregion

                #region 带Tag消息
                case (int)MessageType.BLOB_TAG:
                    {
                        //若是服务端收到数据，且目标用户ID为空或目标用户ID为服务端默认ID，则转发
                        if (this.CheckRelayMessage(sender, packageInfo,true))
                        {
                            break;
                        }
                        BlobAndTagContract blobAndTagContract = SerializeConvert.FastBinaryDeserialize<BlobAndTagContract>(packageInfo.Body);
                        this.BlobAndTagMessageHandler?.HandleBlobAndTagMessage(packageInfo.UserID, packageInfo.ClientType, blobAndTagContract.InformationType, blobAndTagContract.Content, blobAndTagContract.Tag);

                        break;
                    } 
                #endregion
                default:
                    break;
            }
        }

        public void HandleMessage(ISenderById sender,ByteBlock byteBlock)
        {
            if (byteBlock.Len == 0) { return; }

            string[] strs = byteBlock.ToString().Split(ZySocketCore.SenderExtension.Separator_Msg_Str);
            int msgType = -1;
            if (strs.Length > 1)
            {
                int.TryParse(strs[0], out msgType);
            }
            string customMsg = strs[1];
            var customizeMessage = customMsg.Split(ZySocketCore.SenderExtension.Separator_CustomMsg_Str);
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
                        this.HandleInformation(null,ClientType.Win,informationType, Encoding.UTF8.GetBytes(content));
                        break;
                    }
                #endregion

                #region 接收自定义请求回复消息
                case (int)MessageType.QUERY_ASYNC:
                case (int)MessageType.QUERY:
                    {
                        byte[]? resData = this.HandleQuery(null, ClientType.Win, informationType, Encoding.UTF8.GetBytes(content));
                        sender.SendAsync(Encoding.UTF8.GetString( resData));
                        break;
                    }
                #endregion
                default:
                    break;
            }
        }

        private void HandleInformation(string sourceUserID,ClientType clientType, int informationType, byte[] info)
        {
            this.CustomizeHandler?.HandleInformation(sourceUserID, clientType, informationType, info);
        }

        private byte[] HandleQuery(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            return this.CustomizeHandler?.HandleQuery(sourceUserID, clientType, informationType, info);
        }

        private string GetSourceUserID(ITcpClientBase sender)
        {
            if (sender is ISocketClient socketClient)
            {
                return socketClient.Id;
            }
            if (sender is TcpClient tcpClient)
            {
                
            }
            return string.Empty;
        }

        /// <summary>
        /// 检查是否需要转发消息 （若是服务端收到数据，且目标用户ID为空或目标用户ID为服务端默认ID）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="packageInfo"></param>
        /// <returns>true:转发成功，false:未转发</returns>
        private bool CheckRelayMessage(ISenderById sender, ZyLightFixedHeaderPackageInfo packageInfo,bool isAsync)
        {
            //若是服务端收到数据，且目标用户ID为空或目标用户ID为服务端默认ID，则转发
            if (!this._isClientHandler && !string.IsNullOrEmpty(packageInfo.DestUserID) && packageInfo.DestUserID != SystemSettings.ServerDefaultId)
            {
                ISocketClient client = sender as ISocketClient;
                List<string> userList = UserManager.Instance.GetLoginIdList(packageInfo.DestUserID); 

                foreach (string fullUserID in userList)
                {
                    if (isAsync)
                    {
                        client?.SendAsync(fullUserID, packageInfo.ToByteArray());
                    }
                    else
                    {
                        client?.Send(fullUserID, packageInfo.ToByteArray());
                    }
                }
                return true;
                //this.BlobAndTagMessageHandler.HandleRelayMessage(packageInfo);
            }
            return false;
        }

    }

    internal interface IMessageHandler
    {
        IBlobAndTagMessageHandler BlobAndTagMessageHandler { set; }
        ICustomizeHandler CustomizeHandler { set; }
        void HandleMessage(ISenderById sender,ZyLightFixedHeaderPackageInfo packageInfo);
        void HandleMessage(ISenderById sender, ByteBlock byteBlock);
    }
}
