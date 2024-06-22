
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore;
using ZyLightTouchSocketCore.Client;
using ZyLightTouchSocketCore.Interface;

namespace TouchSocketClient_Win
{
    public partial class Form1 : Form, ICustomizeHandler
    {
        ZyLightTcpClientEngine tcpClient;

        public Form1()
        {
            InitializeComponent();

           // CustomMessage customMessage = new CustomMessage();
           // customMessage.InformationType = 1;
           // customMessage.Content = Encoding.UTF8.GetBytes("自定义消息内容");
           // var data =  SerializeConvert.ToJsonString(customMessage);
           // var customMessage2 = SerializeConvert.FromJsonString<CustomMessage>(data);

           //var data2 =  SerializeConvert.BinarySerialize(customMessage);

      
        }

        private void WriteInfo(string info)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(this.WriteInfo), info);
            }
            else
            {
                this.txb_info.AppendText(info + "\r\n");
                this.ScrollEnd();
            }
        }

        private DateTime lastSrollTime;
        private void ScrollEnd()
        {
            if ((DateTime.Now - lastSrollTime).TotalMilliseconds > 1000)
            {
                this.txb_info.ScrollToCaret();
                lastSrollTime = DateTime.Now;
            }            
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                tcpClient = NetworkEngineFactory.CreateStreamTcpClientEngine(this.txb_ip.Text,int.Parse(this.txb_port.Text));
                tcpClient.Connecting = (client, e) => { this.WriteInfo($"{client.IP}正在连接"); return EasyTask.CompletedTask; };//即将连接到服务器，此时已经创建socket，但是还未建立tcp
                tcpClient.Connected = (client, e) => { this.WriteInfo($"{client.IP}已连接"); return EasyTask.CompletedTask; };//成功连接到服务器
                tcpClient.Disconnecting = (client, e) => { this.WriteInfo($"{client.IP}正在断开"); return EasyTask.CompletedTask; };//即将从服务器断开连接。此处仅主动断开才有效。
                tcpClient.Disconnected = (client, e) => { this.WriteInfo($"{client.IP}已断开"); return EasyTask.CompletedTask; };//从服务器断开连接，当连接不成功时不会触发。
                tcpClient.Received += (TcpClient client, ReceivedDataEventArgs e) =>
                {
                    //从服务器收到信息。但是一般byteBlock和requestInfo会根据适配器呈现不同的值。
                    string mes = "";
                    if (e.ByteBlock?.Len > 0)
                    {
                        mes = Encoding.UTF8.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len);
                    }
                    else { 
                        mes = e.RequestInfo.ToJsonString();
                    }
                    client.Logger.Info($"客户端接收到信息：{mes}");
                    this.WriteInfo($"收到信息：{mes}");
                    return EasyTask.CompletedTask;
                };
                tcpClient.CustomizeHandler = this;
                tcpClient.Connect();//调用连接，当连接不成功时，会抛出异常。
                this.WriteInfo($"连接{tcpClient.GetIPPort()}成功！");
            }
            catch (Exception ee)
            {
                MessageBox.Show($"连接失败，{ee.Message}");
            }

        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txb_msg.Text))
            {
                for (int i = 1; i > 0; i--)
                {
                    //tcpClient.SendMessageToServerAsync(i, i.ToString());
                    tcpClient.SendMessageToServerAsync(i, Encoding.UTF8.GetBytes(i.ToString()));
        
                    
                    //byte[]? res = waitingClient.SendThenReturn(Encoding.UTF8.GetBytes(i.ToString()), 5000, CancellationToken.None);
                    //tcpClient.Logger.Info($"收到返回消息：{Encoding.UTF8.GetString(res)}");
                }
            }
            else
            {
                tcpClient.Send(this.txb_msg.Text + "\0");
            }
        }

        private async void button_query_Click(object sender, EventArgs e)
        {
            try
            {
                //for (int i = 100; i > 0; i--)
                //{
                    //string res = await this.tcpClient.QueryMessageFromServerAsync(i, "1+2"); 
                    byte[] res = await this.tcpClient.QueryMessageFromServerAsync(1234, Encoding.UTF8.GetBytes("1+2"));
                    this.WriteInfo($"_收到返回消息：{ Encoding.UTF8.GetString(res)}");
                //}

            }
            catch (Exception ee)
            {
                this.WriteInfo(ee.Message);
            }

        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            if (this.tcpClient.Online)
            {
                this.tcpClient.SafeDispose();
            }
        }

        #region ICustomizeHandler
        public void HandleInformation(int informationType, byte[] info)
        {
            this.WriteInfo("收到消息：" + informationType + "_" + Encoding.UTF8.GetString(info));
        }

        public byte[] HandleQuery(int informationType, byte[] info)
        {
            return Encoding.UTF8.GetBytes($"客户端回复服务端发送过来的查询消息");
        } 
        #endregion
    }
}