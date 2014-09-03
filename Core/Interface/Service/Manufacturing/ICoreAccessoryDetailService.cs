using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICoreAccessoryDetailService
    {
        ICoreAccessoryDetailValidator GetValidator();
        ICoreAccessoryDetailRepository GetRepository();
        IQueryable<CoreAccessoryDetail> GetQueryable();
        IList<CoreAccessoryDetail> GetAll();
        IList<CoreAccessoryDetail> GetObjectsByCoreIdentificationDetailId(int coreIdentificationDetailId);
        IList<CoreAccessoryDetail> GetObjectsByItemId(int ItemId);
        CoreAccessoryDetail GetObjectById(int Id);
        CoreAccessoryDetail CreateObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        CoreAccessoryDetail CreateObject(int CoreIdentificationDetailId, int ItemId, int Quantity, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        CoreAccessoryDetail UpdateObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        CoreAccessoryDetail SoftDeleteObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationDetailService _coreIdentificationDetailService);
        bool DeleteObject(int Id);
    }
}