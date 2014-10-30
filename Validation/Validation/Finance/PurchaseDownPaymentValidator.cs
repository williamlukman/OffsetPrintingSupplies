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

        public PurchaseDownPayment VHasCashBank(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(purchaseDownPayment.CashBankId);
            if (cashBank == null)
            {
                purchaseDownPayment.Errors.Add("CashBankId", "Tidak boleh tidak ada");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasPaymentVoucher(PurchaseDownPayment purchaseDownPayment, IPaymentVoucherService _paymentVoucherService)
        {
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

        public PurchaseDownPayment VNotIsGBCH(PurchaseDownPayment purchaseDownPayment)
        {
            if (!purchaseDownPayment.IsGBCH)
            {
                purchaseDownPayment.Errors.Add("Generic", "Non GBCH does not need reconcile");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VIfGBCHThenIsBank(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService)
        {
            if (purchaseDownPayment.IsGBCH)
            {
                CashBank cashBank = _cashBankService.GetObjectById(purchaseDownPayment.CashBankId);
                if (!cashBank.IsBank)
                {
                    purchaseDownPayment.Errors.Add("Generic", "Jika GBCH Harus IsBank");
                }
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VIfGBCHThenHasDueDate(PurchaseDownPayment purchaseDownPayment)
        {
            if (purchaseDownPayment.IsGBCH)
            {
                if (purchaseDownPayment.DueDate == null)
                {
                    purchaseDownPayment.Errors.Add("DueDate", "Jika GBCH maka DueDate harus diisi");
                }
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VPurchaseDownPaymentAllocationHasNoDetails(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                                       IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService)
        {
            PurchaseDownPaymentAllocation purchaseDownPaymentAllocation = _purchaseDownPaymentAllocationService.GetObjectByPurchaseDownPaymentId(purchaseDownPayment.Id);
            if (purchaseDownPaymentAllocation == null)
            {
                return purchaseDownPayment;
            }
            IList<PurchaseDownPaymentAllocationDetail> details = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);
            if (details.Any())
            {
                purchaseDownPayment.Errors.Add("Generic", "Purchase DownPayment Allocation masih memiliki detail.");
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

        public PurchaseDownPayment VGeneralLedgerPostingHasNotBeenClosed(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService, int CaseConfirmUnconfirmReconcileUnreconcile)
        {
            switch(CaseConfirmUnconfirmReconcileUnreconcile)
            {
                case(1): // Confirm
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

        public PurchaseDownPayment VCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                                                 ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService)
        {
            VHasContact(purchaseDownPayment, _contactService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasCashBank(purchaseDownPayment, _cashBankService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasDownPaymentDate(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VIfGBCHThenIsBank(purchaseDownPayment, _cashBankService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VIfGBCHThenHasDueDate(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VTotalAmountNotNegativeNorZero(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                                                 ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService)
        {
            VHasPaymentVoucher(purchaseDownPayment, _paymentVoucherService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNotBeenConfirmed(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VCreateObject(purchaseDownPayment, _purchaseDownPaymentService, _contactService, _cashBankService, _paymentVoucherService);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPaymentVoucherDetailService _paymentVoucherDetailService)
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

        public PurchaseDownPayment VConfirmObject(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                                                  IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            VHasPaymentVoucher(purchaseDownPayment, _paymentVoucherService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasConfirmationDate(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNotBeenConfirmed(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNotBeenDeleted(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseDownPayment, _closingService, 1);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VCreateObject(purchaseDownPayment, _purchaseDownPaymentService, _contactService, _cashBankService, _paymentVoucherService);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VUnconfirmObject(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                                   IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                   IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            VHasBeenConfirmed(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VPurchaseDownPaymentAllocationHasNoDetails(purchaseDownPayment, _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseDownPayment, _closingService, 2);
            return purchaseDownPayment;
        }

        public bool ValidCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                                      ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService)
        {
            VCreateObject(purchaseDownPayment, _purchaseDownPaymentService, _contactService, _cashBankService, _paymentVoucherService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                                      ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService)
        {
            purchaseDownPayment.Errors.Clear();
            VUpdateObject(purchaseDownPayment, _purchaseDownPaymentService, _contactService, _cashBankService, _paymentVoucherService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                      IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            purchaseDownPayment.Errors.Clear();
            VDeleteObject(purchaseDownPayment, _purchaseDownPaymentAllocationService, _paymentVoucherDetailService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidConfirmObject(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                                       IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService, IAccountService _accountService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseDownPayment.Errors.Clear();
            VConfirmObject(purchaseDownPayment, _cashBankService, _paymentVoucherService, _purchaseDownPaymentService, _contactService,
                           _accountService, _generalLedgerJournalService, _closingService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidUnconfirmObject(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                                         IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseDownPayment.Errors.Clear();
            VUnconfirmObject(purchaseDownPayment, _cashBankService, _paymentVoucherService, _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService,
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