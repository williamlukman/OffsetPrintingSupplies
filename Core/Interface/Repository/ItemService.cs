using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
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

        public IList<Item> GetAll()
        {
            return _repository.GetAll();
        }

        public Item GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Item GetObjectBySku(string Sku)
        {
            return _repository.GetObjectBySku(Sku);
        }

        public Item GetObjectByName(string Name)
        {
            return _repository.Find(i => i.Name == Name && !i.IsDeleted);
        }

        public Item CreateObject(Item item)
        {
            item.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(item, this) ? _repository.CreateObject(item) : item);
        }

        public Item CreateObject(string name, string description, string Sku)
        {
            Item item = new Item
            {
                Name = name,
                Description = description,
                Sku = Sku
            };
            return this.CreateObject(item);
        }

        public Item UpdateObject(Item item)
        {
            return (_validator.ValidUpdateObject(item, this) ? _repository.UpdateObject(item) : item);
        }

        public Item SoftDeleteObject(Item item, IStockMutationService _stockMutationService)
        {
            return (_validator.ValidDeleteObject(item, _stockMutationService) ? _repository.SoftDeleteObject(item) : item);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsSkuDuplicated(Item item)
        {
            IQueryable<Item> items = _repository.FindAll(i => i.Sku == item.Sku && !i.IsDeleted && i.Id != item.Id);
            return (items.Count() > 0 ? true : false);
        }

        public decimal CalculateAvgCost(Item item, int addedQuantity, decimal addedAvgCost)
        {
            decimal AvgCost = _repository.CalculateAvgCost(item, addedQuantity, addedAvgCost);
            return AvgCost;
        }
    }
}