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
    public class PurchaseAllowanceValidator : IPurchaseAllowanceValidator
    {
        public PurchaseAllowance VHasContact(PurchaseAllowance purchaseAllowance, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(purchaseAllowance.ContactId);
            if (contact == null)
            {
                purchaseAllowance.Errors.Add("ContactId", "Tidak boleh tidak ada");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VHasCashBank(PurchaseAllowance purchaseAllowance, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(purchaseAllowance.CashBankId);
            if (cashBank == null)
            {
                purchaseAllowance.Errors.Add("CashBankId", "Tidak boleh tidak ada");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VHasPaymentDate(PurchaseAllowance purchaseAllowance)
        {
            if (purchaseAllowance.AllowanceAllocationDate == null)
            {
                purchaseAllowance.Errors.Add("AllowanceAllocationDate", "Tidak boleh kosong");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VNotIsGBCH(PurchaseAllowance purchaseAllowance)
        {
            if (!purchaseAllowance.IsGBCH)
            {
                purchaseAllowance.Errors.Add("Generic", "Non GBCH does not need reconcile");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VIfGBCHThenIsBank(PurchaseAllowance purchaseAllowance, ICashBankService _cashBankService)
        {
            if (purchaseAllowance.IsGBCH)
            {
                CashBank cashBank = _cashBankService.GetObjectById(purchaseAllowance.CashBankId);
                if (!cashBank.IsBank)
                {
                    purchaseAllowance.Errors.Add("Generic", "Jika GBCH Harus IsBank");
                }
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VIfGBCHThenHasDueDate(PurchaseAllowance purchaseAllowance)
        {
            if (purchaseAllowance.IsGBCH)
            {
                if (purchaseAllowance.DueDate == null)
                {
                    purchaseAllowance.Errors.Add("DueDate", "Jika GBCH maka DueDate harus diisi");
                }
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VHasPurchaseAllowanceDetails(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService)
        {
            IList<PurchaseAllowanceDetail> details = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowance.Id);
            if (!details.Any())
            {
                purchaseAllowance.Errors.Add("Generic", "Harus memiliki payment voucher details");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VHasNoPurchaseAllowanceDetail(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService)
        {
            IList<PurchaseAllowanceDetail> details = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowance.Id);
            if (details.Any())
            {
                purchaseAllowance.Errors.Add("Generic", "Tidak boleh ada payment voucher details");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VTotalAmountIsNotZero(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService)
        {
            decimal totalamount = 0;
            IList<PurchaseAllowanceDetail> details = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowance.Id);
            foreach (var detail in details)
            {
                totalamount += detail.Amount;
            }
            if (totalamount == 0)
            {
                purchaseAllowance.Errors.Add("Generic", "Total Amount tidak boleh 0");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VHasNotBeenDeleted(PurchaseAllowance purchaseAllowance)
        {
            if (purchaseAllowance.IsDeleted)
            {
                purchaseAllowance.Errors.Add("Generic", "Tidak boleh sudah di deleted");
            }
            return purchaseAllowance;
        }
        
        public PurchaseAllowance VHasNotBeenConfirmed(PurchaseAllowance purchaseAllowance)
        {
            if (purchaseAllowance.IsConfirmed)
            {
                purchaseAllowance.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VHasBeenConfirmed(PurchaseAllowance purchaseAllowance)
        {
            if (!purchaseAllowance.IsConfirmed)
            {
                purchaseAllowance.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VTotalAmountEqualDetailsAmount(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService)
        {
            IList<PurchaseAllowanceDetail> details = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowance.Id);
            decimal detailsamount = 0;
            foreach (var detail in details)
            {
                detailsamount += detail.Amount;
            }
            if (detailsamount != purchaseAllowance.TotalAmount)
            {
                purchaseAllowance.Errors.Add("Generic", "Jumlah amount di details harus sama dengan totalamount");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VAllPurchaseAllowanceDetailsAreConfirmable(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService,
                                                              IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService,
                                                              IPayableService _payableService)
        {
            IList<PurchaseAllowanceDetail> details = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowance.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                detail.ConfirmationDate = purchaseAllowance.ConfirmationDate;
                detail.Errors = new Dictionary<string, string>();
                if (!_purchaseAllowanceDetailService.GetValidator().ValidConfirmObject(detail, _payableService))
                {
                    foreach (var error in detail.Errors)
                    {
                        purchaseAllowance.Errors.Add(error.Key, error.Value);
                    }
                    if (purchaseAllowance.Errors.Any()) { return purchaseAllowance; }
                }
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VCashBankIsGreaterThanOrEqualPurchaseAllowanceDetails(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, bool CasePayment)
        {
            if (CasePayment)
            {
                decimal totalamount = 0;
                IList<PurchaseAllowanceDetail> details = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowance.Id);
                foreach (var detail in details)
                {
                    totalamount += detail.Amount;
                }

                CashBank cashBank = _cashBankService.GetObjectById(purchaseAllowance.CashBankId);
                if (cashBank.Amount < totalamount)
                {
                    purchaseAllowance.Errors.Add("Generic", "Cash bank tidak boleh kurang dari total amount");
                }
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VHasBeenReconciled(PurchaseAllowance purchaseAllowance)
        {
            if (!purchaseAllowance.IsReconciled)
            {
                purchaseAllowance.Errors.Add("Generic", "Belum di reconcile");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VHasNotBeenReconciled(PurchaseAllowance purchaseAllowance)
        {
            if (purchaseAllowance.IsReconciled)
            {
                purchaseAllowance.Errors.Add("Generic", "Sudah di reconcile");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VHasReconciliationDate(PurchaseAllowance purchaseAllowance)
        {
            if (purchaseAllowance.ReconciliationDate == null)
            {
                purchaseAllowance.Errors.Add("ReconciliationDate", "Harus memiliki reconciliation date");
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VGeneralLedgerPostingHasNotBeenClosed(PurchaseAllowance purchaseAllowance, IClosingService _closingService, int CaseConfirmUnconfirmReconcileUnreconcile)
        {
            switch(CaseConfirmUnconfirmReconcileUnreconcile)
            {
                case(1): // Confirm
                case(2): // Unconfirm
                {
                    if (_closingService.IsDateClosed(purchaseAllowance.AllowanceAllocationDate))
                    {
                        purchaseAllowance.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
                case(3): // Reconcile
                case(4): // Unreconcile
                {
                    if (_closingService.IsDateClosed(purchaseAllowance.ReconciliationDate.GetValueOrDefault()))
                    {
                        purchaseAllowance.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance VCreateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                            IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasContact(purchaseAllowance, _contactService);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasCashBank(purchaseAllowance, _cashBankService);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasPaymentDate(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VIfGBCHThenIsBank(purchaseAllowance, _cashBankService);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VIfGBCHThenHasDueDate(purchaseAllowance);
            return purchaseAllowance;
        }

        public PurchaseAllowance VUpdateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                            IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasNotBeenConfirmed(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasNoPurchaseAllowanceDetail(purchaseAllowance, _purchaseAllowanceDetailService);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VCreateObject(purchaseAllowance, _purchaseAllowanceService, _purchaseAllowanceDetailService, _payableService, _contactService, _cashBankService);
            return purchaseAllowance;
        }

        public PurchaseAllowance VDeleteObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService)
        {
            VHasNotBeenDeleted(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasNoPurchaseAllowanceDetail(purchaseAllowance, _purchaseAllowanceDetailService);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasNotBeenConfirmed(purchaseAllowance);
            return purchaseAllowance;
        }

        public PurchaseAllowance VHasConfirmationDate(PurchaseAllowance obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PurchaseAllowance VConfirmObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService,
                                             IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService,
                                             IPayableService _payableService, IClosingService _closingService)
        {
            VHasConfirmationDate(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasPurchaseAllowanceDetails(purchaseAllowance, _purchaseAllowanceDetailService);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VTotalAmountIsNotZero(purchaseAllowance, _purchaseAllowanceDetailService);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasNotBeenConfirmed(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasNotBeenDeleted(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VTotalAmountEqualDetailsAmount(purchaseAllowance, _purchaseAllowanceDetailService);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VAllPurchaseAllowanceDetailsAreConfirmable(purchaseAllowance, _purchaseAllowanceService, _purchaseAllowanceDetailService, _cashBankService, _payableService);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VCashBankIsGreaterThanOrEqualPurchaseAllowanceDetails(purchaseAllowance, _purchaseAllowanceDetailService, _cashBankService, !purchaseAllowance.IsGBCH);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseAllowance, _closingService, 1);
            return purchaseAllowance;
        }

        public PurchaseAllowance VUnconfirmObject(PurchaseAllowance purchaseAllowance, IClosingService _closingService)
        {
            VHasBeenConfirmed(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasNotBeenReconciled(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseAllowance, _closingService, 2);
            return purchaseAllowance;
        }

        public PurchaseAllowance VReconcileObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VNotIsGBCH(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasBeenConfirmed(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasNotBeenReconciled(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasReconciliationDate(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseAllowance, _closingService, 3);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VCashBankIsGreaterThanOrEqualPurchaseAllowanceDetails(purchaseAllowance, _purchaseAllowanceDetailService, _cashBankService, true);
            return purchaseAllowance;
        }

        public PurchaseAllowance VUnreconcileObject(PurchaseAllowance purchaseAllowance, IClosingService _closingService)
        {
            VHasBeenConfirmed(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VHasBeenReconciled(purchaseAllowance);
            if (!isValid(purchaseAllowance)) { return purchaseAllowance; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseAllowance, _closingService, 4);
            return purchaseAllowance;
        }

        public bool ValidCreateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                      IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VCreateObject(purchaseAllowance, _purchaseAllowanceService, _purchaseAllowanceDetailService, _payableService, _contactService, _cashBankService);
            return isValid(purchaseAllowance);
        }

        public bool ValidUpdateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                      IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            purchaseAllowance.Errors.Clear();
            VUpdateObject(purchaseAllowance, _purchaseAllowanceService, _purchaseAllowanceDetailService, _payableService, _contactService, _cashBankService);
            return isValid(purchaseAllowance);
        }

        public bool ValidDeleteObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService)
        {
            purchaseAllowance.Errors.Clear();
            VDeleteObject(purchaseAllowance, _purchaseAllowanceDetailService);
            return isValid(purchaseAllowance);
        }

        public bool ValidConfirmObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService,
                                       IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService,
                                       IPayableService _payableService, IClosingService _closingService)
        {
            purchaseAllowance.Errors.Clear();
            VConfirmObject(purchaseAllowance, _purchaseAllowanceService, _purchaseAllowanceDetailService, _cashBankService, _payableService, _closingService);
            return isValid(purchaseAllowance);
        }

        public bool ValidUnconfirmObject(PurchaseAllowance purchaseAllowance, IClosingService _closingService)
        {
            purchaseAllowance.Errors.Clear();
            VUnconfirmObject(purchaseAllowance, _closingService);
            return isValid(purchaseAllowance);
        }

        public bool ValidReconcileObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            purchaseAllowance.Errors.Clear();
            VReconcileObject(purchaseAllowance, _purchaseAllowanceDetailService, _cashBankService, _closingService);
            return isValid(purchaseAllowance);
        }

        public bool ValidUnreconcileObject(PurchaseAllowance purchaseAllowance, IClosingService _closingService)
        {
            purchaseAllowance.Errors.Clear();
            VUnreconcileObject(purchaseAllowance, _closingService);
            return isValid(purchaseAllowance);
        }

        public bool isValid(PurchaseAllowance obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseAllowance obj)
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