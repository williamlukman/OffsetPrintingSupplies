using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IRollerWarehouseMutationDetailValidator
    {
        RollerWarehouseMutationDetail VHasCoreIdentificationDetail(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, ICoreIdentificationDetailService _coreIdentificationDetailService);
        RollerWarehouseMutationDetail VHasRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService);
        RollerWarehouseMutationDetail VHasWarehouseItemFrom(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VHasWarehouseItemTo(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VUniqueItem(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService);
        RollerWarehouseMutationDetail VNonNegativeStockQuantity(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                               IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, bool CaseConfirmOrFinish);
        RollerWarehouseMutationDetail VRollerWarehouseMutationHasBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService);
        RollerWarehouseMutationDetail VRollerWarehouseMutationHasNotBeenCompleted(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService);
        RollerWarehouseMutationDetail VHasNotBeenFinished(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail VHasBeenFinished(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail VCreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                   IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VUpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                   IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail VFinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                    IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VUnfinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidCreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                               IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                               IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidFinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidUnfinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        bool isValid(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        string PrintError(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
    }
}