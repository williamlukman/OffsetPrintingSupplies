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

        public IList<PurchaseDownPayment> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public PurchaseDownPayment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseDownPayment> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public PurchaseDownPayment CreateObject(PurchaseDownPayment purchaseDownPayment, IContactService _contactService,
                                                IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                                ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService)
        {
            purchaseDownPayment.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(purchaseDownPayment, this, _contactService, _cashBankService, _paymentVoucherService))
            {
                PaymentVoucher paymentVoucher = new PaymentVoucher()
                {
                    ContactId = purchaseDownPayment.ContactId,
                    CashBankId = purchaseDownPayment.CashBankId,
                    IsGBCH = purchaseDownPayment.IsGBCH,
                    PaymentDate = purchaseDownPayment.DownPaymentDate,
                    TotalAmount = purchaseDownPayment.TotalAmount,
                    DueDate = purchaseDownPayment.DueDate,
                };
                _paymentVoucherService.CreateObject(paymentVoucher, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);
                purchaseDownPayment.PaymentVoucherId = paymentVoucher.Id;
                _repository.CreateObject(purchaseDownPayment);
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment UpdateObject(PurchaseDownPayment purchaseDownPayment, IContactService _contactService,
                                                IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                                ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService)
        {
            if (_validator.ValidUpdateObject(purchaseDownPayment, this, _contactService, _cashBankService, _paymentVoucherService))
            {
                _repository.UpdateObject(purchaseDownPayment);
                PaymentVoucher paymentVoucher = _paymentVoucherService.GetObjectById(purchaseDownPayment.PaymentVoucherId);
                paymentVoucher.ContactId = purchaseDownPayment.ContactId;
                paymentVoucher.CashBankId = purchaseDownPayment.CashBankId;
                paymentVoucher.IsGBCH = purchaseDownPayment.IsGBCH;
                paymentVoucher.PaymentDate = purchaseDownPayment.DownPaymentDate;
                paymentVoucher.TotalAmount = purchaseDownPayment.TotalAmount;
                paymentVoucher.DueDate = purchaseDownPayment.DueDate;
                _paymentVoucherService.UpdateObject(paymentVoucher, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment SoftDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                    IPaymentVoucherDetailService _paymentVoucherDetailService, IPaymentVoucherService _paymentVoucherService)
        {
            if (_validator.ValidDeleteObject(purchaseDownPayment, _purchaseDownPaymentAllocationService, _paymentVoucherDetailService))
            {
                _repository.SoftDeleteObject(purchaseDownPayment);
                _paymentVoucherService.DeleteObject(purchaseDownPayment.PaymentVoucherId);
            }
            return purchaseDownPayment;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseDownPayment ConfirmObject(PurchaseDownPayment purchaseDownPayment, DateTime ConfirmationDate, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                                                 IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService, IContactService _contactService,
                                                 IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseDownPayment.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseDownPayment, _cashBankService, _paymentVoucherService, this, _contactService, _accountService, _generalLedgerJournalService, _closingService))
            {
                _repository.ConfirmObject(purchaseDownPayment);
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment UnconfirmObject(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService, 
                                                   IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                                   IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService, IContactService _contactService,
                                                   IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(purchaseDownPayment, _cashBankService, _paymentVoucherService,
                                                _purchaseDownPaymentAllocationService, _purchaseDownPaymentAllocationDetailService,
                                                _accountService, _generalLedgerJournalService, _closingService))
            {
                _repository.UnconfirmObject(purchaseDownPayment);
            }
            return purchaseDownPayment;
        }
    }
}