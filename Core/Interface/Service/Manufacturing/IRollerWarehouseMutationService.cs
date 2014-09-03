using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IRollerWarehouseMutationService
    {
        IQueryable<RollerWarehouseMutation> GetQueryable();
        IRollerWarehouseMutationValidator GetValidator();
        IList<RollerWarehouseMutation> GetAll();
        IList<RollerWarehouseMutation> GetObjectsByRecoveryOrderId(int recoveryOrderId);
        RollerWarehouseMutation GetObjectById(int Id);
        Warehouse GetWarehouseFrom(RollerWarehouseMutation rollerWarehouseMutation);
        Warehouse GetWarehouseTo(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation CreateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService);
        RollerWarehouseMutation UpdateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService);
        RollerWarehouseMutation SoftDeleteObject(RollerWarehouseMutation rollerWarehouseMutation);
        bool DeleteObject(int Id);
        RollerWarehouseMutation ConfirmObject(RollerWarehouseMutation rollerWarehouseMutation, DateTime ConfirmationDate, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                              IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                              IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService);
        RollerWarehouseMutation UnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                               IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                               IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService);
    }
}