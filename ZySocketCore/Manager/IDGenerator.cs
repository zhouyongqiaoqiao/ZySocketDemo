using TouchSocket.Core;

namespace ZySocketCore.Manager
{
    public class IDGenerator
    {
        private static IDGenerator iDGenerator = null;

        public static IDGenerator Instance
        {
            get
            {
                if (iDGenerator == null)
                {
                    iDGenerator = new IDGenerator();
                }
                return iDGenerator;
            }
        }

        private volatile uint currentID = 0;

        private IDGenerator()
        {

        }

        public uint GetNextID()
        {
            lock (this)
            {
                currentID++;
                return currentID;
            }
        }

    }
}

