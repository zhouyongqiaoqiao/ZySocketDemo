using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Server;
using ZySocketCore.Core;
using ZySocketCore.Core.Contacts.Server;
using ZySocketCore.Core.CustomizeInfo.Server;
using ZySocketCore.Core.DynamicGroup.Server;
using ZySocketCore.Core.Enum;
using ZySocketCore.Extension;
using ZySocketCore.Interface;
using ZySocketCore.Manager;
using ZySocketCore.Server.Basic;
using ZySocketCore.Server.Plugin;
using ZySocketCore.Server.User;
using ZySocketCore.Utils;


namespace ZySocketCore.Server
{
    /// <summary>
    /// 轻量级 TCP 服务器
    /// </summary>
    public class ZyTcpServiceEngine : IZyServerEngine, IBlobAndTagMessageHandler
    {
        private ZyLightTcpServiceEngine tcpService = new ZyLightTcpServiceEngine();

        private ICustomizeHandler _servieCustomizeHandler;
        private IBasicHandler _basicHandler;
 
        private Timer _heartBeatTimer;

        internal ZyTcpServiceEngine()
        {
            this.tcpService.MessageHandler.BlobAndTagMessageHandler = this;           
            this._heartBeatTimer = new Timer(this.CheckHeartBeat, null, 1000, 10 * 1000);
            this.BasicController = new BasicController(this.tcpService);
            this.CustomizeController = new CustomizeController(this.tcpService);
            this.UserManager = User.UserManager.Instance; 
            this.DynamicGroupController = new DynamicGroupController(this.tcpService);
        }

        /// <summary>
        /// 每10s检查一次客户端心跳
        /// </summary>
        /// <param name="state"></param>
        private void CheckHeartBeat(object state)
        {
             foreach (ZySocketClient client in this.tcpService.GetClients())
            {
                if ((DateTime.Now - client.LastReceivedTime) > TimeSpan.FromSeconds(HeartbeatTimeoutInSecs))
                {
                    Console.WriteLine($"客户端{client.Id}心跳超时！");
                    this.BasicController.KickOut(client.Id);
                }
            }
        }

        /// <summary>
        /// 踢出客户端，通知客户端被踢出（客户端主动断开连接）
        /// </summary>
        /// <param name="client"></param>
        private void KickOut(ZySocketClient client)
        {
            client.SendMessageAsync(SystemSettings.ServerDefaultId, client.Id, (int)MessageType.BE_KICKED_OUT_NOTIFY, new byte[0]);
            Console.WriteLine($"发送通知客户端{client.Id}被踢出！");
        }

        public ICustomizeHandler ServieCustomizeHandler
        {
            get => _servieCustomizeHandler;
            set {
                _servieCustomizeHandler = value;
                this.tcpService.MessageHandler.CustomizeHandler = value;
            }
        }

        #region IZyServerEngine

        public event Action<string, ClientType, int, byte[], string> MessageReceived;

        public string IPAddressBinding { get; set; }
        public int HeartbeatTimeoutInSecs { get; set; } = 30;
        public int WaitResponseTimeoutInSecs { get; set; } = 10;
        public bool AutoRespondHeartBeat { get; set; } = true;
        public bool UseAsP2PServer { get; set; } = false;

        public int ConnectionCount => this.tcpService.Count;

        public int MaxConnectionCount => -1;

        public int Port
        {
            get
            {
                if (this.tcpService.ServerState == ServerState.Running)
                {
                    return this.tcpService.Monitors.First().Option.IpHost.Port;
                }
                return -1;
            }
        }

        public string ServerName => this.tcpService.ServerName;

        public IContactsManager ContactsManager { private get; set; } = new EmptyContactsManager();
        public IBasicController BasicController { get; private set; }
        public ICustomizeController CustomizeController { get; private set; }
        public IContactsController ContactsController { get; private set; } 
        public IUserManager UserManager { get; private set; }

        public IDynamicGroupController DynamicGroupController { get; private set; }

        public void Close()
        {            
            this.tcpService.Stop();
            User.UserManager.Instance.Clear();
        }

        public void Initialize(int port, ICustomizeHandler customizeHandler)
        {
            this.Initialize(port, customizeHandler, new EmptyBasicHandler());
        }

        public void Initialize(int port, ICustomizeHandler customizeHandler, IBasicHandler basicHandler)
        {
            this.ServieCustomizeHandler = customizeHandler;
            this._basicHandler = basicHandler;
            this.ContactsController = new ContactsController(this.tcpService, this.ContactsManager, this.UserManager);
            var config = new TouchSocketConfig()
                .SetListenOptions(option =>
                {
                    option.Add(new TcpListenOption()
                    {
                        IpHost = this.IPAddressBinding ?? "0.0.0.0" + ":" + port,
                        Adapter = () => new ZyLightFixedHeaderDataAdapter()
                    });
                });
            this.SetPluigns(config);
            this.tcpService.Setup(config);

            this.SetPluigns();
            this.tcpService.Start();
        }


        public void SendMessage(string targetUserID, int informationType, byte[] message, string tag, ClientType? clientType = null)
        {
            try
            {
                targetUserID = clientType == null? targetUserID : IdUtil.BuildFullUserId(clientType, targetUserID);

                List<ZySocketClient> clientList = this.tcpService.GetSocketClients(targetUserID);
                foreach (ZySocketClient socketClient in clientList)
                {
                    socketClient.SendTagMessage(SystemSettings.ServerDefaultId, targetUserID, informationType, message, tag, clientType);
                }
            }
            catch (Exception ee)
            {
                this.tcpService.Logger.Error(ee, "发送消息失败！");
            }     
        }
        #endregion

        private void SetPluigns(TouchSocketConfig config)
        {
            config.ConfigurePlugins(plugin =>
            {
                plugin.Add(new LoginPlugin(this._basicHandler, this.tcpService));
                plugin.Add(new HeartBeatPlugin());
                plugin.Add(new BasicPlugin(this));
                plugin.Add(new UserDevicePlugin(this));
                plugin.Add((UserManager)this.UserManager);
                plugin.Add((ContactsController)this.ContactsController);
                plugin.Add((DynamicGroupController)this.DynamicGroupController);
            });
        }

        private void SetPluigns()
        {
            this.tcpService.PluginManager.Add(new IdChangedPlugin());
        }

        #region IBlobAndTagMessageHandler
        public void HandleBlobAndTagMessage(string sourceUserID, ClientType clientType, int informationType, byte[] blob, string tag)
        {
                this.MessageReceived?.Invoke(sourceUserID, clientType, informationType, blob, tag);
        }
        #endregion
    }

}
