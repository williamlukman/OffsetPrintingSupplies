﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class PaymentVoucherValidator : IPaymentVoucherValidator
    {
        public PaymentVoucher VHasContact(PaymentVoucher paymentVoucher, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(paymentVoucher.ContactId);
            if (contact == null)
            {
                paymentVoucher.Errors.Add("ContactId", "Tidak boleh tidak ada");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasCashBank(PaymentVoucher paymentVoucher, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
            if (cashBank == null)
            {
                paymentVoucher.Errors.Add("CashBankId", "Tidak boleh tidak ada");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasPaymentDate(PaymentVoucher paymentVoucher)
        {
            if (paymentVoucher.PaymentDate == null)
            {
                paymentVoucher.Errors.Add("PaymentDate", "Tidak boleh kosong");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VNotIsGBCH(PaymentVoucher paymentVoucher)
        {
            if (!paymentVoucher.IsGBCH)
            {
                paymentVoucher.Errors.Add("Generic", "Non GBCH does not need reconcile");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VIfGBCHThenIsBank(PaymentVoucher paymentVoucher, ICashBankService _cashBankService)
        {
            if (paymentVoucher.IsGBCH)
            {
                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                if (!cashBank.IsBank)
                {
                    paymentVoucher.Errors.Add("Generic", "Jika GBCH Harus IsBank");
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher VIfGBCHThenHasGBCHNo(PaymentVoucher paymentVoucher)
        {
            if (paymentVoucher.IsGBCH)
            {
                if (paymentVoucher.GBCH_No == null || paymentVoucher.GBCH_No.Trim() == "")
                {
                    paymentVoucher.Errors.Add("GBCH_No", "Jika GBCH maka GBCH No. harus diisi");
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher VIfGBCHThenHasDueDate(PaymentVoucher paymentVoucher)
        {
            if (paymentVoucher.IsGBCH)
            {
                if (paymentVoucher.DueDate == null)
                {
                    paymentVoucher.Errors.Add("DueDate", "Jika GBCH maka DueDate harus diisi");
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasPaymentVoucherDetails(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
            if (!details.Any())
            {
                paymentVoucher.Errors.Add("Generic", "Harus memiliki payment voucher details");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasNoPaymentVoucherDetail(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
            if (details.Any())
            {
                paymentVoucher.Errors.Add("Generic", "Tidak boleh ada payment voucher details");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VTotalAmountIsNotZero(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            decimal totalamount = 0;
            IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
            foreach (var detail in details)
            {
                totalamount += detail.Amount;
            }
            if (totalamount == 0)
            {
                paymentVoucher.Errors.Add("Generic", "Total Amount tidak boleh 0");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasNotBeenDeleted(PaymentVoucher paymentVoucher)
        {
            if (paymentVoucher.IsDeleted)
            {
                paymentVoucher.Errors.Add("Generic", "Tidak boleh sudah di deleted");
            }
            return paymentVoucher;
        }
        
        public PaymentVoucher VHasNotBeenConfirmed(PaymentVoucher paymentVoucher)
        {
            if (paymentVoucher.IsConfirmed)
            {
                paymentVoucher.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasBeenConfirmed(PaymentVoucher paymentVoucher)
        {
            if (!paymentVoucher.IsConfirmed)
            {
                paymentVoucher.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VTotalAmountEqualDetailsAmount(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
            decimal detailsamount = 0;
            foreach (var detail in details)
            {
                detailsamount += detail.AmountPaid;
            }
            if (Math.Round(detailsamount, 2) != Math.Round(paymentVoucher.TotalAmount, 2))
            {
                paymentVoucher.Errors.Add("Generic", "Jumlah amount di details harus sama dengan totalamount");  
            }
            return paymentVoucher;
        }

        public PaymentVoucher VAllPaymentVoucherDetailsAreConfirmable(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService,
                                                              IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService,
                                                              IPayableService _payableService)
        {
            IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                detail.ConfirmationDate = paymentVoucher.ConfirmationDate;
                detail.Errors = new Dictionary<string, string>();
                if (!_paymentVoucherDetailService.GetValidator().ValidConfirmObject(detail, _payableService))
                {
                    foreach (var error in detail.Errors)
                    {
                        paymentVoucher.Errors.Add(error.Key, error.Value);
                    }
                    if (paymentVoucher.Errors.Any()) { return paymentVoucher; }
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher VCashBankIsGreaterThanOrEqualPaymentVoucherDetails(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, bool CasePayment)
        {
            if (CasePayment)
            {
                decimal totalamount = 0;
                IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach (var detail in details)
                {
                    totalamount += detail.AmountPaid; //Amount;
                }

                totalamount = totalamount - (paymentVoucher.TotalPPH21 + paymentVoucher.TotalPPH23 - paymentVoucher.BiayaBank + (paymentVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? paymentVoucher.Pembulatan : -paymentVoucher.Pembulatan));

                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                if (cashBank.Amount < totalamount)
                {
                    paymentVoucher.Errors.Add("Generic", "Cash bank tidak boleh kurang dari total amount");
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasBeenReconciled(PaymentVoucher paymentVoucher)
        {
            if (!paymentVoucher.IsReconciled)
            {
                paymentVoucher.Errors.Add("Generic", "Belum di reconcile");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasNotBeenReconciled(PaymentVoucher paymentVoucher)
        {
            if (paymentVoucher.IsReconciled)
            {
                paymentVoucher.Errors.Add("Generic", "Sudah di reconcile");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasReconciliationDate(PaymentVoucher paymentVoucher)
        {
            if (paymentVoucher.ReconciliationDate == null)
            {
                paymentVoucher.Errors.Add("ReconciliationDate", "Harus memiliki reconciliation date");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasValidRateToIDR(PaymentVoucher paymentVoucher)
        {
            if (paymentVoucher.RateToIDR <= 0)
            {
                paymentVoucher.Errors.Add("RateToIDR", "Harus lebih besar dari 0");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasValidBiayaBank(PaymentVoucher paymentVoucher)
        {
            if (paymentVoucher.BiayaBank < 0)
            {
                paymentVoucher.Errors.Add("BiayaBank", "Tidak boleh negatif");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasValidPembulatan(PaymentVoucher paymentVoucher)
        {
            if (paymentVoucher.Pembulatan < 0)
            {
                paymentVoucher.Errors.Add("Pembulatan", "Tidak boleh negatif");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VHasValidStatusPembulatan(PaymentVoucher paymentVoucher)
        {
            if (paymentVoucher.StatusPembulatan != Constant.GeneralLedgerStatus.Debit && paymentVoucher.StatusPembulatan != Constant.GeneralLedgerStatus.Credit)
            {
                paymentVoucher.Errors.Add("Status", "Harus Debit atau Credit");
            }
            return paymentVoucher;
        }

        public PaymentVoucher VGeneralLedgerPostingHasNotBeenClosed(PaymentVoucher paymentVoucher, IClosingService _closingService, int CaseConfirmUnconfirmReconcileUnreconcile)
        {
            switch(CaseConfirmUnconfirmReconcileUnreconcile)
            {
                case(1): // Confirm
                case(2): // Unconfirm
                {
                    if (_closingService.IsDateClosed(paymentVoucher.PaymentDate))
                    {
                        paymentVoucher.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
                case(3): // Reconcile
                case(4): // Unreconcile
                {
                    if (_closingService.IsDateClosed(paymentVoucher.ReconciliationDate.GetValueOrDefault()))
                    {
                        paymentVoucher.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher VCreateObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasContact(paymentVoucher, _contactService);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasCashBank(paymentVoucher, _cashBankService);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasPaymentDate(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VIfGBCHThenIsBank(paymentVoucher, _cashBankService);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VIfGBCHThenHasGBCHNo(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VIfGBCHThenHasDueDate(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasValidRateToIDR(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasValidBiayaBank(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasValidPembulatan(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasValidStatusPembulatan(paymentVoucher);
            return paymentVoucher;
        }

        public PaymentVoucher VUpdateObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasNotBeenConfirmed(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasNoPaymentVoucherDetail(paymentVoucher, _paymentVoucherDetailService);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VCreateObject(paymentVoucher, _paymentVoucherService, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);
            return paymentVoucher;
        }

        public PaymentVoucher VDeleteObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            VHasNotBeenDeleted(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasNoPaymentVoucherDetail(paymentVoucher, _paymentVoucherDetailService);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasNotBeenConfirmed(paymentVoucher);
            return paymentVoucher;
        }

        public PaymentVoucher VHasConfirmationDate(PaymentVoucher obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PaymentVoucher VConfirmObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService,
                                             IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService,
                                             IPayableService _payableService, IClosingService _closingService)
        {
            VHasConfirmationDate(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasPaymentVoucherDetails(paymentVoucher, _paymentVoucherDetailService);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VTotalAmountIsNotZero(paymentVoucher, _paymentVoucherDetailService);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasNotBeenConfirmed(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasNotBeenDeleted(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VTotalAmountEqualDetailsAmount(paymentVoucher, _paymentVoucherDetailService);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }            
            VAllPaymentVoucherDetailsAreConfirmable(paymentVoucher, _paymentVoucherService, _paymentVoucherDetailService, _cashBankService, _payableService);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VCashBankIsGreaterThanOrEqualPaymentVoucherDetails(paymentVoucher, _paymentVoucherDetailService, _cashBankService, !paymentVoucher.IsGBCH);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VGeneralLedgerPostingHasNotBeenClosed(paymentVoucher, _closingService, 1);
            return paymentVoucher;
        }

        public PaymentVoucher VUnconfirmObject(PaymentVoucher paymentVoucher, IClosingService _closingService)
        {
            VHasBeenConfirmed(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasNotBeenReconciled(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VGeneralLedgerPostingHasNotBeenClosed(paymentVoucher, _closingService, 2);
            return paymentVoucher;
        }

        public PaymentVoucher VReconcileObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VNotIsGBCH(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasBeenConfirmed(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasNotBeenReconciled(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasReconciliationDate(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VGeneralLedgerPostingHasNotBeenClosed(paymentVoucher, _closingService, 3);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VCashBankIsGreaterThanOrEqualPaymentVoucherDetails(paymentVoucher, _paymentVoucherDetailService, _cashBankService, true);
            return paymentVoucher;
        }

        public PaymentVoucher VUnreconcileObject(PaymentVoucher paymentVoucher, IClosingService _closingService)
        {
            VHasBeenConfirmed(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VHasBeenReconciled(paymentVoucher);
            if (!isValid(paymentVoucher)) { return paymentVoucher; }
            VGeneralLedgerPostingHasNotBeenClosed(paymentVoucher, _closingService, 4);
            return paymentVoucher;
        }

        public bool ValidCreateObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                      IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VCreateObject(paymentVoucher, _paymentVoucherService, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);
            return isValid(paymentVoucher);
        }

        public bool ValidUpdateObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                      IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            paymentVoucher.Errors.Clear();
            VUpdateObject(paymentVoucher, _paymentVoucherService, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);
            return isValid(paymentVoucher);
        }

        public bool ValidDeleteObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            paymentVoucher.Errors.Clear();
            VDeleteObject(paymentVoucher, _paymentVoucherDetailService);
            return isValid(paymentVoucher);
        }

        public bool ValidConfirmObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService,
                                       IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService,
                                       IPayableService _payableService, IClosingService _closingService)
        {
            paymentVoucher.Errors.Clear();
            VConfirmObject(paymentVoucher, _paymentVoucherService, _paymentVoucherDetailService, _cashBankService, _payableService, _closingService);
            return isValid(paymentVoucher);
        }

        public bool ValidUnconfirmObject(PaymentVoucher paymentVoucher, IClosingService _closingService)
        {
            paymentVoucher.Errors.Clear();
            VUnconfirmObject(paymentVoucher, _closingService);
            return isValid(paymentVoucher);
        }

        public bool ValidReconcileObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            paymentVoucher.Errors.Clear();
            VReconcileObject(paymentVoucher, _paymentVoucherDetailService, _cashBankService, _closingService);
            return isValid(paymentVoucher);
        }

        public bool ValidUnreconcileObject(PaymentVoucher paymentVoucher, IClosingService _closingService)
        {
            paymentVoucher.Errors.Clear();
            VUnreconcileObject(paymentVoucher, _closingService);
            return isValid(paymentVoucher);
        }

        public bool isValid(PaymentVoucher obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PaymentVoucher obj)
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