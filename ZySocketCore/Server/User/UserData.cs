using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using ZySocketCore.Core.Enum;
using ZySocketCore.Extension;
using ZySocketCore.Utils;

namespace ZySocketCore.Server.User
{
    public class UserData
    {
        public UserData(LoginDeviceData data)
        {
            this.DeviceDictionary = new ConcurrentDictionary<ClientType, LoginDeviceData>();
            this.DeviceDictionary.TryAdd(data.ClientType, data);
            this.TimeLogon = DateTime.Now;
            this.UserID = data.UserID;
        }

        public ConcurrentDictionary<ClientType, LoginDeviceData> DeviceDictionary { get; set; }
        public string UserID { get; private set; }
        //
        // 摘要:
        //     第一个设备上线的时间。
        public DateTime TimeLogon { get; private set; }
        //
        // 摘要:
        //     在线设备的数量。
        public int DeviceCount { get => this.DeviceDictionary.Count; }
        //
        // 摘要:
        //     在线设备列表。
        public List<ClientType> DeviceTypeList { get => new List<ClientType>(this.DeviceDictionary.Keys); }
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
        public LoginDeviceData GetDevice(ClientType type)
        {
            return this.DeviceDictionary.GetValueOrDefault(type);
        }

        public List<LoginDeviceData> GetDevice(ClientType? type)
        {
            if (type == null)
            {
                return new List<LoginDeviceData>(this.DeviceDictionary.Values);
            }
            return new List<LoginDeviceData> { this.DeviceDictionary.GetValueOrDefault(type.Value) };
        }

        public bool AddDevice(LoginDeviceData data)
        {
            if (this.DeviceDictionary.ContainsKey(data.ClientType))
            {
                return false;
            }
            this.DeviceDictionary.TryAdd(data.ClientType, data);
            return true;
        }

        public LoginDeviceData RemoveDevice(ClientType type)
        {
            this.DeviceDictionary.TryRemove(type, out LoginDeviceData data);
            return data;
        }

        //
        // 摘要:
        //     获取所有在线设备数据。
        public List<LoginDeviceData> GetDevices()
        {
            return new List<LoginDeviceData>(this.DeviceDictionary.Values);
        }

        /// <summary>
        /// 获取所有完整的 Client ID
        /// </summary>
        /// <returns></returns>
        public List<string> GetLoginIdList()
        {
            List<string> list = new List<string>();
            foreach (LoginDeviceData item in this.DeviceDictionary.Values)
            {
                list.Add(item.LoginID);
            }
            return list;
        }

        public List<ClientType> GetClientTypes()
        {
            return new List<ClientType>(this.DeviceDictionary.Keys);
        }

        //
        // 摘要:
        //     获取在线目标设备数据中的任意一个。
        public LoginDeviceData GetRandomDevice()
        {
            return this.DeviceDictionary.Values.RandomItem();
        }
    }
}
