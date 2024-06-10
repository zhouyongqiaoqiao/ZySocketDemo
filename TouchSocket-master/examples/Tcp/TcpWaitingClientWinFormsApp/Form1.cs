using System.Diagnostics;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace TcpWaitingClientWinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
        }

        TcpClient m_tcpClient;

        private void IsConnected()
        {
            try
            {
                if (this.m_tcpClient?.Online == true)
                {
                    return;
                }
                this.m_tcpClient.SafeDispose();
                this.m_tcpClient = new TcpClient();

                //��������
                this.m_tcpClient.Setup(new TouchSocketConfig()
                    .SetRemoteIPHost(this.textBox1.Text));

                this.m_tcpClient.Connect();//�������ӣ������Ӳ��ɹ�ʱ�����׳��쳣��
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.IsConnected();
                IWaitingClient<TcpClient>? waitingClient = this.m_tcpClient.GetWaitingClient(new WaitingOptions()
                {
                    AdapterFilter = AdapterFilter.AllAdapter,
                    BreakTrigger = true,
                    ThrowBreakException = true
                });
                for (int i = 1; i >= 0; i--)
                {
                    TimeSpan timeSpan = TimeMeasurer.Run(() =>
                    {
                        var bytes = waitingClient.SendThenReturn(i.ToString().ToUTF8Bytes());
                        if (bytes != null)
                        {
                            Debug.WriteLine($"���ܷ��ص�����{Encoding.UTF8.GetString(bytes)}");
                            // MessageBox.Show($"�յ��ȴ����ݣ�{Encoding.UTF8.GetString(bytes)}");
                        }
                    });
                    this.m_tcpClient.SendAsync($"����tcpClient{i-1}".ToUTF8Bytes());
                    Debug.WriteLine($"{i}��ʱ��{timeSpan.TotalMilliseconds}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                this.IsConnected();
                var waitingClient = this.m_tcpClient.GetWaitingClient(new WaitingOptions()
                {
                    AdapterFilter = AdapterFilter.AllAdapter,
                    BreakTrigger = true,
                    ThrowBreakException = true
                },
                (response) =>
                {
                    if (response.Data != null)
                    {
                        var str = Encoding.UTF8.GetString(response.Data);
                        if (str.Contains(this.textBox4.Text))
                        {
                            Debug.WriteLine("����true");                            
                            return true;
                        }
                    }
                    Debug.WriteLine("����false");
                    return false; //����false�����˵�����false��
                });

                var bytes = waitingClient.SendThenReturn(this.textBox3.Text.ToUTF8Bytes());
                if (bytes != null)
                {
                    MessageBox.Show($"�յ��ȴ����ݣ�{Encoding.UTF8.GetString(bytes)}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}