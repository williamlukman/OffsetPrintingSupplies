using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseReceivalRepository : IRepository<PurchaseReceival>
    {
        IList<PurchaseReceival> GetAll();
        PurchaseReceival GetObjectById(int Id);
        IList<PurchaseReceival> GetObjectsByCustomerId(int customerId);
        PurchaseReceival CreateObject(PurchaseReceival purchaseReceival);
        PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival);
        PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival);
        bool DeleteObject(int Id);
        PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival);
        PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival);
        PurchaseReceival CompleteObject(PurchaseReceival purchaseReceival);
        string SetObjectCode();
    }
}