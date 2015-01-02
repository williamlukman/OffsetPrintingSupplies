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

        public IQueryable<Item> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Item> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<Item> GetQueryableAccessories(IItemService _itemService, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Accessory);
            IQueryable<Item> items = _repository.GetQueryableObjectsByItemTypeId(itemType.Id);
            return items;
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

        public Item CreateObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                 IWarehouseService _warehouseService, IPriceMutationService _priceMutationService)
        {
            item.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(item, _uomService, this, _itemTypeService))
            {
                item.CreatedAt = DateTime.Now;
                item = _repository.CreateObject(item);
                PriceMutation priceMutation = _priceMutationService.CreateObject(item, item.CreatedAt);
                item.PriceMutationId = priceMutation.Id;
                item = _repository.UpdateObject(item);
            }
            return item;
        }

        public Item CreateLegacyObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                       IWarehouseService _warehouseService, IPriceMutationService _priceMutationService)
        {
            item.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateLegacyObject(item, _uomService, this, _itemTypeService))
            {
                item = _repository.CreateObject(item);
                PriceMutation priceMutation = _priceMutationService.CreateObject(item, item.CreatedAt);
                item.PriceMutationId = priceMutation.Id;
                _repository.UpdateObject(item);
            }
            return item;
        }

        public Item UpdateObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService,
                                 IPriceMutationService _priceMutationService)
        {
            if (_validator.ValidUpdateObject(item, _uomService, this, _itemTypeService))
            {
                Item olditem = _repository.GetObjectById(item.Id);
                PriceMutation oldpriceMutation = _priceMutationService.GetObjectById(item.PriceMutationId);
                item.UpdatedAt = DateTime.Now;
                if (olditem.SellingPrice != item.SellingPrice)
                {
                    PriceMutation priceMutation = _priceMutationService.CreateObject(item, (DateTime)item.UpdatedAt);
                    item.PriceMutationId = priceMutation.Id;
                    _priceMutationService.DeactivateObject(oldpriceMutation, item.UpdatedAt);
                }
                item = _repository.UpdateObject(item);
            }
            return item;
        }

        public Item UpdateLegacyObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                       IWarehouseService _warehouseService, IBlanketService _blanketService, IContactService _contactService,
                                       IMachineService _machineService, IPriceMutationService _priceMutationService)
        {
            Blanket blanket = _blanketService.GetObjectById(item.Id);
            if (blanket != null)
            {
                _blanketService.UpdateObject(blanket, _blanketService, _uomService, this, _itemTypeService, _contactService,
                                             _machineService, _warehouseItemService, _warehouseService, _priceMutationService);
                return blanket;
            }

            if(_validator.ValidUpdateLegacyObject(item, _uomService, this, _itemTypeService)) 
            {
                Item olditem = _repository.GetObjectById(item.Id);
                PriceMutation oldpriceMutation = _priceMutationService.GetObjectById(item.PriceMutationId);
                if (olditem.SellingPrice != item.SellingPrice)
                {
                    DateTime priceMutationTimeStamp = DateTime.Now;
                    PriceMutation priceMutation = _priceMutationService.CreateObject(item, priceMutationTimeStamp);
                    item.PriceMutationId = priceMutation.Id;
                    _priceMutationService.DeactivateObject(oldpriceMutation, priceMutationTimeStamp);
                }
                item = _repository.UpdateObject(item);
            }
            return item;
        }

        public Item SoftDeleteObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                     IBlanketService _blanketService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                     ISalesOrderDetailService _salesOrderDetailService, IPriceMutationService _priceMutationService)
        {
            if (_validator.ValidDeleteObject(item, _stockMutationService, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService,
                                             _stockAdjustmentDetailService, _salesOrderDetailService, _blanketService))
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
                IList<PriceMutation> priceMutations = _priceMutationService.GetActiveObjectsByItemId(item.Id);
                foreach (var x in priceMutations)
                {
                    _priceMutationService.DeactivateObject(x, item.DeletedAt);
                }
            }
            return item;
        }

        public Item SoftDeleteLegacyObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                           IBlanketService _blanketService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                           ISalesOrderDetailService _salesOrderDetailService, IPriceMutationService _priceMutationService, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            Blanket blanket = _blanketService.GetObjectById(item.Id);
            if (blanket != null)
            {
                _blanketService.SoftDeleteObject(blanket, _itemTypeService, _warehouseItemService, _priceMutationService,
                                                 _purchaseOrderDetailService, _stockAdjustmentDetailService, _salesOrderDetailService,
                                                 _stockMutationService, _blanketOrderDetailService);
                return blanket;
            }

            if (_validator.ValidDeleteLegacyObject(item, _stockMutationService, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService, _salesOrderDetailService))
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
                IList<PriceMutation> priceMutations = _priceMutationService.GetActiveObjectsByItemId(item.Id);
                foreach (var priceMutation in priceMutations)
                {
                    _priceMutationService.DeactivateObject(priceMutation, item.DeletedAt);
                }
            }
            return item;
        }

        public Item AdjustCustomerQuantity(Item item, decimal quantity)
        {
            item.CustomerQuantity += (int) quantity;
            return (item = _validator.ValidAdjustCustomerQuantity(item) ? _repository.UpdateObject(item) : item);
        }

        //public Item AdjustCustomerVirtual(Item item, int quantity)
        //{
        //    item.CustomerVirtual += quantity;
        //    return (item = _validator.ValidAdjustCustomerVirtual(item) ? _repository.UpdateObject(item) : item);
        //}

        public Item AdjustQuantity(Item item, decimal quantity)
        {
            item.Quantity += quantity;
            return (item = _validator.ValidAdjustQuantity(item) ? _repository.UpdateObject(item) : item);
        }

        public Item AdjustPendingReceival(Item item, decimal quantity)
        {
            item.PendingReceival += quantity;
            return (item = _validator.ValidAdjustPendingReceival(item) ? _repository.UpdateObject(item) : item);
        }

        public Item AdjustPendingDelivery(Item item, decimal quantity)
        {
            item.PendingDelivery += quantity;
            return (item = _validator.ValidAdjustPendingDelivery(item) ? _repository.UpdateObject(item) : item);
        }

        public Item AdjustVirtual(Item item, decimal quantity)
        {
            item.Virtual += quantity;
            return (item = _validator.ValidAdjustVirtual(item) ? _repository.UpdateObject(item) : item);
        }

        public decimal CalculateAvgPrice(Item item, decimal addedQuantity, decimal addedAvgPrice)
        {
            // Use this function to calculate averagePrice
            decimal originalQuantity = item.Quantity + item.Virtual;
            decimal originalAvgPrice = item.AvgPrice;
            decimal avgPrice = (originalQuantity + addedQuantity == 0) ? 0 :
                ((originalQuantity * originalAvgPrice) + (addedQuantity * addedAvgPrice)) / (originalQuantity + addedQuantity);
            return avgPrice;
        }

        public decimal CalculateAndUpdateAvgPrice(Item item, decimal addedQuantity, decimal addedAvgPrice)
        {
            item.AvgPrice = CalculateAvgPrice(item, addedQuantity, addedAvgPrice);
            _repository.Update(item);
            return item.AvgPrice;
        }

        public decimal CalculateCustomerAvgPrice(Item item, decimal addedQuantity, decimal addedAvgPrice)
        {
            // Use this function to calculate averagePrice
            decimal originalQuantity = item.CustomerQuantity + item.CustomerVirtual;
            decimal originalAvgPrice = item.CustomerAvgPrice;
            decimal avgPrice = (originalQuantity + addedQuantity == 0) ? 0 :
                ((originalQuantity * originalAvgPrice) + (addedQuantity * addedAvgPrice)) / (originalQuantity + addedQuantity);
            return avgPrice;
        }

        public decimal CalculateAndUpdateCustomerAvgPrice(Item item, decimal addedQuantity, decimal addedAvgPrice)
        {
            item.CustomerAvgPrice = CalculateCustomerAvgPrice(item, addedQuantity, addedAvgPrice);
            _repository.Update(item);
            return item.CustomerAvgPrice;
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