using TouchSocket.Sockets;
using ZySocketCore.Interface;

namespace ZySocketServer
{
    internal class BasicHandler : IBasicHandler
    {
        public string HandleQueryBeforeLogin(IPHost clientAddr, int queryType, string query)
        {
            switch (queryType)
            {
                case 1:
                    return "欢迎来到TouchSocket服务器";
                case 2:
                    return "TouchSocket服务器支持以下功能：\n1.登录验证\n2.消息推送\n3.文件传输\n4.在线用户查询";
                default:
                    return "TouchSocket服务器暂不支持此功能";
            }
        }

        public bool VerifyUser(string systemToken, string userID, string password, out string failureCause)
        {
            Console.WriteLine($"systemToken:{systemToken} userID:{userID} password:{password}");
            if (password == "123456")
            {
                failureCause = "密码错误";
                return false;
            }
            failureCause = "";
            return true;
        }
    }
}
