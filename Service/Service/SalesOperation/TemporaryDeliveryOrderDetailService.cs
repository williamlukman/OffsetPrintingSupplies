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

        public TemporaryDeliveryOrderDetail GetObjectByCode(string Code)
        {
            return _repository.GetObjectByCode(Code);
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
                IList<StockMutation> stockMutations = _stockMutationService.DeleteStockMutationForTemporaryDeliveryOrder(temporaryDeliveryOrderDetail, warehouseItem);
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

        public TemporaryDeliveryOrderDetail ProcessObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, int wasteQuantity, int restockQuantity)
        {
            temporaryDeliveryOrderDetail.WasteQuantity = wasteQuantity;
            temporaryDeliveryOrderDetail.RestockQuantity = restockQuantity;
            _repository.UpdateObject(temporaryDeliveryOrderDetail);
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail ReconcileObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, DateTime PushDate, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                            IStockMutationService _stockMutationService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
                                                            IClosingService _closingService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService)
        {
            if (_validator.ValidReconcileObject(temporaryDeliveryOrderDetail, _closingService))
            {
                TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(temporaryDeliveryOrder.WarehouseId, temporaryDeliveryOrderDetail.ItemId);
                Item item = _itemService.GetObjectById(temporaryDeliveryOrderDetail.ItemId);
                if (temporaryDeliveryOrderDetail.WasteQuantity > 0)
                {
                    IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForTemporaryDeliveryOrderWaste(temporaryDeliveryOrderDetail, PushDate, warehouseItem);
                    foreach (var stockMutation in stockMutations)
                    {
                        _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    }
                    temporaryDeliveryOrderDetail.WasteCOGS = temporaryDeliveryOrderDetail.WasteQuantity * item.AvgPrice;
                }
                if (temporaryDeliveryOrderDetail.RestockQuantity > 0)
                {
                    IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForTemporaryDeliveryOrderRestock(temporaryDeliveryOrderDetail, PushDate, warehouseItem);
                    foreach (var stockMutation in stockMutations)
                    {
                        _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    }
                }
                temporaryDeliveryOrderDetail.IsReconciled = true;
                _repository.UpdateObject(temporaryDeliveryOrderDetail);
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail UnreconcileObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                              IStockMutationService _stockMutationService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
                                                              IClosingService _closingService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService)
        {
            if (_validator.ValidUnreconcileObject(temporaryDeliveryOrderDetail, _closingService))
            {
                temporaryDeliveryOrderDetail.IsReconciled = false;
                temporaryDeliveryOrderDetail.WasteCOGS = 0;
                _repository.UpdateObject(temporaryDeliveryOrderDetail);
                TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(temporaryDeliveryOrder.WarehouseId, temporaryDeliveryOrderDetail.ItemId);
                if (temporaryDeliveryOrderDetail.WasteQuantity > 0)
                {
                    IList<StockMutation> stockMutations = _stockMutationService.DeleteStockMutationForTemporaryDeliveryOrderWaste(temporaryDeliveryOrderDetail, warehouseItem);
                    foreach (var stockMutation in stockMutations)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    }
                }
                if (temporaryDeliveryOrderDetail.RestockQuantity > 0)
                {
                    IList<StockMutation> stockMutations = _stockMutationService.DeleteStockMutationForTemporaryDeliveryOrderRestock(temporaryDeliveryOrderDetail, warehouseItem);
                    foreach (var stockMutation in stockMutations)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    }
                }
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail CompleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            temporaryDeliveryOrderDetail.IsAllCompleted = true;
            _repository.UpdateObject(temporaryDeliveryOrderDetail);
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail UndoCompleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            temporaryDeliveryOrderDetail.IsAllCompleted = false;
            _repository.UpdateObject(temporaryDeliveryOrderDetail);
            return temporaryDeliveryOrderDetail;
        }
    }
}