using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IBlanketWarehouseMutationValidator
    {
        BlanketWarehouseMutation VHasBlanketOrder(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketOrderService _blanketOrderService);
        BlanketWarehouseMutation VBlanketOrderHasBeenCompleted(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketOrderService _blanketOrderService);
        BlanketWarehouseMutation VHasDifferentWarehouse(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation VHasWarehouseFrom(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService);
        BlanketWarehouseMutation VHasWarehouseTo(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService);
        BlanketWarehouseMutation VHasMutationDate(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation VHasBlanketWarehouseMutationDetails(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService);
        BlanketWarehouseMutation VQuantityIsEqualTheNumberOfDetails(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService);
        BlanketWarehouseMutation VHasNotBeenConfirmed(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation VHasBeenConfirmed(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation VDetailsAreVerifiedConfirmable(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketWarehouseMutationService _blanketWarehouseMutationService, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService,
                                                               IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService,
                                                               IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService);
        BlanketWarehouseMutation VAllDetailsAreUnconfirmable(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                                            IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IBlanketWarehouseMutationService _blanketWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        BlanketWarehouseMutation VCreateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService);
        BlanketWarehouseMutation VUpdateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService);
        BlanketWarehouseMutation VDeleteObject(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation VHasConfirmationDate(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation VConfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketWarehouseMutationService _blanketWarehouseMutationService, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService,
                                               IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService,
                                               IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService);
        BlanketWarehouseMutation VUnconfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService, 
                                                IBlanketWarehouseMutationService _blanketWarehouseMutationService, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService,
                                                IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        bool ValidCreateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService);
        bool ValidUpdateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService);
        bool ValidDeleteObject(BlanketWarehouseMutation blanketWarehouseMutation);
        bool ValidConfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketWarehouseMutationService _blanketWarehouseMutationService, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService,
                                IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService,
                                IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService);
        bool ValidUnconfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                  IBlanketWarehouseMutationService _blanketWarehouseMutationService, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService,
                                  IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        bool isValid(BlanketWarehouseMutation blanketWarehouseMutation);
        string PrintError(BlanketWarehouseMutation blanketWarehouseMutation);
    }
}