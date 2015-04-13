using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class BlanketWarehouseMutationDetailValidator : IBlanketWarehouseMutationDetailValidator
    {
        public BlanketWarehouseMutationDetail VHasBlanketOrderDetail(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            BlanketOrderDetail blanketOrderDetail = _blanketOrderDetailService.GetObjectById(blanketWarehouseMutationDetail.BlanketOrderDetailId);
            if (blanketOrderDetail == null)
            {
                blanketWarehouseMutationDetail.Errors.Add("Generic", "Blanket work order tidak terasosiasi dengan Blanket order detail");
            }
            return blanketWarehouseMutationDetail;
        }

        //public BlanketWarehouseMutationDetail VCoreIdentificationDetailHasNotBeenDelivered(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail,
        //                                     IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService)
        //{
        //    BlanketOrderDetail blanketOrderDetail = _blanketOrderDetailService.GetObjectById(blanketWarehouseMutationDetail.BlanketOrderDetailId);
        //    CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(blanketOrderDetail.CoreIdentificationDetailId);
        //    if (coreIdentificationDetail.IsDelivered)
        //    {
        //        blanketWarehouseMutationDetail.Errors.Add("Generic", "Blanket sudah terkirim");
        //    }
        //    return blanketWarehouseMutationDetail;
        //}

        public BlanketWarehouseMutationDetail VBlanketOrderDetailHasBeenFinished(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            BlanketOrderDetail blanketOrderDetail = _blanketOrderDetailService.GetObjectById(blanketWarehouseMutationDetail.BlanketOrderDetailId);
            if (!blanketOrderDetail.IsFinished)
            {
                blanketWarehouseMutationDetail.Errors.Add("Generic", "Blanket work order detail belum selesai");
            }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VHasBlanketWarehouseMutation(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService)
        {
            BlanketWarehouseMutation blanketWarehouseMutation = _blanketWarehouseMutationService.GetObjectById(blanketWarehouseMutationDetail.BlanketWarehouseMutationId);
            if (blanketWarehouseMutation == null)
            {
                blanketWarehouseMutationDetail.Errors.Add("BlanketWarehouseMutationId", "Tidak terasosiasi dengan Blanket Warehouse Mutation");
            }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VHasWarehouseItemFrom(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService, IWarehouseItemService _warehouseItemService)
        {
            BlanketWarehouseMutation blanketWarehouseMutation = _blanketWarehouseMutationService.GetObjectById(blanketWarehouseMutationDetail.BlanketWarehouseMutationId);
            WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(blanketWarehouseMutation.WarehouseFromId, blanketWarehouseMutationDetail.ItemId);
            if (warehouseItemFrom == null)
            {
                blanketWarehouseMutationDetail.Errors.Add("Generic", "Tidak terasosiasi dengan item dari warehouse yang sebelum");
            }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VHasWarehouseItemTo(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService, IWarehouseItemService _warehouseItemService)
        {
            BlanketWarehouseMutation blanketWarehouseMutation = _blanketWarehouseMutationService.GetObjectById(blanketWarehouseMutationDetail.BlanketWarehouseMutationId);
            WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(blanketWarehouseMutation.WarehouseToId, blanketWarehouseMutationDetail.ItemId);
            if (warehouseItemTo == null)
            {
                blanketWarehouseMutationDetail.Errors.Add("Generic", "Tidak terasosiasi dengan item dari warehouse yang dituju");
            }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VUniqueBlanketOrderDetail(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService)
        {
            IList<BlanketWarehouseMutationDetail> details = _blanketWarehouseMutationDetailService.GetObjectsByBlanketWarehouseMutationId(blanketWarehouseMutationDetail.BlanketWarehouseMutationId);
            foreach (var detail in details)
            {
                if (detail.BlanketOrderDetailId == blanketWarehouseMutationDetail.BlanketOrderDetailId && detail.Id != blanketWarehouseMutationDetail.Id)
                {
                    blanketWarehouseMutationDetail.Errors.Add("Generic", "Tidak boleh ada duplikasi blanket work order detail dalam 1 Blanket Warehouse Mutation");
                }
            }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VBlanketWarehouseMutationHasBeenConfirmed(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService)
        {
            BlanketWarehouseMutation blanketWarehouseMutation = _blanketWarehouseMutationService.GetObjectById(blanketWarehouseMutationDetail.BlanketWarehouseMutationId);
            if (!blanketWarehouseMutation.IsConfirmed)
            {
                blanketWarehouseMutationDetail.Errors.Add("Generic", "BlanketWarehouseMutation harus sudah dikonfirmasi");
            }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VBlanketWarehouseMutationHasNotBeenConfirmed(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService)
        {
            BlanketWarehouseMutation blanketWarehouseMutation = _blanketWarehouseMutationService.GetObjectById(blanketWarehouseMutationDetail.BlanketWarehouseMutationId);
            if (blanketWarehouseMutation.IsConfirmed)
            {
                blanketWarehouseMutationDetail.Errors.Add("Generic", "BlanketWarehouseMutation sudah dikonfirmasi");
            }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VHasNotBeenConfirmed(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail)
        {
            if (blanketWarehouseMutationDetail.IsConfirmed)
            {
                blanketWarehouseMutationDetail.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi.");
            }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VHasBeenConfirmed(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail)
        {
            if (!blanketWarehouseMutationDetail.IsConfirmed)
            {
                blanketWarehouseMutationDetail.Errors.Add("Generic", "Harus sudah dikonfirmasi.");
            }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VNonNegativeStockQuantity(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                                                       IBlanketWarehouseMutationService _blanketWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                                       IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService, bool CaseConfirm)
        {
            int Quantity = CaseConfirm ? 1 : -1;
            BlanketWarehouseMutation blanketWarehouseMutation = _blanketWarehouseMutationService.GetObjectById(blanketWarehouseMutationDetail.BlanketWarehouseMutationId);
            WarehouseItem warehouseItem = null;
            if (CaseConfirm)
            {
                warehouseItem = _warehouseItemService.FindOrCreateObject(blanketWarehouseMutation.WarehouseFromId, blanketWarehouseMutationDetail.ItemId);
            }
            else
            {
                warehouseItem = _warehouseItemService.FindOrCreateObject(blanketWarehouseMutation.WarehouseToId, blanketWarehouseMutationDetail.ItemId);
            }
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketWarehouseMutation.BlanketOrderId);
            //CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(blanketOrder.CoreIdentificationId);
            //if (coreIdentification.IsInHouse)
            {
                if (warehouseItem.Quantity + Quantity < 0)
                {
                    blanketWarehouseMutationDetail.Errors.Add("Generic", "Stock barang " + item.Name + " di warehouse " + warehouseItem.Warehouse.Name + " tinggal " + warehouseItem.Quantity);
                    return blanketWarehouseMutationDetail;
                }
            }
            //else
            //{
            //    CustomerItem customerItemFrom = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), warehouseItem.Id);
            //    if (customerItemFrom.Quantity + Quantity < 0)
            //    {
            //        blanketWarehouseMutationDetail.Errors.Add("Generic", "Stock barang Customer " + item.Name + " di warehouse " + warehouseItem.Warehouse.Name + " tinggal " + customerItemFrom.Quantity);
            //        return blanketWarehouseMutationDetail;
            //    }
            //}
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VCreateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                           IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                           IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasBlanketOrderDetail(blanketWarehouseMutationDetail, _blanketOrderDetailService);
            if (!isValid(blanketWarehouseMutationDetail)) { return blanketWarehouseMutationDetail; }
            //VCoreIdentificationDetailHasNotBeenDelivered(blanketWarehouseMutationDetail, _blanketOrderDetailService, _coreIdentificationDetailService);
            //if (!isValid(blanketWarehouseMutationDetail)) { return blanketWarehouseMutationDetail; }
            VBlanketOrderDetailHasBeenFinished(blanketWarehouseMutationDetail, _blanketOrderDetailService);
            if (!isValid(blanketWarehouseMutationDetail)) { return blanketWarehouseMutationDetail; }
            VHasBlanketWarehouseMutation(blanketWarehouseMutationDetail, _blanketWarehouseMutationService);
            if (!isValid(blanketWarehouseMutationDetail)) { return blanketWarehouseMutationDetail; }
            VHasWarehouseItemFrom(blanketWarehouseMutationDetail, _blanketWarehouseMutationService, _warehouseItemService);
            if (!isValid(blanketWarehouseMutationDetail)) { return blanketWarehouseMutationDetail; }
            VHasWarehouseItemTo(blanketWarehouseMutationDetail, _blanketWarehouseMutationService, _warehouseItemService);
            if (!isValid(blanketWarehouseMutationDetail)) { return blanketWarehouseMutationDetail; }
            VUniqueBlanketOrderDetail(blanketWarehouseMutationDetail, _blanketWarehouseMutationDetailService);
            if (!isValid(blanketWarehouseMutationDetail)) { return blanketWarehouseMutationDetail; }
            VBlanketWarehouseMutationHasNotBeenConfirmed(blanketWarehouseMutationDetail, _blanketWarehouseMutationService);
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VUpdateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                           IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                           IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasNotBeenConfirmed(blanketWarehouseMutationDetail);
            if (!isValid(blanketWarehouseMutationDetail)) { return blanketWarehouseMutationDetail; }
            VCreateObject(blanketWarehouseMutationDetail, _blanketWarehouseMutationService, _blanketOrderDetailService, _coreIdentificationDetailService,
                          _blanketWarehouseMutationDetailService, _itemService, _warehouseItemService);
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VDeleteObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail)
        {
            VHasNotBeenConfirmed(blanketWarehouseMutationDetail);
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VHasConfirmationDate(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail)
        {
            if (blanketWarehouseMutationDetail.ConfirmationDate == null)
            {
                blanketWarehouseMutationDetail.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VConfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                                            IBlanketWarehouseMutationService _blanketWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                            IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            VHasConfirmationDate(blanketWarehouseMutationDetail);
            if (!isValid(blanketWarehouseMutationDetail)) { return blanketWarehouseMutationDetail; }
            VNonNegativeStockQuantity(blanketWarehouseMutationDetail, _blanketOrderService, _coreIdentificationService, _blanketWarehouseMutationService,
                                      _itemService, _blanketService, _warehouseItemService, _customerItemService, true);
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail VUnconfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                                              IBlanketWarehouseMutationService _blanketWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                              IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            VHasBeenConfirmed(blanketWarehouseMutationDetail);
            if (!isValid(blanketWarehouseMutationDetail)) { return blanketWarehouseMutationDetail; }
            VNonNegativeStockQuantity(blanketWarehouseMutationDetail, _blanketOrderService, _coreIdentificationService, _blanketWarehouseMutationService,
                                      _itemService, _blanketService, _warehouseItemService, _customerItemService, false);
            return blanketWarehouseMutationDetail;
        }

        public bool ValidCreateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                      IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                      IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(blanketWarehouseMutationDetail, _blanketWarehouseMutationService, _blanketOrderDetailService, _coreIdentificationDetailService,
                          _blanketWarehouseMutationDetailService, _itemService, _warehouseItemService);
            return isValid(blanketWarehouseMutationDetail);
        }

        public bool ValidUpdateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                      IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                      IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            blanketWarehouseMutationDetail.Errors.Clear();
            VUpdateObject(blanketWarehouseMutationDetail, _blanketWarehouseMutationService, _blanketOrderDetailService, _coreIdentificationDetailService,
                          _blanketWarehouseMutationDetailService, _itemService, _warehouseItemService);
            return isValid(blanketWarehouseMutationDetail);
        }

        public bool ValidDeleteObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail)
        {
            blanketWarehouseMutationDetail.Errors.Clear();
            VDeleteObject(blanketWarehouseMutationDetail);
            return isValid(blanketWarehouseMutationDetail);
        }

        public bool ValidConfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService, 
                                       IBlanketWarehouseMutationService _blanketWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                       IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            blanketWarehouseMutationDetail.Errors.Clear();
            VConfirmObject(blanketWarehouseMutationDetail, _blanketOrderService, _coreIdentificationService, _blanketWarehouseMutationService, _itemService, _blanketService, _warehouseItemService, _customerItemService);
            return isValid(blanketWarehouseMutationDetail);
        }

        public bool ValidUnconfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService, 
                                         IBlanketWarehouseMutationService _blanketWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                         IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            blanketWarehouseMutationDetail.Errors.Clear();
            VUnconfirmObject(blanketWarehouseMutationDetail, _blanketOrderService, _coreIdentificationService, _blanketWarehouseMutationService,
                             _itemService, _blanketService, _warehouseItemService, _customerItemService);
            return isValid(blanketWarehouseMutationDetail);
        }

        public bool isValid(BlanketWarehouseMutationDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(BlanketWarehouseMutationDetail obj)
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