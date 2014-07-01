using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public class Constant
    {
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

        public class RepairRequestCase
        {
            public static int BearingSeat = 1;
            public static int CentreDrill = 2;
        }

        public class ItemTypeCase
        {
            public static string Accessories = "Accessories";
            public static string Bearing = "Bearing";
            public static string Blanket = "Blanket";
            public static string Chemical = "Chemical";
            public static string Compound = "Compound";
            public static string Consumable = "Consumable";
            public static string Core = "Core";
            public static string Glue = "Glue";
            public static string Underpacking = "Underpacking";
            public static string Roller = "Roller";
        }

        public class RollerTypeCase
        {
            public static string Damp = "Damp";
            public static string DampFormDQ = "Damp Form DQ";
            public static string FoundDT = "Found DT";
            public static string InkFormX = "Ink Form X";
            public static string InkDistD = "Ink Dist D";
            public static string InkDistE = "Ink Dist E";
            public static string InkDistH = "Ink Dist H";
            public static string InkDistHQ = "Ink Dist HQ";
            public static string InkDistM = "Ink Dist M";
            public static string InkDuctB = "Ink Duct B";
            public static string InkFormW = "Ink Form W";
            public static string InkFormY = "Ink Form Y";
        }
    }
}
