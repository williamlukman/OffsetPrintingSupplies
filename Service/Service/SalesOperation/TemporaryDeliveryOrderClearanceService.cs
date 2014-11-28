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
    public class TemporaryDeliveryOrderClearanceService : ITemporaryDeliveryOrderClearanceService
    {
        private ITemporaryDeliveryOrderClearanceRepository _repository;
        private ITemporaryDeliveryOrderClearanceValidator _validator;

        public TemporaryDeliveryOrderClearanceService(ITemporaryDeliveryOrderClearanceRepository _temporaryDeliveryOrderClearanceRepository, ITemporaryDeliveryOrderClearanceValidator _temporaryDeliveryOrderClearanceValidator)
        {
            _repository = _temporaryDeliveryOrderClearanceRepository;
            _validator = _temporaryDeliveryOrderClearanceValidator;
        }

        public ITemporaryDeliveryOrderClearanceValidator GetValidator()
        {
            return _validator;
        }

        public ITemporaryDeliveryOrderClearanceRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<TemporaryDeliveryOrderClearance> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<TemporaryDeliveryOrderClearance> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<TemporaryDeliveryOrderClearance> GetConfirmedObjects()
        {
            return _repository.GetConfirmedObjects();
        }

        public TemporaryDeliveryOrderClearance GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<TemporaryDeliveryOrderClearance> GetObjectsByTemporaryDeliveryOrderId(int deliveryOrderId)
        {
            return _repository.GetObjectsByTemporaryDeliveryOrderId(deliveryOrderId);
        }

        public TemporaryDeliveryOrderClearance CreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            temporaryDeliveryOrderClearance.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(temporaryDeliveryOrderClearance, this, _temporaryDeliveryOrderService) ? _repository.CreateObject(temporaryDeliveryOrderClearance) : temporaryDeliveryOrderClearance);
        }

        public TemporaryDeliveryOrderClearance UpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            return (_validator.ValidUpdateObject(temporaryDeliveryOrderClearance, this, _temporaryDeliveryOrderService) ? _repository.UpdateObject(temporaryDeliveryOrderClearance) : temporaryDeliveryOrderClearance);
        }

        public TemporaryDeliveryOrderClearance SoftDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService)
        {
            return (_validator.ValidDeleteObject(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceDetailService) ? _repository.SoftDeleteObject(temporaryDeliveryOrderClearance) : temporaryDeliveryOrderClearance);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public TemporaryDeliveryOrderClearance ConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, DateTime ConfirmationDate, 
                                     ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IStockMutationService _stockMutationService, IItemService _itemService,
                                     IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                     IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService)
        {
            temporaryDeliveryOrderClearance.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceDetailService, _closingService))
            {
                temporaryDeliveryOrderClearance.TotalWastedCoGS = 0;
                IList<TemporaryDeliveryOrderClearanceDetail> temporaryDeliveryOrderClearanceDetails = _temporaryDeliveryOrderClearanceDetailService.GetObjectsByTemporaryDeliveryOrderClearanceId(temporaryDeliveryOrderClearance.Id);
                foreach (var detail in temporaryDeliveryOrderClearanceDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _temporaryDeliveryOrderClearanceDetailService.ConfirmObject(detail, ConfirmationDate, this, _stockMutationService, _itemService,
                                                                      _blanketService, _warehouseItemService, _temporaryDeliveryOrderDetailService);
                    temporaryDeliveryOrderClearance.TotalWastedCoGS += detail.WastedCoGS;
                }
                _repository.ConfirmObject(temporaryDeliveryOrderClearance);
                if (temporaryDeliveryOrderClearance.IsWasted)
                {
                    // Posting GL (Inventory Credit, SalesExpense debit) senilai wasted cogs dari average cost
                    _generalLedgerJournalService.CreateConfirmationJournalForTemporaryDeliveryOrderClearanceWaste(temporaryDeliveryOrderClearance, _accountService);
                }
                
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance UnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                                      IStockMutationService _stockMutationService, IItemService _itemService,
                                                      IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                                      IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(temporaryDeliveryOrderClearance, _closingService))
            {
                if (temporaryDeliveryOrderClearance.IsWasted)
                {
                    // Posting GL (Inventory Debit, SalesExpense Credit) senilai wasted cogs dari average cost
                    _generalLedgerJournalService.CreateUnconfirmationJournalForTemporaryDeliveryOrderClearanceWaste(temporaryDeliveryOrderClearance, _accountService);
                }
                
                IList<TemporaryDeliveryOrderClearanceDetail> temporaryDeliveryOrderClearanceDetails = _temporaryDeliveryOrderClearanceDetailService.GetObjectsByTemporaryDeliveryOrderClearanceId(temporaryDeliveryOrderClearance.Id);
                foreach (var detail in temporaryDeliveryOrderClearanceDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _temporaryDeliveryOrderClearanceDetailService.UnconfirmObject(detail, this, _stockMutationService, _itemService, _blanketService, _warehouseItemService, _temporaryDeliveryOrderDetailService);
                }
                temporaryDeliveryOrderClearance.TotalWastedCoGS = 0;
                _repository.UnconfirmObject(temporaryDeliveryOrderClearance);
            }
            return temporaryDeliveryOrderClearance;
        }


        
    }
}