using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Resources;
using OIG_Option;
using System.IO;
using System.Xml.Linq;

namespace OIG_Option
{
    public static class Units
    {
        public static Fo_Main Fo_Main;

        public static MacroLimitList MacroLimitListmm;
        public static MacroLimitList MacroLimitList;
        public static MacroLimitListName MacroLimitListName;
        //程式庫
        public static TProgram_DB ProgramDB = new TProgram_DB();
        //預設工序名稱
        public static TProcessNameFile DefProcessNames;
        public static XDocument xmlDefaultProcessLang = new XDocument();//DefaultProcessLang.xml
        public static List<TProcess> ProcessList = new List<TProcess>();//DefaultProcessLang.xml + DefaultProcess.xml

        //預設引數名稱
        public static TArgValueNameFile ArgValueNames = new TArgValueNameFile();

        public static String langfile;
        public static String LangCode;

        public static TAlarmNameFile alarmfile;

        public static String DisplayFmt = "0.0000";

        public static double BalanceAngle1 = 0;
        public static double BalanceAngle2 = 120;
        public static double BalanceAngle3 = 240;
        public static string BalanceVersion = "";
        public static string BalanceStatus = "";
        public static int BalanceError = 0;
        public static int BalanceMode = 0;
        public static double BalanceLock1 = 0;
        public static double BalanceLock2 = 0;
        public static int BalanceDO = 0;
        public static double BalanceVibration_um = 0;
        public static double BalanceVibration_G = 0;
        public static double BalanceRPM = 0;
        public static int BalanceStep = 0;
        public static double BalanceTrialAngle = 0;
        public static double BalanceVibration1_um = 0;
        public static double BalanceVibration2_um = 0;
        public static double BalanceVibration3_um = 0;

        public static int BA_Version = 0x1010;
        public static int BA_Status = 0x1020;
        public static int BA_Error = 0x1030;
        public static int BA_ModeStatus = 0x1040;
        public static int BA_ShockLevel = 0x1110;
        public static int BA_DOStatus = 0x1120;
        public static int BA_Vibration_um = 0x1210;
        public static int BA_Vibration_G = 0x1220;
        public static int BA_RPM = 0x1230;
        public static int BA_BalancingStep = 0x1240;
        public static int BA_InitialAngle = 0x1250;
        public static int BA_TrialAngle = 0x1260;
        public static int BA_Angle = 0x1270;
        public static int BA_NarrowBandVibration = 0x1280;
        public static int BA_InitialRun = 0x1310;
        public static int BA_TrialRun = 0x1320;
        public static int BA_ResidualRun = 0x1330;
        public static int BA_ParameterRun = 0x1340;
        public static int BA_Abort = 0x1350;
    }
}

public class Limit
{
    public int No;
    public double Max;
    public double Min;
    public String Unit;

    public Limit() { }

    public Limit(int no, double min, double max, string unit)
    {
        No = no;
        Max = max;
        Min = min;
        Unit = unit;
    }

    internal Limit DeepCopy()
    {
        return new Limit
        {
            No = this.No,
            Max = this.Max,
            Min = this.Min,
            Unit = this.Unit
        };
    }
}
public class LimitName
{
    public int No;
    public String Name;

    public LimitName() { }
    public LimitName(int no, string name)
    {
        No = no;
        Name = name;
    }

}

public class MacroLimitList
{
    public Dictionary<int, Limit> Items = new Dictionary<int, Limit>();
    public String FileName;
    bool xmltrue;
    public MacroLimitList() { }

    public MacroLimitList(string filename)
    {
        LoadFromFile(filename);
    }
    public MacroLimitList(string filename, bool xml)
    {
        if (xml)//判斷使用xml或txt
            LoadFromxmlFile(filename + ".xml");
        else
            LoadFromFile(filename + ".txt");
        xmltrue = xml;
    }
    public void LoadFromFile(string filename)
    {
        FileName = filename;
        if (!File.Exists(filename)) return;

        string[] lines = File.ReadAllLines(filename);
        foreach (string line in lines)
        {
            if (line == "") continue;

            string[] csv = line.Split(',');
            if (csv.Length == 4)
            {
                int.TryParse(csv[0], out int no);
                double.TryParse(csv[1], out double min);
                double.TryParse(csv[2], out double max);
                string unit = csv[3];
                if (!Items.ContainsKey(no))
                {
                    Items.Add(no, new Limit(no, min, max, unit));
                }
            }
        }
    }
    public void LoadFromxmlFile(string filename)
    {
        FileName = filename;
        if (!File.Exists(filename)) return;

        XDocument xmlDefaultProcess = new XDocument();//DefaultProcess.xml
        xmlDefaultProcess = XDocument.Load(filename);//讀取XML檔案

        var processNode = xmlDefaultProcess.Descendants("Macro");
        //從找到的工序中, 取出(所有PCode 標籤).(有Text 標籤的)
        foreach (var process in processNode)
        {
            int.TryParse(process.Attribute("No")?.Value, out int no); //工序ID 
            double.TryParse(process.Attribute("Min")?.Value, out double min); //最小值Min
            double.TryParse(process.Attribute("Max")?.Value, out double max); //最大值Max
            string unit = process.Attribute("Unit")?.Value;
            if (!Items.ContainsKey(no))
            {
                Items.Add(no, new Limit(no, min, max, unit));
            }
        }

    }



