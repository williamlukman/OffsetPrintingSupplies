using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class BlanketOrderValidator : IBlanketOrderValidator
    {
        public BlanketOrder VHasUniqueCode(BlanketOrder blanketOrder, IBlanketOrderService _blanketOrderService)
        {
            if (String.IsNullOrEmpty(blanketOrder.Code) || blanketOrder.Code.Trim() == "")
            {
                blanketOrder.Errors.Add("Code", "Tidak boleh kosong");
            }
            if (_blanketOrderService.IsCodeDuplicated(blanketOrder))
            {
                blanketOrder.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return blanketOrder;
        }

        public BlanketOrder VHasBlanketOrderDetails(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            IList<BlanketOrderDetail> details = _blanketOrderDetailService.GetObjectsByBlanketOrderId(blanketOrder.Id);
            if (!details.Any())
            {
                blanketOrder.Errors.Add("Generic", "Harus membuat blanket order detail dahulu");
            }
            return blanketOrder;
        }

        public BlanketOrder VHasQuantityReceived(BlanketOrder blanketOrder)
        {
            if (blanketOrder.QuantityReceived <= 0)
            {
                blanketOrder.Errors.Add("QuantityReceived", "Harus lebih dari 0");
            }
            return blanketOrder;
        }

        public BlanketOrder VQuantityFinalAndRejectedIsLessThanOrEqualQuantityReceived(BlanketOrder blanketOrder)
        {
            if (blanketOrder.QuantityFinal + blanketOrder.QuantityRejected > blanketOrder.QuantityReceived)
            {
                blanketOrder.Errors.Add("Generic", "jumlah sudah melebihi jumalah yang diterima diawal");
            }
            return blanketOrder;
        }

        public BlanketOrder VQuantityReceivedEqualDetails(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            IList<BlanketOrderDetail> details = _blanketOrderDetailService.GetObjectsByBlanketOrderId(blanketOrder.Id);
            if (blanketOrder.QuantityReceived != details.Count())
            {
                blanketOrder.Errors.Add("QuantityReceived", "Jumlah quantity received dan jumlah blanket order detail tidak sama");
            }
            return blanketOrder;
        }

        public BlanketOrder VQuantityIsInStock(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketService _blanketService,
                                               IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            IList<BlanketOrderDetail> details = _blanketOrderDetailService.GetObjectsByBlanketOrderId(blanketOrder.Id);

            // itemId contains Id of the rollBlanket, leftbar, and rightbar
            IDictionary<int, int> ValuePairItemIdQuantity = new Dictionary<int, int>();
            foreach (var detail in details)
            {
                Blanket blanket = _blanketService.GetObjectById(detail.BlanketId);
                // rollBlanket
                if (ValuePairItemIdQuantity.ContainsKey(blanket.RollBlanketItemId))
                {
                    ValuePairItemIdQuantity[blanket.RollBlanketItemId] += 1;
                }
                else
                {
                    ValuePairItemIdQuantity.Add(blanket.RollBlanketItemId, 1);
                }

                // leftbar
                if (blanket.LeftBarItemId != null)
                {
                    if (ValuePairItemIdQuantity.ContainsKey((int)blanket.LeftBarItemId))
                    {
                        ValuePairItemIdQuantity[(int)blanket.LeftBarItemId] += 1;
                    }
                    else
                    {
                        ValuePairItemIdQuantity.Add((int)blanket.LeftBarItemId, 1);
                    }
                }

                // rightbar
                if (blanket.RightBarItemId != null)
                {
                    if (ValuePairItemIdQuantity.ContainsKey((int)blanket.RightBarItemId))
                    {
                        ValuePairItemIdQuantity[(int)blanket.RightBarItemId] += 1;
                    }
                    else
                    {
                        ValuePairItemIdQuantity.Add((int)blanket.RightBarItemId, 1);
                    }
                }
            }

            foreach (var ValuePair in ValuePairItemIdQuantity)
            {
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, ValuePair.Key);
                if (warehouseItem.Quantity < ValuePair.Value)
                {
                    blanketOrder.Errors.Add("Generic", "Stock quantity BoM untuk blanket tidak boleh kurang dari jumlah di dalam blanket order");
                    return blanketOrder;
                }
            }
            return blanketOrder;
        }

        public BlanketOrder VHasBeenConfirmationDate(BlanketOrder blanketOrder)
        {
            if (blanketOrder.ConfirmationDate == null)
            {
                blanketOrder.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return blanketOrder;
        }

        public BlanketOrder VHasBeenConfirmed(BlanketOrder blanketOrder)
        {
            if (!blanketOrder.IsConfirmed)
            {
                blanketOrder.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return blanketOrder;
        }

        public BlanketOrder VHasNotBeenConfirmed(BlanketOrder blanketOrder)
        {
            if (blanketOrder.IsConfirmed)
            {
                blanketOrder.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return blanketOrder;
        }

        public BlanketOrder VHasBeenCompleted(BlanketOrder blanketOrder)
        {
            if (!blanketOrder.IsCompleted)
            {
                blanketOrder.Errors.Add("Generic", "Belum complete");
            }
            return blanketOrder;
        }

        public BlanketOrder VHasNotBeenCompleted(BlanketOrder blanketOrder)
        {
            if (blanketOrder.IsCompleted)
            {
                blanketOrder.Errors.Add("Generic", "Sudah complete");
            }
            return blanketOrder;
        }

        public BlanketOrder VAllDetailsHaveBeenFinishedOrRejected(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            IList<BlanketOrderDetail> details = _blanketOrderDetailService.GetObjectsByBlanketOrderId(blanketOrder.Id);
            foreach (var detail in details)
            {
                if (!detail.IsFinished && !detail.IsRejected)
                {
                    blanketOrder.Errors.Add("Generic", "Semua blanket order detail harus telah selesai atau di reject");
                    return blanketOrder;
                }
            }
            return blanketOrder;
        }

        public BlanketOrder VAllDetailsHaveBeenCutOrRejected(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            IList<BlanketOrderDetail> details = _blanketOrderDetailService.GetObjectsByBlanketOrderId(blanketOrder.Id);
            foreach (var detail in details)
            {
                if (!detail.IsCut && !detail.IsRejected)
                {
                    blanketOrder.Errors.Add("Generic", "Semua blanket order detail harus telah di cut atau di reject");
                    return blanketOrder;
                }
            }
            return blanketOrder;
        }

        public BlanketOrder VAllDetailsHaveNotBeenFinishedNorRejected(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            IList<BlanketOrderDetail> details = _blanketOrderDetailService.GetObjectsByBlanketOrderId(blanketOrder.Id);
            foreach (var detail in details)
            {
                if (detail.IsFinished || detail.IsRejected)
                {
                    blanketOrder.Errors.Add("Generic", "Semua blanket order detail harus belum di finish atau di reject");
                    return blanketOrder;
                }
            }
            return blanketOrder;
        }
        
        public BlanketOrder VCreateObject(BlanketOrder blanketOrder, IBlanketOrderService _blanketOrderService)
        {
            VHasUniqueCode(blanketOrder, _blanketOrderService);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VHasQuantityReceived(blanketOrder);
            return blanketOrder;
        }

        public BlanketOrder VUpdateObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService)
        {
            VHasNotBeenConfirmed(blanketOrder);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VCreateObject(blanketOrder, _blanketOrderService);
            return blanketOrder;
        }

        public BlanketOrder VUpdateAfterConfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService)
        {
            VCreateObject(blanketOrder, _blanketOrderService);
            return blanketOrder;
        }

        public BlanketOrder VDeleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            VHasNotBeenConfirmed(blanketOrder);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VAllDetailsHaveNotBeenFinishedNorRejected(blanketOrder, _blanketOrderDetailService);
            return blanketOrder;
        }

        public BlanketOrder VHasConfirmationDate(BlanketOrder obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public BlanketOrder VConfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasConfirmationDate(blanketOrder);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VHasNotBeenConfirmed(blanketOrder);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VHasBlanketOrderDetails(blanketOrder, _blanketOrderDetailService);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VQuantityReceivedEqualDetails(blanketOrder, _blanketOrderDetailService);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VQuantityIsInStock(blanketOrder, _blanketOrderDetailService, _blanketService, _itemService, _warehouseItemService);
            return blanketOrder;
        }

        public BlanketOrder VUnconfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            VHasBeenConfirmed(blanketOrder);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VHasNotBeenCompleted(blanketOrder);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VAllDetailsHaveBeenFinishedOrRejected(blanketOrder, _blanketOrderDetailService);
            return blanketOrder;
        }

        public BlanketOrder VCompleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            VHasBeenConfirmed(blanketOrder);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VHasNotBeenCompleted(blanketOrder);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VAllDetailsHaveBeenFinishedOrRejected(blanketOrder, _blanketOrderDetailService);
            return blanketOrder;
        }

        public BlanketOrder VUndoCompleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            VHasBeenCompleted(blanketOrder);
            if (!isValid(blanketOrder)) { return blanketOrder; }
            VAllDetailsHaveNotBeenFinishedNorRejected(blanketOrder, _blanketOrderDetailService);
            return blanketOrder;
        }

        public BlanketOrder VAdjustQuantity(BlanketOrder blanketOrder)
        {
            VQuantityFinalAndRejectedIsLessThanOrEqualQuantityReceived(blanketOrder);
            return blanketOrder;
        }

        public bool ValidCreateObject(BlanketOrder blanketOrder, IBlanketOrderService _blanketOrderService)
        {
            VCreateObject(blanketOrder, _blanketOrderService);
            return isValid(blanketOrder);
        }

        public bool ValidUpdateObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService)
        {
            blanketOrder.Errors.Clear();
            VUpdateObject(blanketOrder, _blanketOrderDetailService, _blanketOrderService);
            return isValid(blanketOrder);
        }

        public bool ValidUpdateAfterConfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService)
        {
            blanketOrder.Errors.Clear();
            VUpdateAfterConfirmObject(blanketOrder, _blanketOrderDetailService, _blanketOrderService);
            return isValid(blanketOrder);
        }

        public bool ValidDeleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            blanketOrder.Errors.Clear();
            VDeleteObject(blanketOrder, _blanketOrderDetailService);
            return isValid(blanketOrder);
        }

        public bool ValidConfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService,
                                       IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            blanketOrder.Errors.Clear();
            VConfirmObject(blanketOrder, _blanketOrderDetailService, _blanketService, _itemService, _warehouseItemService);
            return isValid(blanketOrder);
        }

        public bool ValidUnconfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            blanketOrder.Errors.Clear();
            VUnconfirmObject(blanketOrder, _blanketOrderDetailService);
            return isValid(blanketOrder);
        }

        public bool ValidCompleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            blanketOrder.Errors.Clear();
            VCompleteObject(blanketOrder, _blanketOrderDetailService);
            return isValid(blanketOrder);
        }

        public bool ValidUndoCompleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            blanketOrder.Errors.Clear();
            VUndoCompleteObject(blanketOrder, _blanketOrderDetailService);
            return isValid(blanketOrder);
        }

        public bool ValidAdjustQuantity(BlanketOrder blanketOrder)
        {
            blanketOrder.Errors.Clear();
            VAdjustQuantity(blanketOrder);
            return isValid(blanketOrder);
        }
        
        public bool isValid(BlanketOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(BlanketOrder obj)
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
