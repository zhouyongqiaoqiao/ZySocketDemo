using System.Reflection.PortableExecutable;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;
using TouchSocketServer;
using ZyLightTouchSocketCore;
using ZyLightTouchSocketCore.Interface;
using ZyLightTouchSocketCore.Server;
using ZySocketCore;


string UserID = "123456";
string DestUserID = "654321";
string UserID2 = "";
string DestUserID2 = "";
int maxLengthOfUserID = 20;
byte[] data = new byte[1024];
using (ByteBlock byteBlock = new ByteBlock(data))
{
    byteBlock.Write(UserID.PadRight(maxLengthOfUserID));
    byteBlock.Write(DestUserID.PadRight(maxLengthOfUserID));
}

using (ByteBlock byteBlock = new ByteBlock(data)) 
{ 
     UserID2 = byteBlock.ReadString().Trim();
     DestUserID2 = byteBlock.ReadString().Trim();
}

    //WebSocketServer webSocketServer = new WebSocketServer();
    //webSocketServer.Start();


    //ZyLightTcpServiceEngine service = NetworkEngineFactory.CreateStreamTcpServerEngine(4530);
    ZyLightTcpServiceEngine service = NetworkEngineFactory.CreateStreamTcpServerEngine(4530);
service.Connecting = (client, e) => { Console.WriteLine($"{client.Id} 正在连接！");return EasyTask.CompletedTask; };
service.Connected = (client, e) => { Console.WriteLine($"{client.Id} 已连接！"
    );
    //client.Send($"发送消息给{client.Id}");
    //Console.WriteLine($"发送消息给{client.Id} 完成！");
 return EasyTask.CompletedTask; };
service.Disconnected = (client, e) => { Console.WriteLine($"{client.Id} 断开连接"); return EasyTask.CompletedTask; };

service.ServieCustomizeHandler = new ServieCustomizeHandler();

//service.Received = (SocketClient client, ReceivedDataEventArgs e) => {

//    if (e.RequestInfo != null) {
//        return EasyTask.CompletedTask;
//    }
//    //从客户端收到信息
//    string mes = Encoding.UTF8.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len);//注意：数据长度是byteBlock.Len
//    client.Logger.Info($"已从{client.Id}接收到信息：{mes}");

//    client.Send(mes);//将收到的信息直接返回给发送方

//    //client.Send("id",mes);//将收到的信息返回给特定ID的客户端

//    var ids = service.GetIds();
//    foreach (var clientId in ids)//将收到的信息返回给在线的所有客户端。
//    {
//        if (clientId != client.Id)//不给自己发
//        {
//            service.Send(clientId, mes);
//        }
//    }
//    return EasyTask.CompletedTask;
//};

//service.Setup(new TouchSocketConfig()    
//    .SetListenOptions(option => {
//        option.Add(new TcpListenOption()
//        {
//            IpHost = "127.0.0.1:4530",
//            Name = "Server1",
//            Adapter = ()=> new ZyLightFixedHeaderDataAdapter()
//        });
//        option.Add(new TcpListenOption()
//        {
//            IpHost = 4430,
//            Name = "Server2",
//            Adapter = ()=> new TerminatorPackageAdapter("\0")
//        });
//    })
//    .ConfigurePlugins((action) => {   
//        action.Add(new ZyFixedHeaderAdapterPlugin());
//    }));

service.Start();

string ports = string.Empty;
foreach (TcpNetworkMonitor item in service.Monitors)
{
    ports += item.Option.IpHost + " ;";
}

Console.WriteLine($"服务已启动，Port：{ports}");
Console.ReadLine();
service.Stop();
