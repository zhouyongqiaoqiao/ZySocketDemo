using System.Text;
using System.Threading.Tasks;
using ZyTouchSocketCore;
using ZyTouchSocketCore.Core.Enum;

namespace ZyLightTouchSocketCore.Client
{
    public class ZyLightTextTcpClientEngine : ZyLightClientEngine, ITextTcpClientOutter
    {
        public ZyLightTextTcpClientEngine():base(ContractFormatStyle.Text)
        {
            
        }

        #region ITextTcpClientOutter
        public void SendMessageToServer(int informationType, string msg)
        {
            this.SendCustomMessage(informationType, msg);
        }

        public Task SendMessageToServerAsync(int informationType, string msg)
        {
            return this.SendCustomMessageAsync(informationType, msg);
        }

        public string QueryMessageFromServer(int informationType, string msg)
        {
            return this.QueryMessage(informationType, msg);
        }

        public Task<string> QueryMessageFromServerAsync(int informationType, string msg)
        {
            return this.QueryMessageAsync(informationType, msg);
        }

        #endregion
    }

    public interface ITextTcpClientOutter
    {
        /// <summary>
        /// 发送消息到服务器
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        void SendMessageToServer(int informationType, string msg);

        /// <summary>
        /// 异步发送消息到服务器
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendMessageToServerAsync(int informationType, string msg);

        /// <summary>
        /// 同步请求消息
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        string QueryMessageFromServer(int informationType, string msg);

        /// <summary>
        /// 异步请求消息
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task<string> QueryMessageFromServerAsync(int informationType, string msg);
    }
}
