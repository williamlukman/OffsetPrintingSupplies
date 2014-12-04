using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IRollerWarehouseMutationDetailService
    {
        IQueryable<RollerWarehouseMutationDetail> GetQueryable();
        IRollerWarehouseMutationDetailValidator GetValidator();
        IList<RollerWarehouseMutationDetail> GetAll();
        IList<RollerWarehouseMutationDetail> GetObjectsByRollerWarehouseMutationId(int rollerWarehouseMutationId);
        RollerWarehouseMutationDetail GetObjectByRecoveryOrderDetailId(int recoveryOrderDetailId);
        RollerWarehouseMutationDetail GetObjectById(int Id);
        RollerWarehouseMutationDetail CreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                   IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail CreateObject(int rollerWarehouseMutationId, int itemId, int quantity, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                   IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail UpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                   IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail SoftDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail ConfirmObject(RollerWarehouseMutationDetail RollerWarehouseMutationDetail, DateTime ConfirmationDate, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                   IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                   IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService,
                                                   ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService);
        RollerWarehouseMutationDetail UnconfirmObject(RollerWarehouseMutationDetail RollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                     IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                     IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService,
                                                     ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService);
        bool DeleteObject(int Id);
    }
}