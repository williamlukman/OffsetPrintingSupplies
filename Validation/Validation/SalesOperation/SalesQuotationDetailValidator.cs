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
    public class SalesQuotationDetailValidator : ISalesQuotationDetailValidator
    {
        public SalesQuotationDetail VHasSalesQuotation(SalesQuotationDetail salesQuotationDetail, ISalesQuotationService _salesQuotationService)
        {
            SalesQuotation salesQuotation = _salesQuotationService.GetObjectById(salesQuotationDetail.SalesQuotationId);
            if (salesQuotation == null)
            {
                salesQuotationDetail.Errors.Add("SalesQuotationId", "Tidak terasosiasi dengan sales order");
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VSalesQuotationHasNotBeenConfirmed(SalesQuotationDetail salesQuotationDetail, ISalesQuotationService _salesQuotationService)
        {
            SalesQuotation salesQuotation = _salesQuotationService.GetObjectById(salesQuotationDetail.SalesQuotationId);
            if (salesQuotation.IsConfirmed)
            {
                salesQuotationDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VHasItem(SalesQuotationDetail salesQuotationDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(salesQuotationDetail.ItemId);
            if (item == null)
            {
                salesQuotationDetail.Errors.Add("ItemId", "Tidak terasosiasi dengan item");
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VNonZeroNorNegativeQuantity(SalesQuotationDetail salesQuotationDetail)
        {
            if (salesQuotationDetail.Quantity < 0)
            {
                salesQuotationDetail.Errors.Add("Quantity", "Tidak boleh kurang dari 0");
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VNonNegativePrice(SalesQuotationDetail salesQuotationDetail)
        {
            if (salesQuotationDetail.QuotationPrice <= 0)
            {
                salesQuotationDetail.Errors.Add("QuotationPrice", "Tidak boleh kurang dari atau sama dengan 0");
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VUniqueSalesQuotationDetail(SalesQuotationDetail salesQuotationDetail, ISalesQuotationDetailService _salesQuotationDetailService, IItemService _itemService)
        {
            IList<SalesQuotationDetail> details = _salesQuotationDetailService.GetObjectsBySalesQuotationId(salesQuotationDetail.SalesQuotationId);
            foreach (var detail in details)
            {
                if (detail.ItemId == salesQuotationDetail.ItemId && detail.Id != salesQuotationDetail.Id)
                {
                    salesQuotationDetail.Errors.Add("Generic", "Tidak boleh memiliki Sku yang sama dalam 1 Sales Order");
                    return salesQuotationDetail;
                }
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VHasNotBeenConfirmed(SalesQuotationDetail salesQuotationDetail)
        {
            if (salesQuotationDetail.IsConfirmed)
            {
                salesQuotationDetail.Errors.Add("Generic", "Tidak boleh sudah selesai");
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VHasBeenConfirmed(SalesQuotationDetail salesQuotationDetail)
        {
            if (!salesQuotationDetail.IsConfirmed)
            {
                salesQuotationDetail.Errors.Add("Generic", "Belum selesai");
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VHasNoSalesOrderDetail(SalesQuotationDetail salesQuotationDetail, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> details = _salesOrderDetailService.GetObjectsBySalesQuotationDetailId(salesQuotationDetail.Id);
            if (details.Any())
            {
                salesQuotationDetail.Errors.Add("Generic", "Tidak boleh boleh terasosiasi dengan Sales Order Detail");
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VHasConfirmationDate(SalesQuotationDetail salesQuotationDetail)
        {
            if (salesQuotationDetail.ConfirmationDate == null)
            {
                salesQuotationDetail.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VCreateObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationDetailService _salesQuotationDetailService, ISalesQuotationService _salesQuotationService, IItemService _itemService)
        {
            VHasSalesQuotation(salesQuotationDetail, _salesQuotationService);
            if (!isValid(salesQuotationDetail)) { return salesQuotationDetail; }
            VSalesQuotationHasNotBeenConfirmed(salesQuotationDetail, _salesQuotationService);
            if (!isValid(salesQuotationDetail)) { return salesQuotationDetail; }
            VHasItem(salesQuotationDetail, _itemService);
            if (!isValid(salesQuotationDetail)) { return salesQuotationDetail; }
            VNonZeroNorNegativeQuantity(salesQuotationDetail);
            if (!isValid(salesQuotationDetail)) { return salesQuotationDetail; }
            VNonNegativePrice(salesQuotationDetail);
            if (!isValid(salesQuotationDetail)) { return salesQuotationDetail; }
            VUniqueSalesQuotationDetail(salesQuotationDetail, _salesQuotationDetailService, _itemService);
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VUpdateObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationDetailService _salesQuotationDetailService, ISalesQuotationService _salesQuotationService, IItemService _itemService)
        {
            VCreateObject(salesQuotationDetail, _salesQuotationDetailService, _salesQuotationService, _itemService);
            if (!isValid(salesQuotationDetail)) { return salesQuotationDetail; }
            VHasNotBeenConfirmed(salesQuotationDetail);
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VDeleteObject(SalesQuotationDetail salesQuotationDetail)
        {
            VHasNotBeenConfirmed(salesQuotationDetail);
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VConfirmObject(SalesQuotationDetail salesQuotationDetail)
        {
            VHasConfirmationDate(salesQuotationDetail);
            if (!isValid(salesQuotationDetail)) { return salesQuotationDetail; }
            VHasNotBeenConfirmed(salesQuotationDetail);
            return salesQuotationDetail;
        }

        public SalesQuotationDetail VUnconfirmObject(SalesQuotationDetail salesQuotationDetail, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService)
        {
            VHasBeenConfirmed(salesQuotationDetail);
            if (!isValid(salesQuotationDetail)) { return salesQuotationDetail; }
            VHasNoSalesOrderDetail(salesQuotationDetail, _salesOrderDetailService);
            return salesQuotationDetail;
        }

        public bool ValidCreateObject(SalesQuotationDetail salesQuotationDetail,  ISalesQuotationDetailService _salesQuotationDetailService, ISalesQuotationService _salesQuotationService, IItemService _itemService)
        {
            VCreateObject(salesQuotationDetail, _salesQuotationDetailService, _salesQuotationService, _itemService);
            return isValid(salesQuotationDetail);
        }

        public bool ValidUpdateObject(SalesQuotationDetail salesQuotationDetail,  ISalesQuotationDetailService _salesQuotationDetailService, ISalesQuotationService _salesQuotationService, IItemService _itemService)
        {
            salesQuotationDetail.Errors.Clear();
            VUpdateObject(salesQuotationDetail, _salesQuotationDetailService, _salesQuotationService, _itemService);
            return isValid(salesQuotationDetail);
        }

        public bool ValidDeleteObject(SalesQuotationDetail salesQuotationDetail)
        {
            salesQuotationDetail.Errors.Clear();
            VDeleteObject(salesQuotationDetail);
            return isValid(salesQuotationDetail);
        }

        public bool ValidConfirmObject(SalesQuotationDetail salesQuotationDetail)
        {
            salesQuotationDetail.Errors.Clear();
            VConfirmObject(salesQuotationDetail);
            return isValid(salesQuotationDetail);
        }

        public bool ValidUnconfirmObject(SalesQuotationDetail salesQuotationDetail, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService)
        {
            salesQuotationDetail.Errors.Clear();
            VUnconfirmObject(salesQuotationDetail, _salesOrderDetailService, _itemService);
            return isValid(salesQuotationDetail);
        }

        public bool isValid(SalesQuotationDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesQuotationDetail obj)
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