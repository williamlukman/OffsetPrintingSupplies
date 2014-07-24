using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IDeliveryOrderRepository : IRepository<DeliveryOrder>
    {
        IList<DeliveryOrder> GetAll();
        DeliveryOrder GetObjectById(int Id);
        IList<DeliveryOrder> GetObjectsByCustomerId(int customerId);
        DeliveryOrder CreateObject(DeliveryOrder deliveryOrder);
        DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder);
        DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder);
        bool DeleteObject(int Id);
        DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder);
        DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder);
        DeliveryOrder CompleteObject(DeliveryOrder deliveryOrder);
        string SetObjectCode();
    }
}