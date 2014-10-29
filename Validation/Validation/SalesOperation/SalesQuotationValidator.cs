using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class SalesQuotationValidator : ISalesQuotationValidator
    {
        public SalesQuotation VHasUniqueNomorSurat(SalesQuotation salesQuotation, ISalesQuotationService _salesQuotationService)
        {
            IList<SalesQuotation> duplicates = _salesQuotationService.GetQueryable().Where(x => x.NomorSurat == salesQuotation.NomorSurat && x.Id != salesQuotation.Id).ToList();
            if (duplicates.Any())
            {
                salesQuotation.Errors.Add("NomorSurat", "Tidak boleh merupakan duplikasi");
            }
            return salesQuotation;
        }

        public SalesQuotation VHasContact(SalesQuotation salesQuotation, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(salesQuotation.ContactId);
            if (contact == null)
            {
                salesQuotation.Errors.Add("Contact", "Tidak boleh tidak ada");
            }
            return salesQuotation;
        }

        public SalesQuotation VHasQuotationDate(SalesQuotation salesQuotation)
        {
            if (salesQuotation.QuotationDate == null)
            {
                salesQuotation.Errors.Add("QuotationDate", "Tidak boleh kosong");
            }
            return salesQuotation;
        }
        
        public SalesQuotation VHasBeenConfirmed(SalesQuotation salesQuotation)
        {
            if (!salesQuotation.IsConfirmed)
            {
                salesQuotation.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return salesQuotation;
        }

        public SalesQuotation VHasNotBeenConfirmed(SalesQuotation salesQuotation)
        {
            if (salesQuotation.IsConfirmed)
            {
                salesQuotation.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return salesQuotation;
        }

        public SalesQuotation VHasSalesQuotationDetails(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService)
        {
            IList<SalesQuotationDetail> details = _salesQuotationDetailService.GetObjectsBySalesQuotationId(salesQuotation.Id);
            if (!details.Any())
            {
                salesQuotation.Errors.Add("Generic", "Tidak memiliki sales order detail");
            }
            return salesQuotation;
        }

        public SalesQuotation VHasNoSalesQuotationDetail(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService)
        {
            IList<SalesQuotationDetail> details = _salesQuotationDetailService.GetObjectsBySalesQuotationId(salesQuotation.Id);
            if (details.Any())
            {
                salesQuotation.Errors.Add("Generic", "Masih memiliki sales order detail");
            }
            return salesQuotation;
        }

        public SalesQuotation VHasNoSalesOrder(SalesQuotation salesQuotation, ISalesOrderService _salesOrderService)
        {
            IList<SalesOrder> salesOrders = _salesOrderService.GetQueryable().Where(x => x.OrderCode == salesQuotation.Code).ToList();
            if (salesOrders.Any())
            {
                salesQuotation.Errors.Add("Generic", "Sudah memiliki sales order");
            }
            return salesQuotation;
        }

        public SalesQuotation VHasConfirmationDate(SalesQuotation obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public SalesQuotation VHasNotBeenApproved(SalesQuotation salesQuotation)
        {
            if (salesQuotation.IsApproved)
            {
                salesQuotation.Errors.Add("Generic", "Sales Quotation sudah di approve");
            }
            return salesQuotation;
        }

        public SalesQuotation VHasNotBeenRejected(SalesQuotation salesQuotation)
        {
            if (salesQuotation.IsRejected)
            {
                salesQuotation.Errors.Add("Generic", "Sales Quotation sudah di reject");
            }
            return salesQuotation;
        }

        public SalesQuotation VCreateObject(SalesQuotation salesQuotation, ISalesQuotationService _salesQuotationService, IContactService _contactService)
        {
            VHasUniqueNomorSurat(salesQuotation, _salesQuotationService);
            if (!isValid(salesQuotation)) { return salesQuotation; }
            VHasContact(salesQuotation, _contactService);
            if (!isValid(salesQuotation)) { return salesQuotation; }
            VHasQuotationDate(salesQuotation);
            return salesQuotation;
        }

        public SalesQuotation VUpdateObject(SalesQuotation salesQuotation, ISalesQuotationService _salesQuotationService, IContactService _contactService)
        {
            VCreateObject(salesQuotation, _salesQuotationService, _contactService);
            if (!isValid(salesQuotation)) { return salesQuotation; }
            VHasNotBeenConfirmed(salesQuotation);
            return salesQuotation;
        }

        public SalesQuotation VDeleteObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService)
        {
            VHasNotBeenConfirmed(salesQuotation);
            if (!isValid(salesQuotation)) { return salesQuotation; }
            VHasNoSalesQuotationDetail(salesQuotation, _salesQuotationDetailService);
            return salesQuotation;
        }

        public SalesQuotation VConfirmObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService)
        {
            VHasConfirmationDate(salesQuotation);
            if (!isValid(salesQuotation)) { return salesQuotation; }
            VHasNotBeenConfirmed(salesQuotation);
            if (!isValid(salesQuotation)) { return salesQuotation; }
            VHasSalesQuotationDetails(salesQuotation, _salesQuotationDetailService);
            return salesQuotation;
        }

        public SalesQuotation VUnconfirmObject(SalesQuotation salesQuotation, ISalesOrderService _salesOrderService)
        {
            VHasBeenConfirmed(salesQuotation);
            if (!isValid(salesQuotation)) { return salesQuotation; }
            VHasNoSalesOrder(salesQuotation, _salesOrderService);
            return salesQuotation;
        }

        public SalesQuotation VApproveObject(SalesQuotation salesQuotation)
        {
            VHasBeenConfirmed(salesQuotation);
            if (!isValid(salesQuotation)) { return salesQuotation; }
            VHasNotBeenApproved(salesQuotation);
            if (!isValid(salesQuotation)) { return salesQuotation; }
            VHasNotBeenRejected(salesQuotation);
            return salesQuotation;
        }

        public SalesQuotation VRejectObject(SalesQuotation salesQuotation)
        {
            VHasBeenConfirmed(salesQuotation);
            if (!isValid(salesQuotation)) { return salesQuotation; }
            VHasNotBeenApproved(salesQuotation);
            if (!isValid(salesQuotation)) { return salesQuotation; }
            VHasNotBeenRejected(salesQuotation);
            return salesQuotation;
        }

        public bool ValidCreateObject(SalesQuotation salesQuotation, ISalesQuotationService _salesQuotationService, IContactService _contactService)
        {
            VCreateObject(salesQuotation, _salesQuotationService, _contactService);
            return isValid(salesQuotation);
        }

        public bool ValidUpdateObject(SalesQuotation salesQuotation, ISalesQuotationService _salesQuotationService, IContactService _contactService)
        {
            salesQuotation.Errors.Clear();
            VUpdateObject(salesQuotation, _salesQuotationService, _contactService);
            return isValid(salesQuotation);
        }

        public bool ValidDeleteObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService)
        {
            salesQuotation.Errors.Clear();
            VDeleteObject(salesQuotation, _salesQuotationDetailService);
            return isValid(salesQuotation);
        }

        public bool ValidConfirmObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService)
        {
            salesQuotation.Errors.Clear();
            VConfirmObject(salesQuotation, _salesQuotationDetailService);
            return isValid(salesQuotation);
        }

        public bool ValidUnconfirmObject(SalesQuotation salesQuotation, ISalesOrderService _salesOrderService)
        {
            salesQuotation.Errors.Clear();
            VUnconfirmObject(salesQuotation, _salesOrderService);
            return isValid(salesQuotation);
        }

        public bool ValidApproveObject(SalesQuotation salesQuotation)
        {
            VApproveObject(salesQuotation);
            return isValid(salesQuotation);
        }

        public bool ValidRejectObject(SalesQuotation salesQuotation)
        {
            VRejectObject(salesQuotation);
            return isValid(salesQuotation);
        }

        public bool isValid(SalesQuotation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesQuotation obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}