using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Core.Interface.Validation;
using System.Data.Objects;

namespace Service.Service
{
    public class ClosingService : IClosingService
    {
        private IClosingRepository _repository;
        private IClosingValidator _validator;

        public ClosingService(IClosingRepository _closingRepository, IClosingValidator _closingValidator)
        {
            _repository = _closingRepository;
            _validator = _closingValidator;
        }

        public IClosingValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Closing> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Closing> GetAll()
        {
            return _repository.GetAll();
        }

        public Closing GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Closing GetObjectByPeriodAndYear(int Period, int YearPeriod)
        {
            return _repository.GetObjectByPeriodAndYear(Period, YearPeriod);
        }

        public Closing CreateObject(Closing closing, IList<ExchangeRateClosing> exchangeRateClosing ,IAccountService _accountService, IValidCombService _validCombService,IExchangeRateClosingService _exchangeRateClosingService)
        {
            closing.Errors = new Dictionary<String, String>();
            // Create all ValidComb

            if (_validator.ValidCreateObject(closing, this))
            {
                closing = _repository.CreateObject(closing);
                IList<Account> allAccounts = _accountService.GetQueryable().Where(x => !x.IsDeleted).OrderBy(x => x.Code).ToList();
                foreach (var account in allAccounts)
                {
                    ValidComb validComb = new ValidComb()
                    {
                        AccountId = account.Id,
                        ClosingId = closing.Id,
                        Amount = 0
                    };
                    _validCombService.CreateObject(validComb, _accountService, this);
                }
                foreach (var eRClosing in exchangeRateClosing)
                {
                    ExchangeRateClosing er = new ExchangeRateClosing()
                    {
                        ClosingId = closing.Id,
                        Rate = eRClosing.Rate,
                        CurrencyId = eRClosing.CurrencyId,
                    };
                    _exchangeRateClosingService.CreateObject(er);
                }
            }
            return closing;
        }

