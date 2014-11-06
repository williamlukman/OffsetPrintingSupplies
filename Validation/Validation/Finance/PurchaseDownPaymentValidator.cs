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
    public class PurchaseDownPaymentValidator : IPurchaseDownPaymentValidator
    {
        public PurchaseDownPayment VHasContact(PurchaseDownPayment purchaseDownPayment, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(purchaseDownPayment.ContactId);
            if (contact == null)
            {
                purchaseDownPayment.Errors.Add("ContactId", "Tidak boleh tidak ada");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasPayable(PurchaseDownPayment purchaseDownPayment, IPayableService _payableService)
        {
            if (purchaseDownPayment.PayableId == null)
            {
                purchaseDownPayment.Errors.Add("PayableId", "Tidak boleh tidak ada");
            }
            else if (_payableService.GetObjectById((int)purchaseDownPayment.PayableId) == null)
            {
                purchaseDownPayment.Errors.Add("PayableId", "Tidak boleh tidak ada");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasReceivable(PurchaseDownPayment purchaseDownPayment, IReceivableService _receivableService)
        {
            if (purchaseDownPayment.ReceivableId == null)
            {
                purchaseDownPayment.Errors.Add("ReceivableId", "Tidak boleh tidak ada");
            }
            else if (_receivableService.GetObjectById((int)purchaseDownPayment.ReceivableId) == null)
            {
                purchaseDownPayment.Errors.Add("ReceivableId", "Tidak boleh tidak ada");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasDownPaymentDate(PurchaseDownPayment purchaseDownPayment)
        {
            if (purchaseDownPayment.DownPaymentDate == null)
            {
                purchaseDownPayment.Errors.Add("DownPaymentDate", "Tidak boleh kosong");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VPayableHasNotBeenPaidAndHasNoPurchaseDownPaymentAllocation(PurchaseDownPayment purchaseDownPayment, IPayableService _payableService,
                                IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService)
        {
            Payable payable = _payableService.GetObjectById((int)purchaseDownPayment.PayableId);
            PurchaseDownPaymentAllocation purchaseDownPaymentAllocation = _purchaseDownPaymentAllocationService.GetObjectByReceivableId((int)purchaseDownPayment.ReceivableId);
            if (payable.RemainingAmount < payable.Amount)
            {
                purchaseDownPayment.Errors.Add("Generic", "Payable tidak boleh sudah diuangkan");
            }
            else if (purchaseDownPaymentAllocation != null)
            {
                purchaseDownPayment.Errors.Add("Generic", "Purchase DP Allocation sudah dibuat");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VTotalAmountNotNegativeNorZero(PurchaseDownPayment purchaseDownPayment)
        {
            if (purchaseDownPayment.TotalAmount <= 0)
            {
                purchaseDownPayment.Errors.Add("Generic", "Total Amount tidak boleh <= 0");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasNotBeenDeleted(PurchaseDownPayment purchaseDownPayment)
        {
            if (purchaseDownPayment.IsDeleted)
            {
                purchaseDownPayment.Errors.Add("Generic", "Tidak boleh sudah di deleted");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasNotBeenConfirmed(PurchaseDownPayment purchaseDownPayment)
        {
            if (purchaseDownPayment.IsConfirmed)
            {
                purchaseDownPayment.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasBeenConfirmed(PurchaseDownPayment purchaseDownPayment)
        {
            if (!purchaseDownPayment.IsConfirmed)
            {
                purchaseDownPayment.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VGeneralLedgerPostingHasNotBeenClosed(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService, int CaseConfirmUnconfirm)
        {
            switch (CaseConfirmUnconfirm)
            {
                case (1): // Confirm
                    {
                        if (_closingService.IsDateClosed(purchaseDownPayment.ConfirmationDate.GetValueOrDefault()))
                        {
                            purchaseDownPayment.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (2): // Unconfirm
                    {
                        if (_closingService.IsDateClosed(DateTime.Now))
                        {
                            purchaseDownPayment.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService)
        {
            VHasContact(purchaseDownPayment, _contactService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasDownPaymentDate(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNotBeenDeleted(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VTotalAmountNotNegativeNorZero(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService)
        {
            VHasNotBeenConfirmed(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VCreateObject(purchaseDownPayment, _purchaseDownPaymentService, _contactService);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService)
        {
            VHasNotBeenDeleted(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNotBeenConfirmed(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasConfirmationDate(PurchaseDownPayment obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PurchaseDownPayment VConfirmObject(PurchaseDownPayment purchaseDownPayment, IPayableService _payableService, IReceivableService _receivableService,
                                               IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                                               IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            VHasConfirmationDate(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNotBeenConfirmed(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNotBeenDeleted(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseDownPayment, _closingService, 1);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VCreateObject(purchaseDownPayment, _purchaseDownPaymentService, _contactService);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VUnconfirmObject(PurchaseDownPayment purchaseDownPayment, IPayableService _payableService, IReceivableService _receivableService,
                                IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            VHasPayable(purchaseDownPayment, _payableService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasReceivable(purchaseDownPayment, _receivableService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasBeenConfirmed(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VPayableHasNotBeenPaidAndHasNoPurchaseDownPaymentAllocation(purchaseDownPayment, _payableService, _purchaseDownPaymentAllocationService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseDownPayment, _closingService, 2);
            return purchaseDownPayment;
        }

        public bool ValidCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService)
        {
            VCreateObject(purchaseDownPayment, _purchaseDownPaymentService, _contactService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService)
        {
            purchaseDownPayment.Errors.Clear();
            VUpdateObject(purchaseDownPayment, _purchaseDownPaymentService, _contactService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService)
        {
            purchaseDownPayment.Errors.Clear();
            VDeleteObject(purchaseDownPayment, _purchaseDownPaymentAllocationService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidConfirmObject(PurchaseDownPayment purchaseDownPayment, IPayableService _payableService, IReceivableService _receivableService,
                                       IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService, IAccountService _accountService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseDownPayment.Errors.Clear();
            VConfirmObject(purchaseDownPayment, _payableService, _receivableService, _purchaseDownPaymentService, _contactService,
                           _accountService, _generalLedgerJournalService, _closingService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidUnconfirmObject(PurchaseDownPayment purchaseDownPayment, IPayableService _payableService, IReceivableService _receivableService, 
                                         IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseDownPayment.Errors.Clear();
            VUnconfirmObject(purchaseDownPayment, _payableService, _receivableService, _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService,
                             _accountService, _generalLedgerJournalService, _closingService);
            return isValid(purchaseDownPayment);
        }

        public bool isValid(PurchaseDownPayment obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseDownPayment obj)
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