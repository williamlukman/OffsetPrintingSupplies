using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IRollerWarehouseMutationValidator
    {
        RollerWarehouseMutation VHasCoreIdentification(RollerWarehouseMutation rollerWarehouseMutation, ICoreIdentificationService _coreIdentificationService);
        RollerWarehouseMutation VHasDifferentWarehouse(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation VHasWarehouseFrom(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService);
        RollerWarehouseMutation VHasWarehouseTo(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService);
        RollerWarehouseMutation VHasRollerWarehouseMutationDetails(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        RollerWarehouseMutation VHasNotBeenConfirmed(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation VHasBeenConfirmed(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation VDetailsAreVerifiedConfirmable(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                                              IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutation VDetailsAreVerifiedUnconfirmable(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutation VAllDetailsHaveBeenFinished(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        RollerWarehouseMutation VCreateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, ICoreIdentificationService _coreIdentificationService);
        RollerWarehouseMutation VUpdateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, ICoreIdentificationService _coreIdentificationService);
        RollerWarehouseMutation VDeleteObject(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation VConfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                              IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutation VUnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutation VCompleteObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        bool ValidCreateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, ICoreIdentificationService _coreIdentificationService);
        bool ValidUpdateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, ICoreIdentificationService _coreIdentificationService);
        bool ValidDeleteObject(RollerWarehouseMutation rollerWarehouseMutation);
        bool ValidConfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                  IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidCompleteObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        bool isValid(RollerWarehouseMutation rollerWarehouseMutation);
        string PrintError(RollerWarehouseMutation rollerWarehouseMutation);
    }
}