        public Closing CloseObject(Closing closing, IAccountService _accountService,
                                   IGeneralLedgerJournalService _generalLedgerJournalService, IValidCombService _validCombService,
            IGLNonBaseCurrencyService _gLNonBaseCurrencyService,IExchangeRateClosingService _exchangeRateClosingService,
            IVCNonBaseCurrencyService _vCNonBaseCurrencyService)
        {
            if (_validator.ValidCloseObject(closing, this))
            {
                // Get Last Closing
                var lastClosing = _repository.GetQueryable().Where(x => EntityFunctions.AddDays(EntityFunctions.TruncateTime(x.EndDatePeriod), 1) == closing.BeginningPeriod).FirstOrDefault();
                
                // Count ValidComb for each leaf account
                IList<Account> leafAccounts = _accountService.GetLeafObjects();
                foreach(var leaf in leafAccounts)
                {
                   
                    DateTime EndDate = closing.EndDatePeriod.AddDays(1);
                    IList<GeneralLedgerJournal> ledgers = _generalLedgerJournalService.GetQueryable()
                                                          .Where(x => x.AccountId == leaf.Id && 
                                                                 x.TransactionDate >= closing.BeginningPeriod && 
                                                                 x.TransactionDate < EndDate && x.SourceDocument != Constant.GeneralLedgerSource.Closing)
                                                          .ToList();
                  
                    decimal totalAmountInLedgers = 0;
                    decimal totalAmountLast = 0;
                    if (lastClosing != null)
                    {
                        totalAmountLast = _validCombService.GetQueryable().
                            Where(x => x.ClosingId == lastClosing.Id && x.AccountId == leaf.Id).FirstOrDefault().Amount;
                    }
                    foreach(var ledger in ledgers)
                    {
                        Account account = _accountService.GetObjectById(ledger.AccountId);
                        if ((ledger.Status == Constant.GeneralLedgerStatus.Debit &&
                            (account.Group == Constant.AccountGroup.Asset ||
                             account.Group == Constant.AccountGroup.Expense)) ||
                           (ledger.Status == Constant.GeneralLedgerStatus.Credit &&
                            (account.Group == Constant.AccountGroup.Liability ||
                             account.Group == Constant.AccountGroup.Equity ||
                             account.Group == Constant.AccountGroup.Revenue)))
                        {
                            totalAmountInLedgers += ledger.Amount;
                        }
                        else 
                        {
                            totalAmountInLedgers -= ledger.Amount;
                        }
                    }
                    totalAmountInLedgers += totalAmountLast;
                   #region adjust Exrate
                    decimal totalCurrencyAmountInLedger = 0;
                    decimal totalAmountForVCnonBAse = 0;
                    IList<ExchangeRateClosing> exRates = _exchangeRateClosingService.GetQueryable().Where(x => x.ClosingId == closing.Id).ToList();
                    foreach (var exRate in exRates)
                    {
                        if (closing.IsYear == true)
                        {
                            if((leaf.LegacyCode == Constant.AccountLegacyCode.AccountPayable + exRate.CurrencyId) ||
                            (leaf.LegacyCode == Constant.AccountLegacyCode.AccountReceivable + exRate.CurrencyId) ||
                            (leaf.LegacyCode == Constant.AccountLegacyCode.CashBank + exRate.CurrencyId))
                            {
                                if (lastClosing != null)
                                {
                                    var lastValid = _vCNonBaseCurrencyService.GetQueryable()
                                        .Where(x => x.ValidComb.AccountId == leaf.Id && x.ValidComb.ClosingId == lastClosing.Id)
                                        .FirstOrDefault();
                                    if (lastValid != null)
                                    {
                                        totalCurrencyAmountInLedger += lastValid.Amount;
                                        totalAmountForVCnonBAse += lastValid.Amount;
                                    }
                                }

                                IList<GLNonBaseCurrency> glNonBases = _gLNonBaseCurrencyService.GetQueryable()
                                                             .Where(x => x.GeneralLedgerJournal.AccountId == leaf.Id &&
                                                                    x.GeneralLedgerJournal.TransactionDate >= closing.BeginningPeriod &&
                                                                    x.GeneralLedgerJournal.TransactionDate < EndDate && x.CurrencyId == exRate.CurrencyId).ToList();
                                foreach (var glNonBase in glNonBases)
                                {
                                    Account account = _accountService.GetObjectById(glNonBase.GeneralLedgerJournal.AccountId);
                                    if ((glNonBase.GeneralLedgerJournal.Status == Constant.GeneralLedgerStatus.Debit &&
                                        (account.Group == Constant.AccountGroup.Asset ||
                                         account.Group == Constant.AccountGroup.Expense)) ||
                                       (glNonBase.GeneralLedgerJournal.Status == Constant.GeneralLedgerStatus.Credit &&
                                        (account.Group == Constant.AccountGroup.Liability ||
                                         account.Group == Constant.AccountGroup.Equity ||
                                         account.Group == Constant.AccountGroup.Revenue)))
                                    {
                                        totalCurrencyAmountInLedger += glNonBase.Amount;
                                        totalAmountForVCnonBAse += glNonBase.Amount;
                                    }
                                    else
                                    {
                                        totalCurrencyAmountInLedger -= glNonBase.Amount;
                                        totalAmountForVCnonBAse -= glNonBase.Amount;
                                    }
                                }
                                totalCurrencyAmountInLedger = totalCurrencyAmountInLedger * exRate.Rate;
                            }
                        }
                        else
                        {
                            if (leaf.LegacyCode == Constant.AccountLegacyCode.CashBank + exRate.CurrencyId)
                            {
                                if (lastClosing != null)
                                {
                                    var lastValid = _vCNonBaseCurrencyService.GetQueryable()
                                        .Where(x => x.ValidComb.AccountId == leaf.Id && x.ValidComb.ClosingId == lastClosing.Id)
                                        .FirstOrDefault();
                                    if (lastValid != null)
                                    {
                                        totalCurrencyAmountInLedger += lastValid.Amount;
                                        totalAmountForVCnonBAse += lastValid.Amount;
                                    }
                                }

                                IList<GLNonBaseCurrency> glNonBases = _gLNonBaseCurrencyService.GetQueryable()
                              .Where(x => x.GeneralLedgerJournal.AccountId == leaf.Id &&
                                                                    x.GeneralLedgerJournal.TransactionDate >= closing.BeginningPeriod &&
                                                                    x.GeneralLedgerJournal.TransactionDate < EndDate && x.CurrencyId == exRate.CurrencyId).ToList();
                                foreach (var glNonBase in glNonBases)
                                {
                                    Account account = _accountService.GetObjectById(glNonBase.GeneralLedgerJournal.AccountId);
                                    if ((glNonBase.GeneralLedgerJournal.Status == Constant.GeneralLedgerStatus.Debit &&
                                        (account.Group == Constant.AccountGroup.Asset ||
                                         account.Group == Constant.AccountGroup.Expense)) ||
                                       (glNonBase.GeneralLedgerJournal.Status == Constant.GeneralLedgerStatus.Credit &&
                                        (account.Group == Constant.AccountGroup.Liability ||
                                         account.Group == Constant.AccountGroup.Equity ||
                                         account.Group == Constant.AccountGroup.Revenue)))
                                    {
                                        totalCurrencyAmountInLedger += glNonBase.Amount;
                                        totalAmountForVCnonBAse += glNonBase.Amount;
                                    }
                                    else
                                    {
                                        totalCurrencyAmountInLedger -= glNonBase.Amount;
                                        totalAmountForVCnonBAse -= glNonBase.Amount;
                                    }
                                }
                                totalCurrencyAmountInLedger = totalCurrencyAmountInLedger * exRate.Rate;
                            }
                        }

                        if (totalAmountInLedgers > totalCurrencyAmountInLedger)
                            {
                                if (closing.IsYear == true)
                                {
                                    if (leaf.LegacyCode == Constant.AccountLegacyCode.AccountPayable + exRate.CurrencyId)
                                        {
                                            GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                                            { 
                                                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                                                SourceDocument = Constant.GeneralLedgerSource.Closing,
                                                SourceDocumentId = closing.Id,
                                                TransactionDate = (DateTime)closing.EndDatePeriod,
                                                Status = Constant.GeneralLedgerStatus.Credit,
                                                Amount = totalAmountInLedgers - totalCurrencyAmountInLedger
                                            };
                                            creditExchangeGain = _generalLedgerJournalService.CreateObject(creditExchangeGain, _accountService);

                                            GeneralLedgerJournal debitAccountPayable = new GeneralLedgerJournal()
                                            {
                                                AccountId = leaf.Id,
                                                SourceDocument = Constant.GeneralLedgerSource.Closing,
                                                SourceDocumentId = closing.Id,
                                                TransactionDate = (DateTime)closing.EndDatePeriod,
                                                Status = Constant.GeneralLedgerStatus.Debit,
                                                Amount = totalAmountInLedgers - totalCurrencyAmountInLedger
                                            };
                                            debitAccountPayable = _generalLedgerJournalService.CreateObject(debitAccountPayable, _accountService);
                                            totalAmountInLedgers = totalCurrencyAmountInLedger;

                                        }
                                    else if (leaf.LegacyCode == Constant.AccountLegacyCode.AccountReceivable + exRate.CurrencyId)
                                        {
                                            GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                                            {
                                                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                                                SourceDocument = Constant.GeneralLedgerSource.Closing,
                                                SourceDocumentId = closing.Id,
                                                TransactionDate = (DateTime)closing.EndDatePeriod,
                                                Status = Constant.GeneralLedgerStatus.Debit,
                                                Amount = totalAmountInLedgers - totalCurrencyAmountInLedger
                                            };
                                            debitExchangeLoss = _generalLedgerJournalService.CreateObject(debitExchangeLoss, _accountService);

                                            GeneralLedgerJournal creditAccountReceiable = new GeneralLedgerJournal()
                                            {
                                                AccountId = leaf.Id,
                                                SourceDocument = Constant.GeneralLedgerSource.Closing,
                                                SourceDocumentId = closing.Id,
                                                TransactionDate = (DateTime)closing.EndDatePeriod,
                                                Status = Constant.GeneralLedgerStatus.Credit,
                                                Amount = totalAmountInLedgers - totalCurrencyAmountInLedger
                                            };
                                            creditAccountReceiable = _generalLedgerJournalService.CreateObject(creditAccountReceiable, _accountService);
                                            totalAmountInLedgers = totalCurrencyAmountInLedger;

                                        }
                                    else if (leaf.LegacyCode == Constant.AccountLegacyCode.CashBank + exRate.CurrencyId)
                                        {
                                            GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                                            {
                                                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                                                SourceDocument = Constant.GeneralLedgerSource.Closing,
                                                SourceDocumentId = closing.Id,
                                                TransactionDate = (DateTime)closing.EndDatePeriod,
                                                Status = Constant.GeneralLedgerStatus.Debit,
                                                Amount = totalAmountInLedgers - totalCurrencyAmountInLedger
                                            };
                                            debitExchangeLoss = _generalLedgerJournalService.CreateObject(debitExchangeLoss, _accountService);

                                            GeneralLedgerJournal creditCashBank = new GeneralLedgerJournal()
                                            {
                                                AccountId = leaf.Id,
                                                SourceDocument = Constant.GeneralLedgerSource.Closing,
                                                SourceDocumentId = closing.Id,
                                                TransactionDate = (DateTime)closing.EndDatePeriod,
                                                Status = Constant.GeneralLedgerStatus.Credit,
                                                Amount = totalAmountInLedgers - totalCurrencyAmountInLedger
                                            };
                                            creditCashBank = _generalLedgerJournalService.CreateObject(creditCashBank, _accountService);
                                            totalAmountInLedgers = totalCurrencyAmountInLedger;

                                        }
                                }
                                else
                                {
                                    if (leaf.LegacyCode == Constant.AccountLegacyCode.CashBank + exRate.CurrencyId)
                                    {
                                        GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                                        {
                                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                                            SourceDocumentId = closing.Id,
                                            TransactionDate = (DateTime)closing.EndDatePeriod,
                                            Status = Constant.GeneralLedgerStatus.Debit,
                                            Amount = totalAmountInLedgers - totalCurrencyAmountInLedger
                                        };
                                        debitExchangeLoss = _generalLedgerJournalService.CreateObject(debitExchangeLoss, _accountService);

                                        GeneralLedgerJournal creditCashBank = new GeneralLedgerJournal()
                                        {
                                            AccountId = leaf.Id,
                                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                                            SourceDocumentId = closing.Id,
                                            TransactionDate = (DateTime)closing.EndDatePeriod,
                                            Status = Constant.GeneralLedgerStatus.Credit,
                                            Amount = totalAmountInLedgers - totalCurrencyAmountInLedger
                                        };
                                        creditCashBank = _generalLedgerJournalService.CreateObject(creditCashBank, _accountService);
                                        totalAmountInLedgers = totalCurrencyAmountInLedger;
                                    }
                                }
                            }

                            else if (totalAmountInLedgers < totalCurrencyAmountInLedger)

                            {

                                if (closing.IsYear == true)
                                {
                                    if (leaf.LegacyCode == Constant.AccountLegacyCode.AccountPayable + exRate.CurrencyId)
                                    { 
                                        GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                                        {
                                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                                            SourceDocumentId = closing.Id,
                                            TransactionDate = (DateTime)closing.EndDatePeriod,
                                            Status = Constant.GeneralLedgerStatus.Debit,
                                            Amount = totalCurrencyAmountInLedger - totalAmountInLedgers 
                                        };
                                        debitExchangeLoss = _generalLedgerJournalService.CreateObject(debitExchangeLoss, _accountService);

                                        GeneralLedgerJournal creditAccountPayable = new GeneralLedgerJournal()
                                        {
                                            AccountId = leaf.Id,
                                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                                            SourceDocumentId = closing.Id,
                                            TransactionDate = (DateTime)closing.EndDatePeriod,
                                            Status = Constant.GeneralLedgerStatus.Credit,
                                            Amount = totalCurrencyAmountInLedger - totalAmountInLedgers 

                                        };
                                        creditAccountPayable = _generalLedgerJournalService.CreateObject(creditAccountPayable, _accountService);
                                        totalAmountInLedgers = totalCurrencyAmountInLedger;

                                    }
                                    else if (leaf.LegacyCode == Constant.AccountLegacyCode.AccountReceivable + exRate.CurrencyId)
                                    {
                                        GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                                        { 
                                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                                            SourceDocumentId = closing.Id,
                                            TransactionDate = (DateTime)closing.EndDatePeriod,
                                            Status = Constant.GeneralLedgerStatus.Credit,
                                            Amount = totalCurrencyAmountInLedger - totalAmountInLedgers 

                                        };
                                        creditExchangeGain = _generalLedgerJournalService.CreateObject(creditExchangeGain, _accountService);

                                        GeneralLedgerJournal debitAccountReceiable = new GeneralLedgerJournal()
                                        {
                                            AccountId = leaf.Id,
                                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                                            SourceDocumentId = closing.Id,
                                            TransactionDate = (DateTime)closing.EndDatePeriod,
                                            Status = Constant.GeneralLedgerStatus.Debit,
                                            Amount = totalCurrencyAmountInLedger - totalAmountInLedgers 

                                        };
                                        debitAccountReceiable = _generalLedgerJournalService.CreateObject(debitAccountReceiable, _accountService);
                                        totalAmountInLedgers = totalCurrencyAmountInLedger;

                                    }
                                    else if (leaf.LegacyCode == Constant.AccountLegacyCode.CashBank + exRate.CurrencyId)
                                    {
                                        GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                                        {
                                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                                            SourceDocumentId = closing.Id,
                                            TransactionDate = (DateTime)closing.EndDatePeriod,
                                            Status = Constant.GeneralLedgerStatus.Credit,
                                            Amount = totalCurrencyAmountInLedger - totalAmountInLedgers 

                                        };
                                        creditExchangeGain = _generalLedgerJournalService.CreateObject(creditExchangeGain, _accountService);

                                        GeneralLedgerJournal debitCashBank = new GeneralLedgerJournal()
                                        {
                                            AccountId = leaf.Id,
                                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                                            SourceDocumentId = closing.Id,
                                            TransactionDate = (DateTime)closing.EndDatePeriod,
                                            Status = Constant.GeneralLedgerStatus.Debit,
                                            Amount = totalCurrencyAmountInLedger - totalAmountInLedgers 

                                        };
                                        debitCashBank = _generalLedgerJournalService.CreateObject(debitCashBank, _accountService);
                                        totalAmountInLedgers = totalCurrencyAmountInLedger;

                                    }
                                }
                                else
                                {
                                    if (leaf.LegacyCode == Constant.AccountLegacyCode.CashBank + exRate.CurrencyId)
                                    {
                                        GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                                        {
                                            AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                                            SourceDocumentId = closing.Id,
                                            TransactionDate = (DateTime)closing.EndDatePeriod,
                                            Status = Constant.GeneralLedgerStatus.Credit,
                                            Amount = totalCurrencyAmountInLedger - totalAmountInLedgers

                                        };
                                        creditExchangeGain = _generalLedgerJournalService.CreateObject(creditExchangeGain, _accountService);

                                        GeneralLedgerJournal debitCashBank = new GeneralLedgerJournal()
                                        {
                                            AccountId = leaf.Id,
                                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                                            SourceDocumentId = closing.Id,
                                            TransactionDate = (DateTime)closing.EndDatePeriod,
                                            Status = Constant.GeneralLedgerStatus.Debit,
                                            Amount = totalCurrencyAmountInLedger - totalAmountInLedgers

                                        };
                                        debitCashBank = _generalLedgerJournalService.CreateObject(debitCashBank, _accountService);
                                        totalAmountInLedgers = totalCurrencyAmountInLedger;
                                        
                                    }
                                }
                            }
                    }
                   #endregion


                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(leaf.Id, closing.Id);
                    VCNonBaseCurrency vcNonBase = new VCNonBaseCurrency() { 
                        ValidCombId = validComb.Id,
                        Amount = totalAmountForVCnonBAse,
                    };
                    _vCNonBaseCurrencyService.CreateObject(vcNonBase,_accountService,this);
                    validComb.Amount = totalAmountInLedgers;
                    _validCombService.UpdateObject(validComb, _accountService, this);


                }

                foreach (var leaf in leafAccounts)
                {
                    if (leaf.LegacyCode == Constant.AccountLegacyCode.ExchangeGain || leaf.LegacyCode == Constant.AccountLegacyCode.ExchangeLoss)
                    {
                        DateTime EndDate = closing.EndDatePeriod.AddDays(1);
                        IList<GeneralLedgerJournal> ledgers = _generalLedgerJournalService.GetQueryable()
                                                              .Where(x => x.AccountId == leaf.Id &&
                                                                     x.TransactionDate >= closing.BeginningPeriod &&
                                                                     x.TransactionDate < EndDate)
                                                              .ToList();

                        decimal totalAmountInLedgers = 0;
                        decimal totalAmountLast = 0;
                        if (lastClosing != null)
                        {
                            totalAmountLast = _validCombService.GetQueryable().
                                Where(x => x.ClosingId == lastClosing.Id && x.AccountId == leaf.Id).FirstOrDefault().Amount;
                        }
                        foreach (var ledger in ledgers)
                        {
                            Account account = _accountService.GetObjectById(ledger.AccountId);
                            if ((ledger.Status == Constant.GeneralLedgerStatus.Debit &&
                                (account.Group == Constant.AccountGroup.Asset ||
                                 account.Group == Constant.AccountGroup.Expense)) ||
                               (ledger.Status == Constant.GeneralLedgerStatus.Credit &&
                                (account.Group == Constant.AccountGroup.Liability ||
                                 account.Group == Constant.AccountGroup.Equity ||
                                 account.Group == Constant.AccountGroup.Revenue)))
                            {
                                totalAmountInLedgers += ledger.Amount;
                            }
                            else
                            {
                                totalAmountInLedgers -= ledger.Amount;
                            }
                        }
                        totalAmountInLedgers += totalAmountLast;
                        ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(leaf.Id, closing.Id);
                        validComb.Amount = totalAmountInLedgers;
                        _validCombService.UpdateObject(validComb, _accountService, this);
                    }
                }

                var groupNodeAccounts = _accountService.GetQueryable().Where(x => !x.IsLeaf && !x.IsDeleted).OrderByDescending(x => x.Level).ToList();
                foreach (var groupNode in groupNodeAccounts)
                {
                    FillValidComb(groupNode, closing, _accountService, _validCombService);
                }

                _repository.CloseObject(closing);
            }
            return closing;
        }

