using Palmary.Register;
using pmcMessagadll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
//using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Markup;
using UserNumLib;


#pragma warning disable IDE1006

namespace OIG_Option
{
    public partial class Fo_Main : Form
    {
        public Queue<Action> Actions = new Queue<Action>();

        public Panel pa_Main;

        Panel pa = new Panel();
        Label la = new Label();

        public Fo_Logo Logo = new Fo_Logo();

        public Focas1 focas = new Focas1();
        public Focas1 remote = new Focas1();
        public int SUCCESS = Focas1.EW_OK;

        private string IPAddress = "192.168.168.2";
        private int Port = 8193;

        //private string RemoteIP = "192.168.168.12";
        //private int RemotePort = 8193;

        //選配會影響軸順序
        public static Dictionary<string, int> dicAxisIndex = new Dictionary<string, int>();//用軸名稱(XZYABC)取得索引值
        public static Dictionary<int, string> dicAxisName = new Dictionary<int, string>();//用軸號(1~6)取得軸名稱

        private NativeTabControl NativeTabControl1 = new NativeTabControl();//為了讓TabControl 沒有邊框

        public int UserLv = 1;//使用者等級
        //各階級預設帳密
        public string id99 = "palmary", pswd99 = "16524622";

        //暫存上一頁
        private Stack<TabPage> PrevPage = new Stack<TabPage>();

        public int iQueryAlarmTick;
        bool bCycleStart = false;//F0.5
        public bool bRun = false;//F0.7

        private Thread ThrMain;
        private bool bClose = false;
        private bool bCloseFinish = false;
        public List<PmcMessageData> PmcMsgList = new List<PmcMessageData>();
        private AlarmFile CurrentAlarm = new AlarmFile();
        private string[] CurrentAlarmBuf = null;
        private AlarmFile TroubleShootingFile1;
        private AlarmFile PmcAlarm;

        Image imgCheckMark = Properties.Resources.checkmark;
        Image imgOn = Properties.Resources.Lamp_E_On;
        Image imgOff = Properties.Resources.Lamp_E_Off;
        string progFrontDoor = "(Front Door Test)" + "\r\n" + "M24;" + "\r\n" + "G4X1.;" + "\r\n" + "M25;" + "\r\n" + "M30;";
        string progCourtyardDoor = "(Courtyard Door Test)" + "\r\n" + "M51; " + "\r\n" + "G4X1.;" + "\r\n" + "M52;" + "\r\n" + "M30;";

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 281) return;

