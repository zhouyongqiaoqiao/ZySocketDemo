using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Sockets;

namespace ZySocketCore.Interface
{
    public interface IBasicHandler
    {
        /// <summary>
        /// 该回调用于处理客户端通过ZyEngine的QueryBeforeLogin方法提交的请求。
        /// </summary>
        /// <param name="clientAddr">客户端的IP地址</param>
        /// <param name="queryType">请求的类型</param>
        /// <param name="query">请求内容</param>
        /// <returns></returns>
        string HandleQueryBeforeLogin(IPHost clientAddr, int queryType, string query);

        ///   <summary>
        ///  客户端登陆验证。
        ///   </summary>         
        ///   <param name="userID"> 登陆用户账号 </param>
        ///   <param name="systemToken"> 系统标志。用于验证客户端是否与服务端属于同一系统。 </param>
        ///   <param name="password"> 登陆密码 </param>
        ///   <param name="failureCause"> 如果登录失败，该out参数指明失败的原因 </param>
        ///   <returns> 如果密码和系统标志都正确则返回true；否则返回false。 </returns>
        bool VerifyUser(string systemToken, string userID, string password, out string failureCause);
    }

    public class EmptyBasicHandler : IBasicHandler
    {
        public string HandleQueryBeforeLogin(IPHost clientAddr, int queryType, string query)
        {
            return string.Empty;
        }

        public bool VerifyUser(string systemToken, string userID, string password, out string failureCause)
        {
            failureCause = string.Empty;
            return true;
        }
    }
}
