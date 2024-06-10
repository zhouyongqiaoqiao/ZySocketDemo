using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Client;
using ZyLightTouchSocketCore.Core;
using ZyTouchSocketCore.Core.Enum;

namespace ZyTouchSocketCore
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
        /// <param name="messageType">消息类型</param>
        /// <param name="body">消息体</param>
        public static void SendMessage<TClient>(this TClient client, int messageType, byte[] body) where TClient : ISender
        {
            ZyLightFixedHeaderPackageInfo? info = new ZyLightFixedHeaderPackageInfo(messageType, body);
            client.Send(info.ToByteArray());
        }

        /// <summary>
        /// 异步发送 自定义消息
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="messageType">消息类型</param>
        /// <param name="body">消息体</param>
        public static Task SendMessageAsync<TClient>(this TClient client, int messageType, byte[] body) where TClient : ISender
        {
            ZyLightFixedHeaderPackageInfo? info = new ZyLightFixedHeaderPackageInfo(messageType, body);
            return client.SendAsync(info.ToByteArray());
        }

        /// <summary>
        /// 请求回复 自定义消息
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="messageType">消息类型</param>
        /// <param name="body">消息体</param>
        public static ResponsedData QueryMessage<TClient>(this TClient client, int messageType, byte[] body) where TClient : IQueryer
        {
            ZyLightFixedHeaderPackageInfo? info = new ZyLightFixedHeaderPackageInfo(messageType, body);
            return client.Query(info.ToByteArray());
        }

        /// <summary>
        /// 异步请求回复 自定义消息
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="messageType">消息类型</param>
        /// <param name="body">消息体</param>
        public static Task<ResponsedData> QueryMessageAsync<TClient>(this TClient client, int messageType, byte[] body) where TClient : IQueryer
        {
            ZyLightFixedHeaderPackageInfo? info = new ZyLightFixedHeaderPackageInfo(messageType, body);
            return client.QueryAsync(info.ToByteArray());
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="messageType">消息类型</param>
        /// <param name="msg">字符串</param>
        public static void SendTextMessage<TClient>(this TClient client, int messageType, string msg) where TClient : ISender
        {
            string text = messageType + Separator_Msg_Str + msg;
            client.Send(text);
        }

        /// <summary>
        /// 异步发送文本消息
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="messageType">消息类型</param>
        /// <param name="body">字符串</param>
        public static Task SendTextMessageAsync<TClient>(this TClient client, int messageType, string msg) where TClient : ISender
        {
            string text = messageType + Separator_Msg_Str + msg;
            return client.SendAsync(text);
        }


        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="messageType">消息类型</param>
        /// <param name="msg">字符串</param>
        public static ResponsedData QueryTextMessage<TClient>(this TClient client, int messageType, string msg) where TClient : IQueryer
        {
            string text = messageType + Separator_Msg_Str + msg;
            return client.Query(text);
        }

        /// <summary>
        /// 异步发送文本消息
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="client"></param>
        /// <param name="messageType">消息类型</param>
        /// <param name="msg">字符串</param>
        public static Task<ResponsedData> QueryTextMessageAsync<TClient>(this TClient client, int messageType, string msg) where TClient : IQueryer
        {
            string text = messageType + Separator_Msg_Str + msg;
            return client.QueryAsync(text);
        }



        #region CustomMessageSender
        #region SendMessage
        public static void SendCustomMessage<TClient>(this TClient client, int informationType, byte[] body) where TClient : ISender
        {
            byte[] data = CreateCustomMessageData(informationType, body);
            client.SendMessage((int)MessageType.NORMAL_MESSAGE, data);
        }

        public static Task SendCustomMessageAsync<TClient>(this TClient client, int informationType, byte[] body) where TClient : ISender
        {
            byte[] data = CreateCustomMessageData(informationType, body);
            return client.SendMessageAsync((int)MessageType.NORMAL_MESSAGE_ASYNC, data);
        }

        public static void SendCustomMessage<TClient>(this TClient client, int informationType, string body) where TClient : ISender
        {
            string msg = CreateCustomMessageData(informationType, body);
            client.SendTextMessage((int)MessageType.NORMAL_MESSAGE, msg);
        }

        public static Task SendCustomMessageAsync<TClient>(this TClient client, int informationType, string body) where TClient : ISender
        {
            string msg = CreateCustomMessageData(informationType, body);
            return client.SendTextMessageAsync((int)MessageType.NORMAL_MESSAGE_ASYNC, msg);
        }

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


        public static byte[] QueryCustomMessage<TClient>(this TClient queryer, int informationType, byte[] body) where TClient : IQueryer
        {
            byte[] data = CreateCustomMessageData(informationType, body);
            ResponsedData res = queryer.QueryMessage((int)MessageType.QUERY, data);
            var packageInfo = res.RequestInfo as ZyLightFixedHeaderPackageInfo;
            return packageInfo.Body;
        }

        public static async Task<byte[]> QueryCustomMessageAsync<TClient>(this TClient queryer, int informationType, byte[] body) where TClient : IQueryer
        {
            byte[] data = CreateCustomMessageData(informationType, body);
            ResponsedData res = await queryer.QueryMessageAsync((int)MessageType.QUERY_ASYNC, data);
            ZyLightFixedHeaderPackageInfo packageInfo = res.RequestInfo as ZyLightFixedHeaderPackageInfo;
            return packageInfo.Body;
        }

        public static string QueryCustomTextMessage<TClient>(this TClient queryer, int messageType, string msg) where TClient : IQueryer
        {

            string cumstomMsg = CreateCustomMessageData(messageType, msg);
            ResponsedData res = queryer.QueryTextMessage((int)MessageType.QUERY, cumstomMsg);
            return Encoding.UTF8.GetString(res.Data);

        }

        public static async Task<string> QueryCustomTextMessageAsync<TClient>(this TClient queryer, int messageType, string msg) where TClient : IQueryer
        {

            string cumstomMsg = CreateCustomMessageData(messageType, msg);
            ResponsedData res = await queryer.QueryTextMessageAsync((int)MessageType.QUERY_ASYNC, cumstomMsg);
            return Encoding.UTF8.GetString(res.Data);

        }
        #endregion
        #endregion
    }
}
