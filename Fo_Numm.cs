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

namespace OCD
{


    public partial class Fo_Num : Form
    {

        private bool bFirst;
        private bool bDot;
        public bool bMemory;
        private int Mode;
        public String InputData;
        public double TmpVal;


        void Cal()
        {
            if (InputData != "")
            {
                double val;
                double.TryParse(InputData, out val);
                if (Mode == 1)
                {
                    val = TmpVal + val;
                }
                else if (Mode == 2)
                {
                    val = TmpVal - val;
                }
                else if (Mode == 3)
                {
                    val = TmpVal * val;
                }
                else if (Mode == 4)
                {
                    if (val != 0)
                    {
                        val = TmpVal / val;
                    }
                    else
                    {
                        //Fo_Msg.Show(LanguageManager.LoadMessage(Units.langfile, "Message", 10 ,"除以零錯誤"));
                        return;
                    }
                }

                TmpVal = val;
            }
            Mode = 0;
            bDot = false;
            InputData = "";
        }
        public void SetVal(double val)
        {
            Clear();
            la_Num.Text = val.ToString();
            TmpVal = val;
        }
        public void Clear()
        {
            bFirst = true;
            Mode = 0;
            TmpVal = 0;
            bDot = false;
            la_Num.Text = "0";
            la_CalNum.Text = "";
            InputData = "";
        }

        //private THook hook;
        public Fo_Num()
        {
            InitializeComponent();
            this.LoadLanguageFile(Units.langfile, this.Name);
            //hook = new THook(KeyboardHookProc);
        }

        /*
        public int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode != 0) return hook.CallNext(nCode, wParam, lParam);
            if (wParam.ToInt32() == hook.WM_KEYDOWN)
            {
                unsafe
                {
                    KBDLLHOOKSTRUCT* data = (KBDLLHOOKSTRUCT*)lParam;
                    la_CalNum.Text = data->vkCode.ToString();

                    switch (data->vkCode)
                    {
                        case 0x30:
                        case 0x31:
                        case 0x32:
                        case 0x33:
                        case 0x34:
                        case 0x35:
                        case 0x36:
                        case 0x37:
                        case 0x38:
                        case 0x39:
                            PutNum((data->vkCode - 0x30).ToString());
                            break;

                        case 0x60:
                        case 0x61:
                        case 0x62:
                        case 0x63:
                        case 0x64:
                        case 0x65:
                        case 0x66:
                        case 0x67:
                        case 0x68:
                        case 0x69:
                            PutNum((data->vkCode - 0x60).ToString());
                            break;


                        case 0x6E:
                        case 0xBE:
                            btn_Dot_Click(null, null);
                            break;

                        case 0x6B:
                            btn_Add_Click(null, null);
                            break;

                        case 0x6D:
                            btn_Sub_Click(null, null);
                            break;

                        case 0x6A:
                            btn_Mul_Click(null, null);
                            break;

                        case 0x6F:
                            btn_Div_Click(null, null);
                            break;

                    }

                }
            }
            return hook.CallNext(nCode, wParam, lParam);
        }
        */
        private void PutNum(String Data)
        {
            if (!bFirst)
            {
                InputData += Data;
            }
            else
            {
                InputData = Data;
                bFirst = false;
            }

            String N = "";
            if (Mode == 1) N = " + ";
            else if (Mode == 2) N = " - ";
            else if (Mode == 3) N = " * ";
            else if (Mode == 4) N = " / ";


            if (Mode != 0)
            {
                String Val = TmpVal.ToString();
                if (Val.Length > 9) Val = TmpVal.ToString("0.0000");
                la_CalNum.Text = Val + N + InputData;
            }
            else
            {
                la_CalNum.Text = InputData;
            }
            la_Num.Text = InputData;
        }

        private void Fo_Num_Load(object sender, EventArgs e)
        {
            bFirst = true;
            bDot = false;
            
            Mode = 0;
            InputData = "";
            //TmpVal = 0;

            Bitmap bmp = btn_Backspace.Image as Bitmap;
            bmp.MakeTransparent(Color.White);
            //int x = (btn_Clear.Width - bmp.Width) / 2;
            //int y = (btn_Clear.Height - bmp.Height) / 2;
            btn_Backspace.Image = bmp;
            //g.DrawImage(bmp, x, y);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void NumClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            String Data = btn.Text;
            PutNum(Data);
            btn_Enter.Focus();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            Clear();
            btn_Enter.Focus();
        }

