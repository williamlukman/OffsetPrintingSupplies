using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IDeliveryOrderValidator
    {
        DeliveryOrder VCustomer(DeliveryOrder deliveryOrder, ICustomerService _customerService);
        DeliveryOrder VDeliveryDate(DeliveryOrder deliveryOrder);
        DeliveryOrder VIsConfirmed(DeliveryOrder deliveryOrder);
        DeliveryOrder VHasDeliveryOrderDetails(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VAllDetailsHaveBeenFinished(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VCreateObject(DeliveryOrder deliveryOrder, ICustomerService _customerService);
        DeliveryOrder VUpdateObject(DeliveryOrder deliveryOrder, ICustomerService _customerService);
        DeliveryOrder VDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        DeliveryOrder VUnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        DeliveryOrder VCompleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidCreateObject(DeliveryOrder deliveryOrder, ICustomerService _customerService);
        bool ValidUpdateObject(DeliveryOrder deliveryOrder, ICustomerService _customerService);
        bool ValidDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool ValidUnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool ValidCompleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool isValid(DeliveryOrder deliveryOrder);
        string PrintError(DeliveryOrder deliveryOrder);
    }
}
