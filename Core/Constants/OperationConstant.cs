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

        public class GroupType
        {
            public static string Base = "Base";
        }

        public class ItemCase
        {
            public static int Ready = 1;
            public static int PendingReceival = 2;
            public static int PendingDelivery = 3;
        }

        public class MutationStatus
        {
            public static int Addition = 1;
            public static int Deduction = 2;
        }

        public class SourceDocumentType
        {
            public static string BarringOrder = "BarringOrder";
            public static string CashBankMutation = "CashBankMutation";
            public static string CashBankAdjustment = "CashBankAdjustment";
            public static string CoreIdentification = "CoreIdentification";
            public static string DeliveryOrder = "DeliveryOrder";
            public static string PaymentVoucher = "PaymentVoucher";
            public static string PurchaseOrder = "PurchaseOrder";
            public static string PurchaseReceival = "PurchaseReceival";
            public static string ReceiptVoucher = "ReceiptVoucher";
            public static string RecoveryOrder = "RecoveryOrder";
            public static string RecoveryOrderDetail = "RecoveryOrderDetail";
            public static string RetailPurchaseInvoice = "RetailPurchaseInvoice";
            public static string RetailSalesInvoice = "RetailSalesInvoice";
            public static string RollerWarehouseMutation = "RollerWarehouseMutation";
            public static string SalesOrder = "SalesOrder";
            public static string StockAdjustment = "StockAdjustment";
            public static string WarehouseMutationOrder = "WarehouseMutationOrder";
            public static string CashSalesInvoice = "CashSalesInvoice";
            public static string CashSalesReturn = "CashSalesReturn";
            public static string CustomPurchaseInvoice = "CustomPurchaseInvoice";
        }

        public class SourceDocumentDetailType
        {
            public static string BarringOrderDetail = "BarringOrderDetail";
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
            public static string StockAdjustmentDetail = "StockAdjustmentDetail";
            public static string WarehouseMutationOrderDetail = "WarehouseMutationOrderDetail";
            public static string CashSalesInvoiceDetail = "CashSalesInvoiceDetail";
            public static string CashSalesReturnDetail = "CashSalesReturnDetail";
            public static string CustomPurchaseInvoiceDetail = "CustomPurchaseInvoiceDetail";
        }

        public class PayableSource
        {
            public static string PurchaseInvoice = "PurchaseInvoice";
            public static string RetailPurchaseInvoice = "RetailPurchaseInvoice";
            public static string CashSalesReturn = "CashSalesReturn";
            public static string CustomPurchaseInvoice = "CustomPurchaseInvoice";
        }

        public class ReceivableSource
        {
            public static string SalesInvoice = "SalesInvoice";
            public static string RetailSalesInvoice = "RetailSalesInvoice";
            public static string CashSalesInvoice = "CashSalesInvoice";
        }
    }
}
