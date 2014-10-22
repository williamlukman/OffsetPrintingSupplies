using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Constants
{
    public partial class Constant
    {
        public static string BaseContact = "BaseContact";
        public static TimeSpan PaymentDueDateTimeSpan = new TimeSpan(14, 0, 0);

        public class ControllerOutput
        {
            public static string PageViewNotAllowed = "You are not allowed to View this Page. <br/> <a href='/Authentication/Logout'>[Logout]</a>";
            public static string PagePrintNotAllowed = "You are not allowed to Print this Page. <br/> <a href='/Authentication/Logout'>[Logout]</a>";
            public static string ErrorPageHasNoClosingDate = "No report has been produced for this closing date.";
        }

        public class GroupType
        {
            public static string Base = "Base";
        }

        public class ItemCase
        {
            public static int Ready = 1;
            public static int PendingReceival = 2;
            public static int PendingDelivery = 3;
            public static int Virtual = 4;
        }

        public class MutationStatus
        {
            public static int Addition = 1;
            public static int Deduction = 2;
        }

        public class UserType
        {
            public static string Admin = "Admin";
        }

        public class SourceDocumentType
        {
            public static string BlanketOrder = "BlanketOrder";
            public static string CashBankMutation = "CashBankMutation";
            public static string CashBankAdjustment = "CashBankAdjustment";
            public static string CoreIdentification = "CoreIdentification";
            public static string DeliveryOrder = "DeliveryOrder";
            public static string PaymentVoucher = "PaymentVoucher";
            public static string PurchaseDownPayment = "PurchaseDownPayment";
            public static string PurchaseAllowance = "PurchaseAllowance";
            public static string PurchaseOrder = "PurchaseOrder";
            public static string PurchaseReceival = "PurchaseReceival";
            public static string ReceiptVoucher = "ReceiptVoucher";
            public static string RecoveryOrder = "RecoveryOrder";
            public static string RecoveryOrderDetail = "RecoveryOrderDetail";
            public static string RetailPurchaseInvoice = "RetailPurchaseInvoice";
            public static string RetailSalesInvoice = "RetailSalesInvoice";
            public static string RollerWarehouseMutation = "RollerWarehouseMutation";
            public static string SalesDownPayment = "SalesDownPayment";
            public static string SalesAllowance = "SalesAllowance";
            public static string SalesOrder = "SalesOrder";
            public static string TemporaryDeliveryOrder = "TemporaryDeliveryOrder";
            public static string StockAdjustment = "StockAdjustment";
            public static string VirtualOrder = "VirtualOrder";
            public static string WarehouseMutation = "WarehouseMutation";
        }

        public class SourceDocumentDetailType
        {
            public static string BlanketOrderDetail = "BlanketOrderDetail";
            public static string CoreIdentificationDetail = "CoreIdentificationDetail";
            public static string DeliveryOrderDetail = "DeliveryOrderDetail";
            public static string PurchaseOrderDetail = "PurchaseOrderDetail";
            public static string PurchaseReceivalDetail = "PurchaseReceivalDetail";
            public static string RecoveryOrderDetail = "RecoveryOrderDetail";
            public static string RecoveryAccessoryDetail = "RecoveryAccessoryDetail";
            public static string RetailPurchaseInvoiceDetail = "RetailPurchaseInvoiceDetail";
            public static string RetailSalesInvoiceDetail = "RetailSalesInvoiceDetail";
            public static string RollerWarehouseMutationDetail = "RollerWarehouseMutationDetail";
            public static string SalesOrderDetail = "SalesOrderDetail";
            public static string VirtualOrderDetail = "VirtualOrderDetail";
            public static string TemporaryDeliveryOrderDetail = "TemporaryDeliveryOrderDetail";
            public static string TemporaryDeliveryOrderDetailWaste = "TemporaryDeliveryOrderDetailWaste";
            public static string TemporaryDeliveryOrderDetailRestock = "TemporaryDeliveryOrderDetailRestock";
            public static string StockAdjustmentDetail = "StockAdjustmentDetail";
            public static string WarehouseMutationDetail = "WarehouseMutationDetail";
        }

        public class PayableSource
        {
            public static string PaymentRequest = "PaymentRequest";
            public static string PurchaseInvoice = "PurchaseInvoice";
            public static string RetailPurchaseInvoice = "RetailPurchaseInvoice";
        }

        public class ReceivableSource
        {
            public static string SalesInvoice = "SalesInvoice";
            public static string RetailSalesInvoice = "RetailSalesInvoice";
        }

        public class MenuGroupName
        {
            public static string Master = "Master";
            public static string Manufacturing = "Manufacturing";
            public static string Transaction = "Transaction";
            public static string Report = "Report";
            public static string Setting = "Setting";
        }

        public class MenuName
        {
            public static string Contact = "Contact";
            public static string ContactGroup = "Contact Group";
            public static string ItemType = "ItemType";
            public static string UoM = "UoM";
            public static string Machine = "Machine";
            public static string RollerType = "Roller Type";

            public static string QuantityPricing = "Quantity Pricing";
            public static string CashBank = "CashBank";
            public static string CashMutation = "Cash Mutation";
            public static string CashBankAdjustment = "CashBank Adjustment";
            public static string CashBankMutation = "CashBank Mutation";
            public static string PaymentRequest = "Payment Request";
            public static string DownPayment = "Down Payment";
            public static string PurchaseAllowance = "Purchase Allowance";
            public static string SalesAllowance = "Sales Allowance";

            public static string Blanket = "Blanket";
            public static string CoreBuilder = "CoreBuilder";
            public static string Item = "Item";
            public static string RollerBuilder = "RollerBuilder";
            public static string StockAdjustment = "Stock Adjustment";
            public static string StockMutation = "Stock Mutation";
            public static string Warehouse = "Warehouse";
            public static string WarehouseItem = "Warehouse Item";
            public static string WarehouseMutation = "Warehouse Mutation";

            public static string BlanketWorkOrder = "Blanket Work Order";
            public static string BlanketWorkProcess = "Blanket Work Process";
            public static string RollerIdentification = "Roller Identification";
            public static string RollerAccessoryDetail = "Roller Accessory Detail";
            public static string RollerWarehouseMutation = "Roller Warehouse Mutation";
            public static string RecoveryWorkOrder = "Recovery Work Order";
            public static string RecoveryWorkProcess = "Recovery Work Process";

            public static string PurchaseOrder = "Purchase Order";
            public static string PurchaseReceival = "Purchase Receival";
            public static string PurchaseInvoice = "Purchase Invoice";
            public static string CustomPurchaseInvoice = "Custom Purchase Invoice";
            public static string PaymentVoucher = "Payment Voucher";
            public static string Payable = "Payable";

            public static string SalesOrder = "Sales Order";
            public static string VirtualOrder = "Virtual Order";
            public static string TemporaryDeliveryOrder = "Temporary Delivery Order";
            public static string DeliveryOrder = "Delivery Order";
            public static string SalesInvoice = "Sales Invoice";
            public static string CashSalesInvoice = "Cash Sales Invoice";
            public static string CashSalesReturn = "Cash Sales Return";
            public static string RetailSalesInvoice = "Retail Sales Invoice";
            public static string ReceiptVoucher = "Receipt Voucher";
            public static string Receivable = "Receivable";

            public static string Sales = "Sales";
            public static string TopSales = "Top Sales";
            public static string ProfitLoss = "Profit/Loss";
            public static string Account = "Account";
            public static string Closing = "Closing";
            public static string GeneralLedger = "General Ledger";
            public static string ValidComb = "Valid Comb";
            public static string BalanceSheet = "Balance Sheet";
            public static string IncomeStatement = "Income Statement";
            public static string Memorial = "Memorial";

            public static string User = "User";
            public static string UserAccessRight = "User Access Right";
            public static string CompanyInfo = "Company Info";

        }
    }
}
