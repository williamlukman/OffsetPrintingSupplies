using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseReceivalValidator
    {
        PurchaseReceival VHasUniqueNomorSurat(PurchaseReceival purchaseReceival, IPurchaseReceivalService _purchaseReceivalService);
        PurchaseReceival VHasWarehouse(PurchaseReceival purchaseReceival, IWarehouseService _warehouseService);
        PurchaseReceival VHasPurchaseOrder(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService);
        PurchaseReceival VHasReceivalDate(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasNotBeenConfirmed(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasBeenConfirmed(PurchaseReceival purchaseReceival);
        PurchaseReceival VPurchaseOrderHasBeenConfirmed(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService);
        PurchaseReceival VHasPurchaseReceivalDetails(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VHasNoPurchaseReceivalDetail(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VHasConfirmationDate(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasExchangeRateList(PurchaseReceival purchaseReceival, IExchangeRateService _exchangeRateService,
                                              IPurchaseOrderService _purchaseOrderService, ICurrencyService _currencyService);
        PurchaseReceival VHasNoPurchaseInvoice(PurchaseReceival purchaseReceival, IPurchaseInvoiceService _purchaseInvoiceService);
        PurchaseReceival VCreateObject(PurchaseReceival purchaseReceival, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService);
        PurchaseReceival VUpdateObject(PurchaseReceival purchaseReceival, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService);
        PurchaseReceival VDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IExchangeRateService _exchangeRateService,
                                        IPurchaseOrderService _purchaseOrderService, ICurrencyService _currencyService);
        PurchaseReceival VUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseInvoiceService _purchaseInvoiceService);
        bool ValidCreateObject(PurchaseReceival purchaseReceival, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService);
        bool ValidUpdateObject(PurchaseReceival purchaseReceival, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService);
        bool ValidDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IExchangeRateService _exchangeRateService,
                                IPurchaseOrderService _purchaseOrderService, ICurrencyService _currencyService);
        bool ValidUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseInvoiceService _purchaseInvoiceService);
        bool isValid(PurchaseReceival purchaseReceival);
        string PrintError(PurchaseReceival purchaseReceival);
    }
}
