using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class GroupValidator : IGroupValidator
    {
        public Group VHasUniqueName(Group group, IGroupService _groupService)
        {
            if (String.IsNullOrEmpty(group.Name) || group.Name.Trim() == "")
            {
                group.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_groupService.IsNameDuplicated(group))
            {
                group.Errors.Add("Name", "Tidak boleh ada duplikasi");
            }
            return group;
        }

        public Group VCreateObject(Group group, IGroupService _groupService)
        {
            VHasUniqueName(group, _groupService);
            return group;
        }

        public Group VUpdateObject(Group group, IGroupService _groupService)
        {
            VCreateObject(group, _groupService);
            return group;
        }

        public Group VDeleteObject(Group group)
        {
            
            return group;
        }

        public bool ValidCreateObject(Group group, IGroupService _groupService)
        {
            VCreateObject(group, _groupService);
            return isValid(group);
        }

        public bool ValidUpdateObject(Group group, IGroupService _groupService)
        {
            group.Errors.Clear();
            VUpdateObject(group, _groupService);
            return isValid(group);
        }

        public bool ValidDeleteObject(Group group)
        {
            group.Errors.Clear();
            VDeleteObject(group);
            return isValid(group);
        }

        public bool isValid(Group obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Group obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