        private void FillValidComb(Account nodeAccount, Closing closing, IAccountService _accountService, IValidCombService _validCombService)
        {
            ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(nodeAccount.Id, closing.Id);
            _validCombService.CalculateTotalAmount(validComb, _accountService, this);
        }

        public Closing OpenObject(Closing closing, IAccountService _accountService, IValidCombService _validCombService,IVCNonBaseCurrencyService _vCNonBaseCurrencyService)
        {
            if (_validator.ValidOpenObject(closing))
            {
                IList<Account> allAccounts = _accountService.GetAll();
                foreach (var account in allAccounts)
                {
                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(account.Id, closing.Id);
                    validComb.Amount = 0;
                    _validCombService.UpdateObject(validComb, _accountService, this);
                    _vCNonBaseCurrencyService.DeleteObject(validComb.Id);
                }


            }


            return _repository.OpenObject(closing);
        }

        public bool DeleteObject(int Id, IAccountService _accountService, IValidCombService _validCombService, IVCNonBaseCurrencyService _vCNonBaseCurrencyService)
        {
            Closing closing = GetObjectById(Id);
            if (_validator.ValidCloseObject(closing, this))
            {
                IList<Account> allAccounts = _accountService.GetAll();
                foreach (var account in allAccounts)
                {
                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(account.Id, Id);
                    _validCombService.DeleteObject(validComb.Id);
                    _vCNonBaseCurrencyService.DeleteObject(validComb.Id);
                }
                return _repository.DeleteObject(Id);
            }
            return false;
        }

        public bool IsDateClosed(DateTime DateToCheck)
        {
            DateTime dateEnd = DateToCheck.AddDays(1);
            var ClosedDates = _repository.FindAll(x => x.IsClosed && x.BeginningPeriod <= DateToCheck && x.EndDatePeriod > dateEnd).ToList();
            if (ClosedDates.Any())
            {
                return true;
            }
            return false;
        }
    }
}
