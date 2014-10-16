﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class TemporaryDeliveryOrderValidator : ITemporaryDeliveryOrderValidator
    {
        public TemporaryDeliveryOrder VHasWarehouse(TemporaryDeliveryOrder temporaryDeliveryOrder, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(temporaryDeliveryOrder.WarehouseId);
            if (warehouse == null)
            {
                temporaryDeliveryOrder.Errors.Add("WarehouseId", "Tidak terasosiasi dengan warehouse");
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VHasVirtualOrderOrDeliveryOrder(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService,
                                                                      IDeliveryOrderService _deliveryOrderService)
        {
            if (temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.SampleOrder ||
                temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.TrialOrder)
            {
                VirtualOrder virtualOrder = _virtualOrderService.GetObjectById((int)temporaryDeliveryOrder.VirtualOrderId);
                if (virtualOrder == null)
                {
                    temporaryDeliveryOrder.Errors.Add("Generic", "Tidak terasosiasi dengan virtual order");
                }
            }
            else
            {
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById((int)temporaryDeliveryOrder.DeliveryOrderId);
                if (deliveryOrder == null)
                {
                    temporaryDeliveryOrder.Errors.Add("Generic", "Tidak terasosiasi dengan delivery order");
                }
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VHasDeliveryDate(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            if (temporaryDeliveryOrder.DeliveryDate == null)
            {
                temporaryDeliveryOrder.Errors.Add("DeliveryDate", "Tidak boleh kosong");
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VHasBeenConfirmed(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            if (!temporaryDeliveryOrder.IsConfirmed)
            {
                temporaryDeliveryOrder.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VHasNotBeenConfirmed(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            if (temporaryDeliveryOrder.IsConfirmed)
            {
                temporaryDeliveryOrder.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VHasTemporaryDeliveryOrderDetails(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            IList<TemporaryDeliveryOrderDetail> details = _temporaryDeliveryOrderDetailService.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrder.Id);
            if (!details.Any())
            {
                temporaryDeliveryOrder.Errors.Add("Generic", "Tidak memiliki delivery order detail");
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VHasNoTemporaryDeliveryOrderDetail(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            IList<TemporaryDeliveryOrderDetail> details = _temporaryDeliveryOrderDetailService.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrder.Id);
            if (details.Any())
            {
                temporaryDeliveryOrder.Errors.Add("Generic", "Masih memiliki delivery order detail");
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VVirtualOrderHasBeenConfirmed(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService)
        {
            if (temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.TrialOrder ||
                temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.SampleOrder)
            {
                VirtualOrder virtualOrder = _virtualOrderService.GetObjectById((int)temporaryDeliveryOrder.VirtualOrderId);
                if (!virtualOrder.IsConfirmed)
                {
                    temporaryDeliveryOrder.Errors.Add("Generic", "Virtual order belum terkonfirmasi");
                }
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VDeliveryOrderHasNotBeenConfirmed(TemporaryDeliveryOrder temporaryDeliveryOrder, IDeliveryOrderService _deliveryOrderService)
        {
            if (temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.PartDeliveryOrder)
            {
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById((int)temporaryDeliveryOrder.DeliveryOrderId);
                if (deliveryOrder.IsConfirmed)
                {
                    temporaryDeliveryOrder.Errors.Add("Generic", "Delivery order belum terkonfirmasi");
                }
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VHasConfirmationDate(TemporaryDeliveryOrder obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public TemporaryDeliveryOrder VGeneralLedgerPostingHasNotBeenClosed(TemporaryDeliveryOrder temporaryDeliveryOrder, IClosingService _closingService, DateTime PushDate)
        {
            if (_closingService.IsDateClosed(PushDate))
            {
                temporaryDeliveryOrder.Errors.Add("Generic", "Ledger sudah tutup buku");
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VAllQuantitiesEqualWasteAndRestock(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            IList<TemporaryDeliveryOrderDetail> temporaryDeliveryOrderDetails = _temporaryDeliveryOrderDetailService.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrder.Id);
            foreach (var detail in temporaryDeliveryOrderDetails)
            {
                if (detail.WasteQuantity + detail.RestockQuantity != detail.Quantity)
                {
                    temporaryDeliveryOrder.Errors.Add("Generic", "WasteQuantity + RestockQuantity harus sama dengan Quantity");
                    return temporaryDeliveryOrder;
                }
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VDeliveryOrderHasNotBeenConfirmedForPartDeliveryOrder(TemporaryDeliveryOrder temporaryDeliveryOrder, IDeliveryOrderService _deliveryOrderService)
        {
            if (temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.PartDeliveryOrder)
            {
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById((int)temporaryDeliveryOrder.DeliveryOrderId);
                if (deliveryOrder.IsConfirmed)
                {
                    temporaryDeliveryOrder.Errors.Add("Generic", "Delivery order sudah terkonfirmasi");
                }
            }
            return temporaryDeliveryOrder;
        }
        
        public TemporaryDeliveryOrder VCreateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService)
        {
            VHasWarehouse(temporaryDeliveryOrder, _warehouseService);
            if (!isValid(temporaryDeliveryOrder)) { return temporaryDeliveryOrder; }
            VHasVirtualOrderOrDeliveryOrder(temporaryDeliveryOrder, _virtualOrderService, _deliveryOrderService);
            if (!isValid(temporaryDeliveryOrder)) { return temporaryDeliveryOrder; }
            VVirtualOrderHasBeenConfirmed(temporaryDeliveryOrder, _virtualOrderService);
            if (!isValid(temporaryDeliveryOrder)) { return temporaryDeliveryOrder; }
            VDeliveryOrderHasNotBeenConfirmed(temporaryDeliveryOrder, _deliveryOrderService);
            if (!isValid(temporaryDeliveryOrder)) { return temporaryDeliveryOrder; }
            VHasDeliveryDate(temporaryDeliveryOrder);
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VUpdateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService)
        {
            VCreateObject(temporaryDeliveryOrder, _virtualOrderService, _deliveryOrderService, _warehouseService);
            if (!isValid(temporaryDeliveryOrder)) { return temporaryDeliveryOrder; }
            VHasNotBeenConfirmed(temporaryDeliveryOrder);
            
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VDeleteObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            VHasNotBeenConfirmed(temporaryDeliveryOrder);
            if (!isValid(temporaryDeliveryOrder)) { return temporaryDeliveryOrder; }
            VHasNoTemporaryDeliveryOrderDetail(temporaryDeliveryOrder, _temporaryDeliveryOrderDetailService);
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VConfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            VHasConfirmationDate(temporaryDeliveryOrder);
            if (!isValid(temporaryDeliveryOrder)) { return temporaryDeliveryOrder; }
            VHasNotBeenConfirmed(temporaryDeliveryOrder);
            if (!isValid(temporaryDeliveryOrder)) { return temporaryDeliveryOrder; }
            VHasTemporaryDeliveryOrderDetails(temporaryDeliveryOrder, _temporaryDeliveryOrderDetailService);
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VUnconfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            VHasBeenConfirmed(temporaryDeliveryOrder);
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder VPushObject(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IClosingService _closingService,
                                                  IDeliveryOrderService _deliveryOrderService)
        {
            VGeneralLedgerPostingHasNotBeenClosed(temporaryDeliveryOrder, _closingService, PushDate);
            if (!isValid(temporaryDeliveryOrder)) { return temporaryDeliveryOrder; }
            VAllQuantitiesEqualWasteAndRestock(temporaryDeliveryOrder, _temporaryDeliveryOrderDetailService);
            if (!isValid(temporaryDeliveryOrder)) { return temporaryDeliveryOrder; }
            VDeliveryOrderHasNotBeenConfirmedForPartDeliveryOrder(temporaryDeliveryOrder, _deliveryOrderService);
            return temporaryDeliveryOrder;
        }

        public bool ValidCreateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService)
        {
            VCreateObject(temporaryDeliveryOrder, _virtualOrderService, _deliveryOrderService, _warehouseService);
            return isValid(temporaryDeliveryOrder);
        }

        public bool ValidUpdateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService)
        {
            temporaryDeliveryOrder.Errors.Clear();
            VUpdateObject(temporaryDeliveryOrder, _virtualOrderService, _deliveryOrderService, _warehouseService);
            return isValid(temporaryDeliveryOrder);
        }

        public bool ValidDeleteObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            temporaryDeliveryOrder.Errors.Clear();
            VDeleteObject(temporaryDeliveryOrder, _temporaryDeliveryOrderDetailService);
            return isValid(temporaryDeliveryOrder);
        }

        public bool ValidConfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            temporaryDeliveryOrder.Errors.Clear();
            VConfirmObject(temporaryDeliveryOrder, _temporaryDeliveryOrderDetailService);
            return isValid(temporaryDeliveryOrder);
        }

        public bool ValidUnconfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            temporaryDeliveryOrder.Errors.Clear();
            VUnconfirmObject(temporaryDeliveryOrder);
            return isValid(temporaryDeliveryOrder);
        }

        public bool ValidPushObject(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                    IClosingService _closingService, IDeliveryOrderService _deliveryOrderService)
        {
            temporaryDeliveryOrder.Errors.Clear();
            VPushObject(temporaryDeliveryOrder, PushDate, _temporaryDeliveryOrderDetailService, _closingService, _deliveryOrderService);
            return isValid(temporaryDeliveryOrder);
        }

        public bool isValid(TemporaryDeliveryOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(TemporaryDeliveryOrder obj)
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