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
    public class SalesAllowanceValidator : ISalesAllowanceValidator
    {
        public SalesAllowance VHasContact(SalesAllowance salesAllowance, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(salesAllowance.ContactId);
            if (contact == null)
            {
                salesAllowance.Errors.Add("ContactId", "Tidak boleh tidak ada");
            }
            return salesAllowance;
        }

        public SalesAllowance VHasCashBank(SalesAllowance salesAllowance, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(salesAllowance.CashBankId);
            if (cashBank == null)
            {
                salesAllowance.Errors.Add("CashBankId", "Tidak boleh tidak ada");
            }
            return salesAllowance;
        }

        public SalesAllowance VHasReceiptDate(SalesAllowance salesAllowance)
        {
            if (salesAllowance.AllowanceAllocationDate == null)
            {
                salesAllowance.Errors.Add("AllowanceAllocationDate", "Tidak boleh kosong");
            }
            return salesAllowance;
        }

        public SalesAllowance VNotIsGBCH(SalesAllowance salesAllowance)
        {
            if (!salesAllowance.IsGBCH)
            {
                salesAllowance.Errors.Add("Generic", "Non GBCH does not need reconcile");
            }
            return salesAllowance;
        }

        public SalesAllowance VIfGBCHThenIsBank(SalesAllowance salesAllowance, ICashBankService _cashBankService)
        {
            if (salesAllowance.IsGBCH)
            {
                CashBank cashBank = _cashBankService.GetObjectById(salesAllowance.CashBankId);
                if (!cashBank.IsBank)
                {
                    salesAllowance.Errors.Add("IsBank", "Jika GBCH Harus IsBank");
                }
            }
            return salesAllowance;
        }

        public SalesAllowance VIfGBCHThenHasDueDate(SalesAllowance salesAllowance)
        {
            if (salesAllowance.IsGBCH)
            {
                if (salesAllowance.DueDate == null)
                {
                    salesAllowance.Errors.Add("DueDate", "Jika GBCH maka DueDate harus diisi");
                }
            }
            return salesAllowance;
        }

        public SalesAllowance VHasSalesAllowanceDetails(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService)
        {
            IList<SalesAllowanceDetail> details = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowance.Id);
            if (!details.Any())
            {
                salesAllowance.Errors.Add("Generic", "Harus memiliki receipt voucher details");
            }
            return salesAllowance;
        }

        public SalesAllowance VHasNoSalesAllowanceDetail(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService)
        {
            IList<SalesAllowanceDetail> details = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowance.Id);
            if (details.Any())
            {
                salesAllowance.Errors.Add("Generic", "Tidak boleh ada receipt voucher details");
            }
            return salesAllowance;
        }

        public SalesAllowance VTotalAmountIsNotZero(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService)
        {
            decimal totalamount = 0;
            IList<SalesAllowanceDetail> details = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowance.Id);
            foreach (var detail in details)
            {
                totalamount += detail.Amount;
            }
            if (totalamount == 0)
            {
                salesAllowance.Errors.Add("Generic", "Total Amount tidak boleh 0");
            }
            return salesAllowance;
        }

        public SalesAllowance VHasNotBeenDeleted(SalesAllowance salesAllowance)
        {
            if (salesAllowance.IsDeleted)
            {
                salesAllowance.Errors.Add("Generic", "Tidak boleh sudah di deleted");
            }
            return salesAllowance;
        }
        
        public SalesAllowance VHasNotBeenConfirmed(SalesAllowance salesAllowance)
        {
            if (salesAllowance.IsConfirmed)
            {
                salesAllowance.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return salesAllowance;
        }

        public SalesAllowance VHasBeenConfirmed(SalesAllowance salesAllowance)
        {
            if (!salesAllowance.IsConfirmed)
            {
                salesAllowance.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return salesAllowance;
        }

        public SalesAllowance VTotalAmountEqualDetailsAmount(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService)
        {
            IList<SalesAllowanceDetail> details = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowance.Id);
            decimal detailsamount = 0;
            foreach (var detail in details)
            {
                detailsamount += detail.Amount;
            }
            if (detailsamount != salesAllowance.TotalAmount)
            {
                salesAllowance.Errors.Add("Generic", "Jumlah amount di details harus sama dengan totalamount");
            }
            return salesAllowance;
        }

        public SalesAllowance VAllSalesAllowanceDetailsAreConfirmable(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService,
                                                                      ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService,
                                                                      IReceivableService _receivableService)
        {
            IList<SalesAllowanceDetail> details = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowance.Id);
            foreach (var detail in details)
            {
                detail.ConfirmationDate = salesAllowance.ConfirmationDate;
                detail.Errors = new Dictionary<string, string>();
                if (!_salesAllowanceDetailService.GetValidator().ValidConfirmObject(detail, _receivableService))
                {
                    foreach (var error in detail.Errors)
                    {
                        salesAllowance.Errors.Add(error.Key, error.Value);
                    }
                    if (salesAllowance.Errors.Any()) { return salesAllowance; }
                }
            }
            return salesAllowance;
        }

        public SalesAllowance VCashBankIsGreaterThanOrEqualSalesAllowanceDetails(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, bool CaseReceipt)
        {
            if (CaseReceipt)
            {
                decimal totalamount = 0;
                IList<SalesAllowanceDetail> details = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowance.Id);
                foreach (var detail in details)
                {
                    totalamount += detail.Amount;
                }

                CashBank cashBank = _cashBankService.GetObjectById(salesAllowance.CashBankId);
                if (cashBank.Amount < totalamount)
                {
                    salesAllowance.Errors.Add("Generic", "Cash bank tidak boleh kurang dari total amount");
                }
            }
            return salesAllowance;
        }


        public SalesAllowance VHasBeenReconciled(SalesAllowance salesAllowance)
        {
            if (!salesAllowance.IsReconciled)
            {
                salesAllowance.Errors.Add("Generic", "Belum di reconcile");
            }
            return salesAllowance;
        }

        public SalesAllowance VHasNotBeenReconciled(SalesAllowance salesAllowance)
        {
            if (salesAllowance.IsReconciled)
            {
                salesAllowance.Errors.Add("Generic", "Sudah di reconcile");
            }
            return salesAllowance;
        }

        public SalesAllowance VHasReconciliationDate(SalesAllowance salesAllowance)
        {
            if (salesAllowance.ReconciliationDate == null)
            {
                salesAllowance.Errors.Add("ReconciliationDate", "Harus memiliki reconciliation date");
            }
            return salesAllowance;
        }

        public SalesAllowance VGeneralLedgerPostingHasNotBeenClosed(SalesAllowance salesAllowance, IClosingService _closingService, int CaseConfirmUnconfirmReconcileUnreconcile)
        {
            switch (CaseConfirmUnconfirmReconcileUnreconcile)
            {
                case (1): // Confirm
                    {
                        if (_closingService.IsDateClosed(salesAllowance.ConfirmationDate.GetValueOrDefault()))
                        {
                            salesAllowance.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (2): // Unconfirm
                    {
                        if (_closingService.IsDateClosed(DateTime.Now))
                        {
                            salesAllowance.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (3): // Reconcile
                    {
                        if (_closingService.IsDateClosed(salesAllowance.ReconciliationDate.GetValueOrDefault()))
                        {
                            salesAllowance.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (4): // Unreconcile
                    {
                        if (_closingService.IsDateClosed(DateTime.Now))
                        {
                            salesAllowance.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
            }
            return salesAllowance;
        }

        public SalesAllowance VCreateObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                            IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasContact(salesAllowance, _contactService);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasCashBank(salesAllowance, _cashBankService);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasReceiptDate(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VIfGBCHThenIsBank(salesAllowance, _cashBankService);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VIfGBCHThenHasDueDate(salesAllowance);
            return salesAllowance;
        }

        public SalesAllowance VUpdateObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                            IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasNotBeenConfirmed(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasNoSalesAllowanceDetail(salesAllowance, _salesAllowanceDetailService);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VCreateObject(salesAllowance, _salesAllowanceService, _salesAllowanceDetailService, _receivableService, _contactService, _cashBankService);
            return salesAllowance;
        }

        public SalesAllowance VDeleteObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService)
        {
            VHasNotBeenDeleted(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasNoSalesAllowanceDetail(salesAllowance, _salesAllowanceDetailService);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasNotBeenConfirmed(salesAllowance);
            return salesAllowance;
        }

        public SalesAllowance VHasConfirmationDate(SalesAllowance obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;

        }
        public SalesAllowance VConfirmObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService,
                                             ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService,
                                             IReceivableService _receivableService, IClosingService _closingService)
        {
            VHasConfirmationDate(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasSalesAllowanceDetails(salesAllowance, _salesAllowanceDetailService);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VTotalAmountIsNotZero(salesAllowance, _salesAllowanceDetailService);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasNotBeenConfirmed(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasNotBeenDeleted(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VTotalAmountEqualDetailsAmount(salesAllowance, _salesAllowanceDetailService);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VAllSalesAllowanceDetailsAreConfirmable(salesAllowance, _salesAllowanceService, _salesAllowanceDetailService, _cashBankService, _receivableService);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VGeneralLedgerPostingHasNotBeenClosed(salesAllowance, _closingService, 1);
            return salesAllowance;
        }

        public SalesAllowance VUnconfirmObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                               ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasBeenConfirmed(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasNotBeenReconciled(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VGeneralLedgerPostingHasNotBeenClosed(salesAllowance, _closingService, 2);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VCashBankIsGreaterThanOrEqualSalesAllowanceDetails(salesAllowance, _salesAllowanceDetailService, _cashBankService, !salesAllowance.IsGBCH);
            return salesAllowance;
        }

        public SalesAllowance VReconcileObject(SalesAllowance salesAllowance, IClosingService _closingService)
        {
            VNotIsGBCH(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasBeenConfirmed(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasNotBeenReconciled(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasReconciliationDate(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VGeneralLedgerPostingHasNotBeenClosed(salesAllowance, _closingService, 3);
            return salesAllowance;
        }

        public SalesAllowance VUnreconcileObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService, 
                                                 ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasBeenConfirmed(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VHasBeenReconciled(salesAllowance);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VGeneralLedgerPostingHasNotBeenClosed(salesAllowance, _closingService, 4);
            if (!isValid(salesAllowance)) { return salesAllowance; }
            VCashBankIsGreaterThanOrEqualSalesAllowanceDetails(salesAllowance, _salesAllowanceDetailService, _cashBankService, true);
            return salesAllowance;
        }

        public bool ValidCreateObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                      IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VCreateObject(salesAllowance, _salesAllowanceService, _salesAllowanceDetailService, _receivableService, _contactService, _cashBankService);
            return isValid(salesAllowance);
        }

        public bool ValidUpdateObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                      IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            salesAllowance.Errors.Clear();
            VUpdateObject(salesAllowance, _salesAllowanceService, _salesAllowanceDetailService, _receivableService, _contactService, _cashBankService);
            return isValid(salesAllowance);
        }

        public bool ValidDeleteObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService)
        {
            salesAllowance.Errors.Clear();
            VDeleteObject(salesAllowance, _salesAllowanceDetailService);
            return isValid(salesAllowance);
        }

        public bool ValidConfirmObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService,
                                       ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService,
                                       IReceivableService _receivableService, IClosingService _closingService)
        {
            salesAllowance.Errors.Clear();
            VConfirmObject(salesAllowance, _salesAllowanceService, _salesAllowanceDetailService,
                           _cashBankService, _receivableService, _closingService);
            return isValid(salesAllowance);
        }

        public bool ValidUnconfirmObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            salesAllowance.Errors.Clear();
            VUnconfirmObject(salesAllowance, _salesAllowanceDetailService, _cashBankService, _closingService);
            return isValid(salesAllowance);
        }

        public bool ValidReconcileObject(SalesAllowance salesAllowance, IClosingService _closingService)
        {
            salesAllowance.Errors.Clear();
            VReconcileObject(salesAllowance, _closingService);
            return isValid(salesAllowance);
        }

        public bool ValidUnreconcileObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            salesAllowance.Errors.Clear();
            VUnreconcileObject(salesAllowance, _salesAllowanceDetailService, _cashBankService, _closingService);
            return isValid(salesAllowance);
        }

        public bool isValid(SalesAllowance obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesAllowance obj)
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