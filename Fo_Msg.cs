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
    public partial class Fo_Msg : Form
    {


        public Fo_Msg()
        {
            InitializeComponent();
            this.LoadLanguageFile(Units.langfile, this.Name);
            this.DialogResult = DialogResult.Cancel;

            pa_RetryCancel.Height = 104;
            pa_YesNo.Height = 104;
            pa_YesNoCancel.Height = 104;
            pa_AbortRetryIgnore.Height = 104;
            pa_OKCancel.Height = 104;
            pa_OK.Height = 104;
            
            
        }

        public static DialogResult Show(String text)
        {
            return Show(text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        public static DialogResult Show(String text, String caption)
        {
            return Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);            
        }

        public static DialogResult Show(String text, String caption, MessageBoxButtons buttons)
        {
            return Show(text, caption, buttons, MessageBoxIcon.None);
        }

        public static DialogResult Show(String text, String caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Fo_Msg form = new Fo_Msg();
            form.DialogResult = DialogResult.Cancel;
            form.la_Message.Text = text;
            form.Text = caption;
            form.Refresh();

            int btnw = 100;

            if (buttons == MessageBoxButtons.OKCancel)
            {
                form.pa_OKCancel.Visible = true;
                btnw = 200;
            }
            else if (buttons == MessageBoxButtons.AbortRetryIgnore)
            {
                form.pa_AbortRetryIgnore.Visible = true;
                btnw = 300;
            }
            else if (buttons == MessageBoxButtons.RetryCancel)
            {
                form.pa_RetryCancel.Visible = true;
                btnw = 200;
            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                form.pa_YesNo.Visible = true;
                btnw = 200;
            }
            else if (buttons == MessageBoxButtons.YesNoCancel)
            {
                form.pa_YesNoCancel.Visible = true;
                btnw = 300;
            }
            else
            {
                form.pa_OK.Visible = true;
                btnw = 100;
            }

            if (icon == MessageBoxIcon.Asterisk || icon == MessageBoxIcon.Information)
            {
                form.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\icon\\Information.ico");
            }
            else if (icon == MessageBoxIcon.Error || icon == MessageBoxIcon.Hand || icon == MessageBoxIcon.Stop)
            {
                form.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\icon\\Error.ico");
            }
            else if (icon == MessageBoxIcon.Exclamation || icon == MessageBoxIcon.Warning)
            {
                form.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\icon\\Warning.ico");
            }
            else if (icon == MessageBoxIcon.Question)
            {
                form.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\icon\\Question.ico");
            }
            else
            {
                form.pictureBox1.Visible = false;
                form.la_Message.Left = form.pictureBox1.Left;
            }
            
            int imgw = 0;
            if (form.pictureBox1.Visible) imgw = 32;
            form.Width = 120 + form.la_Message.Width + imgw + btnw;
            form.Height = 226;

            return form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_Abort_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void Btn_Retry_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void btn_Ignore_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;
            this.Close();
        }

        private void Btn_Yes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btn_No_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
