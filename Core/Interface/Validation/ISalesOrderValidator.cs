using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesOrderValidator
    {
        SalesOrder VCustomer(SalesOrder _salesOrder, ICustomerService _customerService);
        SalesOrder VSalesDate(SalesOrder _salesOrder);
        SalesOrder VIsConfirmed(SalesOrder _salesOrder);
        SalesOrder VHasSalesOrderDetails(SalesOrder _salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VCreateObject(SalesOrder _salesOrder, ICustomerService _customerService);
        SalesOrder VUpdateObject(SalesOrder _salesOrder, ICustomerService _customerService);
        SalesOrder VDeleteObject(SalesOrder _salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VConfirmObject(SalesOrder _salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VUnconfirmObject(SalesOrder _salesOrder, ISalesOrderDetailService _salesOrderDetailService,
                                    IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        SalesOrder VCompleteObject(SalesOrder _salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidCreateObject(SalesOrder _salesOrder, ICustomerService _customerService);
        bool ValidUpdateObject(SalesOrder _salesOrder, ICustomerService _customerService);
        bool ValidDeleteObject(SalesOrder _salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidConfirmObject(SalesOrder _salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidUnconfirmObject(SalesOrder _salesOrder, ISalesOrderDetailService _salesOrderDetailService,
                                  IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool ValidCompleteObject(SalesOrder _salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool isValid(SalesOrder _salesOrder);
        string PrintError(SalesOrder _salesOrder);
    }
}
