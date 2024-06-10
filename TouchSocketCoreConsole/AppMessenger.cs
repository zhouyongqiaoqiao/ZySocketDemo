using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;

namespace TouchSocketCoreConsole
{
    internal class AppMessenger2
    {


    }

    public class MessageObject : IMessageObject
    {
        private int defaultValue =0;
        public MessageObject(int defaultValue = 0)
        {
            this.defaultValue = defaultValue;
        }

        [AppMessage]
        public Task<int> Add(int a, int b)
        {
            return Task.FromResult( a + b +this.defaultValue);
        }

        [AppMessage]
        public Task<int> Sub(int a, int b)
        {
            return Task.FromResult(a - b+this.defaultValue);
        }
    }

}
