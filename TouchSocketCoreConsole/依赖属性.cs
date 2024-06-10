using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;

namespace TouchSocketCoreConsole
{
    internal class MyDependencyObject : DependencyObject
    {
        #region 方式一
        public int MyProperty
        {
            get { return (int)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty<int> MyPropertyProperty =
            DependencyProperty<int>.Register("MyProperty", 10); 
        #endregion


    }

    static class DependencyExtensions
    {
        #region 拓展方式二
        public static int GetMyProperty<TClient>(this TClient client) where TClient : IDependencyObject
        {
            return (int)client.GetValue(MyPropertyProperty2);
        }

        public static void SetMyproperty<TClient>(this TClient client, int value) where TClient : IDependencyObject
        {
            client.SetValue(MyPropertyProperty2, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty<int> MyPropertyProperty2 =
            DependencyProperty<int>.Register("MyProperty2", 10); 
        #endregion
    }
}
