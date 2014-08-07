using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseOrderService
    {
        IPurchaseOrderValidator GetValidator();
        IList<PurchaseOrder> GetAll();
        PurchaseOrder GetObjectById(int Id);
        IList<PurchaseOrder> GetObjectsByContactId(int contactId);
        PurchaseOrder CreateObject(PurchaseOrder purchaseOrder, IContactService _contactService);
        PurchaseOrder CreateObject(int contactId, DateTime purchaseDate, IContactService _contactService);
        PurchaseOrder UpdateObject(PurchaseOrder purchaseOrder, IContactService _contactService);
        PurchaseOrder SoftDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool DeleteObject(int Id);
        PurchaseOrder ConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                    IStockMutationService _stockMutationService, IItemService _itemService);
        PurchaseOrder UnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                    IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        PurchaseOrder CompleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
    }
}