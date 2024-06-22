using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZySocketCore;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Client
{
    public class ZyTcpClientEngine:ZyClientEngine 
    {
        public ZyTcpClientEngine():base(ContractFormatStyle.Stream) 
        {
            
        }

 
    }

    public interface ITcpClientOutter
    {
        /// <summary>
        /// 发送消息到服务器
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        void SendMessage(string userID, int informationType, byte[] msg);

        /// <summary>
        /// 异步发送消息到服务器
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendMessageAsync(string userID, int informationType, byte[] msg);

        /// <summary>
        /// 同步请求消息
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        byte[] QueryMessage(string userID, int informationType, byte[] msg);

        /// <summary>
        /// 异步请求消息
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task<byte[]> QueryMessageAsync(string userID, int informationType, byte[] msg);
    }
}
