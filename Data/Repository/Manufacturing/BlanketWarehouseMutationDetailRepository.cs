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
    public class BlanketWarehouseMutationDetailRepository : EfRepository<BlanketWarehouseMutationDetail>, IBlanketWarehouseMutationDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BlanketWarehouseMutationDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<BlanketWarehouseMutationDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<BlanketWarehouseMutationDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<BlanketWarehouseMutationDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<BlanketWarehouseMutationDetail> GetObjectsByBlanketWarehouseMutationId(int blanketWarehouseMutationId)
        {
            return FindAll(x => x.BlanketWarehouseMutationId == blanketWarehouseMutationId && !x.IsDeleted).ToList();
        }

        public BlanketWarehouseMutationDetail GetObjectByBlanketOrderDetailId(int blanketOrderDetailId)
        {
            return Find(x => x.BlanketOrderDetailId == blanketOrderDetailId && !x.IsDeleted);
        }

        public BlanketWarehouseMutationDetail GetObjectById(int Id)
        {
            BlanketWarehouseMutationDetail blanketWarehouseMutationDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (blanketWarehouseMutationDetail != null) { blanketWarehouseMutationDetail.Errors = new Dictionary<string, string>(); }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail CreateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.BlanketWarehouseMutations
                              where obj.Id == blanketWarehouseMutationDetail.BlanketWarehouseMutationId
                              select obj.Code).FirstOrDefault();
            }
            blanketWarehouseMutationDetail.Code = SetObjectCode(ParentCode);
            blanketWarehouseMutationDetail.IsConfirmed = false;
            blanketWarehouseMutationDetail.IsDeleted = false;
            blanketWarehouseMutationDetail.CreatedAt = DateTime.Now;
            return Create(blanketWarehouseMutationDetail);
        }

        public BlanketWarehouseMutationDetail UpdateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail)
        {
            blanketWarehouseMutationDetail.UpdatedAt = DateTime.Now;
            Update(blanketWarehouseMutationDetail);
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail SoftDeleteObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail)
        {
            blanketWarehouseMutationDetail.IsDeleted = true;
            blanketWarehouseMutationDetail.DeletedAt = DateTime.Now;
            Update(blanketWarehouseMutationDetail);
            return blanketWarehouseMutationDetail;
        }

        public bool DeleteObject(int Id)
        {
            BlanketWarehouseMutationDetail blanketWarehouseMutationDetail =  Find(x => x.Id == Id);
            return (Delete(blanketWarehouseMutationDetail) == 1) ? true : false;
        }

        public BlanketWarehouseMutationDetail ConfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail)
        {
            blanketWarehouseMutationDetail.IsConfirmed = true;
            Update(blanketWarehouseMutationDetail);
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail UnconfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail)
        {
            blanketWarehouseMutationDetail.IsConfirmed = false;
            blanketWarehouseMutationDetail.ConfirmationDate = null;
            UpdateObject(blanketWarehouseMutationDetail);
            return blanketWarehouseMutationDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + totalnumberinthemonth;
            return Code;
        } 
    }
}