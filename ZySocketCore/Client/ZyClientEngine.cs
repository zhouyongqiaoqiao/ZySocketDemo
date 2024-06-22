using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Client.Basic;
using ZySocketCore.Core;
using ZySocketCore.Core.Contacts.Client;
using ZySocketCore.Core.Contract;
using ZySocketCore.Core.CustomizeInfo.Client;
using ZySocketCore.Core.DynamicGroup.Client;
using ZySocketCore.Core.Enum;
using ZySocketCore.Core.QueryInfo;
using ZySocketCore.Interface;



namespace ZySocketCore.Client
{
    public class ZyClientEngine : TcpClient, ISenderById, IZyClientEngine, IBlobAndTagMessageHandler
    {
        private readonly ContractFormatStyle _contractFormatStyle;
        private IMessageHandler _messageHandler;
        private ICustomizeHandler customizeHandler;
        private Timer _heartBeatTimer;

        internal ZyClientEngine(ContractFormatStyle contractFormatStyle = ContractFormatStyle.Stream) : base()
        {
            this._contractFormatStyle = contractFormatStyle;
            this.queryer = new WaitingClientTcp<TcpClient>(this);
            this.CustomizeOutter = new CustomizeOutter(this);
            this.BasicOutter = new BasicOutter(this);
            this.ContactsOutter =new ContactsOutter(this);
            this.DynamicGroupOutter = new DynamicGroupOutter(this);
            this._messageHandler = new MessageHandler();
            this._messageHandler.BlobAndTagMessageHandler = this;
            this._heartBeatTimer = new Timer(this.SendHeartBeatMessage, null, 0, HeartBeatSpanInSecs * 1000);
        }

        public ICustomizeHandler CustomizeHandler
        {
            private get => customizeHandler;
            set
            {
                customizeHandler = value;
                this._messageHandler.CustomizeHandler = value;
            }
        }

        public ContractFormatStyle ContractFormatStyle => _contractFormatStyle;

        private IQueryer queryer;

        internal IQueryer Queryer => this.queryer;

        /// <summary>
        /// 处理收到的消息 客户端将会先触发 _messageHandler，然后再触发 Received 事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override Task ReceivedData(ReceivedDataEventArgs e)
        {
            if (ContractFormatStyle == ContractFormatStyle.Stream)
            {
                if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
                {
                    this._messageHandler.HandleMessage(this, packageInfo);
                    e.Handled = false;//表示该数据是否已经被本插件处理，无需再投递到其他插件。不影响 Received 的事件触发
                }
                else
                {
                    Logger.Error($"收到发来的非法消息");
                }
            }
            else if (ContractFormatStyle == ContractFormatStyle.Text)
            {
                if (e.ByteBlock.Len > 0)
                {
                    this._messageHandler.HandleMessage(this, e.ByteBlock);
                    e.Handled = true;//表示该数据是否已经被本插件处理，无需再投递到其他插件。 不影响 Received 的事件触发
                }
            }
            return base.ReceivedData(e);
        }

        #region IZyClientEngine

        public string SystemToken { get; set; }
        public int WaitResponseTimeoutInSecs
        {
            get => waitResponseTimeoutInSecs;
            set
            {
                waitResponseTimeoutInSecs = value;
                this.queryer.WaitResponseTimeoutInSecs = value;
            }
        }
        public int HeartBeatSpanInSecs { get; set; } = 10;

        public bool ChannelIsBusy => false;

        bool IZyClientEngine.Connected => this.Online;

        public IPHost ServerAddress => this.RemoteIPHost;

        public bool IsClosed => !this.Online;

        public ClientType CurrentClientType => ClientType.Win;

        public string CurrentUserID { get; private set; }
        private string _logonPassword;
        private int waitResponseTimeoutInSecs = 30;

