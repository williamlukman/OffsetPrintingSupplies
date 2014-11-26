using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Service
{
    public class ReceiptVoucherService : IReceiptVoucherService
    {
        private IReceiptVoucherRepository _repository;
        private IReceiptVoucherValidator _validator;

        public ReceiptVoucherService(IReceiptVoucherRepository _receiptVoucherRepository, IReceiptVoucherValidator _receiptVoucherValidator)
        {
            _repository = _receiptVoucherRepository;
            _validator = _receiptVoucherValidator;
        }

        public IReceiptVoucherValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ReceiptVoucher> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ReceiptVoucher> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<ReceiptVoucher> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public ReceiptVoucher GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<ReceiptVoucher> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public ReceiptVoucher CreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                            IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            receiptVoucher.Errors = new Dictionary<String, String>();
            if (_cashBankService.GetQueryable().Where(x => x.Id == receiptVoucher.CashBankId).Include("Currency").FirstOrDefault().Currency.IsBase == true)
            {
                receiptVoucher.RateToIDR = 1;
            }
            return (_validator.ValidCreateObject(receiptVoucher, this, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService) ?
                    _repository.CreateObject(receiptVoucher) : receiptVoucher);
        }

        public ReceiptVoucher CalculateTotalAmount(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            IList<ReceiptVoucherDetail> receiptVoucherDetails = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
            decimal total = 0;
            foreach (ReceiptVoucherDetail detail in receiptVoucherDetails)
            {
                total += detail.AmountPaid;
            }
            receiptVoucher.TotalAmount = total;
            receiptVoucher = _repository.UpdateObject(receiptVoucher);
            return receiptVoucher;
        }
        public ReceiptVoucher CreateObject(int cashBankId, int contactId, DateTime receiptDate, decimal totalAmount, bool IsGBCH, DateTime DueDate, bool IsBank,
                                    IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                    IContactService _contactService, ICashBankService _cashBankService)
        {
            ReceiptVoucher receiptVoucher = new ReceiptVoucher
            {
                CashBankId = cashBankId,
                ContactId = contactId,
                ReceiptDate = receiptDate,
                TotalAmount = totalAmount,
                IsGBCH = IsGBCH,
                DueDate = DueDate,
                //IsBank = IsBank
            };
            return this.CreateObject(receiptVoucher, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);
        }

        public ReceiptVoucher UpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            if (_cashBankService.GetQueryable().Where(x => x.Id == receiptVoucher.CashBankId).Include("Currency").FirstOrDefault().Currency.IsBase == true)
            {
                receiptVoucher.RateToIDR = 1;
            }
            return (_validator.ValidUpdateObject(receiptVoucher, this, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService) ? _repository.UpdateObject(receiptVoucher) : receiptVoucher);
        }

        public ReceiptVoucher UpdateAmount(ReceiptVoucher receiptVoucher)
        {
            return _repository.UpdateObject(receiptVoucher);
        }

        public ReceiptVoucher SoftDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            return (_validator.ValidDeleteObject(receiptVoucher, _receiptVoucherDetailService) ? _repository.SoftDeleteObject(receiptVoucher) : receiptVoucher);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public ReceiptVoucher ConfirmObject(ReceiptVoucher receiptVoucher, DateTime ConfirmationDate, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                            ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                            ICurrencyService _currencyService,ISalesInvoiceService _salesInvoiceService,IExchangeRateService _exchangeRateService,IReceiptVoucherService _receiptVoucherService)
        {
            receiptVoucher.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(receiptVoucher, this, _receiptVoucherDetailService, _cashBankService, _receivableService, _closingService))
            {
                IList<ReceiptVoucherDetail> details = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    var receiptvoucherdetail =  _receiptVoucherDetailService.ConfirmObject(detail, ConfirmationDate, this, _receivableService);
                }
                _repository.ConfirmObject(receiptVoucher);
                _repository.UpdateObject(receiptVoucher);

                if (!receiptVoucher.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(receiptVoucher.CashBankId);
                    CashMutation cashMutation = _cashMutationService.CreateCashMutationForReceiptVoucher(receiptVoucher, cashBank);
                    _cashMutationService.CashMutateObject(cashMutation, _cashBankService,_currencyService);
                    _generalLedgerJournalService.CreateConfirmationJournalForReceiptVoucher(receiptVoucher, cashBank, _accountService, _receiptVoucherDetailService, _receiptVoucherService);
                }
            }
            return receiptVoucher;
        }

        public ReceiptVoucher UnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                            ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                            ICurrencyService _currencyService,IExchangeRateService _exchangeRateService,IReceiptVoucherService _receiptVoucherService)
        {
            if (_validator.ValidUnconfirmObject(receiptVoucher, _receiptVoucherDetailService, _cashBankService, _closingService))
            {
                IList<ReceiptVoucherDetail> details = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _receiptVoucherDetailService.UnconfirmObject(detail, this, _receivableService);
                }
                _repository.UnconfirmObject(receiptVoucher);

                if (!receiptVoucher.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(receiptVoucher.CashBankId);
                    IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForReceiptVoucher(receiptVoucher, cashBank);
                    foreach (var cashMutation in cashMutations)
                    {
                        _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService,_currencyService);
                    }
                    _generalLedgerJournalService.CreateUnconfirmationJournalForReceiptVoucher(receiptVoucher, cashBank, _accountService, _receiptVoucherDetailService, _receiptVoucherService);
                }
            }
            return receiptVoucher;
        }

        public ReceiptVoucher ReconcileObject(ReceiptVoucher receiptVoucher, DateTime ReconciliationDate, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                              ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                              IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                              ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, ISalesInvoiceService _salesInvoiceService, IReceiptVoucherService _receiptVoucherService)
        {
            receiptVoucher.ReconciliationDate = ReconciliationDate;
            if (_validator.ValidReconcileObject(receiptVoucher, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(receiptVoucher.CashBankId);
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForReceiptVoucher(receiptVoucher, cashBank);

                _generalLedgerJournalService.CreateConfirmationJournalForReceiptVoucher(receiptVoucher, cashBank, _accountService, _receiptVoucherDetailService, _receiptVoucherService);
                _repository.ReconcileObject(receiptVoucher);
                _cashMutationService.CashMutateObject(cashMutation, _cashBankService,_currencyService);

                IList<ReceiptVoucherDetail> receiptVoucherDetails = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
                foreach(var receiptVoucherDetail in receiptVoucherDetails)
                {
                    Receivable receivable = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);
                    receivable.PendingClearanceAmount -= receiptVoucherDetail.Amount;
                    if (receivable.PendingClearanceAmount == 0 && receivable.RemainingAmount == 0)
                    {
                        receivable.IsCompleted = true;
                        receivable.CompletionDate = DateTime.Now;
                    }
                    _receivableService.UpdateObject(receivable);
                }

            }
            return receiptVoucher;
        }

        public ReceiptVoucher UnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                                ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                                ICurrencyService _currencyService,IExchangeRateService _exchangeRateService,IReceiptVoucherService _receiptVoucherService)
        {
            if (_validator.ValidUnreconcileObject(receiptVoucher, _receiptVoucherDetailService, _cashBankService, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(receiptVoucher.CashBankId);
                _generalLedgerJournalService.CreateUnconfirmationJournalForReceiptVoucher(receiptVoucher, cashBank, _accountService, _receiptVoucherDetailService, _receiptVoucherService);
                _repository.UnreconcileObject(receiptVoucher);

                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForReceiptVoucher(receiptVoucher, cashBank);
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService,_currencyService);
                }

                IList<ReceiptVoucherDetail> receiptVoucherDetails = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
                foreach (var receiptVoucherDetail in receiptVoucherDetails)
                {
                    Receivable receivable = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);
                    receivable.PendingClearanceAmount += receiptVoucherDetail.Amount;
                    if (receivable.PendingClearanceAmount != 0 || receivable.RemainingAmount != 0)
                    {
                        receivable.IsCompleted = false;
                        receivable.CompletionDate = null;
                    }
                    _receivableService.UpdateObject(receivable);
                }
            }
            return receiptVoucher;
        }
    }
}