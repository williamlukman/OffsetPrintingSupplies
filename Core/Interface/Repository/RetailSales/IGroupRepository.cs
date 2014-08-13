using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface IGroupRepository : IRepository<Group>
    {
        IList<Group> GetAll();
        Group GetObjectById(int Id);
        Group GetObjectByIsLegacy(bool IsLegacy);
        Group GetObjectByName(string Name);
        Group CreateObject(Group group);
        Group UpdateObject(Group group);
        Group SoftDeleteObject(Group group);
        bool DeleteObject(int Id);
    }
}
