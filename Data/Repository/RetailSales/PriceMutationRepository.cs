using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class PriceMutationRepository : EfRepository<PriceMutation>, IPriceMutationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PriceMutationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<PriceMutation> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<PriceMutation> GetObjectsByIsActive(bool IsActive, int ExcludePriceMutationId, int ItemId, int GroupId)
        {
            return FindAll(x => x.IsActive == IsActive && x.ItemId == ItemId && x.GroupId == GroupId && x.Id != ExcludePriceMutationId).ToList();
        }

        public IList<PriceMutation> GetActiveObjectsByItemId(int ItemId)
        {
            return FindAll(x => x.ItemId == ItemId && x.IsActive).ToList();
        }

        public PriceMutation GetObjectById(int Id)
        {
            PriceMutation priceMutation = Find(x => x.Id == Id);
            if (priceMutation != null) { priceMutation.Errors = new Dictionary<string, string>(); }
            return priceMutation;
        }

        public PriceMutation CreateObject(PriceMutation priceMutation)
        {
            priceMutation.IsActive = true;
            return Create(priceMutation);
        }

        public PriceMutation DeactivateObject(PriceMutation priceMutation)
        {
            priceMutation.IsActive = false;
            Update(priceMutation);
            return priceMutation;
        }

        public bool DeleteObject(int Id)
        {
            PriceMutation priceMutation = Find(x => x.Id == Id);
            return (Delete(priceMutation) == 1) ? true : false;
        }
    }
}
