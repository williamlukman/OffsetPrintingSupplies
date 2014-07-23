using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IRollerWarehouseMutationDetailService
    {
        IRollerWarehouseMutationDetailValidator GetValidator();
        IList<RollerWarehouseMutationDetail> GetAll();
        IList<RollerWarehouseMutationDetail> GetObjectsByRollerWarehouseMutationId(int rollerWarehouseMutationId);
        RollerWarehouseMutationDetail GetObjectByCoreIdentificationDetailId(int coreIdentificationDetailId);
        RollerWarehouseMutationDetail GetObjectById(int Id);
        RollerWarehouseMutationDetail CreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _warehouseMutationOrderService,
                                                   ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail CreateObject(int rollerWarehouseMutationId, int itemId, int quantity, IRollerWarehouseMutationService _warehouseMutationOrderService,
                                                   ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail UpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _warehouseMutationOrderService,
                                                   ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail SoftDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _warehouseMutationOrderService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail FinishObject(RollerWarehouseMutationDetail RollerWarehouseMutationDetail, IRollerWarehouseMutationService _warehouseMutationOrderService,
                                                   IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail UnfinishObject(RollerWarehouseMutationDetail RollerWarehouseMutationDetail, IRollerWarehouseMutationService _warehouseMutationOrderService,
                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool DeleteObject(int Id);
    }
}