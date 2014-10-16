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
        }

        public class CoreBuilderTypeCase
        {
            public static string Hollow = "Hollow";
            public static string Shaft = "Shaft";
        }

        public class CoreTypeCase
        {
            public static string R = "R";
            public static string Z = "Z";
        }

        public class ItemTypeCase
        {
            public static string Accessory = "Accessory";
            public static string Adhesive = "Adhesive";
            public static string Bar = "Bar";
            public static string Blanket = "Blanket";
            public static string Bearing = "Bearing";
            public static string RollBlanket = "RollBlanket";
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

        public class OrderTypeCase
        {
            public static int TrialOrder = 0;
            public static int SampleOrder = 1;
            public static int PartDeliveryOrder = 2;
            public static int SalesOrder = 3;
        }
    }
}
