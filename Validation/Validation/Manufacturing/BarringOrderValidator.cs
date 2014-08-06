using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

namespace Validation.Validation
{
    public class BarringOrderValidator : IBarringOrderValidator
    {
        public BarringOrder VHasUniqueCode(BarringOrder barringOrder, IBarringOrderService _barringOrderService)
        {
            if (String.IsNullOrEmpty(barringOrder.Code) || barringOrder.Code.Trim() == "")
            {
                barringOrder.Errors.Add("Code", "Tidak boleh kosong");
            }
            if (_barringOrderService.IsCodeDuplicated(barringOrder))
            {
                barringOrder.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return barringOrder;
        }

        public BarringOrder VHasBarringOrderDetails(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            IList<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);
            if (!details.Any())
            {
                barringOrder.Errors.Add("Generic", "Harus membuat barring order detail dahulu");
            }
            return barringOrder;
        }

        public BarringOrder VHasQuantityReceived(BarringOrder barringOrder)
        {
            if (barringOrder.QuantityReceived <= 0)
            {
                barringOrder.Errors.Add("QuantityReceived", "Harus lebih dari 0");
            }
            return barringOrder;
        }

        public BarringOrder VQuantityFinalAndRejectedIsLessThanOrEqualQuantityReceived(BarringOrder barringOrder)
        {
            if (barringOrder.QuantityFinal + barringOrder.QuantityRejected > barringOrder.QuantityReceived)
            {
                barringOrder.Errors.Add("Generic", "jumlah sudah melebihi jumalah yang diterima diawal");
            }
            return barringOrder;
        }

        public BarringOrder VQuantityReceivedEqualDetails(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            IList<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);
            if (barringOrder.QuantityReceived != details.Count())
            {
                barringOrder.Errors.Add("QuantityReceived", "Jumlah quantity received dan jumlah barring order detail tidak sama");
            }
            return barringOrder;
        }

        public BarringOrder VQuantityIsInStock(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService,
                                               IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            IList<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);

            // itemId contains Id of the blanket, leftbar, and rightbar
            IDictionary<int, int> ValuePairItemIdQuantity = new Dictionary<int, int>();
            foreach (var detail in details)
            {
                Barring barring = _barringService.GetObjectById(detail.BarringId);
                // blanket
                if (ValuePairItemIdQuantity.ContainsKey(barring.BlanketItemId))
                {
                    ValuePairItemIdQuantity[barring.BlanketItemId] += 1;
                }
                else
                {
                    ValuePairItemIdQuantity.Add(barring.BlanketItemId, 1);
                }

                // leftbar
                if (barring.LeftBarItemId != null)
                {
                    if (ValuePairItemIdQuantity.ContainsKey((int)barring.LeftBarItemId))
                    {
                        ValuePairItemIdQuantity[(int)barring.LeftBarItemId] += 1;
                    }
                    else
                    {
                        ValuePairItemIdQuantity.Add((int)barring.LeftBarItemId, 1);
                    }
                }

                // rightbar
                if (barring.RightBarItemId != null)
                {
                    if (ValuePairItemIdQuantity.ContainsKey((int)barring.RightBarItemId))
                    {
                        ValuePairItemIdQuantity[(int)barring.RightBarItemId] += 1;
                    }
                    else
                    {
                        ValuePairItemIdQuantity.Add((int)barring.RightBarItemId, 1);
                    }
                }
            }

