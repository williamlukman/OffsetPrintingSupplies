﻿using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IRecoveryOrderDetailService
    {
        IRecoveryOrderDetailValidator GetValidator();
        IRecoveryOrderDetailRepository GetRepository();
        IQueryable<RecoveryOrderDetail> GetQueryable();
        IList<RecoveryOrderDetail> GetAll();
        IList<RecoveryOrderDetail> GetObjectsByRecoveryOrderId(int recoveryOrderId);
        IList<RecoveryOrderDetail> GetObjectsByCoreIdentificationDetailId(int coreIdentificationDetailId);
        IList<RecoveryOrderDetail> GetObjectsByRollerBuilderId(int rollerBuilderId);
        RecoveryOrderDetail GetObjectById(int Id);
        Item GetCore(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService,
                     ICoreBuilderService _coreBuilderService, IItemService _itemService);
        Item GetRoller(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService,
                       IRollerBuilderService _rollerBuilderService, IItemService _itemService);
        RecoveryOrderDetail CreateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService);
        RecoveryOrderDetail UpdateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService);
        RecoveryOrderDetail SoftDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                             IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);

        RecoveryOrderDetail DisassembleObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrderDetail StripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail WrapObject(RecoveryOrderDetail recoveryOrderDetail, decimal CompoundUsage, decimal CompoundUnderLayerUsage, IRecoveryOrderService _recoveryOrderService,
                                       IRollerBuilderService _rollerBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RecoveryOrderDetail VulcanizeObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail FaceOffObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail ConventionalGrindObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail CNCGrindObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail PolishAndQCObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail PackageObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail RejectObject(RecoveryOrderDetail recoveryOrderDetail, DateTime RejectedDate, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                         IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                         IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IBlanketService _blanketService, IStockMutationService _stockMutationService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        RecoveryOrderDetail UndoRejectObject(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                             IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                             IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IBlanketService _blanketService, IStockMutationService _stockMutationService,
                                             IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        RecoveryOrderDetail FinishObject(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedDate, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                         IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                         IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IBlanketService _blanketService, IStockMutationService _stockMutationService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, IServiceCostService _serviceCostService,
                                         ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService);
        RecoveryOrderDetail UnfinishObject(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                           IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                           IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IBlanketService _blanketService, IStockMutationService _stockMutationService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, IServiceCostService _serviceCostService,
                                           ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService);
        bool DeleteObject(int Id);
        void CalculateTotalCost(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                ICoreIdentificationService _coreIdentificationService, ICoreBuilderService _coreBuilderService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                IRollerBuilderService _rollerBuilderService, IItemService _itemService);
    }
}