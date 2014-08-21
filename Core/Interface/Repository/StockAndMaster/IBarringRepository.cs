using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBarringRepository : IRepository<Barring>
    {
        IQueryable<Barring> GetQueryable();
        IList<Barring> GetAll();
        IList<Barring> GetObjectsByItemTypeId(int ItemTypeId);
        IList<Barring> GetObjectsByUoMId(int UoMId);
        IList<Barring> GetObjectsByMachineId(int machineId);
        IList<Barring> GetObjectsByContactId(int contactId);
        IList<Barring> GetObjectsByBlanketItemId(int blanketItemId);
        IList<Barring> GetObjectsByLeftBarItemId(int leftBarId);
        IList<Barring> GetObjectsByRightBarItemId(int rightBarId);
        Item GetBlanketItem(Barring barring);
        Item GetLeftBarItem(Barring barring);
        Item GetRightBarItem(Barring barring);
        Barring GetObjectById(int Id);
        Barring GetObjectBySku(string Sku);
        Barring CreateObject(Barring barring);
        Barring UpdateObject(Barring barring);
        Barring SoftDeleteObject(Barring barring);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Barring barring);
    }
}