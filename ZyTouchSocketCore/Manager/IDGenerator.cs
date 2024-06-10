using TouchSocket.Core;

namespace ZyTouchSocketCore.Manager
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
                    iDGenerator = new IDGenerator(4);
                }
                return iDGenerator;
            }
        }

        SnowflakeIdGenerator generator;
        private IDGenerator(int wordID)
        {
            this.generator = new SnowflakeIdGenerator(wordID);
        }

        public uint GetNextID()
        {
            return (uint)this.generator.NextId();
        }
    }
}
