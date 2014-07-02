﻿using Core.DomainModel;
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
    public class MachineValidator : IMachineValidator
    {
        public Machine VNameNotEmpty(Machine machine)
        {
            if (String.IsNullOrEmpty(machine.Name) || machine.Name.Trim() == "")
            {
                machine.Errors.Add("Name", "Tidak boleh kosong");
            }
            return machine;
        }

        public Machine VHasUniqueCode(Machine machine, IMachineService _machineService)
        {
            if (String.IsNullOrEmpty(machine.Code) || machine.Code.Trim() == "")
            {
                machine.Errors.Add("Code", "Tidak boleh kosong");
            }
            if (_machineService.IsCodeDuplicated(machine))
            {
                machine.Errors.Add("Code", "Tidak boleh diduplikasi");
            }
            return machine;
        }

        public Machine VIsInRollerBuilder(Machine machine, IRollerBuilderService _rollerBuilderService)
        {
            IList<RollerBuilder> list = _rollerBuilderService.GetObjectsByMachineId(machine.Id);
            if (list.Any())
            {
                machine.Errors.Add("Generic", "RollerBuilder tidak boleh ada yang terasosiakan dengan machine");
            }
            return machine;
        }

        public Machine VIsInCoreIdentificationDetail(Machine machine, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByMachineId(machine.Id);
            if (details.Any())
            {
                machine.Errors.Add("Generic", "CoreIdentificationDetail tidak boleh ada yang terasosiakan dengan machine");
            }
            return machine;
        }

        public Machine VCreateObject(Machine machine, IMachineService _machineService)
        {
            VNameNotEmpty(machine);
            if (!isValid(machine)) { return machine; }
            VHasUniqueCode(machine, _machineService);
            return machine;
        }

        public Machine VUpdateObject(Machine machine, IMachineService _machineService)
        {
            VCreateObject(machine, _machineService);
            return machine;
        }

        public Machine VDeleteObject(Machine machine, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            VIsInCoreIdentificationDetail(machine, _coreIdentificationDetailService);
            VIsInRollerBuilder(machine, _rollerBuilderService);
            return machine;
        }

        public bool ValidCreateObject(Machine machine, IMachineService _machineService)
        {
            VCreateObject(machine, _machineService);
            return isValid(machine);
        }

        public bool ValidUpdateObject(Machine machine, IMachineService _machineService)
        {
            machine.Errors.Clear();
            VUpdateObject(machine, _machineService);
            return isValid(machine);
        }

        public bool ValidDeleteObject(Machine machine, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            machine.Errors.Clear();
            VDeleteObject(machine, _rollerBuilderService, _coreIdentificationDetailService);
            return isValid(machine);
        }

        public bool isValid(Machine obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Machine obj)
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
