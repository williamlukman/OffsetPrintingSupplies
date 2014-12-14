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
    public class PurchaseDownPaymentService : IPurchaseDownPaymentService
    {
        private IPurchaseDownPaymentRepository _repository;
        private IPurchaseDownPaymentValidator _validator;

        public PurchaseDownPaymentService(IPurchaseDownPaymentRepository _purchaseDownPaymentRepository, IPurchaseDownPaymentValidator _purchaseDownPaymentValidator)
        {
            _repository = _purchaseDownPaymentRepository;
            _validator = _purchaseDownPaymentValidator;
        }

        public IPurchaseDownPaymentValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PurchaseDownPayment> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseDownPayment> GetAll()
        {
            return _repository.GetAll();
        }

        public PurchaseDownPayment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseDownPayment> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public PurchaseDownPayment CreateObject(PurchaseDownPayment purchaseDownPayment, IContactService _contactService)
        {
            purchaseDownPayment.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(purchaseDownPayment, this, _contactService))
            {
                _repository.CreateObject(purchaseDownPayment);
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment UpdateObject(PurchaseDownPayment purchaseDownPayment, IContactService _contactService)
        {
            if (_validator.ValidUpdateObject(purchaseDownPayment, this, _contactService))
            {
                _repository.UpdateObject(purchaseDownPayment);
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment SoftDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService)
        {
            if (_validator.ValidDeleteObject(purchaseDownPayment, _purchaseDownPaymentAllocationService))
            {
                _repository.SoftDeleteObject(purchaseDownPayment);
            }
            return purchaseDownPayment;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseDownPayment ConfirmObject(PurchaseDownPayment purchaseDownPayment, DateTime ConfirmationDate, IPayableService _payableService, IReceivableService _receivableService,
                                                 IContactService _contactService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                                 ICurrencyService _currencyService, IExchangeRateService _exchangeRateService,IGLNonBaseCurrencyService _glNonBaseCurrencyService)
        {
            purchaseDownPayment.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseDownPayment, _payableService, _receivableService, this, _contactService, _accountService,
                                              _generalLedgerJournalService, _closingService))
            {
                Currency currency = _currencyService.GetObjectById(purchaseDownPayment.CurrencyId);
                if (currency.IsBase == false)
                {
                    purchaseDownPayment.ExchangeRateId = _exchangeRateService.GetLatestRate(purchaseDownPayment.ConfirmationDate.Value, currency).Id;
                    purchaseDownPayment.ExchangeRateAmount = _exchangeRateService.GetObjectById(purchaseDownPayment.ExchangeRateId.Value).Rate;
                }
                else
                {
                    purchaseDownPayment.ExchangeRateAmount = 1;
                }

                Receivable receivable = new Receivable()
                {
                    ContactId = purchaseDownPayment.ContactId,
                    Amount = purchaseDownPayment.TotalAmount,
                    RemainingAmount = purchaseDownPayment.TotalAmount,
                    DueDate = purchaseDownPayment.DueDate == null ? purchaseDownPayment.DownPaymentDate : purchaseDownPayment.DueDate.Value,
                    CompletionDate = null,
                    ReceivableSource = Constant.SourceDocumentType.PurchaseDownPayment,
                    ReceivableSourceId = purchaseDownPayment.Id,
                    CurrencyId = purchaseDownPayment.CurrencyId,
                    Rate = purchaseDownPayment.ExchangeRateAmount,
                };
                _receivableService.CreateObject(receivable);

                Payable payable = new Payable()
                {
                    ContactId = purchaseDownPayment.ContactId,
                    Amount = purchaseDownPayment.TotalAmount,
                    RemainingAmount = purchaseDownPayment.TotalAmount,
                    DueDate = purchaseDownPayment.DueDate == null ? purchaseDownPayment.DownPaymentDate : purchaseDownPayment.DueDate.Value,
                    CompletionDate = null,
                    PayableSource = Constant.SourceDocumentType.PurchaseDownPayment,
                    PayableSourceId = purchaseDownPayment.Id,
                    CurrencyId = purchaseDownPayment.CurrencyId,
                    Rate = purchaseDownPayment.ExchangeRateAmount,
                };
                _payableService.CreateObject(payable);

                purchaseDownPayment.ReceivableId = receivable.Id;
                purchaseDownPayment.PayableId = payable.Id;

                _repository.ConfirmObject(purchaseDownPayment);
                _generalLedgerJournalService.CreateConfirmationJournalForPurchaseDownPayment(purchaseDownPayment, _accountService,_currencyService,_glNonBaseCurrencyService);

            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment UnconfirmObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPayableService _payableService, IReceivableService _receivableService, 
                                                IContactService _contactService, IAccountService _accountService,
                                                IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,ICurrencyService _currencyService,IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            if (_validator.ValidUnconfirmObject(purchaseDownPayment, _payableService, _receivableService, _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService,
                                                _accountService, _generalLedgerJournalService, _closingService))
            {
                _receivableService.DeleteObject((int)purchaseDownPayment.ReceivableId);
                purchaseDownPayment.ReceivableId = null;
                _payableService.DeleteObject((int)purchaseDownPayment.PayableId);
                purchaseDownPayment.PayableId = null;

                _repository.UnconfirmObject(purchaseDownPayment);
                _generalLedgerJournalService.CreateUnconfirmationJournalForPurchaseDownPayment(purchaseDownPayment, _accountService, _currencyService, _gLNonBaseCurrencyService);
            }
            return purchaseDownPayment;
        }
    }
}