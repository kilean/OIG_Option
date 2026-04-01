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
    public partial class Fo_Logo : Form
    {
        public bool bSafeMode = false;
        bool bClose = false;
        bool bDown = false;
        int iStart;

        public Fo_Logo()
        {
            InitializeComponent();
        }



        private void Fo_Logo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!bClose)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        public void CloseForm()
        {
            bClose = true;
            this.Close();
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            iStart = Environment.TickCount;
            bDown = true;
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            bDown = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bDown)
            {
                int iTime = Environment.TickCount - iStart;
                if (iTime > 3000)
                {
                    bSafeMode = true;
                    this.Hide();
                    Cursor.Show();

                }
            }
        }

        private void Fo_Logo_Load(object sender, EventArgs e)
        {
            pa_NoSignal.Left = (1024 - pa_NoSignal.Width) / 2;
            pa_NoSignal.Top = (768 - pa_NoSignal.Height) / 2;
        }

        private void btn_NoSignal_Click(object sender, EventArgs e)
        {
            bSafeMode = true;
            this.Hide();
            Cursor.Show();
        }
    }
}
