using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

class FanucMacroMemo
{
    public List<JTU_Encode> EncodeList = new List<JTU_Encode>();
    public List<FTU_Encode> EncodeList2 = new List<FTU_Encode>();

    public FanucMacroMemo()
    {
    }

    public void Load_JTU_File(String filename)
    {
        String[] lines = File.ReadAllLines(filename);//Application.StartupPath + "\\JIS_TO_UNICODE.txt"
        foreach (String line in lines)
        {
            String[] csv = line.Split(',');
            if (csv.Length == 2)
            {
                EncodeList.Add(new JTU_Encode(csv[0], csv[1]));
            }
        }
    }

    public void Load_FTU_File(String filename)
    {
        String[] lines = File.ReadAllLines(filename);//Application.StartupPath + "\\FanucCode_To_Unicode.txt"
        foreach (String line in lines)
        {
            String[] csv = line.Split(',');
            if (csv.Length == 2)
            {
                EncodeList2.Add(new FTU_Encode(csv[1], csv[0]));
            }
        }
    }


    public MacroData GetMacroData(String line)
    {

        int l_pos = line.IndexOf("(");
        int r_pos = line.IndexOf(")");
        if (l_pos >= 0 && r_pos >= 0)
        {
            line = line.Substring(0, line.Length - (r_pos - l_pos + 1));
        }

        //例外處理
        int pos0 = line.IndexOf("G65");
        if (pos0 < 0) return null;

        int title_pos1 = line.IndexOf("H90");
        int title_pos2 = line.IndexOf("H92");

        if (title_pos1 == 0 && title_pos2 == 0) return null;

        int len = 10;

        String Macro = "Macro";
        String Str = "";
        int pos1 = line.IndexOf("P");
        int pos2 = line.IndexOf("Q");
        if (pos1 >= 0 && pos2 >= 0)
        {
            Macro = line.Substring(pos1 + 1, pos2 - pos1 - 1);
            if (title_pos1 >= 0 || title_pos2 >= 0)
            {
                Str = Macro + line.Substring(pos2 + 1, 6);
                len = 12;
            }
            else
            {
                Str = line.Substring(pos2 + 1, 6);
            }
        }
        int pos3 = line.IndexOf("R");
        if (pos3 >= 0) Str += line.Substring(pos3 + 1, 6);
        int pos4 = line.IndexOf("I");
        if (pos4 >= 0) Str += line.Substring(pos4 + 1, 6);
        int pos5 = line.IndexOf("J");
        if (pos5 >= 0) Str += line.Substring(pos5 + 1, 6);
        int pos6 = line.IndexOf("K");
        if (pos6 >= 0) Str += line.Substring(pos6 + 1, 6);

        //例外處理
        if (Str.Length != len * 3) return null;

        String Data = "";
        int index = 0;


        //最多10個[半形字] (Title 12個[半形字])
        while (index < len)
        {
            int A = Int16.Parse(Str.Substring(index * 3, 3));
            String DataA = A.ToString("X2");
            //全型字
            if (A < 0x20)
            {
                //例外處理
                if (index == 9) break;

                int B = Int16.Parse(Str.Substring((index + 1) * 3, 3));
                String DataB = B.ToString("X2");
                Data += DecodeStr2(DataA + DataB);
                index += 2;
            }
            //半形字
            else
            {
                Data += DecodeStr2(DataA);
                index++;
            }
        }

        if (title_pos1 >= 0 || title_pos2 >= 0)
        {
            return null;
        }
        
        return new MacroData(int.Parse(Macro), 0, Data);
    }


    public String FindEncode(String str)
    {
        for (int i = 0; i < EncodeList.Count; i++)
        {
            if (str == EncodeList[i].Jis) return EncodeList[i].Unicode;
        }
        return "";
    }

    public String FindEncode2(String str)
    {
        for (int i = 0; i < EncodeList2.Count; i++)
        {
            if (str == EncodeList2[i].FanucCode) return EncodeList2[i].Unicode;
        }
        return "";
    }

    public String StrToUnicode(String data)
    {
        if (data == "") return "";
        char ch = (char)Int16.Parse(data, System.Globalization.NumberStyles.HexNumber);
        return ch.ToString();
    }

    public String DecodeStr2(String data)
    {
        String U_Str = FindEncode2(data);
        return StrToUnicode(U_Str);
    }
}

public class MacroData
{
    public MacroData()
    { 
    }

    public MacroData(int no, double value)
    {
        No = no;
        Value = value;
    }

    public MacroData(int no, double value, String memo)
    {
        No = no;
        Value = value;
        Memo = memo;
    }
    public int No;
    public double Value;
    public String Memo;
}

public class FTU_Encode
{
    public String FanucCode = "";
    public String Unicode = "";

    public FTU_Encode()
    {
    }
    public FTU_Encode(String code, String unicode)
    {
        FanucCode = code;
        Unicode = unicode;
    }
}


public class JTU_Encode
{
    public String Jis = "";
    public String Unicode = "";

    public JTU_Encode()
    {
    }
    public JTU_Encode(String jis, String unicode)
    {
        Jis = jis;
        Unicode = unicode;
    }
}