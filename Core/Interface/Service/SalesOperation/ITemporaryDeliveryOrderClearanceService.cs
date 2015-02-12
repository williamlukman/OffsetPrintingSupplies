using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITemporaryDeliveryOrderClearanceService
    {
        ITemporaryDeliveryOrderClearanceValidator GetValidator();
        ITemporaryDeliveryOrderClearanceRepository GetRepository();
        IQueryable<TemporaryDeliveryOrderClearance> GetQueryable();
        IList<TemporaryDeliveryOrderClearance> GetAll();
        TemporaryDeliveryOrderClearance GetObjectById(int Id);
        IList<TemporaryDeliveryOrderClearance> GetObjectsByTemporaryDeliveryOrderId(int deliveryOrderId);
        IList<TemporaryDeliveryOrderClearance> GetConfirmedObjects();
        TemporaryDeliveryOrderClearance CreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        TemporaryDeliveryOrderClearance UpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        TemporaryDeliveryOrderClearance SoftDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService);
        bool DeleteObject(int Id);
        TemporaryDeliveryOrderClearance ConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, DateTime ConfirmationDate,
                                     ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IStockMutationService _stockMutationService, 
                                     IItemService _itemService, IItemTypeService _itemTypeService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, 
                                     ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                     IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        TemporaryDeliveryOrderClearance UnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                               IStockMutationService _stockMutationService, IItemService _itemService, IItemTypeService _itemTypeService,
                                               IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                               IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        
    }
}