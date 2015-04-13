using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBlanketWarehouseMutationDetailService
    {
        IQueryable<BlanketWarehouseMutationDetail> GetQueryable();
        IBlanketWarehouseMutationDetailValidator GetValidator();
        IList<BlanketWarehouseMutationDetail> GetAll();
        IList<BlanketWarehouseMutationDetail> GetObjectsByBlanketWarehouseMutationId(int blanketWarehouseMutationId);
        BlanketWarehouseMutationDetail GetObjectByBlanketOrderDetailId(int blanketOrderDetailId);
        BlanketWarehouseMutationDetail GetObjectById(int Id);
        BlanketWarehouseMutationDetail CreateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                   IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BlanketWarehouseMutationDetail CreateObject(int blanketWarehouseMutationId, int itemId, int quantity, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                   IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BlanketWarehouseMutationDetail UpdateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                   IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BlanketWarehouseMutationDetail SoftDeleteObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService, IWarehouseItemService _warehouseItemService);
        BlanketWarehouseMutationDetail ConfirmObject(BlanketWarehouseMutationDetail BlanketWarehouseMutationDetail, DateTime ConfirmationDate, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                   IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                   IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService,
                                                   ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService);
        BlanketWarehouseMutationDetail UnconfirmObject(BlanketWarehouseMutationDetail BlanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                     IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                     IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService,
                                                     ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService);
        bool DeleteObject(int Id);
    }
}