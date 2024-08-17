## 概述

ZySocket 底层是基于 [TouchSocket](https://gitee.com/RRQM_Home/TouchSocket) 而设计，为了减少在项目构架中的时间，使用了本人常用的几种数据处理适配器，它功能强大、简单，易用，且灵活。

它兼容性强能够兼容基于TCP、UDP协议的所有协议，解决了数据分包、粘包的问题，同时稳定性、并发性高。

## 亮点

除了支持TouchSocket的基础功能外，ZySocket 增加了心跳检测、断线重连、登录验证、在线联系人、好友、群组通信（将消息发送到指定的账号或群组），同时也支持多端登录（同一账号在PC、Web、Android 、IOS）等功能，让通信变得更简单。

## 支持环境

.NET Framework4.5及以上

.NET 6.0及以上

.NET Standard2.0及以上

## 简单示例

【服务端】

```typescript
//创建ZySocketServer的实例
IZyServerEngine serverEngine =NetworkEngineFactory.CreateStreamTcpServerEngine();
//预定消息接收事件
serverEngine.MessageReceived += ServerEngine_MessageReceived;
//设置联系人管理器
serverEngine.ContactsManager = new CustomizeContactManager();

void ServerEngine_MessageReceived(string sourceUserID, ClientType clientType, int informationType, byte[] blob, string tag)
{
    Console.WriteLine($"来自{sourceUserID}的{clientType}消息：{informationType}_{Encoding.UTF8.GetString(blob)}, tag:{tag}");
    //发送消息给指定的用户
	serverEngine.SendMessage(sourceUserID, informationType,  blob, tag);
}

//初始化并启动服务
serverEngine.Initialize(4530, new CustomizeHandler(),new BasicHandler());
```

【客户端】

```typescript
	IZyClientEngine clientEngine = NetworkEngineFactory.CreateStreamTcpClientEngine();
	this.clientEngine.MessageReceived += ClientEngine_MessageReceived;

    //初始化并登陆到服务器
    LogonResponse res = this.clientEngine.Initialize(this.txb_userID.Text, this.txb_pwd.Text, ip, port, this);
    if (res.LogonResult == LogonResult.Succeed)
    {
         this.Text = $"当前用户：{this.txb_userID.Text} - {ip}:{port}";
         this.WriteInfo($"{ip}:{port}登录成功");
    }
    else
    {
         this.WriteInfo($"登录失败：{res.FailureCause}");
    }
	//发送消息给指定的用户或者服务端
	this.clientEngine.SendMessage(this.txb_destUserID.Text, 1, Encoding.UTF8.GetBytes(this.txb_msg.Text), "tagtest");
	
    private void ClientEngine_MessageReceived(string sourceUserID, ClientType clientType, int informationType, byte[] blob, string tag)
    {
        this.WriteInfo($"来自{sourceUserID}的{clientType}消息：{informationType}_{Encoding.UTF8.GetString(blob)}, tag:{tag}");
    }
```

【发送消息】

```typescript
        /// <summary>
        /// 向服务器或其它在线用户异步发送消息（当前调用线程立即返回）。如果其它用户不在线，消息将被丢弃。
        /// </summary>
        /// <param name="targetUserID">接收者的ID，如果为服务器，则传入null</param>
        /// <param name="informationType">消息类型</param>
        /// <param name="message">消息内容</param>
        /// <param name="tag">附加内容</param>
        /// <param name="destClientType">对方类型</param>
        void SendMessage(string targetUserID, int informationType, byte[] message, string tag, ClientType? destClientType = null);      
```

