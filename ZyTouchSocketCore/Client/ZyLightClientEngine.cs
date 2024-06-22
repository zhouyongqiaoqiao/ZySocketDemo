
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Core;
using ZyLightTouchSocketCore.Interface;
using ZyLightTouchSocketCore.Server;
using ZySocketCore;
using ZySocketCore.Core.Enum;

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
        protected IQueryer Queryer => this.queryer;

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
                    e.Handled = true;//表示该数据是否已经被本插件处理，无需再投递到其他插件。不影响 Received 的事件触发
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


        #region QueryMessage


        protected byte[] QueryMessage(int informationType, byte[] body)
        {
            try
            {
                return this.queryer.QueryCustomMessage(informationType, body);
            }
            catch (System.Exception ee)
            {
                base.Logger.Error(ee.Message);
            }
            return null;
        }

        protected Task<byte[]> QueryMessageAsync(int informationType, byte[] body)
        {
            try
            {
                return this.queryer.QueryCustomMessageAsync(informationType, body);
            }
            catch (System.Exception ee)
            {
                base.Logger.Error(ee.Message);
            }
            return null;
        }

        protected string QueryMessage(int informationType, string msg)
        {
            try
            {
                return this.queryer.QueryCustomMessage(informationType, msg);
            }
            catch (System.Exception ee)
            {
                base.Logger.Error(ee.Message);
            }
            return null;
        }

        protected Task<string> QueryMessageAsync(int informationType, string msg)
        {
            try
            {
                return this.queryer.QueryCustomMessageAsync(informationType, msg);
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
                            if (responsedData.Data?.Length > 0)
                            {
                                return true;
                            }
                            if (responsedData.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
                            {
                                if (packageInfo.MessageType == (int)MessageType.QUERY_RESPONSE)
                                {
                                    return true;
                                }
                            }
                            return false;
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
