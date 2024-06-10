
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Core;
using ZyLightTouchSocketCore.Interface;
using ZyLightTouchSocketCore.Server;
using ZyTouchSocketCore;
using ZyTouchSocketCore.Core.Enum;

namespace ZyLightTouchSocketCore.Client
{
    public class ZyLightClientEngine : TcpClient
    {
        private readonly ContractFormatStyle _contractFormatStyle;
        private IMessageHandler _messageHandler;
        private ICustomizeHandler customizeHandler;

        internal ZyLightClientEngine(ContractFormatStyle contractFormatStyle = ContractFormatStyle.Stream) : base()
        {
            this._contractFormatStyle = contractFormatStyle;
            this.queryer = new WaitingClientTcp(this);
            this._messageHandler = new MessageHandler();
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
        public IQueryer Queryer => this.queryer;

        protected override Task ReceivedData(ReceivedDataEventArgs e)
        {
            if (ContractFormatStyle == ContractFormatStyle.Stream)
            {
                if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
                {
                    this._messageHandler.HandleMessage(this, packageInfo);
                    e.Handled = true;//表示该数据是否已经被本插件处理，无需再投递到其他插件。
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
                    e.Handled = true;//表示该数据是否已经被本插件处理，无需再投递到其他插件。
                }
            }     
            return base.ReceivedData(e);
        }
       

        #region SendMessage
        protected void SendCustomMessage(int informationType, byte[] body)
        {
            try
            {
                byte[] data = this.CreateCustomMessageData(informationType, body);
                this.SendMessage((int)MessageType.NORMAL_MESSAGE, data);
            }
            catch (System.Exception ee)
            {
                base.Logger.Error(ee.Message);
            }
        }

        protected Task SendCustomMessageAsync(int informationType, byte[] body)
        {
            try
            {
                byte[] data = this.CreateCustomMessageData(informationType, body);
                return this.SendMessageAsync((int)MessageType.NORMAL_MESSAGE_ASYNC, data);
            }
            catch (System.Exception ee)
            {
                base.Logger.Error(ee.Message);
            }
            return Task.FromResult(0);
        }

        protected void SendCustomMessage(int informationType, string body)
        {
            try
            {
                string msg = this.CreateCustomMessageData(informationType, body);
                this.SendTextMessage((int)MessageType.NORMAL_MESSAGE, msg);
            }
            catch (System.Exception ee)
            {
                base.Logger.Error(ee.Message);
            }
        }

        protected Task SendCustomMessageAsync(int informationType, string body)
        {
            try
            {
                string msg = this.CreateCustomMessageData(informationType, body);
                return this.SendTextMessageAsync((int)MessageType.NORMAL_MESSAGE_ASYNC, msg);
            }
            catch (System.Exception ee)
            {
                base.Logger.Error(ee.Message);
            }
            return Task.FromResult(0);
        }

        #endregion

        #region QueryMessage

        /// <summary>
        /// 创建自定义消息
        /// </summary>
        /// <param name="informationType"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        protected byte[] CreateCustomMessageData(int informationType, byte[] body) {
            CustomMessage customeMessage = new CustomMessage()
            {
                InformationType = informationType,
                Content = body
            };
            return SerializeConvert.FastBinarySerialize(customeMessage);
        }

        protected string CreateCustomMessageData(int informationType, string msg)
        {
            return informationType + ZyTouchSocketCore.SenderExtension.Separator_CustomMsg_Str + msg;
        }


        protected byte[] QueryMessage(int informationType, byte[] body)
        {
            try
            {
                byte[] data = this.CreateCustomMessageData(informationType, body);
                ResponsedData res = this.queryer.QueryMessage((int)MessageType.QUERY, data);
                var packageInfo = res.RequestInfo as ZyLightFixedHeaderPackageInfo;
                return packageInfo.Body;
            }
            catch (System.Exception ee)
            {
                base.Logger.Error(ee.Message);
            }
            return null;
        }

