using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Sockets;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Core.QueryInfo
{
    internal class WaitingClientTcp<TClient> : IQueryer where TClient : IReceiverObject, ISender
    {
        private readonly TClient _client;
        public WaitingClientTcp(TClient tcpClient)
        {
            this._client = tcpClient;
        }

        public int WaitResponseTimeoutInSecs { private get; set; } = 5;

        private IWaitingClient<TClient> waitingClient;
        private IWaitingClient<TClient> WaitingClient
        {
            get
            {
                if (this.waitingClient == null)
                {
                    this.waitingClient = this._client.CreateWaitingClient(new WaitingOptions()
                    {
                        FilterFunc = (responsedData) =>
                        {
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
            return this.WaitingClient.SendThenResponse(buffer, offset, length);
            ///TO DO : 未处理
        }

        public ResponsedData Query(byte[] buffer)
        {
            return this.Query(buffer, 0, buffer.Length);
        }

        public ResponsedData Query(string message)
        {
            return this.WaitingClient.SendThenResponse(message, this.WaitResponseTimeoutInSecs * 1000);
        }

        public Task<ResponsedData> QueryAsync(byte[] buffer, int offset, int length)
        {
            return this.WaitingClient.SendThenResponseAsync(buffer, offset, length, this.WaitResponseTimeoutInSecs * 1000);
        }

        public Task<ResponsedData> QueryAsync(byte[] buffer)
        {
            return this.QueryAsync(buffer, 0, buffer.Length);
        }

        public Task<ResponsedData> QueryAsync(string message)
        {
            return this.WaitingClient.SendThenResponseAsync(message, this.WaitResponseTimeoutInSecs * 1000);
        }
    }
}
