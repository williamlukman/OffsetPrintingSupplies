using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;

namespace Service.Service
{
    public class CustomerStockMutationService : ICustomerStockMutationService
    {
        private ICustomerStockMutationRepository _repository;
        private ICustomerStockMutationValidator _validator;

        public CustomerStockMutationService(ICustomerStockMutationRepository _customerStockMutationRepository, ICustomerStockMutationValidator _customerStockMutationValidator)
        {
            _repository = _customerStockMutationRepository;
            _validator = _customerStockMutationValidator;
        }

        public ICustomerStockMutationValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<CustomerStockMutation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CustomerStockMutation> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CustomerStockMutation> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public IList<CustomerStockMutation> GetObjectsByCustomerItemId(int customerItemId)
        {
            return _repository.GetObjectsByCustomerItemId(customerItemId);
        }

        public CustomerStockMutation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<CustomerStockMutation> GetObjectsBySourceDocumentDetailForCustomerItem(int customerItemId, string SourceDocumentDetailType, int SourceDocumentDetailId)
        {
            return _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItemId, SourceDocumentDetailType, SourceDocumentDetailId);
        }

        //public IList<CustomerStockMutation> GetObjectsBySourceDocumentDetailForItem(int itemId, string SourceDocumentDetailType, int SourceDocumentDetailId)
        //{
        //    return _repository.GetObjectsBySourceDocumentDetailForItem(itemId, SourceDocumentDetailType, SourceDocumentDetailId);
        //}