        public bool AutoReconnect { get; set; } =true;
        public ICustomizeOutter CustomizeOutter { get;private set; }
        public IBasicOutter BasicOutter { get; private set; }
        public IContactsOutter ContactsOutter { get; private set; }
        public IDynamicGroupOutter DynamicGroupOutter { get; private set; }
        public string Id => this.CurrentUserID;
        public event Action ConnectionRebuildStart;
        public event Action ConnectionInterrupted;
        public event Action<LogonResponse> RelogonCompleted;
        //public event Action<ClientType, string, int, byte[], string> EchoMessageReceived;
        public event Action<string, ClientType, int, byte[], string> MessageReceived;

        public void Close()
        {
            base.Close(string.Empty);
        }

        public void CloseConnection(bool reconnectNow)
        {
            base.Close(string.Empty);
            if (reconnectNow) {
                try
                {
                    this.Connect();
                    this.DoLogin(this.CurrentUserID, this._logonPassword);
                }
                catch (Exception ee)
                {
                    base.Logger.Error($"重连失败:{ee.Message}");
                }
            }
        }

        public LogonResponse Initialize(string userID, string logonPassword, string serverIP, int serverPort, ICustomizeHandler customizeHandler)
        {
            string failureCause = "初始化出现错误";
            try
            {
                //即将连接到服务器，此时已经创建socket，但是还未建立tcp
                this.Connecting = (client, e) =>
                {
                    if (!string.IsNullOrEmpty(this.CurrentUserID) && this.AutoReconnect)
                    {
                        this.ConnectionRebuildStart?.Invoke();
                    }
                    Console.WriteLine($"{client.IP}正在连接"); return EasyTask.CompletedTask;
                };
                //成功连接到服务器
                this.Connected = (client, e) =>
                {
                    if (!string.IsNullOrEmpty(this.CurrentUserID))
                    {
                        //未设置自动重连，则关闭连接
                        if (!this.AutoReconnect) { this.Close(); return EasyTask.CompletedTask; }
                        //重新登录
                        Task.Delay(10).ContinueWith((a, c) =>
                        {
                            LogonResponse? res = this.DoLogin(this.CurrentUserID, this._logonPassword);
                            this.RelogonCompleted?.Invoke(res);
                            //    client.SendMessage(this.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.ReConnected, new byte[0]);
                        }, null);
                    }
                    Console.WriteLine($"{client.IP}已连接");
                    return EasyTask.CompletedTask;
                };
                //即将从服务器断开连接。此处仅主动断开才有效。
                this.Disconnecting = (client, e) =>
                {
                    Console.WriteLine($"{client.IP}正在断开"); return EasyTask.CompletedTask;
                };
                //从服务器断开连接，当连接不成功时不会触发。
                this.Disconnected = (client, e) =>
                {
                    this.ConnectionInterrupted?.Invoke();
                    Console.WriteLine($"{client.IP}已断开"); return EasyTask.CompletedTask;
                };
                this.Config.SetRemoteIPHost($"{serverIP}:{serverPort}");
                //this.SetupConfig(serverIP, serverPort);
                this.Connect();
                return this.DoLogin(userID, logonPassword, customizeHandler);
            }
            catch (TimeoutException)
            {
                failureCause = "连接超时";
            }
            catch (Exception ee)
            {
                base.Logger.Error(ee.Message);
                failureCause = ee.InnerException != null ? ee.InnerException.Message : ee.Message;
            }
            this.Close();
            return new LogonResponse(LogonResult.Failed, failureCause);
        }

