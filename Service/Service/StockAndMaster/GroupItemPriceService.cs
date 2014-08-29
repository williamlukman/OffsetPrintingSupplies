using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Service.Service
{
    public class GroupItemPriceService : IGroupItemPriceService
    {
        private IGroupItemPriceRepository _repository;
        private IGroupItemPriceValidator _validator;
        public GroupItemPriceService(IGroupItemPriceRepository _groupItemPriceRepository, IGroupItemPriceValidator _groupItemPriceValidator)
        {
            _repository = _groupItemPriceRepository;
            _validator = _groupItemPriceValidator;
        }

        public IGroupItemPriceValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<GroupItemPrice> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<GroupItemPrice> GetAll()
        {
            return _repository.GetAll();
        }

        public GroupItemPrice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public GroupItemPrice CreateObject(GroupItemPrice groupItemPrice, IContactGroupService _contactGroupService, IItemService _itemService, IPriceMutationService _priceMutationService)
        {
            groupItemPrice.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(groupItemPrice, this, _contactGroupService, _itemService))
            {
                Item item = _itemService.GetObjectById(groupItemPrice.ItemId);
                groupItemPrice.CreatedAt = DateTime.Now;
                PriceMutation priceMutation = _priceMutationService.CreateObject(item.Id, /*groupItemPrice.ContactGroupId,*/ item.SellingPrice, groupItemPrice.CreatedAt);
                groupItemPrice.PriceMutationId = priceMutation.Id;
                groupItemPrice = _repository.CreateObject(groupItemPrice);
            }
            return groupItemPrice;
        }

        public GroupItemPrice UpdateObject(GroupItemPrice groupItemPrice, int oldGroupId, int oldItemId, IItemService _itemService, IPriceMutationService _priceMutationService)
        {
            if (_validator.ValidUpdateObject(groupItemPrice, oldGroupId, oldItemId, this, _priceMutationService))
            {
                Item item = _itemService.GetObjectById(groupItemPrice.ItemId);
                PriceMutation oldpriceMutation = _priceMutationService.GetObjectById(item.PriceMutationId);
                groupItemPrice.UpdatedAt = DateTime.Now;
                PriceMutation priceMutation = _priceMutationService.CreateObject(item.Id, /*groupItemPrice.ContactGroupId,*/ item.SellingPrice, (DateTime)groupItemPrice.UpdatedAt.GetValueOrDefault());
                groupItemPrice.PriceMutationId = priceMutation.Id;
                groupItemPrice = _repository.UpdateObject(groupItemPrice);
                _priceMutationService.DeactivateObject(oldpriceMutation, groupItemPrice.UpdatedAt);
            }
            return groupItemPrice;
        }

        public GroupItemPrice SoftDeleteObject(GroupItemPrice groupItemPrice)
        {
            return (groupItemPrice = _validator.ValidDeleteObject(groupItemPrice) ?
                    _repository.SoftDeleteObject(groupItemPrice) : groupItemPrice);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsGroupItemPriceDuplicated(GroupItemPrice groupItemPrice)
        {
            IQueryable<GroupItemPrice> groupItemPrices = _repository.FindAll(x => x.ItemId == groupItemPrice.ItemId && x.ContactGroupId == groupItemPrice.ContactGroupId && !x.IsDeleted && x.Id != groupItemPrice.Id);
            return (groupItemPrices.Count() > 0 ? true : false);
        }
    }
}
