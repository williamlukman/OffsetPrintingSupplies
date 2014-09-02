using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Constants
{
    public partial class Constant
    {
        public class MaterialCase
        {
            public static int New = 1;
            public static int Used = 2;
        }

        public class RepairRequestCase
        {
            public static int BearingSeat = 1;
            public static int CentreDrill = 2;
        }

        public class RecoveryOrderDetailProcessCase
        {
            public static int Disassemble = 1;
            public static int StripAndGlue = 2;
            public static int Wrap = 3;
            public static int Vulcanize = 4;
            public static int FaceOff = 5;
            public static int ConventionalGrind = 6;
            public static int CWCGrind = 7;
            public static int PolishAndQC = 8;
            public static int Package = 9;
        }

        public class CroppingType
        {
            public static string Special = "Special";
            public static string Normal = "Normal";
        }
    }
}
