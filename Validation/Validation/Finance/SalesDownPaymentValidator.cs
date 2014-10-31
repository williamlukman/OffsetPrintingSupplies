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
    public class SalesDownPaymentValidator : ISalesDownPaymentValidator
    {
        public SalesDownPayment VHasContact(SalesDownPayment salesDownPayment, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(salesDownPayment.ContactId);
            if (contact == null)
            {
                salesDownPayment.Errors.Add("ContactId", "Tidak boleh tidak ada");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VHasReceivable(SalesDownPayment salesDownPayment, IReceivableService _receivableService)
        {
            if (salesDownPayment.ReceivableId == null)
            {
                salesDownPayment.Errors.Add("ReceivableId", "Tidak boleh tidak ada");
            }
            else if (_receivableService.GetObjectById((int) salesDownPayment.ReceivableId) == null)
            {
                salesDownPayment.Errors.Add("ReceivableId", "Tidak boleh tidak ada");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VHasDownPaymentDate(SalesDownPayment salesDownPayment)
        {
            if (salesDownPayment.DownPaymentDate == null)
            {
                salesDownPayment.Errors.Add("DownPaymentDate", "Tidak boleh kosong");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VReceivableHasNotBeenPaidAndHasNoSalesDownPaymentAllocation(SalesDownPayment salesDownPayment, IReceivableService _receivable,
                                ISalesDownPaymentAllocationService _salesDownPaymentAllocationService)
        {
            Receivable receivable = _receivable.GetObjectById((int) salesDownPayment.ReceivableId);
            SalesDownPaymentAllocation salesDownPaymentAllocation = _salesDownPaymentAllocationService.GetObjectBySalesDownPaymentId(salesDownPayment.Id);
            if (receivable.RemainingAmount < receivable.Amount)
            {
                salesDownPayment.Errors.Add("Generic", "Receivable tidak boleh sudah diuangkan");
            }
            else if (salesDownPaymentAllocation != null)
            {
                salesDownPayment.Errors.Add("Generic", "Sales DP Allocation sudah dibuat");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VTotalAmountNotNegativeNorZero(SalesDownPayment salesDownPayment)
        {
            if (salesDownPayment.TotalAmount <= 0)
            {
                salesDownPayment.Errors.Add("Generic", "Total Amount tidak boleh <= 0");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VHasNotBeenDeleted(SalesDownPayment salesDownPayment)
        {
            if (salesDownPayment.IsDeleted)
            {
                salesDownPayment.Errors.Add("Generic", "Tidak boleh sudah di deleted");
            }
            return salesDownPayment;
        }
        
        public SalesDownPayment VHasNotBeenConfirmed(SalesDownPayment salesDownPayment)
        {
            if (salesDownPayment.IsConfirmed)
            {
                salesDownPayment.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VHasBeenConfirmed(SalesDownPayment salesDownPayment)
        {
            if (!salesDownPayment.IsConfirmed)
            {
                salesDownPayment.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VGeneralLedgerPostingHasNotBeenClosed(SalesDownPayment salesDownPayment, IClosingService _closingService, int CaseConfirmUnconfirm)
        {
            switch(CaseConfirmUnconfirm)
            {
                case(1): // Confirm
                {
                    if (_closingService.IsDateClosed(salesDownPayment.ConfirmationDate.GetValueOrDefault()))
                    {
                        salesDownPayment.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
                case (2): // Unconfirm
                {
                    if (_closingService.IsDateClosed(DateTime.Now))
                    {
                        salesDownPayment.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
            }
            return salesDownPayment;
        }

        public SalesDownPayment VCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService)
        {
            VHasContact(salesDownPayment, _contactService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasDownPaymentDate(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasNotBeenDeleted(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VTotalAmountNotNegativeNorZero(salesDownPayment);
            return salesDownPayment;
        }

        public SalesDownPayment VUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService)
        {
            VHasNotBeenConfirmed(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VCreateObject(salesDownPayment, _salesDownPaymentService, _contactService);
            return salesDownPayment;
        }

        public SalesDownPayment VDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService)
        {
            VHasNotBeenDeleted(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasNotBeenConfirmed(salesDownPayment);
            return salesDownPayment;
        }

        public SalesDownPayment VHasConfirmationDate(SalesDownPayment obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public SalesDownPayment VConfirmObject(SalesDownPayment salesDownPayment, IReceivableService _receivableService,
                                               ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService,
                                               IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            VHasConfirmationDate(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasNotBeenConfirmed(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasNotBeenDeleted(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(salesDownPayment, _closingService, 1);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VCreateObject(salesDownPayment, _salesDownPaymentService, _contactService);
            return salesDownPayment;
        }

        public SalesDownPayment VUnconfirmObject(SalesDownPayment salesDownPayment, IReceivableService _receivableService,
                                ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            VHasReceivable(salesDownPayment, _receivableService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasBeenConfirmed(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VReceivableHasNotBeenPaidAndHasNoSalesDownPaymentAllocation(salesDownPayment, _receivableService, _salesDownPaymentAllocationService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(salesDownPayment, _closingService, 2);
            return salesDownPayment;
        }

        public bool ValidCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService)
        {
            VCreateObject(salesDownPayment, _salesDownPaymentService, _contactService);
            return isValid(salesDownPayment);
        }

        public bool ValidUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService)
        {
            salesDownPayment.Errors.Clear();
            VUpdateObject(salesDownPayment, _salesDownPaymentService, _contactService);
            return isValid(salesDownPayment);
        }

        public bool ValidDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService)
        {
            salesDownPayment.Errors.Clear();
            VDeleteObject(salesDownPayment, _salesDownPaymentAllocationService);
            return isValid(salesDownPayment);
        }

        public bool ValidConfirmObject(SalesDownPayment salesDownPayment, IReceivableService _receivableService,
                                       ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService, IAccountService _accountService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPayment.Errors.Clear();
            VConfirmObject(salesDownPayment, _receivableService, _salesDownPaymentService, _contactService,
                           _accountService, _generalLedgerJournalService, _closingService);
            return isValid(salesDownPayment);
        }

        public bool ValidUnconfirmObject(SalesDownPayment salesDownPayment, IReceivableService _receivableService,
                                         ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPayment.Errors.Clear();
            VUnconfirmObject(salesDownPayment, _receivableService, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService,
                             _accountService, _generalLedgerJournalService, _closingService);
            return isValid(salesDownPayment);
        }

        public bool isValid(SalesDownPayment obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesDownPayment obj)
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