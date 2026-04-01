using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


public class Macros
{
    public List<MacroInfo> Items = new List<MacroInfo>();
    public int MinAddr;
    public int MaxAddr;

    public MacroInfo this[int i]
    {
        get { return Items[i]; }
    }

    public Macros()
    {
        CreateInfo();
    }

    public Macros(String ini_file)
    {
        CreateInfo();
        LoadFromIni(ini_file);
    }

    private void CreateInfo()
    {
        Items.Add(new MacroInfo("SelectShape", 0));//選擇形狀
        Items.Add(new MacroInfo("GwMinWidth", 0));//砂輪最小寬
        Items.Add(new MacroInfo("GwWidth", 0));//砂輪寬
        Items.Add(new MacroInfo("GwMinDiameter", 0));//砂輪最小外徑
        Items.Add(new MacroInfo("GwDiameter", 0));//砂輪外徑
        Items.Add(new MacroInfo("GwDressTimes", 0));//修整次數
        Items.Add(new MacroInfo("GwDressCount", 0));//修整計次
        Items.Add(new MacroInfo("GwToDressSetting", 0));//幾支修砂
        Items.Add(new MacroInfo("GwToDressCounter", 0));//修砂計數
        Items.Add(new MacroInfo("GwAirDress", 0));//空修次數

        Items.Add(new MacroInfo("DiamDressAmount", 0));//外徑修整量
        Items.Add(new MacroInfo("DiamDressDefAmount", 0));//外徑修整預留量
        Items.Add(new MacroInfo("DiamDressSpeed", 0));//外徑修整速度
        Items.Add(new MacroInfo("LeftDressAmount", 0));//左側修整量
        Items.Add(new MacroInfo("LeftDressDefAmount", 0));//左側修整預留量
        Items.Add(new MacroInfo("LeftDressSpeed", 0));//左側修整速度
        Items.Add(new MacroInfo("RightDressAmount", 0));//右側修整量
        Items.Add(new MacroInfo("RightDressDefAmount", 0));//右側修整預留量
        Items.Add(new MacroInfo("RightDressSpeed", 0));//右側修整速度

        Items.Add(new MacroInfo("ToAndBack", 0));//往復修整
        Items.Add(new MacroInfo("DiamDressStartDir", 0));//外徑修整起始方向

        Items.Add(new MacroInfo("LeftCut", 0));//左側超切
        Items.Add(new MacroInfo("RightCut", 0));//右側超切
        Items.Add(new MacroInfo("DiamRaised", 0));//外徑中凸量

        Items.Add(new MacroInfo("LeftCutLength", 0));//左側幅長
        Items.Add(new MacroInfo("LeftLength", 0));//左側逃離長
        Items.Add(new MacroInfo("LeftDeep", 0));//左側逃離量
        Items.Add(new MacroInfo("LeftRound", 0));//左側R角

        Items.Add(new MacroInfo("RightCutLength", 0));//右側幅長
        Items.Add(new MacroInfo("RightLength", 0));//右側逃離長
        Items.Add(new MacroInfo("RightDeep", 0));//右側逃離量
        Items.Add(new MacroInfo("RightRound", 0));//右側R角

        Items.Add(new MacroInfo("StrLength", 0));//錐度直線幅
        Items.Add(new MacroInfo("TaperLength", 0));//錐度幅
        Items.Add(new MacroInfo("TapAngle", 0));//錐度修整度
        Items.Add(new MacroInfo("TapLength", 0));//端面逃離長(斜頭)

        Items.Add(new MacroInfo("UiProg", 0));//畫面啟動程式

        Items.Add(new MacroInfo("MeasDir", 0));//端測方向
        Items.Add(new MacroInfo("MeasDist", 0));//端測距離
        Items.Add(new MacroInfo("MeasStat", 0));//端測狀態
        Items.Add(new MacroInfo("MeasMast", 0));//端測MASTER值
        Items.Add(new MacroInfo("MeasX", 0));//端測X軸位置
        Items.Add(new MacroInfo("MeasZ", 0));//端測Z軸位置

        Items.Add(new MacroInfo("IdCenter", 0));//內圓中心位置
        Items.Add(new MacroInfo("IdSafePos", 0));//內圓安全位置
        Items.Add(new MacroInfo("ProgNo", 0));//加工程式號

        Items.Add(new MacroInfo("ExecProcess", 0));//要執行的工序
        Items.Add(new MacroInfo("RedoProcess", 0));//要重修的工序        

        Items.Add(new MacroInfo("PmcZStart", 0));//手動研磨Z軸PMC起點
        Items.Add(new MacroInfo("PmcZEnd", 0));//手動研磨Z軸PMC終點
        Items.Add(new MacroInfo("PmcZSeed", 0));//手動研磨Z軸PMC橫移速度
        Items.Add(new MacroInfo("ManualMode", 0));//手動研磨切手輪判斷
        Items.Add(new MacroInfo("ManualRpm", 0));//手動研磨工件轉速
        Items.Add(new MacroInfo("RepeatGrinding", 0));//重修精磨

        Items.Add(new MacroInfo("OdXSafe", 0));//外徑X軸安全行程
        Items.Add(new MacroInfo("OdZSafe", 0));//外徑Z軸安全行程
        Items.Add(new MacroInfo("OdXSafeSpeed", 0));//外徑X軸安全行程速度
        Items.Add(new MacroInfo("OdZSafeSpeed", 0));//外徑Z軸安全行程速度
        Items.Add(new MacroInfo("IdXSafe", 0));//內徑X軸安全行程
        Items.Add(new MacroInfo("IdZsafe", 0));//內徑Z軸安全行程
        Items.Add(new MacroInfo("IdXSafeSpeed", 0));//內徑X軸安全行程速度
        Items.Add(new MacroInfo("IdZSafeSpeed", 0));//內徑Z軸安全行程速度

        Items.Add(new MacroInfo("IdSafeInAmount", 0));//內徑安全行程單次進給量
        Items.Add(new MacroInfo("CoolantOff", 0));//程式結束關切削液
        Items.Add(new MacroInfo("GwCover", 0));//砂輪重疊量
        Items.Add(new MacroInfo("GaugeLimit", 0));//量測誤差上限
        Items.Add(new MacroInfo("ProbeTolerance", 0));//端測容許誤差
        Items.Add(new MacroInfo("ProgRepeat", 0));//程式連動
        Items.Add(new MacroInfo("WorkPosition", 0));//工作主軸定位使用
        Items.Add(new MacroInfo("CPosAngle", 0));//C軸定位角度
        Items.Add(new MacroInfo("ManualSafe", 0));//手動安全行程
        Items.Add(new MacroInfo("ManualSafeSpeed", 0));//手動安全速度

        Items.Add(new MacroInfo("DpSpRpm", 0));
        Items.Add(new MacroInfo("CycDisplay", 0));
        Items.Add(new MacroInfo("TCode", 0));
        Items.Add(new MacroInfo("GrindType", 0));
    }

