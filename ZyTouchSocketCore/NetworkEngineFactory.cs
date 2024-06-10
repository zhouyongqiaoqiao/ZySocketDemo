using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Client;
using ZyLightTouchSocketCore.Core;
using ZyLightTouchSocketCore.Server;

namespace ZyLightTouchSocketCore
{
    public class NetworkEngineFactory
    {
        /// <summary>
        /// 创建使用二进制协议的TCP服务端引擎。对于返回的引擎实例，可以设置其更多属性，然后调用其Initialize方法启动引擎。
        /// </summary>
        /// <param name="port">服务端引擎监听的端口号</param>
        /// <param name="helper">二进制协议助手接口</param>        
        public static ZyLightTcpServiceEngine CreateStreamTcpServerEngine(int port)
        {
            ZyLightTcpServiceEngine engine = new ZyLightTcpServiceEngine(ZyTouchSocketCore.Core.Enum.ContractFormatStyle.Stream);
            engine.Setup(new TouchSocketConfig()
                .SetListenOptions(option =>
                {
                    option.Add(new TcpListenOption()
                    {
                        IpHost = port,
                        Adapter = () => new ZyLightFixedHeaderDataAdapter()
                    });

                })
                );
            return engine;
        }

        /// <summary>
        /// 创建使用文本协议的TCP服务端引擎。对于返回的引擎实例，可以设置其更多属性，然后调用其Initialize方法启动引擎。
        /// 注意：返回的引擎实例，可以强转为ITextEngine接口。
        /// </summary>
        /// <param name="port">服务端引擎监听的端口号</param>
        /// <param name="helper">文本协议助手接口</param>  
        public static ZyLightTcpServiceEngine CreateTextTcpServerEngine(int port)
        {
            ZyLightTcpServiceEngine engine = new ZyLightTcpServiceEngine(ZyTouchSocketCore.Core.Enum.ContractFormatStyle.Text);

            engine.Setup(new TouchSocketConfig()
                .SetListenOptions(option =>
                {
                    option.Add(new TcpListenOption()
                    {
                        IpHost = port,
                        Adapter = () => new TerminatorPackageAdapter("\0")
                    });

                })
                );
            return engine;
        }

        /// <summary>
        /// 创建使用二进制协议的TCP客户端引擎。对于返回的引擎实例，可以设置其更多属性，然后调用其Initialize方法启动引擎。
        /// </summary>        
        /// <param name="serverIP">要连接的服务器的IP</param> 
        /// <param name="serverPort">要连接的服务器的端口</param> 
        /// <param name="helper">二进制协议助手接口</param>        
        public static ZyLightTcpClientEngine CreateStreamTcpClientEngine(string serverIP, int serverPort)
        {

            ZyLightTcpClientEngine clientEngine = new ZyLightTcpClientEngine();

            clientEngine.Setup(new TouchSocketConfig().SetRemoteIPHost($"{serverIP}:{serverPort}")
                    .SetTcpDataHandlingAdapter(() => new ZyLightFixedHeaderDataAdapter())
                    .ConfigurePlugins(plugin =>
                     {
                         //设置断线重连
                         plugin.UseReconnection(-1, false, 1000);
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
        public static ZyLightTextTcpClientEngine CreateTextTcpClientEngine(string serverIP, int serverPort)
        {
            ZyLightTextTcpClientEngine clientEngine = new ZyLightTextTcpClientEngine();

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
