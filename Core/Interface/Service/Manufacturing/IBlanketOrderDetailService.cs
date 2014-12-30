using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBlanketOrderDetailService
    {
        IBlanketOrderDetailValidator GetValidator();
        IBlanketOrderDetailRepository GetRepository();
        IQueryable<BlanketOrderDetail> GetQueryable();
        IList<BlanketOrderDetail> GetAll();
        IList<BlanketOrderDetail> GetObjectsByBlanketOrderId(int blanketOrderId);
        IList<BlanketOrderDetail> GetObjectsByBlanketId(int blanketId);
        BlanketOrderDetail GetObjectById(int Id);
        Blanket GetBlanket(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail CreateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService);
        BlanketOrderDetail UpdateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService);
        BlanketOrderDetail SoftDeleteObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);
        BlanketOrderDetail CutObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);
        BlanketOrderDetail SideSealObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail PrepareObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail ApplyTapeAdhesiveToObject(BlanketOrderDetail blanketOrderDetail, decimal AdhesiveUsage, decimal Adhesive2Usage, IBlanketService _blanketService);
        BlanketOrderDetail MountObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail HeatPressObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail PullOffTestObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail QCAndMarkObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail PackageObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail RejectObject(BlanketOrderDetail blanketOrderDetail, DateTime RejectedDate, IBlanketOrderService _blanketOrderService, IStockMutationService _stockMutationService,
                                        IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                        IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        BlanketOrderDetail UndoRejectObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IStockMutationService _stockMutationService,
                                            IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        BlanketOrderDetail FinishObject(BlanketOrderDetail blanketOrderDetail, DateTime FinishedDate, IBlanketOrderService _blanketOrderService, IStockMutationService _stockMutationService,
                                        IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                        IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        BlanketOrderDetail UnfinishObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IStockMutationService _stockMutationService,
                                          IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                          IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool DeleteObject(int Id);
    }
}