            foreach (var ValuePair in ValuePairItemIdQuantity)
            {
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, ValuePair.Key);
                if (warehouseItem.Quantity < ValuePair.Value)
                {
                    barringOrder.Errors.Add("Generic", "Stock quantity BoM untuk barring tidak boleh kurang dari jumlah di dalam barring order");
                    return barringOrder;
                }
            }
            return barringOrder;
        }
        
        public BarringOrder VHasBeenConfirmed(BarringOrder barringOrder)
        {
            if (!barringOrder.IsConfirmed)
            {
                barringOrder.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return barringOrder;
        }

        public BarringOrder VHasNotBeenConfirmed(BarringOrder barringOrder)
        {
            if (barringOrder.IsConfirmed)
            {
                barringOrder.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return barringOrder;
        }

        public BarringOrder VHasBeenCompleted(BarringOrder barringOrder)
        {
            if (!barringOrder.IsCompleted)
            {
                barringOrder.Errors.Add("Generic", "Belum complete");
            }
            return barringOrder;
        }

        public BarringOrder VHasNotBeenCompleted(BarringOrder barringOrder)
        {
            if (barringOrder.IsCompleted)
            {
                barringOrder.Errors.Add("Generic", "Sudah complete");
            }
            return barringOrder;
        }

        public BarringOrder VAllDetailsHaveBeenFinishedOrRejected(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            IList<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);
            foreach (var detail in details)
            {
                if (!detail.IsFinished && !detail.IsRejected)
                {
                    barringOrder.Errors.Add("Generic", "Semua barring order detail harus telah selesai atau di reject");
                    return barringOrder;
                }
            }
            return barringOrder;
        }

        public BarringOrder VAllDetailsHaveBeenCutOrRejected(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            IList<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);
            foreach (var detail in details)
            {
                if (!detail.IsCut && !detail.IsRejected)
                {
                    barringOrder.Errors.Add("Generic", "Semua barring order detail harus telah di cut atau di reject");
                    return barringOrder;
                }
            }
            return barringOrder;
        }

        public BarringOrder VAllDetailsHaveNotBeenCutNorRejected(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            IList<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);
            foreach (var detail in details)
            {
                if (detail.IsCut || detail.IsRejected)
                {
                    barringOrder.Errors.Add("Generic", "Semua barring order detail harus belum di cut atau di reject");
                    return barringOrder;
                }
            }
            return barringOrder;
        }
        
        public BarringOrder VCreateObject(BarringOrder barringOrder, IBarringOrderService _barringOrderService)
        {
            VHasUniqueCode(barringOrder, _barringOrderService);
            if (!isValid(barringOrder)) { return barringOrder; }
            VHasQuantityReceived(barringOrder);
            return barringOrder;
        }

        public BarringOrder VUpdateObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringOrderService _barringOrderService)
        {
            VHasNotBeenConfirmed(barringOrder);
            if (!isValid(barringOrder)) { return barringOrder; }
            VCreateObject(barringOrder, _barringOrderService);
            return barringOrder;
        }

        public BarringOrder VDeleteObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            VHasNotBeenConfirmed(barringOrder);
            if (!isValid(barringOrder)) { return barringOrder; }
            VAllDetailsHaveNotBeenCutNorRejected(barringOrder, _barringOrderDetailService);
            return barringOrder;
        }

        public BarringOrder VConfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasNotBeenConfirmed(barringOrder);
            if (!isValid(barringOrder)) { return barringOrder; }
            VHasBarringOrderDetails(barringOrder, _barringOrderDetailService);
            if (!isValid(barringOrder)) { return barringOrder; }
            VQuantityReceivedEqualDetails(barringOrder, _barringOrderDetailService);
            if (!isValid(barringOrder)) { return barringOrder; }
            VQuantityIsInStock(barringOrder, _barringOrderDetailService, _barringService, _itemService, _warehouseItemService);
            return barringOrder;
        }

        public BarringOrder VUnconfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            VHasBeenConfirmed(barringOrder);
            if (!isValid(barringOrder)) { return barringOrder; }
            VHasNotBeenCompleted(barringOrder);
            if (!isValid(barringOrder)) { return barringOrder; }
            VAllDetailsHaveNotBeenCutNorRejected(barringOrder, _barringOrderDetailService);
            return barringOrder;
        }

        public BarringOrder VCompleteObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            VHasBeenConfirmed(barringOrder);
            if (!isValid(barringOrder)) { return barringOrder; }
            VHasNotBeenCompleted(barringOrder);
            if (!isValid(barringOrder)) { return barringOrder; }
            VAllDetailsHaveBeenFinishedOrRejected(barringOrder, _barringOrderDetailService);
            return barringOrder;
        }

        public BarringOrder VAdjustQuantity(BarringOrder barringOrder)
        {
            VQuantityFinalAndRejectedIsLessThanOrEqualQuantityReceived(barringOrder);
            return barringOrder;
        }

        public bool ValidCreateObject(BarringOrder barringOrder, IBarringOrderService _barringOrderService)
        {
            VCreateObject(barringOrder, _barringOrderService);
            return isValid(barringOrder);
        }

        public bool ValidUpdateObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringOrderService _barringOrderService)
        {
            barringOrder.Errors.Clear();
            VUpdateObject(barringOrder, _barringOrderDetailService, _barringOrderService);
            return isValid(barringOrder);
        }

        public bool ValidDeleteObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            barringOrder.Errors.Clear();
            VDeleteObject(barringOrder, _barringOrderDetailService);
            return isValid(barringOrder);
        }

        public bool ValidConfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService,
                                       IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            barringOrder.Errors.Clear();
            VConfirmObject(barringOrder, _barringOrderDetailService, _barringService, _itemService, _warehouseItemService);
            return isValid(barringOrder);
        }

        public bool ValidUnconfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            barringOrder.Errors.Clear();
            VUnconfirmObject(barringOrder, _barringOrderDetailService);
            return isValid(barringOrder);
        }

        public bool ValidCompleteObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            barringOrder.Errors.Clear();
            VCompleteObject(barringOrder, _barringOrderDetailService);
            return isValid(barringOrder);
        }

        public bool ValidAdjustQuantity(BarringOrder barringOrder)
        {
            barringOrder.Errors.Clear();
            VAdjustQuantity(barringOrder);
            return isValid(barringOrder);
        }
        
        public bool isValid(BarringOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(BarringOrder obj)
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