        public CustomerStockMutation CreateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService)
        {
            customerStockMutation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(customerStockMutation, _customerItemService, _itemService) ? _repository.CreateObject(customerStockMutation) : customerStockMutation);
        }

        public CustomerStockMutation UpdateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService)
        {
            return (_validator.ValidUpdateObject(customerStockMutation, _customerItemService, _itemService) ? _repository.UpdateObject(customerStockMutation) : customerStockMutation);
        }

        public CustomerStockMutation SoftDeleteObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService)
        {
            return (_validator.ValidDeleteObject(customerStockMutation, _customerItemService) ? _repository.SoftDeleteObject(customerStockMutation) : customerStockMutation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public int DeleteCustomerStockMutations(IList<CustomerStockMutation> customerStockMutations)
        {
            int count = 0;
            foreach (var customerStockMutation in customerStockMutations)
            {
                count += _repository.Delete(customerStockMutation);
            }
            return count;
        }

        //public CustomerStockMutation CreateCustomerStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item)
        //{
        //    CustomerStockMutation customerStockMutation = new CustomerStockMutation();
        //    customerStockMutation.ItemId = item.Id;
        //    customerStockMutation.Quantity = purchaseOrderDetail.Quantity;
        //    customerStockMutation.SourceDocumentType = Constant.SourceDocumentType.PurchaseOrder;
        //    customerStockMutation.SourceDocumentId = purchaseOrderDetail.PurchaseOrderId;
        //    customerStockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.PurchaseOrderDetail;
        //    customerStockMutation.SourceDocumentDetailId = purchaseOrderDetail.Id;
        //    customerStockMutation.ItemCase = Constant.ItemCase.PendingReceival;
        //    customerStockMutation.Status = Constant.MutationStatus.Addition;
        //    customerStockMutation.MutationDate = (DateTime) purchaseOrderDetail.ConfirmationDate;
        //    return _repository.CreateObject(customerStockMutation);
        //}
        
        //public IList<CustomerStockMutation> DeleteCustomerStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item)
        //{
        //    IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForItem(item.Id, Constant.SourceDocumentDetailType.PurchaseOrderDetail, purchaseOrderDetail.Id);
        //    foreach (var customerStockMutation in customerStockMutations)
        //    {
        //        _repository.Delete(customerStockMutation);
        //    }
        //    return customerStockMutations;
        //}

        //public IList<CustomerStockMutation> CreateCustomerStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, CustomerItem customerItem)
        //{
        //    IList<CustomerStockMutation> result = new List<CustomerStockMutation>();
            
        //    CustomerStockMutation customerStockMutationPendingReceival = new CustomerStockMutation();
        //    customerStockMutationPendingReceival.ItemId = customerItem.ItemId;
        //    customerStockMutationPendingReceival.StockMutationId = customerItem.StockMutationId;
        //    customerStockMutationPendingReceival.CustomerItemId = customerItem.Id;
        //    customerStockMutationPendingReceival.Quantity = purchaseReceivalDetail.Quantity;
        //    customerStockMutationPendingReceival.SourceDocumentType = Constant.SourceDocumentType.PurchaseReceival;
        //    customerStockMutationPendingReceival.SourceDocumentId = purchaseReceivalDetail.PurchaseReceivalId;
        //    customerStockMutationPendingReceival.SourceDocumentDetailType = Constant.SourceDocumentDetailType.PurchaseReceivalDetail;
        //    customerStockMutationPendingReceival.SourceDocumentDetailId = purchaseReceivalDetail.Id;
        //    customerStockMutationPendingReceival.ItemCase = Constant.ItemCase.PendingReceival;
        //    customerStockMutationPendingReceival.Status = Constant.MutationStatus.Deduction;
        //    customerStockMutationPendingReceival.MutationDate = (DateTime) purchaseReceivalDetail.ConfirmationDate;
        //    customerStockMutationPendingReceival = _repository.CreateObject(customerStockMutationPendingReceival);

        //    CustomerStockMutation customerStockMutationReady = new CustomerStockMutation();
        //    customerStockMutationReady.ItemId = customerItem.ItemId;
        //    customerStockMutationReady.StockMutationId = customerItem.StockMutationId;
        //    customerStockMutationReady.CustomerItemId = customerItem.Id;
        //    customerStockMutationReady.Quantity = purchaseReceivalDetail.Quantity;
        //    customerStockMutationReady.SourceDocumentType = Constant.SourceDocumentType.PurchaseReceival;
        //    customerStockMutationReady.SourceDocumentId = purchaseReceivalDetail.PurchaseReceivalId;
        //    customerStockMutationReady.SourceDocumentDetailType = Constant.SourceDocumentDetailType.PurchaseReceivalDetail;
        //    customerStockMutationReady.SourceDocumentDetailId = purchaseReceivalDetail.Id;
        //    customerStockMutationReady.ItemCase = Constant.ItemCase.Ready;
        //    customerStockMutationReady.Status = Constant.MutationStatus.Addition;
        //    customerStockMutationReady.MutationDate = (DateTime) purchaseReceivalDetail.ConfirmationDate;
        //    customerStockMutationReady = _repository.CreateObject(customerStockMutationReady);

        //    result.Add(customerStockMutationPendingReceival);
        //    result.Add(customerStockMutationReady);
        //    return result;
        //}

        //public IList<CustomerStockMutation> DeleteCustomerStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, CustomerItem customerItem)
        //{
        //    IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.PurchaseReceivalDetail, purchaseReceivalDetail.Id);
        //    foreach (var customerStockMutation in customerStockMutations)
        //    {
        //        _repository.Delete(customerStockMutation);
        //    }
        //    return customerStockMutations;
        //}

        //public CustomerStockMutation CreateCustomerStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item)
        //{
        //    CustomerStockMutation customerStockMutation = new CustomerStockMutation();
        //    customerStockMutation.ItemId = item.Id;
        //    customerStockMutation.Quantity = salesOrderDetail.Quantity;
        //    customerStockMutation.SourceDocumentType = Constant.SourceDocumentType.SalesOrder;
        //    customerStockMutation.SourceDocumentId = salesOrderDetail.SalesOrderId;
        //    customerStockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.SalesOrderDetail;
        //    customerStockMutation.SourceDocumentDetailId = salesOrderDetail.Id;
        //    customerStockMutation.ItemCase = Constant.ItemCase.PendingDelivery;
        //    customerStockMutation.Status = Constant.MutationStatus.Addition;
        //    customerStockMutation.MutationDate = (DateTime) salesOrderDetail.ConfirmationDate;
        //    return _repository.CreateObject(customerStockMutation);
        //}

        //public IList<CustomerStockMutation> DeleteCustomerStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item)
        //{
        //    IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForItem(item.Id, Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail.Id);
        //    foreach (var customerStockMutation in customerStockMutations)
        //    {
        //        _repository.Delete(customerStockMutation);
        //    }
        //    return customerStockMutations;
        //}

        public IList<CustomerStockMutation> CreateCustomerStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, CustomerItem customerItem)
        {
            IList<CustomerStockMutation> result = new List<CustomerStockMutation>();

            CustomerStockMutation customerStockMutationPendingDelivery = new CustomerStockMutation();
            customerStockMutationPendingDelivery.ContactId = customerItem.ContactId;
            customerStockMutationPendingDelivery.CustomerItemId = customerItem.Id;
            customerStockMutationPendingDelivery.WarehouseItemId = customerItem.WarehouseItemId;
            customerStockMutationPendingDelivery.ItemId = deliveryOrderDetail.ItemId;
            customerStockMutationPendingDelivery.Quantity = deliveryOrderDetail.Quantity;
            customerStockMutationPendingDelivery.SourceDocumentType = Constant.SourceDocumentType.DeliveryOrder;
            customerStockMutationPendingDelivery.SourceDocumentId = deliveryOrderDetail.DeliveryOrderId;
            customerStockMutationPendingDelivery.SourceDocumentDetailType = Constant.SourceDocumentDetailType.DeliveryOrderDetail;
            customerStockMutationPendingDelivery.SourceDocumentDetailId = deliveryOrderDetail.Id;
            customerStockMutationPendingDelivery.ItemCase = Constant.ItemCase.PendingDelivery;
            customerStockMutationPendingDelivery.Status = Constant.MutationStatus.Deduction;
            customerStockMutationPendingDelivery.MutationDate = (DateTime)deliveryOrderDetail.ConfirmationDate;
            customerStockMutationPendingDelivery = _repository.CreateObject(customerStockMutationPendingDelivery);

            CustomerStockMutation customerStockMutationReady = new CustomerStockMutation();
            customerStockMutationReady.ContactId = customerItem.ContactId;
            customerStockMutationReady.CustomerItemId = customerItem.Id;
            customerStockMutationReady.WarehouseItemId = customerItem.WarehouseItemId;
            customerStockMutationReady.ItemId = deliveryOrderDetail.ItemId;
            customerStockMutationReady.Quantity = deliveryOrderDetail.Quantity;
            customerStockMutationReady.SourceDocumentType = Constant.SourceDocumentType.DeliveryOrder;
            customerStockMutationReady.SourceDocumentId = deliveryOrderDetail.DeliveryOrderId;
            customerStockMutationReady.SourceDocumentDetailType = Constant.SourceDocumentDetailType.DeliveryOrderDetail;
            customerStockMutationReady.SourceDocumentDetailId = deliveryOrderDetail.Id;
            customerStockMutationReady.ItemCase = Constant.ItemCase.Ready;
            customerStockMutationReady.Status = Constant.MutationStatus.Deduction;
            customerStockMutationReady.MutationDate = (DateTime)deliveryOrderDetail.ConfirmationDate;
            customerStockMutationReady = _repository.CreateObject(customerStockMutationReady);

            result.Add(customerStockMutationPendingDelivery);
            result.Add(customerStockMutationReady);
            return result;
        }

        public IList<CustomerStockMutation> DeleteCustomerStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, CustomerItem customerItem)
        {
            IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.DeliveryOrderDetail, deliveryOrderDetail.Id);
            foreach (var customerStockMutation in customerStockMutations)
            {
                _repository.Delete(customerStockMutation);
            }
            return customerStockMutations;
        }

        //public CustomerStockMutation CreateCustomerStockMutationForVirtualOrder(VirtualOrderDetail virtualOrderDetail, Item item)
        //{
        //    CustomerStockMutation customerStockMutation = new CustomerStockMutation();
        //    customerStockMutation.ItemId = item.Id;
        //    customerStockMutation.Quantity = virtualOrderDetail.Quantity;
        //    customerStockMutation.SourceDocumentType = Constant.SourceDocumentType.VirtualOrder;
        //    customerStockMutation.SourceDocumentId = virtualOrderDetail.VirtualOrderId;
        //    customerStockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.VirtualOrderDetail;
        //    customerStockMutation.SourceDocumentDetailId = virtualOrderDetail.Id;
        //    customerStockMutation.ItemCase = Constant.ItemCase.PendingDelivery;
        //    customerStockMutation.Status = Constant.MutationStatus.Addition;
        //    customerStockMutation.MutationDate = (DateTime) virtualOrderDetail.ConfirmationDate;
        //    return _repository.CreateObject(customerStockMutation);
        //}

        //public IList<CustomerStockMutation> DeleteCustomerStockMutationForVirtualOrder(VirtualOrderDetail virtualOrderDetail, Item item)
        //{
        //    IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForItem(item.Id, Constant.SourceDocumentDetailType.VirtualOrderDetail, virtualOrderDetail.Id);
        //    foreach (var customerStockMutation in customerStockMutations)
        //    {
        //        _repository.Delete(customerStockMutation);
        //    }
        //    return customerStockMutations;
        //}

        //public IList<CustomerStockMutation> CreateCustomerStockMutationForTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, CustomerItem customerItem)
        //{
        //    IList<CustomerStockMutation> result = new List<CustomerStockMutation>();

        //    CustomerStockMutation customerStockMutationVirtual = new CustomerStockMutation();
        //    customerStockMutationVirtual.ItemId = customerItem.ItemId;
        //    customerStockMutationVirtual.StockMutationId = customerItem.StockMutationId;
        //    customerStockMutationVirtual.CustomerItemId = customerItem.Id;
        //    customerStockMutationVirtual.ItemId = temporaryDeliveryOrderDetail.ItemId;
        //    customerStockMutationVirtual.Quantity = temporaryDeliveryOrderDetail.Quantity;
        //    customerStockMutationVirtual.SourceDocumentType = Constant.SourceDocumentType.TemporaryDeliveryOrder;
        //    customerStockMutationVirtual.SourceDocumentId = temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId;
        //    customerStockMutationVirtual.SourceDocumentDetailType = Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetail;
        //    customerStockMutationVirtual.SourceDocumentDetailId = temporaryDeliveryOrderDetail.Id;
        //    customerStockMutationVirtual.ItemCase = Constant.ItemCase.Virtual;
        //    customerStockMutationVirtual.Status = Constant.MutationStatus.Addition;
        //    customerStockMutationVirtual.MutationDate = (DateTime) temporaryDeliveryOrderDetail.ConfirmationDate;
        //    customerStockMutationVirtual = _repository.CreateObject(customerStockMutationVirtual);

        //    result.Add(customerStockMutationVirtual);
        //    return result;
        //}

        //public IList<CustomerStockMutation> DeleteCustomerStockMutationForTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, CustomerItem customerItem)
        //{
        //    IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetail, temporaryDeliveryOrderDetail.Id);
        //    foreach (var customerStockMutation in customerStockMutations)
        //    {
        //        _repository.Delete(customerStockMutation);
        //    }
        //    return customerStockMutations;
        //}

        //public IList<CustomerStockMutation> CreateCustomerStockMutationForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, DateTime PushDate, CustomerItem customerItem)
        //{
        //    IList<CustomerStockMutation> result = new List<CustomerStockMutation>();

        //    CustomerStockMutation customerStockMutationVirtual = new CustomerStockMutation();
        //    customerStockMutationVirtual.ItemId = customerItem.ItemId;
        //    customerStockMutationVirtual.StockMutationId = customerItem.StockMutationId;
        //    customerStockMutationVirtual.CustomerItemId = customerItem.Id;
        //    customerStockMutationVirtual.ItemId = temporaryDeliveryOrderDetail.ItemId;
        //    customerStockMutationVirtual.Quantity = temporaryDeliveryOrderDetail.WasteQuantity;
        //    customerStockMutationVirtual.SourceDocumentType = Constant.SourceDocumentType.TemporaryDeliveryOrder;
        //    customerStockMutationVirtual.SourceDocumentId = temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId;
        //    customerStockMutationVirtual.SourceDocumentDetailType = Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetailWaste;
        //    customerStockMutationVirtual.SourceDocumentDetailId = temporaryDeliveryOrderDetail.Id;
        //    customerStockMutationVirtual.ItemCase = Constant.ItemCase.Virtual;
        //    customerStockMutationVirtual.Status = Constant.MutationStatus.Deduction;
        //    customerStockMutationVirtual.MutationDate = PushDate;
        //    customerStockMutationVirtual = _repository.CreateObject(customerStockMutationVirtual);

        //    CustomerStockMutation customerStockMutationReady = new CustomerStockMutation();
        //    customerStockMutationReady.ItemId = customerItem.ItemId;
        //    customerStockMutationReady.StockMutationId = customerItem.StockMutationId;
        //    customerStockMutationReady.CustomerItemId = customerItem.Id;
        //    customerStockMutationReady.ItemId = temporaryDeliveryOrderDetail.ItemId;
        //    customerStockMutationReady.Quantity = temporaryDeliveryOrderDetail.WasteQuantity;
        //    customerStockMutationReady.SourceDocumentType = Constant.SourceDocumentType.TemporaryDeliveryOrder;
        //    customerStockMutationReady.SourceDocumentId = temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId;
        //    customerStockMutationReady.SourceDocumentDetailType = Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetailWaste;
        //    customerStockMutationReady.SourceDocumentDetailId = temporaryDeliveryOrderDetail.Id;
        //    customerStockMutationReady.ItemCase = Constant.ItemCase.Ready;
        //    customerStockMutationReady.Status = Constant.MutationStatus.Deduction;
        //    customerStockMutationReady.MutationDate = PushDate;
        //    customerStockMutationReady = _repository.CreateObject(customerStockMutationReady);

        //    result.Add(customerStockMutationVirtual);
        //    result.Add(customerStockMutationReady);
        //    return result;
        //}

        //public IList<CustomerStockMutation> DeleteCustomerStockMutationForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, CustomerItem customerItem)
        //{
        //    IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetailWaste, temporaryDeliveryOrderDetail.Id);
        //    foreach (var customerStockMutation in customerStockMutations)
        //    {
        //        _repository.Delete(customerStockMutation);
        //    }
        //    return customerStockMutations;
        //}

        //public IList<CustomerStockMutation> CreateCustomerStockMutationForTemporaryDeliveryOrderRestock(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, DateTime PushDate, CustomerItem customerItem)
        //{
        //    IList<CustomerStockMutation> result = new List<CustomerStockMutation>();

        //    CustomerStockMutation customerStockMutationVirtual = new CustomerStockMutation();
        //    customerStockMutationVirtual.ItemId = customerItem.ItemId;
        //    customerStockMutationVirtual.StockMutationId = customerItem.StockMutationId;
        //    customerStockMutationVirtual.CustomerItemId = customerItem.Id;
        //    customerStockMutationVirtual.ItemId = temporaryDeliveryOrderDetail.ItemId;
        //    customerStockMutationVirtual.Quantity = temporaryDeliveryOrderDetail.RestockQuantity;
        //    customerStockMutationVirtual.SourceDocumentType = Constant.SourceDocumentType.TemporaryDeliveryOrder;
        //    customerStockMutationVirtual.SourceDocumentId = temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId;
        //    customerStockMutationVirtual.SourceDocumentDetailType = Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetailRestock;
        //    customerStockMutationVirtual.SourceDocumentDetailId = temporaryDeliveryOrderDetail.Id;
        //    customerStockMutationVirtual.ItemCase = Constant.ItemCase.Virtual;
        //    customerStockMutationVirtual.Status = Constant.MutationStatus.Deduction;
        //    customerStockMutationVirtual.MutationDate = PushDate;
        //    customerStockMutationVirtual = _repository.CreateObject(customerStockMutationVirtual);

        //    result.Add(customerStockMutationVirtual);
        //    return result;
        //}

        //public IList<CustomerStockMutation> DeleteCustomerStockMutationForTemporaryDeliveryOrderRestock(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, CustomerItem customerItem)
        //{
        //    IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetailRestock, temporaryDeliveryOrderDetail.Id);
        //    foreach (var customerStockMutation in customerStockMutations)
        //    {
        //        _repository.Delete(customerStockMutation);
        //    }
        //    return customerStockMutations;
        //}

        public CustomerStockMutation CreateCustomerStockMutationForCustomerStockAdjustment(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, CustomerItem customerItem, int ItemId)
        {
            CustomerStockMutation customerStockMutation = new CustomerStockMutation();
            customerStockMutation.ItemId = ItemId;
            customerStockMutation.ContactId = customerItem.ContactId;
            customerStockMutation.CustomerItemId = customerItem.Id;
            customerStockMutation.WarehouseItemId = customerItem.WarehouseItemId;
            customerStockMutation.Quantity = (customerStockAdjustmentDetail.Quantity >= 0) ? customerStockAdjustmentDetail.Quantity : (-1) * customerStockAdjustmentDetail.Quantity;
            customerStockMutation.SourceDocumentType = Constant.SourceDocumentType.CustomerStockAdjustment;
            customerStockMutation.SourceDocumentId = customerStockAdjustmentDetail.CustomerStockAdjustmentId;
            customerStockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.CustomerStockAdjustmentDetail;
            customerStockMutation.SourceDocumentDetailId = customerStockAdjustmentDetail.Id;
            customerStockMutation.ItemCase = Constant.ItemCase.Ready;
            customerStockMutation.Status = (customerStockAdjustmentDetail.Quantity >= 0) ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
            customerStockMutation.MutationDate = (DateTime)customerStockAdjustmentDetail.ConfirmationDate;
            return _repository.CreateObject(customerStockMutation);
        }

        public IList<CustomerStockMutation> DeleteCustomerStockMutationForCustomerStockAdjustment(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, CustomerItem customerItem)
        {
            IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.CustomerStockAdjustmentDetail, customerStockAdjustmentDetail.Id);
            foreach (var customerStockMutation in customerStockMutations)
            {
                _repository.Delete(customerStockMutation);
            }
            return customerStockMutations;
        }

        public CustomerStockMutation CreateCustomerStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, CustomerItem customerItem, int ItemId)
        {
            CustomerStockMutation customerStockMutation = new CustomerStockMutation();
            customerStockMutation.ItemId = ItemId; // customerItem.WarehouseItem.ItemId;
            customerStockMutation.ContactId = customerItem.ContactId;
            customerStockMutation.CustomerItemId = customerItem.Id;
            customerStockMutation.WarehouseItemId = customerItem.WarehouseItemId;
            customerStockMutation.Quantity = 1;
            customerStockMutation.SourceDocumentType = Constant.SourceDocumentType.CoreIdentification;
            customerStockMutation.SourceDocumentId = coreIdentificationDetail.CoreIdentificationId;
            customerStockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.CoreIdentificationDetail;
            customerStockMutation.SourceDocumentDetailId = coreIdentificationDetail.Id;
            customerStockMutation.ItemCase = Constant.ItemCase.Ready;
            customerStockMutation.Status = Constant.MutationStatus.Addition;
            customerStockMutation.MutationDate = (DateTime)coreIdentificationDetail.ConfirmationDate;
            return _repository.CreateObject(customerStockMutation);
        }

        public IList<CustomerStockMutation> DeleteCustomerStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, CustomerItem customerItem)
        {
            IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.CoreIdentificationDetail, coreIdentificationDetail.Id);
            foreach (var customerStockMutation in customerStockMutations)
            {
                _repository.Delete(customerStockMutation);
            }
            return customerStockMutations;
        }

        public CustomerStockMutation CreateCustomerStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedOrRejectedDate, CustomerItem customerItem, int ItemId, bool CaseAddition)
        {
            CustomerStockMutation customerStockMutation = new CustomerStockMutation();
            customerStockMutation.ItemId = ItemId; // customerItem.WarehouseItem.ItemId;
            customerStockMutation.ContactId = customerItem.ContactId;
            customerStockMutation.CustomerItemId = customerItem.Id;
            customerStockMutation.WarehouseItemId = customerItem.WarehouseItemId;
            customerStockMutation.Quantity = 1;
            customerStockMutation.SourceDocumentType = Constant.SourceDocumentType.RecoveryOrder;
            customerStockMutation.SourceDocumentId = recoveryOrderDetail.RecoveryOrderId;
            customerStockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RecoveryOrderDetail;
            customerStockMutation.SourceDocumentDetailId = recoveryOrderDetail.Id;
            customerStockMutation.ItemCase = Constant.ItemCase.Ready;
            customerStockMutation.Status = CaseAddition ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
            customerStockMutation.MutationDate = FinishedOrRejectedDate;
            return _repository.CreateObject(customerStockMutation);
        }

        //public CustomerStockMutation CreateCustomerStockMutationForRecoveryOrderCompound(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedOrRejectedDate, CustomerItem customerItem, bool CaseAddition)
        //{
        //    CustomerStockMutation customerStockMutation = new CustomerStockMutation();
        //    customerStockMutation.ItemId = customerItem.ItemId;
        //    customerStockMutation.StockMutationId = customerItem.StockMutationId;
        //    customerStockMutation.CustomerItemId = customerItem.Id;
        //    customerStockMutation.Quantity = recoveryOrderDetail.CompoundUsage;
        //    customerStockMutation.SourceDocumentType = Constant.SourceDocumentType.RecoveryOrder;
        //    customerStockMutation.SourceDocumentId = recoveryOrderDetail.RecoveryOrderId;
        //    customerStockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RecoveryOrderDetail;
        //    customerStockMutation.SourceDocumentDetailId = recoveryOrderDetail.Id;
        //    customerStockMutation.ItemCase = Constant.ItemCase.Ready;
        //    customerStockMutation.Status = CaseAddition ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
        //    customerStockMutation.MutationDate = FinishedOrRejectedDate;
        //    return _repository.CreateObject(customerStockMutation);
        //}

        public IList<CustomerStockMutation> DeleteCustomerStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, CustomerItem customerItem)
        {
            IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.RecoveryOrderDetail, recoveryOrderDetail.Id);
            foreach (var customerStockMutation in customerStockMutations)
            {
                _repository.Delete(customerStockMutation);
            }
            return customerStockMutations;
        }

        //public CustomerStockMutation CreateCustomerStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, DateTime FinishedDate, CustomerItem customerItem)
        //{
        //    CustomerStockMutation customerStockMutation = new CustomerStockMutation();
        //    customerStockMutation.ItemId = customerItem.ItemId;
        //    customerStockMutation.StockMutationId = customerItem.StockMutationId;
        //    customerStockMutation.CustomerItemId = customerItem.Id;
        //    customerStockMutation.Quantity = recoveryAccessoryDetail.Quantity;
        //    customerStockMutation.SourceDocumentType = Constant.SourceDocumentType.RecoveryOrderDetail;
        //    customerStockMutation.SourceDocumentId = recoveryAccessoryDetail.RecoveryOrderDetailId;
        //    customerStockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RecoveryAccessoryDetail;
        //    customerStockMutation.SourceDocumentDetailId = recoveryAccessoryDetail.Id;
        //    customerStockMutation.ItemCase = Constant.ItemCase.Ready;
        //    customerStockMutation.Status = Constant.MutationStatus.Deduction;
        //    customerStockMutation.MutationDate = FinishedDate;
        //    return _repository.CreateObject(customerStockMutation);
        //}

        //public IList<CustomerStockMutation> DeleteCustomerStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, CustomerItem customerItem)
        //{
        //    IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.RecoveryAccessoryDetail, recoveryAccessoryDetail.Id);
        //    foreach (var customerStockMutation in customerStockMutations)
        //    {
        //        _repository.Delete(customerStockMutation);
        //    }
        //    return customerStockMutations;
        //}

        //public CustomerStockMutation CreateCustomerStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, DateTime FinishedOrRejectedDate, CustomerItem customerItem, bool CaseAddition)
        //{
        //    CustomerStockMutation customerStockMutation = new CustomerStockMutation();
        //    customerStockMutation.ItemId = customerItem.ItemId;
        //    customerStockMutation.StockMutationId = customerItem.StockMutationId;
        //    customerStockMutation.CustomerItemId = customerItem.Id;
        //    customerStockMutation.Quantity = 1;
        //    customerStockMutation.SourceDocumentType = Constant.SourceDocumentType.BlanketOrder;
        //    customerStockMutation.SourceDocumentId = blanketOrderDetail.BlanketOrderId;
        //    customerStockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.BlanketOrderDetail;
        //    customerStockMutation.SourceDocumentDetailId = blanketOrderDetail.Id;
        //    customerStockMutation.ItemCase = Constant.ItemCase.Ready;
        //    customerStockMutation.Status = CaseAddition ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
        //    customerStockMutation.MutationDate = FinishedOrRejectedDate;
        //    return _repository.CreateObject(customerStockMutation);
        //}

        //public IList<CustomerStockMutation> DeleteCustomerStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, CustomerItem customerItem)
        //{
        //    IList<CustomerStockMutation> customerStockMutations = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.BlanketOrderDetail, blanketOrderDetail.Id);
        //    foreach (var customerStockMutation in customerStockMutations)
        //    {
        //        _repository.Delete(customerStockMutation);
        //    }
        //    return customerStockMutations;
        //}

        public IList<CustomerStockMutation> CreateCustomerStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, CustomerItem customerItemFrom, CustomerItem customerItemTo, int ItemId)
        {
            IList<CustomerStockMutation> customerStockMutations = new List<CustomerStockMutation>();

            CustomerStockMutation customerStockMutationFrom = new CustomerStockMutation();
            customerStockMutationFrom.ItemId = ItemId;
            customerStockMutationFrom.ContactId = customerItemFrom.ContactId;
            customerStockMutationFrom.CustomerItemId = customerItemFrom.Id;
            customerStockMutationFrom.WarehouseItemId = customerItemFrom.WarehouseItemId;
            customerStockMutationFrom.Quantity = 1;
            customerStockMutationFrom.SourceDocumentType = Constant.SourceDocumentType.RollerWarehouseMutation;
            customerStockMutationFrom.SourceDocumentId = rollerWarehouseMutationDetail.RollerWarehouseMutationId;
            customerStockMutationFrom.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail;
            customerStockMutationFrom.SourceDocumentDetailId = rollerWarehouseMutationDetail.Id;
            customerStockMutationFrom.ItemCase = Constant.ItemCase.Ready;
            customerStockMutationFrom.Status = Constant.MutationStatus.Deduction;
            customerStockMutationFrom.MutationDate = (DateTime)rollerWarehouseMutationDetail.ConfirmationDate;
            customerStockMutationFrom = _repository.CreateObject(customerStockMutationFrom);

            CustomerStockMutation customerStockMutationTo = new CustomerStockMutation();
            customerStockMutationTo.ItemId = ItemId;
            customerStockMutationTo.ContactId = customerItemTo.ContactId;
            customerStockMutationTo.CustomerItemId = customerItemTo.Id;
            customerStockMutationTo.WarehouseItemId = customerItemTo.WarehouseItemId;
            customerStockMutationTo.Quantity = 1;
            customerStockMutationTo.SourceDocumentType = Constant.SourceDocumentType.RollerWarehouseMutation;
            customerStockMutationTo.SourceDocumentId = rollerWarehouseMutationDetail.RollerWarehouseMutationId;
            customerStockMutationTo.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail;
            customerStockMutationTo.SourceDocumentDetailId = rollerWarehouseMutationDetail.Id;
            customerStockMutationTo.ItemCase = Constant.ItemCase.Ready;
            customerStockMutationTo.Status = Constant.MutationStatus.Addition;
            customerStockMutationTo.MutationDate = (DateTime)rollerWarehouseMutationDetail.ConfirmationDate;
            customerStockMutationTo = _repository.CreateObject(customerStockMutationTo);

            customerStockMutations.Add(customerStockMutationFrom);
            customerStockMutations.Add(customerStockMutationTo);
            return customerStockMutations;
        }

        public IList<CustomerStockMutation> DeleteCustomerStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, CustomerItem customerItemFrom, CustomerItem customerItemTo)
        {
            IList<CustomerStockMutation> customerStockMutations = new List<CustomerStockMutation>();

            IList<CustomerStockMutation> customerStockMutationFrom = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItemFrom.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
            customerStockMutationFrom.ToList().ForEach(x => customerStockMutations.Add(x));
            foreach (var customerStockMutation in customerStockMutationFrom)
            {
                _repository.Delete(customerStockMutation);
            }

            IList<CustomerStockMutation> customerStockMutationTo = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItemTo.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
            customerStockMutationTo.ToList().ForEach(x => customerStockMutations.Add(x));
            foreach (var customerStockMutation in customerStockMutationTo)
            {
                _repository.Delete(customerStockMutation);
            }
            return customerStockMutations;
        }

        //public IList<CustomerStockMutation> CreateCustomerStockMutationForStockMutationMutation(StockMutationMutationDetail StockMutationMutationDetail, CustomerItem customerItemFrom, CustomerItem customerItemTo)
        //{
        //    IList<CustomerStockMutation> customerStockMutations = new List<CustomerStockMutation>();

        //    CustomerStockMutation customerStockMutationFrom = new CustomerStockMutation();
        //    customerStockMutationFrom.ItemId = customerItemFrom.ItemId;
        //    customerStockMutationFrom.StockMutationId = customerItemFrom.StockMutationId;
        //    customerStockMutationFrom.CustomerItemId = customerItemFrom.Id;
        //    customerStockMutationFrom.Quantity = StockMutationMutationDetail.Quantity;
        //    customerStockMutationFrom.SourceDocumentType = Constant.SourceDocumentType.StockMutationMutation;
        //    customerStockMutationFrom.SourceDocumentId = StockMutationMutationDetail.StockMutationMutationId;
        //    customerStockMutationFrom.SourceDocumentDetailType = Constant.SourceDocumentDetailType.StockMutationMutationDetail;
        //    customerStockMutationFrom.SourceDocumentDetailId = StockMutationMutationDetail.Id;
        //    customerStockMutationFrom.ItemCase = Constant.ItemCase.Ready;
        //    customerStockMutationFrom.Status = Constant.MutationStatus.Deduction;
        //    customerStockMutationFrom.MutationDate = (DateTime)StockMutationMutationDetail.ConfirmationDate;
        //    customerStockMutationFrom = _repository.CreateObject(customerStockMutationFrom);

        //    CustomerStockMutation customerStockMutationTo = new CustomerStockMutation();
        //    customerStockMutationTo.ItemId = customerItemTo.ItemId;
        //    customerStockMutationTo.StockMutationId = customerItemTo.StockMutationId;
        //    customerStockMutationTo.CustomerItemId = customerItemTo.Id;
        //    customerStockMutationTo.Quantity = StockMutationMutationDetail.Quantity;
        //    customerStockMutationTo.SourceDocumentType = Constant.SourceDocumentType.StockMutationMutation;
        //    customerStockMutationTo.SourceDocumentId = StockMutationMutationDetail.StockMutationMutationId;
        //    customerStockMutationTo.SourceDocumentDetailType = Constant.SourceDocumentDetailType.StockMutationMutationDetail;
        //    customerStockMutationTo.SourceDocumentDetailId = StockMutationMutationDetail.Id;
        //    customerStockMutationTo.ItemCase = Constant.ItemCase.Ready;
        //    customerStockMutationTo.Status = Constant.MutationStatus.Addition;
        //    customerStockMutationTo.MutationDate = (DateTime)StockMutationMutationDetail.ConfirmationDate;
        //    customerStockMutationTo = _repository.CreateObject(customerStockMutationTo);

        //    customerStockMutations.Add(customerStockMutationFrom);
        //    customerStockMutations.Add(customerStockMutationTo);
        //    return customerStockMutations;
        //}

        //public IList<CustomerStockMutation> DeleteCustomerStockMutationForStockMutationMutation(StockMutationMutationDetail StockMutationMutationDetail, CustomerItem customerItemFrom, CustomerItem customerItemTo)
        //{
        //    IList<CustomerStockMutation> customerStockMutations = new List<CustomerStockMutation>();

        //    IList<CustomerStockMutation> customerStockMutationFrom = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItemFrom.Id, Constant.SourceDocumentDetailType.StockMutationMutationDetail, StockMutationMutationDetail.Id);
        //    customerStockMutationFrom.ToList().ForEach(x => customerStockMutations.Add(x));
        //    foreach (var customerStockMutation in customerStockMutationFrom)
        //    {
        //        _repository.Delete(customerStockMutation);
        //    }

        //    IList<CustomerStockMutation> customerStockMutationTo = _repository.GetObjectsBySourceDocumentDetailForCustomerItem(customerItemTo.Id, Constant.SourceDocumentDetailType.StockMutationMutationDetail, StockMutationMutationDetail.Id);
        //    customerStockMutationTo.ToList().ForEach(x => customerStockMutations.Add(x));
        //    foreach (var customerStockMutation in customerStockMutationTo)
        //    {
        //        _repository.Delete(customerStockMutation);
        //    }
        //    return customerStockMutations;
        //}

        public void StockMutateObject(CustomerStockMutation customerStockMutation, bool IsInHouse, IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService)
        {
            if (!IsInHouse)
            {
                // decimal stockAdjustmentDetailPrice = (stockMutation.Status == Constant.MutationStatus.Addition) ? stockAdjustmentDetail.Price : ((-1) * stockAdjustmentDetail.Price);
                // item.CustomerAvgCost = _itemService.CalculateCustomerAvgCost(item, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);

                decimal Quantity = (customerStockMutation.Status == Constant.MutationStatus.Addition) ? customerStockMutation.Quantity : (-1) * customerStockMutation.Quantity;
                Item item = _itemService.GetObjectById(customerStockMutation.ItemId);
                CustomerItem customerItem = customerStockMutation.CustomerItemId == null ? null : _customerItemService.GetObjectById((int)customerStockMutation.CustomerItemId);
                WarehouseItem warehouseItem = customerStockMutation.WarehouseItemId == null ? null : _warehouseItemService.GetObjectById((int)customerStockMutation.WarehouseItemId);

                if (customerStockMutation.ItemCase == Constant.ItemCase.Ready)
                {
                    _itemService.AdjustCustomerQuantity(item, Quantity);
                    if (warehouseItem != null) { _warehouseItemService.AdjustCustomerQuantity(warehouseItem, Quantity); }
                    if (customerItem != null) { _customerItemService.AdjustQuantity(customerItem, Quantity); }
                }
                else if (customerStockMutation.ItemCase == Constant.ItemCase.PendingDelivery)
                {
                    _itemService.AdjustPendingDelivery(item, Quantity);
                }
                else if (customerStockMutation.ItemCase == Constant.ItemCase.PendingReceival)
                {
                    _itemService.AdjustPendingReceival(item, Quantity);
                }
                //else if (customerStockMutation.ItemCase == Constant.ItemCase.Virtual)
                //{
                //    _itemService.AdjustCustomerVirtual(item, Quantity);
                //}
            }
        }

        public void ReverseStockMutateObject(CustomerStockMutation customerStockMutation, bool IsInHouse, IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService)
        {
            if (!IsInHouse)
            {
                // decimal stockAdjustmentDetailPrice = (stockMutation.Status == Constant.MutationStatus.Addition) ? stockAdjustmentDetail.Price : ((-1) * stockAdjustmentDetail.Price);
                // item.CustomerAvgCost = _itemService.CalculateCustomerAvgCost(item, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);

                decimal Quantity = (customerStockMutation.Status == Constant.MutationStatus.Deduction) ? customerStockMutation.Quantity : (-1) * customerStockMutation.Quantity;
                Item item = _itemService.GetObjectById(customerStockMutation.ItemId);
                CustomerItem customerItem = customerStockMutation.CustomerItemId == null ? null : _customerItemService.GetObjectById((int)customerStockMutation.CustomerItemId);
                WarehouseItem warehouseItem = customerStockMutation.WarehouseItemId == null ? null : _warehouseItemService.GetObjectById((int)customerStockMutation.WarehouseItemId);

                if (customerStockMutation.ItemCase == Constant.ItemCase.Ready)
                {
                    _itemService.AdjustCustomerQuantity(item, Quantity);
                    if (warehouseItem != null) { _warehouseItemService.AdjustCustomerQuantity(warehouseItem, Quantity); }
                    if (customerItem != null) { _customerItemService.AdjustQuantity(customerItem, Quantity); }
                }
                else if (customerStockMutation.ItemCase == Constant.ItemCase.PendingDelivery)
                { 
                    _itemService.AdjustPendingDelivery(item, Quantity); 
                }
                else if (customerStockMutation.ItemCase == Constant.ItemCase.PendingReceival)
                { 
                    _itemService.AdjustPendingReceival(item, Quantity); 
                }
                //else if (customerStockMutation.ItemCase == Constant.ItemCase.Virtual)
                //{
                //    _itemService.AdjustCustomerVirtual(item, Quantity);
                //}
            }
        }

    }
}
