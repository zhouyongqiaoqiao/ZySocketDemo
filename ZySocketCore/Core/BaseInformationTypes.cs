using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ZySocketCore.Core
{
    //
    // 摘要:
    //     自定义信息类型空间的基类。 (会自动在特定属性上加上StartKey值，特定属性必须满足可读写、返回类型为int)
    public abstract class BaseInformationTypes
    {
        protected BaseInformationTypes() { }

        //
        // 摘要:
        //     当前信息类型空间的起始值。(Initialize 之前设置才有效)
        public int StartKey { get; set; } =0;
        //
        // 摘要:
        //     当前信息类型空间的最大值。 (Initialize 之前设置才有效)
        public int MaxKeyValue { get; set; } = int.MaxValue;

        public bool Contains(int informationType) { return this.validValidList.Contains(informationType); }
        public void Initialize(int _startKey) { 
        
            this.StartKey = _startKey;
            this.Initialize();
        }

        public void Initialize() {

            this.GetValidProperties();
        }

        private List<int> validValidList = new List<int>();

        private void GetValidProperties()
        {
            Type type = this.GetType();            

            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == nameof(this.StartKey) || property.Name == nameof(this.MaxKeyValue)) continue;

                if (property.PropertyType == typeof(int) && property.CanWrite)
                {
                    int value = (int)property.GetValue(this) + this.StartKey;
                    if (value > this.MaxKeyValue)
                    {
                        throw new Exception("The value of the property is out of MaxKeyValue.");
                    }
                    property.SetValue(this, (int)property.GetValue(this) + this.StartKey);
                    validValidList.Add((int)property.GetValue(this));
                }
            }
        }
    }


    public class BaseInformation :BaseInformationTypes{

        public BaseInformation() : base() 
        {
            base.StartKey = 100;
            base.Initialize();
        }

        public string ID { get; set; } = "id";

        public int Age { get; set; } = 18;

        public string Name { get; set; } = "name";

        public byte Color { get; set; } = 0;

        public  double Money { get; set; } = 10000.0;  
        
        private byte sex =0;

        public byte Sex { get => sex; set => sex = value; }
}
}
