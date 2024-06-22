using System;
using System.Collections.Generic;
using System.Text;
using ZySocketCore.Core.Enum;
using ZySocketCore.Core;
using TouchSocket.Sockets;
using ZySocketCore.Core.CustomizeInfo.Client;
using ZySocketCore.Core.QueryInfo;
using ZySocketCore.Client.Basic;
using ZySocketCore.Core.Contacts.Client;
using ZySocketCore.Core.DynamicGroup.Client;

namespace ZySocketCore.Interface
{
    //
    // 摘要:
    //     迅捷的客户端引擎。基于TCP、使用二进制协议。
    public interface IZyClientEngine 
    {

        //
        // 摘要:
        //     系统标志。引擎在初始化时会提交给服务器验证客户端是否是正确的系统。（也可以被借用于登陆增强验证）。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        string SystemToken { get; set; }
        //
        // 摘要:
        //     P2P服务器的地址。默认值为null。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。） 如果服务端引擎开启了UseAsP2PServer，则可以不设置该属性。如果同时又设置该属性为非null值，则以设置值为准。
        //AgileIPE P2PServerAddress { get; set; }
        //
        // 摘要:
        //     Sock5代理服务器信息。如果不需要代理，则设置为null。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        //Sock5ProxyInfo Sock5ProxyInfo { get; set; }
        //
        // 摘要:
        //     当通过ICustomizeOutter进行同步调用时，等待回复的最长时间。如果小于等于0，表示一直阻塞调用线程直到等到回复为止。默认值为30秒。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        int WaitResponseTimeoutInSecs { get; set; }
        //
        // 摘要:
        //     每隔多长时间（秒）发送一次心跳消息。如果小于等于0，表示不发送定时心跳。默认值为10秒。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        int HeartBeatSpanInSecs { get; set; }

        //
        // 摘要:
        //     该接口用于向服务器或其它在线用户发送自定义信息。（只有Initialize方法调用成功之后，该属性才可正常使用）
        ICustomizeOutter CustomizeOutter { get; }

        //
        // 摘要:
        //     与服务器之间的通道是否处于繁忙状态？
        bool ChannelIsBusy { get; }
        //
        // 摘要:
        //     当前是否处于连接状态。如果为true，则表示不仅TCP连接正常，而且是登录成功的状态。
        bool Connected { get; }
        //
        // 摘要:
        //     当前引擎所连接的服务器的地址。
        IPHost ServerAddress { get; }
        //
        // 摘要:
        //     引擎是否出于关闭状态？（关闭状态：意味着与服务器的连接已经断开，并且所有资源已经释放，也不再进行自动重连，就像未初始化状态一样）
        bool IsClosed { get; }
        //
        // 摘要:
        //     客户端引擎高级控制选项。如果要设置AdvancedOptions的某些属性，必须在调用Initialize方法之前设置才有效。
        //AdvancedOptions Advanced { get; }

        //
        // 摘要:
        //     当前客户端设备类型。
        ClientType CurrentClientType { get; }
        //
        // 摘要:
        //     当前登录的用户UserID。（只有Initialize方法调用成功之后，该属性才可正常使用）
        string CurrentUserID { get; }
        //
        // 摘要:
        //     该接口用于向服务器或其它在线用户发送文件。（只有Initialize方法调用成功之后，该属性才可正常使用）
        //IFileOutter FileOutter { get; }
        //IP2PController P2PController { get; }

        //
        // 摘要:
        //     该接口用于客户端发送与联系人操作相关的信息和广播。（只有Initialize方法调用成功之后，该属性才可正常使用）
        IContactsOutter ContactsOutter { get; }

        //
        // 摘要:
        //     该接口用于进行动态组的相关操作，或监听动态组的相关事件。（只有Initialize方法调用成功之后，该属性才可正常使用）
        IDynamicGroupOutter DynamicGroupOutter { get; }

        //
        // 摘要:
        //     掉线后，是否自动重连。（如果要set该属性，则必须在调用Initialize方法之前设置才有效。）
        bool AutoReconnect { get; set; }
        //
        // 摘要:
        //     该接口用于向服务器发送基本的请求，如获取自己的IP、获取所有在线用户列表等等。（只有Initialize方法调用成功之后，该属性才可正常使用）
        IBasicOutter BasicOutter { get; }

