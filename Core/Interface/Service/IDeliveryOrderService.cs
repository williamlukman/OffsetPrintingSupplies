using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IDeliveryOrderService
    {
        IDeliveryOrderValidator GetValidator();
        IList<DeliveryOrder> GetAll();
        DeliveryOrder GetObjectById(int Id);
        IList<DeliveryOrder> GetObjectsByCustomerId(int customerId);
        DeliveryOrder CreateObject(DeliveryOrder deliveryOrder, ICustomerService _customerService);
        DeliveryOrder CreateObject(int customerId, DateTime deliveryDate, ICustomerService _customerService);
        DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder, ICustomerService _customerService);
        DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool DeleteObject(int Id);
        DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                    ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                    IStockMutationService _stockMutationService, IItemService _itemService);
    }
}