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
    public class CustomerStockAdjustmentService : ICustomerStockAdjustmentService
    {
        private ICustomerStockAdjustmentRepository _repository;
        private ICustomerStockAdjustmentValidator _validator;

        public CustomerStockAdjustmentService(ICustomerStockAdjustmentRepository _customerStockAdjustmentRepository, ICustomerStockAdjustmentValidator _customerStockAdjustmentValidator)
        {
            _repository = _customerStockAdjustmentRepository;
            _validator = _customerStockAdjustmentValidator;
        }

        public ICustomerStockAdjustmentValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<CustomerStockAdjustment> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CustomerStockAdjustment> GetAll()
        {
            return _repository.GetAll();
        }

        public CustomerStockAdjustment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
        
        public CustomerStockAdjustment CreateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService)
        {
            customerStockAdjustment.Errors = new Dictionary<String, String>();
            return (customerStockAdjustment = _validator.ValidCreateObject(customerStockAdjustment, _warehouseService, _contactService) ? _repository.CreateObject(customerStockAdjustment) : customerStockAdjustment);
        }

        public CustomerStockAdjustment CreateObject(int WarehouseId, int ContactId, DateTime adjustmentDate, IWarehouseService _warehouseService, IContactService _contactService)
        {
            CustomerStockAdjustment customerStockAdjustment = new CustomerStockAdjustment
            {
                WarehouseId = WarehouseId,
                ContactId = ContactId,
                AdjustmentDate = adjustmentDate
            };
            return this.CreateObject(customerStockAdjustment, _warehouseService, _contactService);
        }

        public CustomerStockAdjustment UpdateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService)
        {
            return (customerStockAdjustment = _validator.ValidUpdateObject(customerStockAdjustment, _warehouseService, _contactService) ? _repository.UpdateObject(customerStockAdjustment) : customerStockAdjustment);
        }

        public CustomerStockAdjustment SoftDeleteObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService)
        {
            return (customerStockAdjustment = _validator.ValidDeleteObject(customerStockAdjustment) ? _repository.SoftDeleteObject(customerStockAdjustment) : customerStockAdjustment);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public CustomerStockAdjustment ConfirmObject(CustomerStockAdjustment customerStockAdjustment, DateTime ConfirmationDate, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                             ICustomerStockMutationService _customerStockMutationService, IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService,
                                             IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            customerStockAdjustment.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(customerStockAdjustment, this, _customerStockAdjustmentDetailService, _itemService, _customerItemService, _warehouseItemService, _closingService))
            {
                decimal Total = 0;
                IList<CustomerStockAdjustmentDetail> customerStockAdjustmentDetails = _customerStockAdjustmentDetailService.GetObjectsByCustomerStockAdjustmentId(customerStockAdjustment.Id);
                foreach (var detail in customerStockAdjustmentDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _customerStockAdjustmentDetailService.ConfirmObject(detail, ConfirmationDate, this, _customerStockMutationService, _itemService, _customerItemService, _warehouseItemService);
                    Total += detail.Quantity * detail.Price;
                }
                customerStockAdjustment.Total = Total;
                _repository.ConfirmObject(customerStockAdjustment);
                _generalLedgerJournalService.CreateConfirmationJournalForCustomerStockAdjustment(customerStockAdjustment, _accountService);
            }
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment UnconfirmObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                               ICustomerStockMutationService _customerStockMutationService, IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService,
                                               IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(customerStockAdjustment, this, _customerStockAdjustmentDetailService, _itemService, _customerItemService, _warehouseItemService, _closingService))
            {
                IList<CustomerStockAdjustmentDetail> customerStockAdjustmentDetails = _customerStockAdjustmentDetailService.GetObjectsByCustomerStockAdjustmentId(customerStockAdjustment.Id);
                foreach (var detail in customerStockAdjustmentDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _customerStockAdjustmentDetailService.UnconfirmObject(detail, this, _customerStockMutationService, _itemService, _customerItemService, _warehouseItemService);
                }
                _generalLedgerJournalService.CreateUnconfirmationJournalForCustomerStockAdjustment(customerStockAdjustment, _accountService);
                customerStockAdjustment.Total = 0;
                _repository.UnconfirmObject(customerStockAdjustment);
            }
            return customerStockAdjustment;
        }
    }
}