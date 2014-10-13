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

        public SalesDownPayment VHasCashBank(SalesDownPayment salesDownPayment, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(salesDownPayment.CashBankId);
            if (cashBank == null)
            {
                salesDownPayment.Errors.Add("CashBankId", "Tidak boleh tidak ada");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VHasReceiptDate(SalesDownPayment salesDownPayment)
        {
            if (salesDownPayment.DownPaymentDate == null)
            {
                salesDownPayment.Errors.Add("DownPaymentDate", "Tidak boleh kosong");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VNotIsGBCH(SalesDownPayment salesDownPayment)
        {
            if (!salesDownPayment.IsGBCH)
            {
                salesDownPayment.Errors.Add("Generic", "Non GBCH does not need reconcile");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VIfGBCHThenIsBank(SalesDownPayment salesDownPayment, ICashBankService _cashBankService)
        {
            if (salesDownPayment.IsGBCH)
            {
                CashBank cashBank = _cashBankService.GetObjectById(salesDownPayment.CashBankId);
                if (!cashBank.IsBank)
                {
                    salesDownPayment.Errors.Add("IsBank", "Jika GBCH Harus IsBank");
                }
            }
            return salesDownPayment;
        }

        public SalesDownPayment VIfGBCHThenHasDueDate(SalesDownPayment salesDownPayment)
        {
            if (salesDownPayment.IsGBCH)
            {
                if (salesDownPayment.DueDate == null)
                {
                    salesDownPayment.Errors.Add("DueDate", "Jika GBCH maka DueDate harus diisi");
                }
            }
            return salesDownPayment;
        }

        public SalesDownPayment VHasSalesDownPaymentDetails(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService)
        {
            IList<SalesDownPaymentDetail> details = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPayment.Id);
            if (!details.Any())
            {
                salesDownPayment.Errors.Add("Generic", "Harus memiliki receipt voucher details");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VHasNoSalesDownPaymentDetail(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService)
        {
            IList<SalesDownPaymentDetail> details = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPayment.Id);
            if (details.Any())
            {
                salesDownPayment.Errors.Add("Generic", "Tidak boleh ada receipt voucher details");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VTotalAmountIsNotZero(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService)
        {
            decimal totalamount = 0;
            IList<SalesDownPaymentDetail> details = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPayment.Id);
            foreach (var detail in details)
            {
                totalamount += detail.Amount;
            }
            if (totalamount == 0)
            {
                salesDownPayment.Errors.Add("Generic", "Total Amount tidak boleh 0");
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

        public SalesDownPayment VTotalAmountEqualDetailsAmount(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService)
        {
            IList<SalesDownPaymentDetail> details = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPayment.Id);
            decimal detailsamount = 0;
            foreach (var detail in details)
            {
                detailsamount += detail.Amount;
            }
            if (detailsamount != salesDownPayment.TotalAmount)
            {
                salesDownPayment.Errors.Add("Generic", "Jumlah amount di details harus sama dengan totalamount");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VAllSalesDownPaymentDetailsAreConfirmable(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService,
                                                                      ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService,
                                                                      IReceivableService _receivableService)
        {
            IList<SalesDownPaymentDetail> details = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPayment.Id);
            foreach (var detail in details)
            {
                detail.ConfirmationDate = salesDownPayment.ConfirmationDate;
                detail.Errors = new Dictionary<string, string>();
                if (!_salesDownPaymentDetailService.GetValidator().ValidConfirmObject(detail, _receivableService))
                {
                    foreach (var error in detail.Errors)
                    {
                        salesDownPayment.Errors.Add(error.Key, error.Value);
                    }
                    if (salesDownPayment.Errors.Any()) { return salesDownPayment; }
                }
            }
            return salesDownPayment;
        }

        public SalesDownPayment VCashBankIsGreaterThanOrEqualSalesDownPaymentDetails(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, bool CaseReceipt)
        {
            if (CaseReceipt)
            {
                decimal totalamount = 0;
                IList<SalesDownPaymentDetail> details = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPayment.Id);
                foreach (var detail in details)
                {
                    totalamount += detail.Amount;
                }

                CashBank cashBank = _cashBankService.GetObjectById(salesDownPayment.CashBankId);
                if (cashBank.Amount < totalamount)
                {
                    salesDownPayment.Errors.Add("Generic", "Cash bank tidak boleh kurang dari total amount");
                }
            }
            return salesDownPayment;
        }


        public SalesDownPayment VHasBeenReconciled(SalesDownPayment salesDownPayment)
        {
            if (!salesDownPayment.IsReconciled)
            {
                salesDownPayment.Errors.Add("Generic", "Belum di reconcile");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VHasNotBeenReconciled(SalesDownPayment salesDownPayment)
        {
            if (salesDownPayment.IsReconciled)
            {
                salesDownPayment.Errors.Add("Generic", "Sudah di reconcile");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VHasReconciliationDate(SalesDownPayment salesDownPayment)
        {
            if (salesDownPayment.ReconciliationDate == null)
            {
                salesDownPayment.Errors.Add("ReconciliationDate", "Harus memiliki reconciliation date");
            }
            return salesDownPayment;
        }

        public SalesDownPayment VGeneralLedgerPostingHasNotBeenClosed(SalesDownPayment salesDownPayment, IClosingService _closingService, int CaseConfirmUnconfirmReconcileUnreconcile)
        {
            switch (CaseConfirmUnconfirmReconcileUnreconcile)
            {
                case (1): // Confirm
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
                case (3): // Reconcile
                    {
                        if (_closingService.IsDateClosed(salesDownPayment.ReconciliationDate.GetValueOrDefault()))
                        {
                            salesDownPayment.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (4): // Unreconcile
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

        public SalesDownPayment VCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                            IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasContact(salesDownPayment, _contactService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasCashBank(salesDownPayment, _cashBankService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasReceiptDate(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VIfGBCHThenIsBank(salesDownPayment, _cashBankService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VIfGBCHThenHasDueDate(salesDownPayment);
            return salesDownPayment;
        }

        public SalesDownPayment VUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                            IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasNotBeenConfirmed(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasNoSalesDownPaymentDetail(salesDownPayment, _salesDownPaymentDetailService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VCreateObject(salesDownPayment, _salesDownPaymentService, _salesDownPaymentDetailService, _receivableService, _contactService, _cashBankService);
            return salesDownPayment;
        }

        public SalesDownPayment VDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService)
        {
            VHasNotBeenDeleted(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasNoSalesDownPaymentDetail(salesDownPayment, _salesDownPaymentDetailService);
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
        public SalesDownPayment VConfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService,
                                             ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService,
                                             IReceivableService _receivableService, IClosingService _closingService)
        {
            VHasConfirmationDate(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasSalesDownPaymentDetails(salesDownPayment, _salesDownPaymentDetailService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VTotalAmountIsNotZero(salesDownPayment, _salesDownPaymentDetailService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasNotBeenConfirmed(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasNotBeenDeleted(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VTotalAmountEqualDetailsAmount(salesDownPayment, _salesDownPaymentDetailService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VAllSalesDownPaymentDetailsAreConfirmable(salesDownPayment, _salesDownPaymentService, _salesDownPaymentDetailService, _cashBankService, _receivableService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(salesDownPayment, _closingService, 1);
            return salesDownPayment;
        }

        public SalesDownPayment VUnconfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                               ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasBeenConfirmed(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasNotBeenReconciled(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(salesDownPayment, _closingService, 2);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VCashBankIsGreaterThanOrEqualSalesDownPaymentDetails(salesDownPayment, _salesDownPaymentDetailService, _cashBankService, !salesDownPayment.IsGBCH);
            return salesDownPayment;
        }

        public SalesDownPayment VReconcileObject(SalesDownPayment salesDownPayment, IClosingService _closingService)
        {
            VNotIsGBCH(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasBeenConfirmed(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasNotBeenReconciled(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasReconciliationDate(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(salesDownPayment, _closingService, 3);
            return salesDownPayment;
        }

        public SalesDownPayment VUnreconcileObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService, 
                                                 ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasBeenConfirmed(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasBeenReconciled(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(salesDownPayment, _closingService, 4);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VCashBankIsGreaterThanOrEqualSalesDownPaymentDetails(salesDownPayment, _salesDownPaymentDetailService, _cashBankService, true);
            return salesDownPayment;
        }

        public bool ValidCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                      IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VCreateObject(salesDownPayment, _salesDownPaymentService, _salesDownPaymentDetailService, _receivableService, _contactService, _cashBankService);
            return isValid(salesDownPayment);
        }

        public bool ValidUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                      IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            salesDownPayment.Errors.Clear();
            VUpdateObject(salesDownPayment, _salesDownPaymentService, _salesDownPaymentDetailService, _receivableService, _contactService, _cashBankService);
            return isValid(salesDownPayment);
        }

        public bool ValidDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService)
        {
            salesDownPayment.Errors.Clear();
            VDeleteObject(salesDownPayment, _salesDownPaymentDetailService);
            return isValid(salesDownPayment);
        }

        public bool ValidConfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService,
                                       ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService,
                                       IReceivableService _receivableService, IClosingService _closingService)
        {
            salesDownPayment.Errors.Clear();
            VConfirmObject(salesDownPayment, _salesDownPaymentService, _salesDownPaymentDetailService,
                           _cashBankService, _receivableService, _closingService);
            return isValid(salesDownPayment);
        }

        public bool ValidUnconfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            salesDownPayment.Errors.Clear();
            VUnconfirmObject(salesDownPayment, _salesDownPaymentDetailService, _cashBankService, _closingService);
            return isValid(salesDownPayment);
        }

        public bool ValidReconcileObject(SalesDownPayment salesDownPayment, IClosingService _closingService)
        {
            salesDownPayment.Errors.Clear();
            VReconcileObject(salesDownPayment, _closingService);
            return isValid(salesDownPayment);
        }

        public bool ValidUnreconcileObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            salesDownPayment.Errors.Clear();
            VUnreconcileObject(salesDownPayment, _salesDownPaymentDetailService, _cashBankService, _closingService);
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