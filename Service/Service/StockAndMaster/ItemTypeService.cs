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
    public class ItemTypeService : IItemTypeService
    {
        private IItemTypeRepository _repository;
        private IItemTypeValidator _validator;
        public ItemTypeService(IItemTypeRepository _itemTypeRepository, IItemTypeValidator _itemTypeValidator)
        {
            _repository = _itemTypeRepository;
            _validator = _itemTypeValidator;
        }

        public IItemTypeValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ItemType> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ItemType> GetAll()
        {
            return _repository.GetAll();
        }

        public ItemType GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ItemType GetObjectByName(string name)
        {
            return _repository.GetObjectByName(name);
        }

        public ItemType CreateObject(string Name, string Description, IAccountService _accountService)
        {
            ItemType itemType = new ItemType
            {
                Name = Name,
                Description = Description
            };
            return this.CreateObject(itemType, _accountService);
        }

        public ItemType CreateObject(string Name, string Description, bool IsLegacy, IAccountService _accountService)
        {
            ItemType itemType = new ItemType
            {
                Name = Name,
                Description = Description,
                IsLegacy = IsLegacy
            };
            return this.CreateObject(itemType, _accountService);
        }

        public ItemType CreateObject(string Name, string Description, bool IsLegacy, Account account, IAccountService _accountService)
        {
            ItemType itemType = new ItemType
            {
                Name = Name,
                Description = Description,
                IsLegacy = IsLegacy,
                AccountId = account.Id
            };
            return this.CreateObject(itemType, _accountService);
        }

        public ItemType CreateObject(ItemType itemType, IAccountService _accountService)
        {
            itemType.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(itemType, this, _accountService) ? _repository.CreateObject(itemType) : itemType);
        }

        public ItemType UpdateObject(ItemType itemType, IAccountService _accountService)
        {
            return (itemType = _validator.ValidUpdateObject(itemType, this, _accountService) ? _repository.UpdateObject(itemType) : itemType);
        }

        public ItemType SoftDeleteObject(ItemType itemType, IItemService _itemService)
        {
            return (itemType = _validator.ValidDeleteObject(itemType, _itemService) ? _repository.SoftDeleteObject(itemType) : itemType);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(ItemType itemType)
        {
            IQueryable<ItemType> types = _repository.FindAll(x => x.Name == itemType.Name && !x.IsDeleted && x.Id != itemType.Id);
            return (types.Count() > 0 ? true : false);
        }

    }
}