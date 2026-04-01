using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OIG_Option
{
    //維護手冊中的異常表
    public class AlarmFile
    {
        //異常表
        public List<Alarm> Items = new List<Alarm>();

        //建構子
        public AlarmFile()
        { 
        }

        //建構子
        public AlarmFile(String FileName)
        {
            //開檔
            Open(FileName);
        }

        //開檔
        public void Open(String FileName)
        {
            if (!File.Exists(FileName)) return;

            String[] lines = File.ReadAllLines(FileName);
            foreach (string s in lines)
            {
                string[] csv = s.Split(',');
                if (csv.Length < 3) continue;

                Items.Add(new Alarm(csv[0], csv[1], csv[2]));
            }
        }

        //異常碼查詢
        public Alarm FindCode(String code)
        {
            foreach (Alarm a in Items)
            {
                if (a.Code == code) return a;
            }
            return null;
        }
    }

    public class Alarm
    {
        //異常碼
        public String Code = "";
        //說明
        public String Msg = "";
        //處理方式
        public String TroubleShooting = "";
        //時間
        public DateTime Time;
        //軸
        public int Axis;

        //建構子
        public Alarm() { }
        public Alarm(String code, String msg, String ts)
        {
            Code = code;
            Msg = msg;
            TroubleShooting = ts;
        }

        public Alarm(String code, String msg, String ts, DateTime dt)
        {
            Code = code;
            Msg = msg;
            TroubleShooting = ts;
            Time = dt;
        }
    }

    public class GwParam
    {
        public  int Address;
        public double Value;

        public GwParam() { }
        public GwParam(int addr, double val)
        {
            Address = addr;
            Value = val;
        }
    }

    //功能開啟時與關閉時要送PULSE的M暫存器可能會不同
    public class PB_Status
    {
        public uint OnAddr;//要將功能開啟時，需送Pulse的M Device
        public uint OffAddr;//要將功能關閉時，需送Pulse的M Device
        public uint TmpAddr;//MouseDown 會先送ON的訊號，TmpAddr會暫存被ON的Device，MouseUp 時再將該Device OFF

        public bool IsOn = false;

        //建構子
        public PB_Status()
        {
        }

        //建構子
        public PB_Status(uint on, uint off)
        {
            this.OnAddr = on;
            this.OffAddr = off;
        }

        //建構子
        public PB_Status(int on, int off)
        {
            this.OnAddr = (uint)on;
            this.OffAddr = (uint)off;
        }
    }


    #region 列舉
    public enum M_Index
    {
        M450_OilInjector = 450,
        M451_GWRun,
        M452,
        M453_ChuckOpen,
        M454_ChuckClose,
        M455_ProbePush,
        M456_ProbeBack,
        M457_GaugePush,
        M458_GaugeBack,
        M459_Balance,
        M460_SpCW,
        M461,
        M462_SpCCW,
        M463_SpPos,
        M464_DoorOpen,
        M465_DoorClose,
        M466_OilMist,
        M467,
        M468_Light,
        M469_SvLimit,
        M470_TailFront,
        M471_TailBack,
        M472_Coolt,
        M473_Blow,
        M474_Safety,
        M475,
        M476,
        M477,
        M478,
        M479,
        M480,
        M481_FeedHold,
        M482,
        M483,
        M484,
        M485_XHome,
        M486_ZHome,
        M487,
        M488,
        M489,
        M490_JogXP,
        M491_JogXN,
        M492_JogZP,
        M493_JogZN,
        M494_Jog3P,
        M495_Jog3N,
        M496_Jog4P,
        M497_Jog4N,
        M498_Jog5P,
        M499_Jog5N,
        M1060_SBK = 1060,
        M1061_Cycle_Start,
        M1062_NC_Pause,
        M1063_Sys_Stop,
        M1064_Sys_Reset,
        M1065_DRY_RUN,
        M1066_M01,
        M1067_BDT,
        M1068_Lock_Axes,
        M1069_Lock_Z,
        M1070_Release_Limit,
        M1071_MST_Lock,
        M1072_DMCNET_Success,
        M1073,
        M1074_Macro_Init,
        M1075_Macro_Start,
        M1076_Sys_Reset,
        M2114_EMG = 2114,
        M2115_SVO
    }

    public enum AlarmCode
    {
        A000 = 0,    //氣壓源異常發生
        A001,    //砂輪異常發生
        A002,    //夾頭操作異常發生
        A003,    //注油器液位低異常
        A004,    //注油器壓力異常
        A005,    //端測動作異常
        A006,    //自動門動作異常
        A007,    //動平衡異常發生
        A008,    //動平衡操作異常
        A009,    //油霧回收機過載異常
        A010,    //頂針操作異常發生
        A011,    //油壓馬達過載異常
        A012,    //油壓壓力低異常
        A013,    //砂輪軸潤滑過載異常
        A014,    //砂輪軸潤滑壓力低異常
        A015,    //砂輪軸潤滑冷卻異常
        A016,    //切削泵浦過載異常
        A017,    //水冷機異常
        A018,    //鐵屑分離機過載異常
        A019,    //抽水馬達過載異常
        A020,    //油冷機異警
        A021,    //油水分離機過載異常
        A022,    //EMG系統急停異常
        A030 = 30,    //X軸MLC操作異常
        A031    //Z軸MLC操作異常
    }

    #endregion
}
