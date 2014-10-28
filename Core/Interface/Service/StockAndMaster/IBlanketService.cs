using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBlanketService
    {
        IBlanketValidator GetValidator();
        IBlanketRepository GetRepository();
        IQueryable<Blanket> GetQueryable();
        IList<Blanket> GetAll();
        IList<Blanket> GetObjectsByItemTypeId(int ItemTypeId);
        IList<Blanket> GetObjectsByUoMId(int UoMId);
        IList<Blanket> GetObjectsByMachineId(int machineId);
        IList<Blanket> GetObjectsByContactId(int contactId);
        IList<Blanket> GetObjectsByRollBlanketItemId(int rollBlanketItemId);
        IList<Blanket> GetObjectsByLeftBarItemId(int leftBarItemId);
        IList<Blanket> GetObjectsByRightBarItemId(int rightBarItemId);
        Item GetRollBlanketItem(Blanket blanket);
        Item GetLeftBarItem(Blanket blanket);
        Item GetRightBarItem(Blanket blanket);
        Blanket GetObjectById(int Id);
        Blanket GetObjectBySku(string Sku);
        Blanket CreateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                             IContactService _contactService, IMachineService _machineService, IWarehouseItemService _warehouseItemService,
                             IWarehouseService _warehouseService, IPriceMutationService _priceMutationService);
        Blanket UpdateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService,
                               IItemTypeService _itemTypeService, IContactService _contactService, IMachineService _machineService,
                               IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IPriceMutationService _priceMutationService);
        Blanket SoftDeleteObject(Blanket blanket, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                 IPriceMutationService _priceMutationService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                 IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                 IStockMutationService _stockMutationService, IBlanketOrderDetailService _blanketOrderDetailService);
        Blanket AdjustQuantity(Blanket blanket, int quantity);
        Blanket AdjustPendingReceival(Blanket blanket, int quantity);
        Blanket AdjustPendingDelivery(Blanket blanket, int quantity);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Blanket blanket);
    }
}