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
    public interface IItemService
    {
        IItemValidator GetValidator();
        IItemRepository GetRepository();
        IList<Item> GetAll();
        IList<Item> GetAllAccessories(IItemService _itemService, IItemTypeService _itemTypeService);
        IList<Item> GetObjectsByItemTypeId(int ItemTypeId);
        Item GetObjectById(int Id);
        Item GetObjectBySku(string Sku);
        Item CreateObject(Item item, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService);
        Item UpdateObject(Item item, IItemTypeService _itemTypeService);
        Item SoftDeleteObject(Item item, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IBarringService _barringService);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Item item);
    }
}