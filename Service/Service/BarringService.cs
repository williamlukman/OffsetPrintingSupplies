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
    public class BarringService : IBarringService
    {
        private IBarringRepository _repository;
        private IBarringValidator _validator;
        public BarringService(IBarringRepository _barringRepository, IBarringValidator _barringValidator)
        {
            _repository = _barringRepository;
            _validator = _barringValidator;
        }

        public IBarringValidator GetValidator()
        {
            return _validator;
        }

        public IBarringRepository GetRepository()
        {
            return _repository;
        }

        public IList<Barring> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Barring> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return _repository.GetObjectsByItemTypeId(ItemTypeId);
        }

        public IList<Barring> GetObjectsByMachineId(int MachineId)
        {
            return _repository.GetObjectsByMachineId(MachineId);
        }

        public IList<Barring> GetObjectsByCustomerId(int CustomerId)
        {
            return _repository.GetObjectsByCustomerId(CustomerId);
        }

        public Barring GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Barring GetObjectBySku(string Sku)
        {
            return _repository.GetObjectBySku(Sku);
        }

        public Barring CreateObject(Barring barring, IBarringService _barringService, IItemService _itemService, IItemTypeService _itemTypeService,
                                    ICustomerService _customerService, IMachineService _machineService,
                                    IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService)
        {
            barring.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(barring, _barringService, _itemService, _itemTypeService, _customerService, _machineService) ? _repository.CreateObject(barring) : barring);
        }

        public Barring UpdateObject(Barring barring, IBarringService _barringService, IItemService _itemService, IItemTypeService _itemTypeService,
                                    ICustomerService _customerService, IMachineService _machineService,
                                    IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService)
        {        
            return (barring = _validator.ValidUpdateObject(barring, _barringService, _itemService, _itemTypeService, _customerService, _machineService) ? _repository.UpdateObject(barring) : barring);
        }

        public Barring SoftDeleteObject(Barring barring, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidDeleteObject(barring, _warehouseItemService))
            {
                IList<WarehouseItem> allwarehouseitems = _warehouseItemService.GetObjectsByItemId(barring.Id);
                foreach (var warehouseitem in allwarehouseitems)
                {
                    IWarehouseItemValidator warehouseItemValidator = _warehouseItemService.GetValidator();
                    if (!warehouseItemValidator.ValidDeleteObject(warehouseitem))
                    {
                        barring.Errors.Add("Generic", "Tidak bisa menghapus item yang berhubungan dengan warehouse");
                        return barring;
                    }
                }
                foreach (var warehouseitem in allwarehouseitems)
                {
                    _warehouseItemService.SoftDeleteObject(warehouseitem);
                }
                _repository.SoftDeleteObject(barring);
            }
            return barring;
        }

        public Barring AdjustQuantity(Barring barring, int quantity)
        {
            barring.Quantity += quantity;
            return (barring = _validator.ValidAdjustQuantity(barring) ? _repository.UpdateObject(barring) : barring);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsSkuDuplicated(Barring barring)
        {
            return _repository.IsSkuDuplicated(barring);
        }
    }
}