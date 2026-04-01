    namespace OIG_Option
{
    partial class Fo_Num
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
            this.uc_UserNum1 = new UserNumLib.Uc_UserNum();
            this.SuspendLayout();
            // 
            // uc_UserNum1
            // 
            this.uc_UserNum1.AllowDrop = true;
            this.uc_UserNum1.BackColor = System.Drawing.Color.Black;
            this.uc_UserNum1.Location = new System.Drawing.Point(0, 0);
            this.uc_UserNum1.Margin = new System.Windows.Forms.Padding(0);
            this.uc_UserNum1.Name = "uc_UserNum1";
            this.uc_UserNum1.ShowCancelBtn = true;
            this.uc_UserNum1.ShowMemoryBtn = false;
            this.uc_UserNum1.Size = new System.Drawing.Size(440, 280);
            this.uc_UserNum1.TabIndex = 0;
            this.uc_UserNum1.OnBtnOkClick += new System.EventHandler(this.uc_UserNum1_OnBtnOkClick);
            this.uc_UserNum1.OnBtnCancelClick += new System.EventHandler(this.uc_UserNum1_OnBtnCancelClick);
            // 
            // Fo_Num
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(450, 289);
            this.Controls.Add(this.uc_UserNum1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Fo_Num";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "數字鍵盤";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Fo_Num_Load);
            this.ResumeLayout(false);

        }

        #endregion


        public UserNumLib.Uc_UserNum uc_UserNum1;
    }
}