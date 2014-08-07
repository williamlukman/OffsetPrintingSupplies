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

        public DeliveryOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<DeliveryOrder> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public DeliveryOrder CreateObject(DeliveryOrder deliveryOrder, IContactService _contactService)
        {
            deliveryOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(deliveryOrder, _contactService) ? _repository.CreateObject(deliveryOrder) : deliveryOrder);
        }

        public DeliveryOrder CreateObject(int contactId, int warehouseId, DateTime deliveryDate, IContactService _contactService)
        {
            DeliveryOrder deliveryOrder = new DeliveryOrder
            {
                ContactId = contactId,
                WarehouseId = warehouseId,
                DeliveryDate = deliveryDate
            };
            return this.CreateObject(deliveryOrder, _contactService);
        }

        public DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder, IContactService _contactService)
        {
            return (_validator.ValidUpdateObject(deliveryOrder, _contactService) ? _repository.UpdateObject(deliveryOrder) : deliveryOrder);
        }

        public DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            return (_validator.ValidDeleteObject(deliveryOrder, _deliveryOrderDetailService) ? _repository.SoftDeleteObject(deliveryOrder) : deliveryOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                    ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidConfirmObject(deliveryOrder, _deliveryOrderDetailService, _itemService))
            {
                _repository.ConfirmObject(deliveryOrder);
            }
            return deliveryOrder;
        }

        public DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                    IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidUnconfirmObject(deliveryOrder, _deliveryOrderDetailService, _itemService))
            {
                _repository.UnconfirmObject(deliveryOrder);
            }
            return deliveryOrder;
        }

        public DeliveryOrder CompleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            return (deliveryOrder = _validator.ValidCompleteObject(deliveryOrder, _deliveryOrderDetailService) ? _repository.CompleteObject(deliveryOrder) : deliveryOrder);
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