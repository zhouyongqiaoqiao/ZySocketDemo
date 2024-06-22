using System;
using System.Collections.Generic;
using System.Text;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Server.UserManagement
{
    public class UserData
    {
        public UserData();
        public UserData(LoginDeviceData data);

        public Dictionary<ClientType, LoginDeviceData> DeviceDictionary { get; set; }
        public string UserID { get; }
        //
        // 摘要:
        //     第一个设备上线的时间。
        public DateTime TimeLogon { get; }
        //
        // 摘要:
        //     在线设备的数量。
        public int DeviceCount { get; }
        //
        // 摘要:
        //     在线设备列表。
        public List<ClientType> DeviceTypeList { get; }
        //
        // 摘要:
        //     用户的携带数据（应用程序可以使用该属性保存与当前用户相关的其它信息），该Tag会在ACMS和AS之间自动同步。该Tag指向的对象必须可序列化。
        public byte[] Tag { get; set; }
        //
        // 摘要:
        //     用户的携带数据（应用程序可以使用该属性保存与当前用户相关的其它信息），仅在本地使用。不会在ACMS和AS之间同步。
        public object LocalTag { get; set; }

        //
        // 摘要:
        //     获取在线目标设备数据。
        public LoginDeviceData GetDevice(ClientType type);
        public List<LoginDeviceData> GetDevice(ClientType? type);
        //
        // 摘要:
        //     获取所有在线设备数据。
        public List<LoginDeviceData> GetDevices();
        //
        // 摘要:
        //     获取在线目标设备数据中的任意一个。
        public LoginDeviceData GetRandomDevice();
    }
}
