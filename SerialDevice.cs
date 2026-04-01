using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OIG_Option
{
    public class SerialDevice
    {
        public int Slave = 1;
        public double Rate = 1;
        public double ShowRate = 1;
        public int MaxRpm = 2000;
        public int MinRpm = 0;
        public double Unit = 0.1;

        public double CmdSpeed;
        public double OutSpeed;
        public double OutCurrent = 0;
    }
}
