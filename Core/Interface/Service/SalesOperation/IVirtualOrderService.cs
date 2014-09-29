using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IVirtualOrderService
    {
        IVirtualOrderValidator GetValidator();
        IQueryable<VirtualOrder> GetQueryable();
        IList<VirtualOrder> GetAll();
        VirtualOrder GetObjectById(int Id);
        IList<VirtualOrder> GetObjectsByContactId(int contactId);
        IList<VirtualOrder> GetConfirmedObjects();
        VirtualOrder CreateObject(VirtualOrder virtualOrder, IContactService _contactService);
        VirtualOrder UpdateObject(VirtualOrder virtualOrder, IContactService _contactService);
        VirtualOrder SoftDeleteObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService);
        bool DeleteObject(int Id);
        VirtualOrder ConfirmObject(VirtualOrder virtualOrder, DateTime ConfirmationDate, IVirtualOrderDetailService _virtualOrderDetailService,
                                 IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService,
                                 IWarehouseItemService _warehouseItemService);
        VirtualOrder UnconfirmObject(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService,
                                   ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                   IStockMutationService _stockMutationService,
                                   IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        VirtualOrder CheckAndSetDeliveryComplete(VirtualOrder virtualOrder, IVirtualOrderDetailService _virtualOrderDetailService);
        VirtualOrder UnsetDeliveryComplete(VirtualOrder virtualOrder);
    }
}