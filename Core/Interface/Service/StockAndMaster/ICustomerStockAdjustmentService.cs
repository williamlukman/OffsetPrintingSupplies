using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICustomerStockAdjustmentService
    {
        ICustomerStockAdjustmentValidator GetValidator();
        IQueryable<CustomerStockAdjustment> GetQueryable();
        IList<CustomerStockAdjustment> GetAll();
        CustomerStockAdjustment GetObjectById(int Id);
        CustomerStockAdjustment CreateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService);
        CustomerStockAdjustment CreateObject(int WarehouseId, int ContactId, DateTime AdjustmentDate, IWarehouseService _warehouseService, IContactService _contactService);
        CustomerStockAdjustment UpdateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService);
        CustomerStockAdjustment SoftDeleteObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService);
        bool DeleteObject(int Id);
        CustomerStockAdjustment ConfirmObject(CustomerStockAdjustment customerStockAdjustment, DateTime ConfirmationDate, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                             ICustomerStockMutationService _customerStockMutationService, IItemService _itemService, IItemTypeService _itemTypeService,
                                             ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService,
                                             IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        CustomerStockAdjustment UnconfirmObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                               ICustomerStockMutationService _customerStockMutationService, IItemService _itemService, IItemTypeService _itemTypeService,
                                               ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService,
                                               IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
    }
}