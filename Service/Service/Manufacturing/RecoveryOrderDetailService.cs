using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class RecoveryOrderDetailService : IRecoveryOrderDetailService
    {
        private IRecoveryOrderDetailRepository _repository;
        private IRecoveryOrderDetailValidator _validator;
        public RecoveryOrderDetailService(IRecoveryOrderDetailRepository _recoveryOrderDetailRepository, IRecoveryOrderDetailValidator _recoveryOrderDetailValidator)
        {
            _repository = _recoveryOrderDetailRepository;
            _validator = _recoveryOrderDetailValidator;
        }

        public IRecoveryOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IRecoveryOrderDetailRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<RecoveryOrderDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<RecoveryOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RecoveryOrderDetail> GetObjectsByRecoveryOrderId(int recoveryOrderId)
        {
            return _repository.GetObjectsByRecoveryOrderId(recoveryOrderId);
        }

        public IList<RecoveryOrderDetail> GetObjectsByCoreIdentificationDetailId(int coreIdentificationDetailId)
        {
            return _repository.GetObjectsByCoreIdentificationDetailId(coreIdentificationDetailId);
        }

        public IList<RecoveryOrderDetail> GetObjectsByRollerBuilderId(int rollerBuilderId)
        {
            return _repository.GetObjectsByRollerBuilderId(rollerBuilderId);
        }

        public RecoveryOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Item GetCore(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService,
                            ICoreBuilderService _coreBuilderService, IItemService _itemService)
        {
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
            Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                          _coreBuilderService.GetNewCore(coreIdentificationDetail.CoreBuilderId) : _coreBuilderService.GetUsedCore(coreIdentificationDetail.CoreBuilderId);
            return core;
        }

        public Item GetRoller(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService,
                              IRollerBuilderService _rollerBuilderService, IItemService _itemService)
        {
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
            Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                          _rollerBuilderService.GetRollerNewCore(recoveryOrderDetail.RollerBuilderId) : _rollerBuilderService.GetRollerUsedCore(recoveryOrderDetail.RollerBuilderId);
            return roller;
        }

        public RecoveryOrderDetail CreateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService)
        {
            recoveryOrderDetail.Errors = new Dictionary<String, String>();
            return (recoveryOrderDetail = _validator.ValidCreateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService) ?
                                          _repository.CreateObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail UpdateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService)
        {
            return (recoveryOrderDetail = _validator.ValidUpdateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService) ?
                                          _repository.UpdateObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail SoftDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            return (recoveryOrderDetail = _validator.ValidDeleteObject(recoveryOrderDetail, _recoveryOrderService, _recoveryAccessoryDetailService) ?
                                          _repository.SoftDeleteObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail DisassembleObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            return (recoveryOrderDetail = _validator.ValidDisassembleObject(recoveryOrderDetail, _recoveryOrderService) ? _repository.DisassembleObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail StripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidStripAndGlueObject(recoveryOrderDetail) ? _repository.StripAndGlueObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail WrapObject(RecoveryOrderDetail recoveryOrderDetail, int CompoundUsage, IRecoveryOrderService _recoveryOrderService,
                                              IRollerBuilderService _rollerBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            recoveryOrderDetail.CompoundUsage = CompoundUsage;
            return (recoveryOrderDetail = _validator.ValidWrapObject(recoveryOrderDetail, _recoveryOrderService, _rollerBuilderService, _itemService, _warehouseItemService) ? _repository.WrapObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail VulcanizeObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidVulcanizeObject(recoveryOrderDetail) ? _repository.VulcanizeObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail FaceOffObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidFaceOffObject(recoveryOrderDetail) ? _repository.FaceOffObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail ConventionalGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidConventionalGrindObject(recoveryOrderDetail) ? _repository.ConventionalGrindObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail CNCGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidCNCGrindObject(recoveryOrderDetail) ? _repository.CNCGrindObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail PolishAndQCObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidPolishAndQCObject(recoveryOrderDetail) ? _repository.PolishAndQCObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail PackageObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidPackageObject(recoveryOrderDetail) ? _repository.PackageObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail RejectObject(RecoveryOrderDetail recoveryOrderDetail, DateTime RejectedDate, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                                IItemService _itemService, IWarehouseItemService _warehouseItemService, IBlanketService _blanketService, IStockMutationService _stockMutationService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            recoveryOrderDetail.RejectedDate = RejectedDate;
            if (_validator.ValidRejectObject(recoveryOrderDetail, _recoveryOrderService))
            {
                // calculate total cost to produce the finished goods, then create general ledger with total cost                
                CalculateTotalCost(recoveryOrderDetail, _recoveryAccessoryDetailService, _coreIdentificationDetailService, _coreIdentificationService, _coreBuilderService,
                                   this, _rollerBuilderService, _itemService);
                _generalLedgerJournalService.CreateRejectedJournalForRecoveryOrderDetail(recoveryOrderDetail, _accountService);
                _repository.RejectObject(recoveryOrderDetail);

                // add recovery order quantity reject
                // if valid, complete recovery order = true 
                RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
                recoveryOrder.QuantityRejected += 1;
                _recoveryOrderService.AdjustQuantity(recoveryOrder);
                if (_recoveryOrderService.GetValidator().ValidCompleteObject(recoveryOrder, this, _recoveryAccessoryDetailService))
                {
                    _recoveryOrderService.CompleteObject(recoveryOrder, _coreIdentificationDetailService, this, _recoveryAccessoryDetailService);
                }

                bool CaseAddition = false;

                // deduce compound
                RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
                Item compound = _itemService.GetObjectById(rollerBuilder.CompoundId);
                WarehouseItem warehouseCompound = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, compound.Id);
                StockMutation stockMutationCompound = _stockMutationService.CreateStockMutationForRecoveryOrderCompound(recoveryOrderDetail, RejectedDate, warehouseCompound, CaseAddition);
                _stockMutationService.StockMutateObject(stockMutationCompound, _itemService, _blanketService, _warehouseItemService);

                // deduce core
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.UnsetJobScheduled(coreIdentificationDetail, _recoveryOrderService, this);

                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                WarehouseItem warehouseCore = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, core.Id);
                StockMutation stockMutationCore = _stockMutationService.CreateStockMutationForRecoveryOrder(recoveryOrderDetail, RejectedDate, warehouseCore, CaseAddition);
                _stockMutationService.StockMutateObject(stockMutationCore, _itemService, _blanketService, _warehouseItemService);
               
                // accesories uncounted
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail UndoRejectObject(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                    IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                                    IItemService _itemService, IWarehouseItemService _warehouseItemService, IBlanketService _blanketService, IStockMutationService _stockMutationService,
                                                    IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUndoRejectObject(recoveryOrderDetail, _recoveryOrderService))
            {
                // undo reject general ledger with old total cost, then set total cost to 0
                _generalLedgerJournalService.CreateUndoRejectedJournalForRecoveryOrderDetail(recoveryOrderDetail, _accountService);
                recoveryOrderDetail.TotalCost = 0;
                _repository.UndoRejectObject(recoveryOrderDetail);

                // add recovery order quantity reject
                // if valid, complete recovery order = true 
                RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
                recoveryOrder.QuantityRejected -= 1;
                _recoveryOrderService.AdjustQuantity(recoveryOrder);
                // no longer Completed
                _recoveryOrderService.UncompleteObject(recoveryOrder, _coreIdentificationDetailService, this, _recoveryAccessoryDetailService);

                // reverse stock mutate compound
                RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
                Item compound = _itemService.GetObjectById(rollerBuilder.CompoundId);
                WarehouseItem warehouseCompound = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, compound.Id);
                IList<StockMutation> stockMutationCompounds = _stockMutationService.DeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCompound);
                foreach (var stockMutationCompound in stockMutationCompounds)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationCompound, _itemService, _blanketService, _warehouseItemService);
                }

                // reverse stock mutate core
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.SetJobScheduled(coreIdentificationDetail, _recoveryOrderService, this);

                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                WarehouseItem warehouseCore = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, core.Id);
                
                IList<StockMutation> stockMutationCores = _stockMutationService.DeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCore);
                foreach (var stockMutationCore in stockMutationCores)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationCore, _itemService, _blanketService, _warehouseItemService);
                }

                // accesories uncounted
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail FinishObject(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedDate, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                                IItemService _itemService, IWarehouseItemService _warehouseItemService, IBlanketService _blanketService, IStockMutationService _stockMutationService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, IServiceCostService _serviceCostService, 
                                                ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService)
        {
            recoveryOrderDetail.FinishedDate = FinishedDate;
            if (_validator.ValidFinishObject(recoveryOrderDetail, _recoveryOrderService, _recoveryAccessoryDetailService))
            {
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);

                // calculate total cost to produce the finished goods, then create general ledger with total cost
                CalculateTotalCost(recoveryOrderDetail, _recoveryAccessoryDetailService, _coreIdentificationDetailService, _coreIdentificationService, _coreBuilderService,
                                   this, _rollerBuilderService, _itemService);
                _generalLedgerJournalService.CreateFinishedJournalForRecoveryOrderDetail(recoveryOrderDetail, _accountService);

                if (!coreIdentification.IsInHouse)
                {
                    ServiceCost serviceCost = _serviceCostService.FindOrCreateObject(recoveryOrderDetail.RollerBuilderId);
                    _serviceCostService.CalculateAndUpdateAvgPrice(serviceCost, 1, recoveryOrderDetail.TotalCost);
                }
                // set object to finish
                _repository.FinishObject(recoveryOrderDetail);

                // add recovery order quantity final
                // if valid, complete recovery order = true 
                RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
                recoveryOrder.QuantityFinal += 1;
                _recoveryOrderService.AdjustQuantity(recoveryOrder);
                if (_recoveryOrderService.GetValidator().ValidCompleteObject(recoveryOrder, this, _recoveryAccessoryDetailService))
                {
                    _recoveryOrderService.CompleteObject(recoveryOrder, _coreIdentificationDetailService, this, _recoveryAccessoryDetailService);
                }

                bool CaseAdditionCompound = false;
                bool CaseAdditionCore = false;
                bool CaseAdditionRoller = true;
             
                // deduce compound
                RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
                Item compound = _itemService.GetObjectById(rollerBuilder.CompoundId);
                WarehouseItem warehouseCompound = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, compound.Id);
                StockMutation stockMutationCompound = _stockMutationService.CreateStockMutationForRecoveryOrderCompound(recoveryOrderDetail, FinishedDate, warehouseCompound, CaseAdditionCompound);
                _stockMutationService.StockMutateObject(stockMutationCompound, _itemService, _blanketService, _warehouseItemService);

                _coreIdentificationDetailService.UnsetJobScheduled(coreIdentificationDetail, _recoveryOrderService, this);
                _coreIdentificationDetailService.BuildRoller(coreIdentificationDetail);

                // deduce core
                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                if (coreIdentification.IsInHouse)
                {
                    WarehouseItem warehouseCore = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, core.Id);
                    StockMutation stockMutationCore = _stockMutationService.CreateStockMutationForRecoveryOrder(recoveryOrderDetail, FinishedDate, warehouseCore, CaseAdditionCore);
                    _stockMutationService.StockMutateObject(stockMutationCore, _itemService, _blanketService, _warehouseItemService);
                } 
                else // if (!coreIdentification.IsInHouse)
                {
                    CustomerItem customerCore = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), core.Id);
                    CustomerStockMutation customerStockMutationCore = _customerStockMutationService.CreateCustomerStockMutationForRecoveryOrder(recoveryOrderDetail, FinishedDate, customerCore, CaseAdditionCore);
                    _customerStockMutationService.CreateObject(customerStockMutationCore, _customerItemService, _itemService);
                    _customerStockMutationService.StockMutateObject(customerStockMutationCore, coreIdentification.IsInHouse, _itemService, _customerItemService);
                }

                // add roller
                Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id) : _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
                if (coreIdentification.IsInHouse)
                {
                    WarehouseItem warehouseRoller = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, roller.Id);
                    StockMutation stockMutationRoller = _stockMutationService.CreateStockMutationForRecoveryOrder(recoveryOrderDetail, FinishedDate, warehouseRoller, CaseAdditionRoller);
                    // update avg cost for roller before mutated
                    _itemService.CalculateAndUpdateAvgPrice(roller, stockMutationRoller.Quantity, recoveryOrderDetail.TotalCost);
                    _stockMutationService.StockMutateObject(stockMutationRoller, _itemService, _blanketService, _warehouseItemService);
                } 
                else // if (!coreIdentification.IsInHouse)
                {
                    CustomerItem customerRoller = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), roller.Id);
                    CustomerStockMutation customerStockMutationRoller = _customerStockMutationService.CreateCustomerStockMutationForRecoveryOrder(recoveryOrderDetail, FinishedDate, customerRoller, CaseAdditionRoller);
                    _customerStockMutationService.CreateObject(customerStockMutationRoller, _customerItemService, _itemService);
                    // update customer avg cost for roller before mutated
                    _itemService.CalculateAndUpdateCustomerAvgPrice(roller, customerStockMutationRoller.Quantity, recoveryOrderDetail.TotalCost);
                    _customerStockMutationService.StockMutateObject(customerStockMutationRoller, coreIdentification.IsInHouse, _itemService, _customerItemService);
                }

                // deduce accessories
                IList<RecoveryAccessoryDetail> recoveryAccessoryDetails = _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(recoveryOrderDetail.Id);
                if (recoveryAccessoryDetails.Any())
                {
                    foreach (var recoveryAccessoryDetail in recoveryAccessoryDetails)
                    {
                        Item accessory = _itemService.GetObjectById(recoveryAccessoryDetail.ItemId);
                        WarehouseItem warehouseAccessory = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, accessory.Id);
                        StockMutation stockMutationAccessory = _stockMutationService.CreateStockMutationForRecoveryAccessory(recoveryAccessoryDetail, FinishedDate, warehouseAccessory);
                        _stockMutationService.StockMutateObject(stockMutationAccessory, _itemService, _blanketService, _warehouseItemService);
                    }
                }
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail UnfinishObject(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                  IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                                  IItemService _itemService, IWarehouseItemService _warehouseItemService, IBlanketService _blanketService, IStockMutationService _stockMutationService,
                                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, IServiceCostService _serviceCostService,
                                                  ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService)
        {
            if (_validator.ValidUnfinishObject(recoveryOrderDetail, _recoveryOrderService, _recoveryAccessoryDetailService, _coreIdentificationService, _coreIdentificationDetailService, _rollerBuilderService, _customerItemService))
            {
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
                CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);

                if (!coreIdentification.IsInHouse)
                {
                    ServiceCost serviceCost = _serviceCostService.GetObjectByRollerBuilderId(recoveryOrderDetail.RollerBuilderId);
                    _serviceCostService.CalculateAndUpdateAvgPrice(serviceCost, -1, recoveryOrderDetail.TotalCost);
                }

                // unfinish general ledger with old total cost, then set total cost to 0
                _generalLedgerJournalService.CreateUnfinishedJournalForRecoveryOrderDetail(recoveryOrderDetail, _accountService);
                decimal totalcost = recoveryOrderDetail.TotalCost;
                recoveryOrderDetail.TotalCost = 0;

                // unfinish object
                _repository.UnfinishObject(recoveryOrderDetail);

                // add recovery order quantity final
                recoveryOrder.QuantityFinal += 1;
                _recoveryOrderService.AdjustQuantity(recoveryOrder);
                // no longer Completed
                _recoveryOrderService.UncompleteObject(recoveryOrder, _coreIdentificationDetailService, this, _recoveryAccessoryDetailService);

                // reverse stock mutate compound
                RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
                Item compound = _itemService.GetObjectById(rollerBuilder.CompoundId);
                WarehouseItem warehouseCompound = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, compound.Id);
                IList<StockMutation> stockMutationCompounds = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseCompound.Id, Constant.SourceDocumentDetailType.RecoveryOrderDetail, recoveryOrderDetail.Id);
                foreach (var stockMutationCompound in stockMutationCompounds)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationCompound, _itemService, _blanketService, _warehouseItemService);
                }
                _stockMutationService.DeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCompound);
                
                // reverse stock mutate core
                _coreIdentificationDetailService.SetJobScheduled(coreIdentificationDetail, _recoveryOrderService, this);

                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                if (coreIdentification.IsInHouse)
                {
                    WarehouseItem warehouseCore = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, core.Id);
                    IList<StockMutation> stockMutationCores = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseCore.Id, Constant.SourceDocumentDetailType.RecoveryOrderDetail, recoveryOrderDetail.Id);
                    foreach (var stockMutationCore in stockMutationCores)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutationCore, _itemService, _blanketService, _warehouseItemService);
                    }
                    _stockMutationService.DeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCore);
                }
                else
                {
                    CustomerItem customerCore = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), core.Id);
                    IList<CustomerStockMutation> customerStockMutationCores = _customerStockMutationService.GetObjectsBySourceDocumentDetailForCustomerItem(customerCore.Id, Constant.SourceDocumentDetailType.RecoveryOrderDetail, recoveryOrderDetail.Id);
                    foreach (var customerStockMutationCore in customerStockMutationCores)
                    {
                        _customerStockMutationService.ReverseStockMutateObject(customerStockMutationCore, coreIdentification.IsInHouse, _itemService, _customerItemService);
                    }
                    _customerStockMutationService.DeleteCustomerStockMutationForRecoveryOrder(recoveryOrderDetail, customerCore);
                }

                // reverse stock mutate roller
                Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id) : _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
                if (coreIdentification.IsInHouse)
                {
                    WarehouseItem warehouseRoller = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, roller.Id);
                    IList<StockMutation> stockMutationRollers = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseRoller.Id, Constant.SourceDocumentDetailType.RecoveryOrderDetail, recoveryOrderDetail.Id);
                    foreach (var stockMutationRoller in stockMutationRollers)
                    {
                        // update avg cost for roller before mutated
                        _itemService.CalculateAndUpdateAvgPrice(roller, (-1) * stockMutationRoller.Quantity, totalcost);
                        _stockMutationService.ReverseStockMutateObject(stockMutationRoller, _itemService, _blanketService, _warehouseItemService);
                    }
                    _stockMutationService.DeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseRoller);
                }
                else
                {
                    CustomerItem customerRoller = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), roller.Id);
                    IList<CustomerStockMutation> customerStockMutationRollers = _customerStockMutationService.GetObjectsBySourceDocumentDetailForCustomerItem(customerRoller.Id, Constant.SourceDocumentDetailType.RecoveryOrderDetail, recoveryOrderDetail.Id);
                    foreach (var customerStockMutationRoller in customerStockMutationRollers)
                    {
                        // update customer avg cost for roller before mutated
                        _itemService.CalculateAndUpdateCustomerAvgPrice(roller, (-1) * customerStockMutationRoller.Quantity, totalcost);
                        _customerStockMutationService.ReverseStockMutateObject(customerStockMutationRoller, coreIdentification.IsInHouse, _itemService, _customerItemService);
                    }
                    _customerStockMutationService.DeleteCustomerStockMutationForRecoveryOrder(recoveryOrderDetail, customerRoller);
                }

                // reverse stock mutate accessories
                IList<RecoveryAccessoryDetail> recoveryAccessoryDetails = _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(recoveryOrderDetail.Id);
                if (recoveryAccessoryDetails.Any())
                {
                    foreach (var recoveryAccessoryDetail in recoveryAccessoryDetails)
                    {
                        Item accessory = _itemService.GetObjectById(recoveryAccessoryDetail.ItemId);
                        WarehouseItem warehouseAccessory = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, accessory.Id);
                        IList<StockMutation> stockMutationAccessories = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseAccessory.Id, Constant.SourceDocumentDetailType.RecoveryAccessoryDetail, recoveryAccessoryDetail.Id);
                        foreach (var stockMutationAccessory in stockMutationAccessories)
                        {
                            _stockMutationService.ReverseStockMutateObject(stockMutationAccessory, _itemService, _blanketService, _warehouseItemService);
                        }
                        _stockMutationService.DeleteStockMutationForRecoveryAccessory(recoveryAccessoryDetail, warehouseAccessory);
                    }
                }
            }
            return recoveryOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public void CalculateTotalCost(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                       ICoreIdentificationService _coreIdentificationService, ICoreBuilderService _coreBuilderService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                       IRollerBuilderService _rollerBuilderService, IItemService _itemService)
        {
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
            RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
            Item Core = _coreIdentificationDetailService.GetCore(coreIdentificationDetail, _coreBuilderService);
            Item Compound = _itemService.GetObjectById(rollerBuilder.CompoundId);
            IList<RecoveryAccessoryDetail> Accessories = _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(recoveryOrderDetail.Id);
            decimal AccessoriesCost = 0;
            foreach (var accessory in Accessories)
            {
                Item item = _itemService.GetObjectById(accessory.ItemId);
                AccessoriesCost += item.AvgPrice * accessory.Quantity;
            }
            decimal TotalCost = 0;
            if (coreIdentification.IsInHouse)
            {
                TotalCost = Core.AvgPrice + (Compound.AvgPrice * recoveryOrderDetail.CompoundUsage) + AccessoriesCost;
            }
            else
            {
                TotalCost = (Compound.AvgPrice * recoveryOrderDetail.CompoundUsage) + AccessoriesCost;
            }
            recoveryOrderDetail.TotalCost = TotalCost;
            _repository.UpdateObject(recoveryOrderDetail);
            return;
        }
    }
}