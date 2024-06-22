using System;
using System.Collections.Generic;
using System.Text;

namespace ZySocketCore.Server.Basic
{
    public interface IBasicController
    {
        /// <summary>
        /// 将目标用户从当前Server中踢出，并关闭对应的连接。 
        /// </summary>
        /// <param name="targetUserID"></param>
        void KickOut(string targetUserID);
    }
}
