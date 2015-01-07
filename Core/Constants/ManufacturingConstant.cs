using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Constants
{
    public partial class Constant
    {
        public class ApplicationCase
        {
            public static string Sheetfed = "Sheetfed";
            public static string Web = "Web";
            public static string Both = "Both";
        }

        public class MaterialCase
        {
            public static int New = 1;
            public static int Used = 2;
        }

        public class CoreTypeCase
        {
            public static string R = "R";
            public static string Z = "Z";
        }

        public class CoreBuilderTypeCase
        {
            public static string Hollow = "Hollow";
            public static string Shaft = "Shaft";
            public static string None = "None";
        }

        public class CroppingType
        {
            public static string Special = "Special";
            public static string Normal = "Normal";
        }
        public class RepairRequestCase
        {
            public static int BearingSeat = 1;
            public static int CentreDrill = 2;
            public static int None = 3;
            public static int Both = 4;
        }
    }
}
