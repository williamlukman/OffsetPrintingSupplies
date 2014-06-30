using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface ICoreIdentificationService
    {
        ICoreIdentificationValidator GetValidator();
        IList<CoreIdentification> GetAll();
        IList<CoreIdentification> GetAllObjectsInHouse();
        IList<CoreIdentification> GetAllObjectsByCustomerId(int CustomerId);
        CoreIdentification GetObjectById(int Id);
        CoreIdentification CreateObject(CoreIdentification coreIdentification);
        CoreIdentification CreateObjectForCustomer(int customerId, int Quantity, DateTime IdentifiedDate);
        CoreIdentification CreateObjectForInHouse(int Quantity, DateTime IdentifiedDate);
        CoreIdentification UpdateObject(CoreIdentification coreIdentification);
        CoreIdentification SoftDeleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                            IRecoveryOrderService _recoveryOrderService);
        CoreIdentification ConfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService);
        CoreIdentification UnconfirmObject(CoreIdentification coreIdentification, IRecoveryOrderService _recoveryOrderService);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(CoreIdentification coreIdentification);
    }
}