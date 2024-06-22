using System;
using System.Collections.Generic;
using System.Text;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Utils
{
    internal static class IdUtil
    {
        /// <summary>
        /// 构建 完整的用户ID
        /// </summary>
        /// <param name="clientType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string BuildFullUserId(ClientType? clientType, string userId)
        {
            if (clientType == null) return userId;
            return (byte)clientType + SystemSettings.Separator_UserID.ToString() + userId;
        }

        /// <summary>
        /// 获取客户端类型
        /// </summary>
        /// <param name="fullUserId">完整的用户ID</param>
        /// <returns></returns>
        public static ClientType GetClientType(string fullUserId)
        {            
            return (ClientType)byte.Parse(fullUserId.Split(SystemSettings.Separator_UserID)[0]);
        }

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <param name="fullUserId"></param>
        /// <returns></returns>
        public static string GetUserId(string fullUserId)
        {
            if (!IsFullUserId(fullUserId)) return fullUserId;
            return fullUserId.Split(SystemSettings.Separator_UserID)[1];
        }

        /// <summary>
        /// 判断是否为完整的用户ID
        /// </summary>
        /// <param name="fullUserId"></param>
        /// <returns></returns>
        public static bool IsFullUserId(string fullUserId)
        {
            if (string.IsNullOrEmpty(fullUserId)) return false;
            return fullUserId.Contains(SystemSettings.Separator_UserID);
        }
    }
}
