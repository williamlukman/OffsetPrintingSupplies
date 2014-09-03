using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ICoreAccessoryDetailValidator
    {
        CoreAccessoryDetail VHasCoreIdentificationDetail(CoreAccessoryDetail CoreAccessoryDetail, ICoreIdentificationDetailService _CoreIdentificationDetailService);
        CoreAccessoryDetail VIsAccessory(CoreAccessoryDetail CoreAccessoryDetail, IItemService _itemService, IItemTypeService _itemTypeService);
        CoreAccessoryDetail VNonNegativeNorZeroQuantity(CoreAccessoryDetail CoreAccessoryDetail);
        CoreAccessoryDetail VQuantityInStock(CoreAccessoryDetail CoreAccessoryDetail, ICoreIdentificationService _CoreIdentificationService,
                                                 ICoreIdentificationDetailService _CoreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);

        CoreAccessoryDetail VCreateObject(CoreAccessoryDetail CoreAccessoryDetail, ICoreIdentificationService _CoreIdentificationService, ICoreIdentificationDetailService _CoreIdentificationDetailService,
                                              IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        CoreAccessoryDetail VUpdateObject(CoreAccessoryDetail CoreAccessoryDetail, ICoreIdentificationService _CoreIdentificationService, ICoreIdentificationDetailService _CoreIdentificationDetailService,
                                              IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        CoreAccessoryDetail VDeleteObject(CoreAccessoryDetail CoreAccessoryDetail, ICoreIdentificationDetailService _CoreIdentificationDetailService);
        bool ValidCreateObject(CoreAccessoryDetail CoreAccessoryDetail, ICoreIdentificationService _CoreIdentificationService, ICoreIdentificationDetailService _CoreIdentificationDetailService,
                               IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(CoreAccessoryDetail CoreAccessoryDetail, ICoreIdentificationService _CoreIdentificationService, ICoreIdentificationDetailService _CoreIdentificationDetailService,
                               IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteObject(CoreAccessoryDetail CoreAccessoryDetail, ICoreIdentificationDetailService _CoreIdentificationDetailService);
        bool isValid(CoreAccessoryDetail CoreAccessoryDetail);
        string PrintError(CoreAccessoryDetail CoreAccessoryDetail);
    }
}