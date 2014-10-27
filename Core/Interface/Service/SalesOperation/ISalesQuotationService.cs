using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesQuotationService
    {
        ISalesQuotationValidator GetValidator();
        IQueryable<SalesQuotation> GetQueryable();
        IList<SalesQuotation> GetAll();
        IList<SalesQuotation> GetApprovedObjects();
        IList<SalesQuotation> GetObjectsByContactId(int contactId);
        SalesQuotation GetObjectById(int Id);
        SalesQuotation CreateObject(SalesQuotation salesQuotation, IContactService _contactService);
        SalesQuotation UpdateObject(SalesQuotation salesQuotation, IContactService _contactService);
        SalesQuotation SoftDeleteObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService);
        bool DeleteObject(int Id);
        SalesQuotation ConfirmObject(SalesQuotation salesQuotation, DateTime ConfirmationDate, ISalesQuotationDetailService _salesQuotationDetailService,
                                     IItemService _itemService, IWarehouseItemService _warehouseItemService);
        SalesQuotation UnconfirmObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService,
                                       IItemService _itemService, IWarehouseItemService _warehouseItemService,
                                       ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService);
        SalesQuotation ApproveObject(SalesQuotation salesQuotation);
        SalesQuotation RejectObject(SalesQuotation salesQuotation);
    }
}