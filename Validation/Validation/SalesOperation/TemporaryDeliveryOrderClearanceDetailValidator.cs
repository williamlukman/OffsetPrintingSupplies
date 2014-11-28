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
    public class TemporaryDeliveryOrderClearanceDetailValidator : ITemporaryDeliveryOrderClearanceDetailValidator
    {
        public TemporaryDeliveryOrderClearanceDetail VHasTemporaryDeliveryOrderClearance(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService)
        {
            TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance = _temporaryDeliveryOrderClearanceService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId);
            if (temporaryDeliveryOrderClearance == null)
            {
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "Temporary Delivery Order Clearance tidak ada");
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VTemporaryDeliveryOrderClearanceHasNotBeenConfirmed(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _purchaseReceivalService)
        {
            TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance = _purchaseReceivalService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId);
            if (temporaryDeliveryOrderClearance.IsConfirmed)
            {
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VNonNegativeQuantity(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            if (temporaryDeliveryOrderClearanceDetail.Quantity <= 0)
            {
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VHasTemporaryDeliveryOrderDetail(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail,
                   ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail = _temporaryDeliveryOrderDetailService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetailId.GetValueOrDefault());
            if (temporaryDeliveryOrderDetail == null)
            {
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "Temporary Delivery Order Detail Tidak boleh tidak ada");
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VUniqueTemporaryDeliveryOrderDetail(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService)
        {
            IList<TemporaryDeliveryOrderClearanceDetail> details = _temporaryDeliveryOrderClearanceDetailService.GetObjectsByTemporaryDeliveryOrderClearanceId(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId);
            foreach (var detail in details)
            {
                if (detail.TemporaryDeliveryOrderDetailId == temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetailId && detail.Id != temporaryDeliveryOrderClearanceDetail.Id && !detail.IsDeleted)
                {
                    temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "Tidak boleh memilih Temporary Delivery Order Detail lebih dari 1 kali di satu Temporary Delivery Order Clearance");
                    return temporaryDeliveryOrderClearanceDetail;
                }
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VTemporaryDeliveryOrderDetailHasBeenConfirmed(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            if (!temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.IsConfirmed)
            {
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "Temporary Delivery Order Detail belum dikonfirmasi");
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VQuantityOfTemporaryDeliveryOrderClearanceDetailsIsLessThanOrEqualTemporaryDeliveryOrderDetail(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail,
                                     ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, bool CaseCreate)
        {
            IList<TemporaryDeliveryOrderClearanceDetail> details = _temporaryDeliveryOrderClearanceDetailService.GetObjectsByTemporaryDeliveryOrderDetailId(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetailId.GetValueOrDefault());
            int totalQuantity = 0;
            foreach (var detail in details)
            {
                if (!detail.IsConfirmed)
                {
                    totalQuantity += detail.Quantity;
                }
            }
            if (CaseCreate) { totalQuantity += temporaryDeliveryOrderClearanceDetail.Quantity; }
            if (totalQuantity > temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.Quantity)
            {
                int maxquantity = temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.Quantity - totalQuantity + temporaryDeliveryOrderClearanceDetail.Quantity;
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "Quantity maximum adalah " + maxquantity);
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VHasItemQuantity(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance = _temporaryDeliveryOrderClearanceService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(temporaryDeliveryOrderClearance.TemporaryDeliveryOrder.WarehouseId, temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.ItemId);
            Item item = _itemService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.ItemId);
            if (item.Quantity - temporaryDeliveryOrderClearanceDetail.Quantity < 0)
            {
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "Item quantity kurang dari quantity untuk dikirim");
            }
            else if (warehouseItem.Quantity - temporaryDeliveryOrderClearanceDetail.Quantity < 0)
            {
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "WarehouseItem quantity kurang dari quantity untuk dikirim");
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VHasBeenConfirmed(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            if (!temporaryDeliveryOrderClearanceDetail.IsConfirmed)
            {
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi.");
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VHasNotBeenConfirmed(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            if (temporaryDeliveryOrderClearanceDetail.IsConfirmed)
            {
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi.");
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VHasConfirmationDate(TemporaryDeliveryOrderClearanceDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public TemporaryDeliveryOrderClearanceDetail VQuantityEqualsWasteAndRestock(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            if (temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.WasteQuantity + temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.RestockQuantity != temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.Quantity)
            {
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "Waste + Restock harus = Quantity");
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VQuantityLessThanOrEqualWasteAndRestock(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            if (temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.WasteQuantity + temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.RestockQuantity > temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.Quantity)
            {
                temporaryDeliveryOrderClearanceDetail.Errors.Add("Generic", "Waste + Restock harus kurang dari atau sama dengan Quantity");
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VCreateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                                          ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            VHasTemporaryDeliveryOrderClearance(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceService);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VTemporaryDeliveryOrderClearanceHasNotBeenConfirmed(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceService);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VHasTemporaryDeliveryOrderDetail(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderDetailService);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VTemporaryDeliveryOrderDetailHasBeenConfirmed(temporaryDeliveryOrderClearanceDetail);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VNonNegativeQuantity(temporaryDeliveryOrderClearanceDetail);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            // specific parameter = true for create
            VQuantityOfTemporaryDeliveryOrderClearanceDetailsIsLessThanOrEqualTemporaryDeliveryOrderDetail(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceDetailService, true);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VUniqueTemporaryDeliveryOrderDetail(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceDetailService);
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VUpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                                          ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            VHasTemporaryDeliveryOrderClearance(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceService);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VTemporaryDeliveryOrderClearanceHasNotBeenConfirmed(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceService);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VHasTemporaryDeliveryOrderDetail(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderDetailService);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VTemporaryDeliveryOrderDetailHasBeenConfirmed(temporaryDeliveryOrderClearanceDetail);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VNonNegativeQuantity(temporaryDeliveryOrderClearanceDetail);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            // specific parameter = false for non create function
            VQuantityOfTemporaryDeliveryOrderClearanceDetailsIsLessThanOrEqualTemporaryDeliveryOrderDetail(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceDetailService, false);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VUniqueTemporaryDeliveryOrderDetail(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceDetailService);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VQuantityLessThanOrEqualWasteAndRestock(temporaryDeliveryOrderClearanceDetail);
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VDeleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            VHasNotBeenConfirmed(temporaryDeliveryOrderClearanceDetail);
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VConfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                    ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasConfirmationDate(temporaryDeliveryOrderClearanceDetail);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VHasNotBeenConfirmed(temporaryDeliveryOrderClearanceDetail);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VHasItemQuantity(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceService, _itemService, _warehouseItemService);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VQuantityOfTemporaryDeliveryOrderClearanceDetailsIsLessThanOrEqualTemporaryDeliveryOrderDetail(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceDetailService, false);
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VUnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            VHasBeenConfirmed(temporaryDeliveryOrderClearanceDetail);
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VProcessObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            VHasBeenConfirmed(temporaryDeliveryOrderClearanceDetail);
            if (!isValid(temporaryDeliveryOrderClearanceDetail)) { return temporaryDeliveryOrderClearanceDetail; }
            VQuantityEqualsWasteAndRestock(temporaryDeliveryOrderClearanceDetail);
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VCompleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail VUndoCompleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            return temporaryDeliveryOrderClearanceDetail;
        }

        public bool ValidCreateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                      ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            VCreateObject(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceDetailService, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);
            return isValid(temporaryDeliveryOrderClearanceDetail);
        }

        public bool ValidUpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                      ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            temporaryDeliveryOrderClearanceDetail.Errors.Clear();
            VUpdateObject(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceDetailService, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);
            return isValid(temporaryDeliveryOrderClearanceDetail);
        }

        public bool ValidDeleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            temporaryDeliveryOrderClearanceDetail.Errors.Clear();
            VDeleteObject(temporaryDeliveryOrderClearanceDetail);
            return isValid(temporaryDeliveryOrderClearanceDetail);
        }

        public bool ValidConfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                       ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            temporaryDeliveryOrderClearanceDetail.Errors.Clear();
            VConfirmObject(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderClearanceDetailService, _itemService, _warehouseItemService);
            return isValid(temporaryDeliveryOrderClearanceDetail);
        }

        public bool ValidUnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail) 
        {
            temporaryDeliveryOrderClearanceDetail.Errors.Clear();
            VUnconfirmObject(temporaryDeliveryOrderClearanceDetail);
            return isValid(temporaryDeliveryOrderClearanceDetail);
        }

        public bool ValidProcessObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            temporaryDeliveryOrderClearanceDetail.Errors.Clear();
            VProcessObject(temporaryDeliveryOrderClearanceDetail);
            return isValid(temporaryDeliveryOrderClearanceDetail);
        }

        public bool ValidCompleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            temporaryDeliveryOrderClearanceDetail.Errors.Clear();
            VCompleteObject(temporaryDeliveryOrderClearanceDetail);
            return isValid(temporaryDeliveryOrderClearanceDetail);
        }

        public bool ValidUndoCompleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            temporaryDeliveryOrderClearanceDetail.Errors.Clear();
            VUndoCompleteObject(temporaryDeliveryOrderClearanceDetail);
            return isValid(temporaryDeliveryOrderClearanceDetail);
        }

        public bool isValid(TemporaryDeliveryOrderClearanceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(TemporaryDeliveryOrderClearanceDetail obj)
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