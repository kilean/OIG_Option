using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace OIG_Option
{
    public partial class Fo_CncConnect : Form
    {
        public Fo_CncConnect()
        {
            InitializeComponent();


            TIniFile ini = new TIniFile(Application.StartupPath + "\\sys.ini");
            TB_IP.Text = ini.ReadString("CNC", "IP", "192.168.168.2");
            TB_Port.Text = ini.ReadString("CNC", "Port", "8193");
            int Mode = ini.ReadInteger("CNC", "Mode", 0);
            if (Mode == 0) rb_Ethernet.Checked = true;
            else rb_HSSB.Checked = true;

            panel1.Dock = DockStyle.Fill;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_OK.PerformClick();
            }
        }

        private void Fo_CncConnect_Load(object sender, EventArgs e)
        {
            this.LoadLanguageFile(Units.langfile, this.Name);
        }
    }
}
