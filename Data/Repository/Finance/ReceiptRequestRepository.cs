using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class ReceiptRequestRepository : EfRepository<ReceiptRequest>, IReceiptRequestRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ReceiptRequestRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ReceiptRequest> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<ReceiptRequest> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<ReceiptRequest> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public ReceiptRequest GetObjectById(int Id)
        {
            ReceiptRequest receiptRequest = Find(x => x.Id == Id && !x.IsDeleted);
            if (receiptRequest != null) { receiptRequest.Errors = new Dictionary<string, string>(); }
            return receiptRequest;
        }

        public IList<ReceiptRequest> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public ReceiptRequest CreateObject(ReceiptRequest receiptRequest)
        {
            receiptRequest.Code = SetObjectCode();
            receiptRequest.IsDeleted = false;
            receiptRequest.IsConfirmed = false;
            receiptRequest.CreatedAt = DateTime.Now;
            return Create(receiptRequest);
        }

        public ReceiptRequest UpdateObject(ReceiptRequest receiptRequest)
        {
            receiptRequest.UpdatedAt = DateTime.Now;
            Update(receiptRequest);
            return receiptRequest;
        }

        public ReceiptRequest SoftDeleteObject(ReceiptRequest receiptRequest)
        {
            receiptRequest.IsDeleted = true;
            receiptRequest.DeletedAt = DateTime.Now;
            Update(receiptRequest);
            return receiptRequest;
        }

        public bool DeleteObject(int Id)
        {
            ReceiptRequest receiptRequest = Find(x => x.Id == Id);
            return (Delete(receiptRequest) == 1) ? true : false;
        }

        public ReceiptRequest ConfirmObject(ReceiptRequest receiptRequest)
        {
            receiptRequest.IsConfirmed = true;
            Update(receiptRequest);
            return receiptRequest;
        }

        public ReceiptRequest UnconfirmObject(ReceiptRequest receiptRequest)
        {
            receiptRequest.IsConfirmed = false;
            receiptRequest.ConfirmationDate = null;
            UpdateObject(receiptRequest);
            return receiptRequest;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}