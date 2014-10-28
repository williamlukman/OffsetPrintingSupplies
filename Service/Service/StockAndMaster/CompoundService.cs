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
    public class CompoundService : ICompoundService
    {
        private ICompoundRepository _repository;
        private ICompoundValidator _validator;
        public CompoundService(ICompoundRepository _compoundRepository, ICompoundValidator _compoundValidator)
        {
            _repository = _compoundRepository;
            _validator = _compoundValidator;
        }

        public ICompoundValidator GetValidator()
        {
            return _validator;
        }

        public ICompoundRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<Compound> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Compound> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Compound> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return _repository.GetObjectsByItemTypeId(ItemTypeId);
        }

        public IList<Compound> GetObjectsByUoMId(int UoMId)
        {
            return _repository.GetObjectsByUoMId(UoMId);
        }
        
        public Compound GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Compound GetObjectBySku(string Sku)
        {
            return _repository.GetObjectBySku(Sku);
        }

        public Compound CreateObject(Compound compound, ICompoundService _compoundService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                    IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IPriceMutationService _priceMutationService)
        {
            compound.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(compound, _compoundService, _uomService, _itemService, _itemTypeService))
            {
                compound = _repository.CreateObject(compound);
                PriceMutation priceMutation = _priceMutationService.CreateObject(compound, compound.CreatedAt);
                compound.PriceMutationId = priceMutation.Id;
                compound = _repository.UpdateObject(compound);
            }
            return compound;
        }

        public Compound UpdateObject(Compound compound, IUoMService _uomService, IItemTypeService _itemTypeService, IItemService _itemService,
                                     IPriceMutationService _priceMutationService)
        {
            if (_validator.ValidUpdateObject(compound, this, _uomService, _itemService, _itemTypeService))
            {
                Compound oldcompound = _repository.GetObjectById(compound.Id);
                PriceMutation oldpriceMutation = _priceMutationService.GetObjectById(compound.PriceMutationId);
                if (oldcompound.SellingPrice != compound.SellingPrice)
                {
                    DateTime priceMutationTimeStamp = DateTime.Now;
                    PriceMutation priceMutation = _priceMutationService.CreateObject(oldcompound, priceMutationTimeStamp);
                    compound.PriceMutationId = priceMutation.Id;
                    _priceMutationService.DeactivateObject(oldpriceMutation, priceMutationTimeStamp);
                }
                _repository.UpdateObject(compound);
            }
            return compound;
        }

        public Compound SoftDeleteObject(Compound compound, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                        IPriceMutationService _priceMutationService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                        IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                        IStockMutationService _stockMutationService)
        {
            if (_validator.ValidDeleteObject(compound, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService,
                                             _salesOrderDetailService, _stockMutationService))
            {
                IList<WarehouseItem> allwarehouseitems = _warehouseItemService.GetObjectsByItemId(compound.Id);
                foreach (var warehouseitem in allwarehouseitems)
                {
                    IWarehouseItemValidator warehouseItemValidator = _warehouseItemService.GetValidator();
                    if (!warehouseItemValidator.ValidDeleteObject(warehouseitem))
                    {
                        compound.Errors.Add("Generic", "Tidak bisa menghapus item yang berhubungan dengan warehouse");
                        return compound;
                    }
                }
                foreach (var warehouseitem in allwarehouseitems)
                {
                    _warehouseItemService.SoftDeleteObject(warehouseitem);
                }
                _repository.SoftDeleteObject(compound);
                IList<PriceMutation> priceMutations = _priceMutationService.GetActiveObjectsByItemId(compound.Id);
                foreach (var priceMutation in priceMutations)
                {
                    _priceMutationService.DeactivateObject(priceMutation, compound.DeletedAt);
                }
            }
            return compound;
        }

        public Compound AdjustQuantity(Compound compound, int quantity)
        {
            compound.Quantity += quantity;
            return (compound = _validator.ValidAdjustQuantity(compound) ? _repository.UpdateObject(compound) : compound);
        }

        public Compound AdjustPendingReceival(Compound compound, int quantity)
        {
            compound.PendingReceival += quantity;
            return (compound = _validator.ValidAdjustPendingReceival(compound) ? _repository.UpdateObject(compound) : compound);
        }

        public Compound AdjustPendingDelivery(Compound compound, int quantity)
        {
            compound.PendingDelivery += quantity;
            return (compound = _validator.ValidAdjustPendingDelivery(compound) ? _repository.UpdateObject(compound) : compound);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsSkuDuplicated(Compound compound)
        {
            return _repository.IsSkuDuplicated(compound);
        }
    }
}