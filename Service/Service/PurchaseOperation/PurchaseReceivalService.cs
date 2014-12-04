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
    public class PurchaseReceivalService : IPurchaseReceivalService
    {
        private IPurchaseReceivalRepository _repository;
        private IPurchaseReceivalValidator _validator;

        public PurchaseReceivalService(IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseReceivalValidator _purchaseReceivalValidator)
        {
            _repository = _purchaseReceivalRepository;
            _validator = _purchaseReceivalValidator;
        }

        public IPurchaseReceivalValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PurchaseReceival> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseReceival> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PurchaseReceival> GetConfirmedObjects()
        {
            return _repository.GetConfirmedObjects();
        }

        public PurchaseReceival GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseReceival> GetObjectsByPurchaseOrderId(int purchaseOrderId)
        {
            return _repository.GetObjectsByPurchaseOrderId(purchaseOrderId);
        }
        
        public PurchaseReceival CreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService)
        {
            purchaseReceival.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseReceival, this, _purchaseOrderService, _warehouseService) ? _repository.CreateObject(purchaseReceival) : purchaseReceival);
        }

        public PurchaseReceival CreateObject(int warehouseId, int purchaseOrderId, DateTime receivalDate, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService)
        {
            PurchaseReceival purchaseReceival = new PurchaseReceival
            {
                PurchaseOrderId = purchaseOrderId,
                ReceivalDate = receivalDate,
                WarehouseId = warehouseId
            };
            return this.CreateObject(purchaseReceival, _purchaseOrderService, _warehouseService);
        }

        public PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService)
        {
            return (_validator.ValidUpdateObject(purchaseReceival, this, _purchaseOrderService, _warehouseService) ? _repository.UpdateObject(purchaseReceival) : purchaseReceival);
        }

        public PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            return (_validator.ValidDeleteObject(purchaseReceival, _purchaseReceivalDetailService) ? _repository.SoftDeleteObject(purchaseReceival) : purchaseReceival);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival, DateTime ConfirmationDate, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                              IPurchaseOrderService _purchaseOrderService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService,
                                              IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                              IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,IExchangeRateService _exchangeRateService)
        {
            purchaseReceival.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseReceival, _purchaseReceivalDetailService,_exchangeRateService))
            {
                decimal TotalCOGS = 0;
                decimal TotalAmount = 0;
                if (purchaseReceival.PurchaseOrder.Currency.IsBase == false)
                {
                    purchaseReceival.ExchangeRateId = _exchangeRateService.GetLatestRate(purchaseReceival.ConfirmationDate.Value, purchaseReceival.PurchaseOrder.CurrencyId).Id;
                    purchaseReceival.ExchangeRateAmount = _exchangeRateService.GetObjectById(purchaseReceival.ExchangeRateId.Value).Rate;
                }
                else
                {
                    purchaseReceival.ExchangeRateAmount = 1;
                }
                _repository.UpdateObject(purchaseReceival);
                IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
                foreach (var detail in purchaseReceivalDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseReceivalDetailService.ConfirmObject(detail, ConfirmationDate, this, _purchaseOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                    TotalCOGS += detail.COGS;
                    TotalAmount += (detail.PurchaseOrderDetail.Price * detail.PurchaseOrderDetail.Quantity);
                }
                purchaseReceival.TotalCOGS = TotalCOGS;
                purchaseReceival.TotalAmount = TotalAmount;
                _repository.ConfirmObject(purchaseReceival);
                _generalLedgerJournalService.CreateConfirmationJournalForPurchaseReceival(purchaseReceival, _accountService);
                PurchaseOrder purchaseOrder = _purchaseOrderService.GetObjectById(purchaseReceival.PurchaseOrderId);
                _purchaseOrderService.CheckAndSetReceivalComplete(purchaseOrder, _purchaseOrderDetailService);
            }
            return purchaseReceival;
        }

        public PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseInvoiceService _purchaseInvoiceService,
                                                IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseOrderService _purchaseOrderService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                                IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(purchaseReceival, _purchaseInvoiceService))
            {
                IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
                foreach (var detail in purchaseReceivalDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseReceivalDetailService.UnconfirmObject(detail, this, _purchaseOrderService, _purchaseOrderDetailService,
                                                                   _purchaseInvoiceDetailService, _stockMutationService, _itemService,
                                                                   _blanketService, _warehouseItemService);
                }
                _generalLedgerJournalService.CreateUnconfirmationJournalForPurchaseReceival(purchaseReceival, _accountService);
                purchaseReceival.TotalCOGS = 0;
                _repository.UnconfirmObject(purchaseReceival);
                PurchaseOrder purchaseOrder = _purchaseOrderService.GetObjectById(purchaseReceival.PurchaseOrderId);
                _purchaseOrderService.UnsetReceivalComplete(purchaseOrder);
            }
            return purchaseReceival;
        }

        public PurchaseReceival CheckAndSetInvoiceComplete(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> details = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);

            foreach (var detail in details)
            {
                if (!detail.IsAllInvoiced)
                {
                    return purchaseReceival;
                }
            }
            return _repository.SetInvoiceComplete(purchaseReceival);
        }

        public PurchaseReceival UnsetInvoiceComplete(PurchaseReceival purchaseReceival)
        {
            _repository.UnsetInvoiceComplete(purchaseReceival);
            return purchaseReceival;
        }
    }
}