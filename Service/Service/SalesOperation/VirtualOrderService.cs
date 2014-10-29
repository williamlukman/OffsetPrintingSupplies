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
    public class VirtualOrderService : IVirtualOrderService
    {
        private IVirtualOrderRepository _repository;
        private IVirtualOrderValidator _validator;

        public VirtualOrderService(IVirtualOrderRepository _virtualOrderRepository, IVirtualOrderValidator _virtualOrderValidator)
        {
            _repository = _virtualOrderRepository;
            _validator = _virtualOrderValidator;
        }

        public IVirtualOrderValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<VirtualOrder> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<VirtualOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<VirtualOrder> GetConfirmedObjects()
        {
            return _repository.GetConfirmedObjects();
        }

        public VirtualOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<VirtualOrder> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }
        
        public VirtualOrder CreateObject(VirtualOrder virtualOrder, IContactService _contactService)
        {
            virtualOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(virtualOrder, this, _contactService) ? _repository.CreateObject(virtualOrder) : virtualOrder);
        }

        public VirtualOrder UpdateObject(VirtualOrder virtualOrder, IContactService _contactService)
        {
            return (_validator.ValidUpdateObject(virtualOrder, this, _contactService) ? _repository.UpdateObject(virtualOrder) : virtualOrder);
        }

        public VirtualOrder SoftDeleteObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService)
        {
            return (_validator.ValidDeleteObject(virtualOrder, _virtualOrderDetailService) ? _repository.SoftDeleteObject(virtualOrder) : virtualOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public VirtualOrder ConfirmObject(VirtualOrder virtualOrder, DateTime ConfirmationDate, IVirtualOrderDetailService _virtualOrderDetailService,
                                        IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService,
                                        IWarehouseItemService _warehouseItemService)
        {
            virtualOrder.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(virtualOrder, _virtualOrderDetailService))
            {
                IList<VirtualOrderDetail> virtualOrderDetails = _virtualOrderDetailService.GetObjectsByVirtualOrderId(virtualOrder.Id);
                foreach (var detail in virtualOrderDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _virtualOrderDetailService.ConfirmObject(detail, ConfirmationDate, _stockMutationService, _itemService,
                                                           _blanketService, _warehouseItemService);
                }
                _repository.ConfirmObject(virtualOrder);
            }
            return virtualOrder;
        }

        public VirtualOrder UnconfirmObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService,
                                          ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                          IStockMutationService _stockMutationService, IItemService _itemService,
                                          IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(virtualOrder, _temporaryDeliveryOrderService))
            {
                IList<VirtualOrderDetail> virtualOrderDetails = _virtualOrderDetailService.GetObjectsByVirtualOrderId(virtualOrder.Id);
                foreach (var detail in virtualOrderDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _virtualOrderDetailService.UnconfirmObject(detail, this, _temporaryDeliveryOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                }
                _repository.UnconfirmObject(virtualOrder);
            }
            return virtualOrder;
        }

        public VirtualOrder CheckAndSetDeliveryComplete(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService)
        {
            IList<VirtualOrderDetail> details = _virtualOrderDetailService.GetObjectsByVirtualOrderId(virtualOrder.Id);

            foreach (var detail in details)
            {
                if (!detail.IsAllDelivered)
                {
                    return virtualOrder;
                }
            }
            return _repository.SetDeliveryComplete(virtualOrder);
        }

        public VirtualOrder UnsetDeliveryComplete(VirtualOrder virtualOrder)
        {
            return _repository.UnsetDeliveryComplete(virtualOrder);
        }
    }
}