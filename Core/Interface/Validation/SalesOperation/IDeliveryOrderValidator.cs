using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IDeliveryOrderValidator
    {
        DeliveryOrder VContact(DeliveryOrder deliveryOrder, IContactService _contactService);
        DeliveryOrder VDeliveryDate(DeliveryOrder deliveryOrder);
        DeliveryOrder VHasBeenConfirmed(DeliveryOrder deliveryOrder);
        DeliveryOrder VHasNotBeenConfirmed(DeliveryOrder deliveryOrder);
        DeliveryOrder VHasDeliveryOrderDetails(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VAllDetailsHaveBeenFinished(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VAllDetailsHaveNotBeenFinished(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VCreateObject(DeliveryOrder deliveryOrder, IContactService _contactService);
        DeliveryOrder VUpdateObject(DeliveryOrder deliveryOrder, IContactService _contactService);
        DeliveryOrder VDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        DeliveryOrder VUnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        DeliveryOrder VCompleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidCreateObject(DeliveryOrder deliveryOrder, IContactService _contactService);
        bool ValidUpdateObject(DeliveryOrder deliveryOrder, IContactService _contactService);
        bool ValidDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool ValidUnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool ValidCompleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool isValid(DeliveryOrder deliveryOrder);
        string PrintError(DeliveryOrder deliveryOrder);
    }
}
