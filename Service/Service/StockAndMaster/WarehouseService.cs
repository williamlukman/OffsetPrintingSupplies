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
    public class WarehouseService : IWarehouseService
    {
        private IWarehouseRepository _repository;
        private IWarehouseValidator _validator;
        public WarehouseService(IWarehouseRepository _warehouseRepository, IWarehouseValidator _warehouseValidator)
        {
            _repository = _warehouseRepository;
            _validator = _warehouseValidator;
        }

        public IWarehouseValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Warehouse> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Warehouse> GetAll()
        {
            return _repository.GetAll();
        }

        public Warehouse GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Warehouse GetObjectByCode(string Code)
        {
            return _repository.GetObjectByCode(Code);
        }

        public Warehouse GetObjectByName(string Name)
        {
            return _repository.GetObjectByName(Name);
        }

        public Warehouse CreateObject(Warehouse warehouse, IWarehouseItemService _warehouseItemService, IItemService _itemService)
        {
            warehouse.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(warehouse, this))
            {
                warehouse = _repository.CreateObject(warehouse);
                // warehouse item will be created upon calling WarehouseItemService.FindOrCreateObject()
            }
            return warehouse;
        }

        public Warehouse UpdateObject(Warehouse warehouse)
        {
            return (warehouse = _validator.ValidUpdateObject(warehouse, this) ? _repository.UpdateObject(warehouse) : warehouse);
        }

        public Warehouse SoftDeleteObject(Warehouse warehouse, IWarehouseItemService _warehouseItemService, ICoreIdentificationService _coreIdentificationService, IBarringOrderService _barringOrderService)
        {
            if (_validator.ValidDeleteObject(warehouse, _warehouseItemService, _coreIdentificationService, _barringOrderService))
            {
                IList<WarehouseItem> allwarehouseitems = _warehouseItemService.GetObjectsByWarehouseId(warehouse.Id);
                foreach (var warehouseitem in allwarehouseitems)
                {
                    IWarehouseItemValidator warehouseItemValidator =  _warehouseItemService.GetValidator();
                    if (!warehouseItemValidator.ValidDeleteObject(warehouseitem))
                    {
                        warehouse.Errors.Add("Generic", "Tidak bisa menghapus item yang berhubungan dengan warehouse ini");
                        return warehouse;
                    }
                }
                foreach (var warehouseitem in allwarehouseitems)
                {
                    _warehouseItemService.SoftDeleteObject(warehouseitem);
                }
                _repository.SoftDeleteObject(warehouse);
            }
            return warehouse;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(Warehouse warehouse)
        {
            return _repository.IsCodeDuplicated(warehouse);
        }
    }
}