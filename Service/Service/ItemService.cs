using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ItemService : IItemService
    {
        private IItemRepository _repository;
        private IItemValidator _validator;
        public ItemService(IItemRepository _itemRepository, IItemValidator _itemValidator)
        {
            _repository = _itemRepository;
            _validator = _itemValidator;
        }

        public IItemValidator GetValidator()
        {
            return _validator;
        }

        public IItemRepository GetRepository()
        {
            return _repository;
        }

        public IList<Item> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Item> GetAllAccessories(IItemService _itemService, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Accessory);
            IList<Item> items = _repository.GetObjectsByItemTypeId(itemType.Id);
            return items.ToList();
        }

        public IList<Item> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return _repository.GetObjectsByItemTypeId(ItemTypeId);
        }

        public Item GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Item GetObjectBySku(string Sku)
        {
            return _repository.GetObjectBySku(Sku);
        }

        public Item CreateObject(Item item, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService)
        {
            item.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(item, this, _itemTypeService))
            {
                item = _repository.CreateObject(item);
                IList<Warehouse> allWarehouses = _warehouseService.GetAll();
                foreach (var warehouse in allWarehouses)
                {
                    WarehouseItem warehouseItem = new WarehouseItem()
                    {
                        WarehouseId = warehouse.Id,
                        ItemId = item.Id,
                        Quantity = 0,
                        Warehouse = warehouse,
                        Item = item
                    };
                    _warehouseItemService.GetRepository().CreateObject(warehouseItem);
                }
            }
            return item;
        }

        public Item UpdateObject(Item item, IItemTypeService _itemTypeService)
        {
            return (item = _validator.ValidUpdateObject(item, this, _itemTypeService) ? _repository.UpdateObject(item) : item);
        }

        public Item SoftDeleteObject(Item item, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                     IBarringService _barringService)
        {
            if (item.GetType() == typeof(Barring))
            {
                _barringService.SoftDeleteObject((Barring)item, _warehouseItemService);
            }
            else
            {
                if (_validator.ValidDeleteObject(item, _recoveryAccessoryDetailService, _itemTypeService, _warehouseItemService))
                {
                    IList<WarehouseItem> allwarehouseitems = _warehouseItemService.GetObjectsByItemId(item.Id);
                    foreach (var warehouseitem in allwarehouseitems)
                    {
                        IWarehouseItemValidator warehouseItemValidator = _warehouseItemService.GetValidator();
                        if (!warehouseItemValidator.ValidDeleteObject(warehouseitem))
                        {
                            item.Errors.Add("Generic", "Tidak bisa menghapus item yang berhubungan dengan warehouse");
                            return item;
                        }
                    }
                    foreach (var warehouseitem in allwarehouseitems)
                    {
                        _warehouseItemService.SoftDeleteObject(warehouseitem);
                    }
                    _repository.SoftDeleteObject(item);
                }
            }
            return item;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsSkuDuplicated(Item item)
        {
            return _repository.IsSkuDuplicated(item);
        }
    }
}