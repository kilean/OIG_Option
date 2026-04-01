
partial class Fo_MultLangEdit
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btn_Reresh = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_Export = new System.Windows.Forms.Button();
            this.btn_Apply = new System.Windows.Forms.Button();
            this.btn_Search = new System.Windows.Forms.Button();
            this.la_Search = new System.Windows.Forms.Label();
            this.tb_Search = new System.Windows.Forms.TextBox();
            this.btn_Next = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmi_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Search = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Next = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 72);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(808, 528);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(832, 32);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(168, 564);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            // 
            // btn_Reresh
            // 
            this.btn_Reresh.Location = new System.Drawing.Point(912, 600);
            this.btn_Reresh.Name = "btn_Reresh";
            this.btn_Reresh.Size = new System.Drawing.Size(88, 32);
            this.btn_Reresh.TabIndex = 2;
            this.btn_Reresh.Text = "刷新";
            this.btn_Reresh.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 608);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(808, 120);
            this.textBox1.TabIndex = 3;
            this.textBox1.WordWrap = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // btn_Export
            // 
            this.btn_Export.Location = new System.Drawing.Point(912, 640);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(88, 32);
            this.btn_Export.TabIndex = 4;
            this.btn_Export.Text = "匯出";
            this.btn_Export.UseVisualStyleBackColor = true;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // btn_Apply
            // 
            this.btn_Apply.Location = new System.Drawing.Point(912, 680);
            this.btn_Apply.Name = "btn_Apply";
            this.btn_Apply.Size = new System.Drawing.Size(88, 32);
            this.btn_Apply.TabIndex = 5;
            this.btn_Apply.Text = "套用";
            this.btn_Apply.UseVisualStyleBackColor = true;
            this.btn_Apply.Click += new System.EventHandler(this.btn_Apply_Click);
            // 
            // btn_Search
            // 
            this.btn_Search.Location = new System.Drawing.Point(232, 32);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(104, 32);
            this.btn_Search.TabIndex = 6;
            this.btn_Search.Text = "搜尋";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // la_Search
            // 
            this.la_Search.AutoSize = true;
            this.la_Search.Location = new System.Drawing.Point(16, 32);
            this.la_Search.Name = "la_Search";
            this.la_Search.Size = new System.Drawing.Size(41, 20);
            this.la_Search.TabIndex = 7;
            this.la_Search.Text = "搜尋";
            // 
            // tb_Search
            // 
            this.tb_Search.Location = new System.Drawing.Point(64, 32);
            this.tb_Search.Name = "tb_Search";
            this.tb_Search.Size = new System.Drawing.Size(168, 29);
            this.tb_Search.TabIndex = 8;
            this.tb_Search.TextChanged += new System.EventHandler(this.tb_Search_TextChanged);
            this.tb_Search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_Search_KeyDown);
            // 
            // btn_Next
            // 
            this.btn_Next.Location = new System.Drawing.Point(336, 32);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(104, 32);
            this.btn_Next.TabIndex = 9;
            this.btn_Next.Text = "下一個";
            this.btn_Next.UseVisualStyleBackColor = true;
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(704, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 24);
            this.label1.TabIndex = 10;
            this.label1.Text = "-";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Delete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(99, 26);
            // 
            // tsmi_Delete
            // 
            this.tsmi_Delete.Name = "tsmi_Delete";
            this.tsmi_Delete.Size = new System.Drawing.Size(98, 22);
            this.tsmi_Delete.Text = "刪除";
            this.tsmi_Delete.Click += new System.EventHandler(this.tsmi_Delete_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Edit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmi_Edit
            // 
            this.tsmi_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Search,
            this.tsmi_Next});
            this.tsmi_Edit.Name = "tsmi_Edit";
            this.tsmi_Edit.Size = new System.Drawing.Size(58, 20);
            this.tsmi_Edit.Text = "編輯(&E)";
            // 
            // tsmi_Search
            // 
            this.tsmi_Search.Name = "tsmi_Search";
            this.tsmi_Search.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.tsmi_Search.Size = new System.Drawing.Size(180, 22);
            this.tsmi_Search.Text = "搜尋";
            this.tsmi_Search.Click += new System.EventHandler(this.tsmi_Search_Click);
            // 
            // tsmi_Next
            // 
            this.tsmi_Next.Name = "tsmi_Next";
            this.tsmi_Next.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.tsmi_Next.Size = new System.Drawing.Size(180, 22);
            this.tsmi_Next.Text = "下一個";
            this.tsmi_Next.Click += new System.EventHandler(this.tsmi_Next_Click);
            // 
            // Fo_MultLangEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Next);
            this.Controls.Add(this.tb_Search);
            this.Controls.Add(this.la_Search);
            this.Controls.Add(this.btn_Search);
            this.Controls.Add(this.btn_Apply);
            this.Controls.Add(this.btn_Export);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_Reresh);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Fo_MultLangEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "多國語言編輯器";
            this.Load += new System.EventHandler(this.Fo_MultLangEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.Button btn_Reresh;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button btn_Export;
    private System.Windows.Forms.Button btn_Apply;
    private System.Windows.Forms.Button btn_Search;
    private System.Windows.Forms.Label la_Search;
    private System.Windows.Forms.TextBox tb_Search;
    private System.Windows.Forms.Button btn_Next;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem tsmi_Delete;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem tsmi_Edit;
    private System.Windows.Forms.ToolStripMenuItem tsmi_Search;
    private System.Windows.Forms.ToolStripMenuItem tsmi_Next;
}
