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
    public class RecoveryAccessoryDetailRepository : EfRepository<RecoveryAccessoryDetail>, IRecoveryAccessoryDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public RecoveryAccessoryDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<RecoveryAccessoryDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<RecoveryAccessoryDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<RecoveryAccessoryDetail> GetObjectsByRecoveryOrderDetailId(int recoveryOrderDetailId)
        {
            return FindAll(x => x.RecoveryOrderDetailId == recoveryOrderDetailId && !x.IsDeleted).ToList();
        }

        public IList<RecoveryAccessoryDetail> GetObjectsByItemId(int ItemId)
        {
            return FindAll(x => x.ItemId == ItemId && !x.IsDeleted).ToList();
        }

        public RecoveryAccessoryDetail GetObjectById(int Id)
        {
            RecoveryAccessoryDetail recoveryAccessoryDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (recoveryAccessoryDetail != null) { recoveryAccessoryDetail.Errors = new Dictionary<string, string>(); }
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail CreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail)
        {
            recoveryAccessoryDetail.IsDeleted = false;
            recoveryAccessoryDetail.CreatedAt = DateTime.Now;
            return Create(recoveryAccessoryDetail);
        }

        public RecoveryAccessoryDetail UpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail)
        {
            recoveryAccessoryDetail.UpdatedAt = DateTime.Now;
            Update(recoveryAccessoryDetail);
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail SoftDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail)
        {
            recoveryAccessoryDetail.IsDeleted = true;
            recoveryAccessoryDetail.DeletedAt = DateTime.Now;
            Update(recoveryAccessoryDetail);
            return recoveryAccessoryDetail;
        }

        public bool DeleteObject(int Id)
        {
            RecoveryAccessoryDetail recoveryAccessoryDetail = Find(x => x.Id == Id);
            return (Delete(recoveryAccessoryDetail) == 1) ? true : false;
        }
    }
}