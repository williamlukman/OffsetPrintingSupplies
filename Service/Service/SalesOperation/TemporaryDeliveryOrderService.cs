using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class TemporaryDeliveryOrderService : ITemporaryDeliveryOrderService
    {
        private ITemporaryDeliveryOrderRepository _repository;
        private ITemporaryDeliveryOrderValidator _validator;

        public TemporaryDeliveryOrderService(ITemporaryDeliveryOrderRepository _temporaryDeliveryOrderRepository, ITemporaryDeliveryOrderValidator _temporaryDeliveryOrderValidator)
        {
            _repository = _temporaryDeliveryOrderRepository;
            _validator = _temporaryDeliveryOrderValidator;
        }

        public ITemporaryDeliveryOrderValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<TemporaryDeliveryOrder> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<TemporaryDeliveryOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<TemporaryDeliveryOrder> GetConfirmedObjects()
        {
            return _repository.GetConfirmedObjects();
        }

        public TemporaryDeliveryOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<TemporaryDeliveryOrder> GetObjectsByVirtualOrderId(int virtualOrderId)
        {
            return _repository.GetObjectsByVirtualOrderId(virtualOrderId);
        }

        public IList<TemporaryDeliveryOrder> GetObjectsByDeliveryOrderId(int deliveryOrderId)
        {
            return _repository.GetObjectsByDeliveryOrderId(deliveryOrderId);
        }

        public TemporaryDeliveryOrder CreateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService)
        {
            temporaryDeliveryOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(temporaryDeliveryOrder, _virtualOrderService, _deliveryOrderService, _warehouseService) ? _repository.CreateObject(temporaryDeliveryOrder) : temporaryDeliveryOrder);
        }

        public TemporaryDeliveryOrder UpdateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService)
        {
            return (_validator.ValidUpdateObject(temporaryDeliveryOrder, _virtualOrderService, _deliveryOrderService, _warehouseService) ? _repository.UpdateObject(temporaryDeliveryOrder) : temporaryDeliveryOrder);
        }

        public TemporaryDeliveryOrder SoftDeleteObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            return (_validator.ValidDeleteObject(temporaryDeliveryOrder, _temporaryDeliveryOrderDetailService) ? _repository.SoftDeleteObject(temporaryDeliveryOrder) : temporaryDeliveryOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public TemporaryDeliveryOrder ConfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime ConfirmationDate, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                     IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                     IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                     ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService,
                                     IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            temporaryDeliveryOrder.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(temporaryDeliveryOrder, _temporaryDeliveryOrderDetailService))
            {
                IList<TemporaryDeliveryOrderDetail> temporaryDeliveryOrderDetails = _temporaryDeliveryOrderDetailService.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrder.Id);
                foreach (var detail in temporaryDeliveryOrderDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _temporaryDeliveryOrderDetailService.ConfirmObject(detail, ConfirmationDate, this, _virtualOrderDetailService, _salesOrderDetailService, _stockMutationService, _itemService,
                                                                      _blanketService, _warehouseItemService);
                }
                _repository.ConfirmObject(temporaryDeliveryOrder);
                if (temporaryDeliveryOrder.OrderType == Core.Constants.Constant.OrderTypeCase.SampleOrder ||
                    temporaryDeliveryOrder.OrderType == Core.Constants.Constant.OrderTypeCase.TrialOrder)
                {
                    VirtualOrder virtualOrder = _virtualOrderService.GetObjectById((int) temporaryDeliveryOrder.VirtualOrderId);
                    _virtualOrderService.CheckAndSetDeliveryComplete(virtualOrder, _virtualOrderDetailService);
                }
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder UnconfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                                      IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                      IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                                      ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                                      IStockMutationService _stockMutationService, IItemService _itemService,
                                                      IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(temporaryDeliveryOrder))
            {
                IList<TemporaryDeliveryOrderDetail> temporaryDeliveryOrderDetails = _temporaryDeliveryOrderDetailService.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrder.Id);
                foreach (var detail in temporaryDeliveryOrderDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _temporaryDeliveryOrderDetailService.UnconfirmObject(detail, this, _virtualOrderDetailService, _virtualOrderService, _salesOrderService, _salesOrderDetailService,
                                                                         _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                }
                _repository.UnconfirmObject(temporaryDeliveryOrder);
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder ReconcileObject(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                                      IStockMutationService _stockMutationService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
                                                      IClosingService _closingService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService)
        {
            decimal TotalWasteCOGS = 0;
            IList<TemporaryDeliveryOrderDetail> details = _temporaryDeliveryOrderDetailService.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrder.Id);
            foreach (var detail in details)
            {
                _temporaryDeliveryOrderDetailService.ReconcileObject(detail, PushDate, this, _stockMutationService, _accountService, _generalLedgerJournalService, _closingService,
                                                                     _warehouseItemService, _itemService, _blanketService);
                TotalWasteCOGS += detail.WasteCOGS;
            }
            temporaryDeliveryOrder.TotalWasteCOGS = TotalWasteCOGS;
            _repository.ReconcileObject(temporaryDeliveryOrder);
            _generalLedgerJournalService.CreateReconciliationJournalForTemporaryDeliveryOrderWaste(temporaryDeliveryOrder, PushDate, _accountService);
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder UnreconcileObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                                      IStockMutationService _stockMutationService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
                                                      IClosingService _closingService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService)
        {
            IList<TemporaryDeliveryOrderDetail> details = _temporaryDeliveryOrderDetailService.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrder.Id);
            foreach (var detail in details)
            {
                _temporaryDeliveryOrderDetailService.UnreconcileObject(detail, this, _stockMutationService, _accountService, _generalLedgerJournalService, _closingService,
                                                                       _warehouseItemService, _itemService, _blanketService);
            }
            _generalLedgerJournalService.CreateUnreconciliationJournalForTemporaryDeliveryOrderWaste(temporaryDeliveryOrder, _accountService);
            temporaryDeliveryOrder.TotalWasteCOGS = 0;
            _repository.UnreconcileObject(temporaryDeliveryOrder);
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder PushObject(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                                 IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderService _salesOrderService,
                                                 ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService,
                                                 IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService, IStockMutationService _stockMutationService,
                                                 IContactService _contactService, IBlanketService _blanketService, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService,
                                                 IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                                 IServiceCostService _serviceCostService, ISalesQuotationService _salesQuotationService, ISalesQuotationDetailService _salesQuotationDetailService)
        {
            if (_validator.ValidPushObject(temporaryDeliveryOrder, PushDate, _temporaryDeliveryOrderDetailService, _closingService, _deliveryOrderService))
            {
                ReconcileObject(temporaryDeliveryOrder, PushDate, _temporaryDeliveryOrderDetailService, _stockMutationService, _accountService, _generalLedgerJournalService,
                                _closingService, _warehouseItemService, _itemService, _blanketService);

                IList<TemporaryDeliveryOrderDetail> temporaryDeliveryOrderDetails = _temporaryDeliveryOrderDetailService.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrder.Id);
                #region Part Delivery Order
                if (temporaryDeliveryOrder.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder)
                {
                    DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById((int)temporaryDeliveryOrder.DeliveryOrderId);
                    foreach (var tempDetail in temporaryDeliveryOrderDetails)
                    {
                        DeliveryOrderDetail deliveryOrderDetail = new DeliveryOrderDetail()
                        {
                            DeliveryOrderId = deliveryOrder.Id,
                            OrderType = temporaryDeliveryOrder.OrderType,
                            OrderCode = tempDetail.Code,
                            ItemId = tempDetail.ItemId,
                            Quantity = tempDetail.RestockQuantity,
                            PendingInvoicedQuantity = tempDetail.RestockQuantity,
                            SalesOrderDetailId = (int)tempDetail.SalesOrderDetailId,
                        };
                        _deliveryOrderDetailService.CreateObject(deliveryOrderDetail, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);
                    }
                }
                #endregion
                #region Virtual Order
                else
                {
                    VirtualOrder virtualOrder = _virtualOrderService.GetObjectById((int)temporaryDeliveryOrder.VirtualOrderId);
                    SalesOrder salesOrder = new SalesOrder()
                    {
                        ContactId = virtualOrder.ContactId,
                        SalesDate = PushDate,
                        OrderType = virtualOrder.OrderType,
                        OrderCode = virtualOrder.Code,
                        IsLegacy = true
                    };
                    _salesOrderService.CreateObject(salesOrder, _contactService, _salesQuotationService);

                    foreach (var tempDetail in temporaryDeliveryOrderDetails)
                    {
                        VirtualOrderDetail virtualOrderDetail = _virtualOrderDetailService.GetObjectById((int)tempDetail.VirtualOrderDetailId);
                        SalesOrderDetail salesOrderDetail = new SalesOrderDetail()
                        {
                            ItemId = tempDetail.ItemId,
                            Quantity = tempDetail.RestockQuantity,
                            Price = virtualOrderDetail.Price,
                            SalesOrderId = salesOrder.Id,
                            OrderCode = virtualOrderDetail.Code,
                            PendingDeliveryQuantity = tempDetail.RestockQuantity
                        };
                        _salesOrderDetailService.CreateObject(salesOrderDetail, _salesOrderService, _itemService, _salesQuotationDetailService);
                    }

                    _salesOrderService.ConfirmObject(salesOrder, PushDate, _salesOrderDetailService,
                                                     _stockMutationService, _itemService, _blanketService, _warehouseItemService);

                    IList<SalesOrderDetail> salesOrderDetails = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
                    DeliveryOrder deliveryOrder = new DeliveryOrder()
                    {
                        SalesOrderId = salesOrder.Id,
                        WarehouseId = temporaryDeliveryOrder.WarehouseId,
                        DeliveryDate = PushDate
                    };
                    _deliveryOrderService.CreateObject(deliveryOrder, _salesOrderService, _warehouseService);

                    foreach (var salesDetail in salesOrderDetails)
                    {
                        DeliveryOrderDetail deliveryOrderDetail = new DeliveryOrderDetail()
                        {
                            OrderType = salesOrder.OrderType,
                            OrderCode = salesDetail.OrderCode,
                            SalesOrderDetailId = salesDetail.Id,
                            DeliveryOrderId = deliveryOrder.Id,
                            ItemId = salesDetail.ItemId,
                            Quantity = salesDetail.Quantity,
                            PendingInvoicedQuantity = salesDetail.Quantity,
                        };
                        _deliveryOrderDetailService.CreateObject(deliveryOrderDetail, _deliveryOrderService,
                                                                 _salesOrderDetailService, _salesOrderService, _itemService);
                    }
                    //_deliveryOrderService.ConfirmObject(deliveryOrder, PushDate, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService,
                    //                                    _stockMutationService, _itemService, _blanketService, _warehouseItemService, _accountService,
                    //                                    _generalLedgerJournalService, _closingService, _serviceCostService, _temporaryDeliveryOrderDetailService, this);
                }
                #endregion
            }
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder CheckAndSetDeliveryComplete(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            IList<TemporaryDeliveryOrderDetail> details = _temporaryDeliveryOrderDetailService.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrder.Id);

            foreach (var detail in details)
            {
                if (!detail.IsAllCompleted)
                {
                    return temporaryDeliveryOrder;
                }
            }
            return _repository.SetDeliveryComplete(temporaryDeliveryOrder);
        }

        public TemporaryDeliveryOrder UnsetDeliveryComplete(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            _repository.UnsetDeliveryComplete(temporaryDeliveryOrder);
            return temporaryDeliveryOrder;
        }
    }
}