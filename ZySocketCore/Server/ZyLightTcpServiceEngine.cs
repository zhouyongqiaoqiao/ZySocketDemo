using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Core;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;
using ZySocketCore;
using System;
using System.Text;
using System.Threading;
using ZySocketCore.Utils;
using ZySocketCore.Manager;
using ZySocketCore.Core.QueryInfo;
using ZySocketCore.Server.User;

namespace ZyLightTouchSocketCore.Server
{
    /// <summary>
    /// 轻量级 TCP 服务器
    /// </summary>
    internal class ZyLightTcpServiceEngine : TcpService<ZySocketClient>
    {
        private readonly ContractFormatStyle _contractFormatStyle;
        private IMessageHandler _messageHandler;
        private ICustomizeHandler _servieCustomizeHandler;
       
        internal ZyLightTcpServiceEngine(ContractFormatStyle contractFormatStyle = ContractFormatStyle.Stream) : base()
        {
            _contractFormatStyle = contractFormatStyle;

            this.MessageHandler = new MessageHandler(false);
            this._timer = new Timer(ShowUsers, null, 1000, 10000);            
        }

        private Timer _timer;

        private void ShowUsers(object state) {
            
            Console.WriteLine($"当前在线用户:{UserManager.Instance.GetOnlineUserList().ToJsonString()}");        
        }


        public ContractFormatStyle ContractFormatStyle => _contractFormatStyle;

        public ICustomizeHandler ServieCustomizeHandler 
        { 
            get => _servieCustomizeHandler; 
            set { 
                _servieCustomizeHandler = value; 
                this.MessageHandler.CustomizeHandler = value; 
            } 
        }

        public IMessageHandler MessageHandler { get => _messageHandler;private set => _messageHandler = value; }

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

        protected override Task OnConnected(ZySocketClient socketClient, ConnectedEventArgs e)
        {
            //receiver可以复用，不需要每次接收都新建
            //using (var receiver = socketClient.CreateReceiver())
            //{
            //    while (true)
            //    {
            //        //receiverResult必须释放
            //        using (var receiverResult = receiver.ReadAsync(CancellationToken.None))
            //        {
            //            if (receiverResult.Result.IsClosed)
            //            {
            //                //断开连接了
            //                return base.OnConnected(socketClient, e);
            //            }

            //            //从服务器收到信息。
            //            string? id = Encoding.UTF8.GetString(receiverResult.Result.ByteBlock.Buffer, 0, receiverResult.Result.ByteBlock.Len);
            //            socketClient.Logger.Info($"客户端接收到信息：{id}");
            //            if (!string.IsNullOrEmpty(id))
            //            {
            //                GlobalUtil.ResetUserId(socketClient, IdUtil.BuildFullUserId(ClientType.Win, id));
            //            }
            //            return base.OnConnected(socketClient, e);
            //            //如果是适配器信息，则可以直接获取receiverResult.RequestInfo;

            //        }
            //    }
            //}
            return base.OnConnected(socketClient, e);

        }

        protected override Task OnDisconnected(ZySocketClient socketClient, DisconnectEventArgs e)
        {
            return base.OnDisconnected(socketClient, e);
        }


        protected override Task OnReceived(ZySocketClient socketClient, ReceivedDataEventArgs e)
        {
            if (ContractFormatStyle == ContractFormatStyle.Stream)
            {
                if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
                {
                    this.MessageHandler.HandleMessage(socketClient, packageInfo);
                    e.Handled = false;//表示该数据是否已经被本插件处理，无需再投递到其他插件。
                }
                else
                {
                    Logger.Error($"收到{socketClient.Id}发来的非法消息");
                }
            }
            else if (ContractFormatStyle == ContractFormatStyle.Text)
            {
                if (e.ByteBlock.Len>0) {
                    this.MessageHandler.HandleMessage(socketClient, e.ByteBlock);
                    e.Handled = false;//表示该数据是否已经被本插件处理，无需再投递到其他插件。
                }
            }

            return base.OnReceived(socketClient, e);
        }



    }

    public class ZySocketClient : SocketClient, ISenderById
    {

        public ZySocketClient()
        {
            this.Queryer = new WaitingClientTcp<SocketClient>(this);
        }
        //public override sealed bool CanSetDataHandlingAdapter => false;//不允许随意赋值

        internal IQueryer Queryer { get; private set; }

        /// <summary>
        /// 回复查询结果
        /// </summary>
        /// <param name="resBytes"></param>
        public void ReplyQueryResult(byte[] resBytes)
        {
            this.SendMessageAsync(SystemSettings.ServerDefaultId, this.Id, (int)MessageType.QUERY_RESPONSE, resBytes);
        }


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
}
