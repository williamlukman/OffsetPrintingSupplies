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
    public class SalesDownPaymentService : ISalesDownPaymentService
    {
        private ISalesDownPaymentRepository _repository;
        private ISalesDownPaymentValidator _validator;

        public SalesDownPaymentService(ISalesDownPaymentRepository _salesDownPaymentRepository, ISalesDownPaymentValidator _salesDownPaymentValidator)
        {
            _repository = _salesDownPaymentRepository;
            _validator = _salesDownPaymentValidator;
        }

        public ISalesDownPaymentValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalesDownPayment> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalesDownPayment> GetAll()
        {
            return _repository.GetAll();
        }

        public SalesDownPayment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<SalesDownPayment> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public SalesDownPayment CreateObject(SalesDownPayment salesDownPayment, IContactService _contactService)
        {
            salesDownPayment.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(salesDownPayment, this, _contactService))
            {
                _repository.CreateObject(salesDownPayment);
            }
            return salesDownPayment;
        }

        public SalesDownPayment UpdateObject(SalesDownPayment salesDownPayment, IContactService _contactService)
        {
            if (_validator.ValidUpdateObject(salesDownPayment, this, _contactService))
            {
                _repository.UpdateObject(salesDownPayment);
            }
            return salesDownPayment;
        }

        public SalesDownPayment SoftDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService)
        {
            if (_validator.ValidDeleteObject(salesDownPayment, _salesDownPaymentAllocationService))
            {
                _repository.SoftDeleteObject(salesDownPayment);
            }
            return salesDownPayment;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesDownPayment ConfirmObject(SalesDownPayment salesDownPayment, DateTime ConfirmationDate, IReceivableService _receivableService, IPayableService _payableService,
                                              IContactService _contactService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                              IExchangeRateService _exchangeRateService,ICurrencyService _currencyService,IGLNonBaseCurrencyService _glNonBaseCurrencyService)
        {
            salesDownPayment.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesDownPayment, _receivableService, _payableService, this, _contactService, _accountService,
                                              _generalLedgerJournalService, _closingService))
            {
                Currency currency = _currencyService.GetObjectById(salesDownPayment.CurrencyId);
                if (currency.IsBase == false)
                {
                    salesDownPayment.ExchangeRateId = _exchangeRateService.GetLatestRate(salesDownPayment.ConfirmationDate.Value, currency).Id;
                    salesDownPayment.ExchangeRateAmount = _exchangeRateService.GetObjectById(salesDownPayment.ExchangeRateId.Value).Rate;
                }
                else
                {
                    salesDownPayment.ExchangeRateAmount = 1;
                }

                Receivable receivable = new Receivable()
                {
                    ContactId = salesDownPayment.ContactId,
                    Amount = salesDownPayment.TotalAmount,
                    RemainingAmount = salesDownPayment.TotalAmount,
                    DueDate = salesDownPayment.DueDate == null ? salesDownPayment.DownPaymentDate : salesDownPayment.DueDate.Value,
                    CompletionDate = null,
                    ReceivableSource = Constant.SourceDocumentType.SalesDownPayment,
                    ReceivableSourceId = salesDownPayment.Id,
                    CurrencyId = salesDownPayment.CurrencyId,
                    Rate = salesDownPayment.ExchangeRateAmount,
                };
                _receivableService.CreateObject(receivable);

                Payable payable = new Payable()
                {
                    ContactId = salesDownPayment.ContactId,
                    Amount = salesDownPayment.TotalAmount,
                    RemainingAmount = salesDownPayment.TotalAmount,
                    DueDate = salesDownPayment.DueDate == null ? salesDownPayment.DownPaymentDate : salesDownPayment.DueDate.Value,
                    CompletionDate = null,
                    PayableSource = Constant.SourceDocumentType.SalesDownPayment,
                    PayableSourceId = salesDownPayment.Id,
                    Rate = salesDownPayment.ExchangeRateAmount,
                    CurrencyId = salesDownPayment.CurrencyId,
                };
                _payableService.CreateObject(payable);

                salesDownPayment.ReceivableId = receivable.Id;
                salesDownPayment.PayableId = payable.Id;

                _repository.ConfirmObject(salesDownPayment);
                _generalLedgerJournalService.CreateConfirmationJournalForSalesDownPayment(salesDownPayment, _accountService,_currencyService,_glNonBaseCurrencyService);
            }
            return salesDownPayment;
        }

        public SalesDownPayment UnconfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IReceivableService _receivableService,
                                                IPayableService _payableService, IContactService _contactService, IAccountService _accountService,
                                                IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,ICurrencyService _currencyService,IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            if (_validator.ValidUnconfirmObject(salesDownPayment, _receivableService, _payableService, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService,
                                                _accountService, _generalLedgerJournalService, _closingService))
            {
                _receivableService.DeleteObject((int) salesDownPayment.ReceivableId);
                salesDownPayment.ReceivableId = null;
                _payableService.DeleteObject((int)salesDownPayment.PayableId);
                salesDownPayment.PayableId = null;

                _repository.UnconfirmObject(salesDownPayment);
                _generalLedgerJournalService.CreateUnconfirmationJournalForSalesDownPayment(salesDownPayment, _accountService, _currencyService, _gLNonBaseCurrencyService);
            }
            return salesDownPayment;
        }
    }
}