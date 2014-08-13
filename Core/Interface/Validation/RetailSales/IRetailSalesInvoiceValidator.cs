using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IRetailSalesInvoiceValidator
    {
        RetailSalesInvoice VConfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);
        RetailSalesInvoice VUnconfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);
        RetailSalesInvoice VPaidObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);
        RetailSalesInvoice VUnpaidObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);

        RetailSalesInvoice VCreateObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);
        RetailSalesInvoice VUpdateObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);
        RetailSalesInvoice VDeleteObject(RetailSalesInvoice retailSalesInvoice);
        
        bool ValidConfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);
        bool ValidUnconfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);
        bool ValidPaidObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);
        bool ValidUnpaidObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);

        bool ValidCreateObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);
        bool ValidUpdateObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceService _retailSalesInvoiceService);
        bool ValidDeleteObject(RetailSalesInvoice retailSalesInvoice);
        bool isValid(RetailSalesInvoice retailSalesInvoice);
        string PrintError(RetailSalesInvoice retailSalesInvoice);
    }
}
