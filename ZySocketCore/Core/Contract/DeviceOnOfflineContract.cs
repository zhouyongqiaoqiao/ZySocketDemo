using System;
using System.Collections.Generic;
using System.Text;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Core.Contract
{
    internal class DeviceOnOfflineContract
    {
        public ClientType ClientType { get; set; }
        public bool IsOnline { get; set; }
    }
}
