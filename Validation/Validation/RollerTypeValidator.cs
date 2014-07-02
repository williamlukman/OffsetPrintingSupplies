using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Validation.Validation
{
    public class RollerTypeValidator : IRollerTypeValidator
    {
        public RollerType VHasUniqueName(RollerType rollerType, IRollerTypeService _rollerTypeService)
        {
            if (String.IsNullOrEmpty(rollerType.Name) || rollerType.Name.Trim() == "")
            {
                rollerType.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_rollerTypeService.IsNameDuplicated(rollerType))
            {
                rollerType.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return rollerType;
        }

        public RollerType VIsInRollerBuilder(RollerType rollerType, IRollerBuilderService _rollerBuilderService)
        {
            IList<RollerBuilder> list = _rollerBuilderService.GetObjectsByRollerTypeId(rollerType.Id);
            if (list.Any())
            {
                rollerType.Errors.Add("Generic", "RollerBuilder tidak boleh ada yang terasosiakan dengan rollerType");
            }
            return rollerType;
        }

        public RollerType VIsInCoreIdentificationDetail(RollerType rollerType, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByRollerTypeId(rollerType.Id);
            if (details.Any())
            {
                rollerType.Errors.Add("Generic", "CoreIdentificationDetail tidak boleh ada yang terasosiakan dengan rollerType");
            }
            return rollerType;
        }

        public RollerType VCreateObject(RollerType rollerType, IRollerTypeService _rollerTypeService)
        {
            VHasUniqueName(rollerType, _rollerTypeService);
            return rollerType;
        }

        public RollerType VUpdateObject(RollerType rollerType, IRollerTypeService _rollerTypeService)
        {
            VHasUniqueName(rollerType, _rollerTypeService);
            return rollerType;
        }

        public RollerType VDeleteObject(RollerType rollerType, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            VIsInCoreIdentificationDetail(rollerType, _coreIdentificationDetailService);
            VIsInRollerBuilder(rollerType, _rollerBuilderService);
            return rollerType;
        }

        public bool ValidCreateObject(RollerType rollerType, IRollerTypeService _rollerTypeService)
        {
            VCreateObject(rollerType, _rollerTypeService);
            return isValid(rollerType);
        }

        public bool ValidUpdateObject(RollerType rollerType, IRollerTypeService _rollerTypeService)
        {
            rollerType.Errors.Clear();
            VUpdateObject(rollerType, _rollerTypeService);
            return isValid(rollerType);
        }

        public bool ValidDeleteObject(RollerType rollerType, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            rollerType.Errors.Clear();
            VDeleteObject(rollerType, _rollerBuilderService, _coreIdentificationDetailService);
            return isValid(rollerType);
        }

        public bool isValid(RollerType obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RollerType obj)
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
