using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IGroupValidator
    {
        Group VHasUniqueName(Group group, IGroupService _groupService);
        
        Group VCreateObject(Group group, IGroupService _groupService);
        Group VUpdateObject(Group group, IGroupService _groupService);
        Group VDeleteObject(Group group);
        bool ValidCreateObject(Group group, IGroupService _groupService);
        bool ValidUpdateObject(Group group, IGroupService _groupService);
        bool ValidDeleteObject(Group group);
        bool isValid(Group group);
        string PrintError(Group group);
    }
}
