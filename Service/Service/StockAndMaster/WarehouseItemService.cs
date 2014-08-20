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
    public class WarehouseItemService : IWarehouseItemService
    {
        private IWarehouseItemRepository _repository;
        private IWarehouseItemValidator _validator;
        public WarehouseItemService(IWarehouseItemRepository _warehouseItemRepository, IWarehouseItemValidator _warehouseItemValidator)
        {
            _repository = _warehouseItemRepository;
            _validator = _warehouseItemValidator;
        }

        public IWarehouseItemValidator GetValidator()
        {
            return _validator;
        }

        public IWarehouseItemRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<WarehouseItem> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<WarehouseItem> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<WarehouseItem> GetObjectsByWarehouseId(int warehouseId)
        {
            return _repository.GetObjectsByWarehouseId(warehouseId);
        }

        public IList<WarehouseItem> GetObjectsByItemId(int ItemId)
        {
            return _repository.GetObjectsByItemId(ItemId);
        }

        public WarehouseItem GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public WarehouseItem FindOrCreateObject(int warehouseId, int itemId)
        {
            return _repository.FindOrCreateObject(warehouseId, itemId);
        }

        public WarehouseItem AddObject(Warehouse warehouse, Item item, IWarehouseService _warehouseService, IItemService _itemService)
        {
            WarehouseItem warehouseItem = new WarehouseItem()
            {
                WarehouseId = warehouse.Id,
                ItemId = item.Id,
                Quantity = 0,
                Warehouse = warehouse,
                Item = item
            };
            return CreateObject(warehouseItem, _warehouseService, _itemService);
        }

        public WarehouseItem CreateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService)
        {
            warehouseItem.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(warehouseItem, _warehouseService, _itemService) ? _repository.CreateObject(warehouseItem) : warehouseItem);
        }

        public WarehouseItem UpdateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService)
        {
            return (warehouseItem = _validator.ValidUpdateObject(warehouseItem, _warehouseService, _itemService) ? _repository.UpdateObject(warehouseItem) : warehouseItem);
        }

        public WarehouseItem SoftDeleteObject(WarehouseItem warehouseItem)
        {
            return (warehouseItem = _validator.ValidDeleteObject(warehouseItem) ? _repository.SoftDeleteObject(warehouseItem) : warehouseItem);
        }

        public WarehouseItem AdjustQuantity(WarehouseItem warehouseItem, int quantity)
        {
            warehouseItem.Quantity += quantity;
            if (_validator.ValidAdjustQuantity(warehouseItem)) 
            {
                _repository.UpdateObject(warehouseItem);
            }
            return warehouseItem;
        }

        public WarehouseItem AdjustPendingReceival(WarehouseItem warehouseItem, int quantity)
        {
            warehouseItem.PendingReceival += quantity;
            if (_validator.ValidAdjustPendingReceival(warehouseItem))
            {
                _repository.UpdateObject(warehouseItem);
            }
            return warehouseItem;
        }

        public WarehouseItem AdjustPendingDelivery(WarehouseItem warehouseItem, int quantity)
        {
            warehouseItem.PendingDelivery += quantity;
            if (_validator.ValidAdjustPendingDelivery(warehouseItem))
            {
                _repository.UpdateObject(warehouseItem);
            }
            return warehouseItem;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}