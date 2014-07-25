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
    public interface IPurchaseReceivalService
    {
        IPurchaseReceivalValidator GetValidator();
        IList<PurchaseReceival> GetAll();
        PurchaseReceival GetObjectById(int Id);
        IList<PurchaseReceival> GetObjectsByCustomerId(int customerId);
        PurchaseReceival CreateObject(PurchaseReceival purchaseReceival, ICustomerService _customerService);
        PurchaseReceival CreateObject(int customerId, DateTime ReceivalDate, ICustomerService _customerService);
        PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival, ICustomerService _customerService);
        PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool DeleteObject(int Id);
        PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _prds,
                                       IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _prds,
                                         IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        PurchaseReceival CompleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
    }
}