using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITemporaryDeliveryOrderService
    {
        ITemporaryDeliveryOrderValidator GetValidator();
        IQueryable<TemporaryDeliveryOrder> GetQueryable();
        IList<TemporaryDeliveryOrder> GetAll();
        TemporaryDeliveryOrder GetObjectById(int Id);
        IList<TemporaryDeliveryOrder> GetObjectsByVirtualOrderId(int virtualOrderId);
        IList<TemporaryDeliveryOrder> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        IList<TemporaryDeliveryOrder> GetConfirmedObjects();
        TemporaryDeliveryOrder CreateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        TemporaryDeliveryOrder UpdateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        TemporaryDeliveryOrder SoftDeleteObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        bool DeleteObject(int Id);
        TemporaryDeliveryOrder ConfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime ConfirmationDate, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                             IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                             IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                             ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService,
                                             IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrder UnconfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                               IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                               IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                               ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                               IStockMutationService _stockMutationService, IItemService _itemService,
                                               IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService);
        TemporaryDeliveryOrder ReconcileObject(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                               IStockMutationService _stockMutationService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
                                               IClosingService _closingService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService);
        TemporaryDeliveryOrder UnreconcileObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                               IStockMutationService _stockMutationService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
                                               IClosingService _closingService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService);
        TemporaryDeliveryOrder PushObject(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                          IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderService _salesOrderService,
                                          ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService,
                                          IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService, IStockMutationService _stockMutationService,
                                          IContactService _contactService, IBlanketService _blanketService, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService,
                                          IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                          IServiceCostService _serviceCostService, ISalesQuotationService _salesQuotationService, ISalesQuotationDetailService _salesQuotationDetailService);
        TemporaryDeliveryOrder CheckAndSetDeliveryComplete(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrder UnsetDeliveryComplete(TemporaryDeliveryOrder temporaryDeliveryOrder);
    }
}