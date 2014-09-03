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
    public class CoreAccessoryDetailRepository : EfRepository<CoreAccessoryDetail>, ICoreAccessoryDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CoreAccessoryDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CoreAccessoryDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<CoreAccessoryDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<CoreAccessoryDetail> GetObjectsByCoreIdentificationDetailId(int CoreIdentificationDetailId)
        {
            return FindAll(x => x.CoreIdentificationDetailId == CoreIdentificationDetailId && !x.IsDeleted).ToList();
        }

        public IList<CoreAccessoryDetail> GetObjectsByItemId(int ItemId)
        {
            return FindAll(x => x.ItemId == ItemId && !x.IsDeleted).ToList();
        }

        public CoreAccessoryDetail GetObjectById(int Id)
        {
            CoreAccessoryDetail coreAccessoryDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (coreAccessoryDetail != null) { coreAccessoryDetail.Errors = new Dictionary<string, string>(); }
            return coreAccessoryDetail;
        }

        public CoreAccessoryDetail CreateObject(CoreAccessoryDetail coreAccessoryDetail)
        {
            coreAccessoryDetail.IsDeleted = false;
            coreAccessoryDetail.CreatedAt = DateTime.Now;
            return Create(coreAccessoryDetail);
        }

        public CoreAccessoryDetail UpdateObject(CoreAccessoryDetail coreAccessoryDetail)
        {
            coreAccessoryDetail.UpdatedAt = DateTime.Now;
            Update(coreAccessoryDetail);
            return coreAccessoryDetail;
        }

        public CoreAccessoryDetail SoftDeleteObject(CoreAccessoryDetail coreAccessoryDetail)
        {
            coreAccessoryDetail.IsDeleted = true;
            coreAccessoryDetail.DeletedAt = DateTime.Now;
            Update(coreAccessoryDetail);
            return coreAccessoryDetail;
        }

        public bool DeleteObject(int Id)
        {
            CoreAccessoryDetail coreAccessoryDetail = Find(x => x.Id == Id);
            return (Delete(coreAccessoryDetail) == 1) ? true : false;
        }
    }
}