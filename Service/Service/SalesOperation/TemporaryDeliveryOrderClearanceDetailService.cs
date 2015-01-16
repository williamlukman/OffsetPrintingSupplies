using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;

namespace Service.Service
{
    public class TemporaryDeliveryOrderClearanceDetailService : ITemporaryDeliveryOrderClearanceDetailService
    {
        private ITemporaryDeliveryOrderClearanceDetailRepository _repository;
        private ITemporaryDeliveryOrderClearanceDetailValidator _validator;

        public TemporaryDeliveryOrderClearanceDetailService(ITemporaryDeliveryOrderClearanceDetailRepository _temporaryDeliveryOrderClearanceDetailRepository, ITemporaryDeliveryOrderClearanceDetailValidator _temporaryDeliveryOrderClearanceDetailValidator)
        {
            _repository = _temporaryDeliveryOrderClearanceDetailRepository;
            _validator = _temporaryDeliveryOrderClearanceDetailValidator;
        }

        public ITemporaryDeliveryOrderClearanceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<TemporaryDeliveryOrderClearanceDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<TemporaryDeliveryOrderClearanceDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<TemporaryDeliveryOrderClearanceDetail> GetObjectsByTemporaryDeliveryOrderClearanceId(int temporaryDeliveryOrderClearanceId)
        {
            return _repository.GetObjectsByTemporaryDeliveryOrderClearanceId(temporaryDeliveryOrderClearanceId);
        }

        public TemporaryDeliveryOrderClearanceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public TemporaryDeliveryOrderClearanceDetail GetObjectByCode(string Code)
        {
            return _repository.GetObjectByCode(Code);
        }

        public IList<TemporaryDeliveryOrderClearanceDetail> GetObjectsByTemporaryDeliveryOrderDetailId(int temporaryDeliveryOrderDetailId)
        {
            return _repository.GetObjectsByTemporaryDeliveryOrderDetailId(temporaryDeliveryOrderDetailId);
        }

        public TemporaryDeliveryOrderClearanceDetail CreateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                         ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            temporaryDeliveryOrderClearanceDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(temporaryDeliveryOrderClearanceDetail, this, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService))
            { 
                _repository.CreateObject(temporaryDeliveryOrderClearanceDetail);
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail UpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                         ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            return (_validator.ValidUpdateObject(temporaryDeliveryOrderClearanceDetail, this, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService) ?
                    _repository.UpdateObject(temporaryDeliveryOrderClearanceDetail) : temporaryDeliveryOrderClearanceDetail);
        }

        public TemporaryDeliveryOrderClearanceDetail SoftDeleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            return (_validator.ValidDeleteObject(temporaryDeliveryOrderClearanceDetail) ? _repository.SoftDeleteObject(temporaryDeliveryOrderClearanceDetail) : temporaryDeliveryOrderClearanceDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public TemporaryDeliveryOrderClearanceDetail ConfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, DateTime ConfirmationDate, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                                   IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                                                    ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            temporaryDeliveryOrderClearanceDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceService, this, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _itemService, _warehouseItemService))
            {
                TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance = _temporaryDeliveryOrderClearanceService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId);
                TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail = _temporaryDeliveryOrderDetailService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetailId.GetValueOrDefault());
                TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(temporaryDeliveryOrder.WarehouseId, temporaryDeliveryOrderDetail.ItemId);
                Item item = _itemService.GetObjectById(temporaryDeliveryOrderDetail.ItemId);
                IList<StockMutation> stockMutations = null;
                if (temporaryDeliveryOrderClearance.IsWaste)
                {
                    // ready berkurang sejumlah waste
                    // virtual berkurang sejumlah yang dikembalikan
                    stockMutations = _stockMutationService.CreateStockMutationForTemporaryDeliveryOrderClearanceWaste(temporaryDeliveryOrderClearanceDetail, ConfirmationDate, warehouseItem);
                    temporaryDeliveryOrderClearanceDetail.WasteCoGS = temporaryDeliveryOrderClearanceDetail.Quantity * item.AvgPrice;
                    temporaryDeliveryOrderDetail.WasteQuantity += temporaryDeliveryOrderClearanceDetail.Quantity;
                }
                else
                {
                    // virtual berkurang sejumlah yang dikembalikan
                    stockMutations = _stockMutationService.CreateStockMutationForTemporaryDeliveryOrderClearanceReturn(temporaryDeliveryOrderClearanceDetail, ConfirmationDate, warehouseItem);
                    temporaryDeliveryOrderClearanceDetail.WasteCoGS = 0;
                    temporaryDeliveryOrderDetail.RestockQuantity += temporaryDeliveryOrderClearanceDetail.Quantity;
                    temporaryDeliveryOrderDetail.SellingPrice = temporaryDeliveryOrderClearanceDetail.SellingPrice;
                }
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingDelivery -= temporaryDeliveryOrderClearanceDetail.Quantity;
                    //item.Virtual -= stockMutation.Quantity;
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }

                // update TDO Detail to keep track wasted/return quantity
                _temporaryDeliveryOrderDetailService.GetRepository().UpdateObject(temporaryDeliveryOrderDetail);

                temporaryDeliveryOrderClearanceDetail = _repository.ConfirmObject(temporaryDeliveryOrderClearanceDetail);
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail UnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                            IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            if (_validator.ValidUnconfirmObject(temporaryDeliveryOrderClearanceDetail))
            {
                temporaryDeliveryOrderClearanceDetail.WasteCoGS = 0;
                temporaryDeliveryOrderClearanceDetail = _repository.UnconfirmObject(temporaryDeliveryOrderClearanceDetail);
                TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance = _temporaryDeliveryOrderClearanceService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId);
                TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail = _temporaryDeliveryOrderDetailService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetailId.GetValueOrDefault());
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(temporaryDeliveryOrderClearance.TemporaryDeliveryOrder.WarehouseId, temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.ItemId);
                Item item = _itemService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderDetail.ItemId);
                // jika waste, ready bertambah sejumlah waste & virtual bertambah sejumlah yang dikembalikan
                // jika return, virtual bertambah sejumlah yang dikembalikan
                IList<StockMutation> stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, temporaryDeliveryOrderClearance.IsWaste ? Constant.SourceDocumentDetailType.TemporaryDeliveryOrderClearanceDetailWaste : Constant.SourceDocumentDetailType.TemporaryDeliveryOrderClearanceDetailReturn, temporaryDeliveryOrderClearanceDetail.Id);
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingDelivery += temporaryDeliveryOrderClearanceDetail.Quantity;
                    //item.Virtual += stockMutation.Quantity;
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }
                _stockMutationService.DeleteStockMutations(stockMutations);

                // update TDO Detail to keep track wasted/return quantity
                if (temporaryDeliveryOrderClearance.IsWaste)
                {
                    temporaryDeliveryOrderDetail.WasteQuantity -= temporaryDeliveryOrderClearanceDetail.Quantity;
                }
                else
                {
                    temporaryDeliveryOrderDetail.RestockQuantity -= temporaryDeliveryOrderClearanceDetail.Quantity;
                }
                _temporaryDeliveryOrderDetailService.GetRepository().UpdateObject(temporaryDeliveryOrderDetail);
                
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail ProcessObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            if (_validator.ValidProcessObject(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderDetailService))
            {
                _repository.UpdateObject(temporaryDeliveryOrderClearanceDetail);
            }
            return temporaryDeliveryOrderClearanceDetail;
        }        
    }
}