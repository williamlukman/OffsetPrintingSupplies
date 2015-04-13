using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBlanketWarehouseMutationService
    {
        IQueryable<BlanketWarehouseMutation> GetQueryable();
        IBlanketWarehouseMutationValidator GetValidator();
        IList<BlanketWarehouseMutation> GetAll();
        IList<BlanketWarehouseMutation> GetObjectsByBlanketOrderId(int blanketOrderId);
        BlanketWarehouseMutation GetObjectById(int Id);
        Warehouse GetWarehouseFrom(BlanketWarehouseMutation blanketWarehouseMutation);
        Warehouse GetWarehouseTo(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation CreateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService);
        BlanketWarehouseMutation UpdateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService);
        BlanketWarehouseMutation SoftDeleteObject(BlanketWarehouseMutation blanketWarehouseMutation);
        bool DeleteObject(int Id);
        BlanketWarehouseMutation ConfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, DateTime ConfirmationDate, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService,
                                              IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                              IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                              ICoreIdentificationService _coreIdentificationService, ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService);
        BlanketWarehouseMutation UnconfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService,
                                               IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                               IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService,
                                               ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService);
    }
}