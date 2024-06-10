using System;
using System.Reflection;
using TouchSocket.Core;
using TouchSocketCoreConsole;

AppMessenger appMessenger = new AppMessenger();
MessageObject messageObject = new MessageObject();
MessageObject messageObject2 = new MessageObject();
appMessenger.Register(messageObject);
//appMessenger.Register(messageObject2); // 同样的方法名不能同时注册，会报错 TouchSocket.Core.MessageRegisteredException:“Token消息为‘Add’的已注册。”

ConsoleAction consoleAction = new ConsoleAction("h|help|?");//设置帮助命令
consoleAction.OnException += ConsoleAction_OnException;//订阅执行异常输出

void ConsoleAction_OnException(Exception exception)
{
    Console.WriteLine(exception.ToString());
}

//下列的ShareProxy，StopShareProxy，GetAll均为无参数的方法
consoleAction.Add("am|AppMessenger", "应用信使", SendAppMessenger);//示例命令
consoleAction.Add("ssp|stopShareProxy", "停止分享代理", StopShareProxy);
consoleAction.Add("dc|DependencyInject", "依赖注入", PropertyInject);
consoleAction.Add("do|DependencyObject", "依赖属性", DependencyObjectInject);
consoleAction.Add("pl|Plugin", "插件", PluginAction);


consoleAction.ShowAll();
while (true)
{
    if (!consoleAction.Run(Console.ReadLine()))
    {
        Console.WriteLine("命令不正确，请输入“h|help|?”获得帮助。");
    }
}


#region 命令方法
void SendAppMessenger()
{
    Task<int> add = appMessenger.SendAsync<int>("Add", 20, 10);
    Console.WriteLine($"add:{add.Result}");  //30
    var sub = appMessenger.SendAsync<int>("Sub", 20, 10);
    Console.WriteLine($"sub:{sub.Result}");  //10
}
void StopShareProxy()
{
    Console.WriteLine("StopShareProxy");
}

void DependencyInject() 
{
    var container = GetContainer();
    container.RegisterTransient<MyClass1>();
    container.RegisterSingleton<MyClass2>();

    var myClass1 = container.Resolve<MyClass1>();
    var myClass2 = container.Resolve<MyClass2>();

    Console.WriteLine(MethodBase.GetCurrentMethod().Name);
}

static void PropertyInject()
{
    var container2 = GetContainer();
    container2.RegisterSingleton<MyClass1>();
    container2.RegisterSingleton<MyClass2>();

    var myClass1 = container2.Resolve<MyClass1>();
    var myClass2 = container2.Resolve<MyClass2>();
    var myClass22 = container2.Resolve<MyClass2>();


    var container = GetContainer();
    container.RegisterSingleton<MyClass1>();
    container.RegisterSingleton<MyClass1>("key233");
    container.RegisterSingleton<MyClass2>();
    container.RegisterSingleton<MyClass3>();

    var myClass3 = container.Resolve<MyClass3>();
    var myClass33 = container.Resolve<MyClass3>();
    myClass3.KeyMyClass1 = new MyClass1();
    Console.WriteLine(MethodBase.GetCurrentMethod().Name);
}

static void MethodInject()
{
    var container = GetContainer();
    container.RegisterSingleton<MyClass1>();
    container.RegisterSingleton<MyClass4>();
    var mycalss1 = container.Resolve<MyClass1>();
    var myClass4 = container.Resolve<MyClass4>();
    Console.WriteLine(MethodBase.GetCurrentMethod().Name);
}

static void TransientInject()
{
    var container = GetContainer();
    container.RegisterSingleton<MyClass1>();
    container.RegisterTransient<MyClass5>();//默认参数只在瞬时生命有效

    var myClass5_1 = container.Resolve<MyClass5>();
    myClass5_1.A = 1;
    myClass5_1.B = "hi10";
    var myClass5_2 = container.Resolve<MyClass5>();
    myClass5_2.A = 12;
    myClass5_2.B = "hi20";
    var myClass5_3 = container.Resolve<MyClass5>();
    myClass5_3.A = 13;
    myClass5_3.B = "hi30";
    myClass5_3.MyClass1 = new MyClass1();

    //以下结果为true，因为在构建时并未覆盖MyClass1，所以MyClass1只能从容器获得，而MyClass1在容器是以单例注册，所以相同
    var b1 = myClass5_1.MyClass1 == myClass5_2.MyClass1;

    //以下结果为false，因为在构建时覆盖MyClass1，所以MyClass1是从ps参数直接传递的，所以不相同
    var b2 = myClass5_1.MyClass1 == myClass5_3.MyClass1;
}

static IContainer GetContainer()
{
    return new Container();//默认IOC容器

    //return new AspNetCoreContainer(new ServiceCollection());//使用Aspnetcore的容器
}

void DependencyObjectInject()
{
    MyDependencyObject myDependencyObject = new MyDependencyObject();
    myDependencyObject.MyProperty = 1;
    myDependencyObject.SetMyproperty(200);
    myDependencyObject.TryGetValue(DependencyExtensions.MyPropertyProperty2,out  var value);
}

void PluginAction()
{
    插件 plugin =new 插件();
    plugin.Run();
}
#endregion
