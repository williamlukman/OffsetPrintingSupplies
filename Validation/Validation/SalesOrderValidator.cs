using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class SalesOrderValidator : ISalesOrderValidator
    {
        public SalesOrder VCustomer(SalesOrder salesOrder, ICustomerService _customerService)
        {
            Customer customer = _customerService.GetObjectById(salesOrder.CustomerId);
            if (customer == null)
            {
                salesOrder.Errors.Add("Customer", "Tidak boleh tidak ada");
            }
            return salesOrder;
        }

        public SalesOrder VSalesDate(SalesOrder salesOrder)
        {
            /* salesDate is never null
            if (salesOrder.SalesDate == null)
            {
                salesOrder.Errors.Add("Sales Date, "Tidak boleh tidak ada");
            }
            */
            return salesOrder;
        }

        public SalesOrder VIsConfirmed(SalesOrder salesOrder)
        {
            if (salesOrder.IsConfirmed)
            {
                salesOrder.Errors.Add("IsConfirmed", "Tidak boleh sudah dikonfirmasi");
            }
            return salesOrder;
        }

        public SalesOrder VHasSalesOrderDetails(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> details = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
            if (!details.Any())
            {
                salesOrder.Errors.Add("SalesOrderDetail", "Tidak boleh tidak ada");
            }
            return salesOrder;
        }

        public SalesOrder VAllDetailsHaveBeenFinished(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> details = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
            foreach (var detail in details)
            {
                if (!detail.IsFinished)
                {
                    salesOrder.Errors.Add("Generic", "Detail belum selesai");
                    return salesOrder;
                }
            }
            return salesOrder;
        }

        public SalesOrder VCreateObject(SalesOrder salesOrder, ICustomerService _customerService)
        {
            VCustomer(salesOrder, _customerService);
            if (!isValid(salesOrder)) { return salesOrder; }
            VSalesDate(salesOrder);
            return salesOrder;
        }

        public SalesOrder VUpdateObject(SalesOrder salesOrder, ICustomerService _customerService)
        {
            VCustomer(salesOrder, _customerService);
            if (!isValid(salesOrder)) { return salesOrder; }
            VSalesDate(salesOrder);
            if (!isValid(salesOrder)) { return salesOrder; }
            VIsConfirmed(salesOrder);
            return salesOrder;
        }

        public SalesOrder VDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            VIsConfirmed(salesOrder);
            return salesOrder;
        }

        public SalesOrder VConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            VIsConfirmed(salesOrder);
            if (!isValid(salesOrder)) { return salesOrder; }
            VHasSalesOrderDetails(salesOrder, _salesOrderDetailService);
            /*
                if (isValid(salesOrder))
                {
                    IList<SalesOrderDetail> details = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
                    ISalesOrderDetailValidator detailvalidator = new SalesOrderDetailValidator();
                    foreach (var detail in details)
                    {
                        detailvalidator.VConfirmObject(detail);
                        foreach (var error in detail.Errors)
                        {
                            salesOrder.Errors.Add(error.Key, error.Value);
                        }
                        if (!isValid(salesOrder)) { return salesOrder; }
                    }
                }
             */
            // TODO
            return salesOrder;
        }

        public SalesOrder VUnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            // TODO
            /*
            if (isValid(salesOrder))
            {
                IList<SalesOrderDetail> details = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
                foreach (var detail in details)
                {
                    if (!_salesOrderDetailService.GetValidator().ValidUnconfirmObject(detail, _salesOrderDetailService, _dods, _is))
                    {
                        foreach (var error in detail.Errors)
                        {
                            salesOrder.Errors.Add(error.Key, error.Value);
                        }
                        if (!isValid(salesOrder)) { return salesOrder; }
                    }
                }
            }
            */
            return salesOrder;
        }

        public SalesOrder VCompleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            VAllDetailsHaveBeenFinished(salesOrder, _salesOrderDetailService);
            return salesOrder;
        }

        public bool ValidCreateObject(SalesOrder salesOrder, ICustomerService _customerService)
        {
            VCreateObject(salesOrder, _customerService);
            return isValid(salesOrder);
        }

        public bool ValidUpdateObject(SalesOrder salesOrder, ICustomerService _customerService)
        {
            salesOrder.Errors.Clear();
            VUpdateObject(salesOrder, _customerService);
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

        public bool ValidUnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            salesOrder.Errors.Clear();
            VUnconfirmObject(salesOrder, _salesOrderDetailService, _dods, _is);
            return isValid(salesOrder);
        }

        public bool ValidCompleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            salesOrder.Errors.Clear();
            VCompleteObject(salesOrder, _salesOrderDetailService);
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