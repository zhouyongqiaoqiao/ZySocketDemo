using System;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Core;
using ZySocketCore.Core.Contract;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;

namespace ZySocketCore.Server.Plugin
{
    internal class BlobAndTagMessagePlugin : PluginBase, ITcpReceivedPlugin
    {
        private readonly IZyServerEngine server;
        public event Action<string, ClientType, int, byte[], string> MessageReceived;

        public BlobAndTagMessagePlugin(IZyServerEngine serverEngine)
        {
            server = serverEngine;
        }

        public Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.BLOB_TAG)
                {
                    BlobAndTagContract blobAndTagContract = SerializeConvert.FastBinaryDeserialize<BlobAndTagContract>(packageInfo.Body);

                }
            }
            return e.InvokeNext();
        }
    }
}
