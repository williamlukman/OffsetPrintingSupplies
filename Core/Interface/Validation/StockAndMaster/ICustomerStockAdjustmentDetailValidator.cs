using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICustomerStockAdjustmentDetailValidator
    {
        CustomerStockAdjustmentDetail VCreateObject(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail, ICustomerStockAdjustmentDetailService _CustomerStockAdjustmentDetails, ICustomerStockAdjustmentService _CustomerStockAdjustmentService,
                                            IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        CustomerStockAdjustmentDetail VUpdateObject(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail, ICustomerStockAdjustmentDetailService _CustomerStockAdjustmentDetails, ICustomerStockAdjustmentService _CustomerStockAdjustmentService,
                                            IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        CustomerStockAdjustmentDetail VDeleteObject(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail);
        CustomerStockAdjustmentDetail VConfirmObject(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService, IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService);
        CustomerStockAdjustmentDetail VUnconfirmObject(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService, IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService);
        bool ValidCreateObject(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail, ICustomerStockAdjustmentDetailService _CustomerStockAdjustmentDetails, ICustomerStockAdjustmentService _CustomerStockAdjustmentService,
                               IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        bool ValidUpdateObject(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail, ICustomerStockAdjustmentDetailService _CustomerStockAdjustmentDetails, ICustomerStockAdjustmentService _CustomerStockAdjustmentService,
                               IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        bool ValidDeleteObject(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail);
        bool ValidConfirmObject(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService, IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService, IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService);
        bool isValid(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail);
        string PrintError(CustomerStockAdjustmentDetail CustomerStockAdjustmentDetail);
    }
}
