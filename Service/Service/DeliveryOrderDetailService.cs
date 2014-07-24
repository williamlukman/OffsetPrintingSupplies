using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class DeliveryOrderDetailService : IDeliveryOrderDetailService
    {
        private IDeliveryOrderDetailRepository _repository;
        private IDeliveryOrderDetailValidator _validator;

        public DeliveryOrderDetailService(IDeliveryOrderDetailRepository _deliveryOrderDetailRepository, IDeliveryOrderDetailValidator _deliveryOrderDetailValidator)
        {
            _repository = _deliveryOrderDetailRepository;
            _validator = _deliveryOrderDetailValidator;
        }

        public IDeliveryOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int deliveryOrderId)
        {
            return _repository.GetObjectsByDeliveryOrderId(deliveryOrderId);
        }

        public DeliveryOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public DeliveryOrderDetail GetObjectBySalesOrderDetailId(int salesOrderDetailId)
        {
            return _repository.GetObjectBySalesOrderDetailId(salesOrderDetailId);
        }

        public DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail, 
            IDeliveryOrderService _dos, ISalesOrderDetailService _sods,
            ISalesOrderService _sos, IItemService _is, IContactService _cs)
        {
            deliveryOrderDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(deliveryOrderDetail, this, _dos, _sods, _sos, _is, _cs))
            { 
                deliveryOrderDetail.ContactId = _dos.GetObjectById(deliveryOrderDetail.DeliveryOrderId).ContactId;
                return _repository.CreateObject(deliveryOrderDetail);
            }
            else
            {
                return deliveryOrderDetail;
            }
        }

        public DeliveryOrderDetail CreateObject(int deliveryOrderId, int itemId, int quantity, int salesOrderDetailId,
            IDeliveryOrderService _dos, ISalesOrderDetailService _sods,
            ISalesOrderService _sos, IItemService _is, IContactService _cs)
        {
            DeliveryOrderDetail dod = new DeliveryOrderDetail
            {
                DeliveryOrderId = deliveryOrderId,
                ItemId = itemId,
                Quantity = quantity,
                ContactId = 0,
                SalesOrderDetailId = salesOrderDetailId
            };
            return this.CreateObject(dod, _dos, _sods, _sos, _is, _cs);
        }

        public DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail,
            IDeliveryOrderService _dos, ISalesOrderDetailService _sods,
            ISalesOrderService _sos, IItemService _is, IContactService _cs)
        {
            return (_validator.ValidUpdateObject(deliveryOrderDetail, this, _dos, _sods, _sos, _is, _cs) ?
                    _repository.UpdateObject(deliveryOrderDetail) : deliveryOrderDetail);
        }

        public DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            return (_validator.ValidDeleteObject(deliveryOrderDetail) ? _repository.SoftDeleteObject(deliveryOrderDetail) : deliveryOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public DeliveryOrderDetail ConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidConfirmObject(deliveryOrderDetail, _itemService))
            {
                deliveryOrderDetail = _repository.ConfirmObject(deliveryOrderDetail);
                Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
                item.PendingDelivery -= deliveryOrderDetail.Quantity;
                item.Ready -= deliveryOrderDetail.Quantity;
                _itemService.UpdateObject(item);
                IList<StockMutation> sm = _stockMutationService.CreateStockMutationForDeliveryOrder(deliveryOrderDetail, item);
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail UnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidUnconfirmObject(deliveryOrderDetail, this, _itemService))
            {
                deliveryOrderDetail = _repository.UnconfirmObject(deliveryOrderDetail);
                Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
                item.PendingDelivery += deliveryOrderDetail.Quantity;
                item.Ready += deliveryOrderDetail.Quantity;
                _itemService.UpdateObject(item);
                IList<StockMutation> sm = _stockMutationService.SoftDeleteStockMutationForDeliveryOrder(deliveryOrderDetail, item);
            }
            return deliveryOrderDetail;
        }
    }
}