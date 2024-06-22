using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Core.Contacts.Server
{
    public interface IContactsController
    {
        /// <summary>
        /// 是否监听来自客户端的Blob广播，默认值为false。如果为true，即使客户端广播的是blob消息，也将触发BroadcastReceived事件（将需要更多的内存）。
        /// </summary>
        bool BroadcastBlobListened { get; set; }

        /// <summary>
        /// 用户下线时，是否通知相关联系人。默认值为true。
        /// </summary>
        bool ContactsDisconnectedNotifyEnabled { get; set; }

        /// <summary>
        /// 用户上线时，是否通知相关联系人。默认值为true。
        /// </summary>
        bool ContactsConnectedNotifyEnabled { get; set; }

        /// <summary>
        /// 联系人上下线通知是否使用单独的线程。默认值为false。
        /// </summary>
        //bool UseContactsNotifyThread { get; set; }

        /// <summary>
        /// 当服务端接收到要转发的广播消息时（BroadcastBlobListened属性决定了是否包括blob广播），触发此事件。参数：broadcasterID- groupID - broadcastType - broadcastContent - tag
        /// </summary>
        event Action<string, string, int, byte[], string> BroadcastReceived;

        /// <summary>
        /// 当因为某个组成员不在线而导致对其广播失败时（不包括Blob及其片段信息），将触发该事件。参数：UserID（组成员ID） - BroadcastInformation。
        /// </summary>
        event Action<string, BroadcastInformation> BroadcastFailed;

        /// <summary>
        /// 在组内广播信息。
        /// </summary>
        /// <param name="groupID">接收广播信息的组ID</param>
        /// <param name="broadcastType">广播信息的类型</param>
        /// <param name="broadcastContent">广播的内容</param>
        /// <param name="tag">附加信息</param>
        // <param name="action">当通道繁忙时采取的操作</param>
        Task BroadcastAsync(string groupID, int broadcastType, byte[] broadcastContent, string tag);//ActionTypeOnChannelIsBusy action

        /// <summary>
        /// 在组内广播大数据块信息。直到数据发送完毕，该方法才会返回。如果担心长时间阻塞调用线程，可考虑异步调用本方法。
        /// </summary>
        /// <param name="groupID">接收广播信息的组ID</param>
        /// <param name="broadcastType">广播信息的类型</param>
        /// <param name="blobContent">大数据块的内容</param>
        /// <param name="tag">附加信息</param>
        /// <param name="fragmentSize">分片传递时，片段的大小</param>
        void BroadcastBlob(string groupID, int broadcastType, byte[] blobContent, string tag, int fragmentSize);

        //
        // 摘要:
        //     在组内广播大数据块信息。直到数据发送完毕，该方法才会返回。如果担心长时间阻塞调用线程，可考虑异步调用本方法。
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
        //
        //   handler:
        //     当发送任务结束时，将回调该处理器
        //
        //   handlerTag:
        //     将回传给ResultHandler的参数
        //void BroadcastBlob(string groupID, int broadcastType, byte[] blobContent, string tag, int fragmentSize, ResultHandler handler, object handlerTag);
    }
}
