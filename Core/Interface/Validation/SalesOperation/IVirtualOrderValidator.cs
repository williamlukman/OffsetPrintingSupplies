using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IVirtualOrderValidator
    {
        VirtualOrder VHasContact(VirtualOrder virtualOrder, IContactService _contactService);
        VirtualOrder VHasOrderDate(VirtualOrder virtualOrder);
        VirtualOrder VHasBeenConfirmed(VirtualOrder virtualOrder);
        VirtualOrder VHasNotBeenConfirmed(VirtualOrder virtualOrder);
        VirtualOrder VHasVirtualOrderDetails(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService);
        VirtualOrder VHasNoVirtualOrderDetail(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService);
        VirtualOrder VHasNoTemporaryDeliveryOrder(VirtualOrder virtualOrder, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        VirtualOrder VHasConfirmationDate(VirtualOrder virtualOrder);
        VirtualOrder VCreateObject(VirtualOrder virtualOrder, IContactService _contactService);
        VirtualOrder VUpdateObject(VirtualOrder virtualOrder, IContactService _contactService);
        VirtualOrder VDeleteObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService);
        VirtualOrder VConfirmObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService);
        VirtualOrder VUnconfirmObject(VirtualOrder virtualOrder, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        bool ValidCreateObject(VirtualOrder virtualOrder, IContactService _contactService);
        bool ValidUpdateObject(VirtualOrder virtualOrder, IContactService _contactService);
        bool ValidDeleteObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService);
        bool ValidConfirmObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService);
        bool ValidUnconfirmObject(VirtualOrder virtualOrder, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        bool isValid(VirtualOrder virtualOrder);
        string PrintError(VirtualOrder virtualOrder);
    }
}
