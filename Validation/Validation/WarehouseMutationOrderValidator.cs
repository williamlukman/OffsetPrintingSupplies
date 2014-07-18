using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class WarehouseMutationOrderValidator : IWarehouseMutationOrderValidator
    {

        public WarehouseMutationOrder VHasDifferentWarehouse(WarehouseMutationOrder warehouseMutationOrder)
        {
            if (warehouseMutationOrder.WarehouseFromId == warehouseMutationOrder.WarehouseToId)
            {
                warehouseMutationOrder.Errors.Add("Generic", "Warehouse sebelum dan sesudah tidak boleh sama");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasWarehouseFrom(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            Warehouse warehouseFrom = _warehouseService.GetObjectById(warehouseMutationOrder.WarehouseFromId);
            if (warehouseFrom == null)
            {
                warehouseMutationOrder.Errors.Add("WarehouseFromId", "Tidak terasosiasi dengan warehouse");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasWarehouseTo(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            Warehouse warehouseTo = _warehouseService.GetObjectById(warehouseMutationOrder.WarehouseToId);
            if (warehouseTo == null)
            {
                warehouseMutationOrder.Errors.Add("WarehouseToId", "Tidak terasosiasi dengan warehouse");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasWarehouseMutationOrderDetails(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService)
        {
            IList<WarehouseMutationOrderDetail> details = _warehouseMutationOrderDetailService.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrder.Id);
            if (!details.Any())
            {
                warehouseMutationOrder.Errors.Add("Generic", "Details tidak boleh tidak ada");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VQuantityWarehouseFromIsLargerThanQuantity(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IWarehouseItemService _warehouseItemService)
        {
            IList<WarehouseMutationOrderDetail> details = _warehouseMutationOrderDetailService.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrder.Id);
            foreach (var detail in details)
            {
                WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(warehouseMutationOrder.WarehouseFromId, detail.ItemId);
                if (warehouseItem.Quantity < detail.Quantity)
                {
                    warehouseMutationOrder.Errors.Add("Generic", "Quantity di warehouse tidak mencukupi untuk melakukan mutasi");
                    return warehouseMutationOrder;
                }
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasNotBeenConfirmed(WarehouseMutationOrder warehouseMutationOrder)
        {
            if (warehouseMutationOrder.IsConfirmed)
            {
                warehouseMutationOrder.Errors.Add("IsConfirmed", "Tidak boleh sudah dikonfirmasi");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasBeenConfirmed(WarehouseMutationOrder warehouseMutationOrder)
        {
            if (!warehouseMutationOrder.IsConfirmed)
            {
                warehouseMutationOrder.Errors.Add("IsConfirmed", "Harus sudah dikonfirmasi");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VDetailsAreVerifiedConfirmable(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            IList<WarehouseMutationOrderDetail> details = _warehouseMutationOrderDetailService.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrder.Id);
            foreach (var detail in details)
            {
                if (!_warehouseMutationOrderDetailService.GetValidator().ValidConfirmObject(detail, _warehouseMutationOrderService, _itemService, _barringService, _warehouseItemService))
                {
                    warehouseMutationOrder.Errors.Add("Generic", "Details tidak dapat dikonfirmasi");
                }
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VDetailsAreVerifiedUnconfirmable(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            IList<WarehouseMutationOrderDetail> details = _warehouseMutationOrderDetailService.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrder.Id);
            foreach (var detail in details)
            {
                if (!_warehouseMutationOrderDetailService.GetValidator().ValidUnconfirmObject(detail, _warehouseMutationOrderService, _itemService, _barringService, _warehouseItemService))
                {
                    foreach (var error in detail.Errors)
                    {
                        warehouseMutationOrder.Errors.Add(error.Key, error.Value);
                    }
                    if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
                }
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VCreateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            VHasDifferentWarehouse(warehouseMutationOrder);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VHasWarehouseFrom(warehouseMutationOrder, _warehouseService);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VHasWarehouseTo(warehouseMutationOrder, _warehouseService);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VUpdateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            VHasNotBeenConfirmed(warehouseMutationOrder);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VCreateObject(warehouseMutationOrder, _warehouseService);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VDeleteObject(WarehouseMutationOrder warehouseMutationOrder)
        {
            VHasNotBeenConfirmed(warehouseMutationOrder);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VConfirmObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                              IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasNotBeenConfirmed(warehouseMutationOrder);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VHasWarehouseMutationOrderDetails(warehouseMutationOrder, _warehouseMutationOrderDetailService);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VDetailsAreVerifiedConfirmable(warehouseMutationOrder, _warehouseMutationOrderService, _warehouseMutationOrderDetailService, _itemService, _barringService, _warehouseItemService);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VUnconfirmObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasBeenConfirmed(warehouseMutationOrder);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VDetailsAreVerifiedUnconfirmable(warehouseMutationOrder, _warehouseMutationOrderService, _warehouseMutationOrderDetailService, _itemService, _barringService, _warehouseItemService);
            return warehouseMutationOrder;
        }

        public bool ValidCreateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            VCreateObject(warehouseMutationOrder, _warehouseService);
            return isValid(warehouseMutationOrder);
        }

        public bool ValidUpdateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            warehouseMutationOrder.Errors.Clear();
            VUpdateObject(warehouseMutationOrder, _warehouseService);
            return isValid(warehouseMutationOrder);
        }

        public bool ValidDeleteObject(WarehouseMutationOrder warehouseMutationOrder)
        {
            warehouseMutationOrder.Errors.Clear();
            VDeleteObject(warehouseMutationOrder);
            return isValid(warehouseMutationOrder);
        }

        public bool ValidConfirmObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            warehouseMutationOrder.Errors.Clear();
            VConfirmObject(warehouseMutationOrder, _warehouseMutationOrderService, _warehouseMutationOrderDetailService, _itemService, _barringService, _warehouseItemService);
            return isValid(warehouseMutationOrder);
        }

        public bool ValidUnconfirmObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            warehouseMutationOrder.Errors.Clear();
            VUnconfirmObject(warehouseMutationOrder, _warehouseMutationOrderService, _warehouseMutationOrderDetailService, _itemService, _barringService, _warehouseItemService);
            return isValid(warehouseMutationOrder);
        }

        public bool isValid(WarehouseMutationOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(WarehouseMutationOrder obj)
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