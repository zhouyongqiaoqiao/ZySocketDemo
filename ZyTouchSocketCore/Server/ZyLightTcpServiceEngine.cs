using System.Net;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Core;
using ZyLightTouchSocketCore.Interface;
using ZyTouchSocketCore;
using ZyTouchSocketCore.Core.Enum;

namespace ZyLightTouchSocketCore.Server
{
    /// <summary>
    /// 轻量级 TCP 服务器
    /// </summary>
    public class ZyLightTcpServiceEngine : TcpService<ZySocketClient>
    {
        private readonly ContractFormatStyle _contractFormatStyle;
        private IMessageHandler _messageHandler;
        private IServiceCustomizeHandler _servieCustomizeHandler;

        internal ZyLightTcpServiceEngine(ContractFormatStyle contractFormatStyle = ContractFormatStyle.Stream) : base()
        {
            _contractFormatStyle = contractFormatStyle;

            this._messageHandler = new MessageHandler(false);
        }



        public ContractFormatStyle ContractFormatStyle => _contractFormatStyle;

        public IServiceCustomizeHandler ServieCustomizeHandler 
        { 
            get => _servieCustomizeHandler; 
            set { 
                _servieCustomizeHandler = value; 
                this._messageHandler.ServiceCustomizeHandler = value; 
            } 
        }

        protected override void LoadConfig(TouchSocketConfig config)
        {
            config.ConfigureContainer((action) =>
            {
                action.AddConsoleLogger();
                action.AddFileLogger();
            });
            //此处加载配置，用户可以从配置中获取配置项。
            base.LoadConfig(config);
        }

        protected override Task OnConnecting(ZySocketClient socketClient, ConnectingEventArgs e)
        {
            //此处逻辑会多线程处理。

            //e.Id:对新连接的客户端进行ID初始化，默认情况下是按照设定的规则随机分配的。
            //但是按照需求，您可以自定义设置，例如设置为其IP地址。但是需要注意的是id必须在生命周期内唯一。

            //e.IsPermitOperation:指示是否允许该客户端链接。
            return base.OnConnecting(socketClient, e);
        }


        protected override Task OnReceived(ZySocketClient socketClient, ReceivedDataEventArgs e)
        {
            if (ContractFormatStyle == ContractFormatStyle.Stream)
            {
                if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
                {
                    this._messageHandler.HandleMessage(socketClient, packageInfo);
                    e.Handled = true;//表示该数据是否已经被本插件处理，无需再投递到其他插件。
                }
                else
                {
                    Logger.Error($"收到{socketClient.Id}发来的非法消息");
                }
            }
            else if (ContractFormatStyle == ContractFormatStyle.Text)
            {
                if (e.ByteBlock.Len>0) {
                    this._messageHandler.HandleMessage(socketClient, e.ByteBlock);
                    e.Handled = true;//表示该数据是否已经被本插件处理，无需再投递到其他插件。
                }
            }

            return base.OnReceived(socketClient, e);
        }



    }

    public class ZySocketClient : SocketClient, IServerSender
    {
        #region IServerSender
        public void SendMessageToClient(int informationType, byte[] msg)
        {
            this.SendCustomMessage(informationType, msg);
        }

        public void SendMessageToClientAsync(int informationType, byte[] msg)
        {
            this.SendCustomMessageAsync(informationType, msg);
        }

        public void SendMessageToClient(int informationType, string msg)
        {
            this.SendCustomMessage(informationType, msg);
        }

        public void SendMessageToClientAsync(int informationType, string msg)
        {
            this.SendCustomMessageAsync(informationType, msg);
        }
        #endregion

        protected override Task ReceivedData(ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                e.Handled = false;//表示该数据是否已经被本插件处理，无需再投递到其他插件。
            }
            else
            {
                Logger.Error($"收到{Id}发来的非法消息");
            }
            return base.ReceivedData(e);
        }
    }

    public interface IServerSender
    {
        string Id { get; }

        /// <summary>
        /// 给某个客户端同步发信息。注意：如果引擎已经停止或客户端不在线，则直接返回。   
        /// </summary>
        /// <param name="informationType">接收数据的客户端</param>
        /// <param name="msg">消息</param>
        void SendMessageToClient(int informationType, byte[] msg);

        /// <summary>
        /// 异步给某个客户端发信息。注意：如果引擎已经停止或客户端不在线，则直接返回。
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="msg"></param>
        void SendMessageToClientAsync(int informationType, byte[] msg);


        void SendMessageToClient(int informationType, string msg);

        void SendMessageToClientAsync(int informationType, string msg);
    }
}
