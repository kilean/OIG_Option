using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCD_Interface
{
    public partial class Fo_Measure :Form
    {
        
        public Fo_Measure()
        {
            InitializeComponent();
            tabControl1.Left = 8;
            tabControl1.Top = 8;


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            short ret;
            int dist = Convert.ToInt32(TB_Dist.Text);
            if (dist < 1)
            {
                dist = 1;
                TB_Dist.Text = "1";
            }
            if (dist > 100) 
            {
                dist = 100;
                TB_Dist.Text = "100";
            }
            ret = Units.Focas2.WriteMacro(589, dist);
            if (ret != Focas1.EW_OK)
            {
                MessageBox.Show(Units.GetResString("寫入端測距離失敗。"));
                return;
            }  

            ret= Units.Focas2.WriteMacro(588, -1);
            if(ret != Focas1.EW_OK)
            {
                MessageBox.Show(Units.GetResString("寫入端測方向失敗。"));
                return;
            }            
            tabControl1.SelectedIndex = 1;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            short ret;
            int dist = Convert.ToInt32(TB_Dist.Text);
            if (dist < 1)
            {
                dist = 1;
                TB_Dist.Text = "1";
            }
            if (dist > 100)
            {
                dist = 100;
                TB_Dist.Text = "100";
            }
            ret = Units.Focas2.WriteMacro(589, dist);
            if (ret != Focas1.EW_OK)
            {
                MessageBox.Show(Units.GetResString("寫入端測距離失敗。"));
                return;
            }  
            ret = Units.Focas2.WriteMacro(588, 1);
            if (ret != Focas1.EW_OK)
            {
                MessageBox.Show(Units.GetResString("寫入端測方向失敗。"));
                return;
            }
            tabControl1.SelectedIndex = 1;
        }

        private void btn_Measure_Click(object sender, EventArgs e)
        {
            btn_Measure.Enabled = false;
            Units.ThrMain.AddMeasuringProc();

            while (Units.ThrMain.bProcessMethods)
            {
                int alarm = Units.Focas2.GetAlarm();
                if (alarm == 1) break;
                Application.DoEvents();
            }

            btn_Measure.Enabled = true;
            this.Dispose();
        }
    }
}
