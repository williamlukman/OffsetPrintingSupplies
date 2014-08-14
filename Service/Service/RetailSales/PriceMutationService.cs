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
    public class PriceMutationService : IPriceMutationService
    {
        private IPriceMutationRepository _repository;
        private IPriceMutationValidator _validator;
        public PriceMutationService(IPriceMutationRepository _priceMutationRepository, IPriceMutationValidator _priceMutationValidator)
        {
            _repository = _priceMutationRepository;
            _validator = _priceMutationValidator;
        }

        public IPriceMutationValidator GetValidator()
        {
            return _validator;
        }

        public IList<PriceMutation> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PriceMutation> GetObjectsByIsActive(bool IsActive, int ItemId, int GroupId, int ExcludePriceMutationId)
        {
            return _repository.GetObjectsByIsActive(IsActive, ExcludePriceMutationId, ItemId, GroupId);
        }

        public IList<PriceMutation> GetActiveObjectsByItemId(int ItemId)
        {
            return _repository.GetActiveObjectsByItemId(ItemId);
        }

        public PriceMutation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PriceMutation DeactivateObject(PriceMutation priceMutation, Nullable<DateTime> DeactivationDate)
        {
            priceMutation.DeactivatedAt = DeactivationDate;
            return _repository.DeactivateObject(priceMutation);
        }

        public PriceMutation CreateObject(int ItemId, int GroupId, decimal Price, DateTime CreationDate)
        {
            PriceMutation priceMutation = new PriceMutation()
            {
                ItemId = ItemId,
                GroupId = GroupId,
                Amount = Price,
                CreatedAt = CreationDate
            };
            return this.CreateObject(priceMutation);
        }

        public PriceMutation CreateObject(Item item, Group group, DateTime CreationDate)
        {
            return this.CreateObject(item.Id, group.Id, item.SellingPrice, CreationDate);
        }

        public PriceMutation CreateObject(PriceMutation priceMutation)
        {
            priceMutation.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(priceMutation, this))
            {
                priceMutation = _repository.CreateObject(priceMutation);
                IList<PriceMutation> priceMutations = _repository.GetObjectsByIsActive(true, priceMutation.Id, priceMutation.ItemId, priceMutation.GroupId);
                foreach (var priceMutation2 in priceMutations)
                {
                    priceMutation2.Errors = new Dictionary<String, String>();
                    DeactivateObject(priceMutation2, priceMutation.CreatedAt);
                }
            }
            return priceMutation;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
