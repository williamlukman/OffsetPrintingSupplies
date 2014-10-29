using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class VirtualOrderValidator : IVirtualOrderValidator
    {
        public VirtualOrder VHasUniqueNomorSurat(VirtualOrder virtualOrder, IVirtualOrderService _virtualOrderService)
        {
            IList<VirtualOrder> duplicates = _virtualOrderService.GetQueryable().Where(x => x.NomorSurat == virtualOrder.NomorSurat && x.Id != virtualOrder.Id).ToList();
            if (duplicates.Any())
            {
                virtualOrder.Errors.Add("NomorSurat", "Tidak boleh merupakan duplikasi");
            }
            return virtualOrder;
        }

        public VirtualOrder VHasContact(VirtualOrder virtualOrder, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(virtualOrder.ContactId);
            if (contact == null)
            {
                virtualOrder.Errors.Add("Contact", "Tidak boleh tidak ada");
            }
            return virtualOrder;
        }

        public VirtualOrder VHasOrderDate(VirtualOrder virtualOrder)
        {
            if (virtualOrder.OrderDate == null)
            {
                virtualOrder.Errors.Add("OrderDate", "Tidak boleh kosong");
            }
            return virtualOrder;
        }
        
        public VirtualOrder VHasBeenConfirmed(VirtualOrder virtualOrder)
        {
            if (!virtualOrder.IsConfirmed)
            {
                virtualOrder.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return virtualOrder;
        }

        public VirtualOrder VHasNotBeenConfirmed(VirtualOrder virtualOrder)
        {
            if (virtualOrder.IsConfirmed)
            {
                virtualOrder.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return virtualOrder;
        }

        public VirtualOrder VHasVirtualOrderDetails(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService)
        {
            IList<VirtualOrderDetail> details = _virtualOrderDetailService.GetObjectsByVirtualOrderId(virtualOrder.Id);
            if (!details.Any())
            {
                virtualOrder.Errors.Add("Generic", "Tidak memiliki virtual order detail");
            }
            return virtualOrder;
        }

        public VirtualOrder VHasNoVirtualOrderDetail(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService)
        {
            IList<VirtualOrderDetail> details = _virtualOrderDetailService.GetObjectsByVirtualOrderId(virtualOrder.Id);
            if (details.Any())
            {
                virtualOrder.Errors.Add("Generic", "Masih memiliki virtual order detail");
            }
            return virtualOrder;
        }

        public VirtualOrder VHasNoTemporaryDeliveryOrder(VirtualOrder virtualOrder, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            IList<TemporaryDeliveryOrder> temporaryDeliveryOrders = _temporaryDeliveryOrderService.GetObjectsByVirtualOrderId(virtualOrder.Id); 
            if (temporaryDeliveryOrders.Any())
            {
                virtualOrder.Errors.Add("Generic", "Sudah memiliki temporary delivery order");
            }
            return virtualOrder;
        }

        public VirtualOrder VHasConfirmationDate(VirtualOrder obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public VirtualOrder VCreateObject(VirtualOrder virtualOrder, IVirtualOrderService _virtualOrderService, IContactService _contactService)
        {
            VHasUniqueNomorSurat(virtualOrder, _virtualOrderService);
            if (!isValid(virtualOrder)) { return virtualOrder; }
            VHasContact(virtualOrder, _contactService);
            if (!isValid(virtualOrder)) { return virtualOrder; }
            VHasOrderDate(virtualOrder);
            return virtualOrder;
        }

        public VirtualOrder VUpdateObject(VirtualOrder virtualOrder, IVirtualOrderService _virtualOrderService, IContactService _contactService)
        {
            VCreateObject(virtualOrder, _virtualOrderService, _contactService);
            if (!isValid(virtualOrder)) { return virtualOrder; }
            VHasNotBeenConfirmed(virtualOrder);
            return virtualOrder;
        }

        public VirtualOrder VDeleteObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService)
        {
            VHasNotBeenConfirmed(virtualOrder);
            if (!isValid(virtualOrder)) { return virtualOrder; }
            VHasNoVirtualOrderDetail(virtualOrder, _virtualOrderDetailService);
            return virtualOrder;
        }

        public VirtualOrder VConfirmObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService)
        {
            VHasConfirmationDate(virtualOrder);
            if (!isValid(virtualOrder)) { return virtualOrder; }
            VHasNotBeenConfirmed(virtualOrder);
            if (!isValid(virtualOrder)) { return virtualOrder; }
            VHasVirtualOrderDetails(virtualOrder, _virtualOrderDetailService);
            return virtualOrder;
        }

        public VirtualOrder VUnconfirmObject(VirtualOrder virtualOrder, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            VHasBeenConfirmed(virtualOrder);
            if (!isValid(virtualOrder)) { return virtualOrder; }
            VHasNoTemporaryDeliveryOrder(virtualOrder, _temporaryDeliveryOrderService);
            return virtualOrder;
        }

        public bool ValidCreateObject(VirtualOrder virtualOrder, IVirtualOrderService _virtualOrderService, IContactService _contactService)
        {
            VCreateObject(virtualOrder, _virtualOrderService, _contactService);
            return isValid(virtualOrder);
        }

        public bool ValidUpdateObject(VirtualOrder virtualOrder, IVirtualOrderService _virtualOrderService, IContactService _contactService)
        {
            virtualOrder.Errors.Clear();
            VUpdateObject(virtualOrder, _virtualOrderService, _contactService);
            return isValid(virtualOrder);
        }

        public bool ValidDeleteObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService)
        {
            virtualOrder.Errors.Clear();
            VDeleteObject(virtualOrder, _virtualOrderDetailService);
            return isValid(virtualOrder);
        }

        public bool ValidConfirmObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService)
        {
            virtualOrder.Errors.Clear();
            VConfirmObject(virtualOrder, _virtualOrderDetailService);
            return isValid(virtualOrder);
        }

        public bool ValidUnconfirmObject(VirtualOrder virtualOrder, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            virtualOrder.Errors.Clear();
            VUnconfirmObject(virtualOrder, _temporaryDeliveryOrderService);
            return isValid(virtualOrder);
        }

        public bool isValid(VirtualOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(VirtualOrder obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}