namespace OIG_Option
{
    partial class Fo_Msg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fo_Msg));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pa_Text = new System.Windows.Forms.Panel();
            this.la_Message = new System.Windows.Forms.Label();
            this.pa_OK = new System.Windows.Forms.Panel();
            this.btn_OK1 = new System.Windows.Forms.Button();
            this.pa_OKCancel = new System.Windows.Forms.Panel();
            this.btn_Cancel1 = new System.Windows.Forms.Button();
            this.btn_OK2 = new System.Windows.Forms.Button();
            this.pa_AbortRetryIgnore = new System.Windows.Forms.Panel();
            this.btn_Abort = new System.Windows.Forms.Button();
            this.btn_Ignore = new System.Windows.Forms.Button();
            this.btn_Retry1 = new System.Windows.Forms.Button();
            this.pa_YesNoCancel = new System.Windows.Forms.Panel();
            this.btn_No1 = new System.Windows.Forms.Button();
            this.btn_Cancel2 = new System.Windows.Forms.Button();
            this.btn_Yes1 = new System.Windows.Forms.Button();
            this.pa_YesNo = new System.Windows.Forms.Panel();
            this.btn_No2 = new System.Windows.Forms.Button();
            this.btn_Yes2 = new System.Windows.Forms.Button();
            this.pa_RetryCancel = new System.Windows.Forms.Panel();
            this.btn_Cancel3 = new System.Windows.Forms.Button();
            this.btn_Retyr2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pa_Text.SuspendLayout();
            this.pa_OK.SuspendLayout();
            this.pa_OKCancel.SuspendLayout();
            this.pa_AbortRetryIgnore.SuspendLayout();
            this.pa_YesNoCancel.SuspendLayout();
            this.pa_YesNo.SuspendLayout();
            this.pa_RetryCancel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(24, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ico81.ico");
            this.imageList1.Images.SetKeyName(1, "ico84.ico");
            this.imageList1.Images.SetKeyName(2, "ico98.ico");
            this.imageList1.Images.SetKeyName(3, "ico99.ico");
            // 
            // pa_Text
            // 
            this.pa_Text.BackColor = System.Drawing.Color.White;
            this.pa_Text.Controls.Add(this.la_Message);
            this.pa_Text.Controls.Add(this.pictureBox1);
            this.pa_Text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pa_Text.Location = new System.Drawing.Point(0, 0);
            this.pa_Text.Name = "pa_Text";
            this.pa_Text.Size = new System.Drawing.Size(342, 87);
            this.pa_Text.TabIndex = 3;
            // 
            // la_Message
            // 
            this.la_Message.AutoSize = true;
            this.la_Message.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.la_Message.Location = new System.Drawing.Point(64, 40);
            this.la_Message.Name = "la_Message";
            this.la_Message.Size = new System.Drawing.Size(38, 20);
            this.la_Message.TabIndex = 2;
            this.la_Message.Text = "text";
            // 
            // pa_OK
            // 
            this.pa_OK.Controls.Add(this.btn_OK1);
            this.pa_OK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pa_OK.Location = new System.Drawing.Point(0, 327);
            this.pa_OK.Name = "pa_OK";
            this.pa_OK.Size = new System.Drawing.Size(342, 48);
            this.pa_OK.TabIndex = 6;
            this.pa_OK.Visible = false;
            // 
            // btn_OK1
            // 
            this.btn_OK1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OK1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_OK1.Location = new System.Drawing.Point(245, 8);
            this.btn_OK1.Name = "btn_OK1";
            this.btn_OK1.Size = new System.Drawing.Size(88, 32);
            this.btn_OK1.TabIndex = 2;
            this.btn_OK1.Text = "確定";
            this.btn_OK1.UseVisualStyleBackColor = true;
            this.btn_OK1.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // pa_OKCancel
            // 
            this.pa_OKCancel.Controls.Add(this.btn_Cancel1);
            this.pa_OKCancel.Controls.Add(this.btn_OK2);
            this.pa_OKCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pa_OKCancel.Location = new System.Drawing.Point(0, 279);
            this.pa_OKCancel.Name = "pa_OKCancel";
            this.pa_OKCancel.Size = new System.Drawing.Size(342, 48);
            this.pa_OKCancel.TabIndex = 7;
            this.pa_OKCancel.Visible = false;
            // 
            // btn_Cancel1
            // 
            this.btn_Cancel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancel1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Cancel1.Location = new System.Drawing.Point(245, 8);
            this.btn_Cancel1.Name = "btn_Cancel1";
            this.btn_Cancel1.Size = new System.Drawing.Size(88, 32);
            this.btn_Cancel1.TabIndex = 3;
            this.btn_Cancel1.Text = "取消";
            this.btn_Cancel1.UseVisualStyleBackColor = true;
            this.btn_Cancel1.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // btn_OK2
            // 
            this.btn_OK2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OK2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_OK2.Location = new System.Drawing.Point(149, 8);
            this.btn_OK2.Name = "btn_OK2";
            this.btn_OK2.Size = new System.Drawing.Size(88, 32);
            this.btn_OK2.TabIndex = 2;
            this.btn_OK2.Text = "確定";
            this.btn_OK2.UseVisualStyleBackColor = true;
            this.btn_OK2.Click += new System.EventHandler(this.Btn_OK_Click);
            // 
            // pa_AbortRetryIgnore
            // 
            this.pa_AbortRetryIgnore.Controls.Add(this.btn_Abort);
            this.pa_AbortRetryIgnore.Controls.Add(this.btn_Ignore);
            this.pa_AbortRetryIgnore.Controls.Add(this.btn_Retry1);
            this.pa_AbortRetryIgnore.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pa_AbortRetryIgnore.Location = new System.Drawing.Point(0, 231);
            this.pa_AbortRetryIgnore.Name = "pa_AbortRetryIgnore";
            this.pa_AbortRetryIgnore.Size = new System.Drawing.Size(342, 48);
            this.pa_AbortRetryIgnore.TabIndex = 8;
            this.pa_AbortRetryIgnore.Visible = false;
            // 
            // btn_Abort
            // 
            this.btn_Abort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Abort.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Abort.Location = new System.Drawing.Point(53, 8);
            this.btn_Abort.Name = "btn_Abort";
            this.btn_Abort.Size = new System.Drawing.Size(88, 32);
            this.btn_Abort.TabIndex = 4;
            this.btn_Abort.Text = "中止";
            this.btn_Abort.UseVisualStyleBackColor = true;
            this.btn_Abort.Click += new System.EventHandler(this.btn_Abort_Click);
            // 
            // btn_Ignore
            // 
            this.btn_Ignore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Ignore.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Ignore.Location = new System.Drawing.Point(245, 8);
            this.btn_Ignore.Name = "btn_Ignore";
            this.btn_Ignore.Size = new System.Drawing.Size(88, 32);
            this.btn_Ignore.TabIndex = 3;
            this.btn_Ignore.Text = "忽略";
            this.btn_Ignore.UseVisualStyleBackColor = true;
            this.btn_Ignore.Click += new System.EventHandler(this.btn_Ignore_Click);
            // 
            // btn_Retry1
            // 
            this.btn_Retry1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Retry1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Retry1.Location = new System.Drawing.Point(149, 8);
            this.btn_Retry1.Name = "btn_Retry1";
            this.btn_Retry1.Size = new System.Drawing.Size(88, 32);
            this.btn_Retry1.TabIndex = 2;
            this.btn_Retry1.Text = "重試";
            this.btn_Retry1.UseVisualStyleBackColor = true;
            this.btn_Retry1.Click += new System.EventHandler(this.Btn_Retry_Click);
            // 
            // pa_YesNoCancel
            // 
            this.pa_YesNoCancel.Controls.Add(this.btn_No1);
            this.pa_YesNoCancel.Controls.Add(this.btn_Cancel2);
            this.pa_YesNoCancel.Controls.Add(this.btn_Yes1);
            this.pa_YesNoCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pa_YesNoCancel.Location = new System.Drawing.Point(0, 183);
            this.pa_YesNoCancel.Name = "pa_YesNoCancel";
            this.pa_YesNoCancel.Size = new System.Drawing.Size(342, 48);
            this.pa_YesNoCancel.TabIndex = 9;
            this.pa_YesNoCancel.Visible = false;
            // 
            // btn_No1
            // 
            this.btn_No1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_No1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_No1.Location = new System.Drawing.Point(150, 8);
            this.btn_No1.Name = "btn_No1";
            this.btn_No1.Size = new System.Drawing.Size(88, 32);
            this.btn_No1.TabIndex = 4;
            this.btn_No1.Text = "否";
            this.btn_No1.UseVisualStyleBackColor = true;
            this.btn_No1.Click += new System.EventHandler(this.btn_No_Click);
            // 
            // btn_Cancel2
            // 
            this.btn_Cancel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancel2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Cancel2.Location = new System.Drawing.Point(245, 8);
            this.btn_Cancel2.Name = "btn_Cancel2";
            this.btn_Cancel2.Size = new System.Drawing.Size(88, 32);
            this.btn_Cancel2.TabIndex = 3;
            this.btn_Cancel2.Text = "取消";
            this.btn_Cancel2.UseVisualStyleBackColor = true;
            this.btn_Cancel2.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // btn_Yes1
            // 
            this.btn_Yes1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Yes1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Yes1.Location = new System.Drawing.Point(54, 8);
            this.btn_Yes1.Name = "btn_Yes1";
            this.btn_Yes1.Size = new System.Drawing.Size(88, 32);
            this.btn_Yes1.TabIndex = 2;
            this.btn_Yes1.Text = "是";
            this.btn_Yes1.UseVisualStyleBackColor = true;
            this.btn_Yes1.Click += new System.EventHandler(this.Btn_Yes_Click);
            // 
            // pa_YesNo
            // 
            this.pa_YesNo.Controls.Add(this.btn_No2);
            this.pa_YesNo.Controls.Add(this.btn_Yes2);
            this.pa_YesNo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pa_YesNo.Location = new System.Drawing.Point(0, 135);
            this.pa_YesNo.Name = "pa_YesNo";
            this.pa_YesNo.Size = new System.Drawing.Size(342, 48);
            this.pa_YesNo.TabIndex = 10;
            this.pa_YesNo.Visible = false;
            // 
            // btn_No2
            // 
            this.btn_No2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_No2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_No2.Location = new System.Drawing.Point(245, 8);
            this.btn_No2.Name = "btn_No2";
            this.btn_No2.Size = new System.Drawing.Size(88, 32);
            this.btn_No2.TabIndex = 3;
            this.btn_No2.Text = "否";
            this.btn_No2.UseVisualStyleBackColor = true;
            this.btn_No2.Click += new System.EventHandler(this.btn_No_Click);
            // 
            // btn_Yes2
            // 
            this.btn_Yes2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Yes2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Yes2.Location = new System.Drawing.Point(149, 8);
            this.btn_Yes2.Name = "btn_Yes2";
            this.btn_Yes2.Size = new System.Drawing.Size(88, 32);
            this.btn_Yes2.TabIndex = 2;
            this.btn_Yes2.Text = "是";
            this.btn_Yes2.UseVisualStyleBackColor = true;
            this.btn_Yes2.Click += new System.EventHandler(this.Btn_Yes_Click);
            // 
            // pa_RetryCancel
            // 
            this.pa_RetryCancel.Controls.Add(this.btn_Cancel3);
            this.pa_RetryCancel.Controls.Add(this.btn_Retyr2);
            this.pa_RetryCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pa_RetryCancel.Location = new System.Drawing.Point(0, 87);
            this.pa_RetryCancel.Name = "pa_RetryCancel";
            this.pa_RetryCancel.Size = new System.Drawing.Size(342, 48);
            this.pa_RetryCancel.TabIndex = 11;
            this.pa_RetryCancel.Visible = false;
            // 
            // btn_Cancel3
            // 
            this.btn_Cancel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancel3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Cancel3.Location = new System.Drawing.Point(248, 8);
            this.btn_Cancel3.Name = "btn_Cancel3";
            this.btn_Cancel3.Size = new System.Drawing.Size(88, 32);
            this.btn_Cancel3.TabIndex = 3;
            this.btn_Cancel3.Text = "取消";
            this.btn_Cancel3.UseVisualStyleBackColor = true;
            this.btn_Cancel3.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // btn_Retyr2
            // 
            this.btn_Retyr2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Retyr2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Retyr2.Location = new System.Drawing.Point(152, 8);
            this.btn_Retyr2.Name = "btn_Retyr2";
            this.btn_Retyr2.Size = new System.Drawing.Size(88, 32);
            this.btn_Retyr2.TabIndex = 2;
            this.btn_Retyr2.Text = "重試";
            this.btn_Retyr2.UseVisualStyleBackColor = true;
            this.btn_Retyr2.Click += new System.EventHandler(this.Btn_Retry_Click);
            // 
            // Fo_Msg
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(342, 375);
            this.Controls.Add(this.pa_Text);
            this.Controls.Add(this.pa_RetryCancel);
            this.Controls.Add(this.pa_YesNo);
            this.Controls.Add(this.pa_YesNoCancel);
            this.Controls.Add(this.pa_AbortRetryIgnore);
            this.Controls.Add(this.pa_OKCancel);
            this.Controls.Add(this.pa_OK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Fo_Msg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "caption";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pa_Text.ResumeLayout(false);
            this.pa_Text.PerformLayout();
            this.pa_OK.ResumeLayout(false);
            this.pa_OKCancel.ResumeLayout(false);
            this.pa_AbortRetryIgnore.ResumeLayout(false);
            this.pa_YesNoCancel.ResumeLayout(false);
            this.pa_YesNo.ResumeLayout(false);
            this.pa_RetryCancel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel pa_Text;
        private System.Windows.Forms.Panel pa_OK;
        private System.Windows.Forms.Button btn_OK1;
        private System.Windows.Forms.Panel pa_OKCancel;
        private System.Windows.Forms.Button btn_Cancel1;
        private System.Windows.Forms.Button btn_OK2;
        private System.Windows.Forms.Panel pa_AbortRetryIgnore;
        private System.Windows.Forms.Button btn_Abort;
        private System.Windows.Forms.Button btn_Ignore;
        private System.Windows.Forms.Button btn_Retry1;
        private System.Windows.Forms.Panel pa_YesNoCancel;
        private System.Windows.Forms.Button btn_Cancel2;
        private System.Windows.Forms.Button btn_Yes1;
        private System.Windows.Forms.Panel pa_YesNo;
        private System.Windows.Forms.Button btn_No2;
        private System.Windows.Forms.Button btn_Yes2;
        private System.Windows.Forms.Panel pa_RetryCancel;
        private System.Windows.Forms.Button btn_Cancel3;
        private System.Windows.Forms.Button btn_Retyr2;
        private System.Windows.Forms.Button btn_No1;
        private System.Windows.Forms.Label la_Message;
    }
}