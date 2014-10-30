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

        public IList<SalesDownPayment> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public SalesDownPayment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<SalesDownPayment> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public SalesDownPayment CreateObject(SalesDownPayment salesDownPayment, IContactService _contactService,
                                                IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                                ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService)
        {
            salesDownPayment.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(salesDownPayment, this, _contactService, _cashBankService, _receiptVoucherService))
            {
                Receivable receivable = new Receivable()
                {
                    ContactId = salesDownPayment.ContactId,
                    Amount = salesDownPayment.TotalAmount,
                    DueDate = salesDownPayment.DueDate == null ? salesDownPayment.DownPaymentDate : salesDownPayment.DueDate.Value,
                    CompletionDate = salesDownPayment.DownPaymentDate,
                    ReceivableSource = Constant.SourceDocumentType.SalesDownPayment,
                    ReceivableSourceId = salesDownPayment.Id
                };
                _receivableService.CreateObject(receivable);
                salesDownPayment.ReceivableId = receivable.Id;
                _repository.CreateObject(salesDownPayment);
            }
            return salesDownPayment;
        }

        public SalesDownPayment UpdateObject(SalesDownPayment salesDownPayment, IContactService _contactService,
                                                IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                                ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService)
        {
            if (_validator.ValidUpdateObject(salesDownPayment, this, _contactService, _cashBankService, _receiptVoucherService))
            {
                _repository.UpdateObject(salesDownPayment);
                Receivable receivable = _receivableService.GetObjectBySource(Constant.SourceDocumentType.SalesDownPayment, salesDownPayment.Id);
                receivable.ContactId = salesDownPayment.ContactId;
                receivable.CompletionDate = salesDownPayment.DownPaymentDate;
                receivable.Amount= salesDownPayment.TotalAmount;
                receivable.DueDate = salesDownPayment.DueDate == null ? salesDownPayment.DownPaymentDate : salesDownPayment.DueDate.Value ;
                _receivableService.UpdateObject(receivable);
            }
            return salesDownPayment;
        }

        public SalesDownPayment SoftDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                    IReceiptVoucherDetailService _receiptVoucherDetailService, IReceiptVoucherService _receiptVoucherService, IReceivableService _receivableService)
        {
            if (_validator.ValidDeleteObject(salesDownPayment, _salesDownPaymentAllocationService, _receiptVoucherDetailService))
            {
                _repository.SoftDeleteObject(salesDownPayment);
                _receivableService.DeleteObject(salesDownPayment.ReceivableId);
            }
            return salesDownPayment;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesDownPayment ConfirmObject(SalesDownPayment salesDownPayment, DateTime ConfirmationDate, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService,
                                                 IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, IContactService _contactService,
                                                 IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPayment.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesDownPayment, _cashBankService, _receiptVoucherService, this, _contactService, _accountService, _generalLedgerJournalService, _closingService))
            {
                _repository.ConfirmObject(salesDownPayment);
            }
            return salesDownPayment;
        }

        public SalesDownPayment UnconfirmObject(SalesDownPayment salesDownPayment, ICashBankService _cashBankService, 
                                                   ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                   IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, IContactService _contactService,
                                                   IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(salesDownPayment, _cashBankService, _receiptVoucherService,
                                                _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService,
                                                _accountService, _generalLedgerJournalService, _closingService))
            {
                _repository.UnconfirmObject(salesDownPayment);
            }
            return salesDownPayment;
        }
    }
}