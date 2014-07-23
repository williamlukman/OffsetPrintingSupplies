using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public partial class Constant
    {
        public class StockMutationItemCase
        {
            public static int Ready = 1;
            public static int PendingReceival = 2;
            public static int PendingDelivery = 3;
        }

        public class StockMutationStatus
        {
            public static int Addition = 1;
            public static int Deduction = 2;
        }

        public class SourceDocumentType
        {
            public static string BarringOrder = "BarringOrder";
            public static string CoreIdentification = "CoreIdentification";
            public static string DeliveryOrder = "DeliveryOrder";
            public static string PurchaseOrder = "PurchaseOrder";
            public static string PurchaseReceival = "PurchaseReceival";
            public static string RecoveryOrder = "RecoveryOrder";
            public static string RecoveryOrderDetail = "RecoveryOrderDetail";
            public static string SalesOrder = "SalesOrder";
            public static string StockAdjustment = "StockAdjustment";
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
            public static string SalesOrderDetail = "SalesOrderDetail";
            public static string StockAdjustmentDetail = "StockAdjustmentDetail";
        }

        public class PayableSource
        {
            public static string PurchaseInvoice = "PurchaseInvoice";
        }

        public class ReceivableSource
        {
            public static string SalesInvoice = "SalesInvoice";
        }
    }
}
