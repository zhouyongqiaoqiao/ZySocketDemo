using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZyLightTouchSocketCore.Core;
using ZyTouchSocketCore;

namespace TouchSocketServer
{
    internal class ZyFixedHeaderAdapterPlugin : PluginBase, ITcpReceivedPlugin
    {
        public Task OnTcpReceived(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is ZyLightFixedHeaderPackageInfo packageInfo)
            {
                string msg = Encoding.UTF8.GetString(packageInfo.Body);
                client.Logger.Info($"已从{client.IP}接收到信息：{packageInfo.ToString()}-- {packageInfo.ToJsonString()}-- {msg}");
                Console.WriteLine($"已从{client.IP}接收到信息：{packageInfo.ToString()}-- {packageInfo.ToJsonString()}-- {msg}");
                //if (packageInfo.MessageType == 0) {
                //    client.SendMessageAsync(0,  Encoding.UTF8.GetBytes("Hello, I am server. Answer is 3"));
                
                //}
                e.Handled =true;
                return Task.CompletedTask;
            }
            return e.InvokeNext();//如果本插件无法处理当前数据，请将数据转至下一个插件。
        }
    }
}
