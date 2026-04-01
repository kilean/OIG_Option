using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OIG_Option
{
    public partial class Fo_Permission :Form
    {
        public Fo_Permission()
        {
            InitializeComponent();
            this.LoadLanguageFile(Units.langfile, this.Name);
        }

        private void Btn_Login_Click(Object sender, EventArgs e)
        {


            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void TB_PSWD_KeyPress(Object sender, KeyPressEventArgs e)
        {

        }

        private void TB_ID_KeyPress(Object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Btn_Login.PerformClick();
            }
        }

        private void button1_Click(Object sender, EventArgs e)
        {
            this.Close();
        }

        private void TB_ID_TextChanged(object sender, EventArgs e)
        {

            
            
        }

        private void TB_PSWD_TextChanged(object sender, EventArgs e)
        {

        }

        private void TB_ID_Click(object sender, EventArgs e)
        {
            Fo_Keyboard form = new Fo_Keyboard();
            if (form.ShowDialog() == DialogResult.OK)
            {
                TB_ID.Text = form.TB_Input.Text;
            }
        }

        private void TB_PSWD_Click(object sender, EventArgs e)
        {
            Fo_Keyboard form = new Fo_Keyboard();
            form.TB_Input.PasswordChar = '*';
            if (form.ShowDialog() == DialogResult.OK)
            {
                TB_PSWD.Text = form.TB_Input.Text;
            }
        }

        private void TB_ID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Btn_Login.PerformClick();
            }
        }

        private void TB_PSWD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Btn_Login.PerformClick();
            }
        }
    }
}
