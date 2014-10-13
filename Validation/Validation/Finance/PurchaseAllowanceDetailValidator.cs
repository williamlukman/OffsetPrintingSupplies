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
    public class PurchaseAllowanceDetailValidator : IPurchaseAllowanceDetailValidator
    {
        public PurchaseAllowanceDetail VHasPurchaseAllowance(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService)
        {
            PurchaseAllowance purchaseAllowance = _purchaseAllowanceService.GetObjectById(purchaseAllowanceDetail.PurchaseAllowanceId);
            if (purchaseAllowance == null)
            {
                purchaseAllowanceDetail.Errors.Add("PurchaseAllowanceId", "Tidak boleh tidak ada");
            }
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VHasPayable(PurchaseAllowanceDetail purchaseAllowanceDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(purchaseAllowanceDetail.PayableId);
            if (payable == null)
            {
                purchaseAllowanceDetail.Errors.Add("PayableId", "Tidak boleh tidak ada");
            }
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VHasNotBeenConfirmed(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            if (purchaseAllowanceDetail.IsConfirmed)
            {
                purchaseAllowanceDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VHasBeenConfirmed(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            if (!purchaseAllowanceDetail.IsConfirmed)
            {
                purchaseAllowanceDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VHasNotBeenDeleted(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            if (purchaseAllowanceDetail.IsDeleted)
            {
                purchaseAllowanceDetail.Errors.Add("Generic", "Sudah didelete");
            }
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VPayableHasNotBeenCompleted(PurchaseAllowanceDetail purchaseAllowanceDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(purchaseAllowanceDetail.PayableId);
            if (payable.IsCompleted)
            {
                purchaseAllowanceDetail.Errors.Add("Generic", "Payable sudah complete");
            }
            return purchaseAllowanceDetail;
        }
        
        public PurchaseAllowanceDetail VNonNegativeAmount(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            if (purchaseAllowanceDetail.Amount < 0)
            {
                purchaseAllowanceDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VAmountLessOrEqualPayable(PurchaseAllowanceDetail purchaseAllowanceDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(purchaseAllowanceDetail.PayableId);
            if (purchaseAllowanceDetail.Amount > payable.Amount)
            {
                purchaseAllowanceDetail.Errors.Add("Amount", "Tidak boleh lebih dari payable");
            }
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VUniquePayableId(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                                    IPayableService _payableService)
        {
            IList<PurchaseAllowanceDetail> purchaseAllowanceDetails = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowanceDetail.PurchaseAllowanceId);
            Payable payable = _payableService.GetObjectById(purchaseAllowanceDetail.PayableId);
            foreach (var detail in purchaseAllowanceDetails)
            {
                if (detail.PayableId == purchaseAllowanceDetail.PayableId && detail.Id != purchaseAllowanceDetail.Id)
                {
                    purchaseAllowanceDetail.Errors.Add("Generic", "PayableId harus unique dibandingkan payment voucher details di dalam satu payment voucher");
                    return purchaseAllowanceDetail;
                }
            }
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VDetailsAmountLessOrEqualPurchaseAllowanceTotal(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService,
                                                                                 IPurchaseAllowanceDetailService _purchaseAllowanceDetailService)
        {
            IList<PurchaseAllowanceDetail> purchaseAllowanceDetails = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowanceDetail.PurchaseAllowanceId);
            decimal TotalPurchaseAllowanceDetails = 0;
            foreach (var detail in purchaseAllowanceDetails)
            {
                TotalPurchaseAllowanceDetails += detail.Amount;
            }
            PurchaseAllowance purchaseAllowance = _purchaseAllowanceService.GetObjectById(purchaseAllowanceDetail.PurchaseAllowanceId);
            if (purchaseAllowance.TotalAmount < TotalPurchaseAllowanceDetails)
            {
                decimal sisa = purchaseAllowance.TotalAmount - TotalPurchaseAllowanceDetails + purchaseAllowanceDetail.Amount;
                purchaseAllowanceDetail.Errors.Add("Generic", "Payment Voucher hanya menyediakan sisa dana sebesar " + sisa);
            }
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VCreateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService,
                                                  IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VHasPurchaseAllowance(purchaseAllowanceDetail, _purchaseAllowanceService);
            if (!isValid(purchaseAllowanceDetail)) { return purchaseAllowanceDetail; }
            VHasNotBeenConfirmed(purchaseAllowanceDetail);
            if (!isValid(purchaseAllowanceDetail)) { return purchaseAllowanceDetail; }
            VHasNotBeenDeleted(purchaseAllowanceDetail);
            if (!isValid(purchaseAllowanceDetail)) { return purchaseAllowanceDetail; }
            VHasPayable(purchaseAllowanceDetail, _payableService);
            if (!isValid(purchaseAllowanceDetail)) { return purchaseAllowanceDetail; }
            VPayableHasNotBeenCompleted(purchaseAllowanceDetail, _payableService);
            if (!isValid(purchaseAllowanceDetail)) { return purchaseAllowanceDetail; }
            VAmountLessOrEqualPayable(purchaseAllowanceDetail, _payableService);
            if (!isValid(purchaseAllowanceDetail)) { return purchaseAllowanceDetail; }
            VUniquePayableId(purchaseAllowanceDetail, _purchaseAllowanceDetailService, _payableService);
            if (!isValid(purchaseAllowanceDetail)) { return purchaseAllowanceDetail; }
            VDetailsAmountLessOrEqualPurchaseAllowanceTotal(purchaseAllowanceDetail, _purchaseAllowanceService, _purchaseAllowanceDetailService);
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VUpdateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService,
                                                  IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VHasNotBeenConfirmed(purchaseAllowanceDetail);
            if (!isValid(purchaseAllowanceDetail)) { return purchaseAllowanceDetail; }
            VCreateObject(purchaseAllowanceDetail, _purchaseAllowanceService, _purchaseAllowanceDetailService, _cashBankService, _payableService);
            return purchaseAllowanceDetail;    
        }

        public PurchaseAllowanceDetail VDeleteObject(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            VHasNotBeenConfirmed(purchaseAllowanceDetail);
            if (!isValid(purchaseAllowanceDetail)) { return purchaseAllowanceDetail; }
            VHasNotBeenDeleted(purchaseAllowanceDetail);
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VHasConfirmationDate(PurchaseAllowanceDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PurchaseAllowanceDetail VConfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPayableService _payableService)
        {
            VHasConfirmationDate(purchaseAllowanceDetail);
            if (!isValid(purchaseAllowanceDetail)) { return purchaseAllowanceDetail; }
            VAmountLessOrEqualPayable(purchaseAllowanceDetail, _payableService);
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail VUnconfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            VHasBeenConfirmed(purchaseAllowanceDetail);
            return purchaseAllowanceDetail;
        }

        public bool ValidCreateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VCreateObject(purchaseAllowanceDetail, _purchaseAllowanceService, _purchaseAllowanceDetailService, _cashBankService, _payableService);
            return isValid(purchaseAllowanceDetail);
        }

        public bool ValidUpdateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            VUpdateObject(purchaseAllowanceDetail, _purchaseAllowanceService, _purchaseAllowanceDetailService, _cashBankService, _payableService);
            return isValid(purchaseAllowanceDetail);
        }

        public bool ValidDeleteObject(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            VDeleteObject(purchaseAllowanceDetail);
            return isValid(purchaseAllowanceDetail);
        }

        public bool ValidConfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPayableService _payableService)
        {
            VConfirmObject(purchaseAllowanceDetail, _payableService);
            return isValid(purchaseAllowanceDetail);
        }

        public bool ValidUnconfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            VUnconfirmObject(purchaseAllowanceDetail);
            return isValid(purchaseAllowanceDetail);
        }

        public bool isValid(PurchaseAllowanceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseAllowanceDetail obj)
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