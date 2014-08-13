using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IRetailSalesInvoiceDetailValidator
    {
        RetailSalesInvoiceDetail VConfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        RetailSalesInvoiceDetail VUnconfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        
        RetailSalesInvoiceDetail VCreateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        RetailSalesInvoiceDetail VUpdateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        RetailSalesInvoiceDetail VDeleteObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail);

        bool ValidConfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        bool ValidUnconfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);

        bool ValidCreateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        bool ValidUpdateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        bool ValidDeleteObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail);
        bool isValid(RetailSalesInvoiceDetail retailSalesInvoiceDetail);
        string PrintError(RetailSalesInvoiceDetail retailSalesInvoiceDetail);
    }
}
