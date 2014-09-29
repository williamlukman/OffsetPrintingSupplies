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

        public TemporaryDeliveryOrder SetReconcileComplete(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            IList<TemporaryDeliveryOrderDetail> details = _temporaryDeliveryOrderDetailService.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrder.Id);

            foreach (var detail in details)
            {
                if (!detail.IsAllCompleted)
                {
                    return temporaryDeliveryOrder;
                }
            }
            return _repository.SetReconcileComplete(temporaryDeliveryOrder);
        }

        public TemporaryDeliveryOrder UnsetReconcileComplete(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            _repository.UnsetReconcileComplete(temporaryDeliveryOrder);
            return temporaryDeliveryOrder;
        }
    }
}