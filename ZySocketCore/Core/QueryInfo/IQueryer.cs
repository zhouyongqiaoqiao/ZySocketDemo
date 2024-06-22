using System.Threading.Tasks;
using TouchSocket.Sockets;

namespace ZySocketCore.Core.QueryInfo
{
    public interface IQueryer
    {
        /// <summary>
        /// 等待响应超时时间，单位秒（默认5秒）
        /// </summary>
        int WaitResponseTimeoutInSecs { set; }

        /// <summary>
        /// 同步请求回复
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        ResponsedData Query(byte[] buffer, int offset, int length);

        /// <summary>
        /// 同步请求回复
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        ResponsedData Query(byte[] buffer);

        /// <summary>
        /// 同步请求回复
        /// </summary>
        /// <param name="message">文本消息</param>
        /// <returns></returns>
        ResponsedData Query(string message);

        /// <summary>
        /// 异步请求回复
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        Task<ResponsedData> QueryAsync(byte[] buffer, int offset, int length);

        /// <summary>
        /// 异步请求回复
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        Task<ResponsedData> QueryAsync(byte[] buffer);

        /// <summary>
        /// 异步请求回复
        /// </summary>
        /// <param name="message">文本消息</param>
        /// <returns></returns>
        Task<ResponsedData> QueryAsync(string message);
    }
}
