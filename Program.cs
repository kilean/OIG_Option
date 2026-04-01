using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Palmary.Register;

namespace OIG_Option
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            
            URegister r = new URegister();
            if (!r.CheckRegisterKeygen())
            {
                Fo_Register form = new Fo_Register();
                Byte[] MacData = new Byte[6];
                r.ScanAddressList();
                if(r.AddressList.Count>0) r.StrToByte(r.AddressList[0], MacData);
                String Mac = "";
                for (int j = 0; j < 6; j++)
                {
                    Mac += MacData[j].ToString("X2");
                }
                String SN = r.XOR(Mac, r.Key1);
                form.la_SN.Text = SN;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    String Keygen = form.tb_Keygen.Text;

                    String XMac = r.XOR(SN, r.Key1);
                    String XKeygen = r.XOR(Keygen, r.Key2);
                    bool Check = r.Check(XKeygen, XMac);
                    if (!Check)
                    {
                        Fo_Msg.Show("錯誤");
                        return;
                    }
                    r.RegisterKeygen(XKeygen);
                }
                else
                {
                    return;
                }

            }
            



            Application.Run(new Fo_Main());

        }
    }
}
