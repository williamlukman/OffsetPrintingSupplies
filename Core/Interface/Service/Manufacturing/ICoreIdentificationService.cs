using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICoreIdentificationService
    {
        ICoreIdentificationValidator GetValidator();
        IList<CoreIdentification> GetAll();
        IList<CoreIdentification> GetAllObjectsInHouse();
        IList<CoreIdentification> GetAllObjectsByContactId(int ContactId);
        IList<CoreIdentification> GetAllObjectsByWarehouseId(int WarehouseId);
        IList<CoreIdentification> GetConfirmedObjects();
        CoreIdentification GetObjectById(int Id);
        CoreIdentification CreateObject(CoreIdentification coreIdentification, IContactService _contactService);
        CoreIdentification CreateObjectForContact(int ContactId, string Code, int Quantity, DateTime IdentifiedDate, int WarehouseId, IContactService _contactService);
        CoreIdentification CreateObjectForInHouse(string Code, int Quantity, DateTime IdentifiedDate, int WarehouseId, IContactService _contactService);
        CoreIdentification UpdateObject(CoreIdentification coreIdentification, IContactService _contactService);
        CoreIdentification SoftDeleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                            IRecoveryOrderService _recoveryOrderService);
        CoreIdentification ConfirmObject(CoreIdentification coreIdentification, DateTime ConfirmationDate, ICoreIdentificationDetailService _coreIdentificationDetailService, IStockMutationService _stockMutationService,
                                         IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService,
                                         IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService);
        CoreIdentification UnconfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService, IStockMutationService _stockMutationService,
                                           IRecoveryOrderService _recoveryOrderService, ICoreBuilderService _coreBuilderService, IItemService _itemService,
                                           IWarehouseItemService _warehouseItemService, IBarringService _barringService);
        CoreIdentification CompleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(CoreIdentification coreIdentification);
    }
}