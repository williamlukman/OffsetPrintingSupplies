using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Text;

namespace Service.Service
{
    public class PaymentVoucherService : IPaymentVoucherService
    {
        private IPaymentVoucherRepository _repository;
        private IPaymentVoucherValidator _validator;

        public PaymentVoucherService(IPaymentVoucherRepository _paymentVoucherRepository, IPaymentVoucherValidator _paymentVoucherValidator)
        {
            _repository = _paymentVoucherRepository;
            _validator = _paymentVoucherValidator;
        }

        public IPaymentVoucherValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PaymentVoucher> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PaymentVoucher> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PaymentVoucher> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public PaymentVoucher GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PaymentVoucher> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public PaymentVoucher CreateObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            IPayableService _payableService, IContactService _contactService, 
                                            ICashBankService _cashBankService, ICurrencyService _currencyService)
        {
            paymentVoucher.Errors = new Dictionary<String, String>();
            CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
            if (cashBank == null)
            {
                paymentVoucher.Errors.Add("CashBankId", "Harus diisi");
                return paymentVoucher;
            }
            else
            {
                Currency currency = _currencyService.GetObjectById(cashBank.CurrencyId);
                if (currency.IsBase == true)
                {
                    paymentVoucher.RateToIDR = 1;
                }
            }
            return (_validator.ValidCreateObject(paymentVoucher, this, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService) ?
                    _repository.CreateObject(paymentVoucher) : paymentVoucher);
        }

        public PaymentVoucher UpdateObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            if (_cashBankService.GetQueryable().Where(x => x.Id == paymentVoucher.CashBankId).Include(x => x.Currency).FirstOrDefault().Currency.IsBase == true)
            {
                paymentVoucher.RateToIDR = 1;
            }
            return (_validator.ValidUpdateObject(paymentVoucher, this, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService) ? _repository.UpdateObject(paymentVoucher) : paymentVoucher);
        }

        public PaymentVoucher UpdateAmount(PaymentVoucher paymentVoucher)
        {
            return _repository.UpdateObject(paymentVoucher);
        }

        public PaymentVoucher SoftDeleteObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            return (_validator.ValidDeleteObject(paymentVoucher, _paymentVoucherDetailService) ? _repository.SoftDeleteObject(paymentVoucher) : paymentVoucher);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PaymentVoucher CalculateTotalAmount(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
            decimal total = 0;
            decimal totalPPH23 = 0;
            foreach (PaymentVoucherDetail detail in paymentVoucherDetails)
            {
                total += detail.AmountPaid;
                totalPPH23 += detail.PPH23;
            }
            paymentVoucher.TotalPPH23 = totalPPH23;
            paymentVoucher.TotalAmount = total; // + paymentVoucher.BiayaBank + (paymentVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? paymentVoucher.Pembulatan : -paymentVoucher.Pembulatan);
            paymentVoucher = _repository.UpdateObject(paymentVoucher);
            return paymentVoucher;
        }

        public PaymentVoucher ConfirmObject(PaymentVoucher paymentVoucher, DateTime ConfirmationDate, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
            ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            paymentVoucher.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(paymentVoucher, this, _paymentVoucherDetailService, _cashBankService, _payableService, _closingService))
            {
                IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _paymentVoucherDetailService.ConfirmObject(detail, ConfirmationDate, this, _payableService);
                }
                
                _repository.ConfirmObject(paymentVoucher);
                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);

                if (!paymentVoucher.IsGBCH)
                {
                    CashMutation cashMutation = _cashMutationService.CreateCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                    _cashMutationService.CashMutateObject(cashMutation, _cashBankService,_currencyService);
                }
                _generalLedgerJournalService.CreateConfirmationJournalForPaymentVoucher(paymentVoucher, cashBank, _accountService,_paymentVoucherDetailService,
                                             _payableService,_gLNonBaseCurrencyService, _currencyService);

            }
            return paymentVoucher;
        }

        public PaymentVoucher UnconfirmObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
            IClosingService _closingService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            if (_validator.ValidUnconfirmObject(paymentVoucher, _closingService))
            {
                IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _paymentVoucherDetailService.UnconfirmObject(detail, this, _payableService);
                }
                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                if (!paymentVoucher.IsGBCH)
                {
                    IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                    foreach (var cashMutation in cashMutations)
                    {
                        _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService,_currencyService);
                    }
                }
                _generalLedgerJournalService.CreateUnconfirmationJournalForPaymentVoucher(paymentVoucher, cashBank, _accountService,
                                             _paymentVoucherDetailService, _payableService, _gLNonBaseCurrencyService, _currencyService);
                _repository.UnconfirmObject(paymentVoucher);

            }
            return paymentVoucher;
        }

        public PaymentVoucher ReconcileObject(PaymentVoucher paymentVoucher, DateTime ReconciliationDate, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                              ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                              IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
            IClosingService _closingService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            paymentVoucher.ReconciliationDate = ReconciliationDate;
            if (_validator.ValidReconcileObject(paymentVoucher, _paymentVoucherDetailService, _cashBankService, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                _generalLedgerJournalService.CreateReconcileJournalForPaymentVoucher(paymentVoucher, cashBank, _accountService,
                                             _gLNonBaseCurrencyService, _currencyService);
                _repository.ReconcileObject(paymentVoucher);

                _cashMutationService.CashMutateObject(cashMutation, _cashBankService,_currencyService);

                IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach(var paymentVoucherDetail in paymentVoucherDetails)
                {
                    Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);
                    payable.PendingClearanceAmount -= paymentVoucherDetail.Amount;
                    if (payable.PendingClearanceAmount == 0 && payable.RemainingAmount == 0)
                    {
                        payable.IsCompleted = true;
                        payable.CompletionDate = DateTime.Now;
                    }
                    _payableService.UpdateObject(payable);
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher UnreconcileObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                                ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
            IClosingService _closingService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            if (_validator.ValidUnreconcileObject(paymentVoucher, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                _generalLedgerJournalService.CreateUnReconcileJournalForPaymentVoucher(paymentVoucher, cashBank, _accountService,
                                             _gLNonBaseCurrencyService, _currencyService);
                _repository.UnreconcileObject(paymentVoucher);

                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService,_currencyService);
                }

                IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach (var paymentVoucherDetail in paymentVoucherDetails)
                {
                    Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);
                    payable.PendingClearanceAmount += paymentVoucherDetail.Amount;
                    if (payable.PendingClearanceAmount != 0 || payable.RemainingAmount != 0)
                    {
                        payable.IsCompleted = false;
                        payable.CompletionDate = null;
                    }
                    _payableService.UpdateObject(payable);
                }
            }
            return paymentVoucher;
        }
    }
}