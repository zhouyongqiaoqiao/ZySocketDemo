using System;
using System.Collections.Generic;
using System.Net;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Server.User
{
    /// <summary>
    /// 在支持多端的场景中，将与连接绑定的ID称为LoginID，而客户端用户真正的ID称为ClientID。对于非多端场景，ClientID就是LoginID。
    /// </summary>
    public interface IUserManager
    {
        //
        // 摘要:
        //     在线用户设备显示器。
       // ILoginDeviceDisplayer LoginDeviceDisplayer { get; set; }

        //
        // 摘要:
        //     当前在线用户的数量。
        int UserCount { get; }
        //
        // 摘要:
        //     不检查WebSocket客户端的心跳。默认值：true。（避免：当web页面切到后台时，浏览器可能会休眠心跳定时器从而导致心跳超时掉线。）
        bool DontCheckHeartbeat4Websocket { get; set; }

        //
        // 摘要:
        //     当某用户的第一个设备登录成功时，触发此事件。
        event Action<string> UserConnected;
        //
        // 摘要:
        //     当某用户的最后一个设备下线时，触发此事件。
        event Action<string> UserDisconnected;
        //
        // 摘要:
        //     当在线用户数发生变化时，触发此事件。
        event Action<int> UserCountChanged;
        //
        // 摘要:
        //     当从另外一个新连接上收到一个同名LoginID用户登录成功的消息时，触发此事件。 注意，只有在该事件处理完毕后，才会真正关闭旧的连接并使用新的地址取代旧的地址。可以在该事件的处理函数中，将相关情况通知给旧连接的客户端。
        event Action<LoginDeviceData> ClientDeviceBeingPushedOut;
        //
        // 摘要:
        //     当客户端设备登录成功时，触发此事件。不要远程预定该事件。
        event Action<LoginDeviceData> ClientDeviceConnected;
        //
        // 摘要:
        //     客户端设备断开下线时，触发此事件。不要远程预定该事件。
        event Action<LoginDeviceData, DisconnectedType> ClientDeviceDisconnected;
        //
        // 摘要:
        //     当某个设备的P2PAddress地址被修改时，将触发此事件。参数为 LoginID - P2PAddress
        //event Action<string, P2PAddress> P2PAddressChanged;

        //
        // 摘要:
        //     当某个用户的携带数据被修改时，将触发此事件。参数为 UserID - tag
        event Action<string, byte[]> UserTagChanged;

        //
        // 摘要:
        //     获取所有登录设备。
        List<LoginDeviceData> GetAllLoginDevice();
        //
        // 摘要:
        //     获取所有在线用户信息。
        List<UserData> GetAllUserData();
        //
        // 摘要:
        //     【框架内部使用】根据loginID获取其对应的LoginDeviceData。如果不在线，则返回null。
        LoginDeviceData GetLoginDeviceDataByLoginID(string loginID);
        //
        // 摘要:
        //     获取在线用户的ID列表。
        List<string> GetOnlineUserList();
        //
        // 摘要:
        //     【框架内部使用】根据loginID获取其对应的地址。如果不在线，则返回null。
        EndPoint GetUserAddressByLoginID(string loginID);
        //
        // 摘要:
        //     获取目标在线用户的基础信息。
        //
        // 参数:
        //   userID:
        //     目标用户的ID
        //
        // 返回结果:
        //     如果目标用户不在线，则返回null
        UserData GetUserData(string userID);

        //
        // 摘要:
        //     目标用户是否在线。
        bool IsUserOnLine(string userID);
        //
        // 摘要:
        //     【框架内部使用】设置loginID对应的P2P地址。
        //void SetP2PAddress(string loginID, P2PAddress addr);
    }
}
