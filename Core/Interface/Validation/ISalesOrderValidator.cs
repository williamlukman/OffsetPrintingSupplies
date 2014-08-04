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
        SalesOrder VCustomer(SalesOrder salesOrder, ICustomerService _customerService);
        SalesOrder VSalesDate(SalesOrder salesOrder);
        SalesOrder VHasBeenConfirmed(SalesOrder salesOrder);
        SalesOrder VHasNotBeenConfirmed(SalesOrder salesOrder);
        SalesOrder VHasSalesOrderDetails(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VAllDetailsHaveBeenFinished(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VAllDetailsHaveNotBeenFinished(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VCreateObject(SalesOrder salesOrder, ICustomerService _customerService);
        SalesOrder VUpdateObject(SalesOrder salesOrder, ICustomerService _customerService);
        SalesOrder VDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VUnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService,
                                    IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        SalesOrder VCompleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidCreateObject(SalesOrder salesOrder, ICustomerService _customerService);
        bool ValidUpdateObject(SalesOrder salesOrder, ICustomerService _customerService);
        bool ValidDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidUnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService,
                                  IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool ValidCompleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool isValid(SalesOrder salesOrder);
        string PrintError(SalesOrder salesOrder);
    }
}
