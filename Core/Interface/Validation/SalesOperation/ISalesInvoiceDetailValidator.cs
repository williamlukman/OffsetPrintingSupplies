using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesInvoiceDetailValidator
    {
        SalesInvoiceDetail VHasSalesInvoice(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService);
        SalesInvoiceDetail VSalesInvoiceHasNotBeenConfirmed(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService);
        SalesInvoiceDetail VHasDeliveryOrderDetail(SalesInvoiceDetail salesInvoiceDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail VDeliveryOrderDetailAndSalesInvoiceMustHaveTheSameDeliveryOrder(SalesInvoiceDetail salesInvoiceDetail,
                                                         IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesInvoiceService salesInvoiceService);
        SalesInvoiceDetail VServiceCostQuantityIsNaturalNumber(SalesInvoiceDetail salesInvoiceDetail, IServiceCostService _serviceCostService,
                                                               IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesOrderDetailService _salesOrderDetailService);
        SalesInvoiceDetail VIsUniqueDeliveryOrderDetail(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                                              IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail VNonNegativeNorZeroQuantity(SalesInvoiceDetail salesInvoiceDetail);
        SalesInvoiceDetail VQuantityIsLessThanOrEqualPendingInvoiceQuantity(SalesInvoiceDetail salesInvoiceDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail VHasNotBeenConfirmed(SalesInvoiceDetail salesInvoiceDetail);
        SalesInvoiceDetail VHasBeenConfirmed(SalesInvoiceDetail salesInvoiceDetail);
        SalesInvoiceDetail VCreateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                            IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail VUpdateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                            IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail VDeleteObject(SalesInvoiceDetail salesInvoiceDetail);
        SalesInvoiceDetail VHasConfirmationDate(SalesInvoiceDetail salesInvoiceDetail);
        SalesInvoiceDetail VConfirmObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                          IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                          IServiceCostService _serviceCostService);
        SalesInvoiceDetail VUnconfirmObject(SalesInvoiceDetail salesInvoiceDetail);
        bool ValidCreateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService, ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidUpdateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService, ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidDeleteObject(SalesInvoiceDetail salesInvoiceDetail);
        bool ValidConfirmObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                IServiceCostService _serviceCostService);
        bool ValidUnconfirmObject(SalesInvoiceDetail salesInvoiceDetail);
        bool isValid(SalesInvoiceDetail salesInvoiceDetail);
        string PrintError(SalesInvoiceDetail salesInvoiceDetail);
    }
}
