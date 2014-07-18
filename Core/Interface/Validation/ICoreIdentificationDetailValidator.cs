using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ICoreIdentificationDetailValidator
    {
        CoreIdentificationDetail VHasCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService);
        CoreIdentificationDetail VHasUniqueDetailId(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationDetailService _coreIdentificationDetailService);
        CoreIdentificationDetail VHasMaterialCase(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VHasCoreBuilder(CoreIdentificationDetail coreIdentificationDetail, ICoreBuilderService _coreBuilderService);
        CoreIdentificationDetail VHasRollerType(CoreIdentificationDetail coreIdentificationDetail, IRollerTypeService _rollerTypeService);
        CoreIdentificationDetail VHasMachine(CoreIdentificationDetail coreIdentificationDetail, IMachineService _machineService);
        CoreIdentificationDetail VHasMeasurement(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VIsInRecoveryOrderDetails(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderDetailService _recoveryOrderDetailService);        
        CoreIdentificationDetail VCoreIdentificationHasNotBeenConfirmed(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService);
        CoreIdentificationDetail VIsJobScheduled(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VIsDelivered(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VCreateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                IRollerTypeService _rollerTypeService, IMachineService _machineService);
        CoreIdentificationDetail VUpdateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                IRollerTypeService _rollerTypeService, IMachineService _machineService);
        CoreIdentificationDetail VDeleteObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                               IRecoveryOrderDetailService _recoveryOrderDetailService);
        CoreIdentificationDetail VFinishObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                               IRecoveryOrderDetailService _recoveryOrderDetailService);
        bool ValidCreateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                               IRollerTypeService _rollerTypeService, IMachineService _machineService);
        bool ValidUpdateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                               IRollerTypeService _rollerTypeService, IMachineService _machineService);
        bool ValidDeleteObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                               IRecoveryOrderDetailService _recoveryOrderDetailService);
        bool ValidFinishObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                               IRecoveryOrderDetailService _recoveryOrderDetailService);
        bool isValid(CoreIdentificationDetail coreIdentificationDetail);
        string PrintError(CoreIdentificationDetail coreIdentificationDetail);
    }
}