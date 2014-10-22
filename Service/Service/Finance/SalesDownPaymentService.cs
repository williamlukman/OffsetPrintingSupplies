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
                ReceiptVoucher receiptVoucher = new ReceiptVoucher()
                {
                    ContactId = salesDownPayment.ContactId,
                    CashBankId = salesDownPayment.CashBankId,
                    IsGBCH = salesDownPayment.IsGBCH,
                    ReceiptDate = salesDownPayment.DownPaymentDate,
                    TotalAmount = salesDownPayment.TotalAmount,
                    DueDate = salesDownPayment.DueDate,
                };
                _receiptVoucherService.CreateObject(receiptVoucher, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);
                salesDownPayment.ReceiptVoucherId = receiptVoucher.Id;
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
                ReceiptVoucher receiptVoucher = _receiptVoucherService.GetObjectById(salesDownPayment.ReceiptVoucherId);
                receiptVoucher.ContactId = salesDownPayment.ContactId;
                receiptVoucher.CashBankId = salesDownPayment.CashBankId;
                receiptVoucher.IsGBCH = salesDownPayment.IsGBCH;
                receiptVoucher.ReceiptDate = salesDownPayment.DownPaymentDate;
                receiptVoucher.TotalAmount = salesDownPayment.TotalAmount;
                receiptVoucher.DueDate = salesDownPayment.DueDate;
                _receiptVoucherService.UpdateObject(receiptVoucher, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);
            }
            return salesDownPayment;
        }

        public SalesDownPayment SoftDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                    IReceiptVoucherDetailService _receiptVoucherDetailService, IReceiptVoucherService _receiptVoucherService)
        {
            if (_validator.ValidDeleteObject(salesDownPayment, _salesDownPaymentAllocationService, _receiptVoucherDetailService))
            {
                _repository.SoftDeleteObject(salesDownPayment);
                _receiptVoucherService.DeleteObject(salesDownPayment.ReceiptVoucherId);
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