    public MacroInfo GetInfo(string key)
    {
        foreach (MacroInfo i in Items)
        {
            if (i.Key == key) return i;
        }
        return null;
    }

    public int GetAddress(string key)
    {
        foreach (MacroInfo i in Items)
        {
            if (i.Key == key) return i.Address;
        }
        return 0;
    }

    public double GetValue(string key)
    {
        foreach(MacroInfo i in Items)
        {
            if(i.Key == key) return i.Value;
        }
        return 0;
    }

    public double GetValue(int addr)
    {
        foreach (MacroInfo i in Items)
        {
            if (i.Address == addr) return i.Value;
        }
        return 0;
    }

    public void SetValue(string key, double value)
    {
        foreach (MacroInfo i in Items)
        {
            if (i.Key == key)
            {
                i.Value = value;
                return;
            }
        }
        return;
    }

    public void SetValue(int addr, double value)
    {
        foreach (MacroInfo i in Items)
        {
            if (i.Address == addr)
            {
                i.Value = value;
                return;
            }
        }
        return ;
    }

    public void LoadFromIni(String ini_file)
    {
        if (!File.Exists(ini_file)) return;

        TIniFile ini = new TIniFile(ini_file);
        MinAddr = ini.ReadInteger("Range", "Min", 500);
        MaxAddr = ini.ReadInteger("Range", "Max", 999);
        foreach (MacroInfo i in Items)
        {
            i.Address = ini.ReadInteger("Macro", i.Key, 0);
        }
    }


}

public class MacroInfo
{
    public String Key;
    public double Value;
    public int Address;

    public MacroInfo() { }

    public MacroInfo(String key, int addr)
    {
        Key = key;
        Address = addr;
    }
}