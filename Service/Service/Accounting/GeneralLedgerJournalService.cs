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
                    Amount = Math.Round(memorialDetail.Amount, 2)
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
                    Amount = Math.Round(memorialDetail.Amount, 2)
                };
                journal = CreateObject(journal, _accountService);
                journals.Add(journal);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, 
            IAccountService _accountService,ICurrencyService _currencyService,IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
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
                    Amount = Math.Round(cashBankAdjustment.Amount * cashBankAdjustment.ExchangeRateAmount, 2)
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency debitcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = cashBankAdjustment.Amount,
                    };
                    debitcashbank2 = _gLNonBaseCurrencyService.CreateObject(debitcashbank2, _accountService);
                }

                GeneralLedgerJournal creditcashbankequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(cashBankAdjustment.Amount * cashBankAdjustment.ExchangeRateAmount, 2)
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
                    Amount = Math.Round(Math.Abs(cashBankAdjustment.Amount) * cashBankAdjustment.ExchangeRateAmount, 2)
                };
                debitcashbankadjustmentexpense = CreateObject(debitcashbankadjustmentexpense, _accountService);

                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round( Math.Abs(cashBankAdjustment.Amount) * cashBankAdjustment.ExchangeRateAmount, 2)
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency creditcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = Math.Abs(cashBankAdjustment.Amount)
                    };
                    creditcashbank2 = _gLNonBaseCurrencyService.CreateObject(creditcashbank2, _accountService);
                }

                journals.Add(debitcashbankadjustmentexpense);
                journals.Add(creditcashbank);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService
            ,ICurrencyService _currencyService,IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
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
                    Amount = Math.Round( cashBankAdjustment.Amount * cashBankAdjustment.ExchangeRateAmount, 2)
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency creditcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = cashBankAdjustment.Amount
                    };
                    creditcashbank2 = _gLNonBaseCurrencyService.CreateObject(creditcashbank2, _accountService);
                }

                GeneralLedgerJournal debitcashbankequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(cashBankAdjustment.Amount * cashBankAdjustment.ExchangeRateAmount, 2)
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
                    Amount = Math.Round(Math.Abs(cashBankAdjustment.Amount) * cashBankAdjustment.ExchangeRateAmount, 2)
                };
                creditcashbankadjustmentexpense = CreateObject(creditcashbankadjustmentexpense, _accountService);

                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = cashBankAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Math.Abs(cashBankAdjustment.Amount) * cashBankAdjustment.ExchangeRateAmount, 2)
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency debitcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = cashBankAdjustment.Amount
                    };
                    debitcashbank2 = _gLNonBaseCurrencyService.CreateObject(debitcashbank2, _accountService);
                }

                journals.Add(creditcashbankadjustmentexpense);
                journals.Add(debitcashbank);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForInterestAdjustment(InterestAdjustment interestAdjustment, CashBank cashBank,
            IAccountService _accountService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            // if (Amount >= 0) then Debit CashBank, Credit PendapatanBungaBank
            // if (Amount < 0) then Debit BiayaBungaBank, Credit CashBank
            #region if (Amount >= 0) then Debit CashBank, Credit PendapatanBungaBank
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            decimal Total = (interestAdjustment.PendapatanJasaAmount + interestAdjustment.PendapatanBungaAmount + interestAdjustment.PengembalianPiutangAmount) -
                             (interestAdjustment.BiayaAdminAmount + interestAdjustment.BiayaBungaAmount);

            if (interestAdjustment.PendapatanJasaAmount > 0)
            {
                decimal Amount = interestAdjustment.PendapatanJasaAmount;
                string LegacyCode = Constant.AccountLegacyCode.CashBank + cashBank.Id.ToString();
                int AccountId = _accountService.GetObjectByLegacyCode(LegacyCode).Id;
                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = (DateTime)interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency debitcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = Amount,
                    };
                    debitcashbank2 = _gLNonBaseCurrencyService.CreateObject(debitcashbank2, _accountService);
                }

                GeneralLedgerJournal creditPendapatanJasa = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PendapatanJasaGiro).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = (DateTime)interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                creditPendapatanJasa = CreateObject(creditPendapatanJasa, _accountService);

                journals.Add(debitcashbank);
                journals.Add(creditPendapatanJasa);
            }
            if (interestAdjustment.PendapatanBungaAmount > 0)
            {
                decimal Amount = interestAdjustment.PendapatanBungaAmount;
                string LegacyCode = Constant.AccountLegacyCode.CashBank + cashBank.Id.ToString();
                int AccountId = _accountService.GetObjectByLegacyCode(LegacyCode).Id;
                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = (DateTime)interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency debitcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = Amount,
                    };
                    debitcashbank2 = _gLNonBaseCurrencyService.CreateObject(debitcashbank2, _accountService);
                }

                GeneralLedgerJournal creditPendapatanBunga = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PendapatanBungaBank).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = (DateTime)interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                creditPendapatanBunga = CreateObject(creditPendapatanBunga, _accountService);

                journals.Add(debitcashbank);
                journals.Add(creditPendapatanBunga);
            }
            if (interestAdjustment.PengembalianPiutangAmount > 0)
            {
                decimal Amount = interestAdjustment.PengembalianPiutangAmount;
                string LegacyCode = Constant.AccountLegacyCode.CashBank + cashBank.Id.ToString();
                int AccountId = _accountService.GetObjectByLegacyCode(LegacyCode).Id;
                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = (DateTime)interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency debitcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = Amount,
                    };
                    debitcashbank2 = _gLNonBaseCurrencyService.CreateObject(debitcashbank2, _accountService);
                }

                GeneralLedgerJournal creditPengembalianPiutang = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PiutangLainLain).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = (DateTime)interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                creditPengembalianPiutang = CreateObject(creditPengembalianPiutang, _accountService);

                journals.Add(debitcashbank);
                journals.Add(creditPengembalianPiutang);
            }
            #endregion
            #region if (Amount < 0) then Debit BiayaBungaBank, Credit CashBank
            if (interestAdjustment.BiayaBungaAmount > 0)
            {
                decimal Amount = interestAdjustment.BiayaBungaAmount;
                GeneralLedgerJournal debitBiayaBungaBank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.InterestExpense).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = (DateTime)interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Math.Abs(Amount) * interestAdjustment.ExchangeRateAmount, 2)
                };
                debitBiayaBungaBank = CreateObject(debitBiayaBungaBank, _accountService);

                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = (DateTime)interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Math.Abs(Amount) * interestAdjustment.ExchangeRateAmount, 2)
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency creditcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = Math.Abs(Amount)
                    };
                    creditcashbank2 = _gLNonBaseCurrencyService.CreateObject(creditcashbank2, _accountService);
                }

                journals.Add(debitBiayaBungaBank);
                journals.Add(creditcashbank);
            }
            if (interestAdjustment.BiayaAdminAmount > 0)
            {
                decimal Amount = interestAdjustment.BiayaAdminAmount;
                GeneralLedgerJournal debitBiayaAdmin = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.BiayaAdminBank).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = (DateTime)interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Math.Abs(Amount) * interestAdjustment.ExchangeRateAmount, 2)
                };
                debitBiayaAdmin = CreateObject(debitBiayaAdmin, _accountService);

                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = (DateTime)interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Math.Abs(Amount) * interestAdjustment.ExchangeRateAmount, 2)
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency creditcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = Math.Abs(Amount)
                    };
                    creditcashbank2 = _gLNonBaseCurrencyService.CreateObject(creditcashbank2, _accountService);
                }

                journals.Add(debitBiayaAdmin);
                journals.Add(creditcashbank);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForInterestAdjustment(InterestAdjustment interestAdjustment, CashBank cashBank, IAccountService _accountService,
                ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            // if (Amount >= 0) then Credit CashBank, Debit PendapatanBungaBank
            // if (Amount < 0) then Debit CashBank, Credit BiayaBungaBank
            #region if (Amount >= 0) then Credit CashBank, Debit PendapatanBungaBank
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            decimal Total = (interestAdjustment.PendapatanJasaAmount + interestAdjustment.PendapatanBungaAmount + interestAdjustment.PengembalianPiutangAmount) -
                             (interestAdjustment.BiayaAdminAmount + interestAdjustment.BiayaBungaAmount);

            if (interestAdjustment.PendapatanBungaAmount > 0)
            {
                decimal Amount = interestAdjustment.PendapatanBungaAmount;
                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency creditcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = Amount
                    };
                    creditcashbank2 = _gLNonBaseCurrencyService.CreateObject(creditcashbank2, _accountService);
                }

                GeneralLedgerJournal debitPendapatanBungaBank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PendapatanBungaBank).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                debitPendapatanBungaBank = CreateObject(debitPendapatanBungaBank, _accountService);

                journals.Add(creditcashbank);
                journals.Add(debitPendapatanBungaBank);
            }
            if (interestAdjustment.PendapatanJasaAmount > 0)
            {
                decimal Amount = interestAdjustment.PendapatanJasaAmount;
                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency creditcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = Amount
                    };
                    creditcashbank2 = _gLNonBaseCurrencyService.CreateObject(creditcashbank2, _accountService);
                }

                GeneralLedgerJournal debitPendapatanJasa = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PendapatanJasaGiro).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                debitPendapatanJasa = CreateObject(debitPendapatanJasa, _accountService);

                journals.Add(creditcashbank);
                journals.Add(debitPendapatanJasa);
            }
            if (interestAdjustment.PengembalianPiutangAmount > 0)
            {
                decimal Amount = interestAdjustment.PengembalianPiutangAmount;
                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency creditcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = Amount
                    };
                    creditcashbank2 = _gLNonBaseCurrencyService.CreateObject(creditcashbank2, _accountService);
                }

                GeneralLedgerJournal debitPengembalianPiutang = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PiutangLainLain).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Amount * interestAdjustment.ExchangeRateAmount, 2)
                };
                debitPengembalianPiutang = CreateObject(debitPengembalianPiutang, _accountService);

                journals.Add(creditcashbank);
                journals.Add(debitPengembalianPiutang);
            }
            #endregion
            #region if (Amount < 0) then Debit CashBank, Credit BiayaBungaBank
            if (interestAdjustment.BiayaBungaAmount > 0)
            {
                decimal Amount = interestAdjustment.BiayaBungaAmount;
                GeneralLedgerJournal creditBiayaBungaBank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.InterestExpense).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Math.Abs(Amount) * interestAdjustment.ExchangeRateAmount, 2)
                };
                creditBiayaBungaBank = CreateObject(creditBiayaBungaBank, _accountService);

                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Math.Abs(Amount) * interestAdjustment.ExchangeRateAmount, 2)
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency debitcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = Amount
                    };
                    debitcashbank2 = _gLNonBaseCurrencyService.CreateObject(debitcashbank2, _accountService);
                }

                journals.Add(creditBiayaBungaBank);
                journals.Add(debitcashbank);
            }
            if (interestAdjustment.BiayaAdminAmount > 0)
            {
                decimal Amount = interestAdjustment.BiayaAdminAmount;
                GeneralLedgerJournal creditBiayaAdmin = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.BiayaAdminBank).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Math.Abs(Amount) * interestAdjustment.ExchangeRateAmount, 2)
                };
                creditBiayaAdmin = CreateObject(creditBiayaAdmin, _accountService);

                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.BankAdministration,
                    SourceDocumentId = interestAdjustment.Id,
                    TransactionDate = interestAdjustment.InterestDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Math.Abs(Amount) * interestAdjustment.ExchangeRateAmount, 2)
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    GLNonBaseCurrency debitcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = Amount
                    };
                    debitcashbank2 = _gLNonBaseCurrencyService.CreateObject(debitcashbank2, _accountService);
                }

                journals.Add(creditBiayaAdmin);
                journals.Add(debitcashbank);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService,
                                           ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService)
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
                Amount = Math.Round(cashBankMutation.Amount * cashBankMutation.ExchangeRateAmount, 2)
            };
            debittargetcashbank = CreateObject(debittargetcashbank, _accountService);

            GeneralLedgerJournal creditsourcecashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + sourceCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = (DateTime)cashBankMutation.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(cashBankMutation.Amount * cashBankMutation.ExchangeRateAmount, 2)
            };
            creditsourcecashbank = CreateObject(creditsourcecashbank, _accountService);

            if (_currencyService.GetObjectById(sourceCashBank.CurrencyId).IsBase == false)
            {
                GLNonBaseCurrency creditsourcecashbank2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditsourcecashbank.Id,
                    CurrencyId = sourceCashBank.CurrencyId,
                    Amount = cashBankMutation.Amount
                };
                creditsourcecashbank2 = _glNonBaseCurrencyService.CreateObject(creditsourcecashbank2, _accountService);

                GLNonBaseCurrency debittargetcashbank2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debittargetcashbank.Id,
                    CurrencyId = targetCashBank.CurrencyId,
                    Amount = cashBankMutation.Amount
                };
                debittargetcashbank2 = _glNonBaseCurrencyService.CreateObject(debittargetcashbank2, _accountService);
            }

            journals.Add(debittargetcashbank);
            journals.Add(creditsourcecashbank);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService,
                                           ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService)
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
                Amount = Math.Round(cashBankMutation.Amount * cashBankMutation.ExchangeRateAmount, 2)
            };
            credittargetcashbank = CreateObject(credittargetcashbank, _accountService);

            GeneralLedgerJournal debitsourcecashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + sourceCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = (DateTime) cashBankMutation.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(cashBankMutation.Amount * cashBankMutation.ExchangeRateAmount, 2)
            };
            debitsourcecashbank = CreateObject(debitsourcecashbank, _accountService);

            if (_currencyService.GetObjectById(sourceCashBank.CurrencyId).IsBase == false)
            {
                GLNonBaseCurrency debitsourcecashbank2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitsourcecashbank.Id,
                    CurrencyId = sourceCashBank.CurrencyId,
                    Amount = cashBankMutation.Amount
                };
                debitsourcecashbank2 = _glNonBaseCurrencyService.CreateObject(debitsourcecashbank2, _accountService);

                GLNonBaseCurrency credittargetcashbank2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = credittargetcashbank.Id,
                    CurrencyId = targetCashBank.CurrencyId,
                    Amount = cashBankMutation.Amount
                };
                credittargetcashbank2 = _glNonBaseCurrencyService.CreateObject(credittargetcashbank2, _accountService);
            }

            journals.Add(credittargetcashbank);
            journals.Add(debitsourcecashbank);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService,
                                           IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService,
                                           ICurrencyService _currencyService)
        {
            // GBCH: Credit GBCHPayable, Cash: Credit CashBank
            // Debit Account Payable, Credit ExchangeGain or Debit ExchangeLost

            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency cashBankCurrency = _currencyService.GetObjectById(cashBank.CurrencyId);
            
            #region Debit Account Payable, Credit ExchangeGain or Debit ExchangeLost

            decimal TotalAP = 0;
            decimal TotalGainLoss = 0;
            decimal TotalPPH23 = 0;
            decimal TotalPPH21 = 0;
            IList<PaymentVoucherDetail> pvd = _paymentVoucherDetailService.GetQueryable().Where(x => x.PaymentVoucherId == paymentVoucher.Id & !x.IsDeleted).ToList();
            foreach (var detail in pvd)
            {
                TotalPPH23 += detail.PPH23;
                TotalPPH21 += detail.PPH21;
                Payable payable = _payableService.GetObjectById(detail.PayableId);
                Currency payableCurrency = _currencyService.GetObjectById(payable.CurrencyId);
                GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + payable.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round( detail.Amount * payable.Rate, 2)
                };
                TotalAP += debitaccountpayable.Amount;
                debitaccountpayable = CreateObject(debitaccountpayable, _accountService);
                if (payableCurrency.IsBase == false)
                {
                    GLNonBaseCurrency debitaccountpayable2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitaccountpayable.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = detail.Amount,
                    };
                    debitaccountpayable2 = _gLNonBaseCurrencyService.CreateObject(debitaccountpayable2, _accountService);
                }

                if (payable.Rate < paymentVoucher.RateToIDR)
                {
                    GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round( (paymentVoucher.RateToIDR * detail.Rate * detail.Amount) - (payable.Rate * detail.Amount), 2)
                    };
                    TotalGainLoss -= debitExchangeLoss.Amount;
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
                        Amount = Math.Round( (payable.Rate * detail.Amount) - (paymentVoucher.RateToIDR * detail.Rate * detail.Amount), 2)
                    };
                    TotalGainLoss += creditExchangeGain.Amount;
                    creditExchangeGain = CreateObject(creditExchangeGain, _accountService);
                    journals.Add(creditExchangeGain);
                }

                // Debit GBCH/CashBank, Credit HutangPPh23
                if (detail.PPH23 > 0)
                {
                    decimal PPH23 = (detail.PPH23 * paymentVoucher.RateToIDR);
                    // Credit HutangPPh23
                    GeneralLedgerJournal creditbiayaPPH23 = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangPPH23).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round(PPH23, 2)
                    };
                    creditbiayaPPH23 = CreateObject(creditbiayaPPH23, _accountService);
                    journals.Add(creditbiayaPPH23);

                    if (paymentVoucher.IsGBCH)
                    {
                        //Debit GBCH for HutangPPh23
                        GeneralLedgerJournal debitGBCHPayablePPH23a = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                            SourceDocumentId = paymentVoucher.Id,
                            TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                            Status = Constant.GeneralLedgerStatus.Debit,
                            Amount = Math.Round(PPH23, 2)
                        };
                        debitGBCHPayablePPH23a = CreateObject(debitGBCHPayablePPH23a, _accountService);
                        journals.Add(debitGBCHPayablePPH23a);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency debitGBCHPayablePPH23b = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = debitGBCHPayablePPH23a.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH23,
                            };
                            debitGBCHPayablePPH23b = _gLNonBaseCurrencyService.CreateObject(debitGBCHPayablePPH23b, _accountService);
                        }
                    }
                    else
                    {
                        //Debit CashBank for HutangPPh23
                        GeneralLedgerJournal debitcashbankPPH23a = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                            SourceDocumentId = paymentVoucher.Id,
                            TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                            Status = Constant.GeneralLedgerStatus.Debit,
                            Amount = Math.Round(PPH23, 2)
                        };
                        debitcashbankPPH23a = CreateObject(debitcashbankPPH23a, _accountService);
                        journals.Add(debitcashbankPPH23a);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency debitcashbankPPH23b = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = debitcashbankPPH23a.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH23,
                            };
                            debitcashbankPPH23b = _gLNonBaseCurrencyService.CreateObject(debitcashbankPPH23b, _accountService);
                        }
                    }
                }

                // Debit GBCH/CashBank, Credit HutangPPh21
                if (detail.PPH21 > 0)
                {
                    decimal PPH21 = (detail.PPH21 * paymentVoucher.RateToIDR);
                    // Credit HutangPPh21
                    GeneralLedgerJournal creditbiayaPPH21 = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangPPH21).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round(PPH21, 2)
                    };
                    creditbiayaPPH21 = CreateObject(creditbiayaPPH21, _accountService);
                    journals.Add(creditbiayaPPH21);

                    if (paymentVoucher.IsGBCH)
                    {
                        //Debit GBCH for HutangPPh21
                        GeneralLedgerJournal debitGBCHPayablePPH21a = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                            SourceDocumentId = paymentVoucher.Id,
                            TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                            Status = Constant.GeneralLedgerStatus.Debit,
                            Amount = Math.Round(PPH21, 2)
                        };
                        debitGBCHPayablePPH21a = CreateObject(debitGBCHPayablePPH21a, _accountService);
                        journals.Add(debitGBCHPayablePPH21a);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency debitGBCHPayablePPH21b = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = debitGBCHPayablePPH21a.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH21,
                            };
                            debitGBCHPayablePPH21b = _gLNonBaseCurrencyService.CreateObject(debitGBCHPayablePPH21b, _accountService);
                        }
                    }
                    else
                    {
                        //Debit CashBank for HutangPPh21
                        GeneralLedgerJournal debitcashbankPPH21a = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                            SourceDocumentId = paymentVoucher.Id,
                            TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                            Status = Constant.GeneralLedgerStatus.Debit,
                            Amount = Math.Round(PPH21, 2)
                        };
                        debitcashbankPPH21a = CreateObject(debitcashbankPPH21a, _accountService);
                        journals.Add(debitcashbankPPH21a);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency debitcashbankPPH21b = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = debitcashbankPPH21a.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH21,
                            };
                            debitcashbankPPH21b = _gLNonBaseCurrencyService.CreateObject(debitcashbankPPH21b, _accountService);
                        }
                    }
                }
            }      

            #endregion

            #region GBCH: Credit GBCHPayable, Cash: Credit CashBank
            if (paymentVoucher.IsGBCH)
            {
                GeneralLedgerJournal creditGBCHPayable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(paymentVoucher.TotalAmount * paymentVoucher.RateToIDR, 2)
                };
                creditGBCHPayable = CreateObject(creditGBCHPayable, _accountService);
                journals.Add(creditGBCHPayable);

                if (cashBankCurrency.IsBase == false)
                {
                    GLNonBaseCurrency creditGBCHPayable2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditGBCHPayable.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = paymentVoucher.TotalAmount,
                    };
                    creditGBCHPayable2 = _gLNonBaseCurrencyService.CreateObject(creditGBCHPayable2, _accountService);
                }

                //Credit GBCH for BiayaBank
                if (paymentVoucher.BiayaBank > 0)
                {
                    GeneralLedgerJournal creditGBCHPayableFee = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round(paymentVoucher.BiayaBank * paymentVoucher.RateToIDR, 2)
                    };
                    creditGBCHPayableFee = CreateObject(creditGBCHPayableFee, _accountService);
                    journals.Add(creditGBCHPayableFee);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency creditGBCHPayableFee2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = creditGBCHPayableFee.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = paymentVoucher.BiayaBank,
                        };
                        creditGBCHPayableFee2 = _gLNonBaseCurrencyService.CreateObject(creditGBCHPayableFee2, _accountService);
                    }
                }
                //Credit/Debit GBCH for Pembulatan
                if (paymentVoucher.Pembulatan != 0)
                {
                    GeneralLedgerJournal creditGBCHPayableRnd = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = paymentVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? Constant.GeneralLedgerStatus.Debit : Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round(paymentVoucher.Pembulatan * paymentVoucher.RateToIDR, 2)
                    };
                    creditGBCHPayableRnd = CreateObject(creditGBCHPayableRnd, _accountService);
                    journals.Add(creditGBCHPayableRnd);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency creditGBCHPayableRnd2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = creditGBCHPayableRnd.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = paymentVoucher.Pembulatan,
                        };
                        creditGBCHPayableRnd2 = _gLNonBaseCurrencyService.CreateObject(creditGBCHPayableRnd2, _accountService);
                    }
                }
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
                    Amount = Math.Round(paymentVoucher.TotalAmount * paymentVoucher.RateToIDR, 2)
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);
                journals.Add(creditcashbank);
                if (cashBankCurrency.IsBase == false)
                {
                    GLNonBaseCurrency creditcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = paymentVoucher.TotalAmount,
                    };
                    creditcashbank2 = _gLNonBaseCurrencyService.CreateObject(creditcashbank2, _accountService);
                }

                //Credit CashBank for BiayaBank
                if (paymentVoucher.BiayaBank > 0)
                {
                    GeneralLedgerJournal creditcashbankFee = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round(paymentVoucher.BiayaBank * paymentVoucher.RateToIDR, 2)
                    };
                    creditcashbankFee = CreateObject(creditcashbankFee, _accountService);
                    journals.Add(creditcashbankFee);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency creditcashbankFee2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = creditcashbankFee.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = paymentVoucher.BiayaBank,
                        };
                        creditcashbankFee2 = _gLNonBaseCurrencyService.CreateObject(creditcashbankFee2, _accountService);
                    }
                }
                //Credit/Debit CashBank for Pembulatan
                if (paymentVoucher.Pembulatan != 0)
                {
                    GeneralLedgerJournal creditcashbankRnd = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = paymentVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? Constant.GeneralLedgerStatus.Debit : Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round(paymentVoucher.Pembulatan * paymentVoucher.RateToIDR, 2)
                    };
                    creditcashbankRnd = CreateObject(creditcashbankRnd, _accountService);
                    journals.Add(creditcashbankRnd);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency creditcashbankRnd2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = creditcashbankRnd.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = paymentVoucher.Pembulatan,
                        };
                        creditcashbankRnd2 = _gLNonBaseCurrencyService.CreateObject(creditcashbankRnd2, _accountService);
                    }
                }
            }

            // Debit BiayaBank
            if (paymentVoucher.BiayaBank > 0)
            {
                GeneralLedgerJournal debitBiayaBank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.BiayaAdminBank).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(paymentVoucher.BiayaBank * paymentVoucher.RateToIDR, 2)
                };
                debitBiayaBank = CreateObject(debitBiayaBank, _accountService);
                journals.Add(debitBiayaBank);
            }
            // Debit/Credit Pembulatan
            if (paymentVoucher.Pembulatan != 0)
            {
                GeneralLedgerJournal debitPembulatan = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.BiayaPembulatan).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = paymentVoucher.StatusPembulatan,
                    Amount = Math.Round(paymentVoucher.Pembulatan * paymentVoucher.RateToIDR, 2)
                };
                debitPembulatan = CreateObject(debitPembulatan, _accountService);
                journals.Add(debitPembulatan);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService,
                                           IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                           IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // GBCH: Debit GBCHPayable, Cash: Debit CashBank
            // Credit Account Payable, Debit ExchangeGain or Credit ExchangeLost

            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;
            Currency cashBankCurrency = _currencyService.GetObjectById(cashBank.CurrencyId);

            #region Credit Account Payable, Debit ExchangeGain or Credit ExchangeLost

            decimal TotalAP = 0;
            decimal TotalGainLoss = 0;
            decimal TotalPPH23 = 0;
            decimal TotalPPH21 = 0;
            IList<PaymentVoucherDetail> pvd = _paymentVoucherDetailService.GetQueryable().Where(x => x.PaymentVoucherId == paymentVoucher.Id && !x.IsDeleted).ToList();
            foreach (var detail in pvd)
            {
                TotalPPH23 += detail.PPH23;
                TotalPPH21 += detail.PPH21;
                Payable payable = _payableService.GetObjectById(detail.PayableId);
                Currency payableCurrency = _currencyService.GetObjectById(payable.CurrencyId);
                GeneralLedgerJournal creditaccountpayable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + payable.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(detail.Amount * payable.Rate, 2)
                };
                TotalAP += creditaccountpayable.Amount;
                creditaccountpayable = CreateObject(creditaccountpayable, _accountService);
                journals.Add(creditaccountpayable);

                if (payableCurrency.IsBase == false)
                {
                    GLNonBaseCurrency creditaccountpayable2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditaccountpayable.Id,
                        CurrencyId = payable.CurrencyId,
                        Amount = detail.Amount,
                    };
                    creditaccountpayable2 = _gLNonBaseCurrencyService.CreateObject(creditaccountpayable2, _accountService);
                }

                if (payable.Rate < paymentVoucher.RateToIDR)
                {
                    GeneralLedgerJournal creditExchangeLoss = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round((paymentVoucher.RateToIDR * detail.Rate * detail.Amount) - (payable.Rate * detail.Amount), 2)
                    };
                    TotalGainLoss -= creditExchangeLoss.Amount;
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
                        Amount = Math.Round((payable.Rate * detail.Amount) - (paymentVoucher.RateToIDR * detail.Rate * detail.Amount), 2)
                    };
                    TotalGainLoss += debitExchangeGain.Amount;
                    debitExchangeGain = CreateObject(debitExchangeGain, _accountService);
                    journals.Add(debitExchangeGain);
                }

                // Credit GBCH/CashBank, Debit HutangPPh23
                if (detail.PPH23 > 0)
                {
                    decimal PPH23 = (detail.PPH23 * paymentVoucher.RateToIDR);
                    // Debit HutangPPh23
                    GeneralLedgerJournal debitbiayaPPH23 = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangPPH23).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round(PPH23, 2)
                    };
                    debitbiayaPPH23 = CreateObject(debitbiayaPPH23, _accountService);
                    journals.Add(debitbiayaPPH23);

                    if (paymentVoucher.IsGBCH)
                    {
                        //Credit GBCH for HutangPPh23
                        GeneralLedgerJournal creditGBCHPayablePPH23a = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                            SourceDocumentId = paymentVoucher.Id,
                            TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                            Status = Constant.GeneralLedgerStatus.Credit,
                            Amount = Math.Round(PPH23, 2)
                        };
                        creditGBCHPayablePPH23a = CreateObject(creditGBCHPayablePPH23a, _accountService);
                        journals.Add(creditGBCHPayablePPH23a);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency creditGBCHPayablePPH23b = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = creditGBCHPayablePPH23a.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH23,
                            };
                            creditGBCHPayablePPH23b = _gLNonBaseCurrencyService.CreateObject(creditGBCHPayablePPH23b, _accountService);
                        }
                    }
                    else
                    {
                        //Credit CashBank for HutangPPh23
                        GeneralLedgerJournal creditcashbankPPH23a = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                            SourceDocumentId = paymentVoucher.Id,
                            TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                            Status = Constant.GeneralLedgerStatus.Credit,
                            Amount = Math.Round(PPH23, 2)
                        };
                        creditcashbankPPH23a = CreateObject(creditcashbankPPH23a, _accountService);
                        journals.Add(creditcashbankPPH23a);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency creditcashbankPPH23b = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = creditcashbankPPH23a.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH23,
                            };
                            creditcashbankPPH23b = _gLNonBaseCurrencyService.CreateObject(creditcashbankPPH23b, _accountService);
                        }
                    }
                }

                // Credit GBCH/CashBank, Debit HutangPPh21
                if (detail.PPH21 > 0)
                {
                    decimal PPH21 = (detail.PPH21 * paymentVoucher.RateToIDR);
                    // Debit HutangPPh21
                    GeneralLedgerJournal debitbiayaPPH21 = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangPPH21).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round(PPH21, 2)
                    };
                    debitbiayaPPH21 = CreateObject(debitbiayaPPH21, _accountService);
                    journals.Add(debitbiayaPPH21);

                    if (paymentVoucher.IsGBCH)
                    {
                        //Credit GBCH for HutangPPh21
                        GeneralLedgerJournal creditGBCHPayablePPH21a = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                            SourceDocumentId = paymentVoucher.Id,
                            TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                            Status = Constant.GeneralLedgerStatus.Credit,
                            Amount = Math.Round(PPH21, 2)
                        };
                        creditGBCHPayablePPH21a = CreateObject(creditGBCHPayablePPH21a, _accountService);
                        journals.Add(creditGBCHPayablePPH21a);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency creditGBCHPayablePPH21b = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = creditGBCHPayablePPH21a.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH21,
                            };
                            creditGBCHPayablePPH21b = _gLNonBaseCurrencyService.CreateObject(creditGBCHPayablePPH21b, _accountService);
                        }
                    }
                    else
                    {
                        //Credit CashBank for HutangPPh21
                        GeneralLedgerJournal creditcashbankPPH21a = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                            SourceDocumentId = paymentVoucher.Id,
                            TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                            Status = Constant.GeneralLedgerStatus.Credit,
                            Amount = Math.Round(PPH21, 2)
                        };
                        creditcashbankPPH21a = CreateObject(creditcashbankPPH21a, _accountService);
                        journals.Add(creditcashbankPPH21a);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency creditcashbankPPH21b = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = creditcashbankPPH21a.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH21,
                            };
                            creditcashbankPPH21b = _gLNonBaseCurrencyService.CreateObject(creditcashbankPPH21b, _accountService);
                        }
                    }
                }
            }
            #endregion

            #region GBCH: Debit GBCHPayable, Cash: Debit CashBank
            if (paymentVoucher.IsGBCH)
            {
                GeneralLedgerJournal debitGBCHPayable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(paymentVoucher.TotalAmount * paymentVoucher.RateToIDR, 2)
                };
                debitGBCHPayable = CreateObject(debitGBCHPayable, _accountService);
                journals.Add(debitGBCHPayable);
                if (cashBankCurrency.IsBase == false)
                {
                    GLNonBaseCurrency debitGBCHPayable2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitGBCHPayable.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = paymentVoucher.TotalAmount,
                    };
                    debitGBCHPayable2 = _gLNonBaseCurrencyService.CreateObject(debitGBCHPayable2, _accountService);
                }

                //Debit GBCH for BiayaBank
                if (paymentVoucher.BiayaBank > 0)
                {
                    GeneralLedgerJournal debitGBCHPayableFee = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round(paymentVoucher.BiayaBank * paymentVoucher.RateToIDR, 2)
                    };
                    debitGBCHPayableFee = CreateObject(debitGBCHPayableFee, _accountService);
                    journals.Add(debitGBCHPayableFee);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency debitGBCHPayableFee2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = debitGBCHPayableFee.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = paymentVoucher.BiayaBank,
                        };
                        debitGBCHPayableFee2 = _gLNonBaseCurrencyService.CreateObject(debitGBCHPayableFee2, _accountService);
                    }
                }
                //Debit/Credit GBCH for Pembulatan
                if (paymentVoucher.Pembulatan != 0)
                {
                    GeneralLedgerJournal debitGBCHPayableRnd = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = paymentVoucher.StatusPembulatan,
                        Amount = Math.Round(paymentVoucher.Pembulatan * paymentVoucher.RateToIDR, 2)
                    };
                    debitGBCHPayableRnd = CreateObject(debitGBCHPayableRnd, _accountService);
                    journals.Add(debitGBCHPayableRnd);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency debitGBCHPayableRnd2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = debitGBCHPayableRnd.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = paymentVoucher.Pembulatan,
                        };
                        debitGBCHPayableRnd2 = _gLNonBaseCurrencyService.CreateObject(debitGBCHPayableRnd2, _accountService);
                    }
                }
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
                    Amount = Math.Round(paymentVoucher.TotalAmount * paymentVoucher.RateToIDR, 2)
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);
                journals.Add(debitcashbank);
                if (cashBankCurrency.IsBase == false)
                {
                    GLNonBaseCurrency debitcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = paymentVoucher.TotalAmount,
                    };
                    debitcashbank2 = _gLNonBaseCurrencyService.CreateObject(debitcashbank2, _accountService);
                }

                //Debit CashBank for BiayaBank
                if (paymentVoucher.BiayaBank > 0)
                {
                    GeneralLedgerJournal debitcashbankFee = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round(paymentVoucher.BiayaBank * paymentVoucher.RateToIDR, 2)
                    };
                    debitcashbankFee = CreateObject(debitcashbankFee, _accountService);
                    journals.Add(debitcashbankFee);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency debitcashbankFee2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = debitcashbankFee.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = paymentVoucher.BiayaBank,
                        };
                        debitcashbankFee2 = _gLNonBaseCurrencyService.CreateObject(debitcashbankFee2, _accountService);
                    }
                }
                //Debit/Credit CashBank for Pembulatan
                if (paymentVoucher.Pembulatan != 0)
                {
                    GeneralLedgerJournal debitcashbankRnd = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                        SourceDocumentId = paymentVoucher.Id,
                        TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                        Status = paymentVoucher.StatusPembulatan,
                        Amount = Math.Round(paymentVoucher.Pembulatan * paymentVoucher.RateToIDR, 2)
                    };
                    debitcashbankRnd = CreateObject(debitcashbankRnd, _accountService);
                    journals.Add(debitcashbankRnd);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency debitcashbankRnd2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = debitcashbankRnd.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = paymentVoucher.Pembulatan,
                        };
                        debitcashbankRnd2 = _gLNonBaseCurrencyService.CreateObject(debitcashbankRnd2, _accountService);
                    }
                }
            }

            // Credit BiayaBank
            if (paymentVoucher.BiayaBank > 0)
            {
                GeneralLedgerJournal creditBiayaBank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.BiayaAdminBank).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(paymentVoucher.BiayaBank * paymentVoucher.RateToIDR, 2)
                };
                creditBiayaBank = CreateObject(creditBiayaBank, _accountService);
                journals.Add(creditBiayaBank);
            }
            // Credit/Debit Pembulatan
            if (paymentVoucher.Pembulatan != 0)
            {
                GeneralLedgerJournal creditPembulatan = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.BiayaPembulatan).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                    SourceDocumentId = paymentVoucher.Id,
                    TransactionDate = (DateTime)paymentVoucher.PaymentDate,
                    Status = paymentVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? Constant.GeneralLedgerStatus.Debit : Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(paymentVoucher.Pembulatan * paymentVoucher.RateToIDR, 2)
                };
                creditPembulatan = CreateObject(creditPembulatan, _accountService);
                journals.Add(creditPembulatan);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateReconcileJournalForPaymentVoucher(PaymentVoucher paymentVoucher, 
            CashBank cashBank, IAccountService _accountService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // Debit GBCH, Credit CashBank
            #region Debit GBCH, Credit CashBank

            decimal Total = paymentVoucher.TotalAmount - (paymentVoucher.TotalPPH21 + paymentVoucher.TotalPPH23 - paymentVoucher.BiayaBank + (paymentVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? paymentVoucher.Pembulatan : -paymentVoucher.Pembulatan));
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency cashBankCurrency = _currencyService.GetObjectById(cashBank.CurrencyId);
            GeneralLedgerJournal debitGBCH = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = paymentVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(Total * paymentVoucher.RateToIDR, 2)
            };
            debitGBCH = CreateObject(debitGBCH, _accountService);
            journals.Add(debitGBCH);

            if (cashBankCurrency.IsBase == false)
            {
                GLNonBaseCurrency debitGBCH2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitGBCH.Id,
                    CurrencyId = cashBank.CurrencyId,
                    Amount = Total,
                };
                debitGBCH2 = _gLNonBaseCurrencyService.CreateObject(debitGBCH2, _accountService);
            }
            GeneralLedgerJournal creditcashBank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = paymentVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(Total * paymentVoucher.RateToIDR, 2)
            };
            creditcashBank = CreateObject(creditcashBank, _accountService);
            journals.Add(creditcashBank);
            if (cashBankCurrency.IsBase == false)
            {
                GLNonBaseCurrency creditcashBank2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditcashBank.Id,
                    CurrencyId = cashBank.CurrencyId,
                    Amount = Total,
                };
                creditcashBank2 = _gLNonBaseCurrencyService.CreateObject(creditcashBank2, _accountService);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnReconcileJournalForPaymentVoucher(PaymentVoucher paymentVoucher, 
            CashBank cashBank, IAccountService _accountService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // Credit GBCH, Debit CashBank
            #region Credit GBCH, Debit CashBank

            decimal Total = paymentVoucher.TotalAmount - (paymentVoucher.TotalPPH21 + paymentVoucher.TotalPPH23 - paymentVoucher.BiayaBank + (paymentVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? paymentVoucher.Pembulatan : -paymentVoucher.Pembulatan));
            DateTime unReconcileDate = DateTime.Now;
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency cashBankCurrency = _currencyService.GetObjectById(cashBank.CurrencyId);
            GeneralLedgerJournal creditGBCH = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable + cashBank.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = paymentVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(Total * paymentVoucher.RateToIDR, 2)
            };
            creditGBCH = CreateObject(creditGBCH, _accountService);
            journals.Add(creditGBCH);

            if (cashBankCurrency.IsBase == false)
            {
                GLNonBaseCurrency creditGBCH2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditGBCH.Id,
                    CurrencyId = cashBank.CurrencyId,
                    Amount = Total,
                };
                creditGBCH2 = _gLNonBaseCurrencyService.CreateObject(creditGBCH2, _accountService);
            }

            GeneralLedgerJournal debitcashBank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + paymentVoucher.CashBankId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = paymentVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(Total * paymentVoucher.RateToIDR, 2)
            };
            debitcashBank = CreateObject(debitcashBank, _accountService);
            journals.Add(debitcashBank);

            if (cashBankCurrency.IsBase == false)
            {
                GLNonBaseCurrency debitcashBank2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitcashBank.Id,
                    CurrencyId = cashBank.CurrencyId,
                    Amount = Total,
                };
                debitcashBank2 = _gLNonBaseCurrencyService.CreateObject(debitcashBank2, _accountService);
            }

            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentRequest(PaymentRequest paymentRequest, 
                                           IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService,
                                           IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // Credit AccountPayable, Debit User Input
            #region Credit AccountPayable, Debit User Input

            IList<PaymentRequestDetail> details = _paymentRequestDetailService.GetObjectsByPaymentRequestId(paymentRequest.Id);
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency paymentRequestCurrency = _currencyService.GetObjectById(paymentRequest.CurrencyId);
            GeneralLedgerJournal creditAccountPayable = new GeneralLedgerJournal()
            { 
                AccountId = paymentRequest.AccountPayableId,
                SourceDocument = Constant.GeneralLedgerSource.PaymentRequest,
                SourceDocumentId = paymentRequest.Id,
                TransactionDate = (DateTime)paymentRequest.RequestedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(paymentRequest.Amount * paymentRequest.ExchangeRateAmount, 2)
            };
            creditAccountPayable = CreateObject(creditAccountPayable, _accountService);
            journals.Add(creditAccountPayable);

            if (paymentRequestCurrency.IsBase == false)
            {
                GLNonBaseCurrency creditAccountPayable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditAccountPayable.Id,
                    CurrencyId = paymentRequest.CurrencyId,
                    Amount = paymentRequest.Amount,
                };
                creditAccountPayable2 = _gLNonBaseCurrencyService.CreateObject(creditAccountPayable2, _accountService);
            }

            foreach (var paymentRequestDetail in details)
            {
                GeneralLedgerJournal journal = new GeneralLedgerJournal()
                {
                    AccountId = paymentRequestDetail.AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentRequest,
                    SourceDocumentId = paymentRequest.Id,
                    TransactionDate = (DateTime)paymentRequest.RequestedDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(paymentRequestDetail.Amount * paymentRequest.ExchangeRateAmount, 2)
                };
                journal = CreateObject(journal, _accountService);
                journals.Add(journal);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService,
                                           IAccountService _accountService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // Debit AccountPayable, Credit User Input
            #region Debit AccountPayable, Credit User Input
            IList<PaymentRequestDetail> details = _paymentRequestDetailService.GetObjectsByPaymentRequestId(paymentRequest.Id);
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency paymentRequestCurrency = _currencyService.GetObjectById(paymentRequest.CurrencyId);
            
            GeneralLedgerJournal debitAccountPayable = new GeneralLedgerJournal()
            {
                AccountId = paymentRequest.AccountPayableId,
                SourceDocument = Constant.GeneralLedgerSource.PaymentRequest,
                SourceDocumentId = paymentRequest.Id,
                TransactionDate = paymentRequest.RequestedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(paymentRequest.Amount * paymentRequest.ExchangeRateAmount, 2)
            };
            debitAccountPayable = CreateObject(debitAccountPayable, _accountService);
            journals.Add(debitAccountPayable);

            if (paymentRequestCurrency.IsBase == false)
            {
                GLNonBaseCurrency debitAccountPayable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitAccountPayable.Id,
                    CurrencyId = paymentRequest.CurrencyId,
                    Amount = paymentRequest.Amount,
                };
                debitAccountPayable2 = _gLNonBaseCurrencyService.CreateObject(debitAccountPayable2, _accountService);
            }

            foreach (var paymentRequestDetail in details)
            {
                GeneralLedgerJournal journal = new GeneralLedgerJournal()
                {
                    AccountId = paymentRequestDetail.AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.PaymentRequest,
                    SourceDocumentId = paymentRequest.Id,
                    TransactionDate = paymentRequest.RequestedDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(paymentRequestDetail.Amount * paymentRequest.ExchangeRateAmount, 2)
                };
                journal = CreateObject(journal, _accountService);
                journals.Add(journal);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseDownPayment(PurchaseDownPayment purchaseDownPayment, IAccountService _accountService,
                                           ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService)
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
                Amount = Math.Round(purchaseDownPayment.TotalAmount * purchaseDownPayment.ExchangeRateAmount, 2)
            };
            debitpiutanglainlain = CreateObject(debitpiutanglainlain, _accountService);

            GeneralLedgerJournal creditpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + purchaseDownPayment.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPayment,
                SourceDocumentId = purchaseDownPayment.Id,
                TransactionDate = (DateTime)purchaseDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(purchaseDownPayment.TotalAmount * purchaseDownPayment.ExchangeRateAmount, 2)
            };
            creditpayable = CreateObject(creditpayable, _accountService);

            Currency currency = _currencyService.GetObjectById(purchaseDownPayment.CurrencyId);
            if (currency.IsBase == false)
            {
                GLNonBaseCurrency creditpayable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditpayable.Id,
                    CurrencyId = purchaseDownPayment.CurrencyId,
                    Amount = purchaseDownPayment.TotalAmount,
                };
                creditpayable2 = _glNonBaseCurrencyService.CreateObject(creditpayable2, _accountService);
            }

            journals.Add(debitpiutanglainlain);
            journals.Add(creditpayable);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseDownPayment(PurchaseDownPayment purchaseDownPayment, IAccountService _accountService,
                                           ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService)
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
                Amount = Math.Round(purchaseDownPayment.TotalAmount * purchaseDownPayment.ExchangeRateAmount, 2)
            };
            creditpiutanglainlain = CreateObject(creditpiutanglainlain, _accountService);

            GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + purchaseDownPayment.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPayment,
                SourceDocumentId = purchaseDownPayment.Id,
                TransactionDate = purchaseDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(purchaseDownPayment.TotalAmount * purchaseDownPayment.ExchangeRateAmount, 2)
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);

            Currency currency = _currencyService.GetObjectById(purchaseDownPayment.CurrencyId);
            if (currency.IsBase == false)
            {
                GLNonBaseCurrency debitaccountpayable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitaccountpayable.Id,
                    CurrencyId = purchaseDownPayment.CurrencyId,
                    Amount = purchaseDownPayment.TotalAmount,
                };
                debitaccountpayable2 = _glNonBaseCurrencyService.CreateObject(debitaccountpayable2, _accountService);
            }

            journals.Add(creditpiutanglainlain);
            journals.Add(debitaccountpayable);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseDownPaymentAllocation(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IAccountService _accountService,
                                           IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                           IPayableService _payableService, IReceivableService _receivableService)
        {
            // Debit AccountPayable, Credit PiutangLainLain
            #region Credit PiutangLainLain, Debit AccountPayable
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Receivable receivable = _receivableService.GetObjectById(purchaseDownPaymentAllocation.ReceivableId);

            GeneralLedgerJournal creditpiutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PiutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                SourceDocumentId = purchaseDownPaymentAllocation.Id,
                TransactionDate = (DateTime)purchaseDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(purchaseDownPaymentAllocation.TotalAmount * receivable.Rate, 2)
            };
            creditpiutanglainlain = CreateObject(creditpiutanglainlain, _accountService);

            GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + receivable.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPayment,
                SourceDocumentId = purchaseDownPaymentAllocation.Id,
                TransactionDate = (DateTime)purchaseDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(purchaseDownPaymentAllocation.TotalAmount * receivable.Rate, 2)
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);

            if (receivable.Rate < purchaseDownPaymentAllocation.RateToIDR)
            {
                GeneralLedgerJournal creditExchangeLoss = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                    SourceDocumentId = purchaseDownPaymentAllocation.Id,
                    TransactionDate = purchaseDownPaymentAllocation.AllocationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round((purchaseDownPaymentAllocation.RateToIDR * purchaseDownPaymentAllocation.TotalAmount) - (receivable.Rate * purchaseDownPaymentAllocation.TotalAmount), 2)
                };
                creditExchangeLoss = CreateObject(creditExchangeLoss, _accountService);
                journals.Add(creditExchangeLoss);
            }
            else if (receivable.Rate > purchaseDownPaymentAllocation.RateToIDR)
            {
                GeneralLedgerJournal debitExchangeGain = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                    SourceDocumentId = purchaseDownPaymentAllocation.Id,
                    TransactionDate = purchaseDownPaymentAllocation.AllocationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round((receivable.Rate * purchaseDownPaymentAllocation.TotalAmount) - (purchaseDownPaymentAllocation.RateToIDR * purchaseDownPaymentAllocation.TotalAmount), 2)
                };
                debitExchangeGain = CreateObject(debitExchangeGain, _accountService);
                journals.Add(debitExchangeGain);
            }

            journals.Add(creditpiutanglainlain);
            journals.Add(debitaccountpayable);

            IList<PurchaseDownPaymentAllocationDetail> purchaseDownPaymentAllocationDetail = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);

            foreach (var item in purchaseDownPaymentAllocationDetail)
            {
                Payable payable = _payableService.GetObjectById(item.PayableId);
                if (item.Rate * purchaseDownPaymentAllocation.RateToIDR > payable.Rate)
                {
                    GeneralLedgerJournal debitExchangeGain = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                        SourceDocumentId = purchaseDownPaymentAllocation.Id,
                        TransactionDate = purchaseDownPaymentAllocation.AllocationDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round((item.Amount * item.Rate * purchaseDownPaymentAllocation.RateToIDR) - (item.Amount * payable.Rate), 2)
                    };
                    debitExchangeGain = CreateObject(debitExchangeGain, _accountService);
                    journals.Add(debitExchangeGain);
                }
                else if (item.Rate * purchaseDownPaymentAllocation.RateToIDR < payable.Rate)
                {
                    GeneralLedgerJournal creditExchangeLoss = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                        SourceDocumentId = purchaseDownPaymentAllocation.Id,
                        TransactionDate = purchaseDownPaymentAllocation.AllocationDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round((item.Amount * payable.Rate) - (item.Amount * item.Rate * purchaseDownPaymentAllocation.RateToIDR), 2)
                    };
                    creditExchangeLoss = CreateObject(creditExchangeLoss, _accountService);
                    journals.Add(creditExchangeLoss);
                }
            }
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseDownPaymentAllocation(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IAccountService _accountService,
                                           IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                           IPayableService _payableService, IReceivableService _receivableService)
        {
            // Credit AccountPayable, Debit PiutangLainLain
            #region Debit PiutangLainLain, Credit AccountPayable
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Receivable receivable = _receivableService.GetObjectById(purchaseDownPaymentAllocation.ReceivableId);

            GeneralLedgerJournal debitpiutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PiutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                SourceDocumentId = purchaseDownPaymentAllocation.Id,
                TransactionDate = (DateTime)purchaseDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(purchaseDownPaymentAllocation.TotalAmount * receivable.Rate, 2)
            };
            debitpiutanglainlain = CreateObject(debitpiutanglainlain, _accountService);

            GeneralLedgerJournal creditaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + receivable.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPayment,
                SourceDocumentId = purchaseDownPaymentAllocation.Id,
                TransactionDate = (DateTime)purchaseDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(purchaseDownPaymentAllocation.TotalAmount * receivable.Rate, 2)
            };
            creditaccountpayable = CreateObject(creditaccountpayable, _accountService);

            if (receivable.Rate < purchaseDownPaymentAllocation.RateToIDR)
            {
                GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                    SourceDocumentId = purchaseDownPaymentAllocation.Id,
                    TransactionDate = purchaseDownPaymentAllocation.AllocationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round((purchaseDownPaymentAllocation.RateToIDR * purchaseDownPaymentAllocation.TotalAmount) - (receivable.Rate * purchaseDownPaymentAllocation.TotalAmount), 2)
                };
                debitExchangeLoss = CreateObject(debitExchangeLoss, _accountService);
                journals.Add(debitExchangeLoss);
            }
            else if (receivable.Rate > purchaseDownPaymentAllocation.RateToIDR)
            {
                GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                    SourceDocumentId = purchaseDownPaymentAllocation.Id,
                    TransactionDate = purchaseDownPaymentAllocation.AllocationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round((receivable.Rate * purchaseDownPaymentAllocation.TotalAmount) - (purchaseDownPaymentAllocation.RateToIDR * purchaseDownPaymentAllocation.TotalAmount), 2)
                };
                creditExchangeGain = CreateObject(creditExchangeGain, _accountService);
                journals.Add(creditExchangeGain);
            }

            journals.Add(debitpiutanglainlain);
            journals.Add(creditaccountpayable);

            IList<PurchaseDownPaymentAllocationDetail> purchaseDownPaymentAllocationDetail = _purchaseDownPaymentAllocationDetailService.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocation.Id);

            foreach (var item in purchaseDownPaymentAllocationDetail)
            {
                Payable payable = _payableService.GetObjectById(item.PayableId);
                if (item.Rate * purchaseDownPaymentAllocation.RateToIDR > payable.Rate)
                {
                    GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                        SourceDocumentId = purchaseDownPaymentAllocation.Id,
                        TransactionDate = purchaseDownPaymentAllocation.AllocationDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round((item.Amount * item.Rate * purchaseDownPaymentAllocation.RateToIDR) - (item.Amount * payable.Rate), 2)
                    };
                    creditExchangeGain = CreateObject(creditExchangeGain, _accountService);
                    journals.Add(creditExchangeGain);
                }
                else if (item.Rate * purchaseDownPaymentAllocation.RateToIDR < payable.Rate)
                {
                    GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                        SourceDocument = Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation,
                        SourceDocumentId = purchaseDownPaymentAllocation.Id,
                        TransactionDate = purchaseDownPaymentAllocation.AllocationDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round((item.Amount * payable.Rate) - (item.Amount * item.Rate * purchaseDownPaymentAllocation.RateToIDR), 2)
                    };
                    debitExchangeLoss = CreateObject(debitExchangeLoss, _accountService);
                    journals.Add(debitExchangeLoss);
                }
            }
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
                Amount = Math.Round(purchaseAllowance.TotalAmount, 2)
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);

            GeneralLedgerJournal creditpurchaseallowance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PurchaseAllowance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseAllowance,
                SourceDocumentId = purchaseAllowance.Id,
                TransactionDate = (DateTime)purchaseAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(purchaseAllowance.TotalAmount, 2)
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
                Amount = Math.Round(purchaseAllowance.TotalAmount, 2)
            };
            creditaccountpayable = CreateObject(creditaccountpayable, _accountService);

            GeneralLedgerJournal debitpurchaseallowance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PurchaseAllowance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseAllowance,
                SourceDocumentId = purchaseAllowance.Id,
                TransactionDate = purchaseAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(purchaseAllowance.TotalAmount, 2)
            };
            debitpurchaseallowance = CreateObject(debitpurchaseallowance, _accountService);

            journals.Add(creditaccountpayable);
            journals.Add(debitpurchaseallowance);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService, 
                                           IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                           IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // GBCH: Debit GBCHReceivable, CashBank: DebitCashBank
            // Credit AccountReceivable, Credit ExchangeGain or Debit ExchangeLost

            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency cashBankCurrency = _currencyService.GetObjectById(cashBank.CurrencyId);
            //debit cashbank credit AR
            #region Credit AccountReceivable, Credit ExchangeGain or Debit ExchangeLost

            decimal TotalAR = 0;
            decimal TotalGainLoss = 0;
            decimal TotalPPH23 = 0;
            IList<ReceiptVoucherDetail> rvd = _receiptVoucherDetailService.GetQueryable().Where(x => x.ReceiptVoucherId == receiptVoucher.Id && !x.IsDeleted).ToList();
            foreach (var detail in rvd)
            {
                TotalPPH23 += detail.PPH23;
                Receivable receivable = _receivableService.GetObjectById(detail.ReceivableId);
                Currency receivableCurrency = _currencyService.GetObjectById(receivable.CurrencyId);
                GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + receivable.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(detail.Amount * receivable.Rate, 2)
                };
                TotalAR += creditaccountreceivable.Amount;
                creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);
                journals.Add(creditaccountreceivable);

                if (receivableCurrency.IsBase == false)
                {
                    GLNonBaseCurrency creditaccountreceivable2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditaccountreceivable.Id,
                        CurrencyId = receivable.CurrencyId,
                        Amount = detail.Amount,
                    };
                    creditaccountreceivable2 = _gLNonBaseCurrencyService.CreateObject(creditaccountreceivable2, _accountService);
                }

                if (receivable.Rate < (receiptVoucher.RateToIDR * detail.Rate))
                {
                    GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round((receiptVoucher.RateToIDR * detail.Rate * detail.Amount) - (receivable.Rate * detail.Amount), 2)
                    };
                    TotalGainLoss += creditExchangeGain.Amount;
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
                        Amount = Math.Round((receivable.Rate * detail.Amount) - (receiptVoucher.RateToIDR * detail.Rate * detail.Amount), 2)
                    };
                    TotalGainLoss -= debitExchangeLoss.Amount;
                    debitExchangeLoss = CreateObject(debitExchangeLoss, _accountService);
                    journals.Add(debitExchangeLoss);
                }

                // Credit GBCH/CashBank, Debit BiayaPPh23
                if (detail.PPH23 > 0)
                {
                    decimal PPH23 = (detail.PPH23 * receiptVoucher.RateToIDR);
                    // Debit BiayaPPh23
                    GeneralLedgerJournal debitbiayaPPH23 = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PPH23).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round(PPH23, 2)
                    };
                    debitbiayaPPH23 = CreateObject(debitbiayaPPH23, _accountService);
                    journals.Add(debitbiayaPPH23);

                    if (receiptVoucher.IsGBCH)
                    {
                        //Credit/Deduce GBCH for BiayaPPh23
                        GeneralLedgerJournal creditGBCHReceivablePPH = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                            SourceDocumentId = receiptVoucher.Id,
                            TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                            Status = Constant.GeneralLedgerStatus.Credit,
                            Amount = Math.Round(PPH23, 2)
                        };
                        creditGBCHReceivablePPH = CreateObject(creditGBCHReceivablePPH, _accountService);
                        journals.Add(creditGBCHReceivablePPH);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency creditGBCHReceivablePPH2 = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = creditGBCHReceivablePPH.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH23,
                            };
                            creditGBCHReceivablePPH2 = _gLNonBaseCurrencyService.CreateObject(creditGBCHReceivablePPH2, _accountService);
                        }
                    }
                    else
                    {
                        //Credit/Deduce CashBank for BiayaPPh23
                        GeneralLedgerJournal creditcashbankPPH = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + receiptVoucher.CashBankId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                            SourceDocumentId = receiptVoucher.Id,
                            TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                            Status = Constant.GeneralLedgerStatus.Credit,
                            Amount = Math.Round(PPH23, 2)
                        };
                        creditcashbankPPH = CreateObject(creditcashbankPPH, _accountService);
                        journals.Add(creditcashbankPPH);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency creditcashbankPPH2 = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = creditcashbankPPH.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH23,
                            };
                            creditcashbankPPH2 = _gLNonBaseCurrencyService.CreateObject(creditcashbankPPH2, _accountService);
                        }
                    }
                }

            }
            #endregion

            #region GBCH: Debit GBCHReceivable, CashBank: Debit CashBank
            if (receiptVoucher.IsGBCH)
            {
                GeneralLedgerJournal debitGBCHReceivable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(receiptVoucher.TotalAmount * receiptVoucher.RateToIDR, 2)
                };
                debitGBCHReceivable = CreateObject(debitGBCHReceivable, _accountService);
                journals.Add(debitGBCHReceivable);

                if (cashBankCurrency.IsBase == false)
                {
                    GLNonBaseCurrency debitGBCHReceivable2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitGBCHReceivable.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = receiptVoucher.TotalAmount,
                    };
                    debitGBCHReceivable2 = _gLNonBaseCurrencyService.CreateObject(debitGBCHReceivable2, _accountService);
                }

                //Credit/Deduce GBCH for Admin Fee
                if (receiptVoucher.BiayaBank > 0)
                {
                    GeneralLedgerJournal creditGBCHReceivableFee = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round(receiptVoucher.BiayaBank * receiptVoucher.RateToIDR, 2)
                    };
                    creditGBCHReceivableFee = CreateObject(creditGBCHReceivableFee, _accountService);
                    journals.Add(creditGBCHReceivableFee);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency creditGBCHReceivableFee2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = creditGBCHReceivableFee.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = receiptVoucher.BiayaBank,
                        };
                        creditGBCHReceivableFee2 = _gLNonBaseCurrencyService.CreateObject(creditGBCHReceivableFee2, _accountService);
                    }
                }
                //Credit/Debit GBCH for Pembulatan
                if (receiptVoucher.Pembulatan != 0)
                {
                    GeneralLedgerJournal creditGBCHReceivableRnd = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = receiptVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? Constant.GeneralLedgerStatus.Debit : Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round(receiptVoucher.Pembulatan * receiptVoucher.RateToIDR, 2)
                    };
                    creditGBCHReceivableRnd = CreateObject(creditGBCHReceivableRnd, _accountService);
                    journals.Add(creditGBCHReceivableRnd);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency creditGBCHReceivableRnd2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = creditGBCHReceivableRnd.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = receiptVoucher.Pembulatan,
                        };
                        creditGBCHReceivableRnd2 = _gLNonBaseCurrencyService.CreateObject(creditGBCHReceivableRnd2, _accountService);
                    }
                }
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
                    Amount = Math.Round(receiptVoucher.TotalAmount * receiptVoucher.RateToIDR, 2)
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);
                journals.Add(debitcashbank);
                if (cashBankCurrency.IsBase == false)
                {
                    GLNonBaseCurrency debitcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = receiptVoucher.TotalAmount,
                    };
                    debitcashbank2 = _gLNonBaseCurrencyService.CreateObject(debitcashbank2, _accountService);
                }

                //Credit/Deduce CashBank for Admin Fee
                if (receiptVoucher.BiayaBank > 0)
                {
                    GeneralLedgerJournal creditcashbankFee = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + receiptVoucher.CashBankId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round(receiptVoucher.BiayaBank * receiptVoucher.RateToIDR, 2)
                    };
                    creditcashbankFee = CreateObject(creditcashbankFee, _accountService);
                    journals.Add(creditcashbankFee);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency creditcashbankFee2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = creditcashbankFee.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = receiptVoucher.BiayaBank,
                        };
                        creditcashbankFee2 = _gLNonBaseCurrencyService.CreateObject(creditcashbankFee2, _accountService);
                    }
                }
                //Credit/Debit CashBank for Pembulatan
                if (receiptVoucher.Pembulatan != 0)
                {
                    GeneralLedgerJournal creditcashbankRnd = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + receiptVoucher.CashBankId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = receiptVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? Constant.GeneralLedgerStatus.Debit : Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round(receiptVoucher.Pembulatan * receiptVoucher.RateToIDR, 2)
                    };
                    creditcashbankRnd = CreateObject(creditcashbankRnd, _accountService);
                    journals.Add(creditcashbankRnd);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency creditcashbankRnd2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = creditcashbankRnd.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = receiptVoucher.Pembulatan,
                        };
                        creditcashbankRnd2 = _gLNonBaseCurrencyService.CreateObject(creditcashbankRnd2, _accountService);
                    }
                }
            }

            // Debit BiayaBank
            if (receiptVoucher.BiayaBank > 0)
            {
                GeneralLedgerJournal debitbiayabank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.BiayaAdminBank).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(receiptVoucher.BiayaBank * receiptVoucher.RateToIDR, 2)
                };
                debitbiayabank = CreateObject(debitbiayabank, _accountService);
                journals.Add(debitbiayabank);
            }
            // Debit/Credit Pembulatan
            if (receiptVoucher.Pembulatan != 0)
            {
                GeneralLedgerJournal debitPembulatan = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.BiayaPembulatan).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                    Status = receiptVoucher.StatusPembulatan,
                    Amount = Math.Round(receiptVoucher.Pembulatan * receiptVoucher.RateToIDR, 2)
                };
                debitPembulatan = CreateObject(debitPembulatan, _accountService);
                journals.Add(debitPembulatan);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService, 
                                           IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                           IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // GBCH: Credit GBCHReceivable, CashBank: Credit CashBank
            // Debit AccountReceivable, Debit ExchangeGain or Credit ExchangeLost
            #region Debit AccountReceivable, Debit ExchangeGain or Credit ExchangeLost
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency cashBankCurrency = _currencyService.GetObjectById(cashBank.CurrencyId);
            DateTime UnconfirmationDate = DateTime.Now;

            decimal TotalAR = 0;
            decimal TotalGainLoss = 0;
            decimal TotalPPH23 = 0;
            IList<ReceiptVoucherDetail> rvd = _receiptVoucherDetailService.GetQueryable().Where(x => x.ReceiptVoucherId == receiptVoucher.Id && !x.IsDeleted).ToList();
            foreach (var detail in rvd)
            {
                TotalPPH23 += detail.PPH23;
                Receivable receivable = _receivableService.GetObjectById(detail.ReceivableId);
                Currency receivableCurrency = _currencyService.GetObjectById(receivable.CurrencyId);
                GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + receivable.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(detail.Amount * receivable.Rate, 2)
                };
                TotalAR += debitaccountreceivable.Amount;
                debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

                if (receivableCurrency.IsBase == false)
                {
                    GLNonBaseCurrency debitaccountreceivable2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = debitaccountreceivable.Id,
                        CurrencyId = receivable.CurrencyId,
                        Amount = detail.Amount,
                    };
                    debitaccountreceivable2 = _gLNonBaseCurrencyService.CreateObject(debitaccountreceivable2, _accountService);
                }

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
                        Amount = Math.Round((receiptVoucher.RateToIDR * detail.Rate * detail.Amount) - (receivable.Rate * detail.Amount), 2)
                    };
                    TotalGainLoss += debitExchangeGain.Amount;
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
                        Amount =  Math.Round((receivable.Rate * detail.Amount) - (receiptVoucher.RateToIDR * detail.Rate * detail.Amount), 2)
                    };
                    TotalGainLoss -= creditExchangeLoss.Amount;
                    creditExchangeLoss = CreateObject(creditExchangeLoss, _accountService);
                    journals.Add(creditExchangeLoss);
                }

                // Debit GBCH/CashBank, Credit BiayaPPh23
                if (detail.PPH23 > 0)
                {
                    decimal PPH23 = (detail.PPH23 * receiptVoucher.RateToIDR);
                    // Credit BiayaPPh23
                    GeneralLedgerJournal creditbiayaPPH23 = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PPH23).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round(PPH23, 2)
                    };
                    creditbiayaPPH23 = CreateObject(creditbiayaPPH23, _accountService);
                    journals.Add(creditbiayaPPH23);

                    if (receiptVoucher.IsGBCH)
                    {
                        //Debit GBCH for BiayaPPh23
                        GeneralLedgerJournal debitGBCHReceivablePPH = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                            SourceDocumentId = receiptVoucher.Id,
                            TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                            Status = Constant.GeneralLedgerStatus.Debit,
                            Amount = Math.Round(PPH23, 2)
                        };
                        debitGBCHReceivablePPH = CreateObject(debitGBCHReceivablePPH, _accountService);
                        journals.Add(debitGBCHReceivablePPH);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency debitGBCHReceivablePPH2 = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = debitGBCHReceivablePPH.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH23,
                            };
                            debitGBCHReceivablePPH2 = _gLNonBaseCurrencyService.CreateObject(debitGBCHReceivablePPH2, _accountService);
                        }
                    }
                    else
                    {
                        //Debit CashBank for BiayaPPh23
                        GeneralLedgerJournal debitcashbankPPH = new GeneralLedgerJournal()
                        {
                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + receiptVoucher.CashBankId).Id,
                            SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                            SourceDocumentId = receiptVoucher.Id,
                            TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                            Status = Constant.GeneralLedgerStatus.Debit,
                            Amount = Math.Round(PPH23, 2)
                        };
                        debitcashbankPPH = CreateObject(debitcashbankPPH, _accountService);
                        journals.Add(debitcashbankPPH);
                        if (cashBankCurrency.IsBase == false)
                        {
                            GLNonBaseCurrency debitcashbankPPH2 = new GLNonBaseCurrency()
                            {
                                GeneralLedgerJournalId = debitcashbankPPH.Id,
                                CurrencyId = cashBank.CurrencyId,
                                Amount = detail.PPH23,
                            };
                            debitcashbankPPH2 = _gLNonBaseCurrencyService.CreateObject(debitcashbankPPH2, _accountService);
                        }
                    }
                }
            }
            #endregion

            #region Credit GBCHReceivable, CashBank: Credit CashBank
            if (receiptVoucher.IsGBCH)
            {
                GeneralLedgerJournal creditGBCH = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(receiptVoucher.TotalAmount * receiptVoucher.RateToIDR, 2)
                };
                creditGBCH = CreateObject(creditGBCH, _accountService);
                journals.Add(creditGBCH);
                if (cashBankCurrency.IsBase == false)
                {
                    GLNonBaseCurrency creditGBCH2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditGBCH.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = receiptVoucher.TotalAmount,
                    };
                    creditGBCH2 = _gLNonBaseCurrencyService.CreateObject(creditGBCH2, _accountService);
                }

                //Debit GBCH for Admin Fee
                if (receiptVoucher.BiayaBank > 0)
                {
                    GeneralLedgerJournal debitGBCHReceivableFee = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round(receiptVoucher.BiayaBank * receiptVoucher.RateToIDR, 2)
                    };
                    debitGBCHReceivableFee = CreateObject(debitGBCHReceivableFee, _accountService);
                    journals.Add(debitGBCHReceivableFee);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency debitGBCHReceivableFee2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = debitGBCHReceivableFee.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = receiptVoucher.BiayaBank,
                        };
                        debitGBCHReceivableFee2 = _gLNonBaseCurrencyService.CreateObject(debitGBCHReceivableFee2, _accountService);
                    }
                }
                //Debit/Credit GBCH for Pembulatan
                if (receiptVoucher.Pembulatan != 0)
                {
                    GeneralLedgerJournal debitGBCHReceivableRnd = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = receiptVoucher.StatusPembulatan,
                        Amount = Math.Round(receiptVoucher.Pembulatan * receiptVoucher.RateToIDR, 2)
                    };
                    debitGBCHReceivableRnd = CreateObject(debitGBCHReceivableRnd, _accountService);
                    journals.Add(debitGBCHReceivableRnd);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency debitGBCHReceivableRnd2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = debitGBCHReceivableRnd.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = receiptVoucher.Pembulatan,
                        };
                        debitGBCHReceivableRnd2 = _gLNonBaseCurrencyService.CreateObject(debitGBCHReceivableRnd2, _accountService);
                    }
                }
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
                    Amount = Math.Round(receiptVoucher.TotalAmount * receiptVoucher.RateToIDR, 2)
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);
                journals.Add(creditcashbank);
                if (cashBankCurrency.IsBase == false)
                {
                    GLNonBaseCurrency creditcashbank2 = new GLNonBaseCurrency()
                    {
                        GeneralLedgerJournalId = creditcashbank.Id,
                        CurrencyId = cashBank.CurrencyId,
                        Amount = receiptVoucher.TotalAmount,
                    };
                    creditcashbank2 = _gLNonBaseCurrencyService.CreateObject(creditcashbank2, _accountService);
                }

                //Debit CashBank for Admin Fee
                if (receiptVoucher.BiayaBank > 0)
                {
                    GeneralLedgerJournal debitcashbankFee = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + receiptVoucher.CashBankId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round(receiptVoucher.BiayaBank * receiptVoucher.RateToIDR, 2)
                    };
                    debitcashbankFee = CreateObject(debitcashbankFee, _accountService);
                    journals.Add(debitcashbankFee);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency debitcashbankFee2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = debitcashbankFee.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = receiptVoucher.BiayaBank,
                        };
                        debitcashbankFee2 = _gLNonBaseCurrencyService.CreateObject(debitcashbankFee2, _accountService);
                    }
                }
                //Debit/Credit CashBank for Pembulatan
                if (receiptVoucher.Pembulatan != 0)
                {
                    GeneralLedgerJournal debitcashbankRnd = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + receiptVoucher.CashBankId).Id,
                        SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                        SourceDocumentId = receiptVoucher.Id,
                        TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                        Status = receiptVoucher.StatusPembulatan,
                        Amount = Math.Round(receiptVoucher.Pembulatan * receiptVoucher.RateToIDR, 2)
                    };
                    debitcashbankRnd = CreateObject(debitcashbankRnd, _accountService);
                    journals.Add(debitcashbankRnd);
                    if (cashBankCurrency.IsBase == false)
                    {
                        GLNonBaseCurrency debitcashbankRnd2 = new GLNonBaseCurrency()
                        {
                            GeneralLedgerJournalId = debitcashbankRnd.Id,
                            CurrencyId = cashBank.CurrencyId,
                            Amount = receiptVoucher.Pembulatan,
                        };
                        debitcashbankRnd2 = _gLNonBaseCurrencyService.CreateObject(debitcashbankRnd2, _accountService);
                    }
                }
            }

            // Credit BiayaBank
            if (receiptVoucher.BiayaBank > 0)
            {
                GeneralLedgerJournal creditbiayabank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.BiayaAdminBank).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(receiptVoucher.BiayaBank * receiptVoucher.RateToIDR, 2)
                };
                creditbiayabank = CreateObject(creditbiayabank, _accountService);
                journals.Add(creditbiayabank);
            }
            // Debit/Credit Pembulatan
            if (receiptVoucher.Pembulatan != 0)
            {
                GeneralLedgerJournal creditPembulatan = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.BiayaPembulatan).Id,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                    SourceDocumentId = receiptVoucher.Id,
                    TransactionDate = (DateTime)receiptVoucher.ReceiptDate,
                    Status = receiptVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? Constant.GeneralLedgerStatus.Debit : Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(receiptVoucher.Pembulatan * receiptVoucher.RateToIDR, 2)
                };
                creditPembulatan = CreateObject(creditPembulatan, _accountService);
                journals.Add(creditPembulatan);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateReconcileJournalForReceiptVoucher(ReceiptVoucher receiptVoucher,
                                           CashBank cashBank, IAccountService _accountService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // Credit GBCH, Debit CashBank
            #region Credit GBCH, Debit CashBank

            decimal Total = receiptVoucher.TotalAmount - (receiptVoucher.TotalPPH23 + receiptVoucher.BiayaBank + (receiptVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? -receiptVoucher.Pembulatan : receiptVoucher.Pembulatan));
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency cashBankCurrency = _currencyService.GetObjectById(cashBank.CurrencyId);
            GeneralLedgerJournal creditGBCH = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = receiptVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(Total * receiptVoucher.RateToIDR, 2)
            };
            creditGBCH = CreateObject(creditGBCH, _accountService);
            journals.Add(creditGBCH);

            if (cashBankCurrency.IsBase == false)
            {
                GLNonBaseCurrency creditGBCH2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditGBCH.Id,
                    CurrencyId = cashBank.CurrencyId,
                    Amount = Total,
                };
                creditGBCH2 = _gLNonBaseCurrencyService.CreateObject(creditGBCH2, _accountService);
            }

            GeneralLedgerJournal debitcashBank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + receiptVoucher.CashBankId).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = receiptVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(Total * receiptVoucher.RateToIDR, 2)
            };
            debitcashBank = CreateObject(debitcashBank, _accountService);
            journals.Add(debitcashBank);

            if (cashBankCurrency.IsBase == false)
            {
                GLNonBaseCurrency debitcashBank2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitcashBank.Id,
                    CurrencyId = cashBank.CurrencyId,
                    Amount = Total,
                };
                debitcashBank2 = _gLNonBaseCurrencyService.CreateObject(debitcashBank2, _accountService);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnReconcileJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank,
                                           IAccountService _accountService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // Debit GBCH, Credit CashBank
            #region Debit GBCH, Credit CashBank

            decimal Total = receiptVoucher.TotalAmount - (receiptVoucher.TotalPPH23 + receiptVoucher.BiayaBank + (receiptVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? -receiptVoucher.Pembulatan : receiptVoucher.Pembulatan));
            DateTime unReconcileDate = DateTime.Now;
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency cashBankCurrency = _currencyService.GetObjectById(cashBank.CurrencyId);
            GeneralLedgerJournal debitGBCH = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable + cashBank.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = receiptVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(Total * receiptVoucher.RateToIDR, 2)
            };
            debitGBCH = CreateObject(debitGBCH, _accountService);
            journals.Add(debitGBCH);

            if (cashBankCurrency.IsBase == false)
            {
                GLNonBaseCurrency debitGBCH2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitGBCH.Id,
                    CurrencyId = cashBank.CurrencyId,
                    Amount = Total,
                };
                debitGBCH2 = _gLNonBaseCurrencyService.CreateObject(debitGBCH2, _accountService);
            }
            GeneralLedgerJournal creditcashBank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + receiptVoucher.CashBankId).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = receiptVoucher.ReconciliationDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(Total * receiptVoucher.RateToIDR, 2)
            };
            creditcashBank = CreateObject(creditcashBank, _accountService);

            if (cashBankCurrency.IsBase == false)
            {
                GLNonBaseCurrency creditcashBank2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditcashBank.Id,
                    CurrencyId = cashBank.CurrencyId,
                    Amount = Total,
                };
                creditcashBank2 = _gLNonBaseCurrencyService.CreateObject(creditcashBank2, _accountService);
            }

            journals.Add(creditcashBank);
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForReceiptRequest(ReceiptRequest receiptRequest,
                                   IReceiptRequestDetailService _receiptRequestDetailService, IAccountService _accountService,
                                   IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // Debit AccountReceivable, Credit User Input
            #region Debit AccountReceivable, Credit User Input

            IList<ReceiptRequestDetail> details = _receiptRequestDetailService.GetObjectsByReceiptRequestId(receiptRequest.Id);
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency receiptRequestCurrency = _currencyService.GetObjectById(receiptRequest.CurrencyId);
            GeneralLedgerJournal debitAccountReceivable = new GeneralLedgerJournal()
            {
                AccountId = receiptRequest.AccountReceivableId,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptRequest,
                SourceDocumentId = receiptRequest.Id,
                TransactionDate = (DateTime)receiptRequest.RequestedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(receiptRequest.Amount * receiptRequest.ExchangeRateAmount, 2)
            };
            debitAccountReceivable = CreateObject(debitAccountReceivable, _accountService);
            journals.Add(debitAccountReceivable);

            if (receiptRequestCurrency.IsBase == false)
            {
                GLNonBaseCurrency debitAccountReceivable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitAccountReceivable.Id,
                    CurrencyId = receiptRequest.CurrencyId,
                    Amount = receiptRequest.Amount,
                };
                debitAccountReceivable2 = _gLNonBaseCurrencyService.CreateObject(debitAccountReceivable2, _accountService);
            }

            foreach (var receiptRequestDetail in details)
            {
                GeneralLedgerJournal journal = new GeneralLedgerJournal()
                {
                    AccountId = receiptRequestDetail.AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptRequest,
                    SourceDocumentId = receiptRequest.Id,
                    TransactionDate = (DateTime)receiptRequest.RequestedDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(receiptRequestDetail.Amount * receiptRequest.ExchangeRateAmount, 2)
                };
                journal = CreateObject(journal, _accountService);
                journals.Add(journal);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForReceiptRequest(ReceiptRequest receiptRequest, IReceiptRequestDetailService _receiptRequestDetailService,
                                           IAccountService _accountService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // Credit AccountReceivable, Debit User Input
            #region Credit AccountReceivable, Debit User Input
            IList<ReceiptRequestDetail> details = _receiptRequestDetailService.GetObjectsByReceiptRequestId(receiptRequest.Id);
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency receiptRequestCurrency = _currencyService.GetObjectById(receiptRequest.CurrencyId);

            GeneralLedgerJournal creditAccountReceivable = new GeneralLedgerJournal()
            {
                AccountId = receiptRequest.AccountReceivableId,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptRequest,
                SourceDocumentId = receiptRequest.Id,
                TransactionDate = receiptRequest.RequestedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(receiptRequest.Amount * receiptRequest.ExchangeRateAmount, 2)
            };
            creditAccountReceivable = CreateObject(creditAccountReceivable, _accountService);
            journals.Add(creditAccountReceivable);

            if (receiptRequestCurrency.IsBase == false)
            {
                GLNonBaseCurrency creditAccountReceivable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditAccountReceivable.Id,
                    CurrencyId = receiptRequest.CurrencyId,
                    Amount = receiptRequest.Amount,
                };
                creditAccountReceivable2 = _gLNonBaseCurrencyService.CreateObject(creditAccountReceivable2, _accountService);
            }

            foreach (var receiptRequestDetail in details)
            {
                GeneralLedgerJournal journal = new GeneralLedgerJournal()
                {
                    AccountId = receiptRequestDetail.AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.ReceiptRequest,
                    SourceDocumentId = receiptRequest.Id,
                    TransactionDate = receiptRequest.RequestedDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(receiptRequestDetail.Amount * receiptRequest.ExchangeRateAmount, 2)
                };
                journal = CreateObject(journal, _accountService);
                journals.Add(journal);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesDownPayment(SalesDownPayment salesDownPayment, IAccountService _accountService,
                                           ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService)
        {
            // Debit AccountReceivable, Credit HutangLainLain
            #region Debit AccountReceivable, Credit HutangLainLain
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + salesDownPayment.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPayment.Id,
                TransactionDate = (DateTime)salesDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(salesDownPayment.TotalAmount * salesDownPayment.ExchangeRateAmount, 2)
            };
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            Currency currency = _currencyService.GetObjectById(salesDownPayment.CurrencyId);
            if (currency.IsBase == false)
            {
                GLNonBaseCurrency debitaccountreceivable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitaccountreceivable.Id,
                    CurrencyId = salesDownPayment.CurrencyId,
                    Amount = salesDownPayment.TotalAmount,
                };
                debitaccountreceivable2 = _glNonBaseCurrencyService.CreateObject(debitaccountreceivable2, _accountService);
            }

            GeneralLedgerJournal credithutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPayment.Id,
                TransactionDate = (DateTime)salesDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(salesDownPayment.TotalAmount * salesDownPayment.ExchangeRateAmount, 2)
            };
            credithutanglainlain = CreateObject(credithutanglainlain, _accountService);

            journals.Add(debitaccountreceivable);
            journals.Add(credithutanglainlain);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesDownPayment(SalesDownPayment salesDownPayment, IAccountService _accountService,
                                           ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService)
        {
            // Credit AccountReceivable, Debit HutangLainLain
            #region Credit AccountReceivable, Debit HutangLainLain
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + salesDownPayment.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPayment.Id,
                TransactionDate = salesDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(salesDownPayment.TotalAmount * salesDownPayment.ExchangeRateAmount, 2)
            };
            creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);

            Currency currency = _currencyService.GetObjectById(salesDownPayment.CurrencyId);
            if (currency.IsBase == false)
            {
                GLNonBaseCurrency creditaccountreceivable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditaccountreceivable.Id,
                    CurrencyId = salesDownPayment.CurrencyId,
                    Amount = salesDownPayment.TotalAmount,
                };
                creditaccountreceivable2 = _glNonBaseCurrencyService.CreateObject(creditaccountreceivable2, _accountService);
            }

            GeneralLedgerJournal debithutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPayment.Id,
                TransactionDate = salesDownPayment.DownPaymentDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(salesDownPayment.TotalAmount * salesDownPayment.ExchangeRateAmount, 2)
            };
            debithutanglainlain = CreateObject(debithutanglainlain, _accountService);

            journals.Add(creditaccountreceivable);
            journals.Add(debithutanglainlain);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesDownPaymentAllocation(SalesDownPaymentAllocation salesDownPaymentAllocation, IAccountService _accountService,
                                           ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                           IPayableService _payableService, IReceivableService _receivableService)
        {
            // Debit HutangLainLain, Credit AccountReceivable
            #region Debit HutangLainLain, Credit AccountReceivable
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Payable payable = _payableService.GetObjectById(salesDownPaymentAllocation.PayableId);

            GeneralLedgerJournal debithutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                SourceDocumentId = salesDownPaymentAllocation.Id,
                TransactionDate = (DateTime)salesDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(salesDownPaymentAllocation.TotalAmount * payable.Rate, 2)
            };
            debithutanglainlain = CreateObject(debithutanglainlain, _accountService);

            GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + salesDownPaymentAllocation.Payable.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPaymentAllocation.Id,
                TransactionDate = (DateTime)salesDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(salesDownPaymentAllocation.TotalAmount * payable.Rate, 2)
            };
            creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);

            if (payable.Rate < salesDownPaymentAllocation.RateToIDR)
            {
                GeneralLedgerJournal creditExchangeLoss = new GeneralLedgerJournal()
                { 
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                    SourceDocumentId = salesDownPaymentAllocation.Id,
                    TransactionDate = salesDownPaymentAllocation.AllocationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round((salesDownPaymentAllocation.RateToIDR * salesDownPaymentAllocation.TotalAmount) - (payable.Rate * salesDownPaymentAllocation.TotalAmount), 2)
                };
                creditExchangeLoss = CreateObject(creditExchangeLoss, _accountService);
                journals.Add(creditExchangeLoss);
            }
            else if (payable.Rate > salesDownPaymentAllocation.RateToIDR)
            {
                GeneralLedgerJournal debitExchangeGain = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                    SourceDocumentId = salesDownPaymentAllocation.Id,
                    TransactionDate = salesDownPaymentAllocation.AllocationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round((payable.Rate * salesDownPaymentAllocation.TotalAmount) - (salesDownPaymentAllocation.RateToIDR * salesDownPaymentAllocation.TotalAmount), 2)
                };
                debitExchangeGain = CreateObject(debitExchangeGain, _accountService);
                journals.Add(debitExchangeGain);
            }

            journals.Add(debithutanglainlain);
            journals.Add(creditaccountreceivable);

            IList<SalesDownPaymentAllocationDetail> salesDownPaymentAllocationDetail = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);

            foreach (var item in salesDownPaymentAllocationDetail)
            {
                Receivable receivable = _receivableService.GetObjectById(item.ReceivableId);
                if (item.Rate * salesDownPaymentAllocation.RateToIDR > receivable.Rate)
                {
                    GeneralLedgerJournal debitExchangeGain = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                        SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                        SourceDocumentId = salesDownPaymentAllocation.Id,
                        TransactionDate = salesDownPaymentAllocation.AllocationDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round((item.Amount * item.Rate * salesDownPaymentAllocation.RateToIDR) - (item.Amount * receivable.Rate), 2)
                    };
                    debitExchangeGain = CreateObject(debitExchangeGain, _accountService);
                    journals.Add(debitExchangeGain);
                }
                else if (item.Rate * salesDownPaymentAllocation.RateToIDR < receivable.Rate)
                {
                    GeneralLedgerJournal creditExchangeLoss = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                        SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                        SourceDocumentId = salesDownPaymentAllocation.Id,
                        TransactionDate = salesDownPaymentAllocation.AllocationDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round((item.Amount * receivable.Rate) - (item.Amount * item.Rate * salesDownPaymentAllocation.RateToIDR), 2)
                    };
                    creditExchangeLoss = CreateObject(creditExchangeLoss, _accountService);
                    journals.Add(creditExchangeLoss);
                }
            }
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesDownPaymentAllocation(SalesDownPaymentAllocation salesDownPaymentAllocation, IAccountService _accountService,
                                           ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                           IPayableService _payableService, IReceivableService _receivableService)
        {
            // Credit HutangLainLain, Debit AccountReceivable
            #region Credit HutangLainLain, Debit AccountReceivable
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Payable payable = _payableService.GetObjectById(salesDownPaymentAllocation.PayableId);

            GeneralLedgerJournal credithutanglainlain = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.HutangLainLain).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                SourceDocumentId = salesDownPaymentAllocation.Id,
                TransactionDate = (DateTime)salesDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(salesDownPaymentAllocation.TotalAmount * payable.Rate, 2)
            };
            credithutanglainlain = CreateObject(credithutanglainlain, _accountService);

            GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + salesDownPaymentAllocation.Payable.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesDownPayment,
                SourceDocumentId = salesDownPaymentAllocation.Id,
                TransactionDate = (DateTime)salesDownPaymentAllocation.AllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(salesDownPaymentAllocation.TotalAmount * payable.Rate, 2)
            };
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            if (payable.Rate < salesDownPaymentAllocation.RateToIDR)
            {
                GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                    SourceDocumentId = salesDownPaymentAllocation.Id,
                    TransactionDate = salesDownPaymentAllocation.AllocationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round((salesDownPaymentAllocation.RateToIDR * salesDownPaymentAllocation.TotalAmount) - (payable.Rate * salesDownPaymentAllocation.TotalAmount), 2)
                };
                debitExchangeLoss = CreateObject(debitExchangeLoss, _accountService);
                journals.Add(debitExchangeLoss);
            }
            else if (payable.Rate > salesDownPaymentAllocation.RateToIDR)
            {
                GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                    SourceDocumentId = salesDownPaymentAllocation.Id,
                    TransactionDate = salesDownPaymentAllocation.AllocationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round((payable.Rate * salesDownPaymentAllocation.TotalAmount) - (salesDownPaymentAllocation.RateToIDR * salesDownPaymentAllocation.TotalAmount), 2)
                };
                creditExchangeGain = CreateObject(creditExchangeGain, _accountService);
                journals.Add(creditExchangeGain);
            }

            journals.Add(credithutanglainlain);
            journals.Add(debitaccountreceivable);

            IList<SalesDownPaymentAllocationDetail> salesDownPaymentAllocationDetail = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);

            foreach (var item in salesDownPaymentAllocationDetail)
            {
                Receivable receivable = _receivableService.GetObjectById(item.ReceivableId);
                if (item.Rate * salesDownPaymentAllocation.RateToIDR > receivable.Rate)
                {
                    GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                        SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                        SourceDocumentId = salesDownPaymentAllocation.Id,
                        TransactionDate = salesDownPaymentAllocation.AllocationDate,
                        Status = Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Round((item.Amount * item.Rate * salesDownPaymentAllocation.RateToIDR) - (item.Amount * receivable.Rate), 2)
                    };
                    creditExchangeGain = CreateObject(creditExchangeGain, _accountService);
                    journals.Add(creditExchangeGain);
                }
                else if (item.Rate * salesDownPaymentAllocation.RateToIDR < receivable.Rate)
                {
                    GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                    {
                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                        SourceDocument = Constant.GeneralLedgerSource.SalesDownPaymentAllocation,
                        SourceDocumentId = salesDownPaymentAllocation.Id,
                        TransactionDate = salesDownPaymentAllocation.AllocationDate,
                        Status = Constant.GeneralLedgerStatus.Debit,
                        Amount = Math.Round((item.Amount * receivable.Rate) - (item.Amount * item.Rate * salesDownPaymentAllocation.RateToIDR), 2)
                    };
                    debitExchangeLoss = CreateObject(debitExchangeLoss, _accountService);
                    journals.Add(debitExchangeLoss);
                }
            }
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
                Amount = Math.Round(salesAllowance.TotalAmount, 2)
            };
            debitsalesallowance = CreateObject(debitsalesallowance, _accountService);

            GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesAllowance,
                SourceDocumentId = salesAllowance.Id,
                TransactionDate = (DateTime)salesAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(salesAllowance.TotalAmount, 2)
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
                Amount = Math.Round(salesAllowance.TotalAmount, 2)
            };
            creditsalesallowance = CreateObject(creditsalesallowance, _accountService);

            GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesAllowance,
                SourceDocumentId = salesAllowance.Id,
                TransactionDate = salesAllowance.AllowanceAllocationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(salesAllowance.TotalAmount, 2)
            };
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            journals.Add(creditsalesallowance);
            journals.Add(debitaccountreceivable);

            return journals;
            #endregion
        }

        // MASTER & STOCK
        public GeneralLedgerJournal CreateConfirmationJournalForStockAdjustmentDetail(StockAdjustment stockAdjustment, int AccountId, decimal AvgCost, IAccountService _accountService)
        {
            #region Inventory
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                SourceDocumentId = stockAdjustment.Id,
                TransactionDate = (DateTime) stockAdjustment.AdjustmentDate,
                Status = AvgCost > 0 ? Constant.GeneralLedgerStatus.Debit : Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(Math.Abs(AvgCost), 2)
            };
            glj = CreateObject(glj, _accountService);
            return glj;
            #endregion
        }

        public GeneralLedgerJournal CreateUnconfirmationJournalForStockAdjustmentDetail(StockAdjustment stockAdjustment, int AccountId, decimal AvgCost, IAccountService _accountService)
        {
            #region Inventory
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                SourceDocumentId = stockAdjustment.Id,
                TransactionDate = (DateTime)stockAdjustment.AdjustmentDate,
                Status = AvgCost > 0 ? Constant.GeneralLedgerStatus.Credit : Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(Math.Abs(AvgCost), 2),
            };
            glj = CreateObject(glj, _accountService);
            return glj;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService)
        {
            // if (stockAdjustmentTotal >= 0) then Debit Raw, Credit StockEquityAdjusment
            // if (stockAdjustmentTotal < 0) then Debit StockAdjustmentExpense, Credit Raw
            #region if (stockAdjustmentTotal >= 0) then Debit Raw, Credit StockEquityAdjustment
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            if (stockAdjustment.Total >= 0)
            {
                GeneralLedgerJournal creditstockequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(stockAdjustment.Total, 2)
                };
                creditstockequityadjustment = CreateObject(creditstockequityadjustment, _accountService);

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
                    Amount = Math.Round(Math.Abs(stockAdjustment.Total), 2)
                };
                debitstockadjustmentexpense = CreateObject(debitstockadjustmentexpense, _accountService);

                journals.Add(debitstockadjustmentexpense);
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
                GeneralLedgerJournal debitstockequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = stockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(stockAdjustment.Total, 2)
                };
                debitstockequityadjustment = CreateObject(debitstockequityadjustment, _accountService);

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
                    Amount = Math.Round(Math.Abs(stockAdjustment.Total), 2)
                };
                creditstockadjustmentexpense = CreateObject(creditstockadjustmentexpense, _accountService);

                journals.Add(creditstockadjustmentexpense);
            }
            #endregion
            return journals;
        }

        public GeneralLedgerJournal CreateConfirmationJournalForCustomerStockAdjustmentDetail(CustomerStockAdjustment customerStockAdjustment, int AccountId, decimal AvgCost, IAccountService _accountService)
        {
            #region Inventory
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.CustomerStockAdjustment,
                SourceDocumentId = customerStockAdjustment.Id,
                TransactionDate = (DateTime) customerStockAdjustment.AdjustmentDate,
                Status = AvgCost > 0 ? Constant.GeneralLedgerStatus.Debit : Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(Math.Abs(AvgCost), 2),
            };
            glj = CreateObject(glj, _accountService);
            return glj;
            #endregion
        }

        public GeneralLedgerJournal CreateUnconfirmationJournalForCustomerStockAdjustmentDetail(CustomerStockAdjustment customerStockAdjustment, int AccountId, decimal AvgCost, IAccountService _accountService)
        {
            #region Inventory
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.CustomerStockAdjustment,
                SourceDocumentId = customerStockAdjustment.Id,
                TransactionDate = (DateTime)customerStockAdjustment.AdjustmentDate,
                Status = AvgCost > 0 ? Constant.GeneralLedgerStatus.Credit : Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(Math.Abs(AvgCost), 2),
            };
            glj = CreateObject(glj, _accountService);
            return glj;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForCustomerStockAdjustment(CustomerStockAdjustment customerStockAdjustment, IAccountService _accountService)
        {
            // if (customerStockAdjustmentTotal >= 0) then Debit Raw, Credit StockEquityAdjusment
            // if (customerStockAdjustmentTotal < 0) then Debit StockAdjustmentExpense, Credit Raw
            #region if (customerStockAdjustmentTotal >= 0) then Debit Raw, Credit StockEquityAdjustment
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            if (customerStockAdjustment.Total >= 0)
            {
                GeneralLedgerJournal creditstockequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CustomerStockAdjustment,
                    SourceDocumentId = customerStockAdjustment.Id,
                    TransactionDate = (DateTime)customerStockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(customerStockAdjustment.Total, 2)
                };
                creditstockequityadjustment = CreateObject(creditstockequityadjustment, _accountService);

                journals.Add(creditstockequityadjustment);
            }
            #endregion
            #region if (customerStockAdjustmentTotal < 0) then Debit CustomerStockAdjustmentExpense, Credit Raw
            else
            {
                GeneralLedgerJournal debitstockadjustmentexpense = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.StockAdjustmentExpense).Id, // CustomerStockAdjustmentExpense ??
                    SourceDocument = Constant.GeneralLedgerSource.CustomerStockAdjustment,
                    SourceDocumentId = customerStockAdjustment.Id,
                    TransactionDate = (DateTime)customerStockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Math.Abs(customerStockAdjustment.Total), 2)
                };
                debitstockadjustmentexpense = CreateObject(debitstockadjustmentexpense, _accountService);

                journals.Add(debitstockadjustmentexpense);                
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCustomerStockAdjustment(CustomerStockAdjustment customerStockAdjustment, IAccountService _accountService)
        {
            // if (customerStockAdjustmentTotal >= 0) then Credit Raw, Debit StockEquityAdjustment
            // if (customerStockAdjustmentTotal < 0) then Credit StockAdjustmentExpense, Debit Raw
            #region if (customerStockAdjustmentTotal >= 0) then Credit Raw, Debit StockEquityAdjustment
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            if (customerStockAdjustment.Total >= 0)
            {
                GeneralLedgerJournal debitstockequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CustomerStockAdjustment,
                    SourceDocumentId = customerStockAdjustment.Id,
                    TransactionDate = customerStockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(customerStockAdjustment.Total, 2)
                };
                debitstockequityadjustment = CreateObject(debitstockequityadjustment, _accountService);

                journals.Add(debitstockequityadjustment);
            }
            #endregion
            #region if (customerStockAdjustmentTotal < 0) then Credit CustomerStockAdjustmentExpense, Debit Raw
            else
            {
                GeneralLedgerJournal creditstockadjustmentexpense = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.StockAdjustmentExpense).Id, // CustomerStockAdjustmentExpense ??
                    SourceDocument = Constant.GeneralLedgerSource.CustomerStockAdjustment,
                    SourceDocumentId = customerStockAdjustment.Id,
                    TransactionDate = customerStockAdjustment.AdjustmentDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Math.Abs(customerStockAdjustment.Total), 2)
                };
                creditstockadjustmentexpense = CreateObject(creditstockadjustmentexpense, _accountService);

                journals.Add(creditstockadjustmentexpense);
            }
            #endregion
            return journals;
        }

        // PURCHASE OPERATION
        public GeneralLedgerJournal CreateConfirmationJournalForPurchaseReceivalDetail(PurchaseReceival purchaseReceival, int AccountId, decimal Total, IAccountService _accountService)
        {
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = (DateTime) purchaseReceival.ReceivalDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(Total, 2)
            };
            glj = CreateObject(glj, _accountService);
            return glj;
        }

        public GeneralLedgerJournal CreateUnconfirmationJournalForPurchaseReceivalDetail(PurchaseReceival purchaseReceival, int AccountId, decimal Total, IAccountService _accountService)
        {
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = (DateTime)purchaseReceival.ReceivalDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(Total, 2)
            };
            glj = CreateObject(glj, _accountService);
            return glj;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseReceival(PurchaseReceival purchaseReceival, IAccountService _accountService)
        {
            // Debit Raw, Credit GoodsPendingClearance
            #region Debit Raw, Credit GoodsPendingClearance
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditgoodsPendingclearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = (DateTime)purchaseReceival.ReceivalDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(purchaseReceival.TotalAmount * purchaseReceival.ExchangeRateAmount, 2)
            };
            creditgoodsPendingclearance = CreateObject(creditgoodsPendingclearance, _accountService);

            journals.Add(creditgoodsPendingclearance);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseReceival(PurchaseReceival purchaseReceival, IAccountService _accountService)
        {
            // Credit Raw, Debit GoodsPendingClearance
            #region Credit Raw, Debit GoodsPendingClearance
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitgoodsPendingclearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = purchaseReceival.ReceivalDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(purchaseReceival.TotalAmount * purchaseReceival.ExchangeRateAmount, 2)
            };
            debitgoodsPendingclearance = CreateObject(debitgoodsPendingclearance, _accountService);

            journals.Add(debitgoodsPendingclearance);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateJournalForPurchaseInvoiceMigration(PurchaseInvoiceMigration purchaseInvoiceMigration,
                                           IAccountService _accountService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            // Debit GoodsPendingClearance, Credit AccountPayable
            #region Debit GoodsPendingClearance, Credit AccountPayable

            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency purchaseInvoiceMigrationCurrency = _currencyService.GetObjectById(purchaseInvoiceMigration.CurrencyId);
            GeneralLedgerJournal creditaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + purchaseInvoiceMigration.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoiceMigration,
                SourceDocumentId = purchaseInvoiceMigration.Id,
                TransactionDate = (DateTime)purchaseInvoiceMigration.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(purchaseInvoiceMigration.AmountPayable * purchaseInvoiceMigration.Rate, 2)
            };
            creditaccountpayable = CreateObject(creditaccountpayable, _accountService);
            journals.Add(creditaccountpayable);
            if (purchaseInvoiceMigrationCurrency.IsBase == false)
            {
                GLNonBaseCurrency creditaccountpayable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditaccountpayable.Id,
                    CurrencyId = purchaseInvoiceMigration.CurrencyId,
                    Amount = purchaseInvoiceMigration.AmountPayable,
                };
                creditaccountpayable2 = _gLNonBaseCurrencyService.CreateObject(creditaccountpayable2, _accountService);
            }
            GeneralLedgerJournal debitGoodsPendingClearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoiceMigration,
                SourceDocumentId = purchaseInvoiceMigration.Id,
                TransactionDate = (DateTime)purchaseInvoiceMigration.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(purchaseInvoiceMigration.AmountPayable * purchaseInvoiceMigration.Rate, 2)
            };
            debitGoodsPendingClearance = CreateObject(debitGoodsPendingClearance, _accountService);
            journals.Add(debitGoodsPendingClearance);

            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseInvoice(PurchaseInvoice purchaseInvoice, PurchaseReceival purchaseReceival,
                                           IAccountService _accountService,IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // Debit GoodsPendingClearance, Credit AccountPayable
            // Debit PPNMASUKAN, Debit ExchangeLoss or Credit ExchangeGain
            #region Debit GoodsPendingClearance, Credit AccountPayable
            decimal PreTax = purchaseInvoice.AmountPayable * 100 / (100 + purchaseInvoice.Tax);
            decimal Tax = purchaseInvoice.AmountPayable - PreTax;
            decimal Discount = PreTax * purchaseInvoice.Discount / 100;

            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency purchaseInvoiceCurrency = _currencyService.GetObjectById(purchaseInvoice.CurrencyId);
            GeneralLedgerJournal creditaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + purchaseInvoice.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(purchaseInvoice.AmountPayable * purchaseInvoice.ExchangeRateAmount, 2)
            };
            creditaccountpayable = CreateObject(creditaccountpayable, _accountService);
            journals.Add(creditaccountpayable);
            if (purchaseInvoiceCurrency.IsBase == false)
            {
                GLNonBaseCurrency creditaccountpayable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditaccountpayable.Id,
                    CurrencyId = purchaseInvoice.CurrencyId,
                    Amount = purchaseInvoice.AmountPayable,
                };
                creditaccountpayable2 = _gLNonBaseCurrencyService.CreateObject(creditaccountpayable2, _accountService);
            }
            GeneralLedgerJournal debitGoodsPendingClearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(PreTax * purchaseReceival.ExchangeRateAmount, 2)
            };
            debitGoodsPendingClearance = CreateObject(debitGoodsPendingClearance, _accountService);
            journals.Add(debitGoodsPendingClearance);

            #endregion
            #region Debit PPNMASUKAN, Debit ExchangeLoss or Credit ExchangeGain

            if (Tax > 0)
            {
                GeneralLedgerJournal debitPPNMASUKAN = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PPNMASUKAN).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                    SourceDocumentId = purchaseInvoice.Id,
                    TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(Tax * purchaseInvoice.ExchangeRateAmount, 2)
                };
                debitPPNMASUKAN = CreateObject(debitPPNMASUKAN, _accountService);
                journals.Add(debitPPNMASUKAN);
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
                    Amount = Math.Round((PreTax * purchaseInvoice.ExchangeRateAmount) - (PreTax * purchaseReceival.ExchangeRateAmount), 2)
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
                    Amount = Math.Round((PreTax * purchaseReceival.ExchangeRateAmount) - (PreTax * purchaseInvoice.ExchangeRateAmount), 2)
                };
                creditExchangeGain = CreateObject(creditExchangeGain, _accountService);
                journals.Add(creditExchangeGain);
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseInvoice(PurchaseInvoice purchaseInvoice, PurchaseReceival purchaseReceival,
                                           IAccountService _accountService,IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            // Credit GoodsPendingClearance, Debit AccountPayable
            // Credit PPNMASUKAN, Credit ExchangeLoss or Debit ExchangeGain
            #region Credit GoodsPendingClearance, Debit AccountPayable
            decimal PreTax = purchaseInvoice.AmountPayable * 100 / (100 + purchaseInvoice.Tax);
            decimal Tax = purchaseInvoice.AmountPayable - PreTax;
            decimal Discount = PreTax * purchaseInvoice.Discount / 100;
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency purchaseInvoiceCurrency = _currencyService.GetObjectById(purchaseInvoice.CurrencyId);
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable + purchaseInvoice.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(purchaseInvoice.AmountPayable * purchaseInvoice.ExchangeRateAmount, 2)
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);
            journals.Add(debitaccountpayable);

            if (purchaseInvoiceCurrency.IsBase == false)
            {
                GLNonBaseCurrency debitaccountpayable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitaccountpayable.Id,
                    CurrencyId = purchaseInvoice.CurrencyId,
                    Amount = purchaseInvoice.AmountPayable,
                };
                debitaccountpayable2 = _gLNonBaseCurrencyService.CreateObject(debitaccountpayable2, _accountService);
            }

            GeneralLedgerJournal credittGoodsPendingClearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(PreTax * purchaseReceival.ExchangeRateAmount, 2)
            };
            credittGoodsPendingClearance = CreateObject(credittGoodsPendingClearance, _accountService);
            journals.Add(credittGoodsPendingClearance);

            #endregion
            #region Credit PPNMASUKAN, Credit ExchangeLoss or Debit ExchangeGain
            if (Tax > 0)
            {
                GeneralLedgerJournal creditPPNMASUKAN = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PPNMASUKAN).Id,
                    SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                    SourceDocumentId = purchaseInvoice.Id,
                    TransactionDate = (DateTime)purchaseInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Tax * purchaseInvoice.ExchangeRateAmount, 2)
                };
                creditPPNMASUKAN = CreateObject(creditPPNMASUKAN, _accountService);
                journals.Add(creditPPNMASUKAN);
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
                    Amount = Math.Round((PreTax * purchaseInvoice.ExchangeRateAmount) - (PreTax * purchaseReceival.ExchangeRateAmount), 2)
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
                    Amount = Math.Round((PreTax * purchaseReceival.ExchangeRateAmount) - (PreTax * purchaseInvoice.ExchangeRateAmount), 2)
                };
                debitExchangeGain = CreateObject(debitExchangeGain, _accountService);
                journals.Add(debitExchangeGain);
            }
            #endregion
            return journals;
        }

        // SALES OPERATION
        public GeneralLedgerJournal CreateConfirmationJournalForDeliveryOrderDetail(DeliveryOrder deliveryOrder, int AccountId, decimal COGS, IAccountService _accountService)
        {
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.DeliveryOrder,
                SourceDocumentId = deliveryOrder.Id,
                TransactionDate = (DateTime)deliveryOrder.DeliveryDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(COGS, 2)
            };
            glj = CreateObject(glj, _accountService);
            return glj;
        }

        public GeneralLedgerJournal CreateUnconfirmationJournalForDeliveryOrderDetail(DeliveryOrder deliveryOrder, int AccountId, decimal COGS, IAccountService _accountService)
        {
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.DeliveryOrder,
                SourceDocumentId = deliveryOrder.Id,
                TransactionDate = (DateTime)deliveryOrder.DeliveryDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(COGS, 2)
            };
            glj = CreateObject(glj, _accountService);
            return glj;
        }

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
                Amount = Math.Round(deliveryOrder.TotalCOGS, 2)
            };
            debitcogs = CreateObject(debitcogs, _accountService);

            journals.Add(debitcogs);

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
                Amount = Math.Round(deliveryOrder.TotalCOGS, 2)
            };
            creditcogs = CreateObject(creditcogs, _accountService);

            journals.Add(creditcogs);

            return journals;
            #endregion
        }

        public GeneralLedgerJournal CreateReconciliationJournalForTemporaryDeliveryOrderDetailWaste(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, int AccountId, decimal COGSWaste, IAccountService _accountService)
        {
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrder.Id,
                TransactionDate = PushDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(COGSWaste, 2)
            };
            glj = CreateObject(glj, _accountService);
            return glj;
        }

        public GeneralLedgerJournal CreateUnreconciliationJournalForTemporaryDeliveryOrderDetailWaste(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, int AccountId, decimal COGSWaste, IAccountService _accountService)
        {
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrder.Id,
                TransactionDate = temporaryDeliveryOrder.PushDate.Value,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(COGSWaste, 2)
            };
            glj = CreateObject(glj, _accountService);
            return glj;
        }

        public IList<GeneralLedgerJournal> CreateReconciliationJournalForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, IAccountService _accountService)
        {
            // Credit Raw, Debit SampleAndTrialExpense
            #region Credit Raw , Debit SampleAndTrialExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitsampleandtrialexpense = new GeneralLedgerJournal()
            {
                // TOBECHANGED TrialAndSampleExpense
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrder.Id,
                TransactionDate = PushDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(temporaryDeliveryOrder.TotalWasteCOGS, 2)
            };
            debitsampleandtrialexpense = CreateObject(debitsampleandtrialexpense, _accountService);

            journals.Add(debitsampleandtrialexpense);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnreconciliationJournalForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrder temporaryDeliveryOrder, IAccountService _accountService)
        {
            // Debit Raw, Credit SampleAndTrialExpense
            #region Debit Raw , Credit SampleAndTrialExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditsampleandtrialexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrder.Id,
                TransactionDate = temporaryDeliveryOrder.PushDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(temporaryDeliveryOrder.TotalWasteCOGS, 2)
            };
            creditsampleandtrialexpense = CreateObject(creditsampleandtrialexpense, _accountService);

            journals.Add(creditsampleandtrialexpense);
            return journals;
            #endregion
        }

        public GeneralLedgerJournal CreateConfirmationJournalForTemporaryDeliveryOrderClearanceDetailWaste(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, int AccountId, decimal COGSWaste, IAccountService _accountService)
        {
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrderClearance.Id,
                TransactionDate = temporaryDeliveryOrderClearance.ClearanceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(COGSWaste, 2)
            };
            glj = CreateObject(glj, _accountService);
            return glj;
        }

        public GeneralLedgerJournal CreateUnconfirmationJournalForTemporaryDeliveryOrderClearanceDetailWaste(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, int AccountId, decimal COGSWaste, IAccountService _accountService)
        {
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrderClearance.Id,
                TransactionDate = temporaryDeliveryOrderClearance.ClearanceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(COGSWaste, 2)
            };
            glj = CreateObject(glj, _accountService);

            return glj;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForTemporaryDeliveryOrderClearanceWaste(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IAccountService _accountService)
        {
            // Credit Raw (Inventory), Debit SampleAndTrialExpense (SalesExpense)
            #region Credit Raw , Debit SampleAndTrialExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitsampleandtrialexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrderClearance.Id,
                TransactionDate = temporaryDeliveryOrderClearance.ClearanceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(temporaryDeliveryOrderClearance.TotalWasteCoGS, 2)
            };
            debitsampleandtrialexpense = CreateObject(debitsampleandtrialexpense, _accountService);

            journals.Add(debitsampleandtrialexpense);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForTemporaryDeliveryOrderClearanceWaste(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IAccountService _accountService)
        {
            // Debit Raw (Inventory), Credit SampleAndTrialExpense (SalesExpense)
            #region Debit Raw , Credit SampleAndTrialExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditsampleandtrialexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.SampleAndTrialExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = temporaryDeliveryOrderClearance.Id,
                TransactionDate = temporaryDeliveryOrderClearance.ClearanceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(temporaryDeliveryOrderClearance.TotalWasteCoGS, 2)
            };
            creditsampleandtrialexpense = CreateObject(creditsampleandtrialexpense, _accountService);

            journals.Add(creditsampleandtrialexpense);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateJournalForSalesInvoiceMigration(SalesInvoiceMigration salesInvoiceMigration,
                                            IAccountService _accountService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            // Debit AccountReceivable, Debit Discount, Debit PPNKELUARAN, Credit Revenue
            #region Debit AccountReceivable, Debit Discount, Debit PPNKELUARAN, Credit Revenue for fixed rate
            decimal Rate = salesInvoiceMigration.Rate;
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + salesInvoiceMigration.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoiceMigration,
                SourceDocumentId = salesInvoiceMigration.Id,
                TransactionDate = (DateTime)salesInvoiceMigration.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(salesInvoiceMigration.AmountReceivable * Rate, 2)
            };
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            Currency currency = _currencyService.GetObjectById(salesInvoiceMigration.CurrencyId);
            if (currency.IsBase == false)
            {
                GLNonBaseCurrency debitaccountreceivable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitaccountreceivable.Id,
                    CurrencyId = salesInvoiceMigration.CurrencyId,
                    Amount = salesInvoiceMigration.AmountReceivable,
                };
                   debitaccountreceivable2 = _gLNonBaseCurrencyService.CreateObject(debitaccountreceivable2, _accountService);
            }

            journals.Add(debitaccountreceivable);

            GeneralLedgerJournal creditrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoiceMigration,
                SourceDocumentId = salesInvoiceMigration.Id,
                TransactionDate = (DateTime)salesInvoiceMigration.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(salesInvoiceMigration.AmountReceivable * Rate, 2)
            };
            creditrevenue = CreateObject(creditrevenue, _accountService);

            journals.Add(creditrevenue);
            #endregion
            return journals;
        }

        public GeneralLedgerJournal CreateConfirmationJournalForSalesInvoiceDetail(SalesInvoice salesInvoice, int AccountId, decimal COSRate, IAccountService _accountService)
        {
            #region Credit Inventory
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = (DateTime)salesInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(COSRate, 2),
            };
            glj = CreateObject(glj, _accountService);
            return glj;
            #endregion
        }

        public GeneralLedgerJournal CreateUnconfirmationJournalForSalesInvoiceDetail(SalesInvoice salesInvoice, int AccountId, decimal COSRate, IAccountService _accountService)
        {
            #region Debit Inventory
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = salesInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(COSRate, 2)
            };
            glj = CreateObject(glj, _accountService);
            return glj;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesInvoice(SalesInvoice salesInvoice, Contact contact,
                                           IAccountService _accountService, IExchangeRateService _exchangeRateService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            // Debit AccountReceivable, Debit Discount, Debit PPNKELUARAN, Credit Revenue
            // Debit COS, Credit FinishedGoods

            #region Debit AccountReceivable, Debit Discount, Debit PPNKELUARAN, Credit Revenue for fixed rate
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
                Amount = Math.Round(salesInvoice.AmountReceivable * Rate, 2)
            };  
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            if (currency.IsBase == false)
            {
                GLNonBaseCurrency debitaccountreceivable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = debitaccountreceivable.Id,
                    CurrencyId = salesInvoice.CurrencyId,
                    Amount = salesInvoice.AmountReceivable,
                };
                debitaccountreceivable2 = _gLNonBaseCurrencyService.CreateObject(debitaccountreceivable2, _accountService);
            }

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
                    Amount = Math.Round(Discount * Rate, 2)
                };
                debitdiscount = CreateObject(debitdiscount, _accountService);
                journals.Add(debitdiscount);
            }

            if (Tax > 0)
            {
                GeneralLedgerJournal creditppnkeluaran = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PPNKELUARAN).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = (DateTime)salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = contact.IsTaxable ? (decimal) Math.Round(Tax * Rate, 2) : (decimal) Math.Round(salesInvoice.AmountReceivable * Rate/ 11, 2)
                };
                creditppnkeluaran = CreateObject(creditppnkeluaran, _accountService);
                journals.Add(creditppnkeluaran);
            }

            GeneralLedgerJournal creditrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = (DateTime)salesInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = contact.IsTaxable ? (decimal) Math.Round(PreTax * Rate, 2) : (decimal) Math.Round(salesInvoice.DPP * Rate * 10 / 11, 2)
            };
            creditrevenue = CreateObject(creditrevenue, _accountService);

            journals.Add(creditrevenue);
            #endregion
            #region Debit COS
            if (salesInvoice.TotalCOS > 0)
            {
                GeneralLedgerJournal debitCOS = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGS).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = (DateTime)salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(salesInvoice.TotalCOS, 2)
                };
                debitCOS = CreateObject(debitCOS, _accountService);

                journals.Add(debitCOS);                
            }
            #endregion
            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesInvoice(SalesInvoice salesInvoice, Contact contact,
            IAccountService _accountService, IExchangeRateService _exchangeRateService, 
            ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            // Credit AccountReceivable, Credit Discount, Credit PPNKELUARAN, Debit Revenue
            // Credit COS, Debit FinishedGoods
            #region Credit AccountReceivable, Credit Discount, Credit PPNKELUARAN, Debit Revenue with master exchangeRate
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
                Rate = _exchangeRateService.GetLatestRate(salesInvoice.ConfirmationDate.Value, currency).Rate;
            }

            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            Currency salesInvoiceCurrency = _currencyService.GetObjectById(salesInvoice.CurrencyId);
            GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable + salesInvoice.CurrencyId).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = salesInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(salesInvoice.AmountReceivable * Rate, 2)
            };
            creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);
            journals.Add(creditaccountreceivable);

            if (salesInvoiceCurrency.IsBase == false)
            {
                GLNonBaseCurrency creditaccountreceivable2 = new GLNonBaseCurrency()
                {
                    GeneralLedgerJournalId = creditaccountreceivable.Id,
                    CurrencyId = salesInvoice.CurrencyId,
                    Amount = salesInvoice.AmountReceivable,
                };
                creditaccountreceivable2 = _gLNonBaseCurrencyService.CreateObject(creditaccountreceivable2, _accountService);
            }

            if (Discount > 0)
            {
                GeneralLedgerJournal creditdiscount = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Discount).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Discount * Rate, 2)
                };
                creditdiscount = CreateObject(creditdiscount, _accountService);
                journals.Add(creditdiscount);
            }

            if (Tax > 0)
            {
                GeneralLedgerJournal debitppnkeluaran = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.PPNKELUARAN).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = contact.IsTaxable ? Math.Round (Tax * Rate, 2) : Math.Round(salesInvoice.AmountReceivable * Rate/ 11, 2)
                };
                debitppnkeluaran = CreateObject(debitppnkeluaran, _accountService);
                journals.Add(debitppnkeluaran);
            }

            GeneralLedgerJournal debitrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = salesInvoice.InvoiceDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = contact.IsTaxable ? Math.Round (PreTax * Rate, 2) : Math.Round(salesInvoice.AmountReceivable * Rate * 10 / 11, 2)
            };
            debitrevenue = CreateObject(debitrevenue, _accountService);

            journals.Add(debitrevenue);

            #endregion
            #region Credit COS
            if (salesInvoice.TotalCOS > 0)
            {
                GeneralLedgerJournal creditCOS = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGS).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = salesInvoice.InvoiceDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(salesInvoice.TotalCOS, 2)
                };
                creditCOS = CreateObject(creditCOS, _accountService);
                journals.Add(creditCOS);
            }
            #endregion
            return journals;
        }

        // MANUFACTURING RECOVERY
        public IList<GeneralLedgerJournal> CreateFinishedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            // Credit Raw/Stock (Core, Compound, Accessories), Debit FinishedGoods/ManufacturedGoods (Roller)
            #region Credit Raw (Core, Compound, Accessories), Debit FinishedGoods (Roller)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            if (recoveryOrderDetail.AccessoriesCost > 0)
            {
                GeneralLedgerJournal creditaccessory = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Accessory).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                    SourceDocumentId = recoveryOrderDetail.Id,
                    TransactionDate = (DateTime)recoveryOrderDetail.FinishedDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(recoveryOrderDetail.AccessoriesCost, 2)
                };
                creditaccessory = CreateObject(creditaccessory, _accountService);
                journals.Add(creditaccessory);
            }

            if (recoveryOrderDetail.CoreCost > 0)
            {
                GeneralLedgerJournal creditcore = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Core).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                    SourceDocumentId = recoveryOrderDetail.Id,
                    TransactionDate = (DateTime)recoveryOrderDetail.FinishedDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(recoveryOrderDetail.CoreCost, 2)
                };
                creditcore = CreateObject(creditcore, _accountService);
                journals.Add(creditcore);
            }

            GeneralLedgerJournal creditcompound = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Compound).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = (DateTime)recoveryOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(recoveryOrderDetail.CompoundCost, 2)
            };
            creditcompound = CreateObject(creditcompound, _accountService);
            journals.Add(creditcompound);
            
            GeneralLedgerJournal debitfinishedroller = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Roller).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = (DateTime)recoveryOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(recoveryOrderDetail.TotalCost, 2)
            };
            debitfinishedroller = CreateObject(debitfinishedroller, _accountService);

            journals.Add(debitfinishedroller);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnfinishedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            // Debit Raw (Core, Compound, Accessories), Credit FinishedGoods (Roller)
            #region Debit Raw (Core, Compound, Accessories), Credit FinishedGoods (Roller)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            if (recoveryOrderDetail.AccessoriesCost > 0)
            {
                GeneralLedgerJournal debitaccessory = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Accessory).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                    SourceDocumentId = recoveryOrderDetail.Id,
                    TransactionDate = (DateTime)recoveryOrderDetail.FinishedDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(recoveryOrderDetail.AccessoriesCost, 2)
                };
                debitaccessory = CreateObject(debitaccessory, _accountService);
                journals.Add(debitaccessory);
            }

            if (recoveryOrderDetail.CoreCost > 0)
            {
                GeneralLedgerJournal debitcore = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Core).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                    SourceDocumentId = recoveryOrderDetail.Id,
                    TransactionDate = (DateTime)recoveryOrderDetail.FinishedDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(recoveryOrderDetail.CoreCost, 2)
                };
                debitcore = CreateObject(debitcore, _accountService);
                journals.Add(debitcore);
            }

            GeneralLedgerJournal debitcompound = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Compound).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = (DateTime)recoveryOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(recoveryOrderDetail.CompoundCost, 2)
            };
            debitcompound = CreateObject(debitcompound, _accountService);
            journals.Add(debitcompound);

            GeneralLedgerJournal creditfinishedroller = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Roller).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = (DateTime)recoveryOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(recoveryOrderDetail.TotalCost, 2)
            };
            creditfinishedroller = CreateObject(creditfinishedroller, _accountService);

            journals.Add(creditfinishedroller);
            return journals;
            #endregion
         }

        public IList<GeneralLedgerJournal> CreateRejectedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            // Credit Raw (Core, Compound), Debit RecoveryExpense
            #region Credit Raw (Core, Compound, Accessories), Debit RecoveryExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            if (recoveryOrderDetail.CoreCost > 0)
            {
                GeneralLedgerJournal creditcore = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Core).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                    SourceDocumentId = recoveryOrderDetail.Id,
                    TransactionDate = (DateTime)recoveryOrderDetail.RejectedDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(recoveryOrderDetail.CoreCost, 2)
                };
                creditcore = CreateObject(creditcore, _accountService);
                journals.Add(creditcore);
            }

            GeneralLedgerJournal creditcompound = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Compound).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = (DateTime)recoveryOrderDetail.RejectedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(recoveryOrderDetail.CompoundCost, 2)
            };
            creditcompound = CreateObject(creditcompound, _accountService);
            journals.Add(creditcompound);

            GeneralLedgerJournal debitrecoveryexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = (DateTime)recoveryOrderDetail.RejectedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(recoveryOrderDetail.TotalCost, 2)
            };
            debitrecoveryexpense = CreateObject(debitrecoveryexpense, _accountService);

            journals.Add(debitrecoveryexpense);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUndoRejectedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            // Debit Raw (Core, Compound), Credit RecoveryExpense
            #region Debit Raw (Core, Compound, Accessories), Credit RecoveryExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UndoRejectDate = DateTime.Now;

            if (recoveryOrderDetail.CoreCost > 0)
            {
                GeneralLedgerJournal debitcore = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Core).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                    SourceDocumentId = recoveryOrderDetail.Id,
                    TransactionDate = (DateTime)recoveryOrderDetail.RejectedDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(recoveryOrderDetail.CoreCost, 2)
                };
                debitcore = CreateObject(debitcore, _accountService);
                journals.Add(debitcore);
            }

            GeneralLedgerJournal debitcompound = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Compound).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = (DateTime)recoveryOrderDetail.RejectedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(recoveryOrderDetail.CompoundCost, 2)
            };
            debitcompound = CreateObject(debitcompound, _accountService);
            journals.Add(debitcompound);

            GeneralLedgerJournal creditrecoveryexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = recoveryOrderDetail.RejectedDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(recoveryOrderDetail.TotalCost, 2)
            };
            creditrecoveryexpense = CreateObject(creditrecoveryexpense, _accountService);

            journals.Add(creditrecoveryexpense);
            return journals;
            #endregion
        }

        // MANUFACTURING CONVERSION
        public GeneralLedgerJournal CreateConfirmationJournalForBlendingWorkOrderDetail(BlendingWorkOrder blendingWorkOrder, int AccountId, decimal TotalCOGS, IAccountService _accountService)
        {
            #region Credit Inventory
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.BlendingWorkOrder,
                SourceDocumentId = blendingWorkOrder.Id,
                TransactionDate = (DateTime) blendingWorkOrder.BlendingDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(TotalCOGS, 2),
            };
            glj = CreateObject(glj, _accountService);
            return glj;
            #endregion
        }

        public GeneralLedgerJournal CreateUnconfirmationJournalForBlendingWorkOrderDetail(BlendingWorkOrder blendingWorkOrder, int AccountId, decimal TotalCOGS, IAccountService _accountService)
        {
            #region Debit Inventory
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.BlendingWorkOrder,
                SourceDocumentId = blendingWorkOrder.Id,
                TransactionDate = (DateTime)blendingWorkOrder.BlendingDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(TotalCOGS, 2),
            };
            glj = CreateObject(glj, _accountService);
            return glj;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForBlendingWorkOrder(BlendingWorkOrder blendingWorkOrder, int AccountId, IAccountService _accountService, decimal TotalCost)
        {
            // Credit Raw (Source Items), Debit FinishedGoods (Chemical), Debit BlendingExpense (2%)
            #region Credit Raw (Source Items), Debit FinishedGoods (Chemical), Debit BlendingExpense (2%)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.BlendingWorkOrder,
                SourceDocumentId = blendingWorkOrder.Id,
                TransactionDate = blendingWorkOrder.BlendingDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(TotalCost * (decimal) 0.98, 2),
            };
            debitfinishedgoods = CreateObject(debitfinishedgoods, _accountService);

            GeneralLedgerJournal debitproductioncost = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlendingWorkOrder,
                SourceDocumentId = blendingWorkOrder.Id,
                TransactionDate = blendingWorkOrder.BlendingDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(TotalCost * (decimal) 0.02, 2),
            };
            debitproductioncost = CreateObject(debitproductioncost, _accountService);

            journals.Add(debitfinishedgoods);
            journals.Add(debitproductioncost);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForBlendingWorkOrder(BlendingWorkOrder blendingWorkOrder, int AccountId, IAccountService _accountService, decimal TotalCost)
        {
            // Debit Raw (Source Items), Credit FinishedGoods (Chemical), Credit BlendingExpense (2%)
            #region Debit Raw (Source Items), Credit FinishedGoods (Chemical), Credit BlendingExpense (2%)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            
            GeneralLedgerJournal creditfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.BlendingWorkOrder,
                SourceDocumentId = blendingWorkOrder.Id,
                TransactionDate = blendingWorkOrder.BlendingDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(TotalCost * (decimal) 0.98, 2)
            };
            creditfinishedgoods = CreateObject(creditfinishedgoods, _accountService);

            GeneralLedgerJournal creditproductioncost = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlendingWorkOrder,
                SourceDocumentId = blendingWorkOrder.Id,
                TransactionDate = blendingWorkOrder.BlendingDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(TotalCost * (decimal) 0.02, 2)
            };
            creditproductioncost = CreateObject(creditproductioncost, _accountService);

            journals.Add(creditfinishedgoods);
            journals.Add(creditproductioncost);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateFinishedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            // Credit Raw (RollBlanket, Bars, Adhesive), Debit FinishedGoods (Blanket)
            #region Credit Raw (RollBlanket, Bars, Adhesive), Debit FinishedGoods (Blanket)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditrollblanket = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.RollBlanket).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = (DateTime)blanketOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(blanketOrderDetail.RollBlanketCost, 2)
            };
            creditrollblanket = CreateObject(creditrollblanket, _accountService);
            journals.Add(creditrollblanket);

            if (blanketOrderDetail.AdhesiveCost > 0)
            {
                GeneralLedgerJournal creditadhesive = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.AdhesiveBlanket).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                    SourceDocumentId = blanketOrderDetail.Id,
                    TransactionDate = (DateTime)blanketOrderDetail.FinishedDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(blanketOrderDetail.AdhesiveCost, 2)
                };
                creditadhesive = CreateObject(creditadhesive, _accountService);
                journals.Add(creditadhesive);
            }

            if (blanketOrderDetail.BarCost > 0)
            {
                GeneralLedgerJournal creditbar = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Bar).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                    SourceDocumentId = blanketOrderDetail.Id,
                    TransactionDate = (DateTime)blanketOrderDetail.FinishedDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(blanketOrderDetail.BarCost, 2)
                };
                creditbar = CreateObject(creditbar, _accountService);
                journals.Add(creditbar);
            }

            GeneralLedgerJournal debitfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Blanket).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = (DateTime)blanketOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(blanketOrderDetail.TotalCost, 2)
            };
            debitfinishedgoods = CreateObject(debitfinishedgoods, _accountService);

            journals.Add(debitfinishedgoods);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnfinishedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            // Debit Raw (RollBlanket, Bars, Adhesive), Credit FinishedGoods (Blanket)
            #region Debit Raw (RollBlanket, Bars, Adhesive), Credit FinishedGoods (Blanket)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            GeneralLedgerJournal debitrollblanket = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.RollBlanket).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = (DateTime)blanketOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(blanketOrderDetail.RollBlanketCost, 2)
            };
            debitrollblanket = CreateObject(debitrollblanket, _accountService);
            journals.Add(debitrollblanket);

            if (blanketOrderDetail.AdhesiveCost > 0)
            {
                GeneralLedgerJournal debitadhesive = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.AdhesiveBlanket).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                    SourceDocumentId = blanketOrderDetail.Id,
                    TransactionDate = (DateTime)blanketOrderDetail.FinishedDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(blanketOrderDetail.AdhesiveCost, 2)
                };
                debitadhesive = CreateObject(debitadhesive, _accountService);
                journals.Add(debitadhesive);
            }

            if (blanketOrderDetail.BarCost > 0)
            {
                GeneralLedgerJournal debitbar = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Bar).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                    SourceDocumentId = blanketOrderDetail.Id,
                    TransactionDate = (DateTime)blanketOrderDetail.FinishedDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(blanketOrderDetail.BarCost, 2)
                };
                debitbar = CreateObject(debitbar, _accountService);
                journals.Add(debitbar);
            }

            GeneralLedgerJournal creditfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Blanket).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = (DateTime)blanketOrderDetail.FinishedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(blanketOrderDetail.TotalCost, 2)
            };
            creditfinishedgoods = CreateObject(creditfinishedgoods, _accountService);

            journals.Add(creditfinishedgoods);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateRejectedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            // Credit Raw (RollBlanket, Bars, Adhesive), Debit ConversionExpense
            #region Credit Raw (RollBlanket, Bars, Adhesive), Debit ConversionExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditrollblanket = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.RollBlanket).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = (DateTime)blanketOrderDetail.RejectedDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(blanketOrderDetail.RollBlanketCost, 2)
            };
            creditrollblanket = CreateObject(creditrollblanket, _accountService);
            journals.Add(creditrollblanket);

            if (blanketOrderDetail.AdhesiveCost > 0)
            {
                GeneralLedgerJournal creditadhesive = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.AdhesiveBlanket).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                    SourceDocumentId = blanketOrderDetail.Id,
                    TransactionDate = (DateTime)blanketOrderDetail.RejectedDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(blanketOrderDetail.AdhesiveCost, 2)
                };
                creditadhesive = CreateObject(creditadhesive, _accountService);
                journals.Add(creditadhesive);
            }

            if (blanketOrderDetail.BarCost > 0)
            {
                GeneralLedgerJournal creditbar = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Bar).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                    SourceDocumentId = blanketOrderDetail.Id,
                    TransactionDate = (DateTime)blanketOrderDetail.RejectedDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(blanketOrderDetail.BarCost, 2)
                };
                creditbar = CreateObject(creditbar, _accountService);
                journals.Add(creditbar);
            }

            GeneralLedgerJournal debitconversionexpense = new GeneralLedgerJournal()
            {
                // TODO: Use ConversionExpense
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = (DateTime)blanketOrderDetail.RejectedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(blanketOrderDetail.TotalCost, 2)
            };
            debitconversionexpense = CreateObject(debitconversionexpense, _accountService);

            journals.Add(debitconversionexpense);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUndoRejectedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            // Debit Raw (RollBlanket, Bars, Adhesive), Credit ConversionExpense
            #region Debit Raw (RollBlanket, Bars, Adhesive), Credit ConversionExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitrollblanket = new GeneralLedgerJournal()
            {
                AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.RollBlanket).AccountId.GetValueOrDefault(),
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = (DateTime)blanketOrderDetail.RejectedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(blanketOrderDetail.RollBlanketCost, 2)
            };
            debitrollblanket = CreateObject(debitrollblanket, _accountService);
            journals.Add(debitrollblanket);

            if (blanketOrderDetail.AdhesiveCost > 0)
            {
                GeneralLedgerJournal debitadhesive = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.AdhesiveBlanket).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                    SourceDocumentId = blanketOrderDetail.Id,
                    TransactionDate = (DateTime)blanketOrderDetail.RejectedDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(blanketOrderDetail.AdhesiveCost, 2)
                };
                debitadhesive = CreateObject(debitadhesive, _accountService);
                journals.Add(debitadhesive);
            }

            if (blanketOrderDetail.BarCost > 0)
            {
                GeneralLedgerJournal debitbar = new GeneralLedgerJournal()
                {
                    AccountId = _itemTypeService.GetObjectByName(Constant.ItemTypeCase.Bar).AccountId.GetValueOrDefault(),
                    SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                    SourceDocumentId = blanketOrderDetail.Id,
                    TransactionDate = (DateTime)blanketOrderDetail.RejectedDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Round(blanketOrderDetail.BarCost, 2)
                };
                debitbar = CreateObject(debitbar, _accountService);
                journals.Add(debitbar);
            }

            GeneralLedgerJournal creditconversionexpense = new GeneralLedgerJournal()
            {
                // TODO: Use ConversionExpense
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ManufacturingExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = blanketOrderDetail.RejectedDate.Value,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(blanketOrderDetail.TotalCost, 2)
            };
            creditconversionexpense = CreateObject(creditconversionexpense, _accountService);

            journals.Add(creditconversionexpense);
            return journals;
            #endregion
        }

        public GeneralLedgerJournal CreateConfirmationJournalForRepackingDetail(Repacking repacking, int AccountId, decimal TotalCOGS, IAccountService _accountService)
        {
            #region Credit Inventory
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.Repacking,
                SourceDocumentId = repacking.Id,
                TransactionDate = (DateTime)repacking.RepackingDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(TotalCOGS, 2),
            };
            glj = CreateObject(glj, _accountService);
            return glj;
            #endregion
        }

        public GeneralLedgerJournal CreateUnconfirmationJournalForRepackingDetail(Repacking repacking, int AccountId, decimal TotalCOGS, IAccountService _accountService)
        {
            #region Debit Inventory
            GeneralLedgerJournal glj = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.Repacking,
                SourceDocumentId = repacking.Id,
                TransactionDate = (DateTime)repacking.RepackingDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(TotalCOGS, 2),
            };
            glj = CreateObject(glj, _accountService);
            return glj;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForRepacking(Repacking repacking, int AccountId, IAccountService _accountService, decimal TotalCost)
        {
            // Credit Raw (Source Items), Debit FinishedGoods (Chemical), Debit BlendingExpense (2%)
            #region Credit Raw (Source Items), Debit FinishedGoods (Chemical), Debit BlendingExpense (2%)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.Repacking,
                SourceDocumentId = repacking.Id,
                TransactionDate = repacking.RepackingDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Math.Round(TotalCost, 2),
            };
            debitfinishedgoods = CreateObject(debitfinishedgoods, _accountService);

            journals.Add(debitfinishedgoods);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForRepacking(Repacking repacking, int AccountId, IAccountService _accountService, decimal TotalCost)
        {
            // Debit Raw (Source Items), Credit FinishedGoods (Chemical), Credit BlendingExpense (2%)
            #region Debit Raw (Source Items), Credit FinishedGoods (Chemical), Credit BlendingExpense (2%)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = AccountId,
                SourceDocument = Constant.GeneralLedgerSource.Repacking,
                SourceDocumentId = repacking.Id,
                TransactionDate = repacking.RepackingDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Math.Round(TotalCost, 2)
            };
            creditfinishedgoods = CreateObject(creditfinishedgoods, _accountService);

            journals.Add(creditfinishedgoods);
            return journals;
            #endregion
        }
    }
}