        protected async Task<byte[]> QueryMessageAsync(int informationType, byte[] body)
        {
            try
            {
                byte[] data = this.CreateCustomMessageData(informationType, body);
                ResponsedData res = await this.queryer.QueryMessageAsync((int)MessageType.QUERY_ASYNC, data);
                ZyLightFixedHeaderPackageInfo packageInfo = res.RequestInfo as ZyLightFixedHeaderPackageInfo;
                return packageInfo.Body;
            }
            catch (System.Exception ee)
            {
                base.Logger.Error(ee.Message);
            }
            return null;
        }

        protected string QueryTextMessage(int messageType, string msg)
        {
            try
            {
                string cumstomMsg = this.CreateCustomMessageData(messageType, msg);
                ResponsedData res = this.queryer.QueryTextMessage((int)MessageType.QUERY, cumstomMsg);
                return Encoding.UTF8.GetString(res.Data);
            }
            catch (System.Exception ee)
            {
                base.Logger.Error(ee.Message);
            }
            return null;
        }

        protected async Task<string> QueryTextMessageAsync(int messageType, string msg)
        {
            try
            {
                string cumstomMsg = this.CreateCustomMessageData(messageType, msg);
                ResponsedData res = await this.queryer.QueryTextMessageAsync((int)MessageType.QUERY_ASYNC, cumstomMsg);
                return Encoding.UTF8.GetString(res.Data);
            }
            catch (System.Exception ee)
            {
                base.Logger.Error(ee.Message);
            }
            return null;
        }
        #endregion

    }

    internal class WaitingClientTcp : IQueryer
    {
        private readonly TcpClient _client;
        public WaitingClientTcp(TcpClient tcpClient)
        {
            this._client = tcpClient;
        }


        private IWaitingClient<TcpClient> waitingClient;
        private IWaitingClient<TcpClient> WaitingClient
        {
            get
            {
                if (this.waitingClient == null)
                {
                    this.waitingClient = this._client.CreateWaitingClient(new WaitingOptions()
                    {
                        FilterFunc = (responsedData) =>
                        {
                            return true;
                        }
                    });
                }
                return this.waitingClient;
            }
        }
        public ResponsedData Query(byte[] buffer, int offset, int length)
        {
            ResponsedData data = this.WaitingClient.SendThenResponse(buffer, offset, length);
            return data;///TO DO : 未处理
        }

        public ResponsedData Query(byte[] buffer)
        {
            return this.Query(buffer, 0, buffer.Length);
        }

        public ResponsedData Query(string message)
        {
            return this.WaitingClient.SendThenResponse(message);
        }

        public Task<ResponsedData> QueryAsync(byte[] buffer, int offset, int length)
        {
            return this.WaitingClient.SendThenResponseAsync(buffer, offset, length);
        }

        public Task<ResponsedData> QueryAsync(byte[] buffer)
        {
            return this.QueryAsync(buffer, 0, buffer.Length);
        }

        public Task<ResponsedData> QueryAsync(string message)
        {
            return this.WaitingClient.SendThenResponseAsync(message);
        }
    }

    public interface IQueryer
    {
        /// <summary>
        /// 同步请求回复
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        ResponsedData Query(byte[] buffer, int offset, int length);

        /// <summary>
        /// 同步请求回复
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        ResponsedData Query(byte[] buffer);

        /// <summary>
        /// 同步请求回复
        /// </summary>
        /// <param name="message">文本消息</param>
        /// <returns></returns>
        ResponsedData Query(string message);

        /// <summary>
        /// 异步请求回复
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        Task<ResponsedData> QueryAsync(byte[] buffer, int offset, int length);

        /// <summary>
        /// 异步请求回复
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        Task<ResponsedData> QueryAsync(byte[] buffer);

        /// <summary>
        /// 异步请求回复
        /// </summary>
        /// <param name="message">文本消息</param>
        /// <returns></returns>
        Task<ResponsedData> QueryAsync(string message);
    }

    
}
