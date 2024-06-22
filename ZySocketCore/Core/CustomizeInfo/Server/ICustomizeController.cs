using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Core.CustomizeInfo.Server
{
    public interface ICustomizeController
    {
        /// <summary>
        /// 向ID为userID的在线用户发送信息。如果用户不在线，则直接返回。
        /// </summary>
        /// <param name="userID">接收消息的用户ID</param>
        /// <param name="informationType">自定义信息类型</param>
        /// <param name="info">信息</param>
        /// <param name="clientType">要发给哪个终端设备。如果传入All，表示发送给所有在线终端设备</param>
        Task SendAsync(string userID, int informationType, byte[] info, ClientType? clientType = null);

        /// <summary>
        /// 向当前AS上的在线用户发送信息，并等待其ACK。当前调用线程会一直阻塞，直到收到ACK；如果超时都没有收到ACK，则将抛出Timeout异常。
        /// </summary>
        /// <param name="userID">接收消息的用户ID</param>
        /// <param name="informationType">自定义信息类型</param>
        /// <param name="info">信息</param>
        /// <param name="clientType">要发给哪个终端设备</param>
        Task SendCertainlyToClient(string userID, int informationType, byte[] info, ClientType clientType);

        /// <summary>
        /// 询问当前AS的在线用户，并返回应答信息。如果超时没有应答则将抛出Timeout异常。如果客户端在处理请求时出现未捕获的异常，则该调用会抛出HandingException。
        /// </summary>
        /// <param name="userID">接收并处理服务器询问的目标用户ID</param>
        /// <param name="informationType">自定义请求信息的类型</param>
        /// <param name="info">请求信息</param>
        /// <param name="clientType">要发给哪个终端设备</param>
        /// <returns>客户端给出的应答信息</returns>
       Task<byte[]> QueryAsyncToClient(string userID, int informationType, byte[] info, ClientType clientType);
    }
}
