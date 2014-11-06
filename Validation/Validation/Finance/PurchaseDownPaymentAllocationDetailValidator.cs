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
    public class PurchaseDownPaymentAllocationDetailValidator : IPurchaseDownPaymentAllocationDetailValidator
    {
        public PurchaseDownPaymentAllocationDetail VHasPurchaseDownPaymentAllocation(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService)
        {
            PurchaseDownPaymentAllocation purchaseDownPaymentAllocation = _purchaseDownPaymentAllocationService.GetObjectById(purchaseDownPaymentAllocationDetail.PurchaseDownPaymentAllocationId);
            if (purchaseDownPaymentAllocation == null)
            {
                purchaseDownPaymentAllocationDetail.Errors.Add("PurchaseDownPaymentAllocationId", "Tidak boleh tidak ada");
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VHasPayable(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(purchaseDownPaymentAllocationDetail.PayableId);
            if (payable == null)
            {
                purchaseDownPaymentAllocationDetail.Errors.Add("PayableId", "Tidak boleh tidak ada");
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VHasNotBeenConfirmed(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            if (purchaseDownPaymentAllocationDetail.IsConfirmed)
            {
                purchaseDownPaymentAllocationDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VHasBeenConfirmed(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            if (!purchaseDownPaymentAllocationDetail.IsConfirmed)
            {
                purchaseDownPaymentAllocationDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VHasNotBeenDeleted(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            if (purchaseDownPaymentAllocationDetail.IsDeleted)
            {
                purchaseDownPaymentAllocationDetail.Errors.Add("Generic", "Sudah didelete");
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VPayableHasNotBeenCompleted(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(purchaseDownPaymentAllocationDetail.PayableId);
            if (payable.IsCompleted)
            {
                purchaseDownPaymentAllocationDetail.Errors.Add("Generic", "Payable sudah complete");
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VNonNegativeAmount(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            if (purchaseDownPaymentAllocationDetail.Amount < 0)
            {
                purchaseDownPaymentAllocationDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VAmountLessOrEqualPayable(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(purchaseDownPaymentAllocationDetail.PayableId);
            if (purchaseDownPaymentAllocationDetail.Amount > payable.Amount)
            {
                purchaseDownPaymentAllocationDetail.Errors.Add("Amount", "Tidak boleh lebih dari payable");
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VUniquePayableId(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                                   IPayableService _payableService)
        {
            IList<PurchaseDownPaymentAllocationDetail> purchaseDownPaymentAllocationDetails = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocationDetail.PurchaseDownPaymentAllocationId);
            Payable payable = _payableService.GetObjectById(purchaseDownPaymentAllocationDetail.PayableId);
            foreach (var detail in purchaseDownPaymentAllocationDetails)
            {
                if (detail.PayableId == purchaseDownPaymentAllocationDetail.PayableId && detail.Id != purchaseDownPaymentAllocationDetail.Id)
                {
                    purchaseDownPaymentAllocationDetail.Errors.Add("Generic", "PayableId harus unique dibandingkan payment voucher details di dalam satu payment voucher");
                    return purchaseDownPaymentAllocationDetail;
                }
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VDetailsAmountLessOrEqualPurchaseDownPaymentTotal(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                                                 IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService)
        {
            IList<PurchaseDownPaymentAllocationDetail> purchaseDownPaymentAllocationDetails = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocationDetail.PurchaseDownPaymentAllocationId);
            decimal TotalPurchaseDownPaymentAllocationDetails = 0;
            foreach (var detail in purchaseDownPaymentAllocationDetails)
            {
                TotalPurchaseDownPaymentAllocationDetails += detail.Amount;
            }
            PurchaseDownPaymentAllocation purchaseDownPaymentAllocation = _purchaseDownPaymentAllocationService.GetObjectById(purchaseDownPaymentAllocationDetail.PurchaseDownPaymentAllocationId);
            if (purchaseDownPaymentAllocation.TotalAmount < TotalPurchaseDownPaymentAllocationDetails)
            {
                decimal sisa = purchaseDownPaymentAllocation.TotalAmount - TotalPurchaseDownPaymentAllocationDetails + purchaseDownPaymentAllocationDetail.Amount;
                purchaseDownPaymentAllocationDetail.Errors.Add("Generic", "Payment Voucher hanya menyediakan sisa dana sebesar " + sisa);
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VCreateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                   IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                   IPayableService _payableService, IReceivableService _receivableService)
        {
            VHasPurchaseDownPaymentAllocation(purchaseDownPaymentAllocationDetail, _purchaseDownPaymentAllocationService);
            if (!isValid(purchaseDownPaymentAllocationDetail)) { return purchaseDownPaymentAllocationDetail; }
            VHasNotBeenConfirmed(purchaseDownPaymentAllocationDetail);
            if (!isValid(purchaseDownPaymentAllocationDetail)) { return purchaseDownPaymentAllocationDetail; }
            VHasNotBeenDeleted(purchaseDownPaymentAllocationDetail);
            if (!isValid(purchaseDownPaymentAllocationDetail)) { return purchaseDownPaymentAllocationDetail; }
            VHasPayable(purchaseDownPaymentAllocationDetail, _payableService);
            if (!isValid(purchaseDownPaymentAllocationDetail)) { return purchaseDownPaymentAllocationDetail; }
            VPayableHasNotBeenCompleted(purchaseDownPaymentAllocationDetail, _payableService);
            if (!isValid(purchaseDownPaymentAllocationDetail)) { return purchaseDownPaymentAllocationDetail; }
            VAmountLessOrEqualPayable(purchaseDownPaymentAllocationDetail, _payableService);
            if (!isValid(purchaseDownPaymentAllocationDetail)) { return purchaseDownPaymentAllocationDetail; }
            VUniquePayableId(purchaseDownPaymentAllocationDetail, _purchaseDownPaymentAllocationDetailService, _payableService);
            if (!isValid(purchaseDownPaymentAllocationDetail)) { return purchaseDownPaymentAllocationDetail; }
            VDetailsAmountLessOrEqualPurchaseDownPaymentTotal(purchaseDownPaymentAllocationDetail, _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService);
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VUpdateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                   IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                   IPayableService _payableService, IReceivableService _receivableService)
        {
            VHasNotBeenConfirmed(purchaseDownPaymentAllocationDetail);
            if (!isValid(purchaseDownPaymentAllocationDetail)) { return purchaseDownPaymentAllocationDetail; }
            VCreateObject(purchaseDownPaymentAllocationDetail, _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _payableService, _receivableService);
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VDeleteObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            VHasNotBeenConfirmed(purchaseDownPaymentAllocationDetail);
            if (!isValid(purchaseDownPaymentAllocationDetail)) { return purchaseDownPaymentAllocationDetail; }
            VHasNotBeenDeleted(purchaseDownPaymentAllocationDetail);
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VHasConfirmationDate(PurchaseDownPaymentAllocationDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PurchaseDownPaymentAllocationDetail VConfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService, IReceivableService _receivableService)
        {
            VHasConfirmationDate(purchaseDownPaymentAllocationDetail);
            if (!isValid(purchaseDownPaymentAllocationDetail)) { return purchaseDownPaymentAllocationDetail; }
            VAmountLessOrEqualPayable(purchaseDownPaymentAllocationDetail, _payableService);
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail VUnconfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                                                 IPayableService _payableService, IReceivableService _receivableService)
        {
            VHasBeenConfirmed(purchaseDownPaymentAllocationDetail);
            return purchaseDownPaymentAllocationDetail;
        }

        public bool ValidCreateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                      IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                      IPayableService _payableService, IReceivableService _receivableService)
        {
            VCreateObject(purchaseDownPaymentAllocationDetail, _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService,
                          _payableService, _receivableService);
            return isValid(purchaseDownPaymentAllocationDetail);
        }

        public bool ValidUpdateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                      IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                      IPayableService _payableService, IReceivableService _receivableService)
        {
            VUpdateObject(purchaseDownPaymentAllocationDetail, _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService,
                          _payableService, _receivableService);
            return isValid(purchaseDownPaymentAllocationDetail);
        }

        public bool ValidDeleteObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            VDeleteObject(purchaseDownPaymentAllocationDetail);
            return isValid(purchaseDownPaymentAllocationDetail);
        }

        public bool ValidConfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService, IReceivableService _receivableService)
        {
            VConfirmObject(purchaseDownPaymentAllocationDetail, _payableService, _receivableService);
            return isValid(purchaseDownPaymentAllocationDetail);
        }

        public bool ValidUnconfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService, IReceivableService _receivableService)
        {
            VUnconfirmObject(purchaseDownPaymentAllocationDetail, _payableService, _receivableService);
            return isValid(purchaseDownPaymentAllocationDetail);
        }

        public bool isValid(PurchaseDownPaymentAllocationDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseDownPaymentAllocationDetail obj)
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