            base.WndProc(ref m);
        }

        public Fo_Main()
        {
            InitializeComponent();

            pa_Main = new Panel();
            pa_Main.Parent = this;
            pa_Main.Dock = DockStyle.Fill;
            pa_Main.BringToFront();

            TC_Main.Parent = pa_Main;
            TC_Main.Dock = DockStyle.Fill;

            iQueryAlarmTick = Environment.TickCount;

            pa_AlarmTip.Width = 1024;
            pa_AlarmTip.Left = 0;
            pa_AlarmTip.Parent = this;
            pa_AlarmTip.BringToFront();

            pa.Width = 300;
            pa.Height = 34;
            pa.BackColor = Color.Yellow;
            pa.Left = (this.Width - 300) / 2;
            pa.Top = 0;
            pa.Visible = false;
            pa.Parent = this;
            pa.BringToFront();

            la.Parent = pa;
            la.AutoSize = false;
            la.Left = 10;
            la.Width = 280;
            la.Top = 0;
            la.Height = 20;
            la.Font = new Font("Times New Roman", 12);
            la.ForeColor = Color.Black;
            la.TextAlign = ContentAlignment.MiddleCenter;


            Logo.WindowState = FormWindowState.Maximized;
            Logo.TopLevel = false;
            Logo.Parent = this;

            ucBtn_FunOverView.PerformClick();

            Units.Fo_Main = this;


            //讀取設定值
            TIniFile ini = new TIniFile(Application.StartupPath + "\\sys.ini");

            int argb = ini.ReadInteger("System", "Color", Color.Black.ToArgb());
            Color sysColor = Color.FromArgb(argb);
            pa_Top.BackColor = sysColor;
            pa_Bottom.BackColor = sysColor;

            IPAddress = ini.ReadString("CNC", "IP", "192.168.168.2");
            Port = ini.ReadInteger("CNC", "Port", 8193);

            #region 多國語言
            String lang = ini.ReadString("System", "Language", "CHT");
            Units.langfile = Application.StartupPath + "\\Language\\" + lang + "\\" + lang + ".txt";
            if (!File.Exists(Units.langfile)) Fo_Msg.Show("Language file not find.");

            Units.LangCode = lang;

            SetLanguage(lang, true);

            this.LoadLanguageFile(Units.langfile, this.Name);
            #endregion 多國語言

            // 主畫面 UI 元件初始化
            TC_Main.SizeMode = TabSizeMode.Fixed;
            TC_Main.Appearance = TabAppearance.FlatButtons;
            TC_Main.ItemSize = new Size(0, 1);
            this.NativeTabControl1.AssignHandle(TC_Main.Handle);

            
            btn_Regist.Visible = false;
            btn_Monitor.Visible = false;
            btn_Program.Visible = false;
            btn_Message.Visible = true;
            btn_Maintenance.Visible = false;
            btn_SoftPanel.Visible = false;

            //讀取訊息及異常的對照表
            PmcMsgList = PmcMessage.LoadFromFile(Application.StartupPath + "\\PmcMessage.txt");
            string last_filename = Application.StartupPath + "\\PmcMessage.txt";
            if (!File.Exists(last_filename))
            {
                // 檔案不存在，創建檔案並寫入初始內容
                File.WriteAllText(last_filename, "0,讀取PMC Message\r\n1,讀取PMC Alarm\r\n"); // 你可以改成你需要的初始內容
            }
        }

        private void Fo_Main_Load(object sender, EventArgs e)
        {

            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                bCloseFinish = true;
                Fo_Msg.Show("Software is already running.");
                this.Close();
                return;
            }

            ThrMain = new Thread(Execute);
            ThrMain.Start();

        }

        private void TC_Main_SelectedIndexChanged(object sender, EventArgs e) //畫面切換
        {
            ucBtn_FunOverView.Lamp = TC_Main.SelectedTab == tab_FuntionOverView;
            btn_Message.Lamp = TC_Main.SelectedTab == tab_Message;

            ucBtn_KeyBoard.Visible = ucBtn_FunProgramTest.Visible = TC_Main.SelectedTab != tab_FuntionOverView &&
                TC_Main.SelectedTab != tab_Message;

        }

        private void Fo_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            bClose = true;
            while (!bCloseFinish) Application.DoEvents();
        }
        // tabPage Panel 背景色
        private void tab_Paint(object sender, PaintEventArgs e)
        {
            TabPage p = (TabPage)sender;
            if (p == null)
                return;

            e.Graphics.DrawImage(Properties.Resources.background2, new Rectangle(0, 0, p.Width, p.Height));
        }

        private void SelectMode_Click(object sender, EventArgs e)
        {
            if (bCycleStart)
            {
                Fo_Msg.Show(LanguageManager.LoadMessage(Units.langfile, "Message", 46, "自動啟動中，無法切換模式"));
                return;
            }

            Uc_RoundBtn btn = sender is Uc_RoundBtn ? (Uc_RoundBtn)sender : null;
            if (btn == null) return;

            byte.TryParse(btn.Tag.ToString(), out byte tag);
            Actions.Enqueue(new Action(() =>
            {
                focas.PMC_WriteByte(PmcAddrType.E, 2521, (byte)tag);
            }));
        }

        #region Facas 連線輸入 IP Port 
        private void pic_ConnectStatus_Click(object sender, EventArgs e)
        {
            Fo_CncConnect form = new Fo_CncConnect();

            TIniFile ini = new TIniFile(Application.StartupPath + "\\sys.ini");
            form.TB_IP.Text = ini.ReadString("CNC", "IP", "192.168.168.2");
            form.TB_Port.Text = ini.ReadString("CNC", "Port", "8193");
            int Mode = ini.ReadInteger("CNC", "Mode", 0);
            if (Mode == 0) form.rb_Ethernet.Checked = true;
            else form.rb_HSSB.Checked = true;

            if (form.ShowDialog() == DialogResult.OK)
            {
                ini.WriteString("CNC", "IP", form.TB_IP.Text);
                ini.WriteString("CNC", "Port", form.TB_Port.Text);
                ini.WriteInteger("Remote", "Mode", form.rb_Ethernet.Checked ? 0 : 1);
                Actions.Enqueue(new Action(() =>
                {
                    focas.Disconnect();
                }));

                IPAddress = form.TB_IP.Text;
                int.TryParse(form.TB_Port.Text, out Port);
                if (form.rb_Ethernet.Checked)
                {
                    Actions.Enqueue(new Action(() =>
                    {
                        focas.ConnectByEthernet(IPAddress, Port);
                    }));
                }
                else
                {
                    Actions.Enqueue(new Action(() =>
                    {
                        focas.ConnectByHSSB();
                    }));
                }
            }
        }
        #endregion

        #region 登入帳號密碼與登出
        private void pic_Logo_DoubleClick(object sender, EventArgs e)
        {
            pa_Develop.Visible = false;
            Fo_Permission form = new Fo_Permission();
            if (form.ShowDialog() != DialogResult.OK) return;

            if (form.TB_ID.Text.ToLower() == "palmary" && form.TB_PSWD.Text.ToLower() == "16524622")
            {
                pa_Develop.Visible = true;
            }
        }

        public bool CheckPswd(string user, string pswdd)
        {
            bool flag = false;

            pswdd = pswdd.ToLower();
            user = user.ToLower();

            if (user == id99 && pswdd == pswd99)
            {
                UserLv = 99;
                pic_User.Image = Properties.Resources.user99;
                pa_Develop.Visible = true;
                flag = true;
            }
            else
            {
                pic_User.Image = Properties.Resources.user1s;
                UserLv = 1;

            }

            return flag;
        }

        private void pic_User_Click(object sender, EventArgs e)
        {
            if (UserLv == 1)
            {
                Fo_Permission form = new Fo_Permission();
                if (form.ShowDialog() != DialogResult.OK) { return; }

            }
            else
            {

                if (Fo_Msg.Show(LanguageManager.LoadMessage(Units.langfile, "Message", 223, "確定要登出嗎？"),
                               LanguageManager.LoadMessage(Units.langfile, "Message", 224, "確認訊息"),
                               MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                UserLv = 1;
                pic_User.Image = Properties.Resources.user1s;
                pa_Develop.Visible = false;
            }
        }
        #endregion


        bool bFistOpen = true;

        private void OnConnected()
        {
            if (!bFistOpen) return;
            bFistOpen = false;

            Dictionary<int, char> dicName = new Dictionary<int, char>(); //軸號
            Dictionary<int, char> dicSubName = new Dictionary<int, char>();
            int index = 0; //計算座標系該讀哪個索引值
            focas.Param_ReadByte(987, 0, out byte AxisCount);//軸數
            for (int i = 0; i < AxisCount; i++)
            {
                focas.Param_ReadByte(1020, (short)(i + 1), out byte AxisName);//軸名稱
                focas.Param_ReadByte(1025, (short)(i + 1), out byte SubName);//軸子名稱
                char chrName = (char)AxisName;
                char chrSubName = (char)SubName;
                dicName.Add(i + 1, (char)chrName);
                dicSubName.Add(i + 1, (char)chrSubName);

                string full_name = chrName.ToString();
                if (chrSubName != '\0') full_name += chrSubName;

                dicAxisName.Add(i + 1,  full_name);

                //記錄個軸的索引值 0~5
                if (chrName == 'X' && !dicAxisIndex.ContainsKey("X")) dicAxisIndex["X"] = index;
                else if (chrName == 'Z' && !dicAxisIndex.ContainsKey("Z")) dicAxisIndex["Z"] = index;
                else if (chrName == 'Y' && !dicAxisIndex.ContainsKey("Y")) dicAxisIndex["Y"] = index;
                else if (chrName == 'A' && !dicAxisIndex.ContainsKey("A")) dicAxisIndex["A"] = index;
                else if (chrName == 'B' && !dicAxisIndex.ContainsKey("B")) dicAxisIndex["B"] = index;
                else if (chrName == 'C' && !dicAxisIndex.ContainsKey("C")) dicAxisIndex["C"] = index;
                index++;
            }
        }

        bool bCNCConnect = false;


        private void Execute()
        {
            int ret;

            //Thread.Sleep(1000);
            //int iLogoStart = Environment.TickCount;
            int iConnectStart = Environment.TickCount;//連線失敗已經過的時間
            //this.Invoke((Action)(() =>
            //{
                //顯畫面
                //bShowLogoForm = true;
                //Logo.Show();
                //this.Opacity = 1;
            //}));

            focas.ConnectByEthernet(IPAddress, Port);
            iConnectStart = Environment.TickCount;//連線失敗已經過的時間

            if (focas.IsConnected())//試連成功
            {
                bCNCConnect = true; //這裡只執行一次, 不用判斷正緣觸發
                this.Invoke((Action)(() =>
                {                    
                    pic_ConnectStatus.Image = Properties.Resources.connects;
                }));
                OnConnected();                
            }
            else
            {
                bCNCConnect = false; //這裡只執行一次, 不用判斷負緣觸發
                this.Invoke((Action)(() =>
                {
                    pic_ConnectStatus.Image = Properties.Resources.disconnects;
                }));

            }

            int PmcRefleshStart = Environment.TickCount;//每200ms
            int Request_CNC_Time_Start = Environment.TickCount;//每秒

            //int iQueryMonitorOneSec = Environment.TickCount;
            //int iQuerySoftPanelStart = Environment.TickCount;




            //string[] all_history = null;
            //int iAlarmTimeInterval = 50;
            while (true)
            {
                if (bClose) break;

                //降CPU用
                Thread.Sleep(50);


                if (focas.IsConnected())
                {
                    if (!bCNCConnect)//正緣觸發 連線
                    {
                        bCNCConnect = true;
                        this.Invoke((Action)(() =>
                        {
                            //顯示連線狀態
                            pic_ConnectStatus.Image = Properties.Resources.connects;
                        }));
                    }
                }
                else //已斷線
                {
                    if (bCNCConnect)//負緣觸發 斷線
                    {
                        bCNCConnect = false;
                        this.Invoke((Action)(() =>
                        {
                            //顯示斷線狀態
                            pic_ConnectStatus.Image = Properties.Resources.disconnects;
                        }));
                    }
                }

                //未連線
                if (!focas.IsConnected())
                {
                    //距離上次試連經過多少時間
                    int iConnectTime = Environment.TickCount - iConnectStart;
                    if (iConnectTime > 5000)
                    {                        
                        //自動連線
                        focas.ConnectByEthernet(IPAddress, Port);//先試連
                        iConnectStart = Environment.TickCount;//連線失敗已經過的時間
                    }

                    if (focas.IsConnected())
                    {
                        //每次連線成功檢查
                        OnConnected();
                    }
                    continue;
                }

                

                bool bIsAlarm = false;
                bool bMessage = false;
                bool bProbe = false;
                bool bChuck = false;
                bool bWorkSpindle = false;
                bool bOrientation = false;
                bool bGW1 = false;
                bool bGW2 = false;
                bool bGauge = false;
                bool bSafetyDoor = false;
                bool bAutoDoor = false;
                bool bAcoustic = false;
                bool bOilMistCollect = false;
                bool bAxisLube = false;
                bool bOilCooler = false;              
                bool bCoolantCooler = false;
                bool bCoolantSystem = false;
                bool bCoolantGun = false;
                bool bLightCurtain = false;
                bool bWheelCover = false;
                bool bMaintainDoor = false;
                bool bHydraulicOpt = false;
                bool bGWOilMistLube = false;
                bool bOilDressDevice = false;
                bool bRoller = false;
                bool bAirTight = false;
                bool bAirPressure = false;
                
                this.Invoke(new Action(() =>
                {
                    bIsAlarm = pa_Alarm.Visible;
                    bMessage = TC_Main.SelectedTab == tab_Message;
                    bProbe = TC_Main.SelectedTab == tab_Probe;
                    bChuck = TC_Main.SelectedTab == tab_Chuck;
                    bWorkSpindle = TC_Main.SelectedTab == tab_WorkSpindle;
                    bOrientation = TC_Main.SelectedTab == tab_Orientation;
                    bGW1 = TC_Main.SelectedTab == tab_GW1;
                    bGW2 = TC_Main.SelectedTab == tab_GW2;
                    bGauge = TC_Main.SelectedTab == tab_Gauge;
                    bSafetyDoor = TC_Main.SelectedTab == tab_SafetyDoor;
                    bAutoDoor = TC_Main.SelectedTab == tab_AutoDoor;
                    bAcoustic = TC_Main.SelectedTab == tab_Acoustic;
                    bOilMistCollect = TC_Main.SelectedTab == tab_OilMistCollect;
                    bAxisLube = TC_Main.SelectedTab == tab_AxisLube;
                    bOilCooler = TC_Main.SelectedTab == tab_OilCooler;                          
                    bCoolantCooler = TC_Main.SelectedTab == tab_CoolantCooler;
                    bCoolantSystem = TC_Main.SelectedTab == tab_CoolantSystem;
                    bCoolantGun = TC_Main.SelectedTab == tab_CoolantGun;
                    bLightCurtain = TC_Main.SelectedTab == tab_LightCurtain;
                    bWheelCover = TC_Main.SelectedTab == tab_WheelCover;
                    bMaintainDoor = TC_Main.SelectedTab == tab_MaintainDoor;
                    bHydraulicOpt = TC_Main.SelectedTab == tab_HydraulicOpt;
                    bGWOilMistLube = TC_Main.SelectedTab == tab_GWOilMistLube;
                    bOilDressDevice = TC_Main.SelectedTab == tab_OilDressDevice;
                    bRoller = TC_Main.SelectedTab == tab_Roller;
                    bAirTight = TC_Main.SelectedTab == tab_AirTight;
                    bAirPressure = TC_Main.SelectedTab == tab_AirPressure;
                    
                }));


                //if (ret != SUCCESS)
                //{
                //focas.Disconnect();
                //}

                int iTime = Environment.TickCount - PmcRefleshStart;
                if ((iTime > 200))
                {
                    PmcRefleshStart = Environment.TickCount;

                    byte F0;
                    ret = focas.PMC_ReadByte(PmcAddrType.F, 0, out F0);
                    if (ret != SUCCESS) continue;
                    bCycleStart = F0.BIT_7();

                    if (bProbe)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 1, out byte X1);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 2, out byte Y2);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            pic_X1_3.Image = X1.BIT_3() ? imgOn : imgOff;
                            pic_X1_4.Image = X1.BIT_4() ? imgOn : imgOff;
                            pic_Y2_1.Image = Y2.BIT_1() ? imgOn : imgOff;
                            pic_Y2_2.Image = Y2.BIT_2() ? imgOn : imgOff;
                        }));
                    }
                    else if (bChuck)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 0, out byte X0);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 1, out byte X1);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 0, out byte Y0);
                        if (ret != SUCCESS) continue;
                        this.Invoke(new Action(() =>
                        {
                            pic_X0_1.Image = X0.BIT_1() ? imgOn : imgOff;
                            pic_X0_2.Image = X0.BIT_2() ? imgOn : imgOff;
                            pic_X0_0.Image = X0.BIT_0() ? imgOn : imgOff;
                            pic_X1_0.Image = X1.BIT_0() ? imgOn : imgOff;
                            pic_Y0_3.Image = Y0.BIT_3() ? imgOn : imgOff;
                            pic_Y0_4.Image = Y0.BIT_4() ? imgOn : imgOff;
                            
                        }));
                    }
                    else if (bWorkSpindle)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 4, out byte X4);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 5, out byte X5);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 4, out byte Y4);
                        if (ret != SUCCESS) continue;
                        
                        this.Invoke(new Action(() =>
                        {
                            pic_X4_0.Image = X4.BIT_0() ? imgOn : imgOff;
                            pic_X4_1.Image = X4.BIT_1() ? imgOn : imgOff;
                            pic_X5_6.Image = X5.BIT_6() ? imgOn : imgOff;
                            pic_Y4_6.Image = Y4.BIT_6() ? imgOn : imgOff;
                            pic_Y4_7.Image = Y4.BIT_7() ? imgOn : imgOff;
                            pic_Y4_0.Image = Y4.BIT_0() ? imgOn : imgOff;

                        }));
                    }
                    else if (bOrientation)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 1, out byte X1);
                        if (ret != SUCCESS) continue;
                        
                        this.Invoke(new Action(() =>
                        {
                            pic_X1_2.Image = X1.BIT_2() ? imgOn : imgOff;
                            
                        }));
                    }
                    else if (bGW1)
                    {
                        
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 4, out byte X4);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 5, out byte X5);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 5, out byte Y5);
                        if (ret != SUCCESS) continue;
                        this.Invoke(new Action(() =>
                        {
                            pic_X4_2.Image = X4.BIT_2() ? imgOn : imgOff;
                            pic_X4_3.Image = X4.BIT_3() ? imgOn : imgOff;
                            pic_X5_7.Image = X5.BIT_7() ? imgOn : imgOff;
                            
                            pic_Y5_7.Image = Y5.BIT_7() ? imgOn : imgOff;
                        }));
                    }
                    else if (bGW2)
                    {                     
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 5, out byte X5);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 6, out byte X6);
                        if (ret != SUCCESS) continue;                       
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 3, out byte Y3);
                        if (ret != SUCCESS) continue;
                        this.Invoke(new Action(() =>
                        {
                            pic_X6_0.Image = X6.BIT_0() ? imgOn : imgOff;
                            pic_X6_1.Image = X6.BIT_1() ? imgOn : imgOff;
                            pic_X5_5.Image = X5.BIT_5() ? imgOn : imgOff;
                            
                            pic_Y3_5.Image = Y3.BIT_5() ? imgOn : imgOff;
                        }));
                    }
                    else if (bGauge)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 4, out byte X4);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 9, out byte X9);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 2, out byte Y2);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 3, out byte Y3);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 4, out byte Y4);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            pic_X4_7.Image = X4.BIT_7() ? imgOn : imgOff;
                            pic_X4_4.Image = X4.BIT_4() ? imgOn : imgOff;
                            pic_X9_0.Image = X9.BIT_0() ? imgOn : imgOff;
                            pic_X9_1.Image = X9.BIT_1() ? imgOn : imgOff;                           
                            pic_Y2_3.Image = Y2.BIT_3() ? imgOn : imgOff;      
                            pic_Y4_4.Image = Y4.BIT_4() ? imgOn : imgOff;
                            pic_Y2_7.Image = Y2.BIT_7() ? imgOn : imgOff;   
                            pic_Y3_1.Image = Y3.BIT_1() ? imgOn : imgOff;
                        }));
                    }
                    else if (bSafetyDoor)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 3, out byte X3);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 2, out byte Y2);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            pic_X3_7.Image = X3.BIT_7() ? imgOn : imgOff;
                            pic_Y2_4.Image = Y2.BIT_4() ? imgOn : imgOff;
                        }));
                    }
                    else if (bAutoDoor)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 8, out byte X8);
                        if (ret != SUCCESS) continue;
                        
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 0, out byte Y0);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 4, out byte Y4);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                           
                            pic_X8_0.Image = X8.BIT_0() ? imgOn : imgOff;
                            pic_X8_1.Image = X8.BIT_1() ? imgOn : imgOff;
                           
                            pic_Y0_5.Image = Y0.BIT_5() ? imgOn : imgOff;
                            pic_Y0_6.Image = Y0.BIT_6() ? imgOn : imgOff;
                            pic_Y4_1.Image = Y4.BIT_1() ? imgOn : imgOff;
                            pic_Y4_2.Image = Y4.BIT_2() ? imgOn : imgOff;
                        }));
                    }
                    else if (bAcoustic)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.F, 122, out byte F122);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 4, out byte X4);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.E, 7, out byte E7);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.E, 6, out byte E6);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 3, out byte Y3);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 5, out byte Y5);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            pic_F122_1.Image = F122.BIT_1() ? imgOn : imgOff;
                            pic_X4_5.Image = X4.BIT_5() ? imgOn : imgOff;
                            pic_E7_1.Image = E7.BIT_1() ? imgOn : imgOff;
                            pic_E6_1.Image = E6.BIT_1() ? imgOn : imgOff;
                            pic_E7_3.Image = E7.BIT_3() ? imgOn : imgOff;
                            pic_E6_2.Image = E6.BIT_2() ? imgOn : imgOff;
                            pic_Y3_6.Image = Y3.BIT_6() ? imgOn : imgOff;
                            pic_Y3_7.Image = Y3.BIT_7() ? imgOn : imgOff;
                            pic_Y5_4.Image = Y5.BIT_4() ? imgOn : imgOff;
                        }));
                    }
                    else if (bOilMistCollect)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 2, out byte X2);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 1, out byte Y1);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            pic_X2_6.Image = X2.BIT_6() ? imgOn : imgOff;
                            pic_Y1_4.Image = Y1.BIT_4() ? imgOn : imgOff;
                        }));
                    }
                    else if (bAxisLube)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 1, out byte X1);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 0, out byte Y0);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            pic_X1_5.Image = X1.BIT_5() ? imgOn : imgOff;
                            pic_X1_6.Image = X1.BIT_6() ? imgOn : imgOff;
                            pic_Y0_7.Image = Y0.BIT_7() ? imgOn : imgOff;
                        }));
                    }
                    else if (bOilCooler)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 9, out byte X9);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            pic_X9_4.Image = X9.BIT_4() ? imgOn : imgOff;
                           
                        }));
                    }                 
                    else if (bCoolantCooler)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 2, out byte X2);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            pic_X2_1.Image = X2.BIT_1() ? imgOn : imgOff;
                        }));
                    }
                    else if (bCoolantSystem)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 2, out byte X2);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 6, out byte X6);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 8, out byte X8);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 0, out byte Y0);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 1, out byte Y1);
                        if (ret != SUCCESS) continue;
                        this.Invoke(new Action(() =>
                        {
                            pic_X2_0.Image = X2.BIT_0() ? imgOn : imgOff;
                            pic_X2_2.Image = X2.BIT_2() ? imgOn : imgOff;
                            pic_X2_5.Image = X2.BIT_5() ? imgOn : imgOff;        
                            pic_X6_6.Image = X6.BIT_6() ? imgOn : imgOff;
                            pic_X8_3.Image = X8.BIT_3() ? imgOn : imgOff;
                            pic_Y0_0.Image = Y0.BIT_0() ? imgOn : imgOff;
                            pic_Y1_5.Image = Y1.BIT_5() ? imgOn : imgOff;
                        }));
                    }
                    else if (bCoolantGun)
                    {

                        ret = focas.PMC_ReadByte(PmcAddrType.E, 2524, out byte E2524);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 0, out byte Y0);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 5, out byte Y5);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            pic_E2524_0.Image = E2524.BIT_0() ? imgOn : imgOff;
                            pic_Y0_0.Image = Y0.BIT_0() ? imgOn : imgOff;
                            pic_Y5_2.Image = Y5.BIT_2() ? imgOn : imgOff;
                        }));
                    }
                    else if (bLightCurtain)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 3, out byte X3);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            pic_X3_0.Image = X3.BIT_0() ? imgOn : imgOff;
                        }));
                    }
                    else if (bWheelCover)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 0, out byte X0);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 2, out byte X2);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.E, 8, out byte E8);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 1, out byte Y1);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 2, out byte Y2);
                        if (ret != SUCCESS) continue;
                        this.Invoke(new Action(() =>
                        {
                            pic_X2_3.Image = X2.BIT_3() ? imgOn : imgOff;
                            pic_X2_4.Image = X2.BIT_4() ? imgOn : imgOff;
                            pic_X0_6.Image = X0.BIT_6() ? imgOn : imgOff;
                            pic_X0_7.Image = X0.BIT_7() ? imgOn : imgOff;
                            pic_E8_0.Image = E8.BIT_0() ? imgOn : imgOff;
                            pic_E8_1.Image = E8.BIT_1() ? imgOn : imgOff;
                            pic_Y2_0.Image = Y2.BIT_0() ? imgOn : imgOff;
                            pic_Y1_3.Image = Y1.BIT_3() ? imgOn : imgOff;
                        }));
                    }
                    else if (bMaintainDoor)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 40, out byte X40);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                           
                            // X40.1
                            pic_X40_1.Image = X40.BIT_1() ? imgOn : imgOff;
                            // X40.2
                            pic_X40_2.Image = X40.BIT_2() ? imgOn : imgOff;
                            // X40.3
                            pic_X40_3.Image = X40.BIT_3() ? imgOn : imgOff;
                            // X40.4
                            pic_X40_4.Image = X40.BIT_4() ? imgOn : imgOff;
                        }));
                    }
                    else if (bHydraulicOpt)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 2, out byte X2);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 9, out byte X9);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 1, out byte Y1);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            // X2.7
                            pic_X2_7.Image = X2.BIT_7() == true ? imgOn : imgOff;
                            // X9.3
                            pic_X9_3.Image = X9.BIT_3() == true ? imgOn : imgOff;
                            // Y1.7
                            pic_Y1_7.Image = Y1.BIT_7() == true ? imgOn : imgOff;
                        }));
                    }
                    else if (bGWOilMistLube)
                    {                   
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 8, out byte X8);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 3, out byte X3);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 0, out byte Y0);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 1, out byte Y1);
                        if (ret != SUCCESS) continue;
                        this.Invoke(new Action(() =>
                        {                        
                            pic_X8_6.Image = X8.BIT_6() == true ? imgOn : imgOff;
                            pic_X8_7.Image = X8.BIT_7() == true ? imgOn : imgOff;
                            pic_X3_3.Image = X3.BIT_3() == true ? imgOn : imgOff;
                            pic_X3_4.Image = X3.BIT_4() == true ? imgOn : imgOff;
                            pic_Y0_2.Image = Y0.BIT_2() == true ? imgOn : imgOff;
                            pic_Y1_1.Image = Y1.BIT_1() == true ? imgOn : imgOff;
                        }));
                    }
                    else if (bOilDressDevice)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 0, out byte X0);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 1, out byte X1);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 5, out byte Y5);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            // X1.7
                            pic_X1_7.Image = X1.BIT_7() ? imgOn : imgOff;
                            // X0.5
                            pic_X0_5.Image = X0.BIT_5() ? imgOn : imgOff;
                            pic_Y5_0.Image = Y5.BIT_0() ? imgOn : imgOff;
                            pic_Y5_1.Image = Y5.BIT_1() ? imgOn : imgOff;
                        }));
                    }
                    else if (bRoller)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 6, out byte X6);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 5, out byte Y5);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            pic_X6_2.Image = X6.BIT_2() ? imgOn : imgOff;
                            pic_X6_3.Image = X6.BIT_3() ? imgOn : imgOff;
                            pic_Y5_5.Image = Y5.BIT_5() == true ? imgOn : imgOff;
                            pic_Y5_6.Image = Y5.BIT_6() == true ? imgOn : imgOff;
                        }));
                    }
                    else if (bAirTight)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 9, out byte X9);
                        if (ret != SUCCESS) continue;
                        ret = focas.PMC_ReadByte(PmcAddrType.Y, 1, out byte Y1);
                        if (ret != SUCCESS) continue;
                        this.Invoke(new Action(() =>
                        {
                            // X9.5
                            pic_X9_5.Image = X9.BIT_5() ? imgOn : imgOff;
                            // Y1.0
                            pic_Y1_0.Image = Y1.BIT_0() ? imgOn : imgOff;
                        }));
                    }
                    else if (bAirPressure)
                    {
                        ret = focas.PMC_ReadByte(PmcAddrType.X, 8, out byte X8);
                        if (ret != SUCCESS) continue;

                        this.Invoke(new Action(() =>
                        {
                            // X8.5
                            pic_X8_5.Image = X8.BIT_5() ? imgOn : imgOff;
                        }));
                    }            
                    else if (bMessage)
                    {
                        // 5秒讀 AlarmHistory
                        //int iAlarmTime = Environment.TickCount - iQueryAlarmTick;
                        //bool bAlarmTime = iAlarmTime > iAlarmTimeInterval;
                        //if (bAlarmTime)
                        //{
                        //    iQueryAlarmTick = Environment.TickCount;
                        //    if (bMessage)
                        //    {
                        //        ret = focas.GetAlarmHistory(out all_history);
                        //        iAlarmTimeInterval = 5000;
                        //    }
                        //    else
                        //    {
                        //        iAlarmTimeInterval = 50;
                        //    }
                        //}
                        //if (bIsAlarm)
                        //{
                        //    focas.GetCurrentAlarm(out alarms);
                        //}
                        //if (status.AlarmNo != 0) //CNC目前有異常發生
                        //{
                        //ReadAlarmHistory(); //讀取歷史訊息
                        //}
                        //else  //CNC目前沒有異常
                        //{
                        //    this.Invoke(new Action(() =>
                        //    {
                        //        //清除目前顯示的異常
                        //        if (CurrentAlarm.Items.Count > 0)
                        //        {
                        //            LB_CurrentAlarm.Items.Clear();
                        //            la_TroubleShooting.Text = "";
                        //            CurrentAlarm.Items.Clear();
                        //        }
                        //    }));
                        //}
                    }


                    #region PMC Message/Alarms
                    // PMC Message/Alarms
                    List<string> pmcMessage = new List<string>();
                    List<string> pmcAlarm = new List<string>();
                    byte[] Pmc_A_Reg = new byte[20];
                    Focas1.IODBPMC0 A0_A4 = new Focas1.IODBPMC0(); //cdata Size=5 
                    Focas1.IODBPMC0 A5_A9 = new Focas1.IODBPMC0(); //cdata Size=5 
                    Focas1.IODBPMC0 A10_A14 = new Focas1.IODBPMC0(); //cdata Size=5 
                    Focas1.IODBPMC0 A15_A19 = new Focas1.IODBPMC0(); //cdata Size=5 
                    int result = Focas1.pmc_rdpmcrng(focas.FlibHndl, (short)PmcAddrType.A, 0, 0, 4, 13, A0_A4);
                    result = Focas1.pmc_rdpmcrng(focas.FlibHndl, (short)PmcAddrType.A, 0, 5, 9, 13, A5_A9);
                    result = Focas1.pmc_rdpmcrng(focas.FlibHndl, (short)PmcAddrType.A, 0, 10, 14, 13, A10_A14);
                    result = Focas1.pmc_rdpmcrng(focas.FlibHndl, (short)PmcAddrType.A, 0, 15, 19, 13, A15_A19);
                    foreach (PmcMessageData data in PmcMsgList)
                    {
                        if (data.Type == PmcMessageType.Message)
                        {
                            byte val = 0;
                            //目前僅讀取A0~A19, 超過當初規劃的範圍, 另外讀取
                            if (data.Address > 19) focas.PMC_ReadByte(PmcAddrType.A, (ushort)(data.Address), out val);
                            else val = Pmc_A_Reg[data.Address];
                            int shift = 1;
                            for (int j = 0; j < 8; j++)//bit0 ~ bit7
                            {
                                if ((val & shift) != 0)
                                {
                                    string code = "EX" + (data.Number + j).ToString("0000");
                                    pmcMessage.Add(code);
                                    break;
                                }
                                shift <<= 1;
                            }
                        }
                        if (data.Type == PmcMessageType.Alarm)
                        {
                            byte val = 0;
                            //目前僅讀取A0~A19, 超過當初規劃的範圍, 另外讀取
                            if (data.Address > 19) focas.PMC_ReadByte(PmcAddrType.A, (ushort)(data.Address), out val);
                            else val = Pmc_A_Reg[data.Address];
                            int shift = 1;
                            for (int j = 0; j < 8; j++)//bit0 ~ bit7
                            {
                                if ((val & shift) != 0)
                                {
                                    string code = "EX" + (data.Number + j).ToString("0000");
                                    pmcAlarm.Add(code);
                                    break;
                                }
                                shift <<= 1;
                            }
                        }
                    }
                    this.Invoke(new Action(() =>
                    {
                        //讀取PMC 訊息
                        bool bShowMsg = false;
                        la_Tip.Text = "";
                        if (pmcMessage.Count == 0)
                        {
                            pa_Tip.Visible = false;
                        }
                        foreach (string code in pmcMessage)
                        {
                            var resilt = PmcAlarm.FindCode(code);
                            la_Tip.Text = code + " " + resilt?.Msg ?? "";
                            bShowMsg = true;
                            break;
                        }
                        //在 Top 顯示訊息(黃底)
                        pa_Tip.Visible = bShowMsg;

                        //讀取PMC Alarm
                        bool bShowAlm = false;
                        la_AlarmTip.Text = "";
                        if (pmcAlarm.Count == 0)
                        {
                            pa_AlarmTip.Visible = false;
                        }
                        foreach (string code in pmcAlarm)
                        {
                            var resilt = PmcAlarm.FindCode(code);
                            la_AlarmTip.Text = code + " " + resilt?.Msg ?? "";
                            bShowAlm = true;
                            break;
                        }
                        //在 Top 顯示訊息(紅底)
                        pa_AlarmTip.Visible = bShowAlm;
                    }));

                    byte R135; //OCD 手動模式判斷的R
                    ret = focas.PMC_ReadByte(PmcAddrType.R, 135, out R135);
                    String AutName = string.Empty;
                    //狀態
                    ret = focas.GetStatusInfo(out StatusInfo status);
                    if (status != null)
                    {
                        AutName = status.Automatic;
                        this.Invoke((Action)(() =>
                        {
                            pa_Alarm.Visible = status.AlarmNo != 0;
                            pa_EMG.Visible = status.EmergncyNo == 1;

                            btn_AUTO.Lamp = AutName == "MEM";
                            btn_EDIT.Lamp = AutName == "EDIT";
                            btn_MDI.Lamp = AutName == "MDI";
                            btn_MPG.Lamp = bCycleStart && R135.BIT_5() && !btn_AUTO.Lamp;
                            btn_JOG.Lamp = bCycleStart && R135.BIT_4() && !btn_AUTO.Lamp;
                            btn_HOME.Lamp = AutName == "Reference";
                        }));
                    }
                    #endregion PMC Message/Alarms

                }//end 200 ms cycle

                int Request_CNC_Time = Environment.TickCount - Request_CNC_Time_Start;
                if (Request_CNC_Time > 1000)
                {
                    Request_CNC_Time_Start = Environment.TickCount;
                    focas.GetTimer(out DateTime dt); //FANUC CNC 上的時間
                    this.Invoke((Action)(() =>
                    {
                        la_Time.Text = dt.ToString("yyyy/MM/dd HH:mm");
                    }));
                }


                //畫面事件中要讀取控制器的動作
                while (Actions.Count > 0)
                {
                                      
                    Action action = Actions.Dequeue();
                    action();
                }

            }
            //bCloseFinish = true;
        }


        private void pic_Language_Click(object sender, EventArgs e)
        {
            TC_Main.SelectedTab = tab_Language;

            PrevPage.Push(tab_Language);
            btn_Prev.Visible = true;

            TIniFile ini = new TIniFile(Application.StartupPath + "\\Language\\" + Units.LangCode + "\\font.ini");
            la_Current_Language_Val.Text = ini.ReadString("lang", "lang", Units.LangCode);
            {
                string languagePath = Application.StartupPath + "\\Language";
                if (Directory.Exists(languagePath))
                {
                    dgv_Language.Rows.Clear();
                    // 取得 Language 資料夾下的所有子資料夾
                    string[] subDirectories = Directory.GetDirectories(languagePath);
                    // 遍歷每個子資料夾並根據名稱新增到 DataGridView
                    foreach (string dir in subDirectories)
                    {
                        string folderName = Path.GetFileName(dir); // 取得子資料夾名稱
                        TIniFile lang_ini = new TIniFile(Application.StartupPath + "\\Language\\" + folderName + "\\font.ini");

                        string filename = Application.StartupPath + "\\Language\\" + folderName + "\\img.png";

                        if (File.Exists(filename))
                            dgv_Language.Rows.Add(lang_ini.ReadString("lang", "lang", folderName), folderName, Image.FromFile(filename)); // 預設將未知資料夾名稱直接顯示
                        else
                            dgv_Language.Rows.Add(lang_ini.ReadString("lang", "lang", folderName), folderName); //如果沒圖片就不顯示

                    }
                }
            }
        }

        private void SetLanguage(string lang, bool bFormLoad = false)
        {
            TIniFile ini = new TIniFile(Application.StartupPath + "\\sys.ini");
            ini.WriteString("System", "Language", lang);
            Units.langfile = Application.StartupPath + "\\Language\\" + lang + "\\" + lang + ".txt";
            this.LoadLanguageFile(Units.langfile, "Fo_Main");

            TIniFile langini = new TIniFile(Application.StartupPath + "\\Language\\" + lang + "\\font.ini");
            la_Current_Language_Val.Text = langini.ReadString("lang", "lang", Units.LangCode);

            String tmpFileName;
            tmpFileName = Application.StartupPath + "\\Language\\" + lang + "\\Alarm.txt";
            if (!File.Exists(tmpFileName)) Fo_Msg.Show("Alarm.txt " + LanguageManager.LoadMessage(Units.langfile, "Message", 12, "檔案丟失"));
            TroubleShootingFile1 = new AlarmFile(tmpFileName);//異常表
            //Units.alarmfile.LoadFromFile(tmpFileName);

            tmpFileName = Application.StartupPath + "\\Language\\" + lang + "\\PmcMessageLang.txt";
            if (!File.Exists(tmpFileName)) Fo_Msg.Show("PmcMessageLang.txt " + LanguageManager.LoadMessage(Units.langfile, "Message", 12, "檔案丟失"));
            PmcAlarm = new AlarmFile(tmpFileName);//異常表
        }

        private void pic_LanguageSelect_Click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            if (pic == null) return;
            string lang = pic.Tag.ToString();


            SetLanguage(lang);
        }


        private void btn_Language_Save_Click(object sender, EventArgs e)
        {
            int index = dgv_Language.CurrentRow.Index;
            if (index < 0) return;

            TIniFile ini = new TIniFile(Application.StartupPath + "\\sys.ini");
            string lang = dgv_Language.CurrentRow.Cells[Col_LangCode.Index].Value.ToString();
            Units.LangCode = lang;
            SetLanguage(lang);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //如果兩個都開了
            if (pa_Alarm.Visible == pa_EMG.Visible)
            {
                //同步化
                if (la_Alarm.Visible != la_EMG.Visible)
                {
                    la_Alarm.Visible = la_EMG.Visible;
                }
            }

            la_Alarm.Visible = !la_Alarm.Visible;
            la_EMG.Visible = !la_EMG.Visible;
        }

        private void ReadAlarmHistory()
        {
            //時間,英文代碼+數字,軸號,系統號
            int ret = focas.GetAlarmHistory(out string[] all_history);
            if (ret != SUCCESS) return;

            //越新的資料在越後面，所以要先反轉一次再判斷
            all_history = all_history.Reverse().ToArray();
            this.Invoke(new Action(() =>
            {
                //直接清掉
                dgv_AlarmHistory.Rows.Clear();

                foreach (string alarm in all_history)
                {
                    string[] csv = alarm.Split(',');
                    if (csv.Length == 4)
                    {
                        string time = csv[0];
                        string code = csv[1];
                        string axis = csv[2];
                        string path = csv[3];
                        
                        string alarm_msg = TroubleShootingFile1.FindCode(code) != null ? TroubleShootingFile1.FindCode(code).Msg : "";
                        if (alarm_msg == "" && PmcAlarm.FindCode(code) != null) alarm_msg = PmcAlarm.FindCode(code).Msg;//異常訊息修改 

                        int.TryParse(axis, out int axis_no);

                        if (axis_no > 0)
                        {
                            if (dicAxisName.ContainsKey(axis_no))
                            {
                                alarm_msg = axis_no + LanguageManager.LoadMessage(Units.langfile, "Message", 44, "軸") + "(" + dicAxisName[axis_no] + ")" + alarm_msg;
                            }
                            else
                            {
                                alarm_msg = axis_no + LanguageManager.LoadMessage(Units.langfile, "Message", 44, "軸") + "(" + axis_no + ")" + alarm_msg;
                            }
                        }
                        dgv_AlarmHistory.Rows.Add(time, code, alarm_msg);
                    }
                }

                dgv_AlarmHistory.Sort(Col_Alm_Time, ListSortDirection.Descending);
            }));


            //取得 目前正在發生的Alarm
            ret = focas.GetCurrentAlarm(out String[] alarms);
            if (ret != SUCCESS) return;
            
            if (CurrentAlarmBuf != null)
            {
                //檢查是否剛上次一樣
                bool isSame = alarms.OrderBy(x => x).SequenceEqual(CurrentAlarmBuf.OrderBy(x => x));
                if (isSame) return;//一樣就不處理, 避免閃爍
            }
            
            CurrentAlarmBuf = alarms;
            CurrentAlarm.Items.Clear();
            foreach (String s in alarms)
            {
                //分割資料(Code, Axis)
                String[] csv = s.Split(',');
                if (csv.Length >= 2)
                {
                    CurrentAlarm.Items.Add(new Alarm() { Code = csv[0], Msg = "", Axis = int.Parse(csv[1]) });
                }
            }

            //更新ListBox顯示的資料
            this.Invoke((Action)(() =>
            {
                //清除所有
                LB_CurrentAlarm.Items.Clear();

                //這次全部的清單
                foreach (Alarm data in CurrentAlarm.Items)
                {
                    //從檔案中去搜尋此異常代碼的資訊(Troubleshooting)
                    Alarm a1 = TroubleShootingFile1.FindCode(data.Code);
                    //找到資訊
                    if (a1 != null)
                    {
                        //加入到ListBox 去顯示，將檔案搜尋到的資訊顯示出來 (不含異常排除)
                        string axis_str = "";
                        if (data.Axis > 0)
                        {
                            if (dicAxisName.ContainsKey(data.Axis)) axis_str = LanguageManager.LoadMessage(Units.langfile, "Message", 44, "軸") + "(" + dicAxisName[data.Axis] + ")";
                            else axis_str = LanguageManager.LoadMessage(Units.langfile, "Message", 44, "軸") + "(" + data.Axis + ")";
                        }
                        LB_CurrentAlarm.Items.Add(a1.Code + " " + axis_str + a1.Msg);
                        data.TroubleShooting = a1.TroubleShooting;
                    }
                    //找不到資訊
                    else
                    {
                        //加入到ListBox 去顯示，建立一個空的資訊，僅顯示Alarm Code
                        if (PmcAlarm.FindCode(data.Code) != null)
                        {
                            LB_CurrentAlarm.Items.Add(data.Code + " " + PmcAlarm.FindCode(data.Code).Msg);
                        }
                        else
                        {
                            LB_CurrentAlarm.Items.Add(data.Code);
                        }
                    }
                }
            }));

        }
        private void btn_Prev_Click(object sender, EventArgs e)
        {
            PrevPage.Pop();
            TC_Main.SelectedTab = PrevPage.Peek();
            if (TC_Main.SelectedTab == tab_AutoDoor)
            {
                ucBtn_AuotoDoor.PerformClick();
            }

            if (PrevPage.Count == 1)
            {
                btn_Prev.Visible = false;
            }
        }

        private void btn_Pa_ButtomBtn_Click(object sender, EventArgs e)
        {
            Uc_RoundBtn funBtn = (Uc_RoundBtn)sender;
            tb_ProgramTest.Text = string.Empty;
            if (funBtn.Tag != null)
            {
                switch (funBtn.Tag.ToString())
                {
                    case "OverView":
                        TC_Main.SelectedTab = tab_FuntionOverView;
                        ucBtn_Specific1.Visible = ucBtn_Specific2.Visible = false;
                        ucBtn_NextPage.Parent = tab_UseUI;
                        break;
                    case "Message":
                        TC_Main.SelectedTab = tab_Message;
                        ucBtn_Specific1.Visible = ucBtn_Specific2.Visible = false;
                        ucBtn_NextPage.Parent = tab_UseUI;

                        Actions.Enqueue(new Action(() =>
                        {
                            ReadAlarmHistory();
                        }));
                        break;
                }

                PrevPage.Clear();
                PrevPage.Push(TC_Main.SelectedTab);
                btn_Prev.Visible = false;
            }
        }

        private void btn_FunTionOverView_Click(object sender, EventArgs e)
        {
            Uc_RoundBtn funBtn = (Uc_RoundBtn)sender;
            TIniFile langini = new TIniFile(Units.langfile);
            if (funBtn.Tag != null)
            {
                switch (funBtn.Tag.ToString())
                {
                    case "1":
                        TC_Main.SelectedTab = tab_Probe;
                        ProbeData();
                        tb_ProgramTest.Text = "M13;" + "\r\n" + "G31 W1. F0.01 P1;" + "\r\n" + "M14;" + "\r\n" + "M30;" + "\r\n";
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "ExtendRetract", "Extend\nRetract");
                        ucBtn_Specific1.Tag = "2510.7";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "2":
                        TC_Main.SelectedTab = tab_Chuck;
                        ChuckData();
                        tb_ProgramTest.Text = "M10;" + "\r\n" + "M11;" + "\r\n" + "M30;" + "\r\n";
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "ReliefValve", "Uclamp\nClamp");
                        ucBtn_Specific1.Tag = "2511.2";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "3":
                        TC_Main.SelectedTab = tab_WorkSpindle;
                        WorkSpindleData();
                        tb_ProgramTest.Text = "M03 S100;" + "\r\n" + "G4X3.;" + "\r\n" + "M05;" + "\r\n" +
                            "G4X3.;" + "\r\n" + "M04 S100;" + "\r\n" + "G4X3.;" + "\r\n" + "M05;" + "\r\n" + "M30;" + "\r\n";
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "Fun_CW", "CW\nStart\nStop\n");
                        ucBtn_Specific2.DisplayText = langini.ReadString("Fo_Main", "Fun_CCW", "CCW\nStart\nStop\n");
                        ucBtn_Specific1.Tag = "2520.6";
                        ucBtn_Specific2.Tag = "2520.7";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = true;
                        break;
                    case "4":
                        TC_Main.SelectedTab = tab_Orientation;
                        OrientationData();
                        tb_ProgramTest.Text = "(1. Use Sensor)" + "\r\n" + "M03S50;" + "\r\n" + "G4X3.;" + "\r\n" +
                            "M19;" + "\r\n" + "M30;" + "\r\n" + "(2.Use FANUC Spindle )" + "\r\n" + "M19B10000; (Spindle C100 Position)" + "\r\n" + "M30;" + "\r\n";

                        ucBtn_Specific1.Visible = false;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "5":
                        TC_Main.SelectedTab = tab_GW1;
                        GW1Data();
                        tb_ProgramTest.Text = "M20;" + "\r\n" + "G4X3.;" + "\r\n" + "M21;" + "\r\n" + "M30;";
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "Fun_CW", "CW\nStart\nStop\n");
                        ucBtn_Specific1.Tag = "2525.0";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "6":
                        TC_Main.SelectedTab = tab_GW2;
                        GW2Data();
                        tb_ProgramTest.Text = "M22;" + "\r\n" + "G4X3.;" + "\r\n" + "M23;" + "\r\n" + "M30;";
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "Fun_CW", "CW\nStart\nStop\n");
                        ucBtn_Specific1.Tag = "2525.1";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "7":
                        TC_Main.SelectedTab = tab_Gauge;
                        GaugeData();
                        tb_ProgramTest.Text = "M15 B1;" + "\r\n" + "G31 W1. F0.01 P1;" + "\r\n" + "G31 W1. F0.01 P2;" +
                            "\r\n" + "G31 W1. F0.01 P3; " + "\r\n" + "G31 W1. F0.01 P4;" + "\r\n" + "M16;" + "\r\n" + "M30;";
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "Meas1", "Meas.1\nExtend\nRetract\n");
                        ucBtn_Specific1.Tag = "2511.3";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        
                        break;
                    case "8":
                        TC_Main.SelectedTab = tab_SafetyDoor;
                        SafetyDoorData();
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "UnLock", "UnLock");
                        ucBtn_Specific1.Tag = "2511.6";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        la_K12_3.Parent = pic_K12_3.Parent = la_K12_3Desc.Parent = pa_PSW03.Parent =  tab_SafetyDoor;
                        la_K12_3.Location = new System.Drawing.Point(8, 136);
                        pic_K12_3.Location = new System.Drawing.Point(96, 136);
                        la_K12_3Desc.Location = new System.Drawing.Point(128, 136);
                        pa_PSW03.Location = new System.Drawing.Point(0, 160);
                        
                        pa_PSW03.Visible = pic_K12_3.Image != null;
                        la_K14_2.Parent = pic_K14_2.Parent = la_K14_2Desc.Parent = pa_PSW04.Parent = tab_SafetyDoor;
                        la_K14_2.Location = new System.Drawing.Point(8, 296);
                        pic_K14_2.Location = new System.Drawing.Point(96, 296);
                        la_K14_2Desc.Location = new System.Drawing.Point(128, 296);
                        pa_PSW04.Location = new System.Drawing.Point(0, 320);
                        pa_PSW04.Visible = pic_K14_2.Image != null;
                        break;
                    case "9":
                        TC_Main.SelectedTab = tab_AutoDoor;
                        AutoDoorData();
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "OpenClose", "UnLock");
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        
                        if (pic_D70_0.Image == null)
                        {
                            //ucBtn_NextPage.Parent = pa_Bottom;
                            //ucBtn_NextPage.Location = new Point(128, 8);
                            tb_ProgramTest.Text = progCourtyardDoor;
                            ucBtn_Specific1.Tag = "2510.5";
                        }
                        else
                        {
                            //ucBtn_NextPage.Parent = tab_UseUI;
                            tb_ProgramTest.Text = progFrontDoor;
                            ucBtn_Specific1.Tag = "2511.6";
                        }
                        la_K12_3.Parent = pic_K12_3.Parent = la_K12_3Desc.Parent = pa_PSW03.Parent = tab_AutoDoor;
                        la_K12_3.Location = new System.Drawing.Point(8, 136);
                        pic_K12_3.Location = new System.Drawing.Point(96, 136);
                        la_K12_3Desc.Location = new System.Drawing.Point(128, 136);
                        pa_PSW03.Location = new System.Drawing.Point(0, 160);
                        la_K14_2.Parent = pic_K14_2.Parent = la_K14_2Desc.Parent = pa_PSW04.Parent = tab_AutoDoor;
                        la_K14_2.Location = new System.Drawing.Point(8, 296);
                        pic_K14_2.Location = new System.Drawing.Point(96, 296);
                        la_K14_2Desc.Location = new System.Drawing.Point(128, 296);
                        pa_PSW04.Location = new System.Drawing.Point(0, 320);
                        break;
                    case "10":
                        TC_Main.SelectedTab = tab_Acoustic;
                        AcousticData();
                        tb_ProgramTest.Text = "(GAP Test)" + "\r\n" + "M416;" + "\r\n" + "M61;" + "\r\n" + "M63;" + "\r\n" + "G31W1.F0.01P2;" +
                            "\r\n" + "M417;" + "\r\n" + "M30;" + "\r\n" + "(CRASH Test)" + "\r\n" + "M96P2;" + "\r\n" + "M61;" + "\r\n" + "M63;" +
                            "\r\n" + "G4X100.;" + "\r\n" + "M417;" + "\r\n" + "M30;";
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "StartStop", "Start\nStop");
                        ucBtn_Specific1.Tag = "2511.7";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        break;            
                    case "11":
                        TC_Main.SelectedTab = tab_OilMistCollect;
                        OilMistCollectData();
                        tb_ProgramTest.Text = "M35;" + "\r\n" + "G4X3.;" + "\r\n" + "M36;" + "\r\n" + "M30;";
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "StartStop", "Start\nStop");
                        ucBtn_Specific1.Tag = "2511.4";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "12":
                        TC_Main.SelectedTab = tab_AxisLube;
                        AxisLubeData();
                        tb_ProgramTest.Text = "M06;" + "\r\n" + "M30;";
                        ucBtn_Specific1.Visible = false;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "13":
                        TC_Main.SelectedTab = tab_OilCooler;
                        OilCoolerData();
                        ucBtn_Specific1.Visible = false;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "14":
                        TC_Main.SelectedTab = tab_CoolantCooler;
                        CoolantCoolerData();
                        ucBtn_Specific1.Visible = false;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "15":
                        TC_Main.SelectedTab = tab_CoolantSystem;
                        CoolantSystemData();
                        tb_ProgramTest.Text = "M20;" + "\r\n" + "M08;" + "\r\n" + "G4X3.;" + "\r\n" + "M09;" + "\r\n" + "M30;";
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "StartStop", "Start\nStop");
                        ucBtn_Specific1.Tag = "2525.3";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        pic_Y0_0.Parent = la_Y0_0.Parent = pa_CoolantSystemIO;
                        pic_Y0_0.Location = new Point(360, 8);
                        la_Y0_0.Location = new Point(400, 6);
                        break;
                    case "16":
                        TC_Main.SelectedTab = tab_CoolantGun;
                        CoolantGunData();
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "StartStop", "Start\nStop");
                        ucBtn_Specific1.Tag = "2524.0";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        pic_Y0_0.Parent = la_Y0_0.Parent = tab_CoolantGun;
                        pic_Y0_0.Location = new Point(320, 304);
                        la_Y0_0.Location = new Point(360, 312);
                        break;
                    case "17":
                        TC_Main.SelectedTab = tab_LightCurtain;
                        LightCurtainData();
                        ucBtn_Specific1.Visible = false;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "18":
                        TC_Main.SelectedTab = tab_WheelCover;
                        WheelCoverData();
                        pa_PSW01.Visible = pic_K12_6.Image != null;
                        pa_PSW02.Visible = pic_K12_7.Image != null;
                        string zAxis = "M43;" + "\r\n" + "G4X3.;" + "\r\n" + "M44;" + "\r\n" + "M30;";
                        string yAxis = "M53;" + "\r\n" + "G4X3.;" + "\r\n" + "M54;" + "\r\n" + "M30;";
                        if (pic_K12_6.Image != null)
                        {
                            tb_ProgramTest.Text = zAxis;
                        }
                        if (pic_K12_7.Image != null)
                        {
                            tb_ProgramTest.Text = yAxis;
                        }
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "StartStop", "Start\nStop");
                        ucBtn_Specific1.Tag = "2510.5";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        
                        break;
                    case "19":
                        TC_Main.SelectedTab = tab_MaintainDoor;
                        MaintainDoorData();
                        ucBtn_Specific1.Visible = false;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "20":
                        TC_Main.SelectedTab = tab_HydraulicOpt;
                        HydraulicOptData();
                        ucBtn_Specific1.Visible = false;
                        ucBtn_Specific2.Visible = false;
                        
                        break;
                    case "21":
                        TC_Main.SelectedTab = tab_GWOilMistLube;
                        GWOilMistLubeData();
                        
                        ucBtn_Specific1.Visible = false;
                        ucBtn_Specific2.Visible = false;               
                        break;
                    case "22":
                        TC_Main.SelectedTab = tab_OilDressDevice;
                        OilDressDeviceData();
                        tb_ProgramTest.Text = "G30W0. P3;" + "\r\n" + "M46;" + "\r\n" + "G4X3;" + "\r\n" + "M30;";
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "ExtendRetract", "Extend\nRetract");
                        ucBtn_Specific1.Tag = "2510.5";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "23":
                        TC_Main.SelectedTab = tab_Roller;
                        RollerData();
                        tb_ProgramTest.Text = "M37;" + "\r\n" + "G4X3.;" + "\r\n" + "M39;" + "\r\n" + "M38; (Roller CCW.)" +
                            "\r\n" + "G4X3.;" + "\r\n" + "M39; (Roller Stop)" + "\r\n" + "M30;";
                        ucBtn_Specific1.DisplayText = langini.ReadString("Fo_Main", "Fun_CW", "CW");
                        ucBtn_Specific2.DisplayText = langini.ReadString("Fo_Main", "Fun_CCW", "CCW");
                        ucBtn_Specific1.Tag = "2511.1";
                        ucBtn_Specific2.Tag = "2511.0";
                        ucBtn_Specific1.Visible = true;
                        ucBtn_Specific2.Visible = true;
                        break;
                    case "24":
                        TC_Main.SelectedTab = tab_AirTight;
                        AirTightData();
                        ucBtn_Specific1.Visible = false;
                        ucBtn_Specific2.Visible = false;
                        break;
                    case "25":
                        TC_Main.SelectedTab = tab_AirPressure;
                        AirPressureData();
                        ucBtn_Specific1.Visible = false;
                        ucBtn_Specific2.Visible = false;
                        break;
                    
                    case "26":
                        TC_Main.SelectedTab = tab_LinearScale;
                        ucBtn_Specific1.Visible = false;
                        ucBtn_Specific2.Visible = false;
                        LinearScaleData(pic_LinearScaleAxis.Tag.ToString());
                        break;
                }

                tb_ProgramTest.Parent = uc_UserNumInput.Parent = TC_Main.SelectedTab;
                if (TC_Main.SelectedTab == tab_SafetyDoor || TC_Main.SelectedTab == tab_OilCooler || TC_Main.SelectedTab == tab_CoolantCooler ||
                    TC_Main.SelectedTab == tab_CoolantGun || TC_Main.SelectedTab == tab_LightCurtain || TC_Main.SelectedTab == tab_MaintainDoor ||
                    TC_Main.SelectedTab == tab_HydraulicOpt  || TC_Main.SelectedTab == tab_AirPressure || TC_Main.SelectedTab == tab_GWOilMistLube ||
                    TC_Main.SelectedTab == tab_AirTight || TC_Main.SelectedTab == tab_AirPressure || TC_Main.SelectedTab == tab_LinearScale)
                {
                    tb_ProgramTest.Parent = tab_UseUI;
                    bool bNotShowNumInput = TC_Main.SelectedTab == tab_OilCooler || TC_Main.SelectedTab == tab_CoolantCooler ||
                        TC_Main.SelectedTab == tab_CoolantGun || TC_Main.SelectedTab == tab_LightCurtain || TC_Main.SelectedTab == tab_MaintainDoor ||
                         TC_Main.SelectedTab == tab_AirPressure;
                    if (bNotShowNumInput) uc_UserNumInput.Parent = tab_UseUI;
                    ucBtn_KeyBoard.Visible = ucBtn_FunProgramTest.Visible = false;
                }
                uc_UserNumInput.Location = new Point(616, 320);
                // 頁面文字太長
                
                pa_WorkSpindleIO.SendToBack();
                la_T54Desc.SendToBack();
                la_T54s.SendToBack();
                la_T58Desc.SendToBack();
                la_T58s.SendToBack();
                la_T60Desc.SendToBack();
                la_T60s.SendToBack();
                la_T64Desc.SendToBack();
                la_T64s.SendToBack();
                pa_GaugeIO2.SendToBack();
                la_T40Desc.SendToBack();
                pa_PSW03.SendToBack();               
                pa_AutoDoorIO2.SendToBack();
                la_T100Desc.SendToBack();
                pa_AutoDoorIO.SendToBack();
                pa_CoolantSystemIO.SendToBack();
                pa_WheelCoverIO.SendToBack();
                la_T82Desc.SendToBack();
                PrevPage.Clear();
                PrevPage.Push(TC_Main.SelectedTab);
                btn_Prev.Visible = false;
            }
        }

        Uc_RoundBtn ucBtn_CurrentClicked;

        private void ucBtn_InputData_Click(object sender, EventArgs e)
        {
            Uc_RoundBtn btn = (Uc_RoundBtn)sender;

            if (ucBtn_CurrentClicked != null)
            {
                ucBtn_CurrentClicked.Lamp = false;
            }
            btn.Lamp = true;
            ucBtn_CurrentClicked = btn;

            Uc_UserNum uc_UserNumCurrent = uc_UserNumInput;
            string findTitle = "la_T";
            if (btn.Name.Contains("D") || btn.Name.Contains("N") || btn.Name.Contains("C"))
            {
                findTitle = "la_";
            }

            Control[] foundControls = this.Controls.Find(findTitle + ucBtn_CurrentClicked.Tag.ToString() + "Desc", true);
            if (foundControls.Length > 0)
            {
                uc_UserNumCurrent.la_Msg.Text = foundControls[0].Text;
            }

            uc_UserNumCurrent.la_Num.Text = ucBtn_CurrentClicked.DisplayText;
        }

        private void uc_UserNumInput_OnBtnOkClick(object sender, EventArgs e)
        {
            double.TryParse(uc_UserNumInput.la_Num.Text, out double data);
            string btnTag = ucBtn_CurrentClicked.Tag.ToString();
            bool bNum = false;
            if (!string.IsNullOrEmpty(btnTag))
            {
                bNum = int.TryParse(ucBtn_CurrentClicked.Tag.ToString(), out int no);
                int ret = -1, msData = 0;
                bool bFinish = false;
                bool bLinearScale = TC_Main.SelectedTab != tab_LinearScale;
                short axis = 0;
                if (!bLinearScale && pic_LinearScaleAxis.Tag != null)
                {
                    if (pic_LinearScaleAxis.Tag.ToString().ToUpper() == "X")
                    {
                        axis = 1;
                    }
                    if (pic_LinearScaleAxis.Tag.ToString().ToUpper() == "Z")
                    {
                        axis = 2;
                    }
                }
                Actions.Enqueue(new Action(() =>
                {
                    if (bNum) // Timer
                    {
                        focas.PMC_WriteTimer(no, (int)Math.Round(data * 1000.0));
                        Thread.Sleep(50);
                        ret = focas.PMC_ReadTimer(no, out msData);
                    }
                    else if (btnTag.ToUpper() == "D200")
                    {
                        if (int.TryParse(btnTag.ToUpper().Replace("D", ""), out int typeDAddr))
                        {
                            ret = focas.PMC_WriteDbWord(PmcAddrType.D, (short)typeDAddr, (int)data);
                        }
                    }
                    else if (btnTag.ToUpper().Contains("D"))
                    {
                        if (int.TryParse(btnTag.ToUpper().Replace("D", ""), out int typeDAddr))
                        {
                            ret = focas.PMC_WriteWord(PmcAddrType.D, (short)typeDAddr, (short)data);
                        }
                    }
                    else if (btnTag.ToUpper().Contains("N"))
                    {
                        if (bLinearScale)
                        {
                            if (int.TryParse(btnTag.ToUpper().Replace("N", ""), out int typeDAddr))
                            {
                                ret = focas.Param_WriteDouble((short)typeDAddr, data, 0);
                            }
                        }
                        else if (!bLinearScale)
                        {
                            if (int.TryParse(btnTag.ToUpper().Replace("N", ""), out int typeDAddr))
                            {
                                //ret = focas.Param_WriteDbWord((short)typeDAddr, (int)data, axis);
                                ret = focas.Param_WriteWord((short)typeDAddr, (short)data, axis);
                            }
                        }

                    }
                    else if (btnTag.ToUpper().Contains("C"))
                    {
                        if (int.TryParse(btnTag.ToUpper().Replace("C", ""), out int typeDAddr))
                        {
                            ret = focas.PMC_WriteWord(PmcAddrType.C, (short)typeDAddr, (short)data);
                        }
                    }
                    bFinish = true;
                }));
                NeedWait(ref bFinish);
                if (ret == SUCCESS)
                {
                    if (bNum) // Timer
                    {
                        ucBtn_CurrentClicked.DisplayText = (msData / 1000.0).ToString("F1");
                    }
                    else
                    {
                        ucBtn_CurrentClicked.DisplayText = data.ToString();
                    }
                }
            }
            //ucBtn_CurrentClicked.DisplayText = data.ToString();
        }


        private void cb_PMCTypeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            int val = combo.SelectedIndex;

            if (int.TryParse(combo.Tag.ToString(), out int addr) && val > -1)
            {
                Actions.Enqueue(new Action(() =>
                {
                    focas.PMC_WriteWord(PmcAddrType.D, (short)addr, (byte)val);
                }));

            }
        }

        private void cb_ParameterSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            int val = combo.SelectedIndex;

            if (int.TryParse(combo.Tag.ToString(), out int addr) && val > -1)
            {
                bool bLinearScale = TC_Main.SelectedTab == tab_LinearScale;
                short axis = 0;
                if (bLinearScale && pic_LinearScaleAxis.Tag != null)
                {
                    if (pic_LinearScaleAxis.Tag.ToString().ToUpper() == "X")
                    {
                        axis = 1;
                    }
                    if (pic_LinearScaleAxis.Tag.ToString().ToUpper() == "Z")
                    {
                        axis = 2;
                    }
                }
                Actions.Enqueue(new Action(() =>
                {
                    if (bLinearScale)
                    {
                        focas.Param_WriteByte((short)addr, (byte)val, axis);
                    }
                    else if (!bLinearScale)
                    {
                        focas.Param_WriteByte((short)addr, (byte)val, 0);
                    }
                }));

            }
        }
        private void pic_PmcAddrTypeK_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            if (pic.Tag != null)
            {
                string[] csv = pic.Tag.ToString().Substring(1).Split('.');
                if (csv.Length < 2) return;
                int addr = int.Parse(csv[0]);
                int bit = int.Parse(csv[1]);
                bool bOrgCheck = pic.Image == null;
                bool bReverse = bOrgCheck;

                if (csv.Length == 3 && csv[2] == "R")
                {
                    bReverse = !bReverse;
                }
                PictureBox picRadioCheck = null;
                if (csv.Length == 3 && csv[2] == "0")
                {
                    bReverse = false;
                    string picName = "pic_" + pic.Tag.ToString().Replace(".", "_");
                    picName = picName.Substring(0, picName.Length - 1) + "1";
                    Control[] foundControls = this.Controls.Find(picName, true);
                    if (foundControls.Length > 0)
                    {
                        picRadioCheck = (PictureBox)foundControls[0];
                    }
                }
                if (csv.Length == 3 && csv[2] == "1")
                {
                    bReverse = true;
                    string picName = "pic_" + pic.Tag.ToString().Replace(".", "_");
                    picName = picName.Substring(0, picName.Length - 1) + "0";
                    Control[] foundControls = this.Controls.Find(picName, true);
                    if (foundControls.Length > 0)
                    {
                        picRadioCheck = (PictureBox)foundControls[0];
                    }
                }
                // 畫面會有點小lag 先不作等控制器回傳 success
                int ret = 0;
                //bool bFinish = false;
                Actions.Enqueue(new Action(() =>
                {
                    focas.PMC_ReadByte(PmcAddrType.K, (ushort)addr, out byte tmp);
                    tmp = tmp.SetBit(bit, bReverse);
                    ret = focas.PMC_WriteByte(PmcAddrType.K, (short)addr, tmp);
                    //bFinish = true;
                }));
                //NeedWait(ref bFinish);

                if (ret == SUCCESS)
                {
                    if (picRadioCheck == null)
                    {
                        if (bOrgCheck)
                        {
                            pic.Image = Properties.Resources.checkmark;
                        }
                        else
                        {
                            pic.Image = null;
                        }
                    }
                    else
                    {
                        pic.Image = Properties.Resources.checkmark;
                        picRadioCheck.Image = null;
                    }
                    if (pic.Name.Contains("pic_K12_3"))
                    {
                        pa_PSW03.Visible = pic_K12_3.Image != null;
                    }
                    if (pic.Name.Contains("pic_K14_2"))
                    {
                        pa_PSW04.Visible = pic_K14_2.Image != null;
                    }
                    if (pic.Name.Contains("pic_K12_6"))
                    {
                        pa_PSW01.Visible = pic_K12_6.Image != null;
                        tb_ProgramTest.Text = pic_K12_6.Image != null ? "M43;" + "\r\n" + "G4X3.;" + "\r\n" + "M44;" + "\r\n" + "M30;" : "";
                    }
                    if (pic.Name.Contains("pic_K12_7"))
                    {
                        pa_PSW02.Visible = pic_K12_7.Image != null;
                        tb_ProgramTest.Text = pic_K12_7.Image != null ? "M53;" + "\r\n" + "G4X3.;" + "\r\n" + "M54;" + "\r\n" + "M30;" : "";
                    }

                }
            }
        }
        // 工作主軸選主軸類
        private void pic_PmcAddrTypeK_WorkSpindle_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            PictureBox picTemp = null;
            if (pic.Name.Contains("pic_K9_7"))
            {
                picTemp = pic_K15_6;
            }
            if (pic.Name.Contains("pic_K15_6"))
            {
                picTemp = pic_K9_7;
            }
            int addrTemp = -1;
            int bitTemp = -1;
            if (picTemp.Tag != null)
            {
                string[] csvTemp = picTemp.Tag.ToString().Substring(1).Split('.');
                if (csvTemp.Length > 1)
                {
                    addrTemp = int.Parse(csvTemp[0]);
                    bitTemp = int.Parse(csvTemp[1]);
                }
            }
            if (pic.Tag != null)
            {
                string[] csv = pic.Tag.ToString().Substring(1).Split('.');
                if (csv.Length < 2) return;
                int addr = int.Parse(csv[0]);
                int bit = int.Parse(csv[1]);
                bool bCheck = pic.Image == null;
                // 畫面會有點小lag 先不作等控制器回傳 success
                int ret = 0;
                int retTemp = -1;
                //bool bFinish = false;
                Actions.Enqueue(new Action(() =>
                {
                    focas.PMC_ReadByte(PmcAddrType.K, (ushort)addr, out byte tmp);
                    tmp = tmp.SetBit(bit, bCheck);
                    ret = focas.PMC_WriteByte(PmcAddrType.K, (short)addr, tmp);

                    if (addrTemp != -1 && bitTemp != -1)
                    {
                        focas.PMC_ReadByte(PmcAddrType.K, (ushort)addrTemp, out byte tmpTemp);
                        tmpTemp = tmpTemp.SetBit(bitTemp, !bCheck);
                        retTemp = focas.PMC_WriteByte(PmcAddrType.K, (short)addrTemp, tmpTemp);
                    }
                    //bFinish = true;
                }));
                //NeedWait(ref bFinish);

                if (ret == SUCCESS)
                {
                    if (bCheck)
                    {
                        pic.Image = imgCheckMark;
                        picTemp.Image = null;
                    }
                    else
                    {
                        pic.Image = null;
                        picTemp.Image = imgCheckMark;
                    }
                }
            }
        }
        private void pic_ParameterType_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            PictureBox picTemp = null;
            string tabValue = pic.Tag.ToString();
            PmcAddrType pmcAddrType = PmcAddrType.D;
            short DValue = 0;
            short addr = 0;
            bool bCheck = pic.Image == null;
            if (!string.IsNullOrEmpty(tabValue))
            {
                if (pic.Name.Contains("pic_D70_0"))
                {
                    picTemp = pic_D70_1;
                    DValue = 0;
                    addr = 70;
                }
                if (pic.Name.Contains("pic_D70_1"))
                {
                    picTemp = pic_D70_0;
                    DValue = 1;
                    addr = 70;
                }
                // 畫面會有點小lag 先不作等控制器回傳 success
                int ret = 0;
                //bool bFinish = false;
                Actions.Enqueue(new Action(() =>
                {
                    ret = focas.PMC_WriteWord(pmcAddrType, (short)addr, DValue);
                    //bFinish = true;
                }));
                //NeedWait(ref bFinish);
                if (ret == SUCCESS)
                {
                    if (bCheck)
                    {
                        pic.Image = imgCheckMark;
                        picTemp.Image = null;
                    }
                    else
                    {
                        pic.Image = null;
                        picTemp.Image = imgCheckMark;
                    }
                    if (pic.Name.Contains("D70") && addr == 70)
                    {
                        if (pic_D70_0.Image == null)
                        {
                            //ucBtn_NextPage.Parent = pa_Bottom;
                            //ucBtn_NextPage.Location = new Point(128, 8);
                            tb_ProgramTest.Text = progCourtyardDoor;
                            ucBtn_Specific1.Tag = "2510.5";
                        }
                        else
                        {
                            //ucBtn_NextPage.Parent = tab_UseUI;
                            tb_ProgramTest.Text = progFrontDoor;
                            ucBtn_Specific1.Tag = "2511.6";
                        }
                    }
                }
            }
        }
        // OverView 按鈕特定功能
        private void ucBtn_Specific_Click(object sender, EventArgs e)
        {
            Uc_RoundBtn uc_RoundBtn = (Uc_RoundBtn)sender;
            int address = 0;
            int bitNum = -1;
            if (uc_RoundBtn.Tag != null)
            {
                string[] csv = uc_RoundBtn.Tag.ToString().Split('.');
                if (csv.Length < 2) return;

                int.TryParse(csv[0], out address);
                int.TryParse(csv[1], out bitNum);
            }

            if (address > 0 && bitNum >= 0) WritePmcTypeE((ushort)address, bitNum);
        }
        // 伸出/縮回動作寫入控制器
        private void WritePmcTypeE(ushort address, int bitNum)
        {
            Actions.Enqueue(new Action(() =>
            {
                if (focas.PMC_ReadByte(PmcAddrType.E, address, out byte EAddress) == SUCCESS)
                {
                    focas.PMC_WriteByte(PmcAddrType.E, (short)address, EAddress.SetBit(bitNum, true));
                    Thread.Sleep(50);
                    focas.PMC_WriteByte(PmcAddrType.E, (short)address, EAddress.SetBit(bitNum, false));
                }
            }));
        }

        // 程式測試
        private void ucBtn_FunProgramTest_Click(object sender, EventArgs e)
        {
            string prog = "O0000\n" + tb_ProgramTest.Text.Replace("\r\n", "\n").Replace(";", "").ToUpper();
            if (prog.Last() != '\n') prog += "\n";
            short len = (short)prog.Length;
            bool bFinish = false;
            int ret = -1;
            Actions.Enqueue(new Action(() =>
            {
                ret = Focas1.cnc_wrmdiprog(focas.FlibHndl, len, prog);
                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret != Focas1.EW_OK)
            {
                Fo_Msg.Show("寫入失敗\n請確認在MDI模式下寫入", "錯誤");
                return;
            }
        }

        private void ucBtn_NextPage_Click(object sender, EventArgs e)
        {
            if (TC_Main.SelectedTab == tab_AutoDoor)
            {
                
                PrevPage.Push(TC_Main.SelectedTab);
                btn_Prev.Visible = true;

                tb_ProgramTest.Parent = uc_UserNumInput.Parent = TC_Main.SelectedTab;
                
                la_T40Desc.SendToBack();
                ucBtn_NextPage.Parent = tab_UseUI;
            }
        }

        private void ucBtn_KeyBoard_Click(object sender, EventArgs e)
        {
            try
            {
                Process p = Process.Start(Environment.SystemDirectory + "\\osk.exe");
            }
            catch (Exception)
            {
            }
        }

        private void LB_CurrentAlarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = LB_CurrentAlarm.SelectedIndex;
            if (index < 0)
                return;


            Alarm a = CurrentAlarm.Items[index];
            la_TroubleShooting.Text = a.TroubleShooting.Replace("\\n", "\n");
            if (la_TroubleShooting.Text == "" && PmcAlarm.FindCode(a.Code) != null) la_TroubleShooting.Text = PmcAlarm.FindCode(a.Code).TroubleShooting; //異常訊息修改
        }

        //static bool bFinish = false;
        private void NeedWait(ref bool bFinish)
        {
            DateTime dt_Try3Sencond = DateTime.Now;
            while (!bFinish)
            {
                if (!bCNCConnect || DateTime.Now > dt_Try3Sencond.AddSeconds(3))
                {
                    break;
                }
                Application.DoEvents();//等待通訊結束
            }
        }

        private void la_AlarmTip_MouseDown(object sender, MouseEventArgs e)
        {
            string[] lines = la_AlarmTip.Text.Split('\n');
            int len = 0;
            foreach (string s in lines) if (s.Trim() != "") len++;
            pa_AlarmTip.Height = 22 * len;
            pa_AlarmTip.BringToFront();
        }

        private void la_AlarmTip_MouseUp(object sender, MouseEventArgs e)
        {
            pa_AlarmTip.Height = 22;
            pa_AlarmTip.BringToFront();
        }

        private void la_Tip_MouseDown(object sender, MouseEventArgs e)
        {
            string[] lines = la_Tip.Text.Split('\n');
            int len = 0;
            foreach (string s in lines) if (s.Trim() != "") len++;
            pa_Tip.Height = 22 * len;
            pa_Tip.BringToFront();
        }

        private void la_Tip_MouseUp(object sender, MouseEventArgs e)
        {
            pa_Tip.Height = 22;
            pa_Tip.BringToFront();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            //如果兩個都開了
            if (pa_Alarm.Visible == pa_EMG.Visible)
            {
                //同步化
                if (la_Alarm.Visible != la_EMG.Visible)
                {
                    la_Alarm.Visible = la_EMG.Visible;
                }
            }

            la_Alarm.Visible = !la_Alarm.Visible;
            la_EMG.Visible = !la_EMG.Visible;
        }
        #region 端面量測
        private void ProbeData()
        {
            int ret_K = -1, ret_T = -1, ret_D = -1;
            byte K5 = 0, K14 = 0;
            ushort D56 = 0;
            int msT22 = 0, msT24 = 0, msT56 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 5, out K5);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 14, out K14);

                ret_T = focas.PMC_ReadTimer(22, out msT22);
                ret_T = focas.PMC_ReadTimer(24, out msT24);
                ret_T = focas.PMC_ReadTimer(56, out msT56);

                ret_D = focas.PMC_ReadWord(PmcAddrType.D, 56, out D56);
                bFinish = true;
            }));
            NeedWait(ref bFinish);

            if (ret_K == SUCCESS)
            {
                // 0有效，1無效
                pic_K5_3.Image = K5.BIT_3() == true ? null : imgCheckMark;
            }
            if (ret_T == SUCCESS)
            {
                ucBtn_T22.DisplayText = (msT22 / 1000.0).ToString("F1");
                ucBtn_T24.DisplayText = (msT24 / 1000.0).ToString("F1");
                ucBtn_T56.DisplayText = (msT56 / 1000.0).ToString("F1");
            }
            if (ret_D == SUCCESS)
            {
                if (D56 < cb_ProbeTypeSelect.Items.Count)
                {
                    cb_ProbeTypeSelect.SelectedIndex = D56;
                }
            }
        }
        #endregion

        #region 主軸夾頭
        private void ChuckData()
        {
            int ret_K = -1, ret_T = -1;
            byte K1 = 0, K7 = 0, K22 = 0, K30 = 0, K38 = 0;
            int msT2 = 0, msT4 = 0, msT72 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 1, out K1);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 7, out K7);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 22, out K22);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 30, out K30);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 38, out K38);

                ret_T = focas.PMC_ReadTimer(2, out msT2);
                ret_T = focas.PMC_ReadTimer(4, out msT4);
                ret_T = focas.PMC_ReadTimer(72, out msT72);
                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K30.1
                pic_K30_1_0.Image = K30.BIT_1() == true ? null : imgCheckMark;
                pic_K30_1_1.Image = K30.BIT_1() == true ? imgCheckMark : null;
                // K30.2
                pic_K30_2_0.Image = K30.BIT_2() == true ? null : imgCheckMark;
                pic_K30_2_1.Image = K30.BIT_2() == true ? imgCheckMark : null;

                // K22.2
                pic_K22_2_0.Image = K22.BIT_2() == true ? null : imgCheckMark;
                pic_K22_2_1.Image = K22.BIT_2() == true ? imgCheckMark : null;
                       
                // K30.3
                pic_K30_3.Image = K30.BIT_3() == true ? imgCheckMark : null;
                // K7.5
                pic_K7_5.Image = K7.BIT_5() == true ? imgCheckMark : null;
                // K1.0 0有效 1無效
                pic_K1_0.Image = K1.BIT_0() == true ? null : imgCheckMark;

                pic_K38_1.Image = K38.BIT_1() == true ? imgCheckMark : null;
            }
            if (ret_T == SUCCESS)
            {
                ucBtn_T2.DisplayText = (msT2 / 1000.0).ToString("F1");
                ucBtn_T4.DisplayText = (msT4 / 1000.0).ToString("F1");
                ucBtn_T72.DisplayText = (msT72 / 1000.0).ToString("F1");
            }
        }
        #endregion

        #region 工件主軸
        private void WorkSpindleData()
        {
            int ret_K = -1, ret_T = -1, ret_D = -1;
            byte K0 = 0, K2 = 0, K3 = 0, K8 = 0, K9 = 0, K15 = 0, K22 = 0;
            ushort D60 = 0;
            uint D200 = 0;
            int msT28 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 0, out K0);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 2, out K2);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 3, out K3);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 8, out K8);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 9, out K9);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 15, out K15);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 22, out K22);

                ret_T = focas.PMC_ReadTimer(28, out msT28);

                ret_D = focas.PMC_ReadWord(PmcAddrType.D, 60, out D60);
                ret_D = focas.PMC_ReadDbWord(PmcAddrType.D, 200, out D200);

                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K9.7
                pic_K9_7.Image = K9.BIT_7() == true ? imgCheckMark : null;
                // K8.6
                pic_K8_6_0.Image = K8.BIT_6() == true ? null : imgCheckMark;
                pic_K8_6_1.Image = K8.BIT_6() == true ? imgCheckMark : null;
                // K3.3
                pic_K3_3_0.Image = K3.BIT_3() == true ? null : imgCheckMark;
                pic_K3_3_1.Image = K3.BIT_3() == true ? imgCheckMark : null;
                // K3.4
                pic_K3_4_0.Image = K3.BIT_4() == true ? null : imgCheckMark;
                pic_K3_4_1.Image = K3.BIT_4() == true ? imgCheckMark : null;
                // K0.5
                pic_K0_5_0.Image = K0.BIT_5() == true ? null : imgCheckMark;
                pic_K0_5_1.Image = K0.BIT_5() == true ? imgCheckMark : null;
                // K2.7
                pic_K2_7.Image = K2.BIT_7() == true ? imgCheckMark : null;
                // K2.4 0有效 1無效
                pic_K2_4.Image = K2.BIT_4() == true ? null : imgCheckMark;
                // K22.3
                pic_K22_3.Image = K22.BIT_3() == true ? imgCheckMark : null;
            }
            if (ret_T == SUCCESS)
            {
                ucBtn_T28.DisplayText = (msT28 / 1000.0).ToString("F1");
            }

            if (ret_D == SUCCESS)
            {
                ucBtn_D60.DisplayText = D60.ToString();
                ucBtn_D200.DisplayText = D200.ToString();
            }
        }
        #endregion

        #region 主軸定位
        private void OrientationData()
        {
            int ret_K = -1, ret_D = -1, ret_T = -1;
            byte K1 = 0;
            ushort D62 = 0, D64 = 0;
            int msT38 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 1, out K1);

                ret_D = focas.PMC_ReadWord(PmcAddrType.D, 62, out D62);
                ret_D = focas.PMC_ReadWord(PmcAddrType.D, 64, out D64);

                ret_T = focas.PMC_ReadTimer(38, out msT38);

                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K1.2
                pic_K1_2.Image = K1.BIT_2() == true ? imgCheckMark : null;
            }
            if (ret_D == SUCCESS)
            {
                // D62
                ucBtn_D62.DisplayText = D62.ToString();
                // D64
                ucBtn_D64.DisplayText = D64.ToString();
            }
            if (ret_T == SUCCESS)
            {
                ucBtn_T38.DisplayText = (msT38 / 1000.0).ToString("F1");
            }
        }
        #endregion

        #region 砂輪主軸 1
        private void GW1Data()
        {
            int ret_K = -1, ret_T = -1;
            byte K2 = 0, K3 = 0, K11 = 0;
            int msT6 = 0, msT8 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 2, out K2);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 3, out K3);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 11, out K11);

                ret_T = focas.PMC_ReadTimer(6, out msT6);
                ret_T = focas.PMC_ReadTimer(8, out msT8);

                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K3.1
                pic_K3_1_0.Image = K3.BIT_1() == true ? null : imgCheckMark;
                pic_K3_1_1.Image = K3.BIT_1() == true ? imgCheckMark : null;
                // K3.2
                pic_K3_2_0.Image = K3.BIT_2() == true ? null : imgCheckMark;
                pic_K3_2_1.Image = K3.BIT_2() == true ? imgCheckMark : null;
                // K2.5
                pic_K2_5_0.Image = K2.BIT_5() == true ? null : imgCheckMark;
                pic_K2_5_1.Image = K2.BIT_5() == true ? imgCheckMark : null;
                // K2.6
                pic_K2_6.Image = K2.BIT_6() == true ? imgCheckMark : null;
                // K11.6
                pic_K11_6_0.Image = K11.BIT_6() == true ? null : imgCheckMark;
                pic_K11_6_1.Image = K11.BIT_6() == true ? imgCheckMark : null;
            }

            if (ret_T == SUCCESS)
            {
                ucBtn_T6.DisplayText = (msT6 / 1000.0).ToString("F1");
                ucBtn_T8.DisplayText = (msT8 / 1000.0).ToString("F1");
            }
        }
        #endregion

        #region 砂輪主軸 2
        private void GW2Data()
        {
            int ret_K = -1, ret_T = -1;
            byte K13 = 0;
            int msT66 = 0, msT68 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 13, out K13);

                ret_T = focas.PMC_ReadTimer(66, out msT66);
                ret_T = focas.PMC_ReadTimer(68, out msT68);

                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K13.0
                pic_K13_0.Image = K13.BIT_0() == true ? imgCheckMark : null;
                // K13.1
                pic_K13_1_0.Image = K13.BIT_1() == true ? null : imgCheckMark;
                pic_K13_1_1.Image = K13.BIT_1() == true ? imgCheckMark : null;
                // K13.2
                pic_K13_2_0.Image = K13.BIT_2() == true ? null : imgCheckMark;
                pic_K13_2_1.Image = K13.BIT_2() == true ? imgCheckMark : null;
                // K13.5
                pic_K13_5_0.Image = K13.BIT_5() == true ? null : imgCheckMark;
                pic_K13_5_1.Image = K13.BIT_5() == true ? imgCheckMark : null;
                // K13.4
                pic_K13_4.Image = K13.BIT_4() == true ? imgCheckMark : null;

                pic_K13_6_0.Image = K13.BIT_6() == true ? null : imgCheckMark;
                pic_K13_6_1.Image = K13.BIT_6() == true ? imgCheckMark : null;
            }

            if (ret_T == SUCCESS)
            {
                ucBtn_T66.DisplayText = (msT66 / 1000.0).ToString("F1");
                ucBtn_T68.DisplayText = (msT68 / 1000.0).ToString("F1");
            }
        }
        #endregion

        #region 內徑量測
        private void GaugeData()
        {
            int ret_K = -1, ret_T = -1, ret_D = -1;
            byte K9 = 0, K13 = 0, K39 = 0;
            ushort D54 = 0, D68 = 0;
            int msT54 = 0, msT58 = 0, msT60 = 0, msT64 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 9, out K9);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 13, out K13);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 39, out K39);

                ret_T = focas.PMC_ReadTimer(54, out msT54);
                ret_T = focas.PMC_ReadTimer(58, out msT58);
                ret_T = focas.PMC_ReadTimer(60, out msT60);
                ret_T = focas.PMC_ReadTimer(64, out msT64);

                ret_D = focas.PMC_ReadWord(PmcAddrType.D, 54, out D54);
                ret_D = focas.PMC_ReadWord(PmcAddrType.D, 68, out D68);

                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K9.5
                pic_K9_5.Image = K9.BIT_5() == true ? null : imgCheckMark;
                // K13.3
                pic_K13_3.Image = K13.BIT_3() == true ? imgCheckMark : null;
                // K13.7
                pic_K13_7.Image = K13.BIT_7() == true ? imgCheckMark : null;        
                // K39.0
                pic_K39_0.Image = K39.BIT_0() == true ? imgCheckMark : null;
            }

            if (ret_T == SUCCESS)
            {
                ucBtn_T54.DisplayText = (msT54 / 1000.0).ToString("F1");
                ucBtn_T58.DisplayText = (msT58 / 1000.0).ToString("F1");
                ucBtn_T60.DisplayText = (msT60 / 1000.0).ToString("F1");
                ucBtn_T64.DisplayText = (msT64 / 1000.0).ToString("F1");
            }

            if (ret_D == SUCCESS)
            {
                if (D54 < cb_D54Select.Items.Count)
                {
                    cb_D54Select.SelectedIndex = D54;
                }
                if (D68 < cb_D68Select.Items.Count)
                {
                    cb_D68Select.SelectedIndex = D68;
                }
            }
        }
        #endregion

        #region 安全門鎖
        private void SafetyDoorData()
        {
            int ret_K = -1, ret_T = -1, ret_N = -1;
            byte K12 = 0, K14 = 0;
            int msT50 = 0;
            byte N6912 = 0, N6913 = 0;
            double N6932 = 0, N6952 = 0, N6933 = 0, N6953 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 12, out K12);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 14, out K14);

                ret_T = focas.PMC_ReadTimer(50, out msT50);
                ret_N = focas.Param_ReadByte(6912, 0, out N6912);
                ret_N = focas.Param_ReadDouble(6932, 0, out N6932);
                ret_N = focas.Param_ReadDouble(6952, 0, out N6952);

                ret_N = focas.Param_ReadByte(6913, 0, out N6913);
                ret_N = focas.Param_ReadDouble(6933, 0, out N6933);
                ret_N = focas.Param_ReadDouble(6953, 0, out N6953);
                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K12.2
                pic_K12_2.Image = K12.BIT_2() == true ? imgCheckMark : null;
                // K12.3
                pic_K12_3.Image = K12.BIT_3() == true ? imgCheckMark : null;
                pic_K14_2.Image = K14.BIT_2() == true ? imgCheckMark : null;
            }

            if (ret_T == SUCCESS)
            {
                ucBtn_T50.DisplayText = (msT50 / 1000.0).ToString("F1");
            }
            if (ret_N == SUCCESS)
            {
                if (N6912 < cb_N6912Select.Items.Count)
                {
                    cb_N6912Select.SelectedIndex = N6912;
                }
                ucBtn_N6932.DisplayText = N6932.ToString();
                ucBtn_N6952.DisplayText = N6952.ToString();

                if (N6913 < cb_N6913Select.Items.Count)
                {
                    cb_N6913Select.SelectedIndex = N6913;
                }
                ucBtn_N6933.DisplayText = N6933.ToString();
                ucBtn_N6953.DisplayText = N6953.ToString();
            }


        }
        #endregion

        #region 自動門
        private void AutoDoorData()
        {
            int ret_K = -1, ret_T = -1, ret_N = -1, ret_D = -1;
            byte K11 = 0, K12 = 0, K14 = 0;
            ushort D70 = 0;
            byte N6912 = 0, N6913 = 0;
            double N6932 = 0, N6952 = 0, N6933 = 0, N6953 = 0;
            int msT40 = 0, msT98 = 0, msT100 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
               
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 11, out K11);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 12, out K12);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 14, out K14);

                ret_T = focas.PMC_ReadTimer(40, out msT40);
                ret_T = focas.PMC_ReadTimer(98, out msT98);
                ret_T = focas.PMC_ReadTimer(100, out msT100);

                ret_N = focas.Param_ReadByte(6912, 0, out N6912);
                ret_N = focas.Param_ReadDouble(6932, 0, out N6932);
                ret_N = focas.Param_ReadDouble(6952, 0, out N6952);

                ret_N = focas.Param_ReadByte(6913, 0, out N6913);
                ret_N = focas.Param_ReadDouble(6933, 0, out N6933);
                ret_N = focas.Param_ReadDouble(6953, 0, out N6953);

                ret_D = focas.PMC_ReadWord(PmcAddrType.D, 70, out D70);

                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K11.0
                pic_K11_0.Image = K11.BIT_0() == true ? null : imgCheckMark;
                // K12.3
                pic_K12_3.Image = K12.BIT_3() == true ? imgCheckMark : null;
                // K14.2
                pic_K14_2.Image = K14.BIT_6() == true ? imgCheckMark : null;
            }

            if (ret_T == SUCCESS)
            {
                ucBtn_T40.DisplayText = (msT40 / 1000.0).ToString("F1");
                ucBtn_T98.DisplayText = (msT98 / 1000.0).ToString("F1");
                ucBtn_T100.DisplayText = (msT100 / 1000.0).ToString("F1");
            }

            if (ret_N == SUCCESS)
            {
                if (N6912 < cb_N6912Select.Items.Count)
                {
                    cb_N6912Select.SelectedIndex = N6912;
                }
                ucBtn_N6932.DisplayText = N6932.ToString();
                ucBtn_N6952.DisplayText = N6952.ToString();

                if (N6913 < cb_N6913Select.Items.Count)
                {
                    cb_N6913Select.SelectedIndex = N6913;
                }
                ucBtn_N6933.DisplayText = N6933.ToString();
                ucBtn_N6953.DisplayText = N6953.ToString();
            }

            if (ret_D == SUCCESS)
            {
                pic_D70_0.Image = D70 == 0 ? imgCheckMark : null;
                pic_D70_1.Image = D70 == 1 ? imgCheckMark : null;
            }
        }
        #endregion

        #region 音頻防撞
        private void AcousticData()
        {
            int ret_T = -1, ret_D = -1;
            ushort D52 = 0;
            int msT14 = 0, msT20 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_T = focas.PMC_ReadTimer(14, out msT14);
                ret_T = focas.PMC_ReadTimer(20, out msT20);

                ret_D = focas.PMC_ReadWord(PmcAddrType.D, 52, out D52);
                bFinish = true;
            }));
            NeedWait(ref bFinish);


            if (ret_D == SUCCESS)
            {
                if (D52 < cb_D52Select.Items.Count)
                {
                    cb_D52Select.SelectedIndex = D52;
                }
            }

            if (ret_T == SUCCESS)
            {
                ucBtn_T14.DisplayText = (msT14 / 1000.0).ToString("F1");
                ucBtn_T20.DisplayText = (msT20 / 1000.0).ToString("F1");
            }
        }
        #endregion

        #region 油霧回收機
        private void OilMistCollectData()
        {
            int ret_K = -1;
            byte K25 = 0;

            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 25, out K25);

                bFinish = true;
            }));
            NeedWait(ref bFinish);

            if (ret_K == SUCCESS)
            {
                // K25.6
                pic_K25_6.Image = K25.BIT_6() == true ? imgCheckMark : null;
            }

        }
        #endregion

        #region 注油機
        private void AxisLubeData()
        {
            int ret_K = -1, ret_T = -1, ret_D = -1, ret_C = -1;
            byte K4 = 0, K16 = 0, K20 = 0;
            ushort D850 = 0;
            ushort C0 = 0, C2 = 0;
            int msT42 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 4, out K4);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 16, out K16);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 20, out K20);

                ret_T = focas.PMC_ReadTimer(42, out msT42);

                ret_C = focas.PMC_ReadWord(PmcAddrType.C, 0, out C0);
                ret_C = focas.PMC_ReadWord(PmcAddrType.C, 2, out C2);

                ret_D = focas.PMC_ReadWord(PmcAddrType.D, 850, out D850);

                bFinish = true;
            }));
            NeedWait(ref bFinish);

            if (ret_K == SUCCESS)
            {
                // K4.6
                pic_K4_6_0.Image = K4.BIT_6() == true ? null : imgCheckMark;
                pic_K4_6_1.Image = K4.BIT_6() == true ? imgCheckMark : null;
                // K4.7
                pic_K4_7_0.Image = K4.BIT_7() == true ? null : imgCheckMark;
                pic_K4_7_1.Image = K4.BIT_7() == true ? imgCheckMark : null;
                // K16.1
                pic_K16_1_0.Image = K16.BIT_1() == true ? null : imgCheckMark;
                pic_K16_1_1.Image = K16.BIT_1() == true ? imgCheckMark : null;
                pic_K20_0.Image = K20.BIT_0() == true ? null : imgCheckMark;
            }

            if (ret_T == SUCCESS)
            {
                ucBtn_T42.DisplayText = (msT42 / 1000.0).ToString("F1");
            }

            if (ret_C == SUCCESS)
            {
                ucBtn_C0.DisplayText = C0.ToString();
                ucBtn_C2.DisplayText = C2.ToString();
            }

            if (ret_D == SUCCESS)
            {
                ucBtn_D850.DisplayText = D850.ToString();
            }
        }
        #endregion

        #region 油冷機
        private void OilCoolerData()
        {
            int ret_K = -1;
            byte K9 = 0;

            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 9, out K9);

                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K9.1
                pic_K9_1.Image = K9.BIT_1() == true ? imgCheckMark : null;
            }
        }
        #endregion

        #region 水冷機
        private void CoolantCoolerData()
        {
            int ret_K = -1;
            byte K9 = 0;

            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 9, out K9);


                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K9.1
                pic_K9_4.Image = K9.BIT_4() == true ? imgCheckMark : null;
            }


        }
        #endregion

        #region 水車系統
        private void CoolantSystemData()
        {
            int ret_K = -1, ret_T = -1;
            byte K0 = 0, K5 = 0;

            int msT170 = 0, msT156 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 0, out K0);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 5, out K5);

                ret_T = focas.PMC_ReadTimer(170, out msT170);
                ret_T = focas.PMC_ReadTimer(156, out msT156);

                bFinish = true;
            }));
            NeedWait(ref bFinish);

            if (ret_K == SUCCESS)
            {
                // K0.2
                pic_K0_2.Image = K0.BIT_2() == true ? imgCheckMark : null;
               
                // K5.6
                pic_K5_6.Image = K5.BIT_6() == true ? imgCheckMark : null;
                // K5.7
                pic_K5_7.Image = K5.BIT_7() == true ? imgCheckMark : null;
            }

            if (ret_T == SUCCESS)
            {
                ucBtn_T170.DisplayText = (msT170 / 1000.0).ToString("F1");
                ucBtn_T156.DisplayText = (msT156 / 1000.0).ToString("F1");
            }
        }
        #endregion

        #region 水槍
        private void CoolantGunData()
        {
            int ret_K = -1;
            byte K31 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 31, out K31);
                bFinish = true;
            }));
            NeedWait(ref bFinish);

            if (ret_K == SUCCESS)
            {
                // K31.7
                pic_K31_7.Image = K31.BIT_7() == true ? imgCheckMark : null;
            }

        }
        #endregion

        #region 安全光柵
        private void LightCurtainData()
        {
            int ret_K = -1;
            byte K12 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 12, out K12);


                bFinish = true;
            }));
            NeedWait(ref bFinish);

            if (ret_K == SUCCESS)
            {
                // K12.1
                pic_K12_1.Image = K12.BIT_1() == true ? imgCheckMark : null;
            }


        }
        #endregion

        #region 砂輪遮罩
        private void WheelCoverData()
        {
            int ret_K = -1, ret_N = -1;
            byte K12 = 0;

            byte N6910 = 0, N6911 = 0;
            double N6930 = 0, N6950 = 0, N6931 = 0, N6951 = 0;
            
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 12, out K12);
                
                ret_N = focas.Param_ReadByte(6910, 0, out N6910);
                ret_N = focas.Param_ReadDouble(6930, 0, out N6930);
                ret_N = focas.Param_ReadDouble(6950, 0, out N6950);

                ret_N = focas.Param_ReadByte(6911, 0, out N6911);
                ret_N = focas.Param_ReadDouble(6931, 0, out N6931);
                ret_N = focas.Param_ReadDouble(6951, 0, out N6951);


                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K12.6
                pic_K12_6.Image = K12.BIT_6() == true ? imgCheckMark : null;
                // K12.7
                pic_K12_7.Image = K12.BIT_7() == true ? imgCheckMark : null;

            }

            if (ret_N == SUCCESS)
            {
                if (N6910 < cb_N6910Select.Items.Count)
                {
                    cb_N6910Select.SelectedIndex = N6910;
                }
                ucBtn_N6930.DisplayText = N6930.ToString();
                ucBtn_N6950.DisplayText = N6950.ToString();

                if (N6911 < cb_N6911Select.Items.Count)
                {
                    cb_N6911Select.SelectedIndex = N6911;
                }
                ucBtn_N6931.DisplayText = N6931.ToString();
                ucBtn_N6951.DisplayText = N6951.ToString();
            }
        }
        #endregion

        #region 維修門
        private void MaintainDoorData()
        {
            int ret_K = -1;
            byte K2 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 2, out K2);
                
                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K2.0
                pic_K2_0.Image = K2.BIT_0() == true ? imgCheckMark : null;
                // K2.1
                pic_K2_1.Image = K2.BIT_1() == true ? imgCheckMark : null;
                // K2.2
                pic_K2_2.Image = K2.BIT_2() == true ? imgCheckMark : null;
                // K2.3
                pic_K2_3.Image = K2.BIT_3() == true ? imgCheckMark : null;

            }
        }
        #endregion

        #region 油壓馬達(配件)
        private void HydraulicOptData()
        {
            int ret_K = -1, ret_T = -1;
            byte K3 = 0, K30 = 0;
            int msT18 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 3, out K3);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 30, out K30);

                ret_T = focas.PMC_ReadTimer(18, out msT18);
                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K30.0
                pic_K30_0.Image = K30.BIT_0() == true ? imgCheckMark : null;
                // K3.5
                pic_K3_5.Image = K3.BIT_5() == true ? imgCheckMark : null;
            }
            if (ret_T == SUCCESS)
            {
                ucBtn_T18.DisplayText = (msT18 / 1000.0).ToString("F1");
            }

        }
        #endregion

        #region 砂輪油霧潤滑
        private void GWOilMistLubeData()
        {
            int ret_K = -1, ret_T = -1;
            byte K0 = 0, K4 = 0, K15 = 0, K16 = 0, K31 = 0;

            int msT34 = 0, msT36 = 0, msT152 = 0, msT154 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 0, out K0);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 4, out K4);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 15, out K15);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 16, out K16);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 31, out K31);

                ret_T = focas.PMC_ReadTimer(34, out msT34);
                ret_T = focas.PMC_ReadTimer(36, out msT36);
                ret_T = focas.PMC_ReadTimer(152, out msT152);
                ret_T = focas.PMC_ReadTimer(154, out msT154);

                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K31.0
                pic_K31_0.Image = K31.BIT_0() == true ? imgCheckMark : null;
                // K15.5
                pic_K15_5.Image = K15.BIT_5() == true ? imgCheckMark : null;
                // K16.5
                pic_K16_5.Image = K16.BIT_5() == true ? imgCheckMark : null;
                // K4.1
                pic_K4_1_0.Image = K4.BIT_1() == true ? null : imgCheckMark;
                pic_K4_1_1.Image = K4.BIT_1() == true ? imgCheckMark : null;
                // K31.1
                pic_K31_1.Image = K31.BIT_1() == true ? imgCheckMark : null;
                // K15.6
                pic_K15_6.Image = K15.BIT_6() == true ? imgCheckMark : null;
                // K16.3
                pic_K16_3.Image = K16.BIT_3() == true ? null : imgCheckMark;
                // K4.0
                pic_K4_0_0.Image = K4.BIT_0() == true ? null : imgCheckMark;
                pic_K4_0_1.Image = K4.BIT_0() == true ? imgCheckMark : null;
                // K0.6
                pic_K0_6.Image = K0.BIT_6() == true ? null : imgCheckMark;
                // K4.2
                pic_K4_2.Image = K4.BIT_2() == true ? imgCheckMark : null;

            }

            if (ret_T == SUCCESS)
            {
                ucBtn_T34.DisplayText = (msT34 / 1000.0).ToString("F1");
                ucBtn_T36.DisplayText = (msT36 / 1000.0).ToString("F1");
                ucBtn_T152.DisplayText = (msT152 / 1000.0).ToString("F1");
                ucBtn_T154.DisplayText = (msT154 / 1000.0).ToString("F1");
            }
        }
        #endregion

        #region 油壓修整座
        private void OilDressDeviceData()
        {
            int ret_K = -1, ret_T = -1;
            byte K12 = 0, K14 = 0;
            int msT82 = 0, msT106 = 0, msT108 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 12, out K12);
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 14, out K14);

                ret_T = focas.PMC_ReadTimer(82, out msT82);
                ret_T = focas.PMC_ReadTimer(106, out msT106);
                ret_T = focas.PMC_ReadTimer(108, out msT108);
                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K14.6
                pic_K14_6.Image = K14.BIT_6() == true ? imgCheckMark : null;
                // K12.5
                pic_K12_5.Image = K12.BIT_5() == true ? null : imgCheckMark;
            }
            if (ret_T == SUCCESS)
            {
                ucBtn_T82.DisplayText = (msT82 / 1000.0).ToString("F1");
                ucBtn_T106.DisplayText = (msT106 / 1000.0).ToString("F1");
                ucBtn_T108.DisplayText = (msT108 / 1000.0).ToString("F1");
            }
        }
        #endregion

        #region 修整滾輪
        private void RollerData()
        {
            int ret_K = -1, ret_T = -1, ret_D = -1;
            byte K8 = 0;
            ushort D50 = 0;
            int msT52 = 0, msT74 = 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 8, out K8);

                ret_T = focas.PMC_ReadTimer(52, out msT52);
                ret_T = focas.PMC_ReadTimer(74, out msT74);

                ret_D = focas.PMC_ReadWord(PmcAddrType.D, 50, out D50);
                bFinish = true;
            }));
            NeedWait(ref bFinish);

            if (ret_D == SUCCESS)
            {
                if (D50 < cb_D50Select.Items.Count)
                {
                    cb_D50Select.SelectedIndex = D50;
                }
            }

            if (ret_K == SUCCESS)
            {
                // K8.3
                pic_K8_3.Image = K8.BIT_3() == true ? imgCheckMark : null;
                // K8.4
                pic_K8_4_0.Image = K8.BIT_4() == true ? null : imgCheckMark;
                pic_K8_4_1.Image = K8.BIT_4() == true ? imgCheckMark : null;
                // K8.5
                pic_K8_5_0.Image = K8.BIT_5() == true ? null : imgCheckMark;
                pic_K8_5_1.Image = K8.BIT_5() == true ? imgCheckMark : null;
            }

            if (ret_T == SUCCESS)
            {
                ucBtn_T52.DisplayText = (msT52 / 1000.0).ToString("F1");
                ucBtn_T74.DisplayText = (msT74 / 1000.0).ToString("F1");
            }
        }
        #endregion

        #region 氣密檢知
        private void AirTightData()
        {
            int ret_K = -1, ret_T = -1;
            byte K0 = 0;
            int msT4= 0;
            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 0, out K0);

                ret_T = focas.PMC_ReadTimer(4, out msT4);
                bFinish = true;
            }));
            NeedWait(ref bFinish);

           
            if (ret_K == SUCCESS)
            {
                // K0.0
                pic_K0_0.Image = K0.BIT_0() == true ? imgCheckMark : null;
            }

            if (ret_T == SUCCESS)
            {
                ucBtn_T4.DisplayText = (msT4 / 1000.0).ToString("F1");
            }
        }
        #endregion

        #region 氣壓
        private void AirPressureData()
        {
            int ret_K = -1;
            byte K10 = 0;

            bool bFinish = false;
            Actions.Enqueue(new Action(() =>
            {
                ret_K = focas.PMC_ReadByte(PmcAddrType.K, 10, out K10);


                bFinish = true;
            }));
            NeedWait(ref bFinish);
            if (ret_K == SUCCESS)
            {
                // K10.3
                pic_K10_3.Image = K10.BIT_3() == true ? imgCheckMark : null;
            }


        }
        #endregion

        #region 光學尺
        private void LinearScaleData(string axisName)
        {
            int ret_N = -1;

            byte N1815 = 0;
            short N2024 = 0, N2084 = 0, N2085 = 0, N2185 = 0;

            bool bFinish = false;
            short axis = 0;
            if (axisName.ToUpper() == "X")
            {
                axis = 1;
            }
            if (axisName.ToUpper() == "Z")
            {
                axis = 2;
            }
            Actions.Enqueue(new Action(() =>
            {
                ret_N = focas.Param_ReadByte(1815, axis, out N1815);
                ret_N = focas.Param_ReadWord(2024, axis, out N2024);
                ret_N = focas.Param_ReadWord(2084, axis, out N2084);
                ret_N = focas.Param_ReadWord(2085, axis, out N2085);
                ret_N = focas.Param_ReadWord(2185, axis, out N2185);

                bFinish = true;
            }));
            NeedWait(ref bFinish);

            if (ret_N == SUCCESS)
            {
                if (N1815 < cb_N1815Select.Items.Count)
                {
                    cb_N1815Select.SelectedIndex = N1815;
                }
                ucBtn_N2024.DisplayText = N2024.ToString();
                ucBtn_N2084.DisplayText = N2084.ToString();
                ucBtn_N2085.DisplayText = N2085.ToString();
                ucBtn_N2185.DisplayText = N2185.ToString();
            }

        }
        private void pic_LinearScaleAxis_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            string axisName = "0";
            if (pic != null && pic.Tag != null)
            {
                if (pic.Tag.ToString() == "X")
                {
                    pic.Image = Properties.Resources.BtnRight;
                    axisName = "Z";
                }
                if (pic.Tag.ToString() == "Z")
                {
                    pic.Image = Properties.Resources.BtnLeft;
                    axisName = "X";
                }
                pic.Tag = axisName;
                LinearScaleData(pic.Tag.ToString());
            }
        }
        #endregion
    }
}