        private void btn_Backspace_Click(object sender, EventArgs e)
        {
            String N = "";
            if (Mode == 1) N = " + ";
            else if (Mode == 2) N = " - ";
            else if (Mode == 3) N = " * ";
            else if (Mode == 4) N = " / ";

            if (InputData.Length == 0) return;
            InputData = InputData.Substring(0, InputData.Length - 1);
            if (InputData == "")
            {
                String Val = TmpVal.ToString();
                if (Val.Length > 9) Val = TmpVal.ToString("0.0000");
                la_Num.Text = Val;
                la_CalNum.Text = Val + N;
            }
            else
            {


                double val;
                double.TryParse(InputData, out val);
                int v = (int)val;
                if (val == v) bDot = false;

                String Val = TmpVal.ToString();
                if (Val.Length > 9) Val = TmpVal.ToString("0.0000");
                la_CalNum.Text = Val + N + InputData;
                la_Num.Text = InputData;
            }
            btn_Enter.Focus();
        }



        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Dot_Click(object sender, EventArgs e)
        {
            if (!bDot)
            {
                if (InputData == "") InputData = "0";
                InputData = InputData + ".";
                la_CalNum.Text = InputData;
                la_Num.Text = InputData;
                bDot = true;
                bFirst = false;
            }
            btn_Enter.Focus();
        }
        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (Mode != 0)
            {
                btn_Enter.PerformClick();
                //Fo_Msg.Show(LanguageManager.LoadMessage(Units.langfile, "Message", 11, "檢測出未完成的運算式"));
                //return;
            }
            //else
            //{
                double.TryParse(la_Num.Text , out TmpVal);
                DialogResult = DialogResult.OK;
                this.Close();
            //}
        }
        private void btn_Enter_Click(object sender, EventArgs e)
        {
            Cal();
            String Val = TmpVal.ToString();
            if (Val.Length > 9) Val = TmpVal.ToString("0.0000");
            la_Num.Text = Val;
            la_CalNum.Text = "";
            btn_OK.Focus();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            Cal();
            Mode = 1;
            la_CalNum.Text = TmpVal.ToString() + " + ";
            String Val = TmpVal.ToString();
            if (Val.Length > 9) Val = TmpVal.ToString("0.0000");
            la_Num.Text = Val;
            btn_Enter.Focus();
        }

        private void btn_Sub_Click(object sender, EventArgs e)
        {
            Cal();
            Mode = 2;
            la_CalNum.Text = TmpVal.ToString() + " - ";
            String Val = TmpVal.ToString();
            if (Val.Length > 9) Val = TmpVal.ToString("0.0000");
            la_Num.Text = Val;
            btn_Enter.Focus();
        }

        private void btn_Mul_Click(object sender, EventArgs e)
        {
            Cal();
            Mode = 3;
            la_CalNum.Text = TmpVal.ToString() + " * ";
            String Val = TmpVal.ToString();
            if (Val.Length > 9) Val = TmpVal.ToString("0.0000");
            la_Num.Text = Val;
            btn_Enter.Focus();
        }

        private void btn_Div_Click(object sender, EventArgs e)
        {
            Cal();
            Mode = 4;
            la_CalNum.Text = TmpVal.ToString() + " / ";
            String Val = TmpVal.ToString();
            if (Val.Length > 9) Val = TmpVal.ToString("0.0000");
            la_Num.Text = Val;
            btn_Enter.Focus();
        }

        private void Fo_Num_Shown(object sender, EventArgs e)
        {
            //Clear();
            
            btn_Enter.Focus();
        }

        private void Fo_Num_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btn_UseCurrentPos_Click(object sender, EventArgs e)
        {
            SetVal(Units.Fo_Main.GetMemoryValue());
        }

        private void Fo_Num_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool bActived = false;
        private void Fo_Num_Activated(object sender, EventArgs e)
        {
            bActived = true;
        }
    }
}
