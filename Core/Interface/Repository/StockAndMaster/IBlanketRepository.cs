using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBlanketRepository : IRepository<Blanket>
    {
        IQueryable<Blanket> GetQueryable();
        IList<Blanket> GetAll();
        IList<Blanket> GetObjectsByItemTypeId(int ItemTypeId);
        IList<Blanket> GetObjectsByUoMId(int UoMId);
        IList<Blanket> GetObjectsByMachineId(int machineId);
        IList<Blanket> GetObjectsByContactId(int contactId);
        IList<Blanket> GetObjectsByRollBlanketItemId(int rollBlanketItemId);
        IList<Blanket> GetObjectsByLeftBarItemId(int leftBarId);
        IList<Blanket> GetObjectsByRightBarItemId(int rightBarId);
        Item GetRollBlanketItem(Blanket blanket);
        Item GetLeftBarItem(Blanket blanket);
        Item GetRightBarItem(Blanket blanket);
        Blanket GetObjectById(int Id);
        Blanket GetObjectBySku(string Sku);
        Blanket CreateObject(Blanket blanket);
        Blanket UpdateObject(Blanket blanket);
        Blanket SoftDeleteObject(Blanket blanket);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Blanket blanket);
    }
}