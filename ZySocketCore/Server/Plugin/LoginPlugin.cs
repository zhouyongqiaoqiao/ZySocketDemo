using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Core;
using ZySocketCore.Core.Contract;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;
using ZySocketCore.Utils;

namespace ZySocketCore.Server.Plugin
{
    internal class LoginPlugin : PluginBase, ITcpReceivedPlugin
    {
        private readonly IBasicHandler _basicHandler;

        public LoginPlugin(IBasicHandler basicHandler, ILoggerObject logger)
        {
            _basicHandler = basicHandler;
        }


        public Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.REQ_OR_RESP_LOGON)
                {
                    LogonRequest logonRequest = SerializeConvert.JsonDeserializeFromBytes<LogonRequest>(packageInfo.Body);

                    string failureCause = string.Empty;
                    bool success = _basicHandler.VerifyUser(logonRequest.SystemToken, packageInfo.UserID, logonRequest.Password, out failureCause);

                    LogonResponse logonResponse = new LogonResponse(success ? LogonResult.Succeed : LogonResult.Failed, failureCause);
                    if (success)
                    {
                        SocketClient socketClient = client as SocketClient;
                        GlobalUtil.ResetUserId(socketClient, packageInfo.FullUserID);
                    }

                    //this._basicHandler.HandleLogon(client, packageInfo.Data);
                    client.SendMessage(SystemSettings.ServerDefaultId, packageInfo.UserID, (int)MessageType.QUERY_RESPONSE, SerializeConvert.JsonSerializeToBytes(logonResponse));
                    return Task.CompletedTask;
                }
                else if (packageInfo.MessageType == (int)MessageType.ReConnected)
                {
                    SocketClient socketClient = client as SocketClient;
                    GlobalUtil.ResetUserId(socketClient, packageInfo.FullUserID);
                    return Task.CompletedTask;
                }
                else if (packageInfo.MessageType == (int)MessageType.QueryBeforeLogin)
                {
                    QueryBeforeLoginContract contract = SerializeConvert.JsonDeserializeFromBytes<QueryBeforeLoginContract>(packageInfo.Body);
                    
                    string result = _basicHandler.HandleQueryBeforeLogin(new IPHost($"{client.IP}:{client.Port}"), contract.QueryType, contract.QueryData);
                    client.SendMessage(SystemSettings.ServerDefaultId, packageInfo.UserID, (int)MessageType.QUERY_RESPONSE,Encoding.UTF8.GetBytes(result));
                    return Task.CompletedTask;
                }

            }
            return e.InvokeNext();
        }
    }
}
