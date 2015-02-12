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
    public class SalesOrderValidator : ISalesOrderValidator
    {

        public SalesOrder VHasUniqueNomorSurat(SalesOrder salesOrder, ISalesOrderService _salesOrderService)
        {
            IList<SalesOrder> duplicates = _salesOrderService.GetQueryable().Where(x => x.NomorSurat == salesOrder.NomorSurat && x.Id != salesOrder.Id & !x.IsDeleted).ToList();
            if (duplicates.Any())
            {
                salesOrder.Errors.Add("NomorSurat", "Tidak boleh merupakan duplikasi");
            }
            return salesOrder;
        }

        public SalesOrder VHasContact(SalesOrder salesOrder, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(salesOrder.ContactId);
            if (contact == null)
            {
                salesOrder.Errors.Add("Contact", "Tidak boleh tidak ada");
            }
            return salesOrder;
        }

        public SalesOrder VHasSalesDate(SalesOrder salesOrder)
        {
            if (salesOrder.SalesDate == null)
            {
                salesOrder.Errors.Add("SalesDate", "Tidak boleh kosong");
            }
            return salesOrder;
        }
        
        public SalesOrder VHasBeenConfirmed(SalesOrder salesOrder)
        {
            if (!salesOrder.IsConfirmed)
            {
                salesOrder.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return salesOrder;
        }

        public SalesOrder VHasNotBeenConfirmed(SalesOrder salesOrder)
        {
            if (salesOrder.IsConfirmed)
            {
                salesOrder.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return salesOrder;
        }

        public SalesOrder VHasSalesOrderDetails(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> details = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
            if (!details.Any())
            {
                salesOrder.Errors.Add("Generic", "Tidak memiliki sales order detail");
            }
            return salesOrder;
        }

        public SalesOrder VHasNoSalesOrderDetail(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> details = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
            if (details.Any())
            {
                salesOrder.Errors.Add("Generic", "Masih memiliki sales order detail");
            }
            return salesOrder;
        }

        public SalesOrder VHasNoDeliveryOrder(SalesOrder salesOrder, IDeliveryOrderService _deliveryOrderService)
        {
            IList<DeliveryOrder> deliveryOrders = _deliveryOrderService.GetObjectsBySalesOrderId(salesOrder.Id); 
            if (deliveryOrders.Any())
            {
                salesOrder.Errors.Add("Generic", "Sudah memiliki delivery order");
            }
            return salesOrder;
        }

        public SalesOrder VHasConfirmationDate(SalesOrder obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public SalesOrder VCreateObject(SalesOrder salesOrder, ISalesOrderService _salesOrderService, IContactService _contactService)
        {
            VHasUniqueNomorSurat(salesOrder, _salesOrderService);
            if (!isValid(salesOrder)) { return salesOrder; }
            VHasContact(salesOrder, _contactService);
            if (!isValid(salesOrder)) { return salesOrder; }
            VHasSalesDate(salesOrder);
            return salesOrder;
        }

        public SalesOrder VUpdateObject(SalesOrder salesOrder, ISalesOrderService _salesOrderService, IContactService _contactService)
        {
            VCreateObject(salesOrder, _salesOrderService, _contactService);
            if (!isValid(salesOrder)) { return salesOrder; }
            VHasNotBeenConfirmed(salesOrder);
            return salesOrder;
        }

        public SalesOrder VDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            VHasNotBeenConfirmed(salesOrder);
            if (!isValid(salesOrder)) { return salesOrder; }
            VHasNoSalesOrderDetail(salesOrder, _salesOrderDetailService);
            return salesOrder;
        }

        public SalesOrder VConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            VHasConfirmationDate(salesOrder);
            if (!isValid(salesOrder)) { return salesOrder; }
            VHasNotBeenConfirmed(salesOrder);
            if (!isValid(salesOrder)) { return salesOrder; }
            VHasSalesOrderDetails(salesOrder, _salesOrderDetailService);
            return salesOrder;
        }

        public SalesOrder VUnconfirmObject(SalesOrder salesOrder, IDeliveryOrderService _deliveryOrderService)
        {
            VHasBeenConfirmed(salesOrder);
            if (!isValid(salesOrder)) { return salesOrder; }
            VHasNoDeliveryOrder(salesOrder, _deliveryOrderService);
            return salesOrder;
        }

        public bool ValidCreateObject(SalesOrder salesOrder, ISalesOrderService _salesOrderService, IContactService _contactService)
        {
            VCreateObject(salesOrder, _salesOrderService, _contactService);
            return isValid(salesOrder);
        }

        public bool ValidUpdateObject(SalesOrder salesOrder, ISalesOrderService _salesOrderService, IContactService _contactService)
        {
            salesOrder.Errors.Clear();
            VUpdateObject(salesOrder, _salesOrderService, _contactService);
            return isValid(salesOrder);
        }

        public bool ValidDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            salesOrder.Errors.Clear();
            VDeleteObject(salesOrder, _salesOrderDetailService);
            return isValid(salesOrder);
        }

        public bool ValidConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            salesOrder.Errors.Clear();
            VConfirmObject(salesOrder, _salesOrderDetailService);
            return isValid(salesOrder);
        }

        public bool ValidUnconfirmObject(SalesOrder salesOrder, IDeliveryOrderService _deliveryOrderService)
        {
            salesOrder.Errors.Clear();
            VUnconfirmObject(salesOrder, _deliveryOrderService);
            return isValid(salesOrder);
        }

        public bool isValid(SalesOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesOrder obj)
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