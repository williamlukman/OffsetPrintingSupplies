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

        public PurchaseDownPayment VHasPaymentDate(PurchaseDownPayment purchaseDownPayment)
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

        public PurchaseDownPayment VHasPurchaseDownPaymentDetails(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService)
        {
            IList<PurchaseDownPaymentDetail> details = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPayment.Id);
            if (!details.Any())
            {
                purchaseDownPayment.Errors.Add("Generic", "Harus memiliki payment voucher details");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasNoPurchaseDownPaymentDetail(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService)
        {
            IList<PurchaseDownPaymentDetail> details = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPayment.Id);
            if (details.Any())
            {
                purchaseDownPayment.Errors.Add("Generic", "Tidak boleh ada payment voucher details");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VTotalAmountIsNotZero(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService)
        {
            decimal totalamount = 0;
            IList<PurchaseDownPaymentDetail> details = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPayment.Id);
            foreach (var detail in details)
            {
                totalamount += detail.Amount;
            }
            if (totalamount == 0)
            {
                purchaseDownPayment.Errors.Add("Generic", "Total Amount tidak boleh 0");
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

        public PurchaseDownPayment VTotalAmountEqualDetailsAmount(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService)
        {
            IList<PurchaseDownPaymentDetail> details = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPayment.Id);
            decimal detailsamount = 0;
            foreach (var detail in details)
            {
                detailsamount += detail.Amount;
            }
            if (detailsamount != purchaseDownPayment.TotalAmount)
            {
                purchaseDownPayment.Errors.Add("Generic", "Jumlah amount di details harus sama dengan totalamount");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VAllPurchaseDownPaymentDetailsAreConfirmable(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                              IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService,
                                                              IPayableService _payableService)
        {
            IList<PurchaseDownPaymentDetail> details = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPayment.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                detail.ConfirmationDate = purchaseDownPayment.ConfirmationDate;
                detail.Errors = new Dictionary<string, string>();
                if (!_purchaseDownPaymentDetailService.GetValidator().ValidConfirmObject(detail, _payableService))
                {
                    foreach (var error in detail.Errors)
                    {
                        purchaseDownPayment.Errors.Add(error.Key, error.Value);
                    }
                    if (purchaseDownPayment.Errors.Any()) { return purchaseDownPayment; }
                }
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VCashBankIsGreaterThanOrEqualPurchaseDownPaymentDetails(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, bool CasePayment)
        {
            if (CasePayment)
            {
                decimal totalamount = 0;
                IList<PurchaseDownPaymentDetail> details = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPayment.Id);
                foreach (var detail in details)
                {
                    totalamount += detail.Amount;
                }

                CashBank cashBank = _cashBankService.GetObjectById(purchaseDownPayment.CashBankId);
                if (cashBank.Amount < totalamount)
                {
                    purchaseDownPayment.Errors.Add("Generic", "Cash bank tidak boleh kurang dari total amount");
                }
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasBeenReconciled(PurchaseDownPayment purchaseDownPayment)
        {
            if (!purchaseDownPayment.IsReconciled)
            {
                purchaseDownPayment.Errors.Add("Generic", "Belum di reconcile");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasNotBeenReconciled(PurchaseDownPayment purchaseDownPayment)
        {
            if (purchaseDownPayment.IsReconciled)
            {
                purchaseDownPayment.Errors.Add("Generic", "Sudah di reconcile");
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VHasReconciliationDate(PurchaseDownPayment purchaseDownPayment)
        {
            if (purchaseDownPayment.ReconciliationDate == null)
            {
                purchaseDownPayment.Errors.Add("ReconciliationDate", "Harus memiliki reconciliation date");
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
                case(2): // Unconfirm
                {
                    if (_closingService.IsDateClosed(DateTime.Now))
                    {
                        purchaseDownPayment.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
                case(3): // Reconcile
                {
                    if (_closingService.IsDateClosed(purchaseDownPayment.ReconciliationDate.GetValueOrDefault()))
                    {
                        purchaseDownPayment.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
                case (4): // Unreconcile
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

        public PurchaseDownPayment VCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                            IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasContact(purchaseDownPayment, _contactService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasCashBank(purchaseDownPayment, _cashBankService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasPaymentDate(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VIfGBCHThenIsBank(purchaseDownPayment, _cashBankService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VIfGBCHThenHasDueDate(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                            IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasNotBeenConfirmed(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNoPurchaseDownPaymentDetail(purchaseDownPayment, _purchaseDownPaymentDetailService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VCreateObject(purchaseDownPayment, _purchaseDownPaymentService, _purchaseDownPaymentDetailService, _payableService, _contactService, _cashBankService);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService)
        {
            VHasNotBeenDeleted(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNoPurchaseDownPaymentDetail(purchaseDownPayment, _purchaseDownPaymentDetailService);
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

        public PurchaseDownPayment VConfirmObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                             IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService,
                                             IPayableService _payableService, IClosingService _closingService)
        {
            VHasConfirmationDate(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasPurchaseDownPaymentDetails(purchaseDownPayment, _purchaseDownPaymentDetailService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VTotalAmountIsNotZero(purchaseDownPayment, _purchaseDownPaymentDetailService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNotBeenConfirmed(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNotBeenDeleted(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VTotalAmountEqualDetailsAmount(purchaseDownPayment, _purchaseDownPaymentDetailService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VAllPurchaseDownPaymentDetailsAreConfirmable(purchaseDownPayment, _purchaseDownPaymentService, _purchaseDownPaymentDetailService, _cashBankService, _payableService);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VCashBankIsGreaterThanOrEqualPurchaseDownPaymentDetails(purchaseDownPayment, _purchaseDownPaymentDetailService, _cashBankService, !purchaseDownPayment.IsGBCH);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseDownPayment, _closingService, 1);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VUnconfirmObject(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService)
        {
            VHasBeenConfirmed(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNotBeenReconciled(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseDownPayment, _closingService, 2);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VReconcileObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VNotIsGBCH(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasBeenConfirmed(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasNotBeenReconciled(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasReconciliationDate(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseDownPayment, _closingService, 3);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VCashBankIsGreaterThanOrEqualPurchaseDownPaymentDetails(purchaseDownPayment, _purchaseDownPaymentDetailService, _cashBankService, true);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment VUnreconcileObject(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService)
        {
            VHasBeenConfirmed(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VHasBeenReconciled(purchaseDownPayment);
            if (!isValid(purchaseDownPayment)) { return purchaseDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(purchaseDownPayment, _closingService, 4);
            return purchaseDownPayment;
        }

        public bool ValidCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                      IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VCreateObject(purchaseDownPayment, _purchaseDownPaymentService, _purchaseDownPaymentDetailService, _payableService, _contactService, _cashBankService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                      IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            purchaseDownPayment.Errors.Clear();
            VUpdateObject(purchaseDownPayment, _purchaseDownPaymentService, _purchaseDownPaymentDetailService, _payableService, _contactService, _cashBankService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService)
        {
            purchaseDownPayment.Errors.Clear();
            VDeleteObject(purchaseDownPayment, _purchaseDownPaymentDetailService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidConfirmObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                       IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService,
                                       IPayableService _payableService, IClosingService _closingService)
        {
            purchaseDownPayment.Errors.Clear();
            VConfirmObject(purchaseDownPayment, _purchaseDownPaymentService, _purchaseDownPaymentDetailService, _cashBankService, _payableService, _closingService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidUnconfirmObject(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService)
        {
            purchaseDownPayment.Errors.Clear();
            VUnconfirmObject(purchaseDownPayment, _closingService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidReconcileObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            purchaseDownPayment.Errors.Clear();
            VReconcileObject(purchaseDownPayment, _purchaseDownPaymentDetailService, _cashBankService, _closingService);
            return isValid(purchaseDownPayment);
        }

        public bool ValidUnreconcileObject(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService)
        {
            purchaseDownPayment.Errors.Clear();
            VUnreconcileObject(purchaseDownPayment, _closingService);
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