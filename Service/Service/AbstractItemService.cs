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
    public class AbstractItemService : IAbstractItemService
    {
        private IAbstractItemRepository _repository;
        private IAbstractItemValidator _validator;
        public AbstractItemService(IAbstractItemRepository _abstractItemRepository, IAbstractItemValidator _abstractItemValidator)
        {
            _repository = _abstractItemRepository;
            _validator = _abstractItemValidator;
        }

        public IAbstractItemValidator GetValidator()
        {
            return _validator;
        }

        public IAbstractItemRepository GetRepository()
        {
            return _repository;
        }

        public IList<AbstractItem> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<AbstractItem> GetAllAccessories(IAbstractItemService _abstractItemService, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Accessory);
            IList<AbstractItem> items = _repository.GetObjectsByItemTypeId(itemType.Id);
            return items.ToList();
        }

        public IList<AbstractItem> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return _repository.GetObjectsByItemTypeId(ItemTypeId);
        }

        public AbstractItem GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public AbstractItem GetObjectBySku(string Sku)
        {
            return _repository.GetObjectBySku(Sku);
        }

        public int GetQuantityById(int Id)
        {
            return _repository.GetQuantityById(Id);
        }

        public AbstractItem CreateObject(AbstractItem item, IItemTypeService _itemTypeService)
        {
            item.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(item, this, _itemTypeService) ? _repository.CreateObject(item) : item);
        }

        public AbstractItem UpdateObject(AbstractItem item, IItemTypeService _itemTypeService)
        {
            return (item = _validator.ValidUpdateObject(item, this, _itemTypeService) ? _repository.UpdateObject(item) : item);
        }

        public AbstractItem SoftDeleteObject(AbstractItem item)
        {
            return (item = _validator.ValidDeleteObject(item) ? _repository.SoftDeleteObject(item) : item);
        }

        public AbstractItem AdjustQuantity(AbstractItem item, int quantity)
        {
            item.Quantity += quantity;
            return (item = _validator.ValidAdjustQuantity(item) ? _repository.UpdateObject(item) : item);  
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsSkuDuplicated(AbstractItem item)
        {
            return _repository.IsSkuDuplicated(item);
        }
    }
}