using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IRollerWarehouseMutationValidator
    {
        RollerWarehouseMutation VHasRecoveryOrder(RollerWarehouseMutation rollerWarehouseMutation, IRecoveryOrderService _recoveryOrderService);
        RollerWarehouseMutation VRecoveryOrderHasBeenCompleted(RollerWarehouseMutation rollerWarehouseMutation, IRecoveryOrderService _recoveryOrderService);
        RollerWarehouseMutation VHasDifferentWarehouse(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation VHasWarehouseFrom(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService);
        RollerWarehouseMutation VHasWarehouseTo(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService);
        RollerWarehouseMutation VHasMutationDate(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation VHasRollerWarehouseMutationDetails(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        RollerWarehouseMutation VQuantityIsEqualTheNumberOfDetails(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        RollerWarehouseMutation VHasNotBeenConfirmed(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation VHasBeenConfirmed(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation VDetailsAreVerifiedConfirmable(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                                              IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutation VCreateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService);
        RollerWarehouseMutation VUpdateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService);
        RollerWarehouseMutation VDeleteObject(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation VHasConfirmationDate(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation VConfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                              IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutation VUnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                                IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        bool ValidCreateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService);
        bool ValidUpdateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService);
        bool ValidDeleteObject(RollerWarehouseMutation rollerWarehouseMutation);
        bool ValidConfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                  IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        bool isValid(RollerWarehouseMutation rollerWarehouseMutation);
        string PrintError(RollerWarehouseMutation rollerWarehouseMutation);
    }
}