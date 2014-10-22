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
    public class PurchaseDownPaymentAllocationService : IPurchaseDownPaymentAllocationService
    {
        private IPurchaseDownPaymentAllocationRepository _repository;
        private IPurchaseDownPaymentAllocationValidator _validator;

        public PurchaseDownPaymentAllocationService(IPurchaseDownPaymentAllocationRepository _purchaseDownPaymentAllocationRepository, IPurchaseDownPaymentAllocationValidator _purchaseDownPaymentAllocationValidator)
        {
            _repository = _purchaseDownPaymentAllocationRepository;
            _validator = _purchaseDownPaymentAllocationValidator;
        }

        public IPurchaseDownPaymentAllocationValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PurchaseDownPaymentAllocation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseDownPaymentAllocation> GetAll()
        {
            return _repository.GetAll();
        }

        public PurchaseDownPaymentAllocation GetObjectByPurchaseDownPaymentId(int purchaseDownPaymentId)
        {
            return _repository.GetObjectByPurchaseDownPaymentId(purchaseDownPaymentId);
        }

        public PurchaseDownPaymentAllocation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseDownPaymentAllocation> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public PurchaseDownPaymentAllocation CreateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentService _purchaseDownPaymentService, 
                                                          IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IContactService _contactService)
        {
            purchaseDownPaymentAllocation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseDownPaymentAllocation, this, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _contactService) ?
                    _repository.CreateObject(purchaseDownPaymentAllocation) : purchaseDownPaymentAllocation);
        }

        public PurchaseDownPaymentAllocation UpdateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentService _purchaseDownPaymentService, 
                                                          IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IContactService _contactService)
        {
            return (_validator.ValidUpdateObject(purchaseDownPaymentAllocation, this, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _contactService) ?
                    _repository.UpdateObject(purchaseDownPaymentAllocation) : purchaseDownPaymentAllocation);
        }

        public PurchaseDownPaymentAllocation SoftDeleteObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService)
        {
            return (_validator.ValidDeleteObject(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService) ? _repository.SoftDeleteObject(purchaseDownPaymentAllocation) : purchaseDownPaymentAllocation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseDownPaymentAllocation ConfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, DateTime ConfirmationDate, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                                           IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService,
                                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseDownPaymentAllocation.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _payableService,
                                              _paymentVoucherDetailService, _cashBankService, _accountService, _generalLedgerJournalService, _closingService))
            {
                IList<PurchaseDownPaymentAllocationDetail> details = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseDownPaymentAllocationDetailService.ConfirmObject(detail, ConfirmationDate, this, _purchaseDownPaymentService, _paymentVoucherDetailService, _payableService);
                }
                _repository.ConfirmObject(purchaseDownPaymentAllocation);
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation UnconfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                                             IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                                             IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService, _payableService, _paymentVoucherDetailService,
                                                _accountService, _generalLedgerJournalService, _closingService))
            {
                IList<PurchaseDownPaymentAllocationDetail> details = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseDownPaymentAllocationDetailService.UnconfirmObject(detail, this, _purchaseDownPaymentService, _paymentVoucherDetailService, _payableService);
                }
                _repository.UnconfirmObject(purchaseDownPaymentAllocation);
            }
            return purchaseDownPaymentAllocation;
        }
    }
}