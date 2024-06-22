using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TouchSocket.Sockets;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Client.Basic
{
    public interface IBasicOutter
    {
        //
        // 摘要:
        //     当自己被同名用户挤掉线时，触发此事件。此时，客户端引擎已被Dispose。
        event Action BeingPushedOut;
        //
        // 摘要:
        //     当自己被服务端踢出掉线时，触发此事件。此时，客户端引擎已被Dispose。
        event Action BeingKickedOut;
        //
        // 摘要:
        //     当我的其它设备上线时，触发此事件。
        event Action<ClientType> MyDeviceOnline;
        //
        // 摘要:
        //     当我的其它设备下线时，触发此事件。
        event Action<ClientType> MyDeviceOffline;

        //
        // 摘要:
        //     获取自己的IPE。
        IPHost GetMyIPE();

        //
        // 摘要:
        //     获取自己的在线设备。
        List<ClientType> GetMyOnlineDevice();

        //
        // 摘要:
        //     命令服务端将目标用户踢出。如果目标用户不在当前AS上，则直接返回。
        //
        // 参数:
        //   targetUserID:
        //     要踢出的用户ID
        void KickOut(string targetUserID);
        //
        // 摘要:
        //     ping服务器。在应用层模拟ping，比普通的ICMP的ping大一些（如8-10ms）。
        //
        // 返回结果:
        //     ping耗时，单位毫秒
        int Ping();
        //
        // 摘要:
        //     向服务器发送心跳消息。被框架ESPlus.Application.Basic.Passive.HeartBeater使用。
        void SendHeartBeatMessage();
    }
}
