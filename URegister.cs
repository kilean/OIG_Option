using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Win32;//for Registry
using System.Net.NetworkInformation; // for MAC
namespace Palmary.Register
{
    class URegister
    {
        private int LastError = 0;
        //private String CurrentAddress;

        public List<String> AddressList = new List<string>();
        public List<String> SerialList = new List<string>();
        public String Key1 = "C9664932D156";
        public String Key2 = "D1668938D126";

        public URegister()
        { 
        }

        public String XOR(String Src, String Key)
        {
            if (Src == "") return "";
            if (Key == "") return "";

            Byte[] KeyData = new Byte[6];
            Byte[] SrcData = new Byte[6];
            StrToByte(Src, SrcData);
            StrToByte(Key, KeyData);

            String Result = "";
            for (int i = 0; i < 6; i++)
            {
                Byte code = (Byte)(SrcData[i] ^ KeyData[i]);
                Result += code.ToString("X2");
            }
            return Result;
        }

        public String NXOR(String Src, String Key)
        {
            if (Src == "") return "";
            if (Key == "") return "";

            Byte[] KeyData = new Byte[6];
            Byte[] SrcData = new Byte[6];
            StrToByte(Src, SrcData);
            StrToByte(Key, KeyData);

            String Result = "";
            for (int i = 0; i < 6; i++)
            {
                Byte code = (Byte)((~SrcData[i]) ^ KeyData[i]);
                Result += code.ToString("X2");
            }
            return Result;
        }

        public void StrToByte(String Str, Byte[] buf)
        {
            for (int i = 0; i < 6; i++)
            {
                buf[i] = Byte.Parse(Str.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
        }

        public bool Check(String Keygen, String Mac)
        {
            if (Keygen == "") return false;
            if (Mac == "") return false;

            Byte[] KeyData = new Byte[6];
            Byte[] MacData = new Byte[6];
            StrToByte(Keygen, KeyData);
            StrToByte(Mac, MacData);

            for (int i = 0; i < 6; i++)
            {
                if ((KeyData[i] & MacData[i]) != 0) return false;
            }

            return true;
        }

        public void RegisterKeygen(String Keygen)
        {
            RegistryKey Reg = null;
            try
            {
                Reg = Registry.CurrentUser.OpenSubKey("Software\\Palmary", true);
                if (Reg == null) Reg = Registry.CurrentUser.CreateSubKey("Software\\Palmary");
                if (Reg != null)
                {
                    Reg.SetValue(Keygen, 1);
                }
            }
            catch (Exception)
            {
                LastError = -1;
            }
        }

        public bool CheckRegisterKeygen()
        {
            RegistryKey Reg = null;
            try
            {
                Reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Palmary", true);
                if (Reg == null) return false;

                String[] names = Reg.GetValueNames();
                foreach (String s in names)
                {
                    String Keygen = "";
                    if (s == "Register")
                    {
                        Keygen = Reg.GetValue("Register").ToString();
                    }
                    else
                    {
                        Keygen = s;
                    }

                    if (Keygen.Length != 12) continue;

                    Byte[] KeyData = new Byte[6];
                    Byte[] MacData = new Byte[6];
                    StrToByte(Keygen, KeyData);
                    ScanAddressList();
                    for (int i = 0; i < AddressList.Count; i++)
                    {
                        String Mac = AddressList[i];
                        StrToByte(Mac, MacData);
                        bool bMatch = true;
                        for (int j = 0; j < 6; j++)
                        {
                            int a = KeyData[j];
                            int b = MacData[j];
                            if ((KeyData[j] ^ MacData[j]) != 255)
                            {
                                bMatch = false;
                                break;
                            }
                        }
                        if (bMatch)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                LastError = -1;
            }

            return false;
        }


        public int GetLastError()
        {
            return LastError;
        }
        
        public void ScanAddressList()
        {
            //uint Len;
            //Byte[] Buf;

            NetworkInterface[] info = NetworkInterface.GetAllNetworkInterfaces();

            AddressList.Clear();
            SerialList.Clear();
           // CurrentAddress = "";

            //List<string> MacList = new List<string>();
            foreach (var data in info)
            {

                // 因為電腦中可能有很多的網卡(包含虛擬的網卡)，
                // 我只需要 Ethernet 網卡的 MAC
                if (data.NetworkInterfaceType == NetworkInterfaceType.Ethernet || data.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    String Mac = data.GetPhysicalAddress().ToString();
                    //MacList.Add(Mac);
                    Console.WriteLine(Mac);

                    //String PhysicalAddress = "";
                    //for (int i = 0; i < 6; i++)
                    //{
                        //PhysicalAddress += IntToHex(pipai.Address[i], 2);
                    //}
                    //AddressList.Add(PhysicalAddress);
                    //SerialList.Add(XOR(PhysicalAddress, Key1));
                    AddressList.Add(Mac);
                }
            }

            /*
            if (GetAdaptersInfo(NULL, &Len) != ERROR_BUFFER_OVERFLOW)
            {
                LastError = -2;
            }
            else
            {
                Buf = new Byte[Len];
                if (ERROR_SUCCESS == GetAdaptersInfo((IP_ADAPTER_INFO*)Buf, &Len))
                {
                    pipai = (IP_ADAPTER_INFO*)Buf;
                    while (pipai != NULL)
                    {
                        if ((String(pipai.Description) != "VMware Virtual Ethernet Adapter for VMnet1") &&
                            (String(pipai.Description) != "VMware Virtual Ethernet Adapter for VMnet8"))
                        {
                            String PhysicalAddress = "";
                            for (int i = 0; i < 6; i++)
                            {
                                PhysicalAddress += IntToHex(pipai.Address[i], 2);
                            }
                            AddressList.Add(PhysicalAddress);
                            SerialList.Add(XOR(PhysicalAddress, Key1));
                        }
                        pipai = pipai.Next;
                    }
                    if (AddressList.Count > 0) CurrentAddress = AddressList.Strings[0];
                }
                delete[] Buf;
            }
            */ 
        }



    }
}
