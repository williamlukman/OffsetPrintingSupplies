using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IWarehouseMutationValidator
    {
        WarehouseMutation VHasDifferentWarehouse(WarehouseMutation warehouseMutation);
        WarehouseMutation VHasWarehouseFrom(WarehouseMutation warehouseMutation, IWarehouseService _warehouseService);
        WarehouseMutation VHasWarehouseTo(WarehouseMutation warehouseMutation, IWarehouseService _warehouseService);
        WarehouseMutation VHasWarehouseMutationDetails(WarehouseMutation warehouseMutation, IWarehouseMutationDetailService _warehouseMutationDetailService);
        WarehouseMutation VHasMutationDate(WarehouseMutation warehouseMutation);
        WarehouseMutation VHasNotBeenConfirmed(WarehouseMutation warehouseMutation);
        WarehouseMutation VHasBeenConfirmed(WarehouseMutation warehouseMutation);
        WarehouseMutation VDetailsAreVerifiedConfirmable(WarehouseMutation warehouseMutation, IWarehouseMutationService _warehouseMutationService, IWarehouseMutationDetailService _warehouseMutationDetailService,
                                                              IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        WarehouseMutation VCreateObject(WarehouseMutation warehouseMutation, IWarehouseService _warehouseService);
        WarehouseMutation VUpdateObject(WarehouseMutation warehouseMutation, IWarehouseService _warehouseService);
        WarehouseMutation VDeleteObject(WarehouseMutation warehouseMutation);
        WarehouseMutation VHasConfirmationDate(WarehouseMutation warehouseMutation);
        WarehouseMutation VConfirmObject(WarehouseMutation warehouseMutation, IWarehouseMutationService _warehouseMutationService, IWarehouseMutationDetailService _warehouseMutationDetailService,
                                              IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        WarehouseMutation VUnconfirmObject(WarehouseMutation warehouseMutation, IWarehouseMutationService _warehouseMutationService, IWarehouseMutationDetailService _warehouseMutationDetailService,
                                                IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        bool ValidCreateObject(WarehouseMutation warehouseMutation, IWarehouseService _warehouseService);
        bool ValidUpdateObject(WarehouseMutation warehouseMutation, IWarehouseService _warehouseService);
        bool ValidDeleteObject(WarehouseMutation warehouseMutation);
        bool ValidConfirmObject(WarehouseMutation warehouseMutation, IWarehouseMutationService _warehouseMutationService, IWarehouseMutationDetailService _warehouseMutationDetailService,
                                IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(WarehouseMutation warehouseMutation, IWarehouseMutationService _warehouseMutationService, IWarehouseMutationDetailService _warehouseMutationDetailService,
                                  IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        bool isValid(WarehouseMutation warehouseMutation);
        string PrintError(WarehouseMutation warehouseMutation);
    }
}