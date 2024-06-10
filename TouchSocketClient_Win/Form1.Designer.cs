namespace TouchSocketClient_Win
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
            btn_connect = new Button();
            txb_msg = new TextBox();
            btn_send = new Button();
            btn_disconnect = new Button();
            txb_info = new RichTextBox();
            saveFileDialog1 = new SaveFileDialog();
            button_query = new Button();
            txb_port = new TextBox();
            SuspendLayout();
            // 
            // txb_ip
            // 
            txb_ip.Location = new Point(26, 40);
            txb_ip.Name = "txb_ip";
            txb_ip.Size = new Size(85, 23);
            txb_ip.TabIndex = 0;
            txb_ip.Text = "127.0.0.1";
            // 
            // btn_connect
            // 
            btn_connect.Location = new Point(193, 40);
            btn_connect.Name = "btn_connect";
            btn_connect.Size = new Size(75, 23);
            btn_connect.TabIndex = 1;
            btn_connect.Text = "连接";
            btn_connect.UseVisualStyleBackColor = true;
            btn_connect.Click += btn_connect_Click;
            // 
            // txb_msg
            // 
            txb_msg.Location = new Point(25, 86);
            txb_msg.Multiline = true;
            txb_msg.Name = "txb_msg";
            txb_msg.Size = new Size(143, 89);
            txb_msg.TabIndex = 2;
            // 
            // btn_send
            // 
            btn_send.Location = new Point(193, 86);
            btn_send.Name = "btn_send";
            btn_send.Size = new Size(75, 23);
            btn_send.TabIndex = 4;
            btn_send.Text = "发送";
            btn_send.UseVisualStyleBackColor = true;
            btn_send.Click += btn_send_Click;
            // 
            // btn_disconnect
            // 
            btn_disconnect.Location = new Point(274, 40);
            btn_disconnect.Name = "btn_disconnect";
            btn_disconnect.Size = new Size(75, 23);
            btn_disconnect.TabIndex = 1;
            btn_disconnect.Text = "断开";
            btn_disconnect.UseVisualStyleBackColor = true;
            btn_disconnect.Click += btn_disconnect_Click;
            // 
            // txb_info
            // 
            txb_info.Dock = DockStyle.Bottom;
            txb_info.Location = new Point(0, 197);
            txb_info.Name = "txb_info";
            txb_info.Size = new Size(800, 253);
            txb_info.TabIndex = 5;
            txb_info.Text = "";
            // 
            // button_query
            // 
            button_query.Location = new Point(194, 121);
            button_query.Name = "button_query";
            button_query.Size = new Size(75, 23);
            button_query.TabIndex = 6;
            button_query.Text = "Query";
            button_query.UseVisualStyleBackColor = true;
            button_query.Click += button_query_Click;
            // 
            // txb_port
            // 
            txb_port.Location = new Point(117, 40);
            txb_port.Name = "txb_port";
            txb_port.Size = new Size(49, 23);
            txb_port.TabIndex = 7;
            txb_port.Text = "4530";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txb_port);
            Controls.Add(button_query);
            Controls.Add(txb_info);
            Controls.Add(btn_send);
            Controls.Add(txb_msg);
            Controls.Add(btn_disconnect);
            Controls.Add(btn_connect);
            Controls.Add(txb_ip);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txb_ip;
        private Button btn_connect;
        private TextBox txb_msg;
        private Button btn_send;
        private Button btn_disconnect;
        private RichTextBox txb_info;
        private SaveFileDialog saveFileDialog1;
        private Button button_query;
        private TextBox txb_port;
    }
}