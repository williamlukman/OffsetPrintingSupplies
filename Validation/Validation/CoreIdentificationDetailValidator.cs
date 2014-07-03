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
    public class CoreIdentificationDetailValidator : ICoreIdentificationDetailValidator
    {

        public CoreIdentificationDetail VHasCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
            if (coreIdentification == null)
            {
                coreIdentificationDetail.Errors.Add("Generic", "CoreIdentificationDetail harus memiliki Core Identification");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasUniqueDetailId(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentificationDetail.CoreIdentificationId);
            foreach (var detail in details)
            {
                if (detail.DetailId == coreIdentificationDetail.DetailId && detail.Id != coreIdentificationDetail.Id)
                {
                    coreIdentificationDetail.Errors.Add("DetailId", "Tidak boleh di duplikasi");
                    return coreIdentificationDetail;
                }
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasMaterialCase(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (coreIdentificationDetail.MaterialCase != Core.Constants.Constant.MaterialCase.New &&
                coreIdentificationDetail.MaterialCase != Core.Constants.Constant.MaterialCase.Used)
            {
                coreIdentificationDetail.Errors.Add("MaterialCase", "Hanya boleh 1. New atau 2. Used");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasCoreBuilder(CoreIdentificationDetail coreIdentificationDetail, ICoreBuilderService _coreBuilderService)
        {
            CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
            if (coreBuilder == null)
            {
                coreIdentificationDetail.Errors.Add("CoreBuilderId", "Tidak terasosiasi dengan coreBuilder");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasRollerType(CoreIdentificationDetail coreIdentificationDetail, IRollerTypeService _rollerTypeService)
        {
            RollerType rollerType = _rollerTypeService.GetObjectById(coreIdentificationDetail.RollerTypeId);
            if (rollerType == null)
            {
                coreIdentificationDetail.Errors.Add("RollerTypeId", "Tidak terasosiasi dengan rollerType");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasMachine(CoreIdentificationDetail coreIdentificationDetail, IMachineService _machineService)
        {
            Machine machine = _machineService.GetObjectById(coreIdentificationDetail.MachineId);
            if (machine == null)
            {
                coreIdentificationDetail.Errors.Add("MachineId", "Tidak terasosiasi dengan machine");
            }
            return coreIdentificationDetail;
        }
        
        public CoreIdentificationDetail VHasMeasurement(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (coreIdentificationDetail.CD <= 0) { coreIdentificationDetail.Errors.Add("CD", "Tidak boleh 0 atau negatif"); return coreIdentificationDetail; }
            if (coreIdentificationDetail.RD <= 0) { coreIdentificationDetail.Errors.Add("RD", "Tidak boleh 0 atau negatif"); return coreIdentificationDetail; }
            if (coreIdentificationDetail.RL <= 0) { coreIdentificationDetail.Errors.Add("RL", "Tidak boleh 0 atau negatif"); return coreIdentificationDetail; }
            if (coreIdentificationDetail.WL <= 0) { coreIdentificationDetail.Errors.Add("WL", "Tidak boleh 0 atau negatif"); return coreIdentificationDetail; }
            if (coreIdentificationDetail.TL <= 0) { coreIdentificationDetail.Errors.Add("TL", "Tidak boleh 0 atau negatif"); return coreIdentificationDetail; }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VIsInRecoveryOrderDetails(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByCoreIdentificationDetailId(coreIdentificationDetail.Id);
            if (details.Any())
            {
                coreIdentificationDetail.Errors.Add("Generic", "Tidak boleh memiliki asosiasi Recovery Order Detail");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VCoreIdentificationHasNotBeenConfirmed(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
            if (coreIdentification.IsConfirmed)
            {
                coreIdentificationDetail.Errors.Add("Generic", "CoreIdentification harus belum dikonfirmasi");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VCreateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                      ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                      IRollerTypeService _rollerTypeService, IMachineService _machineService)
        {
            VHasCoreIdentification(coreIdentificationDetail, _coreIdentificationService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasUniqueDetailId(coreIdentificationDetail, _coreIdentificationDetailService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasMaterialCase(coreIdentificationDetail);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasCoreBuilder(coreIdentificationDetail, _coreBuilderService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasMachine(coreIdentificationDetail, _machineService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasMeasurement(coreIdentificationDetail);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasRollerType(coreIdentificationDetail, _rollerTypeService);
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VUpdateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                      ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                      IRollerTypeService _rollerTypeService, IMachineService _machineService)
        {
            VCreateObject(coreIdentificationDetail, _coreIdentificationService, _coreIdentificationDetailService, _coreBuilderService, _rollerTypeService, _machineService);
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VDeleteObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                      IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            VCoreIdentificationHasNotBeenConfirmed(coreIdentificationDetail, _coreIdentificationService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VIsInRecoveryOrderDetails(coreIdentificationDetail, _recoveryOrderDetailService);
            return coreIdentificationDetail;
        }

        public bool ValidCreateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                      ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                      IRollerTypeService _rollerTypeService, IMachineService _machineService)
        {
            VCreateObject(coreIdentificationDetail, _coreIdentificationService, _coreIdentificationDetailService, _coreBuilderService, _rollerTypeService, _machineService);
            return isValid(coreIdentificationDetail);
        }

        public bool ValidUpdateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                      ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                      IRollerTypeService _rollerTypeService, IMachineService _machineService)
        {
            coreIdentificationDetail.Errors.Clear();
            VUpdateObject(coreIdentificationDetail, _coreIdentificationService, _coreIdentificationDetailService, _coreBuilderService, _rollerTypeService, _machineService);
            return isValid(coreIdentificationDetail);
        }

        public bool ValidDeleteObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                      IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            coreIdentificationDetail.Errors.Clear();
            VDeleteObject(coreIdentificationDetail, _coreIdentificationService, _recoveryOrderDetailService);
            return isValid(coreIdentificationDetail);
        }

        public bool isValid(CoreIdentificationDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CoreIdentificationDetail obj)
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