    public void SaveToFile(string filename)
    {
        FileName = filename;
        Save();
    }

    public void Save()
    {
        if (xmltrue)
        {

            var doc = XDocument.Load(FileName);

            // 假設所有 <Macro> 都在根節點底下
            var root = doc.Root;

            // 移除原本的所有 <Macro> 節點
            root.Elements("Macro").Remove();

            // 建立新的 <Macro> 節點
            foreach (var kv in Items)
            {
                var limit = kv.Value;//抓取對應key進行儲存

                var macro = new XElement("Macro",
                    new XAttribute("No", limit.No),
                    new XAttribute("Min", limit.Min),
                    new XAttribute("Max", limit.Max),
                    new XAttribute("Unit", limit.Unit)
                );

                root.Add(macro);
            }

            doc.Save(FileName);

        }
        else
        {
            List<String> list = new List<string>();
            foreach (int no in Items.Keys)
            {
                var data = Items[no];
                list.Add(no + "," + data.Min + "," + data.Max + "," + data.Unit);
            }
            File.WriteAllLines(FileName, list);
        }
    }

    public void GetMinMax(int number, out double min, out double max)
    {
        if (!Items.ContainsKey(number))
        {
            min = 0;
            max = 0;
            return;
        }
        Limit p = Items[number];
        min = p.Min;
        max = p.Max;
    }

    public bool CheckMacroMinMax(int number, ref double val, out Limit macro)
    {
        if (!Items.ContainsKey(number))
        {
            macro = null;
            return false;
        }

        Limit p = Items[number];
        macro = p;

        if (p.Min > val)
        {
            val = p.Min;
            return false;
        }
        if (p.Max < val)
        {
            val = p.Max;
            return false;
        }

        return true;
    }

    internal MacroLimitList DeepCopy()
    {
        // 创建一个新的 MacroLimitList 实例
        var newList = new MacroLimitList();

        // 对字典中的每个键值对进行深拷贝
        foreach (var kvp in Items)
        {
            newList.Items.Add(kvp.Key, kvp.Value.DeepCopy());
        }

        return newList;
    }
}

public class MacroLimitListName
{
    public Dictionary<int, LimitName> ItemsName = new Dictionary<int, LimitName>();
    public String FileName;
    bool langtrue;

    public MacroLimitListName() { }

    public MacroLimitListName(string filename)
    {
        LoadFromFile(filename);
    }
    public MacroLimitListName(string filename, bool lang)
    {
        if (lang)
        {
            filename += Units.LangCode + ".txt";
            LoadFromlangFile(filename);//抓語言檔
        }
        else
        {
            filename += "macro.txt";
            LoadFromFile(filename);//直接抓macro
        }
        langtrue = lang;//true(語言檔) false (macro.txt)
    }
    public void LoadFromlangFile(string filename)//從語言檔抓
    {
        if (!File.Exists(filename)) return;
        TIniFile tIniFile = new TIniFile(filename);
        FileName = filename;

        foreach (var kv in Units.MacroLimitListmm.Items)
        {
            int i = kv.Key;//抓取對應key進行讀取

            string name = tIniFile.ReadString("Macro", i.ToString(), "");
            ItemsName.Add(i, new LimitName(i, name));
        }
    }
    public void LoadFromFile(string filename)
    {
        FileName = filename;
        if (!File.Exists(filename)) return;

        string[] lines = File.ReadAllLines(filename);
        foreach (string line in lines)
        {
            if (line == "") continue;

            string[] csv = line.Split(',');
            if (csv.Length == 2)
            {
                int.TryParse(csv[0], out int no);
                string name = csv[1];
                if (!ItemsName.ContainsKey(no))
                {
                    ItemsName.Add(no, new LimitName(no, name));
                }
            }
        }
    }
    public void SaveName()
    {
        List<String> list = new List<string>();
        foreach (int no in ItemsName.Keys)
        {
            var data = ItemsName[no];
            list.Add(no + "," + data.Name);
        }
        File.WriteAllLines(FileName, list);
    }

    public void SavelangName()//從語言檔抓
    {
        // List<String> list = new List<string>();
        foreach (int no in ItemsName.Keys)
        {
            TIniFile tIniFile = new TIniFile(FileName);
            tIniFile.WriteString("Macro", no.ToString(), ItemsName[no].Name);//將名稱寫入對應位置
        }
    }


}