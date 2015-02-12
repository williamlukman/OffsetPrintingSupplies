using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class ReceiptRequestDetailRepository : EfRepository<ReceiptRequestDetail>, IReceiptRequestDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ReceiptRequestDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ReceiptRequestDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<ReceiptRequestDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<ReceiptRequestDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<ReceiptRequestDetail> GetObjectsByReceiptRequestId(int ReceiptRequestId)
        {
            return FindAll(x => x.ReceiptRequestId == ReceiptRequestId && !x.IsDeleted).ToList();
        }

        public IList<ReceiptRequestDetail> GetNonLegacyObjectsByReceiptRequestId(int ReceiptRequestId)
        {
            return FindAll(x => x.ReceiptRequestId == ReceiptRequestId && !x.IsLegacy && !x.IsDeleted).ToList();
        }

        public ReceiptRequestDetail GetLegacyObjectByReceiptRequestId(int ReceiptRequestId)
        {
            ReceiptRequestDetail detail = Find(x => x.ReceiptRequestId == ReceiptRequestId && x.IsLegacy && !x.IsDeleted);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public ReceiptRequestDetail GetObjectById(int Id)
        {
            ReceiptRequestDetail detail = Find(x => x.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public ReceiptRequestDetail CreateObject(ReceiptRequestDetail ReceiptRequestDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.ReceiptRequests
                              where obj.Id == ReceiptRequestDetail.ReceiptRequestId
                              select obj.Code).FirstOrDefault();
            }
            ReceiptRequestDetail.Code = SetObjectCode(ParentCode);
            ReceiptRequestDetail.IsConfirmed = false;
            ReceiptRequestDetail.IsDeleted = false;
            ReceiptRequestDetail.CreatedAt = DateTime.Now;
            return Create(ReceiptRequestDetail);
        }

        public ReceiptRequestDetail UpdateObject(ReceiptRequestDetail ReceiptRequestDetail)
        {
            ReceiptRequestDetail.UpdatedAt = DateTime.Now;
            Update(ReceiptRequestDetail);
            return ReceiptRequestDetail;
        }

        public ReceiptRequestDetail SoftDeleteObject(ReceiptRequestDetail ReceiptRequestDetail)
        {
            ReceiptRequestDetail.IsDeleted = true;
            ReceiptRequestDetail.DeletedAt = DateTime.Now;
            Update(ReceiptRequestDetail);
            return ReceiptRequestDetail;
        }

        public bool DeleteObject(int Id)
        {
            ReceiptRequestDetail ReceiptRequestDetail = Find(x => x.Id == Id);
            return (Delete(ReceiptRequestDetail) == 1) ? true : false;
        }

        public ReceiptRequestDetail ConfirmObject(ReceiptRequestDetail ReceiptRequestDetail)
        {
            ReceiptRequestDetail.IsConfirmed = true;
            Update(ReceiptRequestDetail);
            return ReceiptRequestDetail;
        }

        public ReceiptRequestDetail UnconfirmObject(ReceiptRequestDetail ReceiptRequestDetail)
        {
            ReceiptRequestDetail.IsConfirmed = false;
            ReceiptRequestDetail.ConfirmationDate = null;
            UpdateObject(ReceiptRequestDetail);
            return ReceiptRequestDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 
    }
}