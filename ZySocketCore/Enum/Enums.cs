namespace ZySocketCore.Core.Enum
{
    internal enum MessageType
    {
        
        /// <summary>
        /// 登录请求应答
        /// </summary>
        REQ_OR_RESP_LOGON = 112,

        /// <summary>
        /// 重连
        /// </summary>
        ReConnected = 113,

        REQ_KICK_OUT = 108,

        REQ_OR_RESP_MY_IP = 101,

        REQ_OR_RESP_ONLINE_USERS = 110,

        REQ_OR_RESP_IF_USER_ONLINE = 111,

        GetMyOnlineDevice = 116,

        DeviceOnOfflineNotify = 117,

        QueryBeforeLogin = 118,

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
        /// <summary>
        /// 好友上下线通知
        /// </summary>
        FRIEND_ONLINE_AND_OFFILNE_NOTIFY = 501,

        REQ_OR_RESP_FRIENDS = 502,

        GROUP_MENBER_ONLINE_NOTIFY = 602,

        GROUP_MENBER_OFFLINE_NOTIFY = 601,

        REQ_OR_RESP_GROUP_MEMBERS = 603,

        REQ_OR_NOTIFY_BROADCAST = 604,

        REQ_OR_NOTIFY_BROADCAST_BLOB = 605,

        CONTACT_ONLINE = 702,

        CONTACT_OFFLINE = 701,

        /// <summary>
        /// query获取群组成员
        /// </summary>
        REQ_RESP_GetGroupMembers = 703,

        /// <summary>
        /// 联系人广播
        /// </summary>
        CONTACT_BROADCAST = 704,

        CONTACT_BROADCAST_BLOB = 705,

        /// <summary>
        /// 获取联系人
        /// </summary>
        REQ_RESP_CONTACT = 706,

        #region DynamicGroup
        GetGroupInfomation = 800,
        JoinGroup = 801,//请求加入组。对应协议为string,utf-8,groupID。回复协议为int,JoinGroupResult
        QuitGroup = 802,//请求退出组。对应协议为string,utf-8,groupID。没有回复。
        PullIntoGroup = 803,
        RemoveFromGroup = 804,
        SomeoneJoinGroupNotify = 805,//对组内通知新成员加入组。Server=>Client。对应协议为GroupNotifyContract
        SomeoneQuitGroupNotify = 806,//对组内通知某成员退出组。Server=>Client。对应协议为GroupNotifyContract
        PullIntoGroupNotify = 807,//自己被拉入某组的通知。
        RemoveFromGroupNotify = 808,//自己被从某组中移除。
        SomeonePullIntoGroupNotify = 809,//其他人被拉入某组。
        SomeoneRemoveFromGroupNotify = 810,//其他人被移出某组。
        GroupmateOfflineNotify = 811,//对组内通知某成员掉线（S->C）。对应协议为UserContract。
        GroupmateOnlineNotify = 812,//对组内通知某成员掉线（S->C）。对应协议为UserContract。
        InvitedJoinGroupNotify = 813,
        GetMyGroups = 814,
        SetGroupTag = 815,
        GroupTagChangedNotify = 816,
        InviteJoinGroup = 817,
        GetOnlineGroupmates = 818,
        /// <summary>
        /// 动态组广播消息
        /// </summary>
        Broadcast = 819,
        /// <summary>
        /// 获取我的动态组详情
        /// </summary>
        GetMyAllDetailGroups=820,
        #endregion


    }


    public enum ClientType : byte
    {
        Win,
        Web,
        Android,
        IOS,
        MAC,
        Linux,
        //All = 100
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

    public enum LogonResult
    {
        //
        // 摘要:
        //     登录成功
        Succeed = 0,
        //
        // 摘要:
        //     登录失败
        Failed = 1,
        //
        // 摘要:
        //     已在其它地方登陆
        HadLoggedOn = 2,
        //
        // 摘要:
        //     客户端与服务端的框架不匹配(比如版本不一致或授权帐号不一致)。
        VersionMismatched = 3,
        //
        // 摘要:
        //     消息头中的客户端类型与UserID的前缀不一致。
        ClientTypeMismatched = 4
    }

    //
    // 摘要:
    //     当通道繁忙时，发送数据所采取的操作。
    public enum ActionTypeOnChannelIsBusy
    {
        //
        // 摘要:
        //     继续发送（排队）。
        Continue = 0,
        //
        // 摘要:
        //     丢弃消息
        Discard = 1
    }

    /// <summary>
    /// 客户端连接断开的原因分类。
    /// </summary>
    public enum DisconnectedType {
        //
        // 摘要:
        //     网络连接中断。
        NetworkInterrupted = 0,
        //
        // 摘要:
        //     无效的消息。
        InvalidMessage = 1,
        //
        // 摘要:
        //     消息中的LoginID与当前连接的OwnerID不一致。
        MessageWithWrongLoginID = 2,
        //
        // 摘要:
        //     心跳超时。
        HeartBeatTimeout = 3,
        //
        // 摘要:
        //     被同名用户挤掉线。（发生于RelogonMode为ReplaceOld）
        BeingPushedOut = 4,
        //
        // 摘要:
        //     当已经有同名用户在线时，新的连接被忽略。（发生于RelogonMode为IgnoreNew）
        NewConnectionIgnored = 5,
        //
        // 摘要:
        //     等待发送以及正在发送的消息个数超过了MaxChannelCacheSize的设定值。
        ChannelCacheOverflow = 6,
        //
        // 摘要:
        //     未授权的客户端类型
        UnauthorizedClientType = 7,
        //
        // 摘要:
        //     已达到最大连接数限制
        MaxConnectionCountLimitted = 8
    }
}
