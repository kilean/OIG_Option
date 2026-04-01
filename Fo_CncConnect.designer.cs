namespace OIG_Option
{
    partial class Fo_CncConnect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.la_ConnectTitle = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_Port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_IP = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_HSSB = new System.Windows.Forms.RadioButton();
            this.rb_Ethernet = new System.Windows.Forms.RadioButton();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // la_ConnectTitle
            // 
            this.la_ConnectTitle.BackColor = System.Drawing.Color.Blue;
            this.la_ConnectTitle.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.la_ConnectTitle.ForeColor = System.Drawing.Color.White;
            this.la_ConnectTitle.Location = new System.Drawing.Point(0, 0);
            this.la_ConnectTitle.Name = "la_ConnectTitle";
            this.la_ConnectTitle.Size = new System.Drawing.Size(471, 48);
            this.la_ConnectTitle.TabIndex = 19;
            this.la_ConnectTitle.Text = "連線設定";
            this.la_ConnectTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_OK
            // 
            this.btn_OK.BackColor = System.Drawing.Color.Lime;
            this.btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OK.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_OK.ForeColor = System.Drawing.Color.White;
            this.btn_OK.Location = new System.Drawing.Point(287, 62);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(162, 42);
            this.btn_OK.TabIndex = 24;
            this.btn_OK.TabStop = false;
            this.btn_OK.Text = "確定";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(20, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 20;
            this.label1.Text = "IP";
            // 
            // TB_Port
            // 
            this.TB_Port.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TB_Port.Location = new System.Drawing.Point(117, 101);
            this.TB_Port.Name = "TB_Port";
            this.TB_Port.Size = new System.Drawing.Size(151, 33);
            this.TB_Port.TabIndex = 23;
            this.TB_Port.Text = "8193";
            this.TB_Port.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(20, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 21);
            this.label2.TabIndex = 21;
            this.label2.Text = "Port";
            // 
            // TB_IP
            // 
            this.TB_IP.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TB_IP.Location = new System.Drawing.Point(117, 62);
            this.TB_IP.Name = "TB_IP";
            this.TB_IP.Size = new System.Drawing.Size(151, 33);
            this.TB_IP.TabIndex = 22;
            this.TB_IP.Text = "192.168.168.2";
            this.TB_IP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btn_Cancel);
            this.panel1.Controls.Add(this.la_ConnectTitle);
            this.panel1.Controls.Add(this.btn_OK);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TB_Port);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.TB_IP);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(464, 280);
            this.panel1.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_HSSB);
            this.groupBox1.Controls.Add(this.rb_Ethernet);
            this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(8, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 128);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "連線模式";
            // 
            // rb_HSSB
            // 
            this.rb_HSSB.AutoSize = true;
            this.rb_HSSB.Location = new System.Drawing.Point(24, 88);
            this.rb_HSSB.Name = "rb_HSSB";
            this.rb_HSSB.Size = new System.Drawing.Size(67, 24);
            this.rb_HSSB.TabIndex = 1;
            this.rb_HSSB.Text = "HSSB";
            this.rb_HSSB.UseVisualStyleBackColor = true;
            // 
            // rb_Ethernet
            // 
            this.rb_Ethernet.AutoSize = true;
            this.rb_Ethernet.Checked = true;
            this.rb_Ethernet.Location = new System.Drawing.Point(24, 48);
            this.rb_Ethernet.Name = "rb_Ethernet";
            this.rb_Ethernet.Size = new System.Drawing.Size(92, 24);
            this.rb_Ethernet.TabIndex = 0;
            this.rb_Ethernet.TabStop = true;
            this.rb_Ethernet.Text = "Ethernet";
            this.rb_Ethernet.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.BackColor = System.Drawing.Color.Red;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Cancel.ForeColor = System.Drawing.Color.White;
            this.btn_Cancel.Location = new System.Drawing.Point(288, 112);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(162, 40);
            this.btn_Cancel.TabIndex = 25;
            this.btn_Cancel.TabStop = false;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // Fo_CncConnect
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(464, 290);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Fo_CncConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fo_CncConnect";
            this.Load += new System.EventHandler(this.Fo_CncConnect_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label la_ConnectTitle;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox TB_Port;
        public System.Windows.Forms.TextBox TB_IP;
        public System.Windows.Forms.RadioButton rb_HSSB;
        public System.Windows.Forms.RadioButton rb_Ethernet;
    }
}