using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class RepackingRepository : EfRepository<Repacking>, IRepackingRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public RepackingRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Repacking> GetQueryable()
        {
            return FindAll();
        }

        public IList<Repacking> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<Repacking> GetObjectsByBlendingRecipeId(int BlendingRecipeId)
        {
            return FindAll(x => x.BlendingRecipeId == BlendingRecipeId && !x.IsDeleted).ToList();
        }

        public Repacking GetObjectById(int Id)
        {
            Repacking repacking = Find(x => x.Id == Id && !x.IsDeleted);
            if (repacking != null) { repacking.Errors = new Dictionary<string, string>(); }
            return repacking;
        }

        public Repacking CreateObject(Repacking repacking)
        {
            repacking.IsConfirmed = false;
            repacking.IsDeleted = false;
            repacking.CreatedAt = DateTime.Now;
            return Create(repacking);
        }

        public Repacking UpdateObject(Repacking repacking)
        {
            repacking.UpdatedAt = DateTime.Now;
            Update(repacking);
            return repacking;
        }

        public Repacking SoftDeleteObject(Repacking repacking)
        {
            repacking.IsDeleted = true;
            repacking.DeletedAt = DateTime.Now;
            Update(repacking);
            return repacking;
        }

        public Repacking ConfirmObject(Repacking repacking)
        {
            repacking.IsConfirmed = true;
            Update(repacking);
            return repacking;
        }

        public Repacking UnconfirmObject(Repacking repacking)
        {
            repacking.IsConfirmed = false;
            repacking.ConfirmationDate = null;
            UpdateObject(repacking);
            return repacking;
        }

        public Repacking AdjustQuantity(Repacking repacking)
        {
            return UpdateObject(repacking);
        }

        public bool DeleteObject(int Id)
        {
            Repacking repacking = Find(x => x.Id == Id);
            return (Delete(repacking) == 1) ? true : false;
        }

        public bool IsCodeDuplicated(Repacking repacking)
        {
            IQueryable<Repacking> orders = FindAll(x => x.Code == repacking.Code && !x.IsDeleted && x.Id != repacking.Id);
            return (orders.Count() > 0 ? true : false);
        }

    }
}