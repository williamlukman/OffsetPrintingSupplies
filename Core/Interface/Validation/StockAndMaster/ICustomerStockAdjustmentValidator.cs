using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICustomerStockAdjustmentValidator
    {
        CustomerStockAdjustment VDetailsAreVerifiedConfirmable(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                                       IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService);
        CustomerStockAdjustment VCreateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService);
        CustomerStockAdjustment VUpdateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService);
        CustomerStockAdjustment VDeleteObject(CustomerStockAdjustment customerStockAdjustment);
        CustomerStockAdjustment VHasConfirmationDate(CustomerStockAdjustment customerStockAdjustment);
        CustomerStockAdjustment VConfirmObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                       IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        CustomerStockAdjustment VUnconfirmObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                       IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        bool ValidCreateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService);
        bool ValidUpdateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService);
        bool ValidDeleteObject(CustomerStockAdjustment customerStockAdjustment);
        bool ValidConfirmObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        bool ValidUnconfirmObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        bool isValid(CustomerStockAdjustment customerStockAdjustment);
        string PrintError(CustomerStockAdjustment customerStockAdjustment);
    }
}
