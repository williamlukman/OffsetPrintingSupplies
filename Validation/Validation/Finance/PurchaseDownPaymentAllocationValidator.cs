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
    public class PurchaseDownPaymentAllocationValidator : IPurchaseDownPaymentAllocationValidator
    {
        public PurchaseDownPaymentAllocation VHasContact(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(purchaseDownPaymentAllocation.ContactId);
            if (contact == null)
            {
                purchaseDownPaymentAllocation.Errors.Add("ContactId", "Tidak boleh tidak ada");
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VHasPurchaseDownPayment(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentService _purchaseDownPaymentService)
        {
            PurchaseDownPayment purchaseDownPayment = _purchaseDownPaymentService.GetObjectById(purchaseDownPaymentAllocation.PurchaseDownPaymentId);
            if (purchaseDownPayment == null)
            {
                purchaseDownPaymentAllocation.Errors.Add("PurchaseDownPaymentId", "Tidak boleh tidak ada");
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VHasAllocationDate(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation)
        {
            if (purchaseDownPaymentAllocation.AllocationDate == null)
            {
                purchaseDownPaymentAllocation.Errors.Add("AllocationDate", "Tidak boleh kosong");
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VHasPurchaseDownPaymentAllocationDetails(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService)
        {
            IList<PurchaseDownPaymentAllocationDetail> details = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);
            if (!details.Any())
            {
                purchaseDownPaymentAllocation.Errors.Add("Generic", "Harus memiliki payment voucher details");
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VHasNoPurchaseDownPaymentAllocationDetail(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService)
        {
            IList<PurchaseDownPaymentAllocationDetail> details = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);
            if (details.Any())
            {
                purchaseDownPaymentAllocation.Errors.Add("Generic", "Tidak boleh ada payment voucher details");
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VTotalAmountNotNegativeNorZero(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation)
        {
            if (purchaseDownPaymentAllocation.TotalAmount <= 0)
            {
                purchaseDownPaymentAllocation.Errors.Add("Generic", "Total Amount tidak boleh <= 0");
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VHasNotBeenDeleted(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation)
        {
            if (purchaseDownPaymentAllocation.IsDeleted)
            {
                purchaseDownPaymentAllocation.Errors.Add("Generic", "Tidak boleh sudah di deleted");
            }
            return purchaseDownPaymentAllocation;
        }
        
        public PurchaseDownPaymentAllocation VHasNotBeenConfirmed(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation)
        {
            if (purchaseDownPaymentAllocation.IsConfirmed)
            {
                purchaseDownPaymentAllocation.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VHasBeenConfirmed(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation)
        {
            if (!purchaseDownPaymentAllocation.IsConfirmed)
            {
                purchaseDownPaymentAllocation.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VTotalAmountEqualDetailsAmount(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService)
        {
            IList<PurchaseDownPaymentAllocationDetail> details = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);
            decimal detailsamount = 0;
            foreach (var detail in details)
            {
                detailsamount += detail.Amount;
            }
            if (detailsamount != purchaseDownPaymentAllocation.TotalAmount)
            {
                purchaseDownPaymentAllocation.Errors.Add("Generic", "Jumlah amount di details harus sama dengan totalamount");
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VAllPurchaseDownPaymentAllocationDetailsAreConfirmable(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation,
                                             IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService)
        {
            IList<PurchaseDownPaymentAllocationDetail> details = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                detail.ConfirmationDate = purchaseDownPaymentAllocation.ConfirmationDate;
                detail.Errors = new Dictionary<string, string>();
                if (!_purchaseDownPaymentAllocationDetailService.GetValidator().ValidConfirmObject(detail, _paymentVoucherDetailService, _payableService))
                {
                    foreach (var error in detail.Errors)
                    {
                        purchaseDownPaymentAllocation.Errors.Add(error.Key, error.Value);
                    }
                    if (purchaseDownPaymentAllocation.Errors.Any()) { return purchaseDownPaymentAllocation; }
                }
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VCashBankIsGreaterThanOrEqualPurchaseDownPaymentAllocationDetails(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, 
                                             IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                             ICashBankService _cashBankService)
        {
            decimal totalamount = 0;
            IList<PurchaseDownPaymentAllocationDetail> details = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);
            foreach (var detail in details)
            {
                totalamount += detail.Amount;
            }

            PurchaseDownPayment purchaseDownPayment = _purchaseDownPaymentService.GetObjectById(purchaseDownPaymentAllocation.PurchaseDownPaymentId);
            CashBank cashBank = _cashBankService.GetObjectById(purchaseDownPayment.CashBankId);
            if (cashBank.Amount < totalamount)
            {
                purchaseDownPaymentAllocation.Errors.Add("Generic", "Cash bank tidak boleh kurang dari total amount");
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VGeneralLedgerPostingHasNotBeenClosed(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IClosingService _closingService, int CaseConfirmUnconfirmReconcileUnreconcile)
        {
            switch(CaseConfirmUnconfirmReconcileUnreconcile)
            {
                case(1): // Confirm
                {
                    if (_closingService.IsDateClosed(purchaseDownPaymentAllocation.ConfirmationDate.GetValueOrDefault()))
                    {
                        purchaseDownPaymentAllocation.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
                case(2): // Unconfirm
                {
                    if (_closingService.IsDateClosed(DateTime.Now))
                    {
                        purchaseDownPaymentAllocation.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VCreateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                           IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                           IContactService _contactService)
        {
            VHasContact(purchaseDownPaymentAllocation, _contactService);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VHasPurchaseDownPayment(purchaseDownPaymentAllocation, _purchaseDownPaymentService);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VHasAllocationDate(purchaseDownPaymentAllocation);
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VUpdateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                             IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                             IContactService _contactService)
        {
            VHasNotBeenConfirmed(purchaseDownPaymentAllocation);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VHasNoPurchaseDownPaymentAllocationDetail(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VCreateObject(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _contactService);
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VDeleteObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService)
        {
            VHasNotBeenDeleted(purchaseDownPaymentAllocation);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VHasNoPurchaseDownPaymentAllocationDetail(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VHasNotBeenConfirmed(purchaseDownPaymentAllocation);
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VHasConfirmationDate(PurchaseDownPaymentAllocation obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PurchaseDownPaymentAllocation VConfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                             IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService,
                                             IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            VHasConfirmationDate(purchaseDownPaymentAllocation);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VHasPurchaseDownPaymentAllocationDetails(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VTotalAmountNotNegativeNorZero(purchaseDownPaymentAllocation);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VHasNotBeenConfirmed(purchaseDownPaymentAllocation);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VHasNotBeenDeleted(purchaseDownPaymentAllocation);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VTotalAmountEqualDetailsAmount(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VAllPurchaseDownPaymentAllocationDetailsAreConfirmable(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService, 
                                                                   _paymentVoucherDetailService, _payableService);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VCashBankIsGreaterThanOrEqualPurchaseDownPaymentAllocationDetails(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _cashBankService);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseDownPaymentAllocation, _closingService, 1);
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation VUnconfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                             IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                             IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            VHasBeenConfirmed(purchaseDownPaymentAllocation);
            if (!isValid(purchaseDownPaymentAllocation)) { return purchaseDownPaymentAllocation; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseDownPaymentAllocation, _closingService, 2);
            return purchaseDownPaymentAllocation;
        }

        public bool ValidCreateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                      IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                      IContactService _contactService)
        {
            VCreateObject(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _contactService);
            return isValid(purchaseDownPaymentAllocation);
        }

        public bool ValidUpdateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                      IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                      IContactService _contactService)
        {
            purchaseDownPaymentAllocation.Errors.Clear();
            VUpdateObject(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _contactService );
            return isValid(purchaseDownPaymentAllocation);
        }

        public bool ValidDeleteObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService)
        {
            purchaseDownPaymentAllocation.Errors.Clear();
            VDeleteObject(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService);
            return isValid(purchaseDownPaymentAllocation);
        }

        public bool ValidConfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                    IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService,
                    IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseDownPaymentAllocation.Errors.Clear();
            VConfirmObject(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService,
                           _payableService, _paymentVoucherDetailService, _cashBankService, _accountService, _generalLedgerJournalService, _closingService);
            return isValid(purchaseDownPaymentAllocation);
        }

        public bool ValidUnconfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                    IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                    IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseDownPaymentAllocation.Errors.Clear();
            VUnconfirmObject(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService, _payableService, _paymentVoucherDetailService,
                             _accountService, _generalLedgerJournalService, _closingService);
            return isValid(purchaseDownPaymentAllocation);
        }

        public bool isValid(PurchaseDownPaymentAllocation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseDownPaymentAllocation obj)
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