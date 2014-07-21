using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IBarringRepository : IRepository<Barring>
    {
        IList<Barring> GetAll();
        IList<Barring> GetObjectsByItemTypeId(int ItemTypeId);
        IList<Barring> GetObjectsByUoMId(int UoMId);
        IList<Barring> GetObjectsByMachineId(int machineId);
        IList<Barring> GetObjectsByCustomerId(int customerId);
        Barring GetObjectById(int Id);
        Barring GetObjectBySku(string Sku);
        Barring CreateObject(Barring barring);
        Barring UpdateObject(Barring barring);
        Barring SoftDeleteObject(Barring barring);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Barring barring);
    }
}