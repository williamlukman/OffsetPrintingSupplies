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
    public class ReceiptRequestValidator : IReceiptRequestValidator
    {
        public ReceiptRequest VHasContact(ReceiptRequest receiptRequest, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(receiptRequest.ContactId);
            if (contact == null)
            {
                receiptRequest.Errors.Add("ContactcId", "Tidak boleh kosong");
            }
            return receiptRequest;
        }

        public ReceiptRequest VIsValidAmount(ReceiptRequest receiptRequest)
        {
            if (receiptRequest.Amount < 0)
            {
                receiptRequest.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
            }
            return receiptRequest;
        }

        public ReceiptRequest VDebitEqualCreditEqualAmount(ReceiptRequest receiptRequest, IReceiptRequestDetailService _receiptRequestDetailService)
        {
            IList<ReceiptRequestDetail> receiptRequestDetails = _receiptRequestDetailService.GetObjectsByReceiptRequestId(receiptRequest.Id);
            decimal debit = 0;
            decimal credit = 0;
            foreach (var detail in receiptRequestDetails)
            {
                debit += detail.Status == Constant.GeneralLedgerStatus.Debit ? detail.Amount : 0;
                credit += detail.Status == Constant.GeneralLedgerStatus.Credit ? detail.Amount : 0;
            }
            if (debit != credit || debit != receiptRequest.Amount)
            {
                receiptRequest.Errors.Add("Generic", "Jumlah debit, credit, dan amount harus sama: " + receiptRequest.Amount);
            }
            return receiptRequest;
        }

        public ReceiptRequest VHasBeenConfirmed(ReceiptRequest receiptRequest)
        {
            if (!receiptRequest.IsConfirmed)
            {
                receiptRequest.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return receiptRequest;
        }

        public ReceiptRequest VHasNotBeenConfirmed(ReceiptRequest receiptRequest)
        {
            if (receiptRequest.IsConfirmed)
            {
                receiptRequest.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return receiptRequest;
        }

        public ReceiptRequest VHasNotBeenDeleted(ReceiptRequest receiptRequest)
        {
            if (receiptRequest.IsDeleted)
            {
                receiptRequest.Errors.Add("Generic", "Sudah dihapus");
            }
            return receiptRequest;
        }

        public ReceiptRequest VHasConfirmationDate(ReceiptRequest obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public ReceiptRequest VGeneralLedgerPostingHasNotBeenClosed(ReceiptRequest receiptRequest, IClosingService _closingService)
        {
            if (_closingService.IsDateClosed(receiptRequest.RequestedDate))
            {
                receiptRequest.Errors.Add("Generic", "Ledger sudah tutup buku");
            }
            return receiptRequest;
        }

        public ReceiptRequest VCreateObject(ReceiptRequest receiptRequest, IContactService _contactService)
        {
            VHasContact(receiptRequest, _contactService);
            if (!isValid(receiptRequest)) { return receiptRequest; }
            VIsValidAmount(receiptRequest);
            return receiptRequest;
        }

        public ReceiptRequest VUpdateObject(ReceiptRequest receiptRequest, IContactService _contactService)
        {
            VHasNotBeenConfirmed(receiptRequest);
            if (!isValid(receiptRequest)) { return receiptRequest; }
            VHasNotBeenDeleted(receiptRequest);
            if (!isValid(receiptRequest)) { return receiptRequest; }
            VCreateObject(receiptRequest, _contactService);
            return receiptRequest;
        }

        public ReceiptRequest VDeleteObject(ReceiptRequest receiptRequest)
        {
            VHasNotBeenConfirmed(receiptRequest);
            if (!isValid(receiptRequest)) { return receiptRequest; }
            VHasNotBeenDeleted(receiptRequest);
            return receiptRequest;
        }

        public ReceiptRequest VConfirmObject(ReceiptRequest receiptRequest, IReceiptRequestDetailService _receiptRequestDetailService, IClosingService _closingService)
        {
            VHasConfirmationDate(receiptRequest);
            if (!isValid(receiptRequest)) { return receiptRequest; }
            VHasNotBeenDeleted(receiptRequest);
            if (!isValid(receiptRequest)) { return receiptRequest; }
            VHasNotBeenConfirmed(receiptRequest);
            if (!isValid(receiptRequest)) { return receiptRequest; }
            VGeneralLedgerPostingHasNotBeenClosed(receiptRequest, _closingService);
            return receiptRequest;
        }

        public ReceiptRequest VUnconfirmObject(ReceiptRequest receiptRequest, IReceiptRequestDetailService _receiptRequestDetailService, IClosingService _closingService)
        {
            VHasBeenConfirmed(receiptRequest);
            if (!isValid(receiptRequest)) { return receiptRequest; }
            VHasNotBeenDeleted(receiptRequest);
            if (!isValid(receiptRequest)) { return receiptRequest; }
            VGeneralLedgerPostingHasNotBeenClosed(receiptRequest, _closingService);
            return receiptRequest;
        }

        public bool ValidCreateObject(ReceiptRequest receiptRequest, IContactService _contactService)
        {
            VCreateObject(receiptRequest, _contactService);
            return isValid(receiptRequest);
        }

        public bool ValidUpdateObject(ReceiptRequest receiptRequest, IContactService _contactService)
        {
            receiptRequest.Errors.Clear();
            VUpdateObject(receiptRequest, _contactService);
            return isValid(receiptRequest);
        }

        public bool ValidDeleteObject(ReceiptRequest receiptRequest)
        {
            receiptRequest.Errors.Clear();
            VDeleteObject(receiptRequest);
            return isValid(receiptRequest);
        }

        public bool ValidConfirmObject(ReceiptRequest receiptRequest, IReceiptRequestDetailService _receiptRequestDetailService, IClosingService _closingService)
        {
            receiptRequest.Errors.Clear();
            VConfirmObject(receiptRequest, _receiptRequestDetailService, _closingService);
            return isValid(receiptRequest);
        }

        public bool ValidUnconfirmObject(ReceiptRequest receiptRequest, IReceiptRequestDetailService _receiptRequestDetailService, IClosingService _closingService)
        {
            receiptRequest.Errors.Clear();
            VUnconfirmObject(receiptRequest, _receiptRequestDetailService, _closingService);
            return isValid(receiptRequest);
        }

        public bool isValid(ReceiptRequest obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ReceiptRequest obj)
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