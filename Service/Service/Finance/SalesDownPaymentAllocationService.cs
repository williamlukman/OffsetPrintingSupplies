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

        public SalesDownPaymentAllocation GetObjectByPayableId(int PayableId)
        {
            return _repository.GetObjectByPayableId(PayableId);
        }

        public SalesDownPaymentAllocation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<SalesDownPaymentAllocation> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }


        public SalesDownPaymentAllocation CalculateTotalAmount(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService)
        {
            IList<SalesDownPaymentAllocationDetail> paymentVoucherDetails = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);
            decimal total = 0;
            foreach (SalesDownPaymentAllocationDetail detail in paymentVoucherDetails)
            {
                total += detail.AmountPaid;
            }
            salesDownPaymentAllocation.TotalAmount = total;
            salesDownPaymentAllocation = _repository.UpdateObject(salesDownPaymentAllocation);
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation CreateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentService _salesDownPaymentService, 
                                                       ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IContactService _contactService, IPayableService _payableService)
        {
            salesDownPaymentAllocation.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(salesDownPaymentAllocation, this, _salesDownPaymentAllocationDetailService, _salesDownPaymentService, _contactService, _payableService))
            {
                _repository.CreateObject(salesDownPaymentAllocation);
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation UpdateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentService _salesDownPaymentService, 
                                                          ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IContactService _contactService, IPayableService _payableService)
        {
            if (_validator.ValidUpdateObject(salesDownPaymentAllocation, this, _salesDownPaymentAllocationDetailService,
                                             _salesDownPaymentService, _contactService, _payableService))
            {
                _repository.UpdateObject(salesDownPaymentAllocation);
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation SoftDeleteObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService)
        {
            return (_validator.ValidDeleteObject(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService) ? _repository.SoftDeleteObject(salesDownPaymentAllocation) : salesDownPaymentAllocation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesDownPaymentAllocation ConfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, DateTime ConfirmationDate,
                                                        ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                        ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IPayableService _payableService,
                                                        IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPaymentAllocation.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService, _salesDownPaymentService, _receivableService, _payableService,
                                              _accountService, _generalLedgerJournalService, _closingService))
            {
                IList<SalesDownPaymentAllocationDetail> details = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesDownPaymentAllocationDetailService.ConfirmObject(detail, ConfirmationDate, this, _salesDownPaymentService, _receivableService, _payableService);
                }
                _repository.ConfirmObject(salesDownPaymentAllocation);
                _generalLedgerJournalService.CreateConfirmationJournalForSalesDownPaymentAllocation(salesDownPaymentAllocation, _accountService, _salesDownPaymentService, _salesDownPaymentAllocationDetailService,
                                             _payableService, _receivableService);
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation UnconfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                          ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IPayableService _payableService,
                                                          IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService, _receivableService, _payableService,
                                                _accountService, _generalLedgerJournalService, _closingService))
            {
                IList<SalesDownPaymentAllocationDetail> details = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesDownPaymentAllocationDetailService.UnconfirmObject(detail, this, _salesDownPaymentService, _receivableService, _payableService);
                }
                _generalLedgerJournalService.CreateUnconfirmationJournalForSalesDownPaymentAllocation(salesDownPaymentAllocation, _accountService, _salesDownPaymentService, _salesDownPaymentAllocationDetailService,
                                             _payableService, _receivableService);
                _repository.UnconfirmObject(salesDownPaymentAllocation);
            }
            return salesDownPaymentAllocation;
        }
    }
}