        private LogonResponse DoLogin(string userID, string logonPassword, ICustomizeHandler? customizeHandler = null)
        {
            LogonRequest request = new LogonRequest(this.SystemToken, logonPassword, "FreeUser");
            byte[] data = SerializeConvert.JsonSerializeToBytes(request);
            ResponsedData res = this.queryer.QueryMessage(userID, SystemSettings.ServerDefaultId, (int)MessageType.REQ_OR_RESP_LOGON, data);
            ZyLightFixedHeaderPackageInfo packageInfo = res.RequestInfo as ZyLightFixedHeaderPackageInfo;
            LogonResponse logonResponse = SerializeConvert.JsonDeserializeFromBytes<LogonResponse>(packageInfo.Body);
            if (logonResponse.LogonResult != LogonResult.Succeed)
            {
                this.Close();
            }
            else
            {
                this.CurrentUserID = userID;
                this._logonPassword = logonPassword;
                if (customizeHandler != null)
                {
                    this.CustomizeHandler = customizeHandler;
                }
            }
            return logonResponse;
        }

        private void SetupConfig(string serverIP, int serverPort)
        {
            this.Setup(new TouchSocketConfig()
                .SetRemoteIPHost($"{serverIP}:{serverPort}")
                .SetTcpDataHandlingAdapter(() => new ZyLightFixedHeaderDataAdapter())
                .ConfigurePlugins(plugin =>
                {
                    //设置断线重连
                    plugin.UseReconnection(-1, false, 1000);
                })
                //SetDataHandlingAdapter(() =>  new TerminatorPackageAdapter("\0") )
                .ConfigureContainer(a =>
                {
                    a.AddConsoleLogger();//添加一个日志注入
                    a.AddFileLogger();
                }));
        }

        public string QueryBeforeLogin(string serverIP, int serverPort, int queryType, string query)
        {
            if (this.Online) { throw new Exception("连接已建立，请求失败，请在登陆前执行。");    }
            this.Config.SetRemoteIPHost($"{serverIP}:{serverPort}");
            this.Connect();
            QueryBeforeLoginContract request = new QueryBeforeLoginContract(queryType, query);
            byte[] data = SerializeConvert.JsonSerializeToBytes(request);
            ResponsedData res = this.queryer.QueryMessage(string.Empty, SystemSettings.ServerDefaultId, (int)MessageType.QueryBeforeLogin, data);
            ZyLightFixedHeaderPackageInfo packageInfo = res.RequestInfo as ZyLightFixedHeaderPackageInfo;
            this.Close();//返回前关闭连接
            return Encoding.UTF8.GetString(packageInfo.Body);      
        }

        public void SendMessage(string targetUserID, int informationType, byte[] message, string tag, ClientType ?destClientType = null)
        {
            this.SendTagMessage(this.CurrentUserID, targetUserID, informationType, message, tag, destClientType);
        }

        public void SendMessage(string targetUserID, int informationType, byte[] message, string tag, bool toOtherClientOfMine = false)
        {

        }

        public void SendMessageToSpecialDevice(string targetUserID, ClientType targetType, int informationType, byte[] message, string tag)
        {
            this.SendTagMessage(this.CurrentUserID, targetUserID, informationType, message, tag, targetType);
        }
        #endregion

        #region IBlobAndTagMessageHandler
        public void HandleBlobAndTagMessage(string sourceUserID, ClientType clientType, int informationType, byte[] blob, string tag)
        {
            this.MessageReceived?.Invoke(sourceUserID, clientType, informationType, blob, tag);
        }
        #endregion



        #region 方法

        /// <summary>
        /// 发送心跳包
        /// </summary>
        /// <param name="state"></param>
        private void SendHeartBeatMessage(object state)
        {
            this.BasicOutter.SendHeartBeatMessage();
        }

        internal T QueryMessage<T>(string targetUserID, int messageType, byte[] message) where T : new()
        {
            var res= this.queryer.QueryMessage(this.CurrentUserID,targetUserID, messageType, message);
            if (res.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.Body == null || packageInfo.Body.Length == 0)
                {
                    return default(T);
                }
                return SerializeConvert.JsonDeserializeFromBytes<T>(packageInfo.Body);
            }
            else
            {
                this.Logger.Error($"请求{messageType}消息失败");
                throw new Exception("请求失败");
            }
        }
        #endregion
    }

    

 

    
}
