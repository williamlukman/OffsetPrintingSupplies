using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IGroupService
    {
        IGroupValidator GetValidator();
        IList<Group> GetAll();
        Group GetObjectById(int Id);
        Group GetObjectByIsLegacy(bool IsLegacy);
        Group GetObjectByName(string Name);
        Group CreateObject(Group group);
        Group CreateObject(string Name, string Description);
        Group UpdateObject(Group group);
        Group SoftDeleteObject(Group group);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(Group group);
    }
}
