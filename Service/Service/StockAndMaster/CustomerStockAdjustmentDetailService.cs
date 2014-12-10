using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class CustomerStockAdjustmentDetailService : ICustomerStockAdjustmentDetailService
    {
        private ICustomerStockAdjustmentDetailRepository _repository;
        private ICustomerStockAdjustmentDetailValidator _validator;

        public CustomerStockAdjustmentDetailService(ICustomerStockAdjustmentDetailRepository _customerStockAdjustmentDetailRepository, ICustomerStockAdjustmentDetailValidator _customerStockAdjustmentDetailValidator)
        {
            _repository = _customerStockAdjustmentDetailRepository;
            _validator = _customerStockAdjustmentDetailValidator;
        }

        public ICustomerStockAdjustmentDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<CustomerStockAdjustmentDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CustomerStockAdjustmentDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CustomerStockAdjustmentDetail> GetObjectsByCustomerStockAdjustmentId(int customerStockAdjustmentId)
        {
            return _repository.GetObjectsByCustomerStockAdjustmentId(customerStockAdjustmentId);
        }

        public IList<CustomerStockAdjustmentDetail> GetObjectsByItemId(int itemId)
        {
            return _repository.GetObjectsByItemId(itemId);
        }

        public CustomerStockAdjustmentDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CustomerStockAdjustmentDetail CreateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService,
                                                  IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            customerStockAdjustmentDetail.Errors = new Dictionary<String, String>();
            return (customerStockAdjustmentDetail = _validator.ValidCreateObject(customerStockAdjustmentDetail, this, _customerStockAdjustmentService, _itemService, _warehouseItemService, _customerItemService) ?
                                            _repository.CreateObject(customerStockAdjustmentDetail) : customerStockAdjustmentDetail);
        }

        public CustomerStockAdjustmentDetail CreateObject(int customerStockAdjustmentId, int itemId, int quantity, decimal price,
                                                    ICustomerStockAdjustmentService _customerStockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            CustomerStockAdjustmentDetail customerStockAdjustmentDetail = new CustomerStockAdjustmentDetail
            {
                CustomerStockAdjustmentId = customerStockAdjustmentId,
                ItemId = itemId,
                Quantity = quantity,
                Price = price,
            };
            return this.CreateObject(customerStockAdjustmentDetail, _customerStockAdjustmentService, _itemService, _warehouseItemService, _customerItemService);
        }

        public CustomerStockAdjustmentDetail UpdateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            return (customerStockAdjustmentDetail = _validator.ValidUpdateObject(customerStockAdjustmentDetail, this, _customerStockAdjustmentService, _itemService, _warehouseItemService, _customerItemService) ?
                                            _repository.UpdateObject(customerStockAdjustmentDetail) : customerStockAdjustmentDetail);
        }

        public CustomerStockAdjustmentDetail SoftDeleteObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            return (customerStockAdjustmentDetail = _validator.ValidDeleteObject(customerStockAdjustmentDetail) ? _repository.SoftDeleteObject(customerStockAdjustmentDetail) : customerStockAdjustmentDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public CustomerStockAdjustmentDetail ConfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, DateTime ConfirmationDate, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockMutationService _customerStockMutationService,
                                                   IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService)
        {
            customerStockAdjustmentDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(customerStockAdjustmentDetail, _customerStockAdjustmentService, _itemService, _customerItemService, _warehouseItemService))
            {
                customerStockAdjustmentDetail = _repository.ConfirmObject(customerStockAdjustmentDetail);
                CustomerStockAdjustment customerStockAdjustment = _customerStockAdjustmentService.GetObjectById(customerStockAdjustmentDetail.CustomerStockAdjustmentId);
                Item item = _itemService.GetObjectById(customerStockAdjustmentDetail.ItemId);
                item.CustomerAvgPrice = _itemService.CalculateAndUpdateCustomerAvgPrice(item, customerStockAdjustmentDetail.Quantity, customerStockAdjustmentDetail.Price);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(customerStockAdjustment.WarehouseId, item.Id);
                CustomerItem customerItem = _customerItemService.FindOrCreateObject(customerStockAdjustment.ContactId, warehouseItem.Id);
                CustomerStockMutation customerStockMutation = _customerStockMutationService.CreateCustomerStockMutationForCustomerStockAdjustment(customerStockAdjustmentDetail, customerItem, item.Id);
                _customerStockMutationService.StockMutateObject(customerStockMutation, false, _itemService, _customerItemService, _warehouseItemService);
            }
            return customerStockAdjustmentDetail;
        }
        public CustomerStockAdjustmentDetail UnconfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockMutationService _customerStockMutationService,
                                                     IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(customerStockAdjustmentDetail, _customerStockAdjustmentService, _itemService, _customerItemService, _warehouseItemService))
            {
                customerStockAdjustmentDetail = _repository.UnconfirmObject(customerStockAdjustmentDetail);
                CustomerStockAdjustment customerStockAdjustment = _customerStockAdjustmentService.GetObjectById(customerStockAdjustmentDetail.CustomerStockAdjustmentId);
                Item item = _itemService.GetObjectById(customerStockAdjustmentDetail.ItemId);
                item.CustomerAvgPrice = _itemService.CalculateAndUpdateCustomerAvgPrice(item, customerStockAdjustmentDetail.Quantity * (-1), customerStockAdjustmentDetail.Price);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(customerStockAdjustment.WarehouseId, item.Id);
                CustomerItem customerItem = _customerItemService.FindOrCreateObject(customerStockAdjustment.ContactId, warehouseItem.Id);
                IList<CustomerStockMutation> customerStockMutations = _customerStockMutationService.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.CustomerStockAdjustmentDetail, customerStockAdjustmentDetail.Id);
                foreach (var customerStockMutation in customerStockMutations)
                {
                    _customerStockMutationService.ReverseStockMutateObject(customerStockMutation, false, _itemService, _customerItemService, _warehouseItemService);
                }
                _customerStockMutationService.DeleteCustomerStockMutations(customerStockMutations);
            }
            return customerStockAdjustmentDetail;
        }
    }
}