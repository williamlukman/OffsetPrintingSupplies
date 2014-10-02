using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesInvoiceValidator
    {
        SalesInvoice VHasDeliveryOrder(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService);
        SalesInvoice VHasNoSalesInvoiceDetails(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService);
        SalesInvoice VHasSalesInvoiceDetails(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService);
        SalesInvoice VDeliveryOrderHasNotBeenInvoiceCompleted(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService);
        SalesInvoice VHasInvoiceDate(SalesInvoice salesInvoice);
        SalesInvoice VHasDueDate(SalesInvoice salesInvoice);
        SalesInvoice VHasDiscountBetweenZeroAndHundred(SalesInvoice salesInvoice);
        SalesInvoice VHasReceiptVoucherDetails(SalesInvoice salesInvoice, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        SalesInvoice VHasNotBeenConfirmed(SalesInvoice salesInvoice);
        SalesInvoice VHasBeenConfirmed(SalesInvoice salesInvoice);
        SalesInvoice VHasNotBeenDeleted(SalesInvoice salesInvoice);
        SalesInvoice VAllSalesInvoiceDetailsAreConfirmable(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoice VAllSalesInvoiceDetailsAreUnconfirmable(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService);
        SalesInvoice VReceivableHasNoOtherAssociation(SalesInvoice salesInvoice, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        SalesInvoice VGeneralLedgerPostingHasNotBeenClosed(SalesInvoice salesInvoice, IClosingService _closingService, int CaseConfirmUnconfirm);
        SalesInvoice VCreateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService);
        SalesInvoice VUpdateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService, ISalesInvoiceDetailService _salesInvoiceDetailService);
        SalesInvoice VDeleteObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService);
        SalesInvoice VHasConfirmationDate(SalesInvoice salesInvoice);
        SalesInvoice VConfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                       IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService, IClosingService _closingService);
        SalesInvoice VUnconfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                      IReceivableService _receivableService, IClosingService _closingService);
        bool ValidCreateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService);
        bool ValidUpdateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService, ISalesInvoiceDetailService _salesInvoiceDetailService);
        bool ValidDeleteObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService);
        bool ValidConfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService, IClosingService _closingService);
        bool ValidUnconfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                  IReceivableService _receivableService, IClosingService _closingService);
        bool isValid(SalesInvoice salesInvoice);
        string PrintError(SalesInvoice salesInvoice);
    }
}
