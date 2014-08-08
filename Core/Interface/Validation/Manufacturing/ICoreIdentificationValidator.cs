using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ICoreIdentificationValidator
    { 
        CoreIdentification VHasUniqueCode(CoreIdentification coreIdentification, ICoreIdentificationService _coreIdentificationService);
        CoreIdentification VInHouseOrHasContact(CoreIdentification coreIdentification, IContactService _contactService);
        CoreIdentification VQuantity(CoreIdentification coreIdentification);
        CoreIdentification VHasIdentifiedDate(CoreIdentification coreIdentification);
        CoreIdentification VHasNotBeenConfirmed(CoreIdentification coreIdentification);
        CoreIdentification VHasBeenConfirmed(CoreIdentification coreIdentification);
        CoreIdentification VIsInRecoveryOrder(CoreIdentification coreIdentification, IRecoveryOrderService _recoveryOrderService);
        CoreIdentification VAllDetailsHaveBeenDelivered(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService);
        CoreIdentification VHasCoreIdentificationDetails(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService);
        CoreIdentification VQuantityEqualNumberOfDetails(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService);
        CoreIdentification VQuantityIsInStock(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                              ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService);
        CoreIdentification VCreateObject(CoreIdentification coreIdentification, ICoreIdentificationService _coreIdentificationService, IContactService _contactService);
        CoreIdentification VUpdateObject(CoreIdentification coreIdentification, ICoreIdentificationService _coreIdentificationService, IContactService _contactService);
        CoreIdentification VDeleteObject(CoreIdentification coreIdentification, IRecoveryOrderService _recoveryOrderService);
        CoreIdentification VHasConfirmationDate(CoreIdentification coreIdentification);
        CoreIdentification VConfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                          ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService);
        CoreIdentification VUnconfirmObject(CoreIdentification coreIdentification, IRecoveryOrderService _recoveryOrderService);
        CoreIdentification VCompleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService);
        bool ValidCreateObject(CoreIdentification coreIdentification, ICoreIdentificationService _coreIdentificationService, IContactService _contactService);
        bool ValidUpdateObject(CoreIdentification coreIdentification, ICoreIdentificationService _coreIdentificationService, IContactService _contactService);
        bool ValidDeleteObject(CoreIdentification coreIdentification, IRecoveryOrderService _recoveryOrderService);
        bool ValidConfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(CoreIdentification coreIdentification, IRecoveryOrderService _recoveryOrderService);
        bool ValidCompleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService);
        bool isValid(CoreIdentification coreIdentification);
        string PrintError(CoreIdentification coreIdentification);
    }
}