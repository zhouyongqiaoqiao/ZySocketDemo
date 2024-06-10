using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;

TcpClient tcpClient = new TcpClient();
tcpClient.Connecting = (client, e) => { Console.WriteLine($"{client.IP}正在连接");return Task.Delay(0); };//即将连接到服务器，此时已经创建socket，但是还未建立tcp
tcpClient.Connected = (client, e) => { Console.WriteLine($"{client.IP}已连接"); return Task.Delay(0); };//成功连接到服务器
tcpClient.Disconnecting = (client, e) => { Console.WriteLine($"{client.IP}正在断开"); return Task.Delay(0); };//即将从服务器断开连接。此处仅主动断开才有效。
tcpClient.Disconnected = (client, e) => { Console.WriteLine($"{client.IP}已断开"); return Task.Delay(0); };//从服务器断开连接，当连接不成功时不会触发。
 

tcpClient.Received += (TcpClient client, ReceivedDataEventArgs e) =>
{
    //从服务器收到信息。但是一般byteBlock和requestInfo会根据适配器呈现不同的值。
    string mes = Encoding.UTF8.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len);
    client.Logger.Info($"客户端接收到信息：{mes}");
    return Task.Delay(0);
};

//载入配置
tcpClient.Setup(new TouchSocketConfig()
    .SetRemoteIPHost("127.0.0.1:4530")
    .SetTcpDataHandlingAdapter(() =>  new FixedHeaderPackageAdapter() )
    //.SetDataHandlingAdapter(() =>  new TerminatorPackageAdapter("\0") )
    .ConfigureContainer(a =>
    {
        a.AddConsoleLogger();//添加一个日志注入
    }));

tcpClient.Connect();//调用连接，当连接不成功时，会抛出异常。

var waitingClient = tcpClient.CreateWaitingClient(new WaitingOptions());
try
{
    for (int i = 100 ; i > 0; i--)
    {
        tcpClient.SendAsync(i.ToString());
        //byte[]? res = waitingClient.SendThenReturn(Encoding.UTF8.GetBytes(i.ToString()), 5000, CancellationToken.None);
        //tcpClient.Logger.Info($"收到返回消息：{Encoding.UTF8.GetString(res)}");
    }

}
catch (Exception ee)
{
    tcpClient.Logger.Error(ee.Message);
}


//Result result = tcpClient.TryConnect();//或者可以调用TryConnect
//if (result.IsSuccess())
//{

//}

tcpClient.Logger.Info("客户端成功连接");

while (true)
{
    tcpClient.Send(Console.ReadLine());
}

