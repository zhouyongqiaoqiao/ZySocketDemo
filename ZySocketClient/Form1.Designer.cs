namespace ZySocketClient
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txb_ip = new TextBox();
            txb_port = new TextBox();
            btn_logon = new Button();
            txb_msg = new TextBox();
            txb_destUserID = new TextBox();
            btn_send = new Button();
            txb_info = new RichTextBox();
            btn_query = new Button();
            txb_userID = new TextBox();
            txb_pwd = new TextBox();
            button1 = new Button();
            btn_QueryBeforeLogin = new Button();
            txb_queryData = new TextBox();
            txb_queryType = new TextBox();
            groupBox1 = new GroupBox();
            btn_tickOut = new Button();
            txb_destID = new TextBox();
            btn_sendHeartBeat = new Button();
            btn_getDevice = new Button();
            btn_getIp = new Button();
            groupBox2 = new GroupBox();
            ckb_isOnline = new CheckBox();
            btn_GetGroupMembers = new Button();
            btn_getContact = new Button();
            btn_Broadcast = new Button();
            txb_groupID = new TextBox();
            groupBox3 = new GroupBox();
            btn_SetGroupTag = new Button();
            btn_SyncGroupMembers = new Button();
            btn_RemoveFromGroup = new Button();
            btn_QuitGroup = new Button();
            btn_PullIntoGroup = new Button();
            btn_JoinGroup = new Button();
            btn_InviteJoinGroup = new Button();
            btn_GetGroup = new Button();
            btn_GetGroupmates = new Button();
            btn_GetMyGroups = new Button();
            btn_Broadcast22 = new Button();
            txb_groupID22 = new TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // txb_ip
            // 
            txb_ip.Location = new Point(26, 35);
            txb_ip.Name = "txb_ip";
            txb_ip.Size = new Size(100, 23);
            txb_ip.TabIndex = 0;
            txb_ip.Text = "127.0.0.1";
            // 
            // txb_port
            // 
            txb_port.Location = new Point(132, 35);
            txb_port.Name = "txb_port";
            txb_port.Size = new Size(68, 23);
            txb_port.TabIndex = 0;
            txb_port.Text = "4530";
            // 
            // btn_logon
            // 
            btn_logon.Location = new Point(377, 35);
            btn_logon.Name = "btn_logon";
            btn_logon.Size = new Size(75, 23);
            btn_logon.TabIndex = 1;
            btn_logon.Text = "登录";
            btn_logon.UseVisualStyleBackColor = true;
            btn_logon.Click += btn_logon_Click;
            // 
            // txb_msg
            // 
            txb_msg.Location = new Point(26, 73);
            txb_msg.Name = "txb_msg";
            txb_msg.Size = new Size(100, 23);
            txb_msg.TabIndex = 2;
            txb_msg.Text = "HI !";
            // 
            // txb_destUserID
            // 
            txb_destUserID.Location = new Point(132, 73);
            txb_destUserID.Name = "txb_destUserID";
            txb_destUserID.Size = new Size(68, 23);
            txb_destUserID.TabIndex = 2;
            txb_destUserID.Text = "_0";
            // 
            // btn_send
            // 
            btn_send.Location = new Point(206, 73);
            btn_send.Name = "btn_send";
            btn_send.Size = new Size(75, 23);
            btn_send.TabIndex = 1;
            btn_send.Text = "发送消息";
            btn_send.UseVisualStyleBackColor = true;
            btn_send.Click += btn_send_Click;
            // 
            // txb_info
            // 
            txb_info.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txb_info.Location = new Point(0, 220);
            txb_info.Name = "txb_info";
            txb_info.Size = new Size(756, 278);
            txb_info.TabIndex = 3;
            txb_info.Text = "";
            // 
            // btn_query
            // 
            btn_query.Location = new Point(377, 73);
            btn_query.Name = "btn_query";
            btn_query.Size = new Size(75, 23);
            btn_query.TabIndex = 1;
            btn_query.Text = "请求回复";
            btn_query.UseVisualStyleBackColor = true;
            btn_query.Click += btn_query_Click;
            // 
            // txb_userID
            // 
            txb_userID.Location = new Point(205, 35);
            txb_userID.Name = "txb_userID";
            txb_userID.Size = new Size(76, 23);
            txb_userID.TabIndex = 4;
            txb_userID.Text = "aa01";
            // 
            // txb_pwd
            // 
            txb_pwd.Location = new Point(287, 35);
            txb_pwd.Name = "txb_pwd";
            txb_pwd.Size = new Size(84, 23);
            txb_pwd.TabIndex = 4;
            txb_pwd.Text = "12345";
            // 
            // button1
            // 
            button1.Location = new Point(287, 73);
            button1.Name = "button1";
            button1.Size = new Size(84, 23);
            button1.TabIndex = 1;
            button1.Text = "发送自定义";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btn_QueryBeforeLogin
            // 
            btn_QueryBeforeLogin.Location = new Point(377, 6);
            btn_QueryBeforeLogin.Name = "btn_QueryBeforeLogin";
            btn_QueryBeforeLogin.Size = new Size(75, 23);
            btn_QueryBeforeLogin.TabIndex = 5;
            btn_QueryBeforeLogin.Text = "QueryBeforeLogin";
            btn_QueryBeforeLogin.UseVisualStyleBackColor = true;
            btn_QueryBeforeLogin.Click += btn_QueryBeforeLogin_Click;
            // 
            // txb_queryData
            // 
            txb_queryData.Location = new Point(287, 6);
            txb_queryData.Name = "txb_queryData";
            txb_queryData.Size = new Size(84, 23);
            txb_queryData.TabIndex = 6;
            txb_queryData.Text = "content";
            // 
            // txb_queryType
            // 
            txb_queryType.Location = new Point(205, 6);
            txb_queryType.Name = "txb_queryType";
            txb_queryType.Size = new Size(76, 23);
            txb_queryType.TabIndex = 7;
            txb_queryType.Text = "1";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btn_tickOut);
            groupBox1.Controls.Add(txb_destID);
            groupBox1.Controls.Add(btn_sendHeartBeat);
            groupBox1.Controls.Add(btn_getDevice);
            groupBox1.Controls.Add(btn_getIp);
            groupBox1.Location = new Point(19, 110);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(262, 104);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Basic";
            // 
            // btn_tickOut
            // 
            btn_tickOut.Location = new Point(88, 58);
            btn_tickOut.Name = "btn_tickOut";
            btn_tickOut.Size = new Size(75, 23);
            btn_tickOut.TabIndex = 9;
            btn_tickOut.Text = "踢人";
            btn_tickOut.UseVisualStyleBackColor = true;
            btn_tickOut.Click += btn_kickOut_Click;
            // 
            // txb_destID
            // 
            txb_destID.Location = new Point(7, 58);
            txb_destID.Name = "txb_destID";
            txb_destID.Size = new Size(75, 23);
            txb_destID.TabIndex = 10;
            // 
            // btn_sendHeartBeat
            // 
            btn_sendHeartBeat.Location = new Point(169, 22);
            btn_sendHeartBeat.Name = "btn_sendHeartBeat";
            btn_sendHeartBeat.Size = new Size(75, 23);
            btn_sendHeartBeat.TabIndex = 9;
            btn_sendHeartBeat.Text = "发送心跳";
            btn_sendHeartBeat.UseVisualStyleBackColor = true;
            btn_sendHeartBeat.Click += btn_sendHeartBeat_Click;
            // 
            // btn_getDevice
            // 
            btn_getDevice.Location = new Point(88, 22);
            btn_getDevice.Name = "btn_getDevice";
            btn_getDevice.Size = new Size(75, 23);
            btn_getDevice.TabIndex = 9;
            btn_getDevice.Text = "获取设备";
            btn_getDevice.UseVisualStyleBackColor = true;
            btn_getDevice.Click += btn_getDevice_Click;
            // 
            // btn_getIp
            // 
            btn_getIp.Location = new Point(7, 22);
            btn_getIp.Name = "btn_getIp";
            btn_getIp.Size = new Size(75, 23);
            btn_getIp.TabIndex = 9;
            btn_getIp.Text = "获取IP";
            btn_getIp.UseVisualStyleBackColor = true;
            btn_getIp.Click += btn_getIp_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(ckb_isOnline);
            groupBox2.Controls.Add(btn_GetGroupMembers);
            groupBox2.Controls.Add(btn_getContact);
            groupBox2.Controls.Add(btn_Broadcast);
            groupBox2.Controls.Add(txb_groupID);
            groupBox2.Location = new Point(298, 110);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(210, 104);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Contact";
            // 
            // ckb_isOnline
            // 
            ckb_isOnline.AutoSize = true;
            ckb_isOnline.Location = new Point(10, 77);
            ckb_isOnline.Name = "ckb_isOnline";
            ckb_isOnline.Size = new Size(51, 21);
            ckb_isOnline.TabIndex = 5;
            ckb_isOnline.Text = "在线";
            ckb_isOnline.UseVisualStyleBackColor = true;
            // 
            // btn_GetGroupMembers
            // 
            btn_GetGroupMembers.Location = new Point(79, 50);
            btn_GetGroupMembers.Name = "btn_GetGroupMembers";
            btn_GetGroupMembers.Size = new Size(110, 23);
            btn_GetGroupMembers.TabIndex = 4;
            btn_GetGroupMembers.Text = "获取组成员";
            btn_GetGroupMembers.UseVisualStyleBackColor = true;
            btn_GetGroupMembers.Click += btn_GetGroupMembers_Click;
            // 
            // btn_getContact
            // 
            btn_getContact.Location = new Point(79, 76);
            btn_getContact.Name = "btn_getContact";
            btn_getContact.Size = new Size(110, 23);
            btn_getContact.TabIndex = 2;
            btn_getContact.Text = "获取联系人";
            btn_getContact.UseVisualStyleBackColor = true;
            btn_getContact.Click += btn_getContact_Click;
            // 
            // btn_Broadcast
            // 
            btn_Broadcast.Location = new Point(79, 22);
            btn_Broadcast.Name = "btn_Broadcast";
            btn_Broadcast.Size = new Size(110, 23);
            btn_Broadcast.TabIndex = 1;
            btn_Broadcast.Text = "Broadcast";
            btn_Broadcast.UseVisualStyleBackColor = true;
            btn_Broadcast.Click += btn_Broadcast_Click;
            // 
            // txb_groupID
            // 
            txb_groupID.Location = new Point(8, 22);
            txb_groupID.Name = "txb_groupID";
            txb_groupID.PlaceholderText = "groupID";
            txb_groupID.Size = new Size(65, 23);
            txb_groupID.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btn_SetGroupTag);
            groupBox3.Controls.Add(btn_SyncGroupMembers);
            groupBox3.Controls.Add(btn_RemoveFromGroup);
            groupBox3.Controls.Add(btn_QuitGroup);
            groupBox3.Controls.Add(btn_PullIntoGroup);
            groupBox3.Controls.Add(btn_JoinGroup);
            groupBox3.Controls.Add(btn_InviteJoinGroup);
            groupBox3.Controls.Add(btn_GetGroup);
            groupBox3.Controls.Add(btn_GetGroupmates);
            groupBox3.Controls.Add(btn_GetMyGroups);
            groupBox3.Controls.Add(btn_Broadcast22);
            groupBox3.Controls.Add(txb_groupID22);
            groupBox3.Location = new Point(527, 24);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(217, 190);
            groupBox3.TabIndex = 10;
            groupBox3.TabStop = false;
            groupBox3.Text = "DynamicGroup";
            // 
            // btn_SetGroupTag
            // 
            btn_SetGroupTag.Location = new Point(119, 156);
            btn_SetGroupTag.Name = "btn_SetGroupTag";
            btn_SetGroupTag.Size = new Size(75, 23);
            btn_SetGroupTag.TabIndex = 8;
            btn_SetGroupTag.Text = "设置Tag";
            btn_SetGroupTag.UseVisualStyleBackColor = true;
            btn_SetGroupTag.Click += btn_SetGroupTag_Click;
            // 
            // btn_SyncGroupMembers
            // 
            btn_SyncGroupMembers.Location = new Point(119, 48);
            btn_SyncGroupMembers.Name = "btn_SyncGroupMembers";
            btn_SyncGroupMembers.Size = new Size(75, 23);
            btn_SyncGroupMembers.TabIndex = 9;
            btn_SyncGroupMembers.Text = "同步组信息";
            btn_SyncGroupMembers.UseVisualStyleBackColor = true;
            btn_SyncGroupMembers.Click += btn_SyncGroupMembers_Click;
            // 
            // btn_RemoveFromGroup
            // 
            btn_RemoveFromGroup.Location = new Point(14, 156);
            btn_RemoveFromGroup.Name = "btn_RemoveFromGroup";
            btn_RemoveFromGroup.Size = new Size(74, 23);
            btn_RemoveFromGroup.TabIndex = 8;
            btn_RemoveFromGroup.Text = "移人出组";
            btn_RemoveFromGroup.UseVisualStyleBackColor = true;
            btn_RemoveFromGroup.Click += btn_RemoveFromGroup_Click;
            // 
            // btn_QuitGroup
            // 
            btn_QuitGroup.Location = new Point(119, 102);
            btn_QuitGroup.Name = "btn_QuitGroup";
            btn_QuitGroup.Size = new Size(75, 23);
            btn_QuitGroup.TabIndex = 7;
            btn_QuitGroup.Text = "退出组";
            btn_QuitGroup.UseVisualStyleBackColor = true;
            btn_QuitGroup.Click += btn_QuitGroup_Click;
            // 
            // btn_PullIntoGroup
            // 
            btn_PullIntoGroup.Location = new Point(14, 129);
            btn_PullIntoGroup.Name = "btn_PullIntoGroup";
            btn_PullIntoGroup.Size = new Size(74, 23);
            btn_PullIntoGroup.TabIndex = 7;
            btn_PullIntoGroup.Text = "拉人入组";
            btn_PullIntoGroup.UseVisualStyleBackColor = true;
            btn_PullIntoGroup.Click += btn_PullIntoGroup_Click;
            // 
            // btn_JoinGroup
            // 
            btn_JoinGroup.Location = new Point(14, 102);
            btn_JoinGroup.Name = "btn_JoinGroup";
            btn_JoinGroup.Size = new Size(74, 23);
            btn_JoinGroup.TabIndex = 6;
            btn_JoinGroup.Text = "加入组";
            btn_JoinGroup.UseVisualStyleBackColor = true;
            btn_JoinGroup.Click += btn_JoinGroup_Click;
            // 
            // btn_InviteJoinGroup
            // 
            btn_InviteJoinGroup.Location = new Point(119, 129);
            btn_InviteJoinGroup.Name = "btn_InviteJoinGroup";
            btn_InviteJoinGroup.Size = new Size(75, 23);
            btn_InviteJoinGroup.TabIndex = 5;
            btn_InviteJoinGroup.Text = "邀请入组";
            btn_InviteJoinGroup.UseVisualStyleBackColor = true;
            btn_InviteJoinGroup.Click += btn_InviteJoinGroup_Click;
            // 
            // btn_GetGroup
            // 
            btn_GetGroup.Location = new Point(14, 48);
            btn_GetGroup.Name = "btn_GetGroup";
            btn_GetGroup.Size = new Size(74, 23);
            btn_GetGroup.TabIndex = 4;
            btn_GetGroup.Text = "组信息";
            btn_GetGroup.UseVisualStyleBackColor = true;
            btn_GetGroup.Click += btn_GetGroup_Click;
            // 
            // btn_GetGroupmates
            // 
            btn_GetGroupmates.Location = new Point(119, 75);
            btn_GetGroupmates.Name = "btn_GetGroupmates";
            btn_GetGroupmates.Size = new Size(75, 23);
            btn_GetGroupmates.TabIndex = 3;
            btn_GetGroupmates.Text = "在线组友";
            btn_GetGroupmates.UseVisualStyleBackColor = true;
            btn_GetGroupmates.Click += btn_GetGroupmates_Click;
            // 
            // btn_GetMyGroups
            // 
            btn_GetMyGroups.Location = new Point(14, 75);
            btn_GetMyGroups.Name = "btn_GetMyGroups";
            btn_GetMyGroups.Size = new Size(74, 23);
            btn_GetMyGroups.TabIndex = 2;
            btn_GetMyGroups.Text = "组列表";
            btn_GetMyGroups.UseVisualStyleBackColor = true;
            btn_GetMyGroups.Click += btn_GetMyGroups_Click;
            // 
            // btn_Broadcast22
            // 
            btn_Broadcast22.Location = new Point(119, 21);
            btn_Broadcast22.Name = "btn_Broadcast22";
            btn_Broadcast22.Size = new Size(75, 23);
            btn_Broadcast22.TabIndex = 1;
            btn_Broadcast22.Text = "Broadcast";
            btn_Broadcast22.UseVisualStyleBackColor = true;
            btn_Broadcast22.Click += btn_Broadcast22_Click;
            // 
            // txb_groupID22
            // 
            txb_groupID22.Location = new Point(14, 21);
            txb_groupID22.Name = "txb_groupID22";
            txb_groupID22.Size = new Size(74, 23);
            txb_groupID22.TabIndex = 0;
            txb_groupID22.Text = "g001";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(756, 498);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(txb_queryData);
            Controls.Add(txb_queryType);
            Controls.Add(btn_QueryBeforeLogin);
            Controls.Add(txb_pwd);
            Controls.Add(txb_userID);
            Controls.Add(txb_info);
            Controls.Add(txb_destUserID);
            Controls.Add(txb_msg);
            Controls.Add(btn_query);
            Controls.Add(button1);
            Controls.Add(btn_send);
            Controls.Add(btn_logon);
            Controls.Add(txb_port);
            Controls.Add(txb_ip);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txb_ip;
        private TextBox txb_port;
        private Button btn_logon;
        private TextBox txb_msg;
        private TextBox txb_destUserID;
        private Button btn_send;
        private RichTextBox txb_info;
        private Button btn_query;
        private TextBox txb_userID;
        private TextBox txb_pwd;
        private Button button1;
        private Button btn_QueryBeforeLogin;
        private TextBox txb_queryData;
        private TextBox txb_queryType;
        private GroupBox groupBox1;
        private Button btn_Broadcast;
        private Button btn_getIp;
        private Button btn_getDevice;
        private Button btn_sendHeartBeat;
        private Button btn_tickOut;
        private TextBox txb_destID;
        private GroupBox groupBox2;
        private TextBox txb_groupID;
        private Button btn_getContact;
        private Button btn_GetGroupMembers;
        private CheckBox ckb_isOnline;
        private GroupBox groupBox3;
        private Button btn_Broadcast22;
        private TextBox txb_groupID22;
        private Button btn_GetMyGroups;
        private Button btn_GetGroupmates;
        private Button btn_GetGroup;
        private Button btn_InviteJoinGroup;
        private Button btn_QuitGroup;
        private Button btn_PullIntoGroup;
        private Button btn_JoinGroup;
        private Button btn_RemoveFromGroup;
        private Button btn_SyncGroupMembers;
        private Button btn_SetGroupTag;
    }
}
