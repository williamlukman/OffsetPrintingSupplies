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

        public SalesDownPayment VHasReceiptVoucher(SalesDownPayment salesDownPayment, IReceiptVoucherService _receiptVoucherService)
        {
            ReceiptVoucher receiptVoucher = _receiptVoucherService.GetObjectById(salesDownPayment.ReceiptVoucherId);
            if (receiptVoucher == null)
            {
                salesDownPayment.Errors.Add("Generic", "ReceiptVoucherId tidak terasosiasi dengan payment voucher");
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
                    salesDownPayment.Errors.Add("Generic", "Jika GBCH Harus IsBank");
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

        public SalesDownPayment VSalesDownPaymentAllocationHasNoDetails(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                                       ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService)
        {
            SalesDownPaymentAllocation salesDownPaymentAllocation = _salesDownPaymentAllocationService.GetObjectBySalesDownPaymentId(salesDownPayment.Id);
            if (salesDownPaymentAllocation == null)
            {
                return salesDownPayment;
            }
            IList<SalesDownPaymentAllocationDetail> details = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);
            if (details.Any())
            {
                salesDownPayment.Errors.Add("Generic", "Sales DownPayment Allocation masih memiliki detail.");
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

        public SalesDownPayment VGeneralLedgerPostingHasNotBeenClosed(SalesDownPayment salesDownPayment, IClosingService _closingService, int CaseConfirmUnconfirmReconcileUnreconcile)
        {
            switch(CaseConfirmUnconfirmReconcileUnreconcile)
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

        public SalesDownPayment VCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService,
                                                 ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService)
        {
            VHasContact(salesDownPayment, _contactService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasCashBank(salesDownPayment, _cashBankService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasReceiptVoucher(salesDownPayment, _receiptVoucherService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VHasDownPaymentDate(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VIfGBCHThenIsBank(salesDownPayment, _cashBankService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VIfGBCHThenHasDueDate(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VTotalAmountNotNegativeNorZero(salesDownPayment);
            return salesDownPayment;
        }

        public SalesDownPayment VUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService,
                                                 ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService)
        {
            VHasNotBeenConfirmed(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VCreateObject(salesDownPayment, _salesDownPaymentService, _contactService, _cashBankService, _receiptVoucherService);
            return salesDownPayment;
        }

        public SalesDownPayment VDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, IReceiptVoucherDetailService _receiptVoucherDetailService)
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

        public SalesDownPayment VConfirmObject(SalesDownPayment salesDownPayment, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService,
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
            VCreateObject(salesDownPayment, _salesDownPaymentService, _contactService, _cashBankService, _receiptVoucherService);
            return salesDownPayment;
        }

        public SalesDownPayment VUnconfirmObject(SalesDownPayment salesDownPayment, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService,
                                   ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                   IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            VHasBeenConfirmed(salesDownPayment);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VSalesDownPaymentAllocationHasNoDetails(salesDownPayment, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService);
            if (!isValid(salesDownPayment)) { return salesDownPayment; }
            VGeneralLedgerPostingHasNotBeenClosed(salesDownPayment, _closingService, 2);
            return salesDownPayment;
        }

        public bool ValidCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService,
                                      ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService)
        {
            VCreateObject(salesDownPayment, _salesDownPaymentService, _contactService, _cashBankService, _receiptVoucherService);
            return isValid(salesDownPayment);
        }

        public bool ValidUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService,
                                      ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService)
        {
            salesDownPayment.Errors.Clear();
            VUpdateObject(salesDownPayment, _salesDownPaymentService, _contactService, _cashBankService, _receiptVoucherService);
            return isValid(salesDownPayment);
        }

        public bool ValidDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                      IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            salesDownPayment.Errors.Clear();
            VDeleteObject(salesDownPayment, _salesDownPaymentAllocationService, _receiptVoucherDetailService);
            return isValid(salesDownPayment);
        }

        public bool ValidConfirmObject(SalesDownPayment salesDownPayment, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService,
                                       ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService, IAccountService _accountService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPayment.Errors.Clear();
            VConfirmObject(salesDownPayment, _cashBankService, _receiptVoucherService, _salesDownPaymentService, _contactService,
                           _accountService, _generalLedgerJournalService, _closingService);
            return isValid(salesDownPayment);
        }

        public bool ValidUnconfirmObject(SalesDownPayment salesDownPayment, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService,
                                         ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPayment.Errors.Clear();
            VUnconfirmObject(salesDownPayment, _cashBankService, _receiptVoucherService, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService,
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