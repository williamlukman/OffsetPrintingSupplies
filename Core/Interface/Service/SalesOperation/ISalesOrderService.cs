using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesOrderService
    {
        ISalesOrderValidator GetValidator();
        IList<SalesOrder> GetAll();
        SalesOrder GetObjectById(int Id);
        IList<SalesOrder> GetObjectsByContactId(int contactId);
        SalesOrder CreateObject(SalesOrder salesOrder, IContactService _contactService);
        SalesOrder CreateObject(int contactId, DateTime salesDate, IContactService _contactService);
        SalesOrder UpdateObject(SalesOrder salesOrder, IContactService _contactService);
        SalesOrder SoftDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool DeleteObject(int Id);
        SalesOrder ConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _sods,
                                    IStockMutationService _stockMutationService, IItemService _itemService);
        SalesOrder UnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService,
                                    IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        SalesOrder CompleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
    }
}