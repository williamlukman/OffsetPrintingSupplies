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
    public class PaymentVoucherDetailValidator : IPaymentVoucherDetailValidator
    {
        public PaymentVoucherDetail VHasPaymentVoucher(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService)
        {
            PaymentVoucher paymentVoucher = _paymentVoucherService.GetObjectById(paymentVoucherDetail.PaymentVoucherId);
            if (paymentVoucher == null)
            {
                paymentVoucherDetail.Errors.Add("PaymentVoucherId", "Tidak boleh tidak ada");
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VHasPayable(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);
            if (payable == null)
            {
                paymentVoucherDetail.Errors.Add("PayableId", "Tidak boleh tidak ada");
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VHasNotBeenConfirmed(PaymentVoucherDetail paymentVoucherDetail)
        {
            if (paymentVoucherDetail.IsConfirmed)
            {
                paymentVoucherDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VHasBeenConfirmed(PaymentVoucherDetail paymentVoucherDetail)
        {
            if (!paymentVoucherDetail.IsConfirmed)
            {
                paymentVoucherDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VHasNotBeenDeleted(PaymentVoucherDetail paymentVoucherDetail)
        {
            if (paymentVoucherDetail.IsDeleted)
            {
                paymentVoucherDetail.Errors.Add("Generic", "Sudah didelete");
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VPayableHasNotBeenCompleted(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);
            if (payable.IsCompleted)
            {
                paymentVoucherDetail.Errors.Add("Generic", "Payable sudah complete");
            }
            return paymentVoucherDetail;
        }
        
        public PaymentVoucherDetail VNonNegativeAmount(PaymentVoucherDetail paymentVoucherDetail)
        {
            if (paymentVoucherDetail.Amount < 0)
            {
                paymentVoucherDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VAmountLessOrEqualPayable(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);
            if (paymentVoucherDetail.Amount > payable.Amount)
            {
                paymentVoucherDetail.Errors.Add("Amount", "Tidak boleh lebih dari payable");
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VUniquePayableId(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                                    IPayableService _payableService)
        {
            IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucherDetail.PaymentVoucherId);
            Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);
            foreach (var detail in paymentVoucherDetails)
            {
                if (detail.PayableId == paymentVoucherDetail.PayableId && detail.Id != paymentVoucherDetail.Id)
                {
                    paymentVoucherDetail.Errors.Add("Generic", "PayableId harus unique dibandingkan payment voucher details di dalam satu payment voucher");
                    return paymentVoucherDetail;
                }
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VDetailsAmountLessOrEqualPaymentVoucherTotal(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService,
                                                                                 IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucherDetail.PaymentVoucherId);
            decimal TotalPaymentVoucherDetails = 0;
            foreach (var detail in paymentVoucherDetails)
            {
                TotalPaymentVoucherDetails += detail.Amount;
            }
            PaymentVoucher paymentVoucher = _paymentVoucherService.GetObjectById(paymentVoucherDetail.PaymentVoucherId);
            if (paymentVoucher.TotalAmount < TotalPaymentVoucherDetails)
            {
                decimal sisa = paymentVoucher.TotalAmount - TotalPaymentVoucherDetails + paymentVoucherDetail.Amount;
                paymentVoucherDetail.Errors.Add("Generic", "Payment Voucher hanya menyediakan sisa dana sebesar " + sisa);
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VCreateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService,
                                                  IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VHasPaymentVoucher(paymentVoucherDetail, _paymentVoucherService);
            if (!isValid(paymentVoucherDetail)) { return paymentVoucherDetail; }
            VHasNotBeenConfirmed(paymentVoucherDetail);
            if (!isValid(paymentVoucherDetail)) { return paymentVoucherDetail; }
            VHasNotBeenDeleted(paymentVoucherDetail);
            if (!isValid(paymentVoucherDetail)) { return paymentVoucherDetail; }
            VHasPayable(paymentVoucherDetail, _payableService);
            if (!isValid(paymentVoucherDetail)) { return paymentVoucherDetail; }
            VPayableHasNotBeenCompleted(paymentVoucherDetail, _payableService);
            if (!isValid(paymentVoucherDetail)) { return paymentVoucherDetail; }
            VAmountLessOrEqualPayable(paymentVoucherDetail, _payableService);
            if (!isValid(paymentVoucherDetail)) { return paymentVoucherDetail; }
            VUniquePayableId(paymentVoucherDetail, _paymentVoucherDetailService, _payableService);
            if (!isValid(paymentVoucherDetail)) { return paymentVoucherDetail; }
            VDetailsAmountLessOrEqualPaymentVoucherTotal(paymentVoucherDetail, _paymentVoucherService, _paymentVoucherDetailService);
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VUpdateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService,
                                                  IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VHasNotBeenConfirmed(paymentVoucherDetail);
            if (!isValid(paymentVoucherDetail)) { return paymentVoucherDetail; }
            VCreateObject(paymentVoucherDetail, _paymentVoucherService, _paymentVoucherDetailService, _cashBankService, _payableService);
            return paymentVoucherDetail;    
        }

        public PaymentVoucherDetail VDeleteObject(PaymentVoucherDetail paymentVoucherDetail)
        {
            VHasNotBeenConfirmed(paymentVoucherDetail);
            if (!isValid(paymentVoucherDetail)) { return paymentVoucherDetail; }
            VHasNotBeenDeleted(paymentVoucherDetail);
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VHasConfirmationDate(PaymentVoucherDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PaymentVoucherDetail VConfirmObject(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService)
        {
            VHasConfirmationDate(paymentVoucherDetail);
            if (!isValid(paymentVoucherDetail)) { return paymentVoucherDetail; }
            VAmountLessOrEqualPayable(paymentVoucherDetail, _payableService);
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail VUnconfirmObject(PaymentVoucherDetail paymentVoucherDetail)
        {
            VHasBeenConfirmed(paymentVoucherDetail);
            return paymentVoucherDetail;
        }

        public bool ValidCreateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VCreateObject(paymentVoucherDetail, _paymentVoucherService, _paymentVoucherDetailService, _cashBankService, _payableService);
            return isValid(paymentVoucherDetail);
        }

        public bool ValidUpdateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VUpdateObject(paymentVoucherDetail, _paymentVoucherService, _paymentVoucherDetailService, _cashBankService, _payableService);
            return isValid(paymentVoucherDetail);
        }

        public bool ValidDeleteObject(PaymentVoucherDetail paymentVoucherDetail)
        {
            VDeleteObject(paymentVoucherDetail);
            return isValid(paymentVoucherDetail);
        }

        public bool ValidConfirmObject(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService)
        {
            VConfirmObject(paymentVoucherDetail, _payableService);
            return isValid(paymentVoucherDetail);
        }

        public bool ValidUnconfirmObject(PaymentVoucherDetail paymentVoucherDetail)
        {
            VUnconfirmObject(paymentVoucherDetail);
            return isValid(paymentVoucherDetail);
        }

        public bool isValid(PaymentVoucherDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PaymentVoucherDetail obj)
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