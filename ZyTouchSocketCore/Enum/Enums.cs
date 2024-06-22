using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZySocketCore.Core.Enum
{
    internal enum MessageType
    {
        /// <summary>
        /// 登录请求应答
        /// </summary>
        REQ_OR_RESP_LOGON = 112,

        REQ_KICK_OUT = 108,

        REQ_OR_RESP_MY_IP = 101,

        REQ_OR_RESP_ONLINE_USERS = 110,

        REQ_OR_RESP_IF_USER_ONLINE = 111,

        REQ_PING = 102,

        RESP_PING_ACK = 103,

        REQ_HEART_BEAT = 104,

        BE_KICKED_OUT_NOTIFY = 109,
        /**
         * 被挤下线
         */
        BE_FORCED_OUT_NOTIFY = 105,
        /**
         * 使用send方法发送的普通消息
         * 201表示采用异步发送模型，
         */
        NORMAL_MESSAGE_ASYNC = 201,

        /**
         * 使用send方法发送的普通消息
         * 202表示采用同步发送模型
         */
        NORMAL_MESSAGE = 202,

        /**
         * 使用query方法发送的消息
         */
        QUERY = 203,

        QUERY_ASYNC = 204,

        QUERY_RESPONSE = 213,

        BLOB = 207,

        BLOB_TAG = 208,

        ACK_REQ = 205,

        ACK_RESP = 206,

        FILE_TRANSFER = 301,

        FILE_REJECT_OR_ACCEPT_REQ = 302,

        FILE_PACKAGE = 303,

        FILE_CANCEL_SEND = 304,

        FILE_CANCEL_RECEIVE = 305,

        FRIEND_ONLINE_NOTIFY = 501,

        REQ_OR_RESP_FRIENDS = 502,

        GROUP_MENBER_ONLINE_NOTIFY = 602,

        GROUP_MENBER_OFFLINE_NOTIFY = 601,

        REQ_OR_RESP_GROUP_MEMBERS = 603,

        REQ_OR_NOTIFY_BROADCAST = 604,

        REQ_OR_NOTIFY_BROADCAST_BLOB = 605,

        CONTACT_ONLINE = 702,

        CONTACT_OFFLINE = 701,

        REQ_RESP_GetGroupMembers = 703,

        CONTACT_BROADCAST = 704,

        CONTACT_BROADCAST_BLOB = 705,

        REQ_RESP_CONTACT = 706

    }


    public enum ClinetType : byte
    {
        Win,
        Web,
        Android,
        IOS,
        MAC,
        Linux
    }

    //
    // 摘要:
    //     Socket server running mode
    public enum SocketMode
    {
        //
        // 摘要:
        //     Tcp mode
        Tcp,
        //
        // 摘要:
        //     Udp mode
        Udp
    }

    /// <summary>
    /// 协议的格式。
    /// </summary>
    public enum ContractFormatStyle
    {
        /// <summary>
        /// 流协议或称二进制协议。
        /// </summary>
        Stream = 0,
        /// <summary>
        /// 文本协议，如基于xml的。
        /// </summary>
        Text
    }
}
