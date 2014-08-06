using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class ReceiptVoucherRepository : EfRepository<ReceiptVoucher>, IReceiptVoucherRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ReceiptVoucherRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<ReceiptVoucher> GetAll()
        {
            return FindAll(rv => !rv.IsDeleted).ToList();
        }

        public IList<ReceiptVoucher> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(rv => rv.CashBankId == cashBankId && !rv.IsDeleted).ToList();
        }

        public ReceiptVoucher GetObjectById(int Id)
        {
            ReceiptVoucher receiptVoucher = Find(rv => rv.Id == Id && !rv.IsDeleted);
            if (receiptVoucher != null) { receiptVoucher.Errors = new Dictionary<string, string>(); }
            return receiptVoucher;
        }

        public IList<ReceiptVoucher> GetObjectsByContactId(int contactId)
        {
            return FindAll(rv => rv.ContactId == contactId && !rv.IsDeleted).ToList();
        }

        public ReceiptVoucher CreateObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.PendingClearanceAmount = 0;
            receiptVoucher.Code = SetObjectCode();
            receiptVoucher.IsDeleted = false;
            receiptVoucher.IsConfirmed = false;
            receiptVoucher.CreatedAt = DateTime.Now;
            return Create(receiptVoucher);
        }

        public ReceiptVoucher UpdateObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.UpdatedAt = DateTime.Now;
            Update(receiptVoucher);
            return receiptVoucher;
        }

        public ReceiptVoucher SoftDeleteObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.IsDeleted = true;
            receiptVoucher.DeletedAt = DateTime.Now;
            Update(receiptVoucher);
            return receiptVoucher;
        }

        public bool DeleteObject(int Id)
        {
            ReceiptVoucher rv = Find(x => x.Id == Id);
            return (Delete(rv) == 1) ? true : false;
        }

        public ReceiptVoucher ConfirmObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.IsConfirmed = true;
            Update(receiptVoucher);
            return receiptVoucher;
        }

        public ReceiptVoucher UnconfirmObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.IsConfirmed = false;
            Update(receiptVoucher);
            return receiptVoucher;
        }

        public ReceiptVoucher ClearObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.IsCleared = true;
            Update(receiptVoucher);
            return receiptVoucher;
        }

        public ReceiptVoucher UnclearObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.IsCleared = false;
            Update(receiptVoucher);
            return receiptVoucher;
        }


        public string SetObjectCode()
        {
            // Code: #{year}/#{total_number
            int totalobject = FindAll().Count() + 1;
            string Code = "#" + DateTime.Now.Year.ToString() + "/#" + totalobject;
            return Code;
        }
    }
}