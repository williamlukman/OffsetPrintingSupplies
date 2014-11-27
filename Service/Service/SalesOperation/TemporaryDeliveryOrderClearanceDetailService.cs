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
                                                         IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                                         IDeliveryOrderService _deliveryOrderService, IItemService _itemService)
        {
            temporaryDeliveryOrderClearanceDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(temporaryDeliveryOrderClearanceDetail, this, _temporaryDeliveryOrderClearanceService, _virtualOrderDetailService,
                                             _salesOrderDetailService, _deliveryOrderService,_itemService))
            { 
                _repository.CreateObject(temporaryDeliveryOrderClearanceDetail);
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail UpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                         IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                                         IDeliveryOrderService _deliveryOrderService, IItemService _itemService)
        {
            return (_validator.ValidUpdateObject(temporaryDeliveryOrderClearanceDetail, this, _temporaryDeliveryOrderClearanceService, _virtualOrderDetailService,
                                                 _salesOrderDetailService, _deliveryOrderService, _itemService) ?
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
                                                          IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService,
                                                          IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            temporaryDeliveryOrderClearanceDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(temporaryDeliveryOrderClearanceDetail, _temporaryDeliveryOrderClearanceService, this, _virtualOrderDetailService,
                                              _salesOrderDetailService, _itemService, _warehouseItemService))
            {
                temporaryDeliveryOrderClearanceDetail = _repository.ConfirmObject(temporaryDeliveryOrderClearanceDetail);

                TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance = _temporaryDeliveryOrderClearanceService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId);    
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(temporaryDeliveryOrderClearance.WarehouseId, temporaryDeliveryOrderClearanceDetail.ItemId);
                Item item = _itemService.GetObjectById(temporaryDeliveryOrderClearanceDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForTemporaryDeliveryOrderClearance(temporaryDeliveryOrderClearanceDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingDelivery -= temporaryDeliveryOrderClearanceDetail.Quantity;
                    //item.Quantity -= temporaryDeliveryOrderClearanceDetail.Quantity;
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }

            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail UnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                            IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService,
                                                            ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService,
                                                            IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(temporaryDeliveryOrderClearanceDetail))
            {
                temporaryDeliveryOrderClearanceDetail = _repository.UnconfirmObject(temporaryDeliveryOrderClearanceDetail);
                TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance = _temporaryDeliveryOrderClearanceService.GetObjectById(temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(temporaryDeliveryOrderClearance.WarehouseId, temporaryDeliveryOrderClearanceDetail.ItemId);
                Item item = _itemService.GetObjectById(temporaryDeliveryOrderClearanceDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.DeleteStockMutationForTemporaryDeliveryOrderClearance(temporaryDeliveryOrderClearanceDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingDelivery += temporaryDeliveryOrderClearanceDetail.Quantity;
                    //item.Quantity += temporaryDeliveryOrderClearanceDetail.Quantity;
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }

                if (temporaryDeliveryOrderClearance.OrderType == Core.Constants.Constant.OrderTypeCase.SampleOrder ||
                    temporaryDeliveryOrderClearance.OrderType == Core.Constants.Constant.OrderTypeCase.TrialOrder)
                {
                    VirtualOrderDetail virtualOrderDetail = _virtualOrderDetailService.GetObjectById((int)temporaryDeliveryOrderClearanceDetail.VirtualOrderDetailId);
                    _virtualOrderDetailService.UnsetDeliveryComplete(virtualOrderDetail, temporaryDeliveryOrderClearanceDetail.Quantity, _virtualOrderService);
                }
                else
                {
                    SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById((int)temporaryDeliveryOrderClearanceDetail.SalesOrderDetailId);
                    _salesOrderDetailService.UnsetDeliveryComplete(salesOrderDetail, temporaryDeliveryOrderClearanceDetail.Quantity, _salesOrderService);
                }
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail ProcessObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            if (_validator.ValidProcessObject(temporaryDeliveryOrderClearanceDetail))
            {
                _repository.UpdateObject(temporaryDeliveryOrderClearanceDetail);
            }
            return temporaryDeliveryOrderClearanceDetail;
        }

        
    }
}