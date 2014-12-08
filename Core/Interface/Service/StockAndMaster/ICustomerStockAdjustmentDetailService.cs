using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICustomerStockAdjustmentDetailService
    {
        ICustomerStockAdjustmentDetailValidator GetValidator();
        IQueryable<CustomerStockAdjustmentDetail> GetQueryable();
        IList<CustomerStockAdjustmentDetail> GetAll();
        IList<CustomerStockAdjustmentDetail> GetObjectsByCustomerStockAdjustmentId(int customerStockAdjustmentId);
        IList<CustomerStockAdjustmentDetail> GetObjectsByItemId(int itemId);
        CustomerStockAdjustmentDetail GetObjectById(int Id);
        CustomerStockAdjustmentDetail CreateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService,
                                           IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        CustomerStockAdjustmentDetail CreateObject(int customerStockAdjustmentId, int itemId, int quantity, decimal price, ICustomerStockAdjustmentService _customerStockAdjustmentService,
                                           IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        CustomerStockAdjustmentDetail UpdateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService,
                                           IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        CustomerStockAdjustmentDetail SoftDeleteObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail);
        bool DeleteObject(int Id);
        CustomerStockAdjustmentDetail ConfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, DateTime ConfirmationDate, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockMutationService _customerStockMutationService,
                                                   IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService);
        CustomerStockAdjustmentDetail UnconfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockMutationService _customerStockMutationService,
                                                     IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService);
    }
}