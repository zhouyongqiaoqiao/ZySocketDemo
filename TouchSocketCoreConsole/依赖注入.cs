using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;

namespace TouchSocketCoreConsole
{
    internal class 依赖注入
    {

    }

    class MyClass1
    {
        public long Id { get; set; } = DateTime.Now.Ticks;
    }

    class MyClass2
    {
        public MyClass2(MyClass1 myClass1)
        {
            this.MyClass1 = myClass1;
        }

        public MyClass1 MyClass1 { get; }
    }

    class MyClass3
    {
        /// <summary>
        /// 直接按类型，默认方式获取
        /// </summary>
        [DependencyInject]
        public MyClass1 MyClass1 { get; set; }

        /// <summary>
        /// 获得指定类型的对象，然后赋值到object
        /// </summary>
        [DependencyInject(typeof(MyClass2))]
        public object MyClass2 { get; set; }

        /// <summary>
        /// 按照类型+Key获取
        /// </summary>
        [DependencyInject("key233")]
        public MyClass1 KeyMyClass1 { get; set; }
    }

    class MyClass4
    {
        public MyClass1 MyClass1 { get; private set; }


        [DependencyInject]
        public void MethodInject22(MyClass1 myClass1)
        {
            this.MyClass1 = myClass1;
        }
    }

    class MyClass5
    {
        public MyClass5(int a, string b, MyClass1 myClass1)
        {
            this.A = a;
            this.B = b;
            this.MyClass1 = myClass1;
        }

        public int A { get; set; }
        public string B { get; set; }
        public MyClass1 MyClass1 { get; set; }
    }
}
