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
    public class ReceiptRequestService : IReceiptRequestService
    {
        private IReceiptRequestRepository _repository;
        private IReceiptRequestValidator _validator;

        public ReceiptRequestService(IReceiptRequestRepository _receiptRequestRepository, IReceiptRequestValidator _receiptRequestValidator)
        {
            _repository = _receiptRequestRepository;
            _validator = _receiptRequestValidator;
        }

        public IReceiptRequestValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ReceiptRequest> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ReceiptRequest> GetAll()
        {
            return _repository.GetAll();
        }

        public ReceiptRequest GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<ReceiptRequest> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public ReceiptRequest CreateObject(ReceiptRequest receiptRequest, IContactService _contactService, IReceiptRequestDetailService _receiptRequestDetailService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            receiptRequest.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(receiptRequest, _contactService))
            {
                receiptRequest = _repository.CreateObject(receiptRequest);
            }
            return receiptRequest;
        }

        public ReceiptRequest UpdateObject(ReceiptRequest receiptRequest, IContactService _contactService, IReceiptRequestDetailService _receiptRequestDetailService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUpdateObject(receiptRequest, _contactService))
            {
                _repository.UpdateObject(receiptRequest);
            }
            return receiptRequest;
        }

        public ReceiptRequest SoftDeleteObject(ReceiptRequest receiptRequest)
        {
            return (_validator.ValidDeleteObject(receiptRequest) ? _repository.SoftDeleteObject(receiptRequest) : receiptRequest);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public ReceiptRequest ConfirmObject(ReceiptRequest receiptRequest, DateTime ConfirmationDate, IReceivableService _receivableService,
                                            IReceiptRequestDetailService _receiptRequestDetailService, IAccountService _accountService,
                                            IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                            IExchangeRateService _exchangeRateService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService,
                                            ICurrencyService _currencyService)
        {
            receiptRequest.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(receiptRequest, _receiptRequestDetailService, _closingService))
            {
                // confirm object
                // create receivable
                Currency currency = _currencyService.GetObjectById(receiptRequest.CurrencyId);
                if (currency.IsBase == false)
                {
                    receiptRequest.ExchangeRateId = _exchangeRateService.GetLatestRate(receiptRequest.ConfirmationDate.Value, currency).Id;
                    receiptRequest.ExchangeRateAmount = _exchangeRateService.GetObjectById(receiptRequest.ExchangeRateId.Value).Rate;
                }
                else
                {
                    receiptRequest.ExchangeRateAmount = 1;
                }

                receiptRequest = _repository.ConfirmObject(receiptRequest);
                Receivable receivable = new Receivable()
                {
                    ContactId = receiptRequest.ContactId,
                    ReceivableSource = Constant.ReceivableSource.ReceiptRequest,
                    ReceivableSourceId = receiptRequest.Id,
                    Code = receiptRequest.Code,
                    Amount = receiptRequest.Amount,
                    CurrencyId = receiptRequest.CurrencyId,
                    RemainingAmount = receiptRequest.Amount,
                    DueDate = receiptRequest.DueDate
                };
                _receivableService.CreateObject(receivable);
                _generalLedgerJournalService.CreateConfirmationJournalForReceiptRequest(receiptRequest, _receiptRequestDetailService,
                                             _accountService, _gLNonBaseCurrencyService, _currencyService);
            }
            return receiptRequest;
        }

        public ReceiptRequest UnconfirmObject(ReceiptRequest receiptRequest, IReceiptRequestDetailService _receiptRequestDetailService, IReceivableService _receivableService,
                                              IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                              IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            if (_validator.ValidUnconfirmObject(receiptRequest, _receiptRequestDetailService, _closingService))
            {
                _repository.UnconfirmObject(receiptRequest);
                Receivable receivable = _receivableService.GetObjectBySource(Constant.ReceivableSource.ReceiptRequest, receiptRequest.Id);
                _receivableService.DeleteObject(receivable.Id);
                _generalLedgerJournalService.CreateUnconfirmationJournalForReceiptRequest(receiptRequest, _receiptRequestDetailService, _accountService,
                                             _gLNonBaseCurrencyService, _currencyService);
            }
            return receiptRequest;
        }

        public ReceiptRequest CalculateTotalAmount(ReceiptRequest receiptRequest, IReceiptRequestDetailService _receiptRequestDetailService)
        {
            IList<ReceiptRequestDetail> paymenRequestDetails = _receiptRequestDetailService.GetObjectsByReceiptRequestId(receiptRequest.Id);
            decimal total = 0;
            foreach (ReceiptRequestDetail detail in paymenRequestDetails)
            {
                total += detail.Amount;
            }
            receiptRequest.Amount = total;
            receiptRequest = _repository.UpdateObject(receiptRequest);
            return receiptRequest;
        }
    }
}