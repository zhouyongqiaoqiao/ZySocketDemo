
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
           // customMessage.Content = Encoding.UTF8.GetBytes("�Զ�����Ϣ����");
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
                tcpClient.Connecting = (client, e) => { this.WriteInfo($"{client.IP}��������"); return EasyTask.CompletedTask; };//�������ӵ�����������ʱ�Ѿ�����socket�����ǻ�δ����tcp
                tcpClient.Connected = (client, e) => { this.WriteInfo($"{client.IP}������"); return EasyTask.CompletedTask; };//�ɹ����ӵ�������
                tcpClient.Disconnecting = (client, e) => { this.WriteInfo($"{client.IP}���ڶϿ�"); return EasyTask.CompletedTask; };//�����ӷ������Ͽ����ӡ��˴��������Ͽ�����Ч��
                tcpClient.Disconnected = (client, e) => { this.WriteInfo($"{client.IP}�ѶϿ�"); return EasyTask.CompletedTask; };//�ӷ������Ͽ����ӣ������Ӳ��ɹ�ʱ���ᴥ����
                tcpClient.Received += (TcpClient client, ReceivedDataEventArgs e) =>
                {
                    //�ӷ������յ���Ϣ������һ��byteBlock��requestInfo��������������ֲ�ͬ��ֵ��
                    string mes = "";
                    if (e.ByteBlock?.Len > 0)
                    {
                        mes = Encoding.UTF8.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len);
                    }
                    else { 
                        mes = e.RequestInfo.ToJsonString();
                    }
                    client.Logger.Info($"�ͻ��˽��յ���Ϣ��{mes}");
                    this.WriteInfo($"�յ���Ϣ��{mes}");
                    return EasyTask.CompletedTask;
                };
                tcpClient.CustomizeHandler = this;
                tcpClient.Connect();//�������ӣ������Ӳ��ɹ�ʱ�����׳��쳣��
                this.WriteInfo($"����{tcpClient.GetIPPort()}�ɹ���");
            }
            catch (Exception ee)
            {
                MessageBox.Show($"����ʧ�ܣ�{ee.Message}");
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
                    //tcpClient.Logger.Info($"�յ�������Ϣ��{Encoding.UTF8.GetString(res)}");
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
                    this.WriteInfo($"_�յ�������Ϣ��{ Encoding.UTF8.GetString(res)}");
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
            this.WriteInfo("�յ���Ϣ��" + informationType + "_" + Encoding.UTF8.GetString(info));
        }

        public byte[] HandleQuery(int informationType, byte[] info)
        {
            return Encoding.UTF8.GetBytes($"�ͻ��˻ظ�����˷��͹����Ĳ�ѯ��Ϣ");
        } 
        #endregion
    }
}