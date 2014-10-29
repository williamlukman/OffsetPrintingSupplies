using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesQuotationValidator
    {
        SalesQuotation VHasUniqueNomorSurat(SalesQuotation salesQuotation, ISalesQuotationService _salesQuotationService);
        SalesQuotation VHasContact(SalesQuotation salesQuotation, IContactService _contactService);
        SalesQuotation VHasQuotationDate(SalesQuotation salesQuotation);
        SalesQuotation VHasBeenConfirmed(SalesQuotation salesQuotation);
        SalesQuotation VHasNotBeenConfirmed(SalesQuotation salesQuotation);
        SalesQuotation VHasSalesQuotationDetails(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService);
        SalesQuotation VHasNoSalesQuotationDetail(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService);
        SalesQuotation VHasNoSalesOrder(SalesQuotation salesQuotation, ISalesOrderService _salesOrderService);
        SalesQuotation VHasConfirmationDate(SalesQuotation salesQuotation);
        SalesQuotation VHasNotBeenApproved(SalesQuotation salesQuotation);
        SalesQuotation VHasNotBeenRejected(SalesQuotation salesQuotation);

        SalesQuotation VCreateObject(SalesQuotation salesQuotation, ISalesQuotationService _salesQuotationService, IContactService _contactService);
        SalesQuotation VUpdateObject(SalesQuotation salesQuotation, ISalesQuotationService _salesQuotationService, IContactService _contactService);
        SalesQuotation VDeleteObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService);
        SalesQuotation VConfirmObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService);
        SalesQuotation VUnconfirmObject(SalesQuotation salesQuotation, ISalesOrderService _salesOrderService);
        SalesQuotation VApproveObject(SalesQuotation salesQuotation);
        SalesQuotation VRejectObject(SalesQuotation salesQuotation);
        bool ValidCreateObject(SalesQuotation salesQuotation, ISalesQuotationService _salesQuotationService, IContactService _contactService);
        bool ValidUpdateObject(SalesQuotation salesQuotation, ISalesQuotationService _salesQuotationService, IContactService _contactService);
        bool ValidDeleteObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService);
        bool ValidConfirmObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService);
        bool ValidUnconfirmObject(SalesQuotation salesQuotation, ISalesOrderService _salesOrderService);
        bool ValidApproveObject(SalesQuotation salesQuotation);
        bool ValidRejectObject(SalesQuotation salesQuotation);
        bool isValid(SalesQuotation salesQuotation);
        string PrintError(SalesQuotation salesQuotation);
    }
}
