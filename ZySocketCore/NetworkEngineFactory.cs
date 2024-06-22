using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Client;
using ZySocketCore.Client.Plugin;
using ZySocketCore.Core;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;
using ZySocketCore.Server;

namespace ZySocketCore
{
    public class NetworkEngineFactory
    {
        /// <summary>
        /// 创建使用二进制协议的TCP服务端引擎。对于返回的引擎实例，可以设置其更多属性，然后调用其Initialize方法启动引擎。
        /// </summary>
        /// <param name="port">服务端引擎监听的端口号</param>
        /// <param name="helper">二进制协议助手接口</param>        
        public static IZyServerEngine CreateStreamTcpServerEngine()
        {
            ZyTcpServiceEngine engine = new ZyTcpServiceEngine();

            return engine;
        }


        /// <summary>
        /// 创建使用二进制协议的TCP客户端引擎。对于返回的引擎实例，可以设置其更多属性，然后调用其Initialize方法启动引擎。
        /// </summary>        
        /// <param name="serverIP">要连接的服务器的IP</param> 
        /// <param name="serverPort">要连接的服务器的端口</param> 
        /// <param name="helper">二进制协议助手接口</param>        
        public static IZyClientEngine CreateStreamTcpClientEngine()
        {

            ZyTcpClientEngine clientEngine = new ZyTcpClientEngine();

            clientEngine.Setup(new TouchSocketConfig()
                    .SetTcpDataHandlingAdapter(() => new ZyLightFixedHeaderDataAdapter())
                    .ConfigurePlugins(plugin =>
                     {
                         //设置断线重连
                         plugin.UseReconnection(-1, false, 1000);
                         plugin.Add(new MessageReceivedPlugin(clientEngine));
                     })
                    //SetDataHandlingAdapter(() =>  new TerminatorPackageAdapter("\0") )
                    .ConfigureContainer(a =>
                    {
                        a.AddConsoleLogger();//添加一个日志注入
                        a.AddFileLogger();
                    }));

            return clientEngine;
        }

        /// <summary>
        /// 创建使用文本协议的TCP客户端引擎。对于返回的引擎实例，可以设置其更多属性，然后调用其Initialize方法启动引擎。
        /// 注意：返回的引擎实例，可以强转为ITextEngine接口。
        /// </summary>
        /// <param name="serverIP">要连接的服务器的IP</param> 
        /// <param name="serverPort">要连接的服务器的端口</param> 
        /// <param name="helper">文本协议助手接口</param>  
        public static ZyTextTcpClientEngine CreateTextTcpClientEngine(string serverIP, int serverPort)
        {
            ZyTextTcpClientEngine clientEngine = new ZyTextTcpClientEngine();

            clientEngine.Setup(new TouchSocketConfig().SetRemoteIPHost($"{serverIP}:{serverPort}")
                    //.SetTcpDataHandlingAdapter(() => new ZyLightFixedHeaderDataAdapter())
                    .SetTcpDataHandlingAdapter(() =>  new TerminatorPackageAdapter("\0") )
                    .ConfigurePlugins(plugin =>
                     {
                         plugin.UseReconnection(-1,false,1000);
                     })
                    .ConfigureContainer(a =>
                    {
                        a.AddConsoleLogger();//添加一个日志注入
                        a.AddFileLogger();
                    }));

            return clientEngine;
        }

        /// <summary>
        /// 创建UDP引擎。对于返回的引擎实例，可以设置其更多属性，然后调用其Initialize方法启动引擎。
        /// </summary>       
        public static UdpSession CreateUdpEngine() { return null; }

    }
}
