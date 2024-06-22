using System;
using System.Collections.Generic;
using System.Text;
using ZySocketCore.Core.Contacts.Server;
using ZySocketCore.Core.CustomizeInfo.Server;
using ZySocketCore.Core.DynamicGroup.Server;
using ZySocketCore.Core.Enum;
using ZySocketCore.Server.Basic;
using ZySocketCore.Server.User;

namespace ZySocketCore.Interface
{

    public interface IZyServerEngine
    {
        /// <summary>
        /// 引擎收到消息事件 string sourceUserID, ClientType clientType,int informationType, byte[] blob,string tag
        /// </summary>
        event Action<string, ClientType, int, byte[], string> MessageReceived;
        //IServiceTypeNameMatcher ServiceTypeNameMatcher { get; }

        /// <summary>
        /// 【可选】如果需要好友关系支持，则必须设置该属性。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        /// </summary>
        //IFriendsManager FriendsManager { set; }

        /// <summary>
        /// 【可选】如果需要组关系支持，则必须设置该属性。可以使用内置的动态组DynamicGroupManager。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        /// </summary>
        IDynamicGroupController DynamicGroupController { get; }

        /// <summary>
        /// 通过哪个IP地址提供服务。如果设为null，则表示绑定本地所有IP。默认值为null。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        /// </summary>
        string IPAddressBinding { get; set; }

        /// <summary>
        /// 心跳超时间隔（秒）。即服务端多久没有收到客户端的心跳消息，就视客户端为超时掉线。默认值为30秒。如果设置小于等于0，则表示不做心跳检查。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        /// </summary>
        int HeartbeatTimeoutInSecs { get; set; }

        /// <summary>
        /// 当通过ICustomizeController.QueryClient方法进行同步调用时，等待回复的最长时间。如果小于等于0，表示一直阻塞调用线程直到等到回复为止。默认值为30秒。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        /// </summary>
        int WaitResponseTimeoutInSecs { get; set; }

        /// <summary>
        /// 是否自动回复客户端发过来的心跳消息（将心跳消息原封不动地发回客户端）。默认值为true。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        /// </summary>
        bool AutoRespondHeartBeat { get; set; }
        /// <summary>
        /// 是否同时作为P2P服务器运行。默认值为true（P2P服务器将使用UDP端口，端口号为当前引擎监听的端口号加1）。（如果要set该属性，则必须要在调用Initialize方法之前设置才有效。）
        /// </summary>
        //bool UseAsP2PServer { get; set; }

        /// <summary>
        /// 当前在线连接的数量。
        /// </summary>
        int ConnectionCount { get; }

        /// <summary>
        /// 服务器允许最大的同时连接数。 (-1表示不限制)
        /// </summary>
        int MaxConnectionCount { get; }

        /// <summary>
        /// 当前监听的端口。
        /// </summary>
        int Port { get; }

        /// <summary>
        /// 服务端实例的唯一编号。
        /// </summary>
        string ServerName { get; }

        /// <summary>
        /// 【可选】如果需要支持联系人，则必须设置该属性。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        /// </summary>
        IContactsManager ContactsManager { set; }

        /// <summary>
        /// 通过此接口，可以获取用户的相关信息以及用户上/下线的事件通知。（只有在ServerEngine初始化成功后，才能正常使用。）
        /// </summary>
        IUserManager UserManager { get; }

        /// <summary>
        /// 通过此接口，可以获取用户联系人的相关信息以及事件通知。（只有在ServerEngine初始化成功后，才能正常使用。）
        /// </summary>
        IContactsController ContactsController { get; }

        /// <summary>
        /// 平台用户管理器（用于ESPlatform）。可以获取群集中所有在线用户信息。（只有在ServerEngine初始化成功后，才能正常使用。）
        /// </summary>
        //IPlatformUserManager PlatformUserManager { get; }

        /// <summary>
        /// 通过此接口，服务端可以将目标用户从服务器中踢出，并关闭其对应的tcp连接。（只有在ServerEngine初始化成功后，才能正常使用。）
        /// </summary>
        IBasicController BasicController { get; }

        /// <summary>
        /// 通过此接口，服务端可以主动向在线用户发送/投递自定义信息。（只有在RapidServerEngine初始化成功后，才能正常使用。）
        /// </summary>
        ICustomizeController CustomizeController { get; }

        /// <summary>
        /// 通过此接口，服务端可以主动向在线用户发送文件。（只有在RapidServerEngine初始化成功后，才能正常使用。）
        /// </summary>
        //IFileController FileController { get; }

         /// <summary>
        /// 完成服务端引擎的初始化，并启动服务端引擎。
        /// </summary>
        /// <param name="port">用于提供tcp通信服务的端口</param>
        /// <param name="customizeHandler">服务器通过此接口来处理客户端提交给服务端自定义信息。</param>
        void Initialize(int port, ICustomizeHandler customizeHandler);

        /// <summary>
        /// 完成服务端引擎的初始化，并启动服务端引擎。
        /// </summary>
        /// <param name="port">用于提供tcp通信服务的端口</param>
        /// <param name="customizeHandler">服务器通过此接口来处理客户端提交给服务端自定义信息。</param>
        /// <param name="basicHandler">用于验证客户端登陆密码。如果不需要验证，则直接传入null。</param>    
        void Initialize(int port, ICustomizeHandler customizeHandler, IBasicHandler basicHandler);

        //
        // 摘要:
        //     向在线用户发送消息。如果目标用户不在线，消息将被丢弃。
        //
        // 参数:
        //   targetUserID:
        //     接收者的UserID
        //
        //   informationType:
        //     消息类型
        //
        //   message:
        //     消息内容
        //
        //   tag:
        //     附加内容
        //
        //   clientType:
        //     要发送给哪个客户端设备，如果为null，则表示发给所有设备
        void SendMessage(string targetUserID, int informationType, byte[] message, string tag, ClientType? clientType = null);

        /// <summary>
        /// 关闭服务端引擎。
        /// </summary>
        void Close();
    }
}
