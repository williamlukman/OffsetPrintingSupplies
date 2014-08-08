using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesOrderValidator
    {
        SalesOrder VHasContact(SalesOrder salesOrder, IContactService _contactService);
        SalesOrder VHasSalesDate(SalesOrder salesOrder);
        SalesOrder VHasBeenConfirmed(SalesOrder salesOrder);
        SalesOrder VHasNotBeenConfirmed(SalesOrder salesOrder);
        SalesOrder VHasSalesOrderDetails(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VHasNoSalesOrderDetail(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VHasNoDeliveryOrder(SalesOrder salesOrder, IDeliveryOrderService _deliveryOrderService);
        SalesOrder VHasConfirmationDate(SalesOrder salesOrder);
        SalesOrder VCreateObject(SalesOrder salesOrder, IContactService _contactService);
        SalesOrder VUpdateObject(SalesOrder salesOrder, IContactService _contactService);
        SalesOrder VDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        SalesOrder VUnconfirmObject(SalesOrder salesOrder, IDeliveryOrderService _deliveryOrderService);
        bool ValidCreateObject(SalesOrder salesOrder, IContactService _contactService);
        bool ValidUpdateObject(SalesOrder salesOrder, IContactService _contactService);
        bool ValidDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidUnconfirmObject(SalesOrder salesOrder, IDeliveryOrderService _deliveryOrderService);
        bool isValid(SalesOrder salesOrder);
        string PrintError(SalesOrder salesOrder);
    }
}
