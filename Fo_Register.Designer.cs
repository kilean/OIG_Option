
namespace OIG_Option
{
    partial class Fo_Register
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fo_Register));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Keygen = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.la_SN = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(312, 56);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "SN : ";
            // 
            // tb_Keygen
            // 
            this.tb_Keygen.Location = new System.Drawing.Point(72, 96);
            this.tb_Keygen.Name = "tb_Keygen";
            this.tb_Keygen.Size = new System.Drawing.Size(240, 22);
            this.tb_Keygen.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Keygen : ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(320, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 40);
            this.button1.TabIndex = 4;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // la_SN
            // 
            this.la_SN.AutoSize = true;
            this.la_SN.Location = new System.Drawing.Point(72, 72);
            this.la_SN.Name = "la_SN";
            this.la_SN.Size = new System.Drawing.Size(9, 12);
            this.la_SN.TabIndex = 5;
            this.la_SN.Text = "-";
            // 
            // Fo_Register
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(407, 151);
            this.Controls.Add(this.la_SN);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_Keygen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Fo_Register";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "註冊";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox tb_Keygen;
        public System.Windows.Forms.Label la_SN;
    }
}