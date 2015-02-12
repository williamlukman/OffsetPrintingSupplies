using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseReceivalService
    {
        IPurchaseReceivalValidator GetValidator();
        IQueryable<PurchaseReceival> GetQueryable();
        IList<PurchaseReceival> GetAll();
        PurchaseReceival GetObjectById(int Id);
        IList<PurchaseReceival> GetObjectsByPurchaseOrderId(int purchaseOrderId);
        IList<PurchaseReceival> GetConfirmedObjects();
        PurchaseReceival CreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService);
        PurchaseReceival CreateObject(int warehouseId, int purchaseOrderId, DateTime ReceivalDate, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService);
        PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService);
        PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool DeleteObject(int Id);
        PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival, DateTime ConfirmationDate, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                       IPurchaseOrderService _purchaseOrderService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService,
                                       IItemService _itemService, IItemTypeService _itemTypeService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                       ICurrencyService _currencyService, IExchangeRateService _exchangeRateService);
        PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseInvoiceService _purchaseInvoiceService,
                                         IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseOrderService _purchaseOrderService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                         IStockMutationService _stockMutationService, IItemService _itemService, IItemTypeService _itemTypeService, IBlanketService _blanketService,
                                         IWarehouseItemService _warehouseItemService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PurchaseReceival CheckAndSetInvoiceComplete(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival UnsetInvoiceComplete(PurchaseReceival purchaseReceival);
    }
}