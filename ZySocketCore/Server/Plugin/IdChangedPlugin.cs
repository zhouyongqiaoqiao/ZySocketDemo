using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using ZySocketCore.Manager;

namespace ZySocketCore.Server.Plugin
{
    internal class IdChangedPlugin : PluginBase, IIdChangedPlugin
    {
        public Task OnIdChanged(IClient client, IdChangedEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
