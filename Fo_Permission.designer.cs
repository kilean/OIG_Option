namespace OIG_Option
{
    partial class Fo_Permission
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fo_Permission));
            this.label1 = new System.Windows.Forms.Label();
            this.TB_ID = new System.Windows.Forms.TextBox();
            this.TB_PSWD = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_Login = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // TB_ID
            // 
            resources.ApplyResources(this.TB_ID, "TB_ID");
            this.TB_ID.Name = "TB_ID";
            this.TB_ID.Click += new System.EventHandler(this.TB_ID_Click);
            this.TB_ID.TextChanged += new System.EventHandler(this.TB_ID_TextChanged);
            this.TB_ID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_ID_KeyDown);
            this.TB_ID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_ID_KeyPress);
            // 
            // TB_PSWD
            // 
            resources.ApplyResources(this.TB_PSWD, "TB_PSWD");
            this.TB_PSWD.Name = "TB_PSWD";
            this.TB_PSWD.Click += new System.EventHandler(this.TB_PSWD_Click);
            this.TB_PSWD.TextChanged += new System.EventHandler(this.TB_PSWD_TextChanged);
            this.TB_PSWD.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_PSWD_KeyDown);
            this.TB_PSWD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_PSWD_KeyPress);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // Btn_Login
            // 
            this.Btn_Login.BackColor = System.Drawing.Color.Lime;
            resources.ApplyResources(this.Btn_Login, "Btn_Login");
            this.Btn_Login.ForeColor = System.Drawing.Color.White;
            this.Btn_Login.Name = "Btn_Login";
            this.Btn_Login.UseVisualStyleBackColor = false;
            this.Btn_Login.Click += new System.EventHandler(this.Btn_Login_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.button1, "button1");
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Fo_Permission
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DodgerBlue;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Btn_Login);
            this.Controls.Add(this.TB_PSWD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TB_ID);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Fo_Permission";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Btn_Login;
        public System.Windows.Forms.TextBox TB_ID;
        public System.Windows.Forms.TextBox TB_PSWD;
        private System.Windows.Forms.Button button1;
    }
}