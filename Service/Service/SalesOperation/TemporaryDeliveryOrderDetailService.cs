using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class TemporaryDeliveryOrderDetailService : ITemporaryDeliveryOrderDetailService
    {
        private ITemporaryDeliveryOrderDetailRepository _repository;
        private ITemporaryDeliveryOrderDetailValidator _validator;

        public TemporaryDeliveryOrderDetailService(ITemporaryDeliveryOrderDetailRepository _temporaryDeliveryOrderDetailRepository, ITemporaryDeliveryOrderDetailValidator _temporaryDeliveryOrderDetailValidator)
        {
            _repository = _temporaryDeliveryOrderDetailRepository;
            _validator = _temporaryDeliveryOrderDetailValidator;
        }

        public ITemporaryDeliveryOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<TemporaryDeliveryOrderDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<TemporaryDeliveryOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<TemporaryDeliveryOrderDetail> GetObjectsByTemporaryDeliveryOrderId(int temporaryDeliveryOrderId)
        {
            return _repository.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrderId);
        }

        public TemporaryDeliveryOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<TemporaryDeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int salesOrderDetailId)
        {
            return _repository.GetObjectsBySalesOrderDetailId(salesOrderDetailId);
        }

        public IList<TemporaryDeliveryOrderDetail> GetObjectsByVirtualOrderDetailId(int virtualOrderDetailId)
        {
            return _repository.GetObjectsByVirtualOrderDetailId(virtualOrderDetailId);
        }

        public TemporaryDeliveryOrderDetail CreateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                         IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                                         IDeliveryOrderService _deliveryOrderService, IItemService _itemService)
        {
            temporaryDeliveryOrderDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(temporaryDeliveryOrderDetail, this, _temporaryDeliveryOrderService, _virtualOrderDetailService,
                                             _salesOrderDetailService, _deliveryOrderService,_itemService))
            { 
                _repository.CreateObject(temporaryDeliveryOrderDetail);
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail UpdateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                         IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                                         IDeliveryOrderService _deliveryOrderService, IItemService _itemService)
        {
            return (_validator.ValidUpdateObject(temporaryDeliveryOrderDetail, this, _temporaryDeliveryOrderService, _virtualOrderDetailService,
                                                 _salesOrderDetailService, _deliveryOrderService, _itemService) ?
                    _repository.UpdateObject(temporaryDeliveryOrderDetail) : temporaryDeliveryOrderDetail);
        }

        public TemporaryDeliveryOrderDetail SoftDeleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            return (_validator.ValidDeleteObject(temporaryDeliveryOrderDetail) ? _repository.SoftDeleteObject(temporaryDeliveryOrderDetail) : temporaryDeliveryOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public TemporaryDeliveryOrderDetail ConfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, DateTime ConfirmationDate, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                          IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService,
                                                          IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            temporaryDeliveryOrderDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService, this, _virtualOrderDetailService,
                                              _salesOrderDetailService, _itemService, _warehouseItemService))
            {
                temporaryDeliveryOrderDetail = _repository.ConfirmObject(temporaryDeliveryOrderDetail);

                TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);    
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(temporaryDeliveryOrder.WarehouseId, temporaryDeliveryOrderDetail.ItemId);
                Item item = _itemService.GetObjectById(temporaryDeliveryOrderDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForTemporaryDeliveryOrder(temporaryDeliveryOrderDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingDelivery -= temporaryDeliveryOrderDetail.Quantity;
                    //item.Quantity -= temporaryDeliveryOrderDetail.Quantity;
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }

                if (temporaryDeliveryOrder.OrderType == Core.Constants.Constant.OrderTypeCase.SampleOrder ||
                    temporaryDeliveryOrder.OrderType == Core.Constants.Constant.OrderTypeCase.TrialOrder)
                {
                    VirtualOrderDetail virtualOrderDetail = _virtualOrderDetailService.GetObjectById((int)temporaryDeliveryOrderDetail.VirtualOrderDetailId);
                    _virtualOrderDetailService.SetDeliveryComplete(virtualOrderDetail, temporaryDeliveryOrderDetail.Quantity); 
                }
                else
                {
                    SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById((int) temporaryDeliveryOrderDetail.SalesOrderDetailId);
                    _salesOrderDetailService.SetDeliveryComplete(salesOrderDetail, temporaryDeliveryOrderDetail.Quantity);
                }
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail UnconfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                            IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService,
                                                            ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService,
                                                            IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(temporaryDeliveryOrderDetail))
            {
                temporaryDeliveryOrderDetail = _repository.UnconfirmObject(temporaryDeliveryOrderDetail);
                TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(temporaryDeliveryOrder.WarehouseId, temporaryDeliveryOrderDetail.ItemId);
                Item item = _itemService.GetObjectById(temporaryDeliveryOrderDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForTemporaryDeliveryOrder(temporaryDeliveryOrderDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingDelivery += temporaryDeliveryOrderDetail.Quantity;
                    //item.Quantity += temporaryDeliveryOrderDetail.Quantity;
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }

                if (temporaryDeliveryOrder.OrderType == Core.Constants.Constant.OrderTypeCase.SampleOrder ||
                    temporaryDeliveryOrder.OrderType == Core.Constants.Constant.OrderTypeCase.TrialOrder)
                {
                    VirtualOrderDetail virtualOrderDetail = _virtualOrderDetailService.GetObjectById((int)temporaryDeliveryOrderDetail.VirtualOrderDetailId);
                    _virtualOrderDetailService.UnsetDeliveryComplete(virtualOrderDetail, temporaryDeliveryOrderDetail.Quantity, _virtualOrderService);
                }
                else
                {
                    SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById((int)temporaryDeliveryOrderDetail.SalesOrderDetailId);
                    _salesOrderDetailService.UnsetDeliveryComplete(salesOrderDetail, temporaryDeliveryOrderDetail.Quantity, _salesOrderService);
                }
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail ReconcileObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, int wasteQuantity, int restockQuantity)
        {
            // TODO: validasi + automation
            temporaryDeliveryOrderDetail.WasteQuantity = wasteQuantity;
            temporaryDeliveryOrderDetail.RestockQuantity = restockQuantity;
            if (temporaryDeliveryOrderDetail.WasteQuantity + temporaryDeliveryOrderDetail.RestockQuantity == temporaryDeliveryOrderDetail.Quantity)
            {
                temporaryDeliveryOrderDetail.IsAllCompleted = true;
            }
            _repository.UpdateObject(temporaryDeliveryOrderDetail);
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail UnreconcileObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, int wasteQuantity, int restockQuantity, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            temporaryDeliveryOrderDetail.WasteQuantity = 0;
            temporaryDeliveryOrderDetail.RestockQuantity = 0;
            temporaryDeliveryOrderDetail.IsAllCompleted = false;
            _repository.UpdateObject(temporaryDeliveryOrderDetail);
            return temporaryDeliveryOrderDetail;
        }

    }
}