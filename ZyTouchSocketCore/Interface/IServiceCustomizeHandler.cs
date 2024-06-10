using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Server;

namespace ZyLightTouchSocketCore.Interface
{
    /// <summary>
    /// 服务端自定义消息处理器
    /// </summary>
    public interface IServiceCustomizeHandler
    {

        /**
         * 处理来自其他用户的信息（包括大数据块信息）。
         * @param socketClient 发出信息的用户。如果为null，表示信息来自服务端。
         * @param informationType 自定义信息类型
         * @param info 信息
         */
        void HandleInformation(IServerSender socketClient, int informationType, byte[] info);

        /**
         * 处理接收到的请求并返回应答信息。
         * @param socketClient 发送请求信息的用户ID。如果为null，表示信息来自服务端。
         * @param informationType 自定义请求信息的类型
         * @param info 请求信息
         * @return 应答信息
         */
        byte[] HandleQuery(IServerSender socketClient, int informationType, byte[] info);
    }

    public interface ICustomizeHandler
    {

        /**
         * 处理来自其他用户的信息（包括大数据块信息）。
         * @param informationType 自定义信息类型
         * @param info 信息
         */
        void HandleInformation(int informationType, byte[] info);

        /**
         * 处理接收到的请求并返回应答信息。
         * @param informationType 自定义请求信息的类型
         * @param info 请求信息
         * @return 应答信息
         */
        byte[] HandleQuery(int informationType, byte[] info);
    }
}
