using System.Net.Sockets;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Core;
using ZySocketCore.Core.Contract;
using ZySocketCore.Core.Enum;
using ZySocketCore.Core.QueryInfo;
using ZySocketCore.Utils;

namespace ZySocketCore
{
    internal static class SenderExtension
    {
        /// <summary>
        /// 消息分隔符
        /// </summary>
        internal const string Separator_Msg_Str = "$$$";

        /// <summary>
        /// 自定义消息分隔符
        /// </summary>
        internal const string Separator_CustomMsg_Str = "^^^";

        /// <summary>
        /// 发送 自定义消息
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="userID">发送者ID</param>
        /// <param name="destUserID">接收者ID</param>
        /// <param name="messageType">信息类型</param>
        /// <param name="body">消息体</param>        
        /// <param name="destClientType">接收者设备类型</param>
        public static void SendMessage<TClient>(this TClient client, string userID, string destUserID, int messageType, byte[] body, ClientType? destClientType = null) where TClient : ISender
        {
            destUserID = GetRealDestUserID(destClientType, destUserID);
            ZyLightFixedHeaderPackageInfo? info = new ZyLightFixedHeaderPackageInfo(userID, destUserID, messageType, body);
            client.Send(info.ToByteArray());
        }

        /// <summary>
        /// 异步发送 自定义消息
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="messageType">消息类型</param>
        /// <param name="body">消息体</param>
        public static Task SendMessageAsync<TClient>(this TClient client, string userID, string destUserID, int messageType, byte[] body, ClientType? destClientType = null) where TClient : ISender
        {  
            destUserID = GetRealDestUserID(destClientType, destUserID);            
            ZyLightFixedHeaderPackageInfo? info = new ZyLightFixedHeaderPackageInfo(userID, destUserID, messageType, body);
            return client.SendAsync(info.ToByteArray());
        }

        /// <summary>
        /// 获取真实的接收者ID(拼接设备类型)
        /// </summary>
        /// <param name="destClientType"></param>
        /// <param name="destUserID"></param>
        /// <returns></returns>
        private static string GetRealDestUserID(ClientType? destClientType, string destUserID)
        {
            if(string.IsNullOrEmpty(destUserID) || IdUtil.IsFullUserId(destUserID)) return destUserID;
            //接收者不是 服务端 且设备类型不是 All 时，需要拼接设备类型
            if (destClientType != null && (destUserID != null || destUserID != SystemSettings.ServerDefaultId))
            {
                destUserID = IdUtil.BuildFullUserId(destClientType, destUserID);
            }
            return destUserID;
        }

        /// <summary>
        /// 请求回复 自定义消息
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="messageType">消息类型</param>
        /// <param name="body">消息体</param>
        public static ResponsedData QueryMessage<TClient>(this TClient client, string userID, string destUserID, int messageType, byte[] body) where TClient : IQueryer
        {
            ZyLightFixedHeaderPackageInfo? info = new ZyLightFixedHeaderPackageInfo(userID, destUserID, messageType, body);
            return client.Query(info.ToByteArray());
        }

        /// <summary>
        /// 异步请求回复 自定义消息
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="messageType">消息类型</param>
        /// <param name="body">消息体</param>
        public static Task<ResponsedData> QueryMessageAsync<TClient>(this TClient client, string userID, string destUserID, int messageType, byte[] body) where TClient : IQueryer
        {
            ZyLightFixedHeaderPackageInfo? info = new ZyLightFixedHeaderPackageInfo(userID, destUserID, messageType, body);
            return client.QueryAsync(info.ToByteArray());
        }

        ///// <summary>
        ///// 发送文本消息
        ///// </summary>
        ///// <typeparam name="TClient"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="messageType">消息类型</param>
        ///// <param name="msg">字符串</param>
        //public static void SendMessage<TClient>(this TClient client, int messageType, string msg) where TClient : ISender
        //{
        //    string text = messageType + Separator_Msg_Str + msg;
        //    client.SendAsync(text);
        //}

        ///// <summary>
        ///// 异步发送文本消息
        ///// </summary>
        ///// <typeparam name="TClient"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="messageType">消息类型</param>
        ///// <param name="body">字符串</param>
        //public static Task SendMessageAsync<TClient>(this TClient client, int messageType, string msg) where TClient : ISender
        //{
        //    string text = messageType + Separator_Msg_Str + msg;
        //    return client.SendAsync(text);
        //}


        ///// <summary>
        ///// 发送文本消息
        ///// </summary>
        ///// <typeparam name="TClient"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="messageType">消息类型</param>
        ///// <param name="msg">字符串</param>
        //public static ResponsedData QueryMessage<TClient>(this TClient client, int messageType, string msg) where TClient : IQueryer
        //{
        //    string text = messageType + Separator_Msg_Str + msg;
        //    return client.QueryAsync(text);
        //}

        ///// <summary>
        ///// 异步发送文本消息
        ///// </summary>
        ///// <typeparam name="TClient"></typeparam>
        ///// <param name="client"></param>
        ///// <param name="messageType">消息类型</param>
        ///// <param name="msg">字符串</param>
        //public static Task<ResponsedData> QueryMessageAsync<TClient>(this TClient client, int messageType, string msg) where TClient : IQueryer
        //{
        //    string text = messageType + Separator_Msg_Str + msg;
        //    return client.QueryAsync(text);
        //}



