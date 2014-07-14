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

        public WarehouseItem GetObjectByWarehouseAndItem(int warehouseId, int abstractItemId)
        {
            return _repository.GetObjectByWarehouseAndItem(warehouseId, abstractItemId);
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

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}