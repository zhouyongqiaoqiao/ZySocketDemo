"use strict";(self.webpackChunktouchsocket=self.webpackChunktouchsocket||[]).push([[631],{3905:(e,t,n)=>{n.d(t,{Zo:()=>u,kt:()=>d});var r=n(7294);function c(e,t,n){return t in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function a(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);t&&(r=r.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,r)}return n}function o(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?a(Object(n),!0).forEach((function(t){c(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):a(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function p(e,t){if(null==e)return{};var n,r,c=function(e,t){if(null==e)return{};var n,r,c={},a=Object.keys(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||(c[n]=e[n]);return c}(e,t);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(c[n]=e[n])}return c}var l=r.createContext({}),s=function(e){var t=r.useContext(l),n=t;return e&&(n="function"==typeof e?e(t):o(o({},t),e)),n},u=function(e){var t=s(e.components);return r.createElement(l.Provider,{value:t},e.children)},i={inlineCode:"code",wrapper:function(e){var t=e.children;return r.createElement(r.Fragment,{},t)}},v=r.forwardRef((function(e,t){var n=e.components,c=e.mdxType,a=e.originalType,l=e.parentName,u=p(e,["components","mdxType","originalType","parentName"]),v=s(n),d=c,m=v["".concat(l,".").concat(d)]||v[d]||i[d]||a;return n?r.createElement(m,o(o({ref:t},u),{},{components:n})):r.createElement(m,o({ref:t},u))}));function d(e,t){var n=arguments,c=t&&t.mdxType;if("string"==typeof e||c){var a=n.length,o=new Array(a);o[0]=v;var p={};for(var l in t)hasOwnProperty.call(t,l)&&(p[l]=t[l]);p.originalType=e,p.mdxType="string"==typeof e?e:c,o[1]=p;for(var s=2;s<a;s++)o[s]=n[s];return r.createElement.apply(null,o)}return r.createElement.apply(null,n)}v.displayName="MDXCreateElement"},7183:(e,t,n)=>{n.r(t),n.d(t,{assets:()=>l,contentTitle:()=>o,default:()=>i,frontMatter:()=>a,metadata:()=>p,toc:()=>s});var r=n(7462),c=(n(7294),n(3905));const a={id:"eventbus",title:"EventBus"},o=void 0,p={unversionedId:"eventbus",id:"eventbus",title:"EventBus",description:"\u8bf4\u660e",source:"@site/docs/eventbus.mdx",sourceDirName:".",slug:"/eventbus",permalink:"/touchsocket/docs/eventbus",draft:!1,editUrl:"https://gitee.com/rrqm_home/touchsocket/tree/master/handbook/docs/eventbus.mdx",tags:[],version:"current",lastUpdatedBy:"\u82e5\u6c5d\u68cb\u8317",lastUpdatedAt:1675770803,formattedLastUpdatedAt:"Feb 7, 2023",frontMatter:{id:"eventbus",title:"EventBus"}},l={},s=[{value:"\u8bf4\u660e",id:"\u8bf4\u660e",level:2},{value:"\u521b\u5efa\u670d\u52a1\u5668",id:"\u521b\u5efa\u670d\u52a1\u5668",level:2},{value:"\u521b\u5efa\u5ba2\u6237\u7aef",id:"\u521b\u5efa\u5ba2\u6237\u7aef",level:2},{value:"\u670d\u52a1\u5668\u89e6\u53d1",id:"\u670d\u52a1\u5668\u89e6\u53d1",level:2},{value:"\u5176\u4ed6",id:"\u5176\u4ed6",level:2}],u={toc:s};function i(e){let{components:t,...n}=e;return(0,c.kt)("wrapper",(0,r.Z)({},u,n,{components:t,mdxType:"MDXLayout"}),(0,c.kt)("h2",{id:"\u8bf4\u660e"},"\u8bf4\u660e"),(0,c.kt)("p",null,"EventBus\u529f\u80fd\u662f\u4f01\u4e1a\u7248\u4e13\u5c5e\u529f\u80fd\uff0c\u5176\u804c\u80fd\u7c7b\u4f3cMQTT\u7684\u53d1\u5e03\u8ba2\u9605\u6a21\u5f0f\uff0c\u4e5f\u7c7b\u4f3cRabbitMQ\u7684Sub\u6a21\u5f0f\u3002\u5982\u679c\u6ca1\u6709\u4f7f\u7528\u5bc6\u94a5\uff0c\u53ef\u4ee5",(0,c.kt)("a",{parentName:"p",href:"https://www.yuque.com/eo2w71/rrqm/80696720a95e415d94c87fa03642513d#Dfy2T"},"\u8bd5\u7528"),"\u53c2\u8003\u3002 ",(0,c.kt)("a",{name:"cmsde"})),(0,c.kt)("h2",{id:"\u521b\u5efa\u670d\u52a1\u5668"},"\u521b\u5efa\u670d\u52a1\u5668"),(0,c.kt)("p",null,"\u670d\u52a1\u5668\u7684\u521b\u5efa\u5c31\u662fTouchRpc\u670d\u52a1\u5668\u3002\u9664udp\u534f\u8bae\u5916\uff0ctcp\u3001http\u3001websocket\u534f\u8bae\u7684\u7248\u672c\u5747\u652f\u6301\u8be5\u529f\u80fd\u3002"),(0,c.kt)("p",null,"\u4e0b\u5217\u4ee5TcpTouchRpcService\u4e3a\u4f8b\u3002"),(0,c.kt)("pre",null,(0,c.kt)("code",{parentName:"pre",className:"language-csharp"},"TcpTouchRpcService tcpRpcService = new TcpTouchRpcService();\n\nvar config = new RRQMConfig();\nconfig.SetListenIPHosts(new IPHost[] { new RRQMSocket.IPHost(7789) });\ntcpRpcService\n    .Setup(config)\n    .Start();\n\n")),(0,c.kt)("p",null,"\u7531",(0,c.kt)("strong",{parentName:"p"},"\u670d\u52a1\u5668"),"\u53d1\u5e03\u4e00\u4e2a\u4e8b\u4ef6\u3002\n\u7b2c\u4e00\u4e2a\u53c2\u6570\u4e3a\u4e8b\u4ef6\u540d\uff0c\u7b2c\u4e8c\u4e2a\u4e3a\u8bbf\u95ee\u6743\u9650\u3002"),(0,c.kt)("pre",null,(0,c.kt)("code",{parentName:"pre",className:"language-csharp"},'tcpRpcService.PublishEvent("Hello", AccessType.Owner | AccessType.Service | AccessType.Everyone);\n')),(0,c.kt)("a",{name:"fesMG"}),(0,c.kt)("h2",{id:"\u521b\u5efa\u5ba2\u6237\u7aef"},"\u521b\u5efa\u5ba2\u6237\u7aef"),(0,c.kt)("p",null,"\u5ba2\u6237\u7aef\u8ba2\u9605\u8be5\u4e8b\u4ef6\u3002"),(0,c.kt)("pre",null,(0,c.kt)("code",{parentName:"pre",className:"language-csharp"},'TcpTouchRpcClient tcpRpcClient = new TcpTouchRpcClient();\ntcpRpcClient\n    .Setup("127.0.0.1:7789")\n    .Connect();\n\ntcpRpcClient.SubscribeEvent<string>("Hello", SubscribeEvent);\n\n')),(0,c.kt)("p",null,"\u5176\u4e2dSubscribeEvent\u662f\u63a5\u6536\u59d4\u6258\u3002\u6b64\u5904\u7528\u65b9\u6cd5\u8f6c\u6362\u63a5\u6536\u3002\u5176\u76ee\u7684\u4e3a\uff0c\u5f53\u670d\u52a1\u5668\u89e6\u53d1\u8be5\u65b9\u6cd5\u65f6\uff0c\u5c31\u4f1a\u5206\u53d1\u5230\u6b64\u5904\u3002"),(0,c.kt)("pre",null,(0,c.kt)("code",{parentName:"pre",className:"language-csharp"},' private void SubscribeEvent(EventSender eventSender, string arg)\n {\n     this.ShowMsg($"\u4ece{eventSender.RaiseSourceType}\u6536\u5230\u901a\u77e5\u4e8b\u4ef6{eventSender.EventName}\uff0c\u4fe1\u606f\uff1a{arg}");\n }\n')),(0,c.kt)("a",{name:"lwUT0"}),(0,c.kt)("h2",{id:"\u670d\u52a1\u5668\u89e6\u53d1"},"\u670d\u52a1\u5668\u89e6\u53d1"),(0,c.kt)("p",null,"\u7b2c\u4e00\u4e2a\u53c2\u6570\u662f\u4e8b\u4ef6\u540d\uff0c\u7b2c\u4e8c\u4e2a\u662f\u4e8b\u4ef6\u53c2\u6570\u3002\u53ef\u4ee5\u662f\u4efb\u610f\u7c7b\u578b\uff0c\u4f46\u662f\u76ee\u524d\u4ec5\u652f\u6301\u4e00\u4e2a\u53c2\u6570\u3002"),(0,c.kt)("pre",null,(0,c.kt)("code",{parentName:"pre",className:"language-csharp"},'tcpRpcService.RaiseEvent("Hello", "Hi");\n')),(0,c.kt)("a",{name:"cLPrt"}),(0,c.kt)("h2",{id:"\u5176\u4ed6"},"\u5176\u4ed6"),(0,c.kt)("p",null,"\u5b9e\u9645\u4e0a\u5728TouchRpc\u67b6\u6784\u4e2d\u3002",(0,c.kt)("strong",{parentName:"p"},"TouchService"),"\u3001",(0,c.kt)("strong",{parentName:"p"},"TouchSocketClient"),"\u3001",(0,c.kt)("strong",{parentName:"p"},"TouchClient"),"\u4e09\u8005\u5747\u5df2\u5b9e\u73b0",(0,c.kt)("strong",{parentName:"p"},"IEventObject"),"\u63a5\u53e3\uff0c\u8fd9\u610f\u5473\u5747\u53ef\u4ee5",(0,c.kt)("strong",{parentName:"p"},"\u53d1\u5e03\u3001\u53d6\u6d88\u53d1\u5e03\u3001\u8ba2\u9605\u3001\u53d6\u6d88\u8ba2\u9605\u3001\u89e6\u53d1"),"\u7b49\u64cd\u4f5c\uff08\u4f1a\u9a8c\u8bc1\u64cd\u4f5c\u6743\u9650\uff09\u3002"))}i.isMDXComponent=!0}}]);