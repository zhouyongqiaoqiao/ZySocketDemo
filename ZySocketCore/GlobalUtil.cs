using System;
using TouchSocket.Sockets;

namespace ZySocketCore
{
    public class GlobalUtil
    {
        public static int MaxLengthOfUserID { get; private set; } = 20;

        /// <summary>
        /// 设置UserID（包括GroupID）的最大长度(不能超过255)。必须在引擎初始化之前设置才有效。注意，客户端与服务端要统一设置。 (默认值20)
        /// </summary>
        /// <param name="maxLength"></param>
        public static void SetMaxLengthOfUserID(byte maxLength)
        {
            MaxLengthOfUserID = maxLength;
        }

        /// <summary>
        /// 设置消息的最大长度
        /// </summary>
        /// <param name="maxLen"></param>
        internal static void SetMaxLengthOfMessage(int maxLen)
        {
            
        }

        internal static void ResetUserId(ISocketClient zySocketClient, string newID)
        {
            Console.WriteLine("Reset UserId from {0} to {1}", zySocketClient.Id, newID);
            zySocketClient?.ResetId(newID);
        }

    
    }
}
