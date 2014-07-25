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
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private IPurchaseOrderRepository _repository;
        private IPurchaseOrderValidator _validator;

        public PurchaseOrderService(IPurchaseOrderRepository _purchaseOrderRepository, IPurchaseOrderValidator _purchaseOrderValidator)
        {
            _repository = _purchaseOrderRepository;
            _validator = _purchaseOrderValidator;
        }

        public IPurchaseOrderValidator GetValidator()
        {
            return _validator;
        }

        public IList<PurchaseOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public PurchaseOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseOrder> GetObjectsByCustomerId(int customerId)
        {
            return _repository.GetObjectsByCustomerId(customerId);
        }

        public PurchaseOrder CreateObject(PurchaseOrder purchaseOrder, ICustomerService _customerService)
        {
            purchaseOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseOrder, _customerService) ? _repository.CreateObject(purchaseOrder) : purchaseOrder);
        }

        public PurchaseOrder CreateObject(int customerId, DateTime purchaseDate, ICustomerService _customerService)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder
            {
                CustomerId = customerId,
                PurchaseDate = purchaseDate
            };
            return this.CreateObject(purchaseOrder, _customerService);
        }

        public PurchaseOrder UpdateObject(PurchaseOrder purchaseOrder, ICustomerService _customerService)
        {
            return (_validator.ValidUpdateObject(purchaseOrder, _customerService) ? _repository.UpdateObject(purchaseOrder) : purchaseOrder);
        }

        public PurchaseOrder SoftDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            return (_validator.ValidDeleteObject(purchaseOrder, _purchaseOrderDetailService) ? _repository.SoftDeleteObject(purchaseOrder) : purchaseOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseOrder ConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                    IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidConfirmObject(purchaseOrder, _purchaseOrderDetailService))
            {
                _repository.ConfirmObject(purchaseOrder);
            }
            return purchaseOrder;
        }

        public PurchaseOrder UnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                             IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidUnconfirmObject(purchaseOrder, _purchaseOrderDetailService, _purchaseReceivalDetailService, _itemService))
            {
                _repository.UnconfirmObject(purchaseOrder);
            }
            return purchaseOrder;
        }

        public PurchaseOrder CompleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            return (purchaseOrder = _validator.ValidCompleteObject(purchaseOrder, _purchaseOrderDetailService) ? _repository.CompleteObject(purchaseOrder) : purchaseOrder);
        }
    }
}