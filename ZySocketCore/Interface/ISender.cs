using System;
using System.Collections.Generic;
using System.Text;
using TouchSocket.Sockets;

namespace ZySocketCore.Interface
{
    internal interface ISenderById : ISender
    {
        string Id { get; }

        bool IsClient { get; }
    
    }
}
