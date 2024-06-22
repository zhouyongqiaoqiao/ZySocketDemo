// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using TouchSocket.Core;
using ZySocketCore;
using ZySocketCore.Core;
using ZySocketCore.Core.Contacts.Server;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;
using ZySocketServer;

//创建ZySocketServer的实例
IZyServerEngine serverEngine =NetworkEngineFactory.CreateStreamTcpServerEngine();
//预定消息接收事件
serverEngine.MessageReceived += ServerEngine_MessageReceived;
//设置联系人管理器
serverEngine.ContactsManager = new CustomizeContactManager();

void ServerEngine_MessageReceived(string sourceUserID, ClientType clientType, int informationType, byte[] blob, string tag)
{
    Console.WriteLine($"来自{sourceUserID}的{clientType}消息：{informationType}_{Encoding.UTF8.GetString(blob)}, tag:{tag}");
    serverEngine.SendMessage(sourceUserID, informationType,  blob, tag);
}

//初始化并启动服务
serverEngine.Initialize(4530, new CustomizeHandler(),new BasicHandler());


serverEngine.ContactsController.BroadcastReceived += ContactsController_BroadcastReceived;

void ContactsController_BroadcastReceived(string broadcasterID, string groupID, int broadcastType, byte[] broadcastContent, string tag)
{
    Console.WriteLine($"收到广播消息：{broadcasterID} {groupID} {broadcastType} {Encoding.UTF8.GetString(broadcastContent)} {tag}");
}

serverEngine.ContactsController.BroadcastFailed += ContactsController_BroadcastFailed;

void ContactsController_BroadcastFailed(string userID, BroadcastInformation information)
{
    Console.WriteLine($"发送广播失败：{userID} {information.GroupID} {information.BroadcastType} {Encoding.UTF8.GetString(information.Content)} {information.Tag}");
}
Console.WriteLine("server 4530 start");


DynamicGroupControllerTest dynamicGroupControllerTest = new DynamicGroupControllerTest(serverEngine.DynamicGroupController);

//serverEngine. = (client, e) => { Console.WriteLine($"{client.Id} 正在连接！"); return EasyTask.CompletedTask; };
//serverEngine.Connected = (client, e) => {
//    Console.WriteLine($"{client.Id} 已连接！"
//    );
//    //client.SendAsync($"发送消息给{client.Id}");
//    //Console.WriteLine($"发送消息给{client.Id} 完成！");
//    return EasyTask.CompletedTask;
//};
//serverEngine.Disconnected = (client, e) => { Console.WriteLine($"{client.Id} 断开连接"); return EasyTask.CompletedTask; };

ConsoleAction consoleAction = new ConsoleAction("h|help|?");//设置帮助命令
consoleAction.OnException += ConsoleAction_OnException;//订阅执行异常输出

void ConsoleAction_OnException(Exception exception)
{
    Console.WriteLine(exception.ToString());
}
consoleAction.Add("bro|broadcast", "群内广播", Broadcast);//示例命令

void Broadcast()
{
    serverEngine.ContactsController.BroadcastAsync("g01", 1001, Encoding.UTF8.GetBytes("服务端群广播测试"), "tag");
    Console.WriteLine("广播完成！");
}
consoleAction.ShowAll();
Console.ReadLine();
