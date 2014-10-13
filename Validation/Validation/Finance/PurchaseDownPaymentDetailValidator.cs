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
    public class PurchaseDownPaymentDetailValidator : IPurchaseDownPaymentDetailValidator
    {
        public PurchaseDownPaymentDetail VHasPurchaseDownPayment(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService)
        {
            PurchaseDownPayment purchaseDownPayment = _purchaseDownPaymentService.GetObjectById(purchaseDownPaymentDetail.PurchaseDownPaymentId);
            if (purchaseDownPayment == null)
            {
                purchaseDownPaymentDetail.Errors.Add("PurchaseDownPaymentId", "Tidak boleh tidak ada");
            }
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VHasPayable(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(purchaseDownPaymentDetail.PayableId);
            if (payable == null)
            {
                purchaseDownPaymentDetail.Errors.Add("PayableId", "Tidak boleh tidak ada");
            }
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VHasNotBeenConfirmed(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            if (purchaseDownPaymentDetail.IsConfirmed)
            {
                purchaseDownPaymentDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VHasBeenConfirmed(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            if (!purchaseDownPaymentDetail.IsConfirmed)
            {
                purchaseDownPaymentDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VHasNotBeenDeleted(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            if (purchaseDownPaymentDetail.IsDeleted)
            {
                purchaseDownPaymentDetail.Errors.Add("Generic", "Sudah didelete");
            }
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VPayableHasNotBeenCompleted(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(purchaseDownPaymentDetail.PayableId);
            if (payable.IsCompleted)
            {
                purchaseDownPaymentDetail.Errors.Add("Generic", "Payable sudah complete");
            }
            return purchaseDownPaymentDetail;
        }
        
        public PurchaseDownPaymentDetail VNonNegativeAmount(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            if (purchaseDownPaymentDetail.Amount < 0)
            {
                purchaseDownPaymentDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VAmountLessOrEqualPayable(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(purchaseDownPaymentDetail.PayableId);
            if (purchaseDownPaymentDetail.Amount > payable.Amount)
            {
                purchaseDownPaymentDetail.Errors.Add("Amount", "Tidak boleh lebih dari payable");
            }
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VUniquePayableId(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                                    IPayableService _payableService)
        {
            IList<PurchaseDownPaymentDetail> purchaseDownPaymentDetails = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPaymentDetail.PurchaseDownPaymentId);
            Payable payable = _payableService.GetObjectById(purchaseDownPaymentDetail.PayableId);
            foreach (var detail in purchaseDownPaymentDetails)
            {
                if (detail.PayableId == purchaseDownPaymentDetail.PayableId && detail.Id != purchaseDownPaymentDetail.Id)
                {
                    purchaseDownPaymentDetail.Errors.Add("Generic", "PayableId harus unique dibandingkan payment voucher details di dalam satu payment voucher");
                    return purchaseDownPaymentDetail;
                }
            }
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VDetailsAmountLessOrEqualPurchaseDownPaymentTotal(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                                                 IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService)
        {
            IList<PurchaseDownPaymentDetail> purchaseDownPaymentDetails = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPaymentDetail.PurchaseDownPaymentId);
            decimal TotalPurchaseDownPaymentDetails = 0;
            foreach (var detail in purchaseDownPaymentDetails)
            {
                TotalPurchaseDownPaymentDetails += detail.Amount;
            }
            PurchaseDownPayment purchaseDownPayment = _purchaseDownPaymentService.GetObjectById(purchaseDownPaymentDetail.PurchaseDownPaymentId);
            if (purchaseDownPayment.TotalAmount < TotalPurchaseDownPaymentDetails)
            {
                decimal sisa = purchaseDownPayment.TotalAmount - TotalPurchaseDownPaymentDetails + purchaseDownPaymentDetail.Amount;
                purchaseDownPaymentDetail.Errors.Add("Generic", "Payment Voucher hanya menyediakan sisa dana sebesar " + sisa);
            }
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VCreateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                  IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VHasPurchaseDownPayment(purchaseDownPaymentDetail, _purchaseDownPaymentService);
            if (!isValid(purchaseDownPaymentDetail)) { return purchaseDownPaymentDetail; }
            VHasNotBeenConfirmed(purchaseDownPaymentDetail);
            if (!isValid(purchaseDownPaymentDetail)) { return purchaseDownPaymentDetail; }
            VHasNotBeenDeleted(purchaseDownPaymentDetail);
            if (!isValid(purchaseDownPaymentDetail)) { return purchaseDownPaymentDetail; }
            VHasPayable(purchaseDownPaymentDetail, _payableService);
            if (!isValid(purchaseDownPaymentDetail)) { return purchaseDownPaymentDetail; }
            VPayableHasNotBeenCompleted(purchaseDownPaymentDetail, _payableService);
            if (!isValid(purchaseDownPaymentDetail)) { return purchaseDownPaymentDetail; }
            VAmountLessOrEqualPayable(purchaseDownPaymentDetail, _payableService);
            if (!isValid(purchaseDownPaymentDetail)) { return purchaseDownPaymentDetail; }
            VUniquePayableId(purchaseDownPaymentDetail, _purchaseDownPaymentDetailService, _payableService);
            if (!isValid(purchaseDownPaymentDetail)) { return purchaseDownPaymentDetail; }
            VDetailsAmountLessOrEqualPurchaseDownPaymentTotal(purchaseDownPaymentDetail, _purchaseDownPaymentService, _purchaseDownPaymentDetailService);
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VUpdateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                  IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VHasNotBeenConfirmed(purchaseDownPaymentDetail);
            if (!isValid(purchaseDownPaymentDetail)) { return purchaseDownPaymentDetail; }
            VCreateObject(purchaseDownPaymentDetail, _purchaseDownPaymentService, _purchaseDownPaymentDetailService, _cashBankService, _payableService);
            return purchaseDownPaymentDetail;    
        }

        public PurchaseDownPaymentDetail VDeleteObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            VHasNotBeenConfirmed(purchaseDownPaymentDetail);
            if (!isValid(purchaseDownPaymentDetail)) { return purchaseDownPaymentDetail; }
            VHasNotBeenDeleted(purchaseDownPaymentDetail);
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VHasConfirmationDate(PurchaseDownPaymentDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PurchaseDownPaymentDetail VConfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPayableService _payableService)
        {
            VHasConfirmationDate(purchaseDownPaymentDetail);
            if (!isValid(purchaseDownPaymentDetail)) { return purchaseDownPaymentDetail; }
            VAmountLessOrEqualPayable(purchaseDownPaymentDetail, _payableService);
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail VUnconfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            VHasBeenConfirmed(purchaseDownPaymentDetail);
            return purchaseDownPaymentDetail;
        }

        public bool ValidCreateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VCreateObject(purchaseDownPaymentDetail, _purchaseDownPaymentService, _purchaseDownPaymentDetailService, _cashBankService, _payableService);
            return isValid(purchaseDownPaymentDetail);
        }

        public bool ValidUpdateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VUpdateObject(purchaseDownPaymentDetail, _purchaseDownPaymentService, _purchaseDownPaymentDetailService, _cashBankService, _payableService);
            return isValid(purchaseDownPaymentDetail);
        }

        public bool ValidDeleteObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            VDeleteObject(purchaseDownPaymentDetail);
            return isValid(purchaseDownPaymentDetail);
        }

        public bool ValidConfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPayableService _payableService)
        {
            VConfirmObject(purchaseDownPaymentDetail, _payableService);
            return isValid(purchaseDownPaymentDetail);
        }

        public bool ValidUnconfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            VUnconfirmObject(purchaseDownPaymentDetail);
            return isValid(purchaseDownPaymentDetail);
        }

        public bool isValid(PurchaseDownPaymentDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseDownPaymentDetail obj)
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