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
        IList<SalesOrder> GetObjectsByCustomerId(int customerId);
        SalesOrder CreateObject(SalesOrder salesOrder, ICustomerService _customerService);
        SalesOrder CreateObject(int customerId, DateTime salesDate, ICustomerService _customerService);
        SalesOrder UpdateObject(SalesOrder salesOrder, ICustomerService _customerService);
        SalesOrder SoftDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
        bool DeleteObject(int Id);
        SalesOrder ConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _sods,
                                    IStockMutationService _stockMutationService, IItemService _itemService);
        SalesOrder UnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService,
                                    IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        SalesOrder CompleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService);
    }
}