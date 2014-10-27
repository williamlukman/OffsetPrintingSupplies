using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesQuotationDetailValidator
    {
        SalesQuotationDetail VHasSalesQuotation(SalesQuotationDetail salesQuotationDetail, ISalesQuotationService _salesOrderService);
        SalesQuotationDetail VSalesQuotationHasNotBeenConfirmed(SalesQuotationDetail salesQuotationDetail, ISalesQuotationService _salesOrderService);
        SalesQuotationDetail VHasItem(SalesQuotationDetail salesQuotationDetail, IItemService _itemService);
        SalesQuotationDetail VNonZeroNorNegativeQuantity(SalesQuotationDetail salesQuotationDetail);
        SalesQuotationDetail VNonNegativePrice(SalesQuotationDetail salesQuotationDetail);
        SalesQuotationDetail VUniqueSalesQuotationDetail(SalesQuotationDetail salesQuotationDetail, ISalesQuotationDetailService _salesQuotationDetailService, IItemService _itemService);
        SalesQuotationDetail VHasBeenConfirmed(SalesQuotationDetail salesQuotationDetail);
        SalesQuotationDetail VHasNotBeenConfirmed(SalesQuotationDetail salesQuotationDetail);
        SalesQuotationDetail VHasNoSalesOrderDetail(SalesQuotationDetail salesQuotationDetail, ISalesOrderDetailService _salesOrderDetailService);
        SalesQuotationDetail VHasConfirmationDate(SalesQuotationDetail salesQuotationDetail);
        SalesQuotationDetail VCreateObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationDetailService _salesQuotationDetailService, ISalesQuotationService _salesOrderService, IItemService _itemService);
        SalesQuotationDetail VUpdateObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationDetailService _salesQuotationDetailService, ISalesQuotationService _salesOrderService, IItemService _itemService);
        SalesQuotationDetail VDeleteObject(SalesQuotationDetail salesQuotationDetail);
        SalesQuotationDetail VConfirmObject(SalesQuotationDetail salesQuotationDetail);
        SalesQuotationDetail VUnconfirmObject(SalesQuotationDetail salesQuotationDetail, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        bool ValidCreateObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationDetailService _salesQuotationDetailService, ISalesQuotationService _salesOrderService, IItemService _itemService);
        bool ValidUpdateObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationDetailService _salesQuotationDetailService, ISalesQuotationService _salesOrderService, IItemService _itemService);
        bool ValidDeleteObject(SalesQuotationDetail salesQuotationDetail);
        bool ValidConfirmObject(SalesQuotationDetail salesQuotationDetail);
        bool ValidUnconfirmObject(SalesQuotationDetail salesQuotationDetail, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        bool isValid(SalesQuotationDetail salesQuotationDetail);
        string PrintError(SalesQuotationDetail salesQuotationDetail);
    }
}
