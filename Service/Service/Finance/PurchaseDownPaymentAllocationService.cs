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

        public PurchaseDownPaymentAllocation GetObjectByReceivableId(int ReceivableId)
        {
            return _repository.GetObjectByReceivableId(ReceivableId);
        }

        public PurchaseDownPaymentAllocation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseDownPaymentAllocation> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public PurchaseDownPaymentAllocation CalculateTotalAmount(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService)
        {
            IList<PurchaseDownPaymentAllocationDetail> paymentVoucherDetails = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);
            decimal total = 0;
            foreach (PurchaseDownPaymentAllocationDetail detail in paymentVoucherDetails)
            {
                total += detail.AmountPaid;
            }
            purchaseDownPaymentAllocation.TotalAmount = total;
            purchaseDownPaymentAllocation = _repository.UpdateObject(purchaseDownPaymentAllocation);
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation CreateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentService _purchaseDownPaymentService, 
                                                          IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IContactService _contactService, IReceivableService _receivableService)
        {
            purchaseDownPaymentAllocation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseDownPaymentAllocation, this, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _contactService, _receivableService) ?
                    _repository.CreateObject(purchaseDownPaymentAllocation) : purchaseDownPaymentAllocation);
        }

        public PurchaseDownPaymentAllocation UpdateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentService _purchaseDownPaymentService, 
                                                          IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IContactService _contactService, IReceivableService _receivableService)
        {
            return (_validator.ValidUpdateObject(purchaseDownPaymentAllocation, this, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _contactService, _receivableService) ?
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
                                                           IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IReceivableService _receivableService,
                                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseDownPaymentAllocation.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _payableService,
                                              _receivableService, _accountService, _generalLedgerJournalService, _closingService))
            {
                IList<PurchaseDownPaymentAllocationDetail> details = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseDownPaymentAllocationDetailService.ConfirmObject(detail, ConfirmationDate, this, _purchaseDownPaymentService, _payableService, _receivableService);
                }
                _repository.ConfirmObject(purchaseDownPaymentAllocation);
                _generalLedgerJournalService.CreateConfirmationJournalForPurchaseDownPaymentAllocation(purchaseDownPaymentAllocation, _accountService, _purchaseDownPaymentService, _purchaseDownPaymentAllocationDetailService,
                                             _payableService, _receivableService);
            }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation UnconfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                                             IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IReceivableService _receivableService,
                                                             IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(purchaseDownPaymentAllocation, _purchaseDownPaymentAllocationDetailService, _payableService, _receivableService,
                                                _accountService, _generalLedgerJournalService, _closingService))
            {
                IList<PurchaseDownPaymentAllocationDetail> details = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseDownPaymentAllocationDetailService.UnconfirmObject(detail, this, _purchaseDownPaymentService, _payableService, _receivableService);
                }
                _generalLedgerJournalService.CreateUnconfirmationJournalForPurchaseDownPaymentAllocation(purchaseDownPaymentAllocation, _accountService, _purchaseDownPaymentService, _purchaseDownPaymentAllocationDetailService,
                                             _payableService, _receivableService);
                _repository.UnconfirmObject(purchaseDownPaymentAllocation);
            }
            return purchaseDownPaymentAllocation;
        }
    }
}