using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;

namespace ZySocketServer
{
    internal class CustomizeHandler : ICustomizeHandler
    {
        public void HandleInformation(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            Console.WriteLine($"CustomizeHandler: HandleInformation, sourceUserID:{sourceUserID}, clientType:{clientType}, informationType:{informationType}, info:{Encoding.UTF8.GetString(info)}");
        }

        public byte[] HandleQuery(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            Console.WriteLine("CustomizeHandler: HandleQuery, sourceUserID:" + sourceUserID + ", clientType:" + clientType + ", informationType:" + informationType + ", info:" + Encoding.UTF8.GetString(info));
            return Encoding.UTF8.GetBytes("from server res");
        }
    }
}
