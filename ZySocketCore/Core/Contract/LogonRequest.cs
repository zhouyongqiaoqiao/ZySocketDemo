using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Core.Contract
{
    internal class LogonRequest
    {
        public LogonRequest() { }
        public LogonRequest(string systemToken, string password, string authorizedUser)
        {
            this.SystemToken = systemToken;
            this.Password = password;
            this.AuthorizedUser = authorizedUser;
        }

        /// <summary>
        /// 系统Token
        /// </summary>
        public string SystemToken { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 授权用户
        /// </summary>
        public string AuthorizedUser { get; set; }
    }
}
