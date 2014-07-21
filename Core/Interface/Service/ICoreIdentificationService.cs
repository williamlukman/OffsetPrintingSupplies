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
        IList<CoreIdentification> GetAllObjectsByWarehouseId(int WarehouseId);
        CoreIdentification GetObjectById(int Id);
        CoreIdentification CreateObject(CoreIdentification coreIdentification, ICustomerService _customerService);
        CoreIdentification CreateObjectForCustomer(int CustomerId, string Code, int Quantity, DateTime IdentifiedDate, int WarehouseId, ICustomerService _customerService);
        CoreIdentification CreateObjectForInHouse(string Code, int Quantity, DateTime IdentifiedDate, int WarehouseId, ICustomerService _customerService);
        CoreIdentification UpdateObject(CoreIdentification coreIdentification, ICustomerService _customerService);
        CoreIdentification SoftDeleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                            IRecoveryOrderService _recoveryOrderService);
        CoreIdentification ConfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService, IStockMutationService _stockMutationService,
                                         IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService,
                                         IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService);
        CoreIdentification UnconfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService, IStockMutationService _stockMutationService,
                                           IRecoveryOrderService _recoveryOrderService, ICoreBuilderService _coreBuilderService, IItemService _itemService,
                                           IWarehouseItemService _warehouseItemService, IBarringService _barringService);
        CoreIdentification CompleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService);
        void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(CoreIdentification coreIdentification);
    }
}