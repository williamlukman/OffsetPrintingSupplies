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
            ItemType itemType = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Accessories);
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
            return _repository.FindAll(i => i.Sku == Sku && !i.IsDeleted).FirstOrDefault();
        }

        public Item CreateObject(Item item, IItemTypeService _itemTypeService)
        {
            item.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(item, _itemTypeService, this) ? _repository.CreateObject(item) : item);
        }

        public Item UpdateObject(Item item, IItemTypeService _itemTypeService)
        {
            return (item = _validator.ValidUpdateObject(item, _itemTypeService, this) ? _repository.UpdateObject(item) : item);
        }

        public Item SoftDeleteObject(Item item, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                                     ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService)
        {
            return (item = _validator.ValidDeleteObject(item, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService) ? _repository.SoftDeleteObject(item) : item);
        }

        public Item AdjustQuantity(Item item, int quantity)
        {
            item.Quantity += quantity;
            return (item = _validator.ValidAdjustQuantity(item) ? _repository.UpdateObject(item) : item);  
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}