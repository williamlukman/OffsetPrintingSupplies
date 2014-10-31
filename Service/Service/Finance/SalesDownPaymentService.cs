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

        public SalesDownPayment ConfirmObject(SalesDownPayment salesDownPayment, DateTime ConfirmationDate, IReceivableService _receivableService, IContactService _contactService,
                                              IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPayment.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesDownPayment, _receivableService, this, _contactService, _accountService, _generalLedgerJournalService, _closingService))
            {
                Receivable receivable = new Receivable()
                {
                    ContactId = salesDownPayment.ContactId,
                    Amount = salesDownPayment.TotalAmount,
                    DueDate = salesDownPayment.DueDate == null ? salesDownPayment.DownPaymentDate : salesDownPayment.DueDate.Value,
                    CompletionDate = null,
                    ReceivableSource = Constant.SourceDocumentType.SalesDownPayment,
                    ReceivableSourceId = salesDownPayment.Id
                };
                _receivableService.CreateObject(receivable);
                salesDownPayment.ReceivableId = receivable.Id;
                _repository.ConfirmObject(salesDownPayment);
                _generalLedgerJournalService.CreateConfirmationJournalForSalesDownPayment(salesDownPayment, _accountService);
            }
            return salesDownPayment;
        }

        public SalesDownPayment UnconfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IReceivableService _receivableService, IContactService _contactService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(salesDownPayment, _receivableService, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService,
                                                _accountService, _generalLedgerJournalService, _closingService))
            {
                _receivableService.DeleteObject((int) salesDownPayment.ReceivableId);
                salesDownPayment.ReceivableId = null;
                _repository.UnconfirmObject(salesDownPayment);
                _generalLedgerJournalService.CreateUnconfirmationJournalForSalesDownPayment(salesDownPayment, _accountService);
            }
            return salesDownPayment;
        }
    }
}