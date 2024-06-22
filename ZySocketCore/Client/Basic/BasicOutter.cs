using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Core;
using ZySocketCore.Core.Contract;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;

namespace ZySocketCore.Client.Basic
{
    internal class BasicOutter: IBasicOutter
    {
        private readonly ZyClientEngine _engine;

        public BasicOutter(ZyClientEngine engine)
        {
            _engine = engine;
            this._engine.Received += DataReceived;
        }

        private Task DataReceived(TcpClient client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.BE_FORCED_OUT_NOTIFY)
                {
                    this.BeingPushedOut?.Invoke();
                    client.Close();
                }
                else if (packageInfo.MessageType == (int)MessageType.BE_KICKED_OUT_NOTIFY)
                {
                    this.BeingKickedOut?.Invoke();
                    client.Close();
                }
                else if (packageInfo.MessageType == (int)MessageType.DeviceOnOfflineNotify)
                {
                    DeviceOnOfflineContract deviceType = SerializeConvert.JsonDeserializeFromBytes<DeviceOnOfflineContract>(packageInfo.Body);
                    if (deviceType.IsOnline)
                    {
                        this.MyDeviceOnline?.Invoke(deviceType.ClientType);
                    }
                    else
                    {
                        this.MyDeviceOffline?.Invoke(deviceType.ClientType);
                    }
                }
                /// TODO: 处理设备上线下线通知 

                //else if (packageInfo.MessageType == (int)MessageType.BE_KICKED_OUT_NOTIFY)
                //{

                //}

                e.Handled = false;//表示该数据是否已经被本插件处理，无需再投递到其他插件。不影响 Received 的事件触发
            }
            return EasyTask.CompletedTask;
        }

        #region IBasicOutter
        public event Action BeingPushedOut;
        public event Action BeingKickedOut;
        public event Action<ClientType> MyDeviceOnline;
        public event Action<ClientType> MyDeviceOffline;

        public IPHost GetMyIPE()
        {
            return new IPHost( _engine.MainSocket.LocalEndPoint.ToString());
        }

        public List<ClientType> GetMyOnlineDevice()
        {
            ResponsedData message = this._engine.Queryer.QueryMessage(this._engine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.GetMyOnlineDevice, null);
            if (message.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                return SerializeConvert.JsonDeserializeFromBytes<List<ClientType>>(packageInfo.Body);
            }
            return null;
        }

        public void KickOut(string targetUserID)
        {
            this._engine.Queryer.QueryMessage(this._engine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.REQ_KICK_OUT, Encoding.UTF8.GetBytes(targetUserID));
        }

        public int Ping()
        {
            return -1;
        }

        public void SendHeartBeatMessage()
        {
            if (!this._engine.Online) return;
            this._engine.SendMessageAsync(this._engine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.REQ_HEART_BEAT, new byte[0]);
        } 
        #endregion
    }
}
