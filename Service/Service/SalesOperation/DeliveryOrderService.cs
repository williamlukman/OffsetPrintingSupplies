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
    public class DeliveryOrderService : IDeliveryOrderService
    {
        private IDeliveryOrderRepository _repository;
        private IDeliveryOrderValidator _validator;

        public DeliveryOrderService(IDeliveryOrderRepository _deliveryOrderRepository, IDeliveryOrderValidator _deliveryOrderValidator)
        {
            _repository = _deliveryOrderRepository;
            _validator = _deliveryOrderValidator;
        }

        public IDeliveryOrderValidator GetValidator()
        {
            return _validator;
        }

        public IList<DeliveryOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<DeliveryOrder> GetConfirmedObjects()
        {
            return _repository.GetConfirmedObjects();
        }

        public DeliveryOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<DeliveryOrder> GetObjectsBySalesOrderId(int salesOrderId)
        {
            return _repository.GetObjectsBySalesOrderId(salesOrderId);
        }

        public DeliveryOrder CreateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IWarehouseService _warehouseService)
        {
            deliveryOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(deliveryOrder, _salesOrderService, _warehouseService) ? _repository.CreateObject(deliveryOrder) : deliveryOrder);
        }

        public DeliveryOrder CreateObject(int warehouseId, int salesOrderId, DateTime deliveryDate, ISalesOrderService _salesOrderService, IWarehouseService _warehouseService)
        {
            DeliveryOrder deliveryOrder = new DeliveryOrder
            {
                SalesOrderId = salesOrderId,
                WarehouseId = warehouseId,
                DeliveryDate = deliveryDate
            };
            return this.CreateObject(deliveryOrder, _salesOrderService, _warehouseService);
        }

        public DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IWarehouseService _warehouseService)
        {
            return (_validator.ValidUpdateObject(deliveryOrder, _salesOrderService, _warehouseService) ? _repository.UpdateObject(deliveryOrder) : deliveryOrder);
        }

        public DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            return (_validator.ValidDeleteObject(deliveryOrder, _deliveryOrderDetailService) ? _repository.SoftDeleteObject(deliveryOrder) : deliveryOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder, DateTime ConfirmationDate, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                           ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                           IStockMutationService _stockMutationService, IItemService _itemService,
                                           IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            deliveryOrder.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(deliveryOrder, _deliveryOrderDetailService))
            {
                IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
                foreach (var detail in deliveryOrderDetails)
                {
                    _deliveryOrderDetailService.ConfirmObject(detail, ConfirmationDate, this, _salesOrderDetailService, _stockMutationService, _itemService,
                                                              _barringService, _warehouseItemService);
                }
                _repository.ConfirmObject(deliveryOrder);
                SalesOrder salesOrder = _salesOrderService.GetObjectById(deliveryOrder.SalesOrderId);
                _salesOrderService.CheckAndSetDeliveryComplete(salesOrder, _salesOrderDetailService);
            }
            return deliveryOrder;
        }

        public DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                             ISalesInvoiceService _salesInvoiceService, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                             ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                             IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService,
                                             IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(deliveryOrder, _salesInvoiceService))
            {
                IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
                foreach (var detail in deliveryOrderDetails)
                {
                    _deliveryOrderDetailService.UnconfirmObject(detail, this, _salesOrderService, _salesOrderDetailService,
                                                                _salesInvoiceDetailService, _stockMutationService,
                                                                _itemService, _barringService, _warehouseItemService);
                }
                _repository.UnconfirmObject(deliveryOrder);
            }
            return deliveryOrder;
        }

        public DeliveryOrder CheckAndSetInvoiceComplete(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            IList<DeliveryOrderDetail> details = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);

            foreach (var detail in details)
            {
                if (!detail.IsAllInvoiced)
                {
                    return deliveryOrder;
                }
            }
            return _repository.SetInvoiceComplete(deliveryOrder);
        }

        public DeliveryOrder UnsetInvoiceComplete(DeliveryOrder deliveryOrder)
        {
            _repository.UnsetInvoiceComplete(deliveryOrder);
            return deliveryOrder;
        }
    }
}