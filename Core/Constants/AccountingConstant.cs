using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Constants
{
    public partial class Constant
    {
        #region GeneralLedger
        public class GeneralLedgerStatus
        {
            public static int Debit = 1;
            public static int Credit = 2;
        }

        public class GeneralLedgerSource
        {
            public static string CashBankAdjustment = "CashBankAdjustment";
            public static string CashBankMutation = "CashBankMutation";
            //public static string CashSalesInvoice = "CashSalesInvoice";
            //public static string CustomPurchaseInvoice = "CustomPurchaseInvoice";
            public static string Memorial = "Memorial";
            public static string PaymentRequest = "PaymentRequest";
            public static string PaymentVoucher = "PaymentVoucher";
            public static string PurchaseDownPayment = "PurchaseDownPayment";
            public static string PurchaseDownPaymentAllocation = "PurchaseDownPaymentAllocation";
            public static string PurchaseAllowance = "PurchaseAllowance";
            public static string PurchaseReceival = "PurchaseReceival";
            public static string PurchaseInvoice = "PurchaseInvoice";
            public static string StockAdjustment = "StockAdjustment";
            public static string ReceiptVoucher = "ReceiptVoucher";
            //public static string RetailSalesInvoice = "RetailSalesInvoice";
            public static string DeliveryOrder = "DeliveryOrder";
            public static string SalesDownPayment = "SalesDownPayment";
            public static string SalesDownPaymentAllocation = "SalesDownPaymentAllocation";
            public static string SalesAllowance = "SalesAllowance";
            public static string SalesInvoice = "SalesInvoice";
            public static string RecoveryOrderDetail = "RecoveryOrderDetail";
            public static string RecoveryAccessoryDetail = "RecoveryAccessoryDetail";
            public static string BlanketOrderDetail = "BlanketOrderDetail";
            public static string BlendingWorkOrder = "BlendingWorkOrder";
        }
        #endregion

        #region Account
        public class AccountGroup
        {
            public static int Asset = 1;
            public static int Expense = 2;
            public static int Liability = 3;
            public static int Equity = 4;
            public static int Revenue = 5;
        }

        public class AccountCode
        {
            public static string Asset = "1";
            public static string CurrentAsset = "11";
            public static string CashBank = "1101";
            public static string AccountReceivable = "1102";
            public static string GBCHReceivable = "1103";
            public static string Inventory = "1104";
            public static string Raw = "1104001";
            public static string FinishedGoods = "1104002";
            public static string PrepaidExpense = "1105";
            public static string PiutangLainLain = "1106";
            public static string NonCurrentAsset = "12";
            public static string UnrecognizedCapitalGain = "13";

            public static string Expense = "2";
            public static string COGS = "21";
            public static string COS = "22";
            public static string OperationalExpense = "23";
            public static string ManufacturingExpense = "2301";
            public static string RecoveryExpense = "2301001";
            public static string ConversionExpense = "2301002";
            public static string SellingGeneralAndAdministrationExpense = "2302";
            public static string CashBankAdjustmentExpense = "2302001";
            public static string Discount = "2302002";
            public static string SalesAllowance = "2302003";
            public static string StockAdjustmentExpense = "2302004";
            public static string SampleAndTrialExpense = "2302005";
            public static string NonOperationalExpense = "24";

            public static string DepreciationExpense = "2401";
            public static string Amortization = "2402";            
            public static string InterestExpense = "2403";
            public static string TaxExpense = "2404";
            public static string DividentExpense = "2405";
            public static string ExchangeLoss = "25";

            public static string Liability = "3";
            public static string CurrentLiability = "31";
            public static string AccountPayable = "3101";
            public static string GBCHPayable = "3102";
            public static string GoodsPendingClearance = "3103";
            public static string UnearnedRevenue = "3104";
            public static string PurchaseAllowance = "3105";
            public static string AccountPayableNonTrading = "3106";
            public static string HutangLainLain = "3107";
            public static string TaxPayable = "3108";
            public static string NonCurrentLiability = "32";

            public static string Equity = "4";
            public static string OwnersEquity = "41";
            public static string EquityAdjustment = "4101";
            public static string ExchangeGain = "42";

            public static string Revenue = "5";
        }

        public class AccountLegacyCode
        {
            public static string Asset = "A1";
            public static string CurrentAsset = "A11";
            public static string CashBank = "A1101";
            public static string AccountReceivable = "A1102";
            public static string GBCHReceivable = "A1103";
            public static string Inventory = "A1104";
            public static string Raw = "A1104001";
            public static string FinishedGoods = "A1104002";
            public static string PiutangLainLain = "A1106";

            public static string Expense = "X2";
            public static string COGS = "X21";
            public static string COS = "X22"; //
            public static string OperationalExpense = "X23"; //
            public static string ManufacturingExpense = "X2301";
            public static string RecoveryExpense = "X2301001"; //
            public static string ConversionExpense = "X2301002"; //
            public static string SellingGeneralAndAdministrationExpense = "X2302"; //
            public static string CashBankAdjustmentExpense = "X2302001";
            public static string Discount = "X2302002";
            public static string SalesAllowance = "X2302003"; //
            public static string StockAdjustmentExpense = "X2302004";
            public static string SampleAndTrialExpense = "X2302005"; //
            public static string NonOperationalExpense = "X24";
            public static string DepreciationExpense = "X2401";
            public static string Amortization = "X2402";
            public static string InterestExpense = "X2403";
            public static string TaxExpense = "X2404"; 
            public static string DividentExpense = "X2405";
            public static string ExchangeLoss = "X25";

            public static string Liability = "L3";
            public static string CurrentLiability = "L31";
            public static string AccountPayable = "L3101";
            public static string GBCHPayable = "L3102";
            public static string GoodsPendingClearance = "L3103";
            public static string PurchaseAllowance = "L3105";
            public static string HutangLainLain = "L3107";
            public static string TaxPayable = "L3108";

            public static string NonCurrentLiability = "L32";
             
            public static string Equity = "E4";
            public static string OwnersEquity = "E41";
            public static string EquityAdjustment = "E4101";
            public static string ExchangeGain = "E42";
            public static string Revenue = "R5";
        }
        #endregion
    }
}