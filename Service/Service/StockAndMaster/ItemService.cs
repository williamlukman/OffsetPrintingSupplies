using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public IList<Item> GetObjectsByUoMId(int UoMId)
        {
            return _repository.GetObjectsByUoMId(UoMId);
        }

        public Item GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Item GetObjectBySku(string Sku)
        {
            return _repository.GetObjectBySku(Sku);
        }

        public Item CreateObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService)
        {
            item.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(item, _uomService, this, _itemTypeService))
            {
                item = _repository.CreateObject(item);
            }
            return item;
        }

        public Item CreateLegacyObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService)
        {
            item.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateLegacyObject(item, _uomService, this, _itemTypeService))
            {
                item = _repository.CreateObject(item);
            }
            return item;
        }

        public Item UpdateObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService)
        {
            return (item = _validator.ValidUpdateObject(item, _uomService, this, _itemTypeService) ? _repository.UpdateObject(item) : item);
        }

        public Item UpdateLegacyObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                       IBarringService _barringService, IContactService _contactService, IMachineService _machineService)
        {
            Barring barring = _barringService.GetObjectById(item.Id);
            if (barring != null)
            {
                _barringService.UpdateObject(barring, _barringService, _uomService, this, _itemTypeService,
                                             _contactService, _machineService, _warehouseItemService, _warehouseService);
                return barring;
            }

            return (item = _validator.ValidUpdateLegacyObject(item, _uomService, this, _itemTypeService) ? _repository.UpdateObject(item) : item);
        }

        public Item SoftDeleteObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IBarringService _barringService)
        {
            if (_validator.ValidDeleteObject(item, _stockMutationService, _itemTypeService, _warehouseItemService))
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
            return item;
        }

        public Item SoftDeleteLegacyObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IBarringService _barringService)
        {
            Barring barring = _barringService.GetObjectById(item.Id);
            if (barring != null)
            {
                _barringService.SoftDeleteObject(barring, _itemTypeService, _warehouseItemService);
                return barring;
            }

            if (_validator.ValidDeleteLegacyObject(item, _stockMutationService, _itemTypeService, _warehouseItemService))
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
            return item;
        }

        public Item AdjustQuantity(Item item, int quantity)
        {
            item.Quantity += quantity;
            return (item = _validator.ValidAdjustQuantity(item) ? _repository.UpdateObject(item) : item);
        }

        public Item AdjustPendingReceival(Item item, int quantity)
        {
            item.PendingReceival += quantity;
            return (item = _validator.ValidAdjustPendingReceival(item) ? _repository.UpdateObject(item) : item);
        }

        public Item AdjustPendingDelivery(Item item, int quantity)
        {
            item.PendingDelivery += quantity;
            return (item = _validator.ValidAdjustPendingDelivery(item) ? _repository.UpdateObject(item) : item);
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