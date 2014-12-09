using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Constants
{
    public partial class Constant
    {
        #region Contact Tax Code
        public class TaxCode
        {
            public static string Code01 = "01"; // Penyerahan kepada selain pemungut PPN
            public static string Code02 = "02"; // Penyerahan kepada pemungut PPN bendaharawan pemerintah
            public static string Code03 = "03"; // Penyerahan kepada pemungut PPN lainnya (selain Bendaharawan Pemerintah)
            public static string Code04 = "04"; // Penyerahan yang menggunakan DPP Nilai Lain kepada selain pemungut PPN
            public static string Code05 = "05"; // Penyerahan Lainnya kepada selain Pemungut PPN
            public static string Code06 = "06";
            public static string Code07 = "07"; // Penyerahan yang PPN atau PPN dan PPn BM-nya tidak dipungut kepada selain pemungut PPN
            public static string Code08 = "08"; // Digunakan untuk penyerahan yang dibebaskan dari pengenaan PPN atau PPN dan PPn BM kepada selain Pemungut PPN
            public static string Code09 = "09"; // Digunakan untuk penyerahan Aktiva Pasal 16D yang PPN nya dipungut oleh PKP penjual yang melakukan penyerahan BKP
        }

        public class TaxValue
        {
            public static int Code01 = 10; // Penyerahan kepada selain pemungut PPN
            public static int Code02 = 10; // Penyerahan kepada pemungut PPN bendaharawan pemerintah
            public static int Code03 = 10; // Penyerahan kepada pemungut PPN lainnya (selain Bendaharawan Pemerintah)
            public static int Code04 = 1; // Penyerahan yang menggunakan DPP Nilai Lain kepada selain pemungut PPN
            public static int Code05 = 10; // Penyerahan Lainnya kepada selain Pemungut PPN
            public static int Code06 = 10;
            public static int Code07 = 0; // Penyerahan yang PPN atau PPN dan PPn BM-nya tidak dipungut kepada selain pemungut PPN
            public static int Code08 = 0; // Digunakan untuk penyerahan yang dibebaskan dari pengenaan PPN atau PPN dan PPn BM kepada selain Pemungut PPN
            public static int Code09 = 0; // Digunakan untuk penyerahan Aktiva Pasal 16D yang PPN nya dipungut oleh PKP penjual yang melakukan penyerahan BKP
        }
        #endregion

        #region Order Type
        public class OrderTypeCase
        {
            public static int TrialOrder = 0;
            public static int SampleOrder = 1;
            public static int PartDeliveryOrder = 2;
            public static int SalesOrder = 3;
            public static int SalesQuotation = 4;
            public static int Consignment = 5;
        }
        #endregion

        #region Item
        public class ItemCase
        {
            public static int Ready = 1;
            public static int PendingReceival = 2;
            public static int PendingDelivery = 3;
            public static int Virtual = 4;
        }
        #endregion

        #region Item Type
        public class ItemTypeCase
        {
            public static string Accessory = "Accessory";
            public static string AdhesiveBlanket = "AdhesiveBlanket";
            public static string AdhesiveRoller = "AdhesiveRoller";
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
        #endregion

        #region Roller Type
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
        #endregion
    }
}
