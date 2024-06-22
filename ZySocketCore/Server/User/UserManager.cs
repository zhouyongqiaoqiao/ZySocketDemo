using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Server;
using ZySocketCore.Core;
using ZySocketCore.Core.Enum;
using ZySocketCore.Manager;
using ZySocketCore.Utils;

namespace ZySocketCore.Server.User
{
    public class UserManager : PluginBase, IUserManager, IIdChangedPlugin,
        ITcpReceivedPlugin,
        ITcpConnectingPlugin<ZySocketClient>,
        ITcpConnectedPlugin<ZySocketClient>, 
        ITcpDisconnectedPlugin<ZySocketClient>
    {
        private ConcurrentDictionary<string, UserData> dict = new ConcurrentDictionary<string, UserData>();

        private UserManager()
        {
            
        }

        private static UserManager _instance;
        internal static UserManager Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserManager();
                }
                return _instance;
            }
        }

        #region IUserManager
        public int UserCount => this.dict.Count;

        public bool DontCheckHeartbeat4Websocket { get; set; } = true;

        public event Action<string> UserConnected;
        public event Action<string> UserDisconnected;
        public event Action<int> UserCountChanged;
        public event Action<LoginDeviceData> ClientDeviceBeingPushedOut;
        public event Action<LoginDeviceData> ClientDeviceConnected;
        public event Action<LoginDeviceData, DisconnectedType> ClientDeviceDisconnected;
        public event Action<string, byte[]> UserTagChanged;

        public List<LoginDeviceData> GetAllLoginDevice()
        {
            List<LoginDeviceData> list = new List<LoginDeviceData>();
            foreach (var item in dict.Values)
            {
                list.AddRange(item.GetDevices());
            }
            return list;
        }

        public List<UserData> GetAllUserData()
        {
            return new List<UserData>(dict.Values);
        }

        public LoginDeviceData GetLoginDeviceDataByLoginID(string loginID)
        {
            string userID = IdUtil.GetUserId(loginID);
            ClientType clientType = IdUtil.GetClientType(userID);
            UserData? userData = this.dict.GetValueOrDefault(userID);
            if (userData == null)
            {
                return null;
            }
            return userData.GetDevice(clientType);
        }

        public List<string> GetOnlineUserList()
        {
            return this.dict.Keys.ToList();
        }

        public EndPoint GetUserAddressByLoginID(string loginID)
        {
            LoginDeviceData? deviceData = this.GetLoginDeviceDataByLoginID(loginID);
            if (deviceData == null) return null;
            return deviceData.Address.EndPoint;
        }

        public UserData GetUserData(string userID)
        {
            return this.dict.GetValueOrDefault(userID);
        }

        /// <summary>
        /// 获取目标用户所有的登录ID列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        internal List<string> GetLoginIdList(string userID)
        {
            List<string> list = new List<string>();
            if (IdUtil.IsFullUserId(userID))
            {
                list.Add(userID);
            }
            else { 
               UserData? userData =  this.GetUserData(userID);
                if(userData!= null)
                {
                    list.AddRange(userData.GetLoginIdList());
                }
            }

            return list;
        }

        /// <summary>
        /// 获取目标用户在线的设备列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        internal List<ClientType> GetUserClientType(string userID)
        {
            List<ClientType> list = new List<ClientType>();
            UserData? userData = this.GetUserData(userID);
            if (userData == null) return list;
            list.AddRange(userData.GetClientTypes());
            return list;
        }

        internal void Clear()
        {
            this.dict.Clear();
        }

        public bool IsUserOnLine(string userID)
        {
            return this.dict.ContainsKey(userID);
        }
        #endregion


        #region ITcpReceivedPlugin
        public Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.REQ_HEART_BEAT)
                {
                    return EasyTask.CompletedTask;
                }
            }
            return e.InvokeNext();
        }
        #endregion

        #region ITcpConnectingPlugin

        public Task OnTcpConnecting(ZySocketClient client, ConnectingEventArgs e)
        {
            Console.WriteLine($"{client.Id} 正在连接！"); return EasyTask.CompletedTask;
        }

        #endregion

        #region ITcpConnectedPlugin
        public Task OnTcpConnected(ZySocketClient client, ConnectedEventArgs e)
        {
            this.AddClient(client);
            Console.WriteLine($"{client.Id} 已连接！{client.GetIPPort()}");
            //client.SendAsync($"发送消息给{client.Id}");
            //Console.WriteLine($"发送消息给{client.Id} 完成！");
            return EasyTask.CompletedTask;
        }
        #endregion


        #region ITcpDisconnectedPlugin
        public Task OnTcpDisconnected(ZySocketClient client, DisconnectEventArgs e)
        {
            // 检查客户端ID是否为完整的用户ID，如果不是则返回已完成任务
           if (!IdUtil.IsFullUserId(client.Id)) return EasyTask.CompletedTask;
            // 获取用户ID
            string userID = IdUtil.GetUserId(client.Id);
            // 获取客户端类型
            ClientType clientType = IdUtil.GetClientType(client.Id);
            // 从字典中获取用户数据，如果不存在则返回已完成任务
            UserData? userData = this.dict.GetValueOrDefault(userID);
            if (userData == null) return EasyTask.CompletedTask;
            // 从用户数据中移除设备数据
            LoginDeviceData? deviceData = userData.RemoveDevice(clientType);
            // 如果用户数据中设备数量为0，则移除用户数据并触发用户断开连接事件
            if (userData.DeviceCount == 0)
            {
                this.dict.TryRemove(userData.UserID, out UserData userdata);
                this.UserDisconnected?.Invoke(userData.UserID);
                this.UserCountChanged?.Invoke(this.UserCount);
            }
            else
            {
                // 否则触发客户端设备断开连接事件
                this.ClientDeviceDisconnected?.Invoke(deviceData, DisconnectedType.NetworkInterrupted);
            }
            // 输出客户端断开连接的日志
            Console.WriteLine($"{client.Id} 断开连接"); return EasyTask.CompletedTask;

        }

        #endregion

        #region IIdChangedPlugin
        public Task OnIdChanged(IClient client, IdChangedEventArgs e)
        {
            Console.WriteLine($"Client ID 已改变！{e.OldId} -> {e.NewId}");
            this.AddClient(client as ZySocketClient);
            return EasyTask.CompletedTask;
        }
        #endregion

        private void AddClient(ZySocketClient client)
        {
            if (client.Id == null) return; // 如果客户端ID为空，直接返回
            if (IdUtil.IsFullUserId(client.Id)) // 如果客户端ID是完整的用户ID
            {
                ClientType clientType = IdUtil.GetClientType(client.Id); // 获取客户端类型
                LoginDeviceData loginDeviceData = new LoginDeviceData(client.Id, new IPHost(client.GetIPPort()), client.ListenOption.Name, clientType, DateTime.Now); // 创建登录设备数据
                UserData userData = this.GetUserData(IdUtil.GetUserId(client.Id)); // 获取用户数据
                if (userData == null) // 如果用户数据为空
                {
                    userData = new UserData(loginDeviceData); // 创建新的用户数据
                    this.dict.TryAdd(userData.UserID, userData); // 尝试添加用户数据到字典
                    this.UserConnected?.Invoke(userData.UserID); // 触发用户连接事件
                    this.UserCountChanged?.Invoke(this.UserCount); // 触发用户数量变化事件
                }
                else // 如果用户数据不为空
                {
                    userData.AddDevice(loginDeviceData); // 添加设备到用户数据
                    this.ClientDeviceConnected?.Invoke(loginDeviceData); // 触发客户端设备连接事件
                }
            }

        }
    }
}
