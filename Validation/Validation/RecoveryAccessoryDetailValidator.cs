using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Validation.Validation
{
    public class RecoveryAccessoryDetailValidator : IRecoveryAccessoryDetailValidator
    {
        public RecoveryAccessoryDetail VHasRecoveryOrderDetail(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            RecoveryOrderDetail detail = _recoveryOrderDetailService.GetObjectById(recoveryAccessoryDetail.RecoveryOrderDetailId);
            if (detail == null)
            {
                recoveryAccessoryDetail.Errors.Add("RecoveryOrderDetailId", "Tidak terasosiasi dengan Recovery Order Detail");
            }
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VIsAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            Item item = _itemService.GetObjectById(recoveryAccessoryDetail.ItemId);
            ItemType itemType = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Accessory);
            if (item.ItemTypeId != itemType.Id)
            {
                recoveryAccessoryDetail.Errors.Add("ItemId", "Bukan sebuah accessory");
            }
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VNonZeroQuantity(RecoveryAccessoryDetail recoveryAccessoryDetail)
        {
            if (recoveryAccessoryDetail.Quantity <= 0)
            {
                recoveryAccessoryDetail.Errors.Add("Quantity", "Tidak boleh 0 atau negatif");
            }
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VQuantityInStock(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(recoveryAccessoryDetail.ItemId);
            if (item.Quantity < recoveryAccessoryDetail.Quantity)
            {
                recoveryAccessoryDetail.Errors.Add("Quantity", "Tidak boleh lebih dari jumlah stock barang");
            }
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VHasBeenConfirmed(RecoveryAccessoryDetail recoveryAccessoryDetail)
        {
            if (!recoveryAccessoryDetail.IsConfirmed)
            {
                recoveryAccessoryDetail.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VHasNotBeenConfirmed(RecoveryAccessoryDetail recoveryAccessoryDetail)
        {
            if (recoveryAccessoryDetail.IsConfirmed)
            {
                recoveryAccessoryDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VRecoveryOrderHasNotBeenFinished(RecoveryAccessoryDetail recoveryAccessoryDetail,
                            IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            RecoveryOrderDetail detail = _recoveryOrderDetailService.GetObjectById(recoveryAccessoryDetail.RecoveryOrderDetailId);
            RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(detail.RecoveryOrderId);
            if (recoveryOrder.IsFinished)
            {
                recoveryAccessoryDetail.Errors.Add("Generic", "Recovery Order sudah di finish");
            }
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VRecoveryOrderHasBeenFinished(RecoveryAccessoryDetail recoveryAccessoryDetail,
                            IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            RecoveryOrderDetail detail = _recoveryOrderDetailService.GetObjectById(recoveryAccessoryDetail.Id);
            RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(detail.RecoveryOrderId);
            if (!recoveryOrder.IsFinished)
            {
                recoveryAccessoryDetail.Errors.Add("Generic", "Harus sudah finish");
            }
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VCreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                                     IItemService _itemService, IItemTypeService _itemTypeService)
        {
            VHasRecoveryOrderDetail(recoveryAccessoryDetail, _recoveryOrderDetailService);
            if (!isValid(recoveryAccessoryDetail)) { return recoveryAccessoryDetail; }
            VIsAccessory(recoveryAccessoryDetail, _itemService, _itemTypeService);
            if (!isValid(recoveryAccessoryDetail)) { return recoveryAccessoryDetail; }
            VNonZeroQuantity(recoveryAccessoryDetail);
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VUpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                                     IItemService _itemService, IItemTypeService _itemTypeService)
        {
            VCreateObject(recoveryAccessoryDetail, _recoveryOrderDetailService, _itemService, _itemTypeService);
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail)
        {
            VHasNotBeenConfirmed(recoveryAccessoryDetail);
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VConfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService,
                                                      IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService)
        {
            VRecoveryOrderHasNotBeenFinished(recoveryAccessoryDetail, _recoveryOrderService, _recoveryOrderDetailService);
            if (!isValid(recoveryAccessoryDetail)) { return recoveryAccessoryDetail; }
            VHasNotBeenConfirmed(recoveryAccessoryDetail);
            if (!isValid(recoveryAccessoryDetail)) { return recoveryAccessoryDetail; }
            VQuantityInStock(recoveryAccessoryDetail, _itemService);
            return recoveryAccessoryDetail;
        }

        public RecoveryAccessoryDetail VUnconfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService,
                                                      IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            VRecoveryOrderHasNotBeenFinished(recoveryAccessoryDetail, _recoveryOrderService, _recoveryOrderDetailService);
            if (!isValid(recoveryAccessoryDetail)) { return recoveryAccessoryDetail; }
            VHasBeenConfirmed(recoveryAccessoryDetail);
            return recoveryAccessoryDetail;
        }

        public bool ValidCreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                                     IItemService _itemService, IItemTypeService _itemTypeService)
        {
            VCreateObject(recoveryAccessoryDetail, _recoveryOrderDetailService, _itemService, _itemTypeService);
            return isValid(recoveryAccessoryDetail);
        }

        public bool ValidUpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                                     IItemService _itemService, IItemTypeService _itemTypeService)
        {
            recoveryAccessoryDetail.Errors.Clear();
            VUpdateObject(recoveryAccessoryDetail, _recoveryOrderDetailService, _itemService, _itemTypeService);
            return isValid(recoveryAccessoryDetail);
        }

        public bool ValidDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail)
        {
            recoveryAccessoryDetail.Errors.Clear();
            VDeleteObject(recoveryAccessoryDetail);
            return isValid(recoveryAccessoryDetail);
        }

        public bool ValidConfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService,
                                                      IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService)
        {
            recoveryAccessoryDetail.Errors.Clear();
            VConfirmObject(recoveryAccessoryDetail, _recoveryOrderService, _recoveryOrderDetailService, _itemService);
            return isValid(recoveryAccessoryDetail);
        }

        public bool ValidUnconfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService,
                                                      IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            recoveryAccessoryDetail.Errors.Clear();
            VUnconfirmObject(recoveryAccessoryDetail, _recoveryOrderService, _recoveryOrderDetailService);
            return isValid(recoveryAccessoryDetail);
        }

        public bool isValid(RecoveryAccessoryDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RecoveryAccessoryDetail obj)
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
