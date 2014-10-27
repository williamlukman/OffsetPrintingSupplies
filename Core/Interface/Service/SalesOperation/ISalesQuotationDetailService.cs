using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesQuotationDetailService
    {
        ISalesQuotationDetailValidator GetValidator();
        IQueryable<SalesQuotationDetail> GetQueryable();
        IList<SalesQuotationDetail> GetAll();
        IList<SalesQuotationDetail> GetObjectsBySalesQuotationId(int salesQuotationId);
        IList<SalesQuotationDetail> GetObjectsByItemId(int itemId);
        SalesQuotationDetail GetObjectById(int Id);
        SalesQuotationDetail CreateObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationService _salesQuotationService, IItemService _itemService);
        SalesQuotationDetail UpdateObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationService _salesQuotationService, IItemService _itemService);
        SalesQuotationDetail SoftDeleteObject(SalesQuotationDetail salesQuotationDetail);
        bool DeleteObject(int Id);
        SalesQuotationDetail ConfirmObject(SalesQuotationDetail salesQuotationDetail, DateTime ConfirmationDate,
                                           IItemService _itemService, IWarehouseItemService _warehouseItemService);
        SalesQuotationDetail UnconfirmObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationService _salesQuotationService, ISalesOrderDetailService _salesOrderDetailService,
                                             IItemService _itemService, IWarehouseItemService _warehouseItemService);
    }
}