using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICompoundRepository : IRepository<Compound>
    {
        IQueryable<Compound> GetQueryable();
        IList<Compound> GetAll();
        IList<Compound> GetObjectsByItemTypeId(int ItemTypeId);
        IList<Compound> GetObjectsByUoMId(int UoMId);
        Compound GetObjectById(int Id);
        Compound GetObjectBySku(string Sku);
        Compound CreateObject(Compound compound);
        Compound UpdateObject(Compound compound);
        Compound SoftDeleteObject(Compound compound);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Compound compound);
    }
}