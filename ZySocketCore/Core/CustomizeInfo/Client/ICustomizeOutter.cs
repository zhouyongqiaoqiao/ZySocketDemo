using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZySocketCore.Core.CustomizeInfo.Client
{
    public interface ICustomizeOutter
    {

        /// <summary>
        /// 向服务器提交请求信息，并返回服务器的应答信息。如果超时没有应答则将抛出Timeout异常。
        /// </summary>
        /// <param name="informationType">自定义请求信息的类型</param>
        /// <param name="info">请求信息</param>
        /// <returns></returns>
        byte[] Query(int informationType, byte[] info);

        /// <summary>
        /// 向服务器提交请求信息，并返回服务器的应答信息。如果超时没有应答则将抛出Timeout异常。
        /// </summary>
        /// <param name="informationType">自定义请求信息的类型</param>
        /// <param name="info">请求信息</param>
        /// <returns></returns>
        Task<byte[]> QueryAsync(int informationType, byte[] info);

        /// <summary>
        /// 向服务器发送信息。
        /// </summary>
        /// <param name="informationType">自定义信息类型</param>
        /// <param name="info">信息</param>
        Task SendAsync(int informationType, byte[] info);

        /// <summary>
        /// 向在线用户targetUserID发送信息。
        /// </summary>
        /// <param name="targetUserID">接收消息的目标用户ID</param>
        /// <param name="informationType">自定义信息类型</param>
        /// <param name="info">信息</param>
        Task SendAsync(string targetUserID, int informationType, byte[] info);

        /// <summary>
        /// 向在线用户或服务器发送信息，并等待其ACK。当前调用线程会一直阻塞，直到收到ACK；如果超时都没有收到ACK，则将抛出TimeoutException。
        /// </summary>
        /// <param name="targetUserID">接收消息的目标用户ID。如果为null，表示信息接收者为服务端。</param>
        /// <param name="informationType">自定义信息类型</param>
        /// <param name="info">信息</param>
        void SendCertainly(string targetUserID, int informationType, byte[] info);
    }
}