        //
        // 摘要:
        //     自动重连开始时，触发此事件。如果重连成功则将重新登录，并触发RelogonCompleted事件。
        event Action ConnectionRebuildStart;
        //
        // 摘要:
        //     当客户端与服务器的TCP连接断开时，将触发此事件。
        event Action ConnectionInterrupted;
        //
        // 摘要:
        //     当断线重连成功时，会自动登录服务器验证用户账号密码，并触发此事件。如果验证失败，则与服务器的连接将会断开，且后续不会再自动重连。事件参数表明了登录验证的结果。
        event Action<LogonResponse> RelogonCompleted;
        //
        // 摘要:
        //     当（当前用户在其它客户端设备上发送了消息）时，触发此事件。 事件参数：clientType - destUserID - informationType
        //     - message - tag 。 如果原消息的接收者为服务器，则destUserID为null。
        //event Action<ClientType, string, int, byte[], string> EchoMessageReceived;
        //
        // 摘要:
        //     当接收到来自服务器或其它用户的消息时，触发此事件。 事件参数：sourceUserID - ClientType - informationType -
        //     message - tag 。 如果消息来自服务器，则sourceUserID为null。
        event Action<string, ClientType, int, byte[], string> MessageReceived;

        //
        // 摘要:
        //     关闭并释放客户端通信引擎。
        void Close();
        //
        // 摘要:
        //     主动关闭与ESFramework服务器的连接,将引发自动重连。
        //
        // 参数:
        //   reconnectNow:
        //     是否立即重连？
        void CloseConnection(bool reconnectNow);
        //
        // 摘要:
        //     完成客户端引擎的初始化，与服务器建立TCP连接，连接成功后立即验证用户密码。如果连接失败，则抛出异常。
        //
        // 参数:
        //   userID:
        //     当前登录的用户ID，由数字和字母组成，最大长度为10
        //
        //   logonPassword:
        //     用户登陆密码。
        //
        //   serverIP:
        //     服务器的IP地址。
        //
        //   serverPort:
        //     服务器的端口。
        //
        //   customizeHandler:
        //     自定义处理器，用于处理服务器或其它用户发送过来的消息
        LogonResponse Initialize(string userID, string logonPassword, string serverIP, int serverPort, ICustomizeHandler customizeHandler);


        //
        // 摘要:
        //     在登录之前，向服务器发送请求。将被服务端的IBasicHandler的HandleQueryBeforeLogin方法处理。
        //
        // 参数:
        //   serverIP:
        //     ESFramework服务器IP
        //
        //   serverPort:
        //     ESFramework服务器端口
        //
        //   queryType:
        //     请求类型
        //
        //   query:
        //     请求内容
        //
        // 返回结果:
        //     回复
        string QueryBeforeLogin(string serverIP, int serverPort, int queryType, string query);
        //
        // 摘要:
        //     向服务器或其它在线用户异步发送消息（当前调用线程立即返回）。如果其它用户不在线，消息将被丢弃。
        //
        // 参数:
        //   targetUserID:
        //     接收者的UserID，如果为服务器，则传入null
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
        //   toOtherClientOfMine:
        //     是否发送给我的其它在线设备
        //void SendMessage(string targetUserID, int informationType, byte[] message, string tag, bool toOtherClientOfMine = false); 

        /// <summary>
        /// 向服务器或其它在线用户异步发送消息（当前调用线程立即返回）。如果其它用户不在线，消息将被丢弃。
        /// </summary>
        /// <param name="targetUserID">接收者的UserID，如果为服务器，则传入null</param>
        /// <param name="informationType">消息类型</param>
        /// <param name="message">消息内容</param>
        /// <param name="tag">附加内容</param>
        /// <param name="destClientType">对方类型</param>
        void SendMessage(string targetUserID, int informationType, byte[] message, string tag, ClientType? destClientType = null);        
     
        // 摘要:
        //     向在线用户的指定设备发送消息。如果目标用户设备不在线，消息将被丢弃。
        //
        // 参数:
        //   targetUserID:
        //     接收者的UserID
        //
        //   targetType:
        //     接收者的设备类型
        //
        //   informationType:
        //     消息类型
        //
        //   message:
        //     消息内容
        //
        //   tag:
        //     附加内容
        void SendMessageToSpecialDevice(string targetUserID, ClientType targetType, int informationType, byte[] message, string tag);
    }
}
