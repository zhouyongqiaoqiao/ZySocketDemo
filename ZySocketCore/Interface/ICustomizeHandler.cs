using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Core;
using ZySocketCore.Core;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Interface
{
    /// <summary>
    /// 处理接收到的自定义信息。
    /// </summary>
    public interface ICustomizeHandler
    {
        /// <summary>
        /// 处理接收到的自定义信息。
        /// </summary>
        /// <param name="sourceUserID">发送该信息的用户ID。如果为null，表示信息发送者为服务端。</param>
        /// <param name="clientType">客户端设备类型</param>
        /// <param name="informationType">自定义信息类型</param>
        /// <param name="info">信息</param>
        void HandleInformation(string sourceUserID, ClientType clientType, int informationType, byte[] info);

        /// <summary>
        /// 处理接收到的请求并返回应答信息。
        /// </summary>
        /// <param name="sourceUserID">发送该请求信息的用户ID。如果为null，表示信息发送者为服务端。</param>     
        /// <param name="clientType">客户端设备类型</param>
        /// <param name="informationType">自定义请求信息的类型</param>  
        /// <param name="info">请求信息</param>
        /// <returns>应答信息</returns>
        byte[] HandleQuery(string sourceUserID, ClientType clientType, int informationType, byte[] info);
    }

    /// <summary>
    /// 处理接收到的Blob和Tag信息。
    /// </summary>
    internal interface IBlobAndTagMessageHandler
    { 
        /// <summary>
        /// 处理接收到的Blob和Tag信息。
        /// </summary>
        /// <param name="sourceUserID">发送消息的用户ID。如果为null，表示信息发送者为服务端。</param>
        /// <param name="clientType"></param>
        /// <param name="informationType"></param>
        /// <param name="blob"></param>
        /// <param name="tag"></param>
        void HandleBlobAndTagMessage(string sourceUserID, ClientType clientType,int informationType, byte[] blob,string tag);
    }

}
