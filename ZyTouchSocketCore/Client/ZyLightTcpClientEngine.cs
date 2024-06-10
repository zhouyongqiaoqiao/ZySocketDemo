using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZyTouchSocketCore;
using ZyTouchSocketCore.Core.Enum;

namespace ZyLightTouchSocketCore.Client
{
    public class ZyLightTcpClientEngine:ZyLightClientEngine , ITcpClientOutter
    {
        public ZyLightTcpClientEngine():base(ContractFormatStyle.Stream) 
        {
            
        }

        #region ITcpClientOutter
        public void SendMessageToServer(int informationType, byte[] msg)
        {
            this.SendCustomMessage(informationType, msg);
        }

        public Task SendMessageToServerAsync(int informationType, byte[] msg)
        {
            return this.SendCustomMessageAsync(informationType, msg);
        }

        public byte[] QueryMessageFromServer(int informationType, byte[] msg)
        {
            return this.QueryMessage(informationType, msg);
        }

        public Task<byte[]> QueryMessageFromServerAsync(int informationType, byte[] msg)
        {
            return this.QueryMessageAsync(informationType, msg);
        }
        #endregion
    }

    public interface ITcpClientOutter
    {
        /// <summary>
        /// 发送消息到服务器
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        void SendMessageToServer(int informationType, byte[] msg);

        /// <summary>
        /// 异步发送消息到服务器
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendMessageToServerAsync(int informationType, byte[] msg);

        /// <summary>
        /// 同步请求消息
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        byte[] QueryMessageFromServer(int informationType, byte[] msg);

        /// <summary>
        /// 异步请求消息
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task<byte[]> QueryMessageFromServerAsync(int informationType, byte[] msg);
    }
}
