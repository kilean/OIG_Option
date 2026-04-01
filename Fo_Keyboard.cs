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
    public partial class Fo_Keyboard : Form
    {
        public Fo_Keyboard()
        {
            InitializeComponent();

            this.LoadLanguageFile(Units.langfile, this.Name);
        }

        private void btn_OK_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(Object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            TB_Input.SelectedText = btn.Text;

            TB_Input.Focus();
        }

        private void button56_Click(object sender, EventArgs e)
        {
            if (TB_Input.SelectionStart <= 0)
            {
                if (TB_Input.SelectionLength > 0)
                    TB_Input.SelectedText = "";
                TB_Input.Focus();
                return;
            }
            if (TB_Input.Text[TB_Input.SelectionStart - 1] == '\n')
            {
                TB_Input.SelectionStart = TB_Input.SelectionStart - 2;
                TB_Input.SelectionLength = 2;
            }
            else
            {
                if (TB_Input.SelectionLength < 1)
                {
                    TB_Input.SelectionStart = TB_Input.SelectionStart - 1;
                    TB_Input.SelectionLength = 1;
                }
            }
            TB_Input.SelectedText = "";

            TB_Input.Focus();
        }

        private void TB_Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_OK.PerformClick();
            }
        }
    }
}
