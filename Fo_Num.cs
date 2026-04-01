using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace OIG_Option
{
    public partial class Fo_Num : Form
    {

        public bool ShowCancelBtn
        {
            get
            {
                return this.uc_UserNum1.ShowCancelBtn;
            }
            set
            {
                this.uc_UserNum1.ShowCancelBtn = value;
            }
        }

        public bool ShowMemoryBtn
        {
            get
            {
                return this.uc_UserNum1.ShowMemoryBtn;
            }
            set
            {
                this.uc_UserNum1.ShowMemoryBtn = value;
            }
        }
        public Fo_Num()
        {
            InitializeComponent();
            this.Width = 440;
            this.Height = 280;
            this.BackColor = Color.Black;
        }

        public void SetVal(double val)
        {
            uc_UserNum1.SetVal(val);
        }
        private void Fo_Num_Load(object sender, EventArgs e)
        {
            
            this.LoadLanguageFile(Units.langfile, this.Name);
        }

        private void uc_UserNum1_OnBtnCancelClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uc_UserNum1_OnBtnOkClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
