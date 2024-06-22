using System;
using System.Collections.Generic;
using System.Text;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Core.Contacts.Client
{
    //
    // 摘要:
    //     用于客户端发送与联系人操作相关的信息和广播。
    public interface IContactsOutter
    {
        /// <summary>
        /// 当联系人的某设备上线时，触发该事件。参数：UserID
        /// </summary>        
        event Action<string, ClientType> ContactsDeviceConnected;
        
        /// <summary>
        /// 当联系人的某设备下线时，触发该事件。参数：UserID
        /// </summary>
        event Action<string, ClientType> ContactsDeviceDisconnected;

        /// <summary>
        /// 当联系人第一个设备上线时，触发该事件。参数：UserID
        /// </summary>
        event Action<string> ContactsOnline;


        /// <summary>
        /// 当联系人的所有设备都下线时，触发该事件。参数：UserID
        /// </summary>
        event Action<string> ContactsOffline;
 

        /// <summary>
        /// 当接收到某个组内的广播消息（包括大数据块信息）时，触发此事件。参数：broadcasterID - broadcasterClientType - groupID - broadcastType - broadcastContent - tag。 如果broadcasterID为null，表示是服务端发送的广播。
        /// </summary>
        event Action<string, ClientType, string, int, byte[], string> BroadcastReceived;

        //
        // 摘要:
        //     在组内广播信息。
        //
        // 参数:
        //   groupID:
        //     接收广播信息的组ID
        //
        //   broadcastType:
        //     广播信息的类型
        //
        //   broadcastContent:
        //     信息的内容
        //
        //   tag:
        //     附加信息
        //
        //   action:
        //     当通道繁忙时采取的操作。
        void Broadcast(string groupID, int broadcastType, byte[] broadcastContent, string tag, ActionTypeOnChannelIsBusy action = ActionTypeOnChannelIsBusy.Continue);
        //
        // 摘要:
        //     在组内广播大数据块信息。直到数据发送完毕，该方法才会返回。若不想阻塞调用线程，可考虑使用异步广播重载方法。
        //
        // 参数:
        //   groupID:
        //     接收广播信息的组ID
        //
        //   broadcastType:
        //     广播信息的类型
        //
        //   blobContent:
        //     大数据块的内容
        //
        //   tag:
        //     附加信息
        //
        //   fragmentSize:
        //     分片传递时，片段的大小
        //void BroadcastBlob(string groupID, int broadcastType, byte[] blobContent, string tag, int fragmentSize);
     
        //
        // 摘要:
        //     获取所有在线的联系人。
        List<string> GetAllOnlineContacts();
        //
        // 摘要:
        //     获取联系人列表。
        List<string> GetContacts();

        //
        // 摘要:
        //     获取组的成员。
        Groupmates GetGroupMembers(string groupID);
    }
}
