//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在XREF结尾的命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：http://rrqm_home.gitee.io/touchsocket/
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TouchSocket.Core;

namespace TouchSocket.Sockets
{
    /// <summary>
    /// 延迟发送器
    /// </summary>
    public sealed class DelaySender : DisposableObject
    {
        private readonly ReaderWriterLockSlim m_lockSlim;
        private readonly Action<Exception> m_onError;
        private readonly IntelligentDataQueue<QueueDataBytes> m_queueDatas;
        private readonly Socket m_socket;
        private volatile bool m_sending;

        /// <summary>
        /// 延迟发送器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queueLength"></param>
        /// <param name="onError"></param>
        public DelaySender(Socket socket, int queueLength, Action<Exception> onError)
        {
            this.m_socket = socket;
            this.m_onError = onError;
            this.m_queueDatas = new IntelligentDataQueue<QueueDataBytes>(queueLength);
            this.m_lockSlim = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// 延迟包最大尺寸，默认1024*512字节。
        /// </summary>
        public int DelayLength { get; set; } = 1024 * 512;

        /// <summary>
        /// 是否处于发送状态
        /// </summary>
        public bool Sending
        {
            get
            {
                using (new ReadLock(this.m_lockSlim))
                {
                    return this.m_sending;
                }
            }

            private set
            {
                using (new WriteLock(this.m_lockSlim))
                {
                    this.m_sending = value;
                }
            }
        }

        /// <summary>
        /// 发送
        /// </summary>
        public void Send(QueueDataBytes dataBytes)
        {
            this.m_queueDatas.Enqueue(dataBytes);
            if (this.SwitchToRun())
            {
                Task.Factory.StartNew(this.BeginSend);
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            this.m_queueDatas.Clear();
            base.Dispose(disposing);
        }

        private void BeginSend()
        {
            try
            {
                var buffer = BytePool.Default.Rent(this.DelayLength);
                while (!this.DisposedValue)
                {
                    try
                    {
                        if (this.TryGet(buffer, out var asyncByte))
                        {
                            this.m_socket.AbsoluteSend(asyncByte.Buffer, asyncByte.Offset, asyncByte.Length);
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.m_onError?.Invoke(ex);
                        break;
                    }
                }
                BytePool.Default.Return(buffer);
                this.Sending = false;
            }
            catch
            {
            }
        }

        private bool SwitchToRun()
        {
            using (new WriteLock(this.m_lockSlim))
            {
                if (this.m_sending)
                {
                    return false;
                }
                else
                {
                    this.m_sending = true;
                    return true;
                }
            }
        }

        private bool TryGet(byte[] buffer, out QueueDataBytes asyncByteDe)
        {
            var len = 0;
            var surLen = buffer.Length;
            while (true)
            {
                if (this.m_queueDatas.TryPeek(out var asyncB))
                {
                    if (surLen > asyncB.Length)
                    {
                        if (this.m_queueDatas.TryDequeue(out var asyncByte))
                        {
                            Array.Copy(asyncByte.Buffer, asyncByte.Offset, buffer, len, asyncByte.Length);
                            len += asyncByte.Length;
                            surLen -= asyncByte.Length;
                        }
                    }
                    else if (asyncB.Length > buffer.Length)
                    {
                        if (len > 0)
                        {
                            break;
                        }
                        else
                        {
                            asyncByteDe = asyncB;
                            return true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (len > 0)
                    {
                        break;
                    }
                    else
                    {
                        asyncByteDe = default;
                        return false;
                    }
                }
            }
            asyncByteDe = new QueueDataBytes(buffer, 0, len);
            return true;
        }
    }
}