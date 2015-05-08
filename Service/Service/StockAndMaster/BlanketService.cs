using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;

namespace Service.Service
{
    public class BlanketService : IBlanketService
    {
        private IBlanketRepository _repository;
        private IBlanketValidator _validator;
        public BlanketService(IBlanketRepository _blanketRepository, IBlanketValidator _blanketValidator)
        {
            _repository = _blanketRepository;
            _validator = _blanketValidator;
        }

        public IBlanketValidator GetValidator()
        {
            return _validator;
        }

        public IBlanketRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<Blanket> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Blanket> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Blanket> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return _repository.GetObjectsByItemTypeId(ItemTypeId);
        }

        public IList<Blanket> GetObjectsByUoMId(int UoMId)
        {
            return _repository.GetObjectsByUoMId(UoMId);
        }
        
        public IList<Blanket> GetObjectsByMachineId(int MachineId)
        {
            return _repository.GetObjectsByMachineId(MachineId);
        }

        public IList<Blanket> GetObjectsByContactId(int ContactId)
        {
            return _repository.GetObjectsByContactId(ContactId);
        }

        public IList<Blanket> GetObjectsByRollBlanketItemId(int RollBlanketItemId)
        {
            return _repository.GetObjectsByRollBlanketItemId(RollBlanketItemId);
        }

        public IList<Blanket> GetObjectsByLeftBarItemId(int LeftBarItemId)
        {
            return _repository.GetObjectsByLeftBarItemId(LeftBarItemId);
        }

        public IList<Blanket> GetObjectsByRightBarItemId(int RightBarItemId)
        {
            return _repository.GetObjectsByRightBarItemId(RightBarItemId);
        }

        public Item GetRollBlanketItem(Blanket blanket)
        {
            return _repository.GetRollBlanketItem(blanket);
        }

        public Item GetLeftBarItem(Blanket blanket)
        {
            return _repository.GetLeftBarItem(blanket);
        }

        public Item GetRightBarItem(Blanket blanket)
        {
            return _repository.GetRightBarItem(blanket);
        }

        public Blanket GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Blanket GetObjectBySku(string Sku)
        {
            return _repository.GetObjectBySku(Sku);
        }

        public Blanket CreateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                    IContactService _contactService, IMachineService _machineService, IWarehouseItemService _warehouseItemService,
                                    IWarehouseService _warehouseService, IPriceMutationService _priceMutationService)
        {
            blanket.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(blanket, _blanketService, _uomService, _itemService, _itemTypeService, _contactService, _machineService))
            {
                if (blanket.CroppingType == Constant.CroppingType.NO)
                {
                    blanket.LeftOverAC = 0;
                    blanket.LeftOverAR = 0;
                    blanket.Special = 0;
                }
                blanket = _repository.CreateObject(blanket);
                PriceMutation priceMutation = _priceMutationService.CreateObject(blanket, blanket.CreatedAt);
                blanket.PriceMutationId = priceMutation.Id;
                blanket = _repository.UpdateObject(blanket);
            }
            return blanket;
        }

        public Blanket UpdateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService,
                                    IItemTypeService _itemTypeService, IContactService _contactService, IMachineService _machineService,
                                    IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IPriceMutationService _priceMutationService)
        {
            if (_validator.ValidUpdateObject(blanket, _blanketService, _uomService, _itemService, _itemTypeService, _contactService, _machineService))
            {
                Blanket oldblanket = _repository.GetObjectById(blanket.Id);
                PriceMutation oldpriceMutation = _priceMutationService.GetObjectById(blanket.PriceMutationId);
                if (oldblanket.SellingPrice != blanket.SellingPrice)
                {
                    DateTime priceMutationTimeStamp = DateTime.Now;
                    PriceMutation priceMutation = _priceMutationService.CreateObject(oldblanket, priceMutationTimeStamp);
                    blanket.PriceMutationId = priceMutation.Id;
                    _priceMutationService.DeactivateObject(oldpriceMutation, priceMutationTimeStamp);
                }
                if (blanket.CroppingType == Constant.CroppingType.NO)
                {
                    blanket.LeftOverAC = 0;
                    blanket.LeftOverAR = 0;
                    blanket.Special = 0;
                }
                _repository.UpdateObject(blanket);
            }
            return blanket;
        }

        public Blanket SoftDeleteObject(Blanket blanket, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                        IPriceMutationService _priceMutationService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                        IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                        IStockMutationService _stockMutationService, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            if (_validator.ValidDeleteObject(blanket, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService,
                                             _salesOrderDetailService, _stockMutationService, _blanketOrderDetailService))
            {
                IList<WarehouseItem> allwarehouseitems = _warehouseItemService.GetObjectsByItemId(blanket.Id);
                foreach (var warehouseitem in allwarehouseitems)
                {
                    IWarehouseItemValidator warehouseItemValidator = _warehouseItemService.GetValidator();
                    if (!warehouseItemValidator.ValidDeleteObject(warehouseitem))
                    {
                        blanket.Errors.Add("Generic", "Tidak bisa menghapus item yang berhubungan dengan warehouse");
                        return blanket;
                    }
                }
                foreach (var warehouseitem in allwarehouseitems)
                {
                    _warehouseItemService.SoftDeleteObject(warehouseitem);
                }
                _repository.SoftDeleteObject(blanket);
                IList<PriceMutation> priceMutations = _priceMutationService.GetActiveObjectsByItemId(blanket.Id);
                foreach (var priceMutation in priceMutations)
                {
                    _priceMutationService.DeactivateObject(priceMutation, blanket.DeletedAt);
                }
            }
            return blanket;
        }

        public Blanket AdjustQuantity(Blanket blanket, decimal quantity)
        {
            blanket.Quantity += quantity;
            return (blanket = _validator.ValidAdjustQuantity(blanket) ? _repository.UpdateObject(blanket) : blanket);
        }

        public Blanket AdjustPendingReceival(Blanket blanket, int quantity)
        {
            blanket.PendingReceival += quantity;
            return (blanket = _validator.ValidAdjustPendingReceival(blanket) ? _repository.UpdateObject(blanket) : blanket);
        }

        public Blanket AdjustPendingDelivery(Blanket blanket, int quantity)
        {
            blanket.PendingDelivery += quantity;
            return (blanket = _validator.ValidAdjustPendingDelivery(blanket) ? _repository.UpdateObject(blanket) : blanket);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsSkuDuplicated(Blanket blanket)
        {
            return _repository.IsSkuDuplicated(blanket);
        }
    }
}