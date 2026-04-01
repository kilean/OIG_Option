 namespace OIG_Option
{
    partial class Fo_Logo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fo_Logo));
            this.pic_Logo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pa_NoSignal = new System.Windows.Forms.Panel();
            this.btn_NoSignal = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Logo)).BeginInit();
            this.pa_NoSignal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pic_Logo
            // 
            this.pic_Logo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pic_Logo.Image = ((System.Drawing.Image)(resources.GetObject("pic_Logo.Image")));
            this.pic_Logo.Location = new System.Drawing.Point(0, 0);
            this.pic_Logo.Name = "pic_Logo";
            this.pic_Logo.Size = new System.Drawing.Size(695, 474);
            this.pic_Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_Logo.TabIndex = 0;
            this.pic_Logo.TabStop = false;
            this.pic_Logo.Visible = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 48);
            this.label1.TabIndex = 1;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label1_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pa_NoSignal
            // 
            this.pa_NoSignal.Controls.Add(this.btn_NoSignal);
            this.pa_NoSignal.Location = new System.Drawing.Point(168, 112);
            this.pa_NoSignal.Name = "pa_NoSignal";
            this.pa_NoSignal.Size = new System.Drawing.Size(336, 224);
            this.pa_NoSignal.TabIndex = 2;
            this.pa_NoSignal.Visible = false;
            // 
            // btn_NoSignal
            // 
            this.btn_NoSignal.BackColor = System.Drawing.Color.Black;
            this.btn_NoSignal.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.btn_NoSignal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_NoSignal.Font = new System.Drawing.Font("微軟正黑體", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_NoSignal.ForeColor = System.Drawing.Color.Blue;
            this.btn_NoSignal.Location = new System.Drawing.Point(8, 8);
            this.btn_NoSignal.Name = "btn_NoSignal";
            this.btn_NoSignal.Size = new System.Drawing.Size(320, 200);
            this.btn_NoSignal.TabIndex = 0;
            this.btn_NoSignal.Text = "FANUC\r\nNO SIGNAL";
            this.btn_NoSignal.UseVisualStyleBackColor = false;
            this.btn_NoSignal.Click += new System.EventHandler(this.btn_NoSignal_Click);
            // 
            // Fo_Logo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(695, 474);
            this.Controls.Add(this.pa_NoSignal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pic_Logo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Fo_Logo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fo_Logo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Fo_Logo_FormClosing);
            this.Load += new System.EventHandler(this.Fo_Logo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Logo)).EndInit();
            this.pa_NoSignal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pic_Logo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_NoSignal;
        public System.Windows.Forms.Panel pa_NoSignal;
    }
}