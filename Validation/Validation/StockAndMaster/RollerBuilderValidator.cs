using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class RollerBuilderValidator : IRollerBuilderValidator
    {
        public RollerBuilder VHasUniqueBaseSku(RollerBuilder rollerBuilder, IRollerBuilderService _rollerBuilderService)
        {
            if (String.IsNullOrEmpty(rollerBuilder.BaseSku) || rollerBuilder.BaseSku.Trim() == "")
            {
                rollerBuilder.Errors.Add("BaseSku", "Tidak boleh kosong");
            }
            else if (_rollerBuilderService.IsBaseSkuDuplicated(rollerBuilder))
            {
                rollerBuilder.Errors.Add("BaseSku", "Tidak boleh diduplikasi");
            }
            return rollerBuilder;
        }

        public RollerBuilder VNameNotEmpty(RollerBuilder rollerBuilder)
        {
            if (String.IsNullOrEmpty(rollerBuilder.Name) || rollerBuilder.Name.Trim() == "")
            {
                rollerBuilder.Errors.Add("Name", "Tidak boleh kosong");
            }
            return rollerBuilder;
        }

        public RollerBuilder VHasMachine(RollerBuilder rollerBuilder, IMachineService _machineService)
        {
            Machine machine = _machineService.GetObjectById(rollerBuilder.MachineId);
            if (machine == null)
            {
                rollerBuilder.Errors.Add("MachineId", "Tidak terasosiasi dengan machine");
            }
            return rollerBuilder;
        }

        public RollerBuilder VHasCompound(RollerBuilder rollerBuilder, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(rollerBuilder.CompoundId);
            if (item == null)
            {
                rollerBuilder.Errors.Add("CompoundId", "Tidak terasosiasi dengan compound");
            }
            return rollerBuilder;
        }

        public RollerBuilder VHasCoreBuilder(RollerBuilder rollerBuilder, ICoreBuilderService _coreBuilderService)
        {
            CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(rollerBuilder.CoreBuilderId);
            if (coreBuilder == null)
            {
                rollerBuilder.Errors.Add("CoreBuilderId", "Tidak terasosiasi dengan coreBuilder");
            }
            return rollerBuilder;
        }

        public RollerBuilder VHasRollerType(RollerBuilder rollerBuilder, IRollerTypeService _rollerTypeService)
        {
            RollerType rollerType = _rollerTypeService.GetObjectById(rollerBuilder.RollerTypeId);
            if (rollerType == null)
            {
                rollerBuilder.Errors.Add("RollerTypeId", "Tidak terasosiasi dengan rollerType");
            }
            return rollerBuilder;
        }

        public RollerBuilder VHasRollerUsedCoreItem(RollerBuilder rollerBuilder, IItemService _itemService)
        {
            Item RollerUsedCoreItem = _itemService.GetObjectById(rollerBuilder.RollerUsedCoreItemId);
            if (RollerUsedCoreItem == null)
            {
                rollerBuilder.Errors.Add("RollerUsedCoreItemId", "Tidak terasosiasi dengan Item");
            }
            return rollerBuilder;
        }

        public RollerBuilder VHasRollerNewCoreItem(RollerBuilder rollerBuilder, IItemService _itemService)
        {
            Item RollerNewCoreItem = _itemService.GetObjectById(rollerBuilder.RollerNewCoreItemId);
            if (RollerNewCoreItem == null)
            {
                rollerBuilder.Errors.Add("RollerNewCoreItemId", "Tidak terasosiasi dengan Item");
            }
            return rollerBuilder;
        }

        public RollerBuilder VHasMeasurement(RollerBuilder rollerBuilder)
        {
            if (rollerBuilder.CD <= 0) { rollerBuilder.Errors.Add("CD", "Tidak boleh 0 atau negatif"); return rollerBuilder; }
            if (rollerBuilder.RD <= 0) { rollerBuilder.Errors.Add("RD", "Tidak boleh 0 atau negatif"); return rollerBuilder; }
            if (rollerBuilder.RL <= 0) { rollerBuilder.Errors.Add("RL", "Tidak boleh 0 atau negatif"); return rollerBuilder; }
            if (rollerBuilder.WL <= 0) { rollerBuilder.Errors.Add("WL", "Tidak boleh 0 atau negatif"); return rollerBuilder; }
            if (rollerBuilder.TL <= 0) { rollerBuilder.Errors.Add("TL", "Tidak boleh 0 atau negatif"); return rollerBuilder; }
            return rollerBuilder;
        }

        public RollerBuilder VHasUoM(RollerBuilder rollerBuilder, IUoMService _uomService)
        {
            UoM uom = _uomService.GetObjectById(rollerBuilder.UoMId);
            if (uom == null)
            {
                rollerBuilder.Errors.Add("UoMId", "Tidak terasosiasi dengan unit of measurement");
            }
            return rollerBuilder;
        }

        public RollerBuilder VIsInRecoveryOrderDetails(RollerBuilder rollerBuilder, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRollerBuilderId(rollerBuilder.Id);
            if (details.Any())
            {
                rollerBuilder.Errors.Add("Generic", "Tidak boleh memiliki asosiasi Recovery Order Detail");
            }
            return rollerBuilder;
        }
 
        public RollerBuilder VCreateObject(RollerBuilder rollerBuilder, IRollerBuilderService _rollerBuilderService, IMachineService _machineService, IUoMService _uomService,
                                    IItemService _itemService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService)
        {
            VHasUniqueBaseSku(rollerBuilder, _rollerBuilderService);
            if (!isValid(rollerBuilder)) { return rollerBuilder; }
            VNameNotEmpty(rollerBuilder);
            if (!isValid(rollerBuilder)) { return rollerBuilder; }
            VHasCompound(rollerBuilder, _itemService);
            if (!isValid(rollerBuilder)) { return rollerBuilder; }
            VHasCoreBuilder(rollerBuilder, _coreBuilderService);
            if (!isValid(rollerBuilder)) { return rollerBuilder; }
            VHasRollerType(rollerBuilder, _rollerTypeService);
            if (!isValid(rollerBuilder)) { return rollerBuilder; }
            VHasMachine(rollerBuilder, _machineService);
            if (!isValid(rollerBuilder)) { return rollerBuilder; }
            VHasMeasurement(rollerBuilder);
            if (!isValid(rollerBuilder)) { return rollerBuilder; }
            VHasUoM(rollerBuilder, _uomService);
            return rollerBuilder;
        }

        public RollerBuilder VUpdateObject(RollerBuilder rollerBuilder, IRollerBuilderService _rollerBuilderService, IMachineService _machineService, IUoMService _uomService,
                                    IItemService _itemService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService)
        {
            VCreateObject(rollerBuilder, _rollerBuilderService, _machineService, _uomService, _itemService, _coreBuilderService, _rollerTypeService);
            if (!isValid(rollerBuilder)) { return rollerBuilder; }
            VHasRollerUsedCoreItem(rollerBuilder, _itemService);
            if (!isValid(rollerBuilder)) { return rollerBuilder; }
            VHasRollerNewCoreItem(rollerBuilder, _itemService);
            return rollerBuilder;
        }

        public RollerBuilder VDeleteObject(RollerBuilder rollerBuilder, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            VIsInRecoveryOrderDetails(rollerBuilder, _recoveryOrderDetailService);
            return rollerBuilder;
        }

        public bool ValidCreateObject(RollerBuilder rollerBuilder, IRollerBuilderService _rollerBuilderService, IMachineService _machineService, IUoMService _uomService,
                                    IItemService _itemService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService)
        {
            VCreateObject(rollerBuilder, _rollerBuilderService, _machineService, _uomService, _itemService, _coreBuilderService, _rollerTypeService);
            return isValid(rollerBuilder);
        }

        public bool ValidUpdateObject(RollerBuilder rollerBuilder, IRollerBuilderService _rollerBuilderService, IMachineService _machineService, IUoMService _uomService,
                                    IItemService _itemService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService)
        {
            rollerBuilder.Errors.Clear();
            VUpdateObject(rollerBuilder, _rollerBuilderService, _machineService, _uomService, _itemService, _coreBuilderService, _rollerTypeService);
            return isValid(rollerBuilder);
        }

        public bool ValidDeleteObject(RollerBuilder rollerBuilder, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            rollerBuilder.Errors.Clear();
            VDeleteObject(rollerBuilder, _recoveryOrderDetailService);
            return isValid(rollerBuilder);
        }

        public bool isValid(RollerBuilder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RollerBuilder obj)
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
