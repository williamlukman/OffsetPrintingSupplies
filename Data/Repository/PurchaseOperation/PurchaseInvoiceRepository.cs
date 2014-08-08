using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseInvoiceRepository : EfRepository<PurchaseInvoice>, IPurchaseInvoiceRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseInvoiceRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<PurchaseInvoice> GetAll()
        {
            return FindAll(pi => !pi.IsDeleted).ToList();
        }

        public PurchaseInvoice GetObjectById(int Id)
        {
            PurchaseInvoice purchaseInvoice = Find(pi => pi.Id == Id && !pi.IsDeleted);
            if (purchaseInvoice != null) { purchaseInvoice.Errors = new Dictionary<string, string>(); }
            return purchaseInvoice;
        }

        public IList<PurchaseInvoice> GetObjectsByPurchaseReceivalId(int purchaseReceivalId)
        {
            return FindAll(pi => pi.PurchaseReceivalId == purchaseReceivalId && !pi.IsDeleted).ToList();
        }

        public PurchaseInvoice CreateObject(PurchaseInvoice purchaseInvoice)
        {
            purchaseInvoice.Code = SetObjectCode();
            purchaseInvoice.AmountPayable = 0;
            purchaseInvoice.IsDeleted = false;
            purchaseInvoice.IsConfirmed = false;
            purchaseInvoice.CreatedAt = DateTime.Now;
            return Create(purchaseInvoice);
        }

        public PurchaseInvoice UpdateObject(PurchaseInvoice purchaseInvoice)
        {
            purchaseInvoice.UpdatedAt = DateTime.Now;
            Update(purchaseInvoice);
            return purchaseInvoice;
        }

        public PurchaseInvoice SoftDeleteObject(PurchaseInvoice purchaseInvoice)
        {
            purchaseInvoice.IsDeleted = true;
            purchaseInvoice.DeletedAt = DateTime.Now;
            Update(purchaseInvoice);
            return purchaseInvoice;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseInvoice pi = Find(x => x.Id == Id);
            return (Delete(pi) == 1) ? true : false;
        }

        public PurchaseInvoice ConfirmObject(PurchaseInvoice purchaseInvoice)
        {
            purchaseInvoice.IsConfirmed = true;
            Update(purchaseInvoice);
            return purchaseInvoice;
        }

        public PurchaseInvoice UnconfirmObject(PurchaseInvoice purchaseInvoice)
        {
            purchaseInvoice.IsConfirmed = false;
            purchaseInvoice.ConfirmationDate = null;
            UpdateObject(purchaseInvoice);
            return purchaseInvoice;
        }

        public string SetObjectCode()
        {
            // Code: #{year}/#{total_number + 1}
            int totalobject = FindAll().Count() + 1;
            string Code = "#" + DateTime.Now.Year.ToString() + "/#" + totalobject;
            return Code;
        }
    }
}