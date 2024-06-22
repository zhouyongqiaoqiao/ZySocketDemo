using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TouchSocket.Sockets;
using ZySocketCore.Core.Enum;
using ZySocketCore.Utils;

namespace ZySocketCore.Server.User
{
    /// <summary>
    /// 登陆用户的基础信息。即可用于独立的应用服务器
    /// </summary>
    public class LoginDeviceData
    {
        public LoginDeviceData() { }
        public LoginDeviceData(string _loginID, IPHost _address, string serverID, ClientType _clientType, DateTime logon) {
            LoginID = _loginID;
            Address = _address;
            AppServerID = serverID;
            ClientType = _clientType;
            TimeLogon = logon;        
        }

        //
        // 摘要:
        //     在ESPlatform群集环境中，目标用户所连接的应用服务器ID。
        public string AppServerID { get; set; }
        public string LoginID { get; set; }
        public string UserID { get => IdUtil.GetUserId(LoginID); }

        //
        // 摘要:
        //     服务器看到的客户端的地址（通常是经过NAT之后的地址）。
        public IPHost Address { get; set; }

        //
        // 摘要:
        //     客户端的私有地址信息
        //public P2PAddress P2PAddress { get; set; }

        //
        // 摘要:
        //     上线时间。
        public DateTime TimeLogon { get; set; }
        //
        // 摘要:
        //     客户端类型
        public ClientType ClientType { get; set; }
        //
        // 摘要:
        //     用户的携带数据（应用程序可以使用该属性保存与当前用户相关的其它信息），该Tag会在ACMS和AS之间自动同步。该Tag指向的对象必须可序列化。
        public byte[] Tag { get; set; }
        //
        // 摘要:
        //     用户的携带数据（应用程序可以使用该属性保存与当前用户相关的其它信息），仅在本地使用。不会在ACMS和AS之间同步。
        //[NotSerialized]
        public object LocalTag { get; set; }

        //public void SetP2PAddress(P2PAddress addr);

        public override string ToString() { 
        
            return $"LoginID:{LoginID},Address:{Address},AppServerID:{AppServerID},ClientType:{ClientType},TimeLogon:{TimeLogon},Tag:{Tag},LocalTag:{LocalTag}";
        }
    }
}
