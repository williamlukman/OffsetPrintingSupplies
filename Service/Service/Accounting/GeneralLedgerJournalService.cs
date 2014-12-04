using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using System.Data.Entity;
using Core.Interface.Validation;

namespace Service.Service
{
    public class GeneralLedgerJournalService : IGeneralLedgerJournalService
    {
        #region BasicServiceFunctionality
        private IGeneralLedgerJournalRepository _repository;
        private IGeneralLedgerJournalValidator _validator;

        public GeneralLedgerJournalService(IGeneralLedgerJournalRepository _generalLedgerJournalRepository, IGeneralLedgerJournalValidator _generalLedgerJournalValidator)
        {
            _repository = _generalLedgerJournalRepository;
            _validator = _generalLedgerJournalValidator;
        }

        public IGeneralLedgerJournalValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<GeneralLedgerJournal> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<GeneralLedgerJournal> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<GeneralLedgerJournal> GetObjectsByAccountId(int accountId)
        {
            return _repository.GetObjectsByAccountId(accountId);
        }

        public GeneralLedgerJournal GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<GeneralLedgerJournal> GetObjectsBySourceDocument(int accountId, string SourceDocument, int SourceDocumentId)
        {
            return _repository.GetObjectsBySourceDocument(accountId, SourceDocument, SourceDocumentId);
        }

        public GeneralLedgerJournal CreateObject(GeneralLedgerJournal generalLedgerJournal, IAccountService _accountService)
        {
            generalLedgerJournal.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(generalLedgerJournal, _accountService) ? _repository.CreateObject(generalLedgerJournal) : generalLedgerJournal);
        }

        public GeneralLedgerJournal SoftDeleteObject(GeneralLedgerJournal generalLedgerJournal)
        {
            return (_validator.ValidDeleteObject(generalLedgerJournal) ? _repository.SoftDeleteObject(generalLedgerJournal) : generalLedgerJournal);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
        #endregion

        // FINANCE

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForMemorial(Memorial memorial, IMemorialDetailService _memorialDetailService, IAccountService _accountService)
        {
            // User Input Memorial
            #region User Input Memorial

            IList<MemorialDetail> details = _memorialDetailService.GetObjectsByMemorialId(memorial.Id);
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            foreach (var memorialDetail in details)
            {
                GeneralLedgerJournal journal = new GeneralLedgerJournal()
                {
                    AccountId = memorialDetail.AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.Memorial,
                    SourceDocumentId = memorial.Id,
                    TransactionDate = (DateTime)memorial.ConfirmationDate,
                    Status = memorialDetail.Status,
                    Amount = memorialDetail.Amount
                };
                journal = CreateObject(journal, _accountService);
                journals.Add(journal);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForMemorial(Memorial memorial, IMemorialDetailService _memorialDetailService, IAccountService _accountService)
        {
            // Use Input Memorial
            #region User Input Memorial

            IList<MemorialDetail> details = _memorialDetailService.GetObjectsByMemorialId(memorial.Id);
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            foreach (var memorialDetail in details)
            {
                GeneralLedgerJournal journal = new GeneralLedgerJournal()
                {
                    AccountId = memorialDetail.AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.Memorial,
                    SourceDocumentId = memorial.Id,
                    TransactionDate = (DateTime) memorial.ConfirmationDate,
                    Status = (memorialDetail.Status == Constant.GeneralLedgerStatus.Debit) ? Constant.GeneralLedgerStatus.Credit : Constant.GeneralLedgerStatus.Debit,
                    Amount = memorialDetail.Amount
                };
                journal = CreateObject(journal, _accountService);
                journals.Add(journal);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService)
        {
            // if (Amount >= 0) then Debit CashBank, Credit CashBankEquityAdjustment
            // if (Amount < 0) then Debit CashBankAdjustmentExpense, Credit CashBank
            #region if (Amount >= 0) then Debit CashBank, Credit CashBankEquityAdjustment
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            if (cashBankAdjustment.Amount >= 0)
            {
                string LegacyCode = Constant.AccountLegacyCode.CashBank + cashBank.Id.ToString();
                int AccountId = _accountService.GetObjectByLegacyCode(LegacyCode).Id;
                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = cashBankAdjustment.Amount * cashBankAdjustment.ExchangeRateAmount
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                GeneralLedgerJournal creditcashbankequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = cashBankAdjustment.Amount * cashBankAdjustment.ExchangeRateAmount
                };
                creditcashbankequityadjustment = CreateObject(creditcashbankequityadjustment, _accountService);

                journals.Add(debitcashbank);
                journals.Add(creditcashbankequityadjustment);
            }
            #endregion
            #region if (Amount < 0) then Debit CashBankAdjustmentExpense, Credit CashBank
            else
            {
                GeneralLedgerJournal debitcashbankadjustmentexpense = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBankAdjustmentExpense).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Abs(cashBankAdjustment.Amount) * cashBankAdjustment.ExchangeRateAmount
                };
                debitcashbankadjustmentexpense = CreateObject(debitcashbankadjustmentexpense, _accountService);

                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Abs(cashBankAdjustment.Amount) * cashBankAdjustment.ExchangeRateAmount
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                journals.Add(debitcashbankadjustmentexpense);
                journals.Add(creditcashbank);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService)
        {
            // if (Amount >= 0) then Credit CashBank, Debit CashBankEquityAdjustment
            // if (Amount < 0) then Debit CashBank, Credit CashBankAdjustmentExpense
            #region if (Amount >= 0) then Credit CashBank, Debit CashBankEquityAdjustment
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            if (cashBankAdjustment.Amount >= 0)
            {
                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = cashBankAdjustment.Amount * cashBankAdjustment.ExchangeRateAmount
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                GeneralLedgerJournal debitcashbankequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = cashBankAdjustment.GetType().ToString(),
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = cashBankAdjustment.Amount * cashBankAdjustment.ExchangeRateAmount
                };
                debitcashbankequityadjustment = CreateObject(debitcashbankequityadjustment, _accountService);

                journals.Add(creditcashbank);
                journals.Add(debitcashbankequityadjustment);
            }
            #endregion
            #region if (Amount < 0) then Debit CashBank, Credit CashBankAdjustmentExpense
            else
            {
                GeneralLedgerJournal creditcashbankadjustmentexpense = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBankAdjustmentExpense).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Abs(cashBankAdjustment.Amount) * cashBankAdjustment.ExchangeRateAmount
                };
                creditcashbankadjustmentexpense = CreateObject(creditcashbankadjustmentexpense, _accountService);

                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Abs(cashBankAdjustment.Amount) * cashBankAdjustment.ExchangeRateAmount
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                journals.Add(creditcashbankadjustmentexpense);
                journals.Add(debitcashbank);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService)
        {
            // Debit TargetCashBank, Credit SourceCashBank
            #region Debit TargetCashBank, Credit SourceCashBank
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debittargetcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + targetCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = (DateTime)cashBankMutation.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = cashBankMutation.Amount * cashBankMutation.ExchangeRateAmount
            };
            debittargetcashbank = CreateObject(debittargetcashbank, _accountService);

