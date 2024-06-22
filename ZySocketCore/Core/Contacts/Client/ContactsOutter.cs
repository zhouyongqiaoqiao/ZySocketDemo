using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Client;
using ZySocketCore.Core.Contract;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;

namespace ZySocketCore.Core.Contacts.Client
{
    internal class ContactsOutter : IContactsOutter
    {
        private readonly ZyClientEngine clientEngine;
        public ContactsOutter(ZyClientEngine engine)
        {
            this.clientEngine = engine;
            this.clientEngine.Received += DataReceived;
        }

        private Task DataReceived(TcpClient client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                if (packageInfo.MessageType == (int)MessageType.CONTACT_ONLINE)
                {
                    UserContract userContract = SerializeConvert.JsonDeserializeFromBytes<UserContract>(packageInfo.Body);
                    if (userContract.IsAllClientType)
                        this.ContactsOnline?.Invoke(userContract.UserID);
                    else
                        this.ContactsDeviceConnected?.Invoke(userContract.UserID, userContract.ClientType);
                }
                else if (packageInfo.MessageType == (int)MessageType.CONTACT_OFFLINE)
                {
                    UserContract userContract = SerializeConvert.JsonDeserializeFromBytes<UserContract>(packageInfo.Body);
                    if (userContract.IsAllClientType)
                        this.ContactsOffline?.Invoke(userContract.UserID);
                    else this.ContactsDeviceDisconnected?.Invoke(userContract.UserID, userContract.ClientType);
                }
                else if (packageInfo.MessageType == (int)MessageType.CONTACT_BROADCAST)
                {
                    ContactMessage message = SerializeConvert.JsonDeserializeFromBytes<ContactMessage>(packageInfo.Body);
                    this.BroadcastReceived?.Invoke(packageInfo.UserID, packageInfo.ClientType, message.GroupID, message.InformationType, message.Content, message.Tag);
                }

                e.Handled = false;//表示该数据是否已经被本插件处理，无需再投递到其他插件。不影响 Received 的事件触发
            }
            return EasyTask.CompletedTask;
        }

        #region IContactsOutter
        public event Action<string, ClientType> ContactsDeviceConnected;
        public event Action<string, ClientType> ContactsDeviceDisconnected;
        public event Action<string> ContactsOnline;
        public event Action<string> ContactsOffline;
        public event Action<string, ClientType, string, int, byte[], string> BroadcastReceived;

        public void Broadcast(string groupID, int broadcastType, byte[] broadcastContent, string tag, ActionTypeOnChannelIsBusy action = ActionTypeOnChannelIsBusy.Continue)
        {
            ContactMessage message = new ContactMessage(groupID, broadcastType, broadcastContent, tag);
            var task = this.clientEngine.SendMessageAsync(this.clientEngine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.CONTACT_BROADCAST, SerializeConvert.JsonSerializeToBytes(message));
        }

        public List<string> GetAllOnlineContacts()
        {
            return this.GetContacts(true);   
        }

        public List<string> GetContacts()
        {
            return this.GetContacts(false);
        }

        public Groupmates GetGroupMembers(string groupID)
        {
            ResponsedData responsedData = this.clientEngine.Queryer.QueryMessage(this.clientEngine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.REQ_RESP_GetGroupMembers, Encoding.UTF8.GetBytes(groupID));
            ZyLightFixedHeaderPackageInfo packageInfo = responsedData.RequestInfo as ZyLightFixedHeaderPackageInfo;
            return SerializeConvert.JsonDeserializeFromBytes<Groupmates>(packageInfo.Body);            
        }
        #endregion

        private List<string> GetContacts(bool onlyOnline)
        {
            ResponsedData responsedData = this.clientEngine.Queryer.QueryMessage(this.clientEngine.CurrentUserID, SystemSettings.ServerDefaultId, (int)MessageType.REQ_RESP_CONTACT, BitConverter.GetBytes(onlyOnline));
            ZyLightFixedHeaderPackageInfo packageInfo = responsedData.RequestInfo as ZyLightFixedHeaderPackageInfo;
            return SerializeConvert.JsonDeserializeFromBytes<List<string>>(packageInfo.Body);
        }
    }
}
