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
    public class SalesOrderService : ISalesOrderService
    {
        private ISalesOrderRepository _repository;
        private ISalesOrderValidator _validator;

        public SalesOrderService(ISalesOrderRepository _salesOrderRepository, ISalesOrderValidator _salesOrderValidator)
        {
            _repository = _salesOrderRepository;
            _validator = _salesOrderValidator;
        }

        public ISalesOrderValidator GetValidator()
        {
            return _validator;
        }

        public IList<SalesOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public SalesOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<SalesOrder> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }
        
        public SalesOrder CreateObject(SalesOrder salesOrder, IContactService _contactService)
        {
            salesOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salesOrder, _contactService) ? _repository.CreateObject(salesOrder) : salesOrder);
        }

        public SalesOrder CreateObject(int contactId, DateTime salesDate, IContactService _contactService)
        {
            SalesOrder so = new SalesOrder
            {
                ContactId = contactId,
                SalesDate = salesDate
            };
            return this.CreateObject(so, _contactService);
        }

        public SalesOrder UpdateObject(SalesOrder salesOrder, IContactService _contactService)
        {
            return (_validator.ValidUpdateObject(salesOrder, _contactService) ? _repository.UpdateObject(salesOrder) : salesOrder);
        }

        public SalesOrder SoftDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            return (_validator.ValidDeleteObject(salesOrder, _salesOrderDetailService) ? _repository.SoftDeleteObject(salesOrder) : salesOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesOrder ConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService,
                                        IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidConfirmObject(salesOrder, _salesOrderDetailService))
            {
                _repository.ConfirmObject(salesOrder);
            }
            return salesOrder;
        }

        public SalesOrder UnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService,
                                    IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidUnconfirmObject(salesOrder, _salesOrderDetailService, _deliveryOrderDetailService, _itemService))
            {
                _repository.UnconfirmObject(salesOrder);
            }
            return salesOrder;
        }

        public SalesOrder CompleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            if (_validator.ValidCompleteObject(salesOrder, _salesOrderDetailService))
            {
                _repository.CompleteObject(salesOrder);
            }
            return salesOrder;
        }
    }
}