            GeneralLedgerJournal creditsourcecashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + sourceCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = (DateTime)cashBankMutation.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = cashBankMutation.Amount * cashBankMutation.ExchangeRateAmount
            };
            creditsourcecashbank = CreateObject(creditsourcecashbank, _accountService);

            journals.Add(debittargetcashbank);
            journals.Add(creditsourcecashbank);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService)
        {
            // Debit SourceCashBank, Credit TargetCashBank
            #region Debit SourceCashBank, Credit TargetCashBank
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal credittargetcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + targetCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = (DateTime) cashBankMutation.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = cashBankMutation.Amount * cashBankMutation.ExchangeRateAmount
            };
            credittargetcashbank = CreateObject(credittargetcashbank, _accountService);

            GeneralLedgerJournal debitsourcecashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + sourceCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = (DateTime) cashBankMutation.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = cashBankMutation.Amount * cashBankMutation.ExchangeRateAmount
            };
            debitsourcecashbank = CreateObject(debitsourcecashbank, _accountService);

            journals.Add(credittargetcashbank);
            journals.Add(debitsourcecashbank);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService,
                                           IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService)
        {
            // GBCH: Credit GBCHPayable, Cash: Credit CashBank
            // Debit Account Payable, Credit ExchangeGain or Debit ExchangeLost
            #region GBCH: Credit GBCHPayable, Cash: Credit CashBank
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            if (paymentVoucher.IsGBCH)
            {
                GeneralLedgerJournal creditGBCHPayable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = paymentVoucher.TotalAmount * paymentVoucher.RateToIDR
                };
                creditGBCHPayable = CreateObject(creditGBCHPayable, _accountService);
                journals.Add(creditGBCHPayable);
            }
            else
            {
                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = paymentVoucher.TotalAmount * paymentVoucher.RateToIDR
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);
                journals.Add(creditcashbank);
            }

            #endregion
            #region Debit Account Payable, Credit ExchangeGain or Debit ExchangeLost
            IList<PaymentVoucherDetail> pvd = _paymentVoucherDetailService.GetQueryable().Where(x => x.PaymentVoucherId == paymentVoucher.Id & !x.IsDeleted).ToList();
            foreach (var detail in pvd)
            {
                Payable payable = _payableService.GetObjectById(detail.PayableId);
                GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + payable.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = detail.Amount * payable.Rate
                };
                debitaccountpayable = CreateObject(debitaccountpayable, _accountService);
                journals.Add(debitaccountpayable);

                if (payable.Rate < paymentVoucher.RateToIDR)
                {
                    GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = (paymentVoucher.RateToIDR * detail.Rate * detail.Amount) - (payable.Rate * detail.Amount)
                    };
                    debitExchangeLoss = CreateObject(debitExchangeLoss, _accountService);
                    journals.Add(debitExchangeLoss);
                }
                else if (payable.Rate > paymentVoucher.RateToIDR)
                {
                    GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = (payable.Rate * detail.Amount) - (paymentVoucher.RateToIDR * detail.Rate * detail.Amount)
                    };
                    creditExchangeGain = CreateObject(creditExchangeGain, _accountService);
                    journals.Add(creditExchangeGain);
                }
            }      

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService,
                                           IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService)
        {
            // GBCH: Debit GBCHPayable, Cash: Debit CashBank
            // Credit Account Payable, Debit ExchangeGain or Credit ExchangeLost
            #region GBCH: Debit GBCHPayable, Cash: Debit CashBank
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            if (paymentVoucher.IsGBCH)
            {
                GeneralLedgerJournal debitGBCHPayable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = paymentVoucher.TotalAmount * paymentVoucher.RateToIDR
                };
                debitGBCHPayable = CreateObject(debitGBCHPayable, _accountService);
                journals.Add(debitGBCHPayable);
            }
            else
            {
                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = paymentVoucher.TotalAmount * paymentVoucher.RateToIDR
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);
                journals.Add(debitcashbank);
            }
            #endregion
            #region Credit Account Payable, Debit ExchangeGain or Credit ExchangeLost

            IList<PaymentVoucherDetail> pvd = _paymentVoucherDetailService.GetQueryable().Where(x => x.PaymentVoucherId == paymentVoucher.Id && !x.IsDeleted).ToList();
            foreach (var detail in pvd)
            {
                Payable payable = _payableService.GetObjectById(detail.PayableId);
                GeneralLedgerJournal creditaccountpayable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + payable.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = detail.Amount * payable.Rate
                };
                creditaccountpayable = CreateObject(creditaccountpayable, _accountService);
                journals.Add(creditaccountpayable);

                if (payable.Rate < paymentVoucher.RateToIDR)
                {
                    GeneralLedgerJournal creditExchangeLoss = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = (paymentVoucher.RateToIDR * detail.Rate * detail.Amount) - (payable.Rate * detail.Amount)
                    };
                    creditExchangeLoss = CreateObject(creditExchangeLoss, _accountService);
                    journals.Add(creditExchangeLoss);
                }
                else if (payable.Rate > paymentVoucher.RateToIDR)
                {
                    GeneralLedgerJournal debitExchangeGain = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = (payable.Rate * detail.Amount) - (paymentVoucher.RateToIDR * detail.Rate * detail.Amount)
                    };
                    debitExchangeGain = CreateObject(debitExchangeGain, _accountService);
                    journals.Add(debitExchangeGain);
                }
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateReconcileJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService)
        {
            // Debit GBCH, Credit CashBank
            #region Debit GBCH, Credit CashBank
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            GeneralLedgerJournal debitGBCH = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = paymentVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = paymentVoucher.TotalAmount * paymentVoucher.RateToIDR
            };
            debitGBCH = CreateObject(debitGBCH, _accountService);
            journals.Add(debitGBCH);

            GeneralLedgerJournal creditcashBank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = paymentVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = paymentVoucher.TotalAmount * paymentVoucher.RateToIDR
            };
            creditcashBank = CreateObject(creditcashBank, _accountService);
            journals.Add(creditcashBank);
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnReconcileJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService)
        {
            // Credit GBCH, Debit CashBank
            #region Credit GBCH, Debit CashBank

            DateTime unReconcileDate = DateTime.Now;
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            GeneralLedgerJournal creditGBCH = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = paymentVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = paymentVoucher.TotalAmount * paymentVoucher.RateToIDR
            };
            creditGBCH = CreateObject(creditGBCH, _accountService);
            journals.Add(creditGBCH);

            GeneralLedgerJournal debitcashBank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = paymentVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = paymentVoucher.TotalAmount * paymentVoucher.RateToIDR
            };
            debitcashBank = CreateObject(debitcashBank, _accountService);
            journals.Add(debitcashBank);
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService)
        {
            // Credit AccountPayable, Debit User Input
            #region Credit AccountPayable, Debit User Input

            IList<PaymentRequestDetail> details = _paymentRequestDetailService.GetObjectsByPaymentRequestId(paymentRequest.Id);
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            
            GeneralLedgerJournal creditAccountPayable = new GeneralLedgerJournal()
            { 
                AccountId = paymentRequest.AccountPayableId,
                SourceDocument = Constant.GeneralLedgerSource.PaymentRequest,
                SourceDocumentId = paymentRequest.Id,
                TransactionDate = (DateTime)paymentRequest.RequestedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = paymentRequest.Amount * paymentRequest.ExchangeRateAmount
            };
            creditAccountPayable = CreateObject(creditAccountPayable, _accountService);
            journals.Add(creditAccountPayable);

            foreach (var paymentRequestDetail in details)
            {
                GeneralLedgerJournal journal = new GeneralLedgerJournal()
                {
                    AccountId = paymentRequestDetail.AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentRequest,
                    SourceDocumentId = paymentRequest.Id,
                    TransactionDate = (DateTime)paymentRequest.RequestedDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = paymentRequestDetail.Amount * paymentRequest.ExchangeRateAmount
                };
                journal = CreateObject(journal, _accountService);
                journals.Add(journal);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService)
        {
            // Debit AccountPayable, Credit User Input
            #region Debit AccountPayable, Credit User Input

            IList<PaymentRequestDetail> details = _paymentRequestDetailService.GetObjectsByPaymentRequestId(paymentRequest.Id);
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal debitAccountPayable = new GeneralLedgerJournal()
            {
                AccountId = paymentRequest.AccountPayableId,
                SourceDocument = Constant.GeneralLedgerSource.PaymentRequest,
                SourceDocumentId = paymentRequest.Id,
                TransactionDate = paymentRequest.RequestedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = paymentRequest.Amount * paymentRequest.ExchangeRateAmount
            };
            debitAccountPayable = CreateObject(debitAccountPayable, _accountService);
            journals.Add(debitAccountPayable);


            foreach (var paymentRequestDetail in details)
            {
                GeneralLedgerJournal journal = new GeneralLedgerJournal()
                {
                    AccountId = paymentRequestDetail.AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentRequest,
                    SourceDocumentId = paymentRequest.Id,
                    TransactionDate = paymentRequest.RequestedDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = paymentRequestDetail.Amount * paymentRequest.ExchangeRateAmount
                };
                journal = CreateObject(journal, _accountService);
                journals.Add(journal);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseDownPayment(PurchaseDownPayment purchaseDownPayment, IAccountService _accountService)
        {
            // Credit AccountPayable, Debit PiutangLainLain
            #region Credit AccountPayable, Debit PiutangLainLain
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            GeneralLedgerJournal debitpiutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PiutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPayment,
                SourceDocumentId = purchaseDownPayment.Id,
                TransactionDate = (DateTime)purchaseDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseDownPayment.TotalAmount
            };
            debitpiutanglainlain = CreateObject(debitpiutanglainlain, _accountService);

            GeneralLedgerJournal creditpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPayment,
                SourceDocumentId = purchaseDownPayment.Id,
                TransactionDate = (DateTime)purchaseDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseDownPayment.TotalAmount
            };
            creditpayable = CreateObject(creditpayable, _accountService);

            journals.Add(debitpiutanglainlain);
            journals.Add(creditpayable);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseDownPayment(PurchaseDownPayment purchaseDownPayment, IAccountService _accountService)
        {
            // Debit AccountPayable, Credit PiutangLainLain
            #region Debit AccountPayable, Credit PiutangLainLain
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditpiutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PiutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPayment,
                SourceDocumentId = purchaseDownPayment.Id,
                TransactionDate = purchaseDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseDownPayment.TotalAmount
            };
            creditpiutanglainlain = CreateObject(creditpiutanglainlain, _accountService);

            GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPayment,
                SourceDocumentId = purchaseDownPayment.Id,
                TransactionDate = purchaseDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseDownPayment.TotalAmount
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);

            journals.Add(creditpiutanglainlain);
            journals.Add(debitaccountpayable);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseDownPaymentAllocation(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IAccountService _accountService)
        {
            // Debit GoodsPendingClearance, Credit PiutangLainLain
            #region Debit GoodsPendingClearance, Credit PiutangLainLain
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            GeneralLedgerJournal creditpiutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PiutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                SourceDocumentId = purchaseDownPaymentAllocation.Id,
                TransactionDate = (DateTime)purchaseDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseDownPaymentAllocation.TotalAmount
            };
            creditpiutanglainlain = CreateObject(creditpiutanglainlain, _accountService);

            GeneralLedgerJournal debitgoodspendingclearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                SourceDocumentId = purchaseDownPaymentAllocation.Id,
                TransactionDate = (DateTime)purchaseDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseDownPaymentAllocation.TotalAmount
            };
            debitgoodspendingclearance = CreateObject(debitgoodspendingclearance, _accountService);

            journals.Add(creditpiutanglainlain);
            journals.Add(debitgoodspendingclearance);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseDownPaymentAllocation(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IAccountService _accountService)
        {
            // Credit GoodsPendingClearance, Debit PiutangLainLain
            #region Credit GoodsPendingClearance, Debit PiutangLainLain
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            GeneralLedgerJournal debitpiutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PiutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                SourceDocumentId = purchaseDownPaymentAllocation.Id,
                TransactionDate = (DateTime)purchaseDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseDownPaymentAllocation.TotalAmount
            };
            debitpiutanglainlain = CreateObject(debitpiutanglainlain, _accountService);

            GeneralLedgerJournal creditgoodspendingclearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                SourceDocumentId = purchaseDownPaymentAllocation.Id,
                TransactionDate = (DateTime)purchaseDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseDownPaymentAllocation.TotalAmount
            };
            creditgoodspendingclearance = CreateObject(creditgoodspendingclearance, _accountService);

            journals.Add(debitpiutanglainlain);
            journals.Add(creditgoodspendingclearance);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseAllowance(PurchaseAllowance purchaseAllowance, CashBank cashBank, IAccountService _accountService)
        {
            // Debit AccountPayable, Credit PurchaseAllowance
            #region Debit AccountPayable, Credit PurchaseAllowance
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseAllowance,
                SourceDocumentId = purchaseAllowance.Id,
                TransactionDate = (DateTime)purchaseAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseAllowance.TotalAmount
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);

            GeneralLedgerJournal creditpurchaseallowance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PurchaseAllowance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseAllowance,
                SourceDocumentId = purchaseAllowance.Id,
                TransactionDate = (DateTime)purchaseAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseAllowance.TotalAmount
            };
            creditpurchaseallowance = CreateObject(creditpurchaseallowance, _accountService);

            journals.Add(debitaccountpayable);
            journals.Add(creditpurchaseallowance);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseAllowance(PurchaseAllowance purchaseAllowance, CashBank cashBank, IAccountService _accountService)
        {
            // Debit PurchaseAllowance, Credit AccountPayable
            #region Debit PurchaseAllowance, Credit AccountPayable
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseAllowance,
                SourceDocumentId = purchaseAllowance.Id,
                TransactionDate = purchaseAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseAllowance.TotalAmount
            };
            creditaccountpayable = CreateObject(creditaccountpayable, _accountService);

            GeneralLedgerJournal debitpurchaseallowance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PurchaseAllowance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseAllowance,
                SourceDocumentId = purchaseAllowance.Id,
                TransactionDate = purchaseAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseAllowance.TotalAmount
            };
            debitpurchaseallowance = CreateObject(debitpurchaseallowance, _accountService);

            journals.Add(creditaccountpayable);
            journals.Add(debitpurchaseallowance);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService, 
                                           IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService)
        {
            // GBCH: Debit GBCHReceivable, CashBank: DebitCashBank
            // Credit AccountReceivable, Credit ExchangeGain or Debit ExchangeLost
            #region GBCH: Debit GBCHReceivable, CashBank: DebitCashBank
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            //debit cashbank credit AR
            if (receiptVoucher.IsGBCH)
            {
                GeneralLedgerJournal debitGBCHReceivable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = receiptVoucher.TotalAmount * receiptVoucher.RateToIDR
                };
                debitGBCHReceivable = CreateObject(debitGBCHReceivable, _accountService);
                journals.Add(debitGBCHReceivable);
            }
            else
            {
                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + receiptVoucher.CashBankId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = receiptVoucher.TotalAmount * receiptVoucher.RateToIDR
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);
                journals.Add(debitcashbank);
            }

            #endregion
            #region Credit AccountReceivable, Credit ExchangeGain or Debit ExchangeLost

            IList<ReceiptVoucherDetail> rvd = _receiptVoucherDetailService.GetQueryable().Where(x => x.ReceiptVoucherId == receiptVoucher.Id && !x.IsDeleted).ToList();
            foreach (var detail in rvd)
            {
                Receivable receivable = _receivableService.GetObjectById(detail.ReceivableId);
                GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + receivable.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = detail.Amount * receivable.Rate
                };
                creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);
                journals.Add(creditaccountreceivable);

                if (receivable.Rate < (receiptVoucher.RateToIDR * detail.Rate))
                {
                    GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = (receiptVoucher.RateToIDR * detail.Rate * detail.Amount) - (receivable.Rate * detail.Amount)
                    };
                    creditExchangeGain = CreateObject(creditExchangeGain, _accountService);
                    journals.Add(creditExchangeGain);
                }
                else if (receivable.Rate > (receiptVoucher.RateToIDR * detail.Rate))
                {
                    GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = (receivable.Rate * detail.Amount) - (receiptVoucher.RateToIDR * detail.Rate * detail.Amount)
                    };
                    debitExchangeLoss = CreateObject(debitExchangeLoss, _accountService);
                    journals.Add(debitExchangeLoss);
                }
            }
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService, 
                                           IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService)
        {
            // GBCH: Credit GBCHReceivable, CashBank: Credit CashBank
            // Debit AccountReceivable, Debit ExchangeGain or Credit ExchangeLost
            #region Credit GBCHReceivable, CashBank: Credit CashBank
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;
            if (receiptVoucher.IsGBCH)
            {
                GeneralLedgerJournal creditGBCH = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = receiptVoucher.TotalAmount * receiptVoucher.RateToIDR
                };
                creditGBCH = CreateObject(creditGBCH, _accountService);
                journals.Add(creditGBCH);
            }
            else
            {
                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = receiptVoucher.TotalAmount * receiptVoucher.RateToIDR
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);
                journals.Add(creditcashbank);
            }

            #endregion
            #region Debit AccountReceivable, Debit ExchangeGain or Credit ExchangeLost

            IList<ReceiptVoucherDetail> rvd = _receiptVoucherDetailService.GetQueryable().Where(x => x.ReceiptVoucherId == receiptVoucher.Id && !x.IsDeleted).ToList();
            foreach (var detail in rvd)
            {
                Receivable receivable = _receivableService.GetObjectById(detail.ReceivableId);
                GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + receivable.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = detail.Amount * receivable.Rate
                };
                debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);
                journals.Add(debitaccountreceivable);

                if (receivable.Rate < (receiptVoucher.RateToIDR * detail.Rate))
                {
                    GeneralLedgerJournal debitExchangeGain = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = receiptVoucher.ReceiptDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = (receiptVoucher.RateToIDR * detail.Rate * detail.Amount) - (receivable.Rate * detail.Amount) 
                    };
                    debitExchangeGain = CreateObject(debitExchangeGain, _accountService);
                    journals.Add(debitExchangeGain);
                }
                else if (receivable.Rate > (receiptVoucher.RateToIDR * detail.Rate))
                {
                    GeneralLedgerJournal creditExchangeLoss = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = receiptVoucher.ReceiptDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount =  (receivable.Rate * detail.Amount) - (receiptVoucher.RateToIDR * detail.Rate * detail.Amount)
                    };
                    creditExchangeLoss = CreateObject(creditExchangeLoss, _accountService);
                    journals.Add(creditExchangeLoss);
                }
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateReconcileJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService)
        {
            // Credit GBCH, Debit CashBank
            #region Credit GBCH, Debit CashBank

            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            GeneralLedgerJournal creditGBCH = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = receiptVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = receiptVoucher.TotalAmount * receiptVoucher.RateToIDR
            };
            creditGBCH = CreateObject(creditGBCH, _accountService);
            journals.Add(creditGBCH);

            GeneralLedgerJournal debitcashBank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + receiptVoucher.CashBankId).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = receiptVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = receiptVoucher.TotalAmount * receiptVoucher.RateToIDR
            };
            debitcashBank = CreateObject(debitcashBank, _accountService);
            journals.Add(debitcashBank);
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnReconcileJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService)
        {
            // Debit GBCH, Credit CashBank
            #region Debit GBCH, Credit CashBank

            DateTime unReconcileDate = DateTime.Now;
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            GeneralLedgerJournal debitGBCH = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = receiptVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = receiptVoucher.TotalAmount * receiptVoucher.RateToIDR
            };
            debitGBCH = CreateObject(debitGBCH, _accountService);
            journals.Add(debitGBCH);

            GeneralLedgerJournal creditcashBank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + receiptVoucher.CashBankId).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = receiptVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = receiptVoucher.TotalAmount * receiptVoucher.RateToIDR
            };
            creditcashBank = CreateObject(creditcashBank, _accountService);
            journals.Add(creditcashBank);
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesDownPayment(SalesDownPayment salesDownPayment, IAccountService _accountService)
        {
            // Debit AccountReceivable, Credit HutangLainLain
            #region Debit AccountReceivable, Credit HutangLainLain
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPayment.Id,
                TransactionDate = (DateTime)salesDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = salesDownPayment.TotalAmount
            };
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            GeneralLedgerJournal credithutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPayment.Id,
                TransactionDate = (DateTime)salesDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = salesDownPayment.TotalAmount
            };
            credithutanglainlain = CreateObject(credithutanglainlain, _accountService);

            journals.Add(debitaccountreceivable);
            journals.Add(credithutanglainlain);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesDownPayment(SalesDownPayment salesDownPayment, IAccountService _accountService)
        {
            // Credit AccountReceivable, Debit HutangLainLain
            #region Credit AccountReceivable, Debit HutangLainLain
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPayment.Id,
                TransactionDate = salesDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = salesDownPayment.TotalAmount
            };
            creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);

            GeneralLedgerJournal debithutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPayment.Id,
                TransactionDate = salesDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = salesDownPayment.TotalAmount
            };
            debithutanglainlain = CreateObject(debithutanglainlain, _accountService);

            journals.Add(creditaccountreceivable);
            journals.Add(debithutanglainlain);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesDownPaymentAllocation(SalesDownPaymentAllocation salesDownPaymentAllocation, IAccountService _accountService)
        {
            // Debit HutangLainLain, Credit Revenue
            #region Debit HutangLainLain, Credit Revenue
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debithutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                SourceDocumentId = salesDownPaymentAllocation.Id,
                TransactionDate = (DateTime)salesDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = salesDownPaymentAllocation.TotalAmount
            };
            debithutanglainlain = CreateObject(debithutanglainlain, _accountService);

            GeneralLedgerJournal creditrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPaymentAllocation.Id,
                TransactionDate = (DateTime)salesDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = salesDownPaymentAllocation.TotalAmount
            };
            creditrevenue = CreateObject(creditrevenue, _accountService);

            journals.Add(debithutanglainlain);
            journals.Add(creditrevenue);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesDownPaymentAllocation(SalesDownPaymentAllocation salesDownPaymentAllocation, IAccountService _accountService)
        {
            // Credit HutangLainLain, Debit Revenue
            #region Credit HutangLainLain, Debit Revenue
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal credithutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                SourceDocumentId = salesDownPaymentAllocation.Id,
                TransactionDate = (DateTime)salesDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = salesDownPaymentAllocation.TotalAmount
            };
            credithutanglainlain = CreateObject(credithutanglainlain, _accountService);

            GeneralLedgerJournal debitrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPaymentAllocation.Id,
                TransactionDate = (DateTime)salesDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = salesDownPaymentAllocation.TotalAmount
            };
            debitrevenue = CreateObject(debitrevenue, _accountService);

            journals.Add(credithutanglainlain);
            journals.Add(debitrevenue);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesAllowance(SalesAllowance salesAllowance, CashBank cashBank, IAccountService _accountService)
        {
            // Debit SalesAllowance, Credit AccountReceivable
            #region Debit SalesAllowance, Credit AccountReceivable
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitsalesallowance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.SalesAllowance).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesAllowance,
                SourceDocumentId = salesAllowance.Id,
                TransactionDate = (DateTime)salesAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = salesAllowance.TotalAmount
            };
            debitsalesallowance = CreateObject(debitsalesallowance, _accountService);

            GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesAllowance,
                SourceDocumentId = salesAllowance.Id,
                TransactionDate = (DateTime)salesAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = salesAllowance.TotalAmount
            };
            creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);

            journals.Add(debitsalesallowance);
            journals.Add(creditaccountreceivable);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesAllowance(SalesAllowance salesAllowance, CashBank cashBank, IAccountService _accountService)
        {
            // Credit SalesAllowance, Debit AccountReceivable
            #region Credit SalesAllowance, Debit AccountReceivable
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditsalesallowance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.SalesAllowance).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesAllowance,
                SourceDocumentId = salesAllowance.Id,
                TransactionDate = salesAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = salesAllowance.TotalAmount
            };
            creditsalesallowance = CreateObject(creditsalesallowance, _accountService);

            GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesAllowance,
                SourceDocumentId = salesAllowance.Id,
                TransactionDate = salesAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = salesAllowance.TotalAmount
            };
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            journals.Add(creditsalesallowance);
            journals.Add(debitaccountreceivable);

            return journals;
            #endregion
        }

        // MASTER & STOCK

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService)
        {
            // if (stockAdjustmentTotal >= 0) then Debit Raw, Credit StockEquityAdjusment
            // if (stockAdjustmentTotal < 0) then Debit StockAdjustmentExpense, Credit Raw
            #region if (stockAdjustmentTotal >= 0) then Debit Raw, Credit StockEquityAdjustment
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            if (stockAdjustment.Total >= 0)
            {
                GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = stockAdjustment.Total
                };
                debitraw = CreateObject(debitraw, _accountService);

                GeneralLedgerJournal creditstockequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = stockAdjustment.Total
                };
                creditstockequityadjustment = CreateObject(creditstockequityadjustment, _accountService);

                journals.Add(debitraw);
                journals.Add(creditstockequityadjustment);
            }
            #endregion
            #region if (stockAdjustmentTotal < 0) then Debit StockAdjustmentExpense, Credit Raw
            else
            {
                GeneralLedgerJournal debitstockadjustmentexpense = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.StockAdjustmentExpense).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Abs(stockAdjustment.Total)
                };
                debitstockadjustmentexpense = CreateObject(debitstockadjustmentexpense, _accountService);

                GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Abs(stockAdjustment.Total)
                };
                creditraw = CreateObject(creditraw, _accountService);

                journals.Add(debitstockadjustmentexpense);
                journals.Add(creditraw);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService)
        {
            // if (stockAdjustmentTotal >= 0) then Credit Raw, Debit StockEquityAdjustment
            // if (stockAdjustmentTotal < 0) then Credit StockAdjustmentExpense, Debit Raw
            #region if (stockAdjustmentTotal >= 0) then Credit Raw, Debit StockEquityAdjustment
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            if (stockAdjustment.Total >= 0)
            {
                GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = stockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = stockAdjustment.Total
                };
                creditraw = CreateObject(creditraw, _accountService);

                GeneralLedgerJournal debitstockequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = stockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = stockAdjustment.Total
                };
                debitstockequityadjustment = CreateObject(debitstockequityadjustment, _accountService);

                journals.Add(creditraw);
                journals.Add(debitstockequityadjustment);
            }
            #endregion
            #region if (stockAdjustmentTotal < 0) then Credit StockAdjustmentExpense, Debit Raw
            else
            {
                GeneralLedgerJournal creditstockadjustmentexpense = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.StockAdjustmentExpense).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = stockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Abs(stockAdjustment.Total)
                };
                creditstockadjustmentexpense = CreateObject(creditstockadjustmentexpense, _accountService);

                GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = stockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Abs(stockAdjustment.Total)
                };
                debitraw = CreateObject(debitraw, _accountService);

                journals.Add(creditstockadjustmentexpense);
                journals.Add(debitraw);
            }
            #endregion
            return journals;
        }

        // PURCHASE OPERATION

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseReceival(PurchaseReceival purchaseReceival, IAccountService _accountService)
        {
            // Debit Raw, Credit GoodsPendingClearance
            #region Debit Raw, Credit GoodsPendingClearance
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = (DateTime)purchaseReceival.ReceivalDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseReceival.TotalAmount * purchaseReceival.ExchangeRateAmount
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditgoodsPendingclearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = (DateTime)purchaseReceival.ReceivalDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseReceival.TotalAmount * purchaseReceival.ExchangeRateAmount
            };
            creditgoodsPendingclearance = CreateObject(creditgoodsPendingclearance, _accountService);

            journals.Add(debitraw);
            journals.Add(creditgoodsPendingclearance);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseReceival(PurchaseReceival purchaseReceival, IAccountService _accountService)
        {
            // Credit Raw, Debit GoodsPendingClearance
            #region Credit Raw, Debit GoodsPendingClearance
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            DateTime UnconfirmationDate = DateTime.Now;
            GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = purchaseReceival.ReceivalDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseReceival.TotalAmount * purchaseReceival.ExchangeRateAmount
            };
            creditraw = CreateObject(creditraw, _accountService);

            GeneralLedgerJournal debitgoodsPendingclearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = purchaseReceival.ReceivalDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseReceival.TotalAmount * purchaseReceival.ExchangeRateAmount
            };
            debitgoodsPendingclearance = CreateObject(debitgoodsPendingclearance, _accountService);

            journals.Add(creditraw);
            journals.Add(debitgoodsPendingclearance);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseInvoice(PurchaseInvoice purchaseInvoice, PurchaseReceival purchaseReceival, IAccountService _accountService)
        {
            // Debit GoodsPendingClearance, Credit AccountPayable
            // Debit TaxPayable, Debit ExchangeLoss or Credit ExchangeGain
            #region Debit GoodsPendingClearance, Credit AccountPayable
            decimal PreTax = purchaseInvoice.AmountPayable * 100 / (100 + purchaseInvoice.Tax);
            decimal Tax = purchaseInvoice.AmountPayable - PreTax;
            decimal Discount = PreTax * purchaseInvoice.Discount / 100;

            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + purchaseInvoice.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseInvoice.AmountPayable * purchaseInvoice.ExchangeRateAmount
            };
            creditaccountpayable = CreateObject(creditaccountpayable, _accountService);
            journals.Add(creditaccountpayable);

            GeneralLedgerJournal debitGoodsPendingClearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = PreTax * purchaseReceival.ExchangeRateAmount
            };
            debitGoodsPendingClearance = CreateObject(debitGoodsPendingClearance, _accountService);
            journals.Add(debitGoodsPendingClearance);

            #endregion
            #region Debit TaxPayable, Debit ExchangeLoss or Credit ExchangeGain

            if (Tax > 0)
            {
                GeneralLedgerJournal debitTaxPayable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.TaxPayable).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                    SourceDocumentId = purchaseInvoice.Id,
                    TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Tax * purchaseInvoice.ExchangeRateAmount
                };
                debitTaxPayable = CreateObject(debitTaxPayable, _accountService);
                journals.Add(debitTaxPayable);
            }

            if (purchaseInvoice.ExchangeRateAmount > purchaseReceival.ExchangeRateAmount)
            {
                GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                { 
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                    SourceDocumentId = purchaseInvoice.Id,
                    TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = PreTax * purchaseInvoice.ExchangeRateAmount - PreTax * purchaseReceival.ExchangeRateAmount
                };
                debitExchangeLoss = CreateObject(debitExchangeLoss, _accountService);
                journals.Add(debitExchangeLoss);
            }
            else if (purchaseInvoice.ExchangeRateAmount < purchaseReceival.ExchangeRateAmount)
            { 
                GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                    SourceDocumentId = purchaseInvoice.Id,
                    TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = PreTax * purchaseReceival.ExchangeRateAmount - PreTax * purchaseInvoice.ExchangeRateAmount
                };
                creditExchangeGain = CreateObject(creditExchangeGain, _accountService);
                journals.Add(creditExchangeGain);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseInvoice(PurchaseInvoice purchaseInvoice, PurchaseReceival purchaseReceival, IAccountService _accountService)
        {
            // Credit GoodsPendingClearance, Debit AccountPayable
            // Credit TaxPayable, Credit ExchangeLoss or Debit ExchangeGain
            #region Credit GoodsPendingClearance, Debit AccountPayable
            decimal PreTax = purchaseInvoice.AmountPayable * 100 / (100 + purchaseInvoice.Tax);
            decimal Tax = purchaseInvoice.AmountPayable - PreTax;
            decimal Discount = PreTax * purchaseInvoice.Discount / 100;
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + purchaseInvoice.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseInvoice.AmountPayable * purchaseInvoice.ExchangeRateAmount
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);
            journals.Add(debitaccountpayable);

            GeneralLedgerJournal credittGoodsPendingClearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = PreTax * purchaseReceival.ExchangeRateAmount
            };
            credittGoodsPendingClearance = CreateObject(credittGoodsPendingClearance, _accountService);
            journals.Add(credittGoodsPendingClearance);

            #endregion
            #region Credit TaxPayable, Credit ExchangeLoss or Debit ExchangeGain
            if (Tax > 0)
            {
                GeneralLedgerJournal creditTaxPayable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.TaxPayable).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                    SourceDocumentId = purchaseInvoice.Id,
                    TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Tax * purchaseInvoice.ExchangeRateAmount
                };
                creditTaxPayable = CreateObject(creditTaxPayable, _accountService);
                journals.Add(creditTaxPayable);
            }

            if (purchaseInvoice.ExchangeRateAmount > purchaseReceival.ExchangeRateAmount)
            {
                GeneralLedgerJournal creditExchangeLoss = new GeneralLedgerJournal()
                { 
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                    SourceDocumentId = purchaseInvoice.Id,
                    TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = PreTax * purchaseInvoice.ExchangeRateAmount - PreTax * purchaseReceival.ExchangeRateAmount
                };
                creditExchangeLoss = CreateObject(creditExchangeLoss, _accountService);
                journals.Add(creditExchangeLoss);
            }
            else if (purchaseInvoice.ExchangeRateAmount < purchaseReceival.ExchangeRateAmount)
            {
                GeneralLedgerJournal debitExchangeGain = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                    SourceDocumentId = purchaseInvoice.Id,
                    TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = PreTax * purchaseReceival.ExchangeRateAmount - PreTax * purchaseInvoice.ExchangeRateAmount
                };
                debitExchangeGain = CreateObject(debitExchangeGain, _accountService);
                journals.Add(debitExchangeGain);
            }
            #endregion
            return journals;
        }

        // SALES OPERATION

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForDeliveryOrder(DeliveryOrder deliveryOrder, IAccountService _accountService)
        {
            // Debit COGS, Credit Raw
            #region Debit COGS, Credit Raw
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitcogs = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGS).Id,
                SourceDocument = Constant.GeneralLedgerSource.DeliveryOrder,
                SourceDocumentId = deliveryOrder.Id,
                TransactionDate = (DateTime)deliveryOrder.DeliveryDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = deliveryOrder.TotalCOGS
            };
            debitcogs = CreateObject(debitcogs, _accountService);

            GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.DeliveryOrder,
                SourceDocumentId = deliveryOrder.Id,
                TransactionDate = (DateTime)deliveryOrder.DeliveryDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = deliveryOrder.TotalCOGS
            };
            creditraw = CreateObject(creditraw, _accountService);

            journals.Add(debitcogs);
            journals.Add(creditraw);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForDeliveryOrder(DeliveryOrder deliveryOrder, IAccountService _accountService)
        {
            // Credit COGS, Debit Raw
            #region Credit COGS, Debit Raw
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditcogs = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGS).Id,
                SourceDocument = Constant.GeneralLedgerSource.DeliveryOrder,
                SourceDocumentId = deliveryOrder.Id,
                TransactionDate = deliveryOrder.DeliveryDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = deliveryOrder.TotalCOGS
            };
            creditcogs = CreateObject(creditcogs, _accountService);

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.DeliveryOrder,
                SourceDocumentId = deliveryOrder.Id,
                TransactionDate = deliveryOrder.DeliveryDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = deliveryOrder.TotalCOGS
            };
            debitraw = CreateObject(debitraw, _accountService);

            journals.Add(creditcogs);
            journals.Add(debitraw);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateReconciliationJournalForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, IAccountService _accountService)
        {
            // Credit Raw, Debit SampleAndTrialExpense
            #region Credit Raw , Debit SampleAndTrialExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrder.Id,
                TransactionDate = PushDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = temporaryDeliveryOrder.TotalWasteCOGS
            };
            creditraw = CreateObject(creditraw, _accountService);

            GeneralLedgerJournal debitsampleandtrialexpense = new GeneralLedgerJournal()
            {
                // TOBECHANGED TrialAndSampleExpense
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrder.Id,
                TransactionDate = PushDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = temporaryDeliveryOrder.TotalWasteCOGS
            };
            debitsampleandtrialexpense = CreateObject(debitsampleandtrialexpense, _accountService);

            journals.Add(creditraw);
            journals.Add(debitsampleandtrialexpense);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnreconciliationJournalForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrder temporaryDeliveryOrder, IAccountService _accountService)
        {
            // Debit Raw, Credit SampleAndTrialExpense
            #region Debit Raw , Credit SampleAndTrialExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            DateTime UnreconcileDate = DateTime.Now;

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrder.Id,
                TransactionDate = temporaryDeliveryOrder.PushDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = temporaryDeliveryOrder.TotalWasteCOGS
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditsampleandtrialexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrder.Id,
                TransactionDate = temporaryDeliveryOrder.PushDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = temporaryDeliveryOrder.TotalWasteCOGS
            };
            creditsampleandtrialexpense = CreateObject(creditsampleandtrialexpense, _accountService);

            journals.Add(debitraw);
            journals.Add(creditsampleandtrialexpense);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForTemporaryDeliveryOrderClearanceWaste(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IAccountService _accountService)
        {
            // Credit Raw (Inventory), Debit SampleAndTrialExpense (SalesExpense)
            #region Credit Raw , Debit SampleAndTrialExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrderClearance.Id,
                TransactionDate = temporaryDeliveryOrderClearance.ClearanceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = temporaryDeliveryOrderClearance.TotalWasteCoGS
            };
            creditraw = CreateObject(creditraw, _accountService);

            GeneralLedgerJournal debitsampleandtrialexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.SampleAndTrialExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrderClearance.Id,
                TransactionDate = temporaryDeliveryOrderClearance.ClearanceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = temporaryDeliveryOrderClearance.TotalWasteCoGS
            };
            debitsampleandtrialexpense = CreateObject(debitsampleandtrialexpense, _accountService);

            journals.Add(creditraw);
            journals.Add(debitsampleandtrialexpense);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForTemporaryDeliveryOrderClearanceWaste(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IAccountService _accountService)
        {
            // Debit Raw (Inventory), Credit SampleAndTrialExpense (SalesExpense)
            #region Debit Raw , Credit SampleAndTrialExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            DateTime UnconfirmDate = DateTime.Now;

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrderClearance.Id,
                TransactionDate = temporaryDeliveryOrderClearance.ClearanceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = temporaryDeliveryOrderClearance.TotalWasteCoGS
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditsampleandtrialexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.SampleAndTrialExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrderClearance.Id,
                TransactionDate = temporaryDeliveryOrderClearance.ClearanceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = temporaryDeliveryOrderClearance.TotalWasteCoGS
            };
            creditsampleandtrialexpense = CreateObject(creditsampleandtrialexpense, _accountService);

            journals.Add(debitraw);
            journals.Add(creditsampleandtrialexpense);
            return journals;
            #endregion
        }
        
        public IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesInvoice(SalesInvoice salesInvoice, IAccountService _accountService,IExchangeRateService _exchangeRateService,ICurrencyService _currencyService)
        {
            // Debit AccountReceivable, Debit Discount, Debit TaxPayable, Credit Revenue
            // Debit COS, Credit FinishedGoods
            #region Debit AccountReceivable, Debit Discount, Debit TaxPayable, Credit Revenue for fixed rate
            decimal PreTax = salesInvoice.AmountReceivable * 100 / (100 + salesInvoice.Tax);
            decimal Tax = salesInvoice.AmountReceivable - PreTax;
            decimal Discount = PreTax * salesInvoice.Discount / 100;
            decimal Rate = 0;
            Currency currency = _currencyService.GetObjectById(salesInvoice.CurrencyId);
            if (currency.IsBase == true)
            {
                Rate = 1;
            }
            else
            {
                Rate = _exchangeRateService.GetQueryable().Where(x => x.ExRateDate <= salesInvoice.ConfirmationDate.Value)
                .OrderByDescending(x => x.ExRateDate).FirstOrDefault().Rate;
            }
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + salesInvoice.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = (DateTime)salesInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = salesInvoice.AmountReceivable * Rate
            };  
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            journals.Add(debitaccountreceivable);

            if (Discount > 0)
            {
                GeneralLedgerJournal debitdiscount = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Discount).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                   TransactionDate = (DateTime)salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Discount * Rate
                };
                debitdiscount = CreateObject(debitdiscount, _accountService);
                journals.Add(debitdiscount);
            }

            if (Tax > 0)
            {
                GeneralLedgerJournal credittaxpayable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.TaxPayable).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = (DateTime)salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Tax * Rate
                };
                credittaxpayable = CreateObject(credittaxpayable, _accountService);
                journals.Add(credittaxpayable);
            }

            GeneralLedgerJournal creditrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = (DateTime)salesInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = PreTax * Rate
            };
            creditrevenue = CreateObject(creditrevenue, _accountService);

            journals.Add(creditrevenue);
            #endregion
            #region Debit AccountReceivable, Debit Discount, Debit TaxPayable, Credit Revenue for custom rate

            ///*
            //decimal Tax = salesInvoice.AmountReceivable * salesInvoice.Tax / (100 - salesInvoice.Tax);
            //decimal PreTax = salesInvoice.AmountReceivable * 100 / (100 - salesInvoice.Tax);
            //decimal Discount = PreTax * salesInvoice.Discount / (100 - salesInvoice.Discount);
            //*/
            //decimal PreTax = salesInvoice.AmountReceivable * 100 / (100 + salesInvoice.Tax);
            //decimal Tax = salesInvoice.AmountReceivable - PreTax;
            //decimal Discount = PreTax * salesInvoice.Discount / 100;
            //decimal Rate =  _exchangeRateService.GetQueryable().Where(x => x.ExRateDate <= salesInvoice.ConfirmationDate.Value)
            //    .OrderByDescending(x =>x.ExRateDate).FirstOrDefault().Rate;
            //IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            
            //GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            //{
            //    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
            //    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //    SourceDocumentId = salesInvoice.Id,
            //    TransactionDate = (DateTime)salesInvoice.InvoiceDate,
            //    Status = Constant.GeneralLedgerStatus.Debit,
            //    Amount = salesInvoice.AmountReceivable * salesInvoice.ExchangeRateAmount
            //};
            //debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            //journals.Add(debitaccountreceivable);

             
           
            //if (Discount > 0)
            //{
            //    GeneralLedgerJournal debitdiscount = new GeneralLedgerJournal()
            //    {
            //        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Discount).Id,
            //        SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //        SourceDocumentId = salesInvoice.Id,
            //        TransactionDate = (DateTime)salesInvoice.InvoiceDate,
            //        Status = Constant.GeneralLedgerStatus.Debit,
            //        Amount = Discount * salesInvoice.ExchangeRateAmount
            //    };
            //    debitdiscount = CreateObject(debitdiscount, _accountService);
            //    journals.Add(debitdiscount);
            //}

            //if (Tax > 0)
            //{
            //    GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            //    {
            //        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
            //        SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //        SourceDocumentId = salesInvoice.Id,
            //        TransactionDate = (DateTime)salesInvoice.InvoiceDate,
            //        Status = Constant.GeneralLedgerStatus.Credit,
            //        Amount = Tax * salesInvoice.ExchangeRateAmount
            //    };
            //    creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);

            //    journals.Add(creditaccountreceivable);

            //    GeneralLedgerJournal debitaccountreceivable2 = new GeneralLedgerJournal()
            //    {
            //        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
            //        SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //        SourceDocumentId = salesInvoice.Id,
            //        TransactionDate = (DateTime)salesInvoice.InvoiceDate,
            //        Status = Constant.GeneralLedgerStatus.Debit,
            //        Amount = Tax * (_currencyService.GetObjectById(salesInvoice.CurrencyId).IsBase == true ? 1 : Rate)
            //    };
            //    debitaccountreceivable2 = CreateObject(debitaccountreceivable2, _accountService);

            //    journals.Add(debitaccountreceivable2);

            //    GeneralLedgerJournal credittaxpayable = new GeneralLedgerJournal()
            //    {
            //        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.TaxPayable).Id,
            //        SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //        SourceDocumentId = salesInvoice.Id,
            //        TransactionDate = (DateTime)salesInvoice.InvoiceDate,
            //        Status = Constant.GeneralLedgerStatus.Credit,
            //        Amount = Tax * (_currencyService.GetObjectById(salesInvoice.CurrencyId).IsBase == true ? 1 : Rate)
            //    };
            //    credittaxpayable = CreateObject(credittaxpayable, _accountService);
            //    journals.Add(credittaxpayable);
            //}

            //GeneralLedgerJournal creditrevenue = new GeneralLedgerJournal()
            //{
            //    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
            //    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //    SourceDocumentId = salesInvoice.Id,
            //    TransactionDate = (DateTime)salesInvoice.InvoiceDate,
            //    Status = Constant.GeneralLedgerStatus.Credit,
            //    Amount = PreTax * salesInvoice.ExchangeRateAmount
            //};
            //creditrevenue = CreateObject(creditrevenue, _accountService);

            //journals.Add(creditrevenue);
            #endregion
            #region Debit COS, Credit FinishedGoods
            if (salesInvoice.TotalCOS > 0)
            {
                GeneralLedgerJournal debitCOS = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGS).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = (DateTime)salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = salesInvoice.TotalCOS
                };
                debitCOS = CreateObject(debitCOS, _accountService);

                GeneralLedgerJournal creditFinishedGoods = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = (DateTime)salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = salesInvoice.TotalCOS
                };
                creditFinishedGoods = CreateObject(creditFinishedGoods, _accountService);

                journals.Add(debitCOS);
                journals.Add(creditFinishedGoods);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesInvoice(SalesInvoice salesInvoice, IAccountService _accountService, IExchangeRateService _exchangeRateService, ICurrencyService _currencyService)
        {
            // Credit AccountReceivable, Credit Discount, Credit TaxPayable, Debit Revenue
            // Credit COS, Debit FinishedGoods
            #region Credit AccountReceivable, Credit Discount, Credit TaxPayable, Debit Revenue with master exchangeRate
            decimal PreTax = salesInvoice.AmountReceivable * 100 / (100 + salesInvoice.Tax);
            decimal Tax = salesInvoice.AmountReceivable - PreTax;
            decimal Discount = PreTax * salesInvoice.Discount / 100;
            decimal Rate = 0;
            if (_currencyService.GetObjectById(salesInvoice.CurrencyId).IsBase == true)
            {
                Rate = 1;
            }
            else
            {
                Rate = _exchangeRateService.GetLatestRate(salesInvoice.ConfirmationDate.Value,salesInvoice.CurrencyId).Rate;
            }

            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + salesInvoice.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = salesInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = salesInvoice.AmountReceivable * Rate
            };
            creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);
            journals.Add(creditaccountreceivable);

            if (Discount > 0)
            {
                GeneralLedgerJournal creditdiscount = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Discount).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Discount * Rate
                };
                creditdiscount = CreateObject(creditdiscount, _accountService);
                journals.Add(creditdiscount);
            }

            if (Tax > 0)
            {
                GeneralLedgerJournal debittaxpayabale = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.TaxPayable).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Tax * Rate
                };
                debittaxpayabale = CreateObject(debittaxpayabale, _accountService);
                journals.Add(debittaxpayabale);
            }

            GeneralLedgerJournal debitrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = salesInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = PreTax * Rate
            };
            debitrevenue = CreateObject(debitrevenue, _accountService);

            journals.Add(debitrevenue);

            #endregion
            #region Credit AccountReceivable, Credit Discount, Credit TaxPayable, Debit Revenue with custom Rate
            //decimal PreTax = salesInvoice.AmountReceivable * 100 / (100 + salesInvoice.Tax);
            //decimal Tax = salesInvoice.AmountReceivable - PreTax;
            //decimal Discount = PreTax * salesInvoice.Discount / 100;
            //decimal Rate = _exchangeRateService.GetQueryable().Where(x => x.ExRateDate <= salesInvoice.ConfirmationDate.Value)
            //   .OrderByDescending(x => x.ExRateDate).FirstOrDefault().Rate;

            //IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            //DateTime UnconfirmationDate = DateTime.Now;

            //GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            //{
            //    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
            //    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //    SourceDocumentId = salesInvoice.Id,
            //    TransactionDate = salesInvoice.InvoiceDate,
            //    Status = Constant.GeneralLedgerStatus.Credit,
            //    Amount = salesInvoice.AmountReceivable * salesInvoice.ExchangeRateAmount
            //};
            //creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);
            //journals.Add(creditaccountreceivable);

            //if (Discount > 0)
            //{
            //    GeneralLedgerJournal creditdiscount = new GeneralLedgerJournal()
            //    {
            //        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Discount).Id,
            //        SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //        SourceDocumentId = salesInvoice.Id,
            //        TransactionDate = salesInvoice.InvoiceDate,
            //        Status = Constant.GeneralLedgerStatus.Credit,
            //        Amount = Discount * salesInvoice.ExchangeRateAmount
            //    };
            //    creditdiscount = CreateObject(creditdiscount, _accountService);
            //    journals.Add(creditdiscount);
            //}

            //if (Tax > 0)
            //{
            //    GeneralLedgerJournal debetccountreceivable = new GeneralLedgerJournal()
            //    {
            //        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
            //        SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //        SourceDocumentId = salesInvoice.Id,
            //        TransactionDate = (DateTime)salesInvoice.InvoiceDate,
            //        Status = Constant.GeneralLedgerStatus.Debit,
            //        Amount = Tax * salesInvoice.ExchangeRateAmount
            //    };
            //    debetccountreceivable = CreateObject(debetccountreceivable, _accountService);

            //    journals.Add(debetccountreceivable);

            //    GeneralLedgerJournal creditaccountreceivable2 = new GeneralLedgerJournal()
            //    {
            //        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
            //        SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //        SourceDocumentId = salesInvoice.Id,
            //        TransactionDate = (DateTime)salesInvoice.InvoiceDate,
            //        Status = Constant.GeneralLedgerStatus.Credit,
            //        Amount = Tax * (_currencyService.GetObjectById(salesInvoice.CurrencyId).IsBase == true ? 1 : Rate)
            //    };
            //    creditaccountreceivable2 = CreateObject(creditaccountreceivable2, _accountService);

            //    journals.Add(creditaccountreceivable2);

            //    GeneralLedgerJournal debittaxpayabale = new GeneralLedgerJournal()
            //    {
            //        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.TaxPayable).Id,
            //        SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //        SourceDocumentId = salesInvoice.Id,
            //        TransactionDate = salesInvoice.InvoiceDate,
            //        Status = Constant.GeneralLedgerStatus.Debit,
            //        Amount = Tax * (_currencyService.GetObjectById(salesInvoice.CurrencyId).IsBase == true ? 1 : Rate)
            //    };
            //    debittaxpayabale = CreateObject(debittaxpayabale, _accountService);
            //    journals.Add(debittaxpayabale);
            //}

            //GeneralLedgerJournal debitrevenue = new GeneralLedgerJournal()
            //{
            //    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
            //    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
            //    SourceDocumentId = salesInvoice.Id,
            //    TransactionDate = salesInvoice.InvoiceDate,
            //    Status = Constant.GeneralLedgerStatus.Debit,
            //    Amount = PreTax * salesInvoice.ExchangeRateAmount
            //};
            //debitrevenue = CreateObject(debitrevenue, _accountService);

            //journals.Add(debitrevenue);
            #endregion
            #region Credit COS, Debit FinishedGoods
            if (salesInvoice.TotalCOS > 0)
            {
                GeneralLedgerJournal creditCOS = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGS).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = salesInvoice.TotalCOS
                };
                creditCOS = CreateObject(creditCOS, _accountService);

                GeneralLedgerJournal debitFinishedGoods = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = salesInvoice.TotalCOS
                };
                debitFinishedGoods = CreateObject(debitFinishedGoods, _accountService);

                journals.Add(creditCOS);
                journals.Add(debitFinishedGoods);
            }
            #endregion
            return journals;
        }

        // MANUFACTURING RECOVERY

        public IList<GeneralLedgerJournal> CreateFinishedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService)
        {
            // Credit Raw/Stock (Core, Compound, Accessories), Debit FinishedGoods/ManufacturedGoods (Roller)
            #region Credit Raw (Core, Compound, Accessories), Debit FinishedGoods (Roller)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = (DateTime)recoveryOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = recoveryOrderDetail.TotalCost
            };
            creditraw = CreateObject(creditraw, _accountService);

            GeneralLedgerJournal debitfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = (DateTime)recoveryOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = recoveryOrderDetail.TotalCost
            };
            debitfinishedgoods = CreateObject(debitfinishedgoods, _accountService);

            journals.Add(creditraw);
            journals.Add(debitfinishedgoods);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnfinishedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService)
        {
            // Debit Raw (Core, Compound, Accessories), Credit FinishedGoods (Roller)
            #region Debit Raw (Core, Compound, Accessories), Credit FinishedGoods (Roller)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnfinishedDate = DateTime.Now;

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = recoveryOrderDetail.FinishedDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = recoveryOrderDetail.TotalCost
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = recoveryOrderDetail.FinishedDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = recoveryOrderDetail.TotalCost
            };
            creditfinishedgoods = CreateObject(creditfinishedgoods, _accountService);

            journals.Add(debitraw);
            journals.Add(creditfinishedgoods);
            return journals;
            #endregion
         }

        public IList<GeneralLedgerJournal> CreateRejectedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService)
        {
            // Credit Raw (Core, Compound, Accessories), Debit RecoveryExpense
            #region Credit Raw (Core, Compound, Accessories), Debit RecoveryExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = (DateTime)recoveryOrderDetail.RejectedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = recoveryOrderDetail.TotalCost
            };
            creditraw = CreateObject(creditraw, _accountService);

            GeneralLedgerJournal debitrecoveryexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = (DateTime)recoveryOrderDetail.RejectedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = recoveryOrderDetail.TotalCost
            };
            debitrecoveryexpense = CreateObject(debitrecoveryexpense, _accountService);

            journals.Add(creditraw);
            journals.Add(debitrecoveryexpense);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUndoRejectedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService)
        {
                // Debit Raw (Core, Compound, Accessories), Credit RecoveryExpense
            #region Debit Raw (Core, Compound, Accessories), Credit RecoveryExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UndoRejectDate = DateTime.Now;

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = recoveryOrderDetail.RejectedDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = recoveryOrderDetail.TotalCost
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditrecoveryexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = recoveryOrderDetail.RejectedDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = recoveryOrderDetail.TotalCost
            };
            creditrecoveryexpense = CreateObject(creditrecoveryexpense, _accountService);

            journals.Add(debitraw);
            journals.Add(creditrecoveryexpense);
            return journals;
            #endregion
        }

        // MANUFACTURING CONVERSION

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForBlendingWorkOrder(BlendingWorkOrder blendingWorkOrder, IAccountService _accountService, decimal TotalCost)
        {
            // Credit Raw (Source Items), Debit FinishedGoods (Chemical)
            #region Credit Raw (Source Items), Debit FinishedGoods (Chemical)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlendingWorkOrder,
                SourceDocumentId = blendingWorkOrder.Id,
                TransactionDate = blendingWorkOrder.BlendingDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = TotalCost,
            };
            creditraw = CreateObject(creditraw, _accountService);

            GeneralLedgerJournal debitfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlendingWorkOrder,
                SourceDocumentId = blendingWorkOrder.Id,
                TransactionDate = blendingWorkOrder.BlendingDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = TotalCost,
            };
            debitfinishedgoods = CreateObject(debitfinishedgoods, _accountService);

            journals.Add(creditraw);
            journals.Add(debitfinishedgoods);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForBlendingWorkOrder(BlendingWorkOrder blendingWorkOrder, IAccountService _accountService, decimal TotalCost)
        {
            // Debit Raw (Source Items), Credit FinishedGoods (Chemical)
            #region Debit Raw (Source Items), Credit FinishedGoods (Chemical)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnfinishedDate = DateTime.Now;

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlendingWorkOrder,
                SourceDocumentId = blendingWorkOrder.Id,
                TransactionDate = blendingWorkOrder.BlendingDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = TotalCost
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlendingWorkOrder,
                SourceDocumentId = blendingWorkOrder.Id,
                TransactionDate = blendingWorkOrder.BlendingDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = TotalCost
            };
            creditfinishedgoods = CreateObject(creditfinishedgoods, _accountService);

            journals.Add(debitraw);
            journals.Add(creditfinishedgoods);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateFinishedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService)
        {
            // Credit Raw (RollBlanket, Bars, Adhesive), Debit FinishedGoods (Blanket)
            #region Credit Raw (RollBlanket, Bars, Adhesive), Debit FinishedGoods (Blanket)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = (DateTime)blanketOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = blanketOrderDetail.TotalCost
            };
            creditraw = CreateObject(creditraw, _accountService);

            GeneralLedgerJournal debitfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = (DateTime)blanketOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = blanketOrderDetail.TotalCost
            };
            debitfinishedgoods = CreateObject(debitfinishedgoods, _accountService);

            journals.Add(creditraw);
            journals.Add(debitfinishedgoods);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnfinishedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService)
        {
            // Debit Raw (RollBlanket, Bars, Adhesive), Credit FinishedGoods (Blanket)
            #region Debit Raw (RollBlanket, Bars, Adhesive), Credit FinishedGoods (Blanket)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnfinishedDate = DateTime.Now;

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = blanketOrderDetail.FinishedDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = blanketOrderDetail.TotalCost
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = blanketOrderDetail.FinishedDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = blanketOrderDetail.TotalCost
            };
            creditfinishedgoods = CreateObject(creditfinishedgoods, _accountService);

            journals.Add(debitraw);
            journals.Add(creditfinishedgoods);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateRejectedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService)
        {
            // Credit Raw (RollBlanket, Bars, Adhesive), Debit ConversionExpense
            #region Credit Raw (RollBlanket, Bars, Adhesive), Debit ConversionExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = (DateTime)blanketOrderDetail.RejectedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = blanketOrderDetail.TotalCost
            };
            creditraw = CreateObject(creditraw, _accountService);

            GeneralLedgerJournal debitconversionexpense = new GeneralLedgerJournal()
            {
                // TODO: Use ConversionExpense
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = (DateTime)blanketOrderDetail.RejectedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = blanketOrderDetail.TotalCost
            };
            debitconversionexpense = CreateObject(debitconversionexpense, _accountService);

            journals.Add(creditraw);
            journals.Add(debitconversionexpense);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUndoRejectedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService)
        {
            // Debit Raw (RollBlanket, Bars, Adhesive), Credit ConversionExpense
            #region Debit Raw (RollBlanket, Bars, Adhesive), Credit ConversionExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UndoRejectDate = DateTime.Now;

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = blanketOrderDetail.RejectedDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = blanketOrderDetail.TotalCost
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditconversionexpense = new GeneralLedgerJournal()
            {
                // TODO: Use ConversionExpense
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = blanketOrderDetail.RejectedDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = blanketOrderDetail.TotalCost
            };
            creditconversionexpense = CreateObject(creditconversionexpense, _accountService);

            journals.Add(debitraw);
            journals.Add(creditconversionexpense);
            return journals;
            #endregion
        }
    }
}
