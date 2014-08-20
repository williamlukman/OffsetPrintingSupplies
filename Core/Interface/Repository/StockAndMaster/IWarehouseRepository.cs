using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        IQueryable<Warehouse> GetQueryable();
        IList<Warehouse> GetAll();
        Warehouse GetObjectById(int Id);
        Warehouse GetObjectByCode(string Code);
        Warehouse GetObjectByName(string Name);
        Warehouse CreateObject(Warehouse warehouse);
        Warehouse UpdateObject(Warehouse warehouse);
        Warehouse SoftDeleteObject(Warehouse warehouse);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(Warehouse warehouse);
    }
}