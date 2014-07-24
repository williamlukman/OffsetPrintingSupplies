using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public DeliveryOrder CreateObject(DeliveryOrder deliveryOrder, IContactService _cs)
        {
            deliveryOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(deliveryOrder, _cs) ? _repository.CreateObject(deliveryOrder) : deliveryOrder);
        }

        public DeliveryOrder CreateObject(int contactId, DateTime deliveryDate, IContactService _cs)
        {
            DeliveryOrder deliveryOrder = new DeliveryOrder
            {
                ContactId = contactId,
                DeliveryDate = deliveryDate
            };
            return this.CreateObject(deliveryOrder, _cs);
        }

        public DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder, IContactService _cs)
        {
            return (_validator.ValidUpdateObject(deliveryOrder, _cs) ? _repository.UpdateObject(deliveryOrder) : deliveryOrder);
        }

        public DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _dods)
        {
            return (_validator.ValidDeleteObject(deliveryOrder, _dods) ? _repository.SoftDeleteObject(deliveryOrder) : deliveryOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _dods,
                                    ISalesOrderDetailService _sods, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidConfirmObject(deliveryOrder, _dods, _itemService))
            {
                IList<DeliveryOrderDetail> details = _dods.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
                foreach (var detail in details)
                {
                    detail.ConfirmedAt = deliveryOrder.ConfirmedAt;
                    _dods.ConfirmObject(detail, _stockMutationService, _itemService);
                    SalesOrderDetail sod = _sods.GetObjectById(detail.SalesOrderDetailId);
                    _sods.FulfilObject(sod);
                }

                return _repository.ConfirmObject(deliveryOrder);
            }
            return deliveryOrder;
        }

        public DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                    IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidUnconfirmObject(deliveryOrder, _deliveryOrderDetailService, _itemService))
            {
                IList<DeliveryOrderDetail> details = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
                foreach (var detail in details)
                {
                    _deliveryOrderDetailService.UnconfirmObject(detail, _stockMutationService, _itemService);
                }

                return _repository.UnconfirmObject(deliveryOrder);
            }
            return deliveryOrder;
        }
    }
}