        #region CustomMessageSender
        #region SendCustomMessage
        public static void SendCustomMessage<TClient>(this TClient client,string userID, string destUserID, int informationType, byte[] body) where TClient : ISender
        {
            byte[] data = CreateCustomMessageData(informationType, body);
            client.SendMessage(userID,destUserID,(int)MessageType.NORMAL_MESSAGE, data);
        }

        public static Task SendCustomMessageAsync<TClient>(this TClient client, string userID, string destUserID, int informationType, byte[] body) where TClient : ISender
        {
            byte[] data = CreateCustomMessageData(informationType, body);
            return client.SendMessageAsync(userID, destUserID, (int)MessageType.NORMAL_MESSAGE_ASYNC, data);
        }

        //public static void SendCustomMessage<TClient>(this TClient client, string userID, string destUserID, int informationType, string body) where TClient : ISender
        //{
        //    string msg = CreateCustomMessageData(informationType, body);
        //    client.SendMessage((int)MessageType.NORMAL_MESSAGE, msg);
        //}

        //public static Task SendCustomMessageAsync<TClient>(this TClient client, string userID, string destUserID, int informationType, string body) where TClient : ISender
        //{
        //    string msg = CreateCustomMessageData(informationType, body);
        //    return client.SendMessageAsync((int)MessageType.NORMAL_MESSAGE_ASYNC, msg);
        //}

        #endregion

        #region QueryMessage

        /// <summary>
        /// 创建自定义消息
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        internal static byte[] CreateCustomMessageData(int informationType, byte[] body)
        {
            CustomMessage customeMessage = new CustomMessage()
            {
                InformationType = informationType,
                Content = body
            };
            return SerializeConvert.FastBinarySerialize(customeMessage);
        }

        public static string CreateCustomMessageData(int informationType, string msg)
        {
            return informationType + Separator_CustomMsg_Str + msg;
        }

        public static void SendCertainlyCustomMessage<TClient>(this TClient queryer, string userID, string destUserID, int informationType, byte[] body) where TClient : IQueryer
        {
            byte[] data = CreateCustomMessageData(informationType, body);
           ResponsedData responsedData =  queryer.QueryMessage(userID, destUserID, (int)MessageType.ACK_REQ, data);
        }

        public static byte[] QueryCustomMessage<TClient>(this TClient queryer, string userID, string destUserID, int informationType, byte[] body) where TClient : IQueryer
        {
            byte[] data = CreateCustomMessageData(informationType, body);
            ResponsedData res = queryer.QueryMessage(userID, destUserID, (int)MessageType.QUERY, data);
            var packageInfo = res.RequestInfo as ZyLightFixedHeaderPackageInfo;
            return packageInfo.Body;
        }

        public static async Task<byte[]> QueryCustomMessageAsync<TClient>(this TClient queryer, string userID, string destUserID, int informationType, byte[] body) where TClient : IQueryer
        {
            byte[] data = CreateCustomMessageData(informationType, body);
            var res = await  queryer.QueryMessageAsync(userID, destUserID, (int)MessageType.QUERY_ASYNC, data);
            ZyLightFixedHeaderPackageInfo packageInfo = res.RequestInfo as ZyLightFixedHeaderPackageInfo;
            return packageInfo.Body;
        }

        //public static string QueryCustomMessage<TClient>(this TClient queryer, int informationType, string msg) where TClient : IQueryer
        //{

        //    string cumstomMsg = CreateCustomMessageData(informationType, msg);
        //    ResponsedData res = queryer.QueryMessage((int)MessageType.QUERY, cumstomMsg);
        //    return Encoding.UTF8.GetString(res.Data);

        //}

        //public static async Task<string> QueryCustomMessageAsync<TClient>(this TClient queryer, int informationType, string msg) where TClient : IQueryer
        //{

        //    string cumstomMsg = CreateCustomMessageData(informationType, msg);
        //    ResponsedData res = await queryer.QueryMessageAsync((int)MessageType.QUERY_ASYNC, cumstomMsg);
        //    return Encoding.UTF8.GetString(res.Data);

        //}
        #endregion
        #endregion

        #region TagMessageSender
        /// <summary>
        /// 创建自定义消息
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        internal static byte[] CreateTagMessageData(int informationType, byte[] body,string tag)
        {
            BlobAndTagContract tagContract = new BlobAndTagContract()
            {
                InformationType = informationType,
                Content = body,
                Tag = tag
            };            
            return SerializeConvert.FastBinarySerialize(tagContract);
        }

        public static void SendTagMessage<TClient>(this TClient client, string userID, string destUserID, int informationType, byte[] body, string tag = "",ClientType? destClientType = null) where TClient : ISender
        {
            byte[] data = CreateTagMessageData(informationType, body,tag);
            client.SendMessage(userID, destUserID, (int)MessageType.BLOB_TAG, data, destClientType);
        }

        public static Task SendTagMessageAsync<TClient>(this TClient client, string userID, string destUserID, int informationType, byte[] body, string tag = "", ClientType? destClientType = null) where TClient : ISender
        {
            byte[] data = CreateTagMessageData(informationType, body, tag);
            return client.SendMessageAsync(userID, destUserID, (int)MessageType.BLOB_TAG, data, destClientType);
        }
        #endregion
    }
}
