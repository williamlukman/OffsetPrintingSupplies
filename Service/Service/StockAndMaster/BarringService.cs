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

        public IQueryable<Barring> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Barring> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Barring> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return _repository.GetObjectsByItemTypeId(ItemTypeId);
        }

        public IList<Barring> GetObjectsByUoMId(int UoMId)
        {
            return _repository.GetObjectsByUoMId(UoMId);
        }
        
        public IList<Barring> GetObjectsByMachineId(int MachineId)
        {
            return _repository.GetObjectsByMachineId(MachineId);
        }

        public IList<Barring> GetObjectsByContactId(int ContactId)
        {
            return _repository.GetObjectsByContactId(ContactId);
        }

        public IList<Barring> GetObjectsByBlanketItemId(int BlanketItemId)
        {
            return _repository.GetObjectsByBlanketItemId(BlanketItemId);
        }

        public IList<Barring> GetObjectsByLeftBarItemId(int LeftBarItemId)
        {
            return _repository.GetObjectsByLeftBarItemId(LeftBarItemId);
        }

        public IList<Barring> GetObjectsByRightBarItemId(int RightBarItemId)
        {
            return _repository.GetObjectsByRightBarItemId(RightBarItemId);
        }

        public Item GetBlanketItem(Barring barring)
        {
            return _repository.GetBlanketItem(barring);
        }

        public Item GetLeftBarItem(Barring barring)
        {
            return _repository.GetLeftBarItem(barring);
        }

        public Item GetRightBarItem(Barring barring)
        {
            return _repository.GetRightBarItem(barring);
        }

        public Barring GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Barring GetObjectBySku(string Sku)
        {
            return _repository.GetObjectBySku(Sku);
        }

        public Barring CreateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                    IContactService _contactService, IMachineService _machineService,
                                    IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                    IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService)
        {
            barring.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(barring, _barringService, _uomService, _itemService, _itemTypeService, _contactService, _machineService))
            {
                ContactGroup contactGroup = _contactGroupService.GetObjectByIsLegacy(true);
                barring = _repository.CreateObject(barring);
                PriceMutation priceMutation = _priceMutationService.CreateObject(barring, contactGroup, barring.CreatedAt);
                barring.PriceMutationId = priceMutation.Id;
                barring = _repository.UpdateObject(barring);
            }
            return barring;
        }

        public Barring UpdateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService,
                                    IItemTypeService _itemTypeService, IContactService _contactService, IMachineService _machineService,
                                    IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                    IContactGroupService _contactGroupService, IPriceMutationService _priceMutationService)
        {
            if (_validator.ValidUpdateObject(barring, _barringService, _uomService, _itemService, _itemTypeService, _contactService, _machineService))
            {
                ContactGroup contactGroup = _contactGroupService.GetObjectByIsLegacy(true);
                Barring oldbarring = _repository.GetObjectById(barring.Id);
                PriceMutation oldpriceMutation = _priceMutationService.GetObjectById(barring.PriceMutationId);
                if (oldbarring.SellingPrice != barring.SellingPrice)
                {
                    DateTime priceMutationTimeStamp = DateTime.Now;
                    PriceMutation priceMutation = _priceMutationService.CreateObject(oldbarring, contactGroup, priceMutationTimeStamp);
                    barring.PriceMutationId = priceMutation.Id;
                    _priceMutationService.DeactivateObject(oldpriceMutation, priceMutationTimeStamp);
                }
                _repository.UpdateObject(barring);
            }
            return barring;
        }

        public Barring SoftDeleteObject(Barring barring, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                        IPriceMutationService _priceMutationService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                        IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                        IStockMutationService _stockMutationService, IBarringOrderDetailService _barringOrderDetailService)
        {
            if (_validator.ValidDeleteObject(barring, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService,
                                             _salesOrderDetailService, _stockMutationService, _barringOrderDetailService))
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
                IList<PriceMutation> priceMutations = _priceMutationService.GetActiveObjectsByItemId(barring.Id);
                foreach (var priceMutation in priceMutations)
                {
                    _priceMutationService.DeactivateObject(priceMutation, barring.DeletedAt);
                }
            }
            return barring;
        }

        public Barring AdjustQuantity(Barring barring, int quantity)
        {
            barring.Quantity += quantity;
            return (barring = _validator.ValidAdjustQuantity(barring) ? _repository.UpdateObject(barring) : barring);
        }

        public Barring AdjustPendingReceival(Barring barring, int quantity)
        {
            barring.PendingReceival += quantity;
            return (barring = _validator.ValidAdjustPendingReceival(barring) ? _repository.UpdateObject(barring) : barring);
        }

        public Barring AdjustPendingDelivery(Barring barring, int quantity)
        {
            barring.PendingDelivery += quantity;
            return (barring = _validator.ValidAdjustPendingDelivery(barring) ? _repository.UpdateObject(barring) : barring);
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