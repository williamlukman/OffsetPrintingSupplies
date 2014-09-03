using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IMachineRepository : IRepository<Machine>
    {
        IQueryable<Machine> GetQueryable();
        IList<Machine> GetAll();
        Machine GetObjectById(int Id);
        Machine GetObjectByName(string Name);
        Machine GetObjectByCode(string Code);
        Machine CreateObject(Machine machine);
        Machine UpdateObject(Machine machine);
        Machine SoftDeleteObject(Machine machine);
        bool DeleteObject(int Id);
    }
}