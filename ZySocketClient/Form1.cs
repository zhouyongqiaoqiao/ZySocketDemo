using System.Reflection.Metadata;
using System.Text;
using System.Windows.Forms;
using TouchSocket.Core;
using ZySocketCore;
using ZySocketCore.Client;
using ZySocketCore.Core;
using ZySocketCore.Core.DynamicGroup;
using ZySocketCore.Core.Enum;
using ZySocketCore.Interface;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZySocketClient
{
    public partial class Form1 : Form, ICustomizeHandler
    {

        IZyClientEngine clientEngine;
        public Form1()
        {
            InitializeComponent();
            this.clientEngine = NetworkEngineFactory.CreateStreamTcpClientEngine();
            //this.clientEngine.Han
            this.clientEngine.MessageReceived += ClientEngine_MessageReceived;
            this.clientEngine.ConnectionInterrupted += ClientEngine_Disconnected;
            this.clientEngine.ConnectionRebuildStart += ClientEngine_ConnectionRebuildStart;
            this.clientEngine.RelogonCompleted += ClientEngine_RelogonCompleted;

            this.clientEngine.BasicOutter.BeingKickedOut += BasicOutter_BeingKickedOut;
            this.clientEngine.BasicOutter.BeingPushedOut += BasicOutter_BeingPushedOut;
            this.clientEngine.BasicOutter.MyDeviceOnline += BasicOutter_MyDeviceOnline;
            this.clientEngine.BasicOutter.MyDeviceOffline += BasicOutter_MyDeviceOffline;

            this.clientEngine.ContactsOutter.ContactsDeviceConnected += ContactsOutter_ContactsDeviceConnected;
            this.clientEngine.ContactsOutter.ContactsDeviceDisconnected += ContactsOutter_ContactsDeviceDisconnected;
            this.clientEngine.ContactsOutter.ContactsOnline += ContactsOutter_ContactsOnline;
            this.clientEngine.ContactsOutter.ContactsOffline += ContactsOutter_ContactsOffline;
            this.clientEngine.ContactsOutter.BroadcastReceived += ContactsOutter_BroadcastReceived;

            this.clientEngine.DynamicGroupOutter.GroupTagChanged += DynamicGroupOutter_GroupTagChanged;
            this.clientEngine.DynamicGroupOutter.BePulledIntoGroup += DynamicGroupOutter_BePulledIntoGroup;
            this.clientEngine.DynamicGroupOutter.BeInvitedJoinGroup += DynamicGroupOutter_BeInvitedJoinGroup;
            this.clientEngine.DynamicGroupOutter.BeRemovedFromGroup += DynamicGroupOutter_BeRemovedFromGroup;
            this.clientEngine.DynamicGroupOutter.SomeoneBePulledIntoGroup += DynamicGroupOutter_SomeoneBePulledIntoGroup;
            this.clientEngine.DynamicGroupOutter.SomeoneBeRemovedFromGroup += DynamicGroupOutter_SomeoneBeRemovedFromGroup;
            this.clientEngine.DynamicGroupOutter.SomeoneJoinGroup += DynamicGroupOutter_SomeoneJoinGroup;
            this.clientEngine.DynamicGroupOutter.SomeoneQuitGroup += DynamicGroupOutter_SomeoneQuitGroup;
            this.clientEngine.DynamicGroupOutter.BroadcastReceived += DynamicGroupOutter_BroadcastReceived;

        }



        #region ContactsOutter
        private void ContactsOutter_BroadcastReceived(string sourceUserID, ClientType clientType, string groupID, int broadcastType, byte[] broadcastContent, string tag)
        {
            this.WriteInfo($"收到广播消息：{sourceUserID} - {groupID} - {broadcastType} - {Encoding.UTF8.GetString(broadcastContent)} - {tag}");
        }

        private void ContactsOutter_ContactsOffline(string userID)
        {
            this.WriteInfo($"用户{userID}离线");
        }

        private void ContactsOutter_ContactsOnline(string userID)
        {
            this.WriteInfo($"用户{userID}上线");
        }

        private void ContactsOutter_ContactsDeviceDisconnected(string userID, ClientType type)
        {
            this.WriteInfo($"用户{userID}的{type}设备断开连接");
        }

        private void ContactsOutter_ContactsDeviceConnected(string userID, ClientType type)
        {
            this.WriteInfo($"用户{userID}的{type}设备连接成功");
        }
        #endregion

        #region IBasicOutter
        private void BasicOutter_MyDeviceOffline(ClientType obj)
        {
            this.WriteInfo($"设备{obj}离线");
        }

        private void BasicOutter_MyDeviceOnline(ClientType obj)
        {
            this.WriteInfo($"设备{obj}上线");
        }

        private void BasicOutter_BeingPushedOut()
        {
            this.WriteInfo($"在其他地方登陆，被挤下线");
        }

        private void BasicOutter_BeingKickedOut()
        {
            this.WriteInfo($"被管理员踢下线");
        }
        #endregion

        #region DynamicGroup
        private void DynamicGroupOutter_BroadcastReceived(string broadcasterID, string gourpID, int broadcastType, byte[] info, string tag)
        {
            this.WriteInfo($"收到{broadcasterID}的{gourpID}的广播消息：{broadcastType} - {Encoding.UTF8.GetString(info)} - {tag}");
        }

        private void DynamicGroupOutter_SomeoneQuitGroup(string groupID, string memberID)
        {
            this.WriteInfo($"用户{memberID}退出{groupID}组");
        }

        private void DynamicGroupOutter_SomeoneJoinGroup(string groupID, string memberID)
        {
            this.WriteInfo($"用户{memberID}加入{groupID}组");
        }

        private void DynamicGroupOutter_SomeoneBeRemovedFromGroup(string groupID, List<string> memberList, string operatorID)
        {
            this.WriteInfo($"用户{operatorID}将{string.Join(",", memberList)}移除{groupID}组");
        }

        private void DynamicGroupOutter_SomeoneBePulledIntoGroup(string groupID, List<string> memberList, string operatorID)
        {
            this.WriteInfo($"用户{operatorID}将{string.Join(",", memberList)}拉入{groupID}组");
        }

        private void DynamicGroupOutter_BeRemovedFromGroup(string groupID, string operatorID)
        {
            this.WriteInfo($"自己被{operatorID}移除组 {groupID}");
        }

        private void DynamicGroupOutter_BeInvitedJoinGroup(ZySocketCore.Core.DynamicGroup.GroupInfo groupInfo, string operatorID)
        {
            this.WriteInfo($"自己{operatorID}邀请加入{groupInfo.ID}组");
        }

        private void DynamicGroupOutter_BePulledIntoGroup(ZySocketCore.Core.DynamicGroup.GroupInfo groupInfo, string operatorID)
        {
            this.WriteInfo($"自己被{operatorID}被拉入{groupInfo.ID}组");
        }

        private void DynamicGroupOutter_GroupTagChanged(string groupID, string newTag, string operatorID)
        {
            this.WriteInfo($"{operatorID}修改{groupID}组的标签为{newTag}");
        }
        #endregion

        private void ClientEngine_RelogonCompleted(LogonResponse response)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<LogonResponse>(this.ClientEngine_RelogonCompleted), response);
            }
            else
            {
                if (response.LogonResult == LogonResult.Succeed)
                {
                    this.Text = $"当前用户：{this.clientEngine.CurrentUserID}";
                    this.WriteInfo($"{this.clientEngine.CurrentUserID}重连登录成功");
                }
                else
                {
                    this.WriteInfo($"重连失败：{response.FailureCause}");
                }
            }
        }

        private void ClientEngine_ConnectionRebuildStart()
        {
            this.WriteInfo($"正在重连中...");
        }

        private void ClientEngine_Disconnected()
        {
            this.WriteInfo($"连接已断开");
        }

        private DateTime lastSrollTime;
        private void ScrollEnd()
        {
            if ((DateTime.Now - lastSrollTime).TotalMilliseconds > 1000)
            {
                this.txb_info.ScrollToCaret();
                lastSrollTime = DateTime.Now;
            }
        }
        private void WriteInfo(string info)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(this.WriteInfo), info);
            }
            else
            {
                this.txb_info.AppendText(info + "\r\n");
                this.ScrollEnd();
            }
        }

        private void btn_logon_Click(object sender, EventArgs e)
        {
            string ip = this.txb_ip.Text;
            int port = int.Parse(this.txb_port.Text);


            //初始化并登陆到服务器
            LogonResponse res = this.clientEngine.Initialize(this.txb_userID.Text, this.txb_pwd.Text, ip, port, this);
            if (res.LogonResult == LogonResult.Succeed)
            {
                this.Text = $"当前用户：{this.txb_userID.Text} - {ip}:{port}";
                this.WriteInfo($"{ip}:{port}登录成功");
            }
            else
            {
                this.WriteInfo($"登录失败：{res.FailureCause}");
            }
        }

        private void ClientEngine_MessageReceived(string sourceUserID, ClientType clientType, int informationType, byte[] blob, string tag)
        {
            this.WriteInfo($"来自{sourceUserID}的{clientType}消息：{informationType}_{Encoding.UTF8.GetString(blob)}, tag:{tag}");
        }

        #region ICustomizeHandler
        public void HandleInformation(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            this.WriteInfo($"来自{sourceUserID}的{clientType}自定义消息：{informationType}_{Encoding.UTF8.GetString(info)}");
        }

        public byte[] HandleQuery(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            return Encoding.UTF8.GetBytes("自定义回复");
        }
        #endregion

        private void btn_send_Click(object sender, EventArgs e)
        {
            this.clientEngine.SendMessage(this.txb_destUserID.Text, 1, Encoding.UTF8.GetBytes(this.txb_msg.Text), "tagtest");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.clientEngine.CustomizeOutter.SendAsync(this.txb_destUserID.Text, 101, Encoding.UTF8.GetBytes(this.txb_msg.Text));
        }

        private async void btn_query_Click(object sender, EventArgs e)
        {
            // CustomMessage customMessage = new CustomMessage() { InformationType = 101, Content = Encoding.UTF8.GetBytes(this.txb_msg.Text) };
            // ZyLightFixedHeaderPackageInfo info = new ZyLightFixedHeaderPackageInfo(this.clientEngine.CurrentUserID,"_0", (int)MessageType.QUERY_ASYNC, SerializeConvert.FastBinarySerialize(customMessage));
            //var resss= await  this.clientEngine.Queryer.QueryAsync(info.ToByteArray());

            byte[] res = this.clientEngine.CustomizeOutter.Query(101, Encoding.UTF8.GetBytes(this.txb_msg.Text));
            this.WriteInfo($"自定义查询结果：{Encoding.UTF8.GetString(res)}");
        }

        private void btn_QueryBeforeLogin_Click(object sender, EventArgs e)
        {
            var res = this.clientEngine.QueryBeforeLogin(this.txb_ip.Text, int.Parse(this.txb_port.Text), int.Parse(this.txb_queryType.Text), this.txb_queryData.Text);

            this.WriteInfo($"QueryBeforeLogin查询结果：{res}");
        }

        private void btn_getIp_Click(object sender, EventArgs e)
        {
            var ip = this.clientEngine.BasicOutter.GetMyIPE();
            MessageBox.Show(ip.ToString());
        }

        private void btn_getDevice_Click(object sender, EventArgs e)
        {
            var devices = this.clientEngine.BasicOutter.GetMyOnlineDevice();
            string str = "";
            foreach (var item in devices)
            {
                str += item.ToString() + ",";
            }
            MessageBox.Show(str);
        }

        private void btn_sendHeartBeat_Click(object sender, EventArgs e)
        {
            this.clientEngine.BasicOutter.SendHeartBeatMessage();
        }

        private void btn_kickOut_Click(object sender, EventArgs e)
        {
            this.clientEngine.BasicOutter.KickOut(this.txb_destID.Text);
        }

        private void btn_Broadcast_Click(object sender, EventArgs e)
        {
            string groupID = this.txb_groupID.Text;
            string message = "大家好，我是广播消息";
            this.clientEngine.ContactsOutter.Broadcast(groupID, 1001, Encoding.UTF8.GetBytes(message), "tagtest");
        }

        private void btn_GetGroupMembers_Click(object sender, EventArgs e)
        {
            string groupID = this.txb_groupID.Text;
            var groupmates = this.clientEngine.ContactsOutter.GetGroupMembers(groupID);
            string str = $"获取{groupID}的成员列表：\r\n";
            str += "在线成员：\r\n" + string.Join(",", groupmates.OnlineGroupmates.Select(x => x));
            str += "\r\n离线成员：\r\n" + string.Join(",", groupmates.OfflineGroupmates.Select(x => x));
            this.WriteInfo(str);
        }

        private void btn_getContact_Click(object sender, EventArgs e)
        {
            List<string> memberIDList;
            if (this.ckb_isOnline.Checked)
            {
                memberIDList = this.clientEngine.ContactsOutter.GetAllOnlineContacts();
            }
            else
            {
                memberIDList = this.clientEngine.ContactsOutter.GetContacts();
            }
            string str = "获取联系人列表：\r\n";
            str += string.Join(",", memberIDList);
            this.WriteInfo(str);
        }
        #region 动态组方法

        private void btn_Broadcast22_Click(object sender, EventArgs e)
        {
            this.clientEngine.DynamicGroupOutter.Broadcast(this.txb_groupID22.Text, 1001, Encoding.UTF8.GetBytes("大家好，我是动态组广播消息"), "tagtest");
        }

        private void btn_GetGroup_Click(object sender, EventArgs e)
        {
            var group = this.clientEngine.DynamicGroupOutter.GetGroupInformation(this.txb_groupID22.Text,false);
            this.WriteInfo($"获取{group.ID}的组信息：\r\n{group.ToString()}");
        }

        private void btn_SyncGroupMembers_Click(object sender, EventArgs e)
        {
            this.clientEngine.DynamicGroupOutter.SyncGroupMembers(this.txb_groupID22.Text);
            this.WriteInfo($"同步{this.txb_groupID22.Text}的组信息完成");
        }

        private void btn_GetMyGroups_Click(object sender, EventArgs e)
        {
            var groups = this.clientEngine.DynamicGroupOutter.GetMyGroups();
            this.WriteInfo($"获取我的组列表：\r\n{string.Join(",", groups.Select(x => x))}");        
        }

        private void btn_GetGroupmates_Click(object sender, EventArgs e)
        {
            var groupmates = this.clientEngine.DynamicGroupOutter.GetOnlineGroupmates();
            this.WriteInfo($"获取在线组成员：\r\n{string.Join(",", groupmates.Select(x => x))}");
        }

        private void btn_JoinGroup_Click(object sender, EventArgs e)
        {
            var group= this.clientEngine.DynamicGroupOutter.JoinGroup(this.txb_groupID22.Text);
            this.WriteInfo($"加入{group.ID}组成功,组信息 {group.ToString()}");
        }

        private void btn_QuitGroup_Click(object sender, EventArgs e)
        {
            this.clientEngine.DynamicGroupOutter.QuitGroup(this.txb_groupID22.Text);
            this.WriteInfo($"退出{this.txb_groupID22.Text}组成功");        
        }

        private void btn_PullIntoGroup_Click(object sender, EventArgs e)
        {
            this.clientEngine.DynamicGroupOutter.PullIntoGroup(this.txb_groupID22.Text, new List<string>() { "aa02","aa03", "aa05" });
        }

        private void btn_InviteJoinGroup_Click(object sender, EventArgs e)
        {
            this.clientEngine.DynamicGroupOutter.InviteJoinGroup(this.txb_groupID22.Text, new List<string>() { "aa02", "aa03", "aa04" });
        }

        private void btn_RemoveFromGroup_Click(object sender, EventArgs e)
        {
            this.clientEngine.DynamicGroupOutter.RemoveFromGroup(this.txb_groupID22.Text, new List<string>() { "aa02", "aa03" });
        }

        private void btn_SetGroupTag_Click(object sender, EventArgs e)
        {
            this.clientEngine.DynamicGroupOutter.SetGroupTag(this.txb_groupID22.Text, "new tag2222");
        } 
        #endregion
    }


}
