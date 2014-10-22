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
    public class SalesDownPaymentAllocationService : ISalesDownPaymentAllocationService
    {
        private ISalesDownPaymentAllocationRepository _repository;
        private ISalesDownPaymentAllocationValidator _validator;

        public SalesDownPaymentAllocationService(ISalesDownPaymentAllocationRepository _salesDownPaymentAllocationRepository, ISalesDownPaymentAllocationValidator _salesDownPaymentAllocationValidator)
        {
            _repository = _salesDownPaymentAllocationRepository;
            _validator = _salesDownPaymentAllocationValidator;
        }

        public ISalesDownPaymentAllocationValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalesDownPaymentAllocation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalesDownPaymentAllocation> GetAll()
        {
            return _repository.GetAll();
        }

        public SalesDownPaymentAllocation GetObjectBySalesDownPaymentId(int salesDownPaymentId)
        {
            return _repository.GetObjectBySalesDownPaymentId(salesDownPaymentId);
        }

        public SalesDownPaymentAllocation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<SalesDownPaymentAllocation> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public SalesDownPaymentAllocation CreateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentService _salesDownPaymentService, 
                                                          ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IContactService _contactService)
        {
            salesDownPaymentAllocation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salesDownPaymentAllocation, this, _salesDownPaymentAllocationDetailService, _salesDownPaymentService, _contactService) ?
                    _repository.CreateObject(salesDownPaymentAllocation) : salesDownPaymentAllocation);
        }

        public SalesDownPaymentAllocation UpdateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentService _salesDownPaymentService, 
                                                          ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IContactService _contactService)
        {
            return (_validator.ValidUpdateObject(salesDownPaymentAllocation, this, _salesDownPaymentAllocationDetailService, _salesDownPaymentService, _contactService) ?
                    _repository.UpdateObject(salesDownPaymentAllocation) : salesDownPaymentAllocation);
        }

        public SalesDownPaymentAllocation SoftDeleteObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService)
        {
            return (_validator.ValidDeleteObject(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService) ? _repository.SoftDeleteObject(salesDownPaymentAllocation) : salesDownPaymentAllocation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesDownPaymentAllocation ConfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, DateTime ConfirmationDate, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                           ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPaymentAllocation.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService, _salesDownPaymentService, _receivableService,
                                              _receiptVoucherDetailService, _cashBankService, _accountService, _generalLedgerJournalService, _closingService))
            {
                IList<SalesDownPaymentAllocationDetail> details = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesDownPaymentAllocationDetailService.ConfirmObject(detail, ConfirmationDate, this, _salesDownPaymentService, _receiptVoucherDetailService, _receivableService);
                }
                _repository.ConfirmObject(salesDownPaymentAllocation);
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation UnconfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                             ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                                             IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService, _receivableService, _receiptVoucherDetailService,
                                                _accountService, _generalLedgerJournalService, _closingService))
            {
                IList<SalesDownPaymentAllocationDetail> details = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesDownPaymentAllocationDetailService.UnconfirmObject(detail, this, _salesDownPaymentService, _receiptVoucherDetailService, _receivableService);
                }
                _repository.UnconfirmObject(salesDownPaymentAllocation);
            }
            return salesDownPaymentAllocation;
        }
    }
}