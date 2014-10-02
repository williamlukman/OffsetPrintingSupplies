using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Constants
{
    public partial class Constant
    {
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
            public static string PaymentVoucher = "PaymentVoucher";
            public static string ReceiptVoucher = "ReceiptVoucher";
            //public static string RetailSalesInvoice = "RetailSalesInvoice";
            public static string StockAdjustment = "StockAdjustment";
            public static string SalesInvoice = "SalesInvoice";
            public static string PurchaseInvoice = "PurchaseInvoice";
        }

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
            public static string NonCurrentAsset = "12";

            public static string Expense = "2";
            public static string COGS = "21";
            public static string SellingGeneralAndAdministrationExpense = "22";
            public static string CashBankAdjustmentExpense = "2201";
            public static string Discount = "2202";
            public static string SalesAllowance = "2203";
            public static string StockAdjustmentExpense = "2204";
            public static string DepreciationExpense = "23";
            public static string Amortization = "24";            
            public static string InterestExpense = "25";
            public static string TaxExpense = "26";
            public static string DividentExpense = "27";
            
            public static string Liability = "3";
            public static string CurrentLiability = "31";
            public static string AccountPayable = "3101";
            public static string GBCHPayable = "3102";
            public static string GoodsPendingClearance = "3103";
            public static string NonCurrentLiability = "32";

            public static string Equity = "4";
            public static string OwnersEquity = "41";
            public static string EquityAdjustment = "4101";

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
            public static string FinishedGoods = "A114002";
            public static string NonCurrentAsset = "A12";

            public static string Expense = "X2";
            public static string COGS = "X21";
            public static string SellingGeneralAndAdministrationExpense = "X22";
            public static string CashBankAdjustmentExpense = "X2201";
            public static string Discount = "X2202";
            public static string SalesAllowance = "X2203";
            public static string StockAdjustmentExpense = "X2204";
            public static string DepreciationExpense = "X23";
            public static string Amortization = "X24";
            public static string InterestExpense = "X25";
            public static string TaxExpense = "X26";
            public static string DividentExpense = "X27";

            public static string Liability = "L3";
            public static string CurrentLiability = "L31";
            public static string AccountPayable = "L3101";
            public static string GBCHPayable = "L3102";
            public static string GoodsPendingClearance = "L3103";
            public static string NonCurrentLiability = "L32";

            public static string Equity = "E4";
            public static string OwnersEquity = "E41";
            public static string EquityAdjustment = "E4101";

            public static string Revenue = "R5";
        }
    }
}