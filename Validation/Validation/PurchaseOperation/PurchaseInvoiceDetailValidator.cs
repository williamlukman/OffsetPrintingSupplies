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
    public class PurchaseInvoiceDetailValidator : IPurchaseInvoiceDetailValidator
    {

        public PurchaseInvoiceDetail VHasPurchaseInvoice(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService)
        {
            PurchaseInvoice purchaseInvoice = _purchaseInvoiceService.GetObjectById(purchaseInvoiceDetail.PurchaseInvoiceId);
            if (purchaseInvoice == null)
            {
                purchaseInvoiceDetail.Errors.Add("PurchaseInvoiceId", "Tidak terasosiasi dengan purchase invoice");
            }
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VHasPurchaseReceivalDetail(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            PurchaseReceivalDetail prd = _purchaseReceivalDetailService.GetObjectById(purchaseInvoiceDetail.PurchaseReceivalDetailId);
            if (prd == null)
            {
                purchaseInvoiceDetail.Errors.Add("PurchaseReceivalDetail", "Tidak boleh tidak ada");
            }
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VPurchaseReceivalDetailAndPurchaseInvoiceMustHaveTheSamePurchaseReceival(PurchaseInvoiceDetail purchaseInvoiceDetail,
                                     IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseInvoiceService _purchaseInvoiceService)
        {
            PurchaseReceivalDetail purchaseReceivalDetail = _purchaseReceivalDetailService.GetObjectById(purchaseInvoiceDetail.PurchaseReceivalDetailId);
            PurchaseInvoice purchaseInvoice = _purchaseInvoiceService.GetObjectById(purchaseInvoiceDetail.PurchaseInvoiceId);
            if (purchaseReceivalDetail.PurchaseReceivalId != purchaseInvoice.PurchaseReceivalId)
            {
                purchaseReceivalDetail.Errors.Add("Generic", "Purchase receival detail dan purchase invoice memiliki purchase receival berbeda");
            }
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VIsUniquePurchaseReceivalDetail(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseInvoiceDetail> details = _purchaseInvoiceDetail.GetObjectsByPurchaseInvoiceId(purchaseInvoiceDetail.PurchaseInvoiceId);
            foreach (var detail in details)
            {
                if (detail.PurchaseReceivalDetailId == purchaseInvoiceDetail.PurchaseReceivalDetailId && detail.Id != purchaseInvoiceDetail.Id)
                {
                    purchaseInvoiceDetail.Errors.Add("PurchaseInvoiceDetail", "Tidak boleh memiliki lebih dari 2 Purchase Receival Detail");
                }
            }
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VNonNegativeNorZeroQuantity(PurchaseInvoiceDetail purchaseInvoiceDetail)
        {
            if (purchaseInvoiceDetail.Quantity <= 0)
            {
                purchaseInvoiceDetail.Errors.Add("Quantity", "Harus lebih besar dari 0");
            }
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VQuantityIsLessThanOrEqualPendingInvoiceQuantity(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            PurchaseReceivalDetail purchaseReceivalDetail = _purchaseReceivalDetailService.GetObjectById(purchaseInvoiceDetail.PurchaseReceivalDetailId);
            if (purchaseInvoiceDetail.Quantity > purchaseReceivalDetail.PendingInvoicedQuantity)
            {
                purchaseInvoiceDetail.Errors.Add("Generic", "Quantity harus kurang dari atau sama dengan pending invoice quantity");
            }
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VHasBeenConfirmed(PurchaseInvoiceDetail purchaseInvoiceDetail)
        {
            if (!purchaseInvoiceDetail.IsConfirmed)
            {
                purchaseInvoiceDetail.Errors.Add("IsConfirmed", "Belum dikonfirmasi");
            }
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VHasNotBeenConfirmed(PurchaseInvoiceDetail purchaseInvoiceDetail)
        {
            if (purchaseInvoiceDetail.IsConfirmed)
            {
                purchaseInvoiceDetail.Errors.Add("IsConfirmed", "Sudah dikonfirmasi");
            }
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VCreateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService,
                                                   IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VHasPurchaseInvoice(purchaseInvoiceDetail, _purchaseInvoiceService);
            if (!isValid(purchaseInvoiceDetail)) { return purchaseInvoiceDetail; }
            VHasPurchaseReceivalDetail(purchaseInvoiceDetail, _purchaseReceivalDetailService);
            if (!isValid(purchaseInvoiceDetail)) { return purchaseInvoiceDetail; }
            VPurchaseReceivalDetailAndPurchaseInvoiceMustHaveTheSamePurchaseReceival(purchaseInvoiceDetail, _purchaseReceivalDetailService, _purchaseInvoiceService);
            if (!isValid(purchaseInvoiceDetail)) { return purchaseInvoiceDetail; }
            VIsUniquePurchaseReceivalDetail(purchaseInvoiceDetail, _purchaseInvoiceDetailService, _purchaseReceivalDetailService);
            if (!isValid(purchaseInvoiceDetail)) { return purchaseInvoiceDetail; }
            VNonNegativeNorZeroQuantity(purchaseInvoiceDetail);
            if (!isValid(purchaseInvoiceDetail)) { return purchaseInvoiceDetail; }
            VQuantityIsLessThanOrEqualPendingInvoiceQuantity(purchaseInvoiceDetail, _purchaseReceivalDetailService);
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VUpdateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService,
                                                   IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VHasNotBeenConfirmed(purchaseInvoiceDetail);
            if (!isValid(purchaseInvoiceDetail)) { return purchaseInvoiceDetail; }
            VCreateObject(purchaseInvoiceDetail, _purchaseInvoiceService, _purchaseInvoiceDetailService, _purchaseReceivalDetailService);
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VDeleteObject(PurchaseInvoiceDetail purchaseInvoiceDetail)
        {
            VHasNotBeenConfirmed(purchaseInvoiceDetail);
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VConfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VHasNotBeenConfirmed(purchaseInvoiceDetail);
            if (!isValid(purchaseInvoiceDetail)) { return purchaseInvoiceDetail; }
            VQuantityIsLessThanOrEqualPendingInvoiceQuantity(purchaseInvoiceDetail, _purchaseReceivalDetailService);
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail VUnconfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail)
        {
            VHasBeenConfirmed(purchaseInvoiceDetail);
            return purchaseInvoiceDetail;
        }

        public bool ValidCreateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService,
                                      IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VCreateObject(purchaseInvoiceDetail, _purchaseInvoiceService, _purchaseInvoiceDetailService, _purchaseReceivalDetailService);
            return isValid(purchaseInvoiceDetail);
        }

        public bool ValidUpdateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseInvoiceDetail.Errors.Clear();
            VUpdateObject(purchaseInvoiceDetail, _purchaseInvoiceService, _purchaseInvoiceDetailService, _purchaseReceivalDetailService);
            return isValid(purchaseInvoiceDetail);
        }

        public bool ValidDeleteObject(PurchaseInvoiceDetail purchaseInvoiceDetail)
        {
            purchaseInvoiceDetail.Errors.Clear();
            VDeleteObject(purchaseInvoiceDetail);
            return isValid(purchaseInvoiceDetail);
        }

        public bool ValidConfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseInvoiceDetail.Errors.Clear();
            VConfirmObject(purchaseInvoiceDetail, _purchaseInvoiceDetailService, _purchaseReceivalDetailService);
            return isValid(purchaseInvoiceDetail);
        }

        public bool ValidUnconfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail)
        {
            purchaseInvoiceDetail.Errors.Clear();
            VUnconfirmObject(purchaseInvoiceDetail);
            return isValid(purchaseInvoiceDetail);
        }

        public bool isValid(PurchaseInvoiceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseInvoiceDetail obj)
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