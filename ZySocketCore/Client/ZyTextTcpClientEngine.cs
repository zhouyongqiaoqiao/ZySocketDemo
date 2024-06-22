using System.Text;
using System.Threading.Tasks;
using ZySocketCore;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Client
{
    public class ZyTextTcpClientEngine : ZyClientEngine
    {
        public ZyTextTcpClientEngine():base(ContractFormatStyle.Text)
        {
            
        }

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
