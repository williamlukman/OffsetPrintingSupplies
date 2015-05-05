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
using Data.Context;

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

        public Closing CreateObject(Closing closing, IList<ExchangeRateClosing> exchangeRateClosing ,IAccountService _accountService, IValidCombService _validCombService, IValidCombIncomeStatementService _validCombIncomeStatementService, IExchangeRateClosingService _exchangeRateClosingService)
        {
            closing.Errors = new Dictionary<String, String>();
            // Create all ValidComb

            if (_validator.ValidCreateObject(closing, this))
            {
                closing = _repository.CreateObject(closing);
                IList<Account> allAccounts = _accountService.GetQueryable().Where(x => !x.IsDeleted).OrderBy(x => x.Code).ToList();
                foreach (var account in allAccounts)
                {
                    if (account.Group == Constant.AccountGroup.Revenue || account.Group == Constant.AccountGroup.Expense)
                    {
                        ValidCombIncomeStatement validCombIncomeStatement = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(account.Id, closing.Id);
                    }
                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(account.Id, closing.Id);
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
                                   IGeneralLedgerJournalService _generalLedgerJournalService, IValidCombService _validCombService, IValidCombIncomeStatementService _validCombIncomeStatementService,
                                   IGLNonBaseCurrencyService _gLNonBaseCurrencyService,IExchangeRateClosingService _exchangeRateClosingService,
                                   IVCNonBaseCurrencyService _vCNonBaseCurrencyService,ICashBankService _cashBankService,
                                   IClosingReportService _closingReportService,ICurrencyService _currencyService)
        {
            if (_validator.ValidCloseObject(closing, this))
            {
                // Get Last Closing
                var lastClosing = _repository.GetQueryable().Where(x => EntityFunctions.AddDays(EntityFunctions.TruncateTime(x.EndDatePeriod), 1) == closing.BeginningPeriod).FirstOrDefault();
                DateTime EndDate = closing.EndDatePeriod.AddDays(1);
                #region Count ValidComb for each leaf account
                IList<Account> leafAccounts = _accountService.GetLeafObjects();
                foreach(var leaf in leafAccounts)
                {
                    IList<GeneralLedgerJournal> ledgers = _generalLedgerJournalService.GetQueryable()
                                                          .Where(x => x.AccountId == leaf.Id && 
                                                                 x.TransactionDate >= closing.BeginningPeriod && 
                                                                 x.TransactionDate < EndDate)
                                                          .ToList();
                    decimal totalAmountInLedgers = 0;
                    decimal totalAmountLast = 0;
                    if (lastClosing != null)
                    {
                        ValidComb LastValidComb = _validCombService.GetQueryable().Include("Account").
                            Where(x => x.ClosingId == lastClosing.Id && x.AccountId == leaf.Id &&
                                  (x.Account.Group == Constant.AccountGroup.Asset ||
                                   x.Account.Group == Constant.AccountGroup.Liability ||
                                   x.Account.Group == Constant.AccountGroup.Equity ||
                                   x.Account.Group == Constant.AccountGroup.Liability ||
                                   x.Account.Group == Constant.AccountGroup.Expense)).FirstOrDefault();
                        totalAmountLast = LastValidComb == null ? 0 : LastValidComb.Amount;

                    }

                    int contactId;
                    using (var db = new OffsetPrintingSuppliesEntities())
                    {
                        foreach(var ledger in ledgers)
                        {
                            Account account = _accountService.GetObjectById(ledger.AccountId);
                            if ((ledger.Status == Constant.GeneralLedgerStatus.Debit &&
                                (account.Group == Constant.AccountGroup.Asset ||
                                 account.Group == Constant.AccountGroup.Expense))
                                 ||
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

                            contactId = (ledger.SourceDocument == Constant.GeneralLedgerSource.SalesInvoice ? db.SalesInvoices.Where(x => x.Id == ledger.SourceDocumentId).FirstOrDefault().DeliveryOrder.SalesOrder.ContactId :
                                                 ledger.SourceDocument == Constant.ReceivableSource.PurchaseDownPayment ? db.PurchaseDownPayments.Where(x => x.Id == ledger.SourceDocumentId).FirstOrDefault().ContactId :
                                                 ledger.SourceDocument == Constant.ReceivableSource.ReceiptRequest ? db.ReceiptRequests.Where(x => x.Id == ledger.SourceDocumentId).FirstOrDefault().ContactId :
                                                 ledger.SourceDocument == Constant.ReceivableSource.SalesDownPayment ? db.SalesDownPayments.Where(x => x.Id == ledger.SourceDocumentId).FirstOrDefault().ContactId :
                                                 ledger.SourceDocument == Constant.ReceivableSource.SalesInvoiceMigration ? db.SalesInvoiceMigrations.Where(x => x.Id == ledger.SourceDocumentId).FirstOrDefault().ContactId :
                                                 ledger.SourceDocument == Constant.GeneralLedgerSource.ReceiptVoucher ? db.ReceiptVouchers.Where(x => x.Id == ledger.SourceDocumentId).FirstOrDefault().ContactId :
                                                 ledger.SourceDocument == Constant.PayableSource.PaymentRequest ? db.PaymentRequests.Where(x => x.Id == ledger.SourceDocumentId).FirstOrDefault().ContactId :
                                                 ledger.SourceDocument == Constant.PayableSource.PurchaseDownPayment ? db.PurchaseDownPayments.Where(x => x.Id == ledger.SourceDocumentId).FirstOrDefault().ContactId :
                                                 ledger.SourceDocument == Constant.PayableSource.PurchaseInvoiceMigration ? db.PurchaseInvoiceMigrations.Where(x => x.Id == ledger.SourceDocumentId).FirstOrDefault().ContactId :
                                                 ledger.SourceDocument == Constant.PayableSource.PurchaseInvoice ? db.PurchaseInvoices.Where(x => x.Id == ledger.SourceDocumentId).FirstOrDefault().PurchaseReceival.PurchaseOrder.ContactId :
                                                 ledger.SourceDocument == Constant.GeneralLedgerSource.PaymentVoucher ? db.PaymentVouchers.Where(x => x.Id == ledger.SourceDocumentId).FirstOrDefault().ContactId :
                                                 0);
                            if (contactId != 0)
                            {
                                ClosingReport closingReport = _closingReportService.GetQueryable().Where(x => x.ContactId == contactId && x.ClosingId == closing.Id && x.AccountId == ledger.AccountId).FirstOrDefault();
                                if (closingReport == null)
                                {
                                    closingReport = new ClosingReport();
                                    closingReport.ClosingId = closing.Id;
                                    closingReport.AccountId = leaf.Id;
                                    closingReport.ContactId = contactId == 0 ? null : (int?)contactId;
                                    closingReport.AccountParentId = leaf.ParentId;
                                    closingReport.TransactionDate = ledger.TransactionDate;
                                    if ((ledger.Status == Constant.GeneralLedgerStatus.Debit &&
                                       (account.Group == Constant.AccountGroup.Asset ||
                                        account.Group == Constant.AccountGroup.Expense))
                                        ||
                                      (ledger.Status == Constant.GeneralLedgerStatus.Credit &&
                                       (account.Group == Constant.AccountGroup.Liability ||
                                        account.Group == Constant.AccountGroup.Equity ||
                                        account.Group == Constant.AccountGroup.Revenue)))
                                    {
                                        closingReport.AmountIDR += ledger.Amount;
                                    }
                                    else
                                    {
                                        closingReport.AmountIDR -= ledger.Amount;
                                    }
                                    closingReport.AmountCurrency = 0;
                                    closingReport.CurrencyId = _currencyService.GetQueryable().Where(x => x.IsBase == true).FirstOrDefault().Id;
                                    _closingReportService.CreateObject(closingReport, _accountService);
                                }
                                else
                                {
                                    if ((ledger.Status == Constant.GeneralLedgerStatus.Debit &&
                                        (account.Group == Constant.AccountGroup.Asset ||
                                         account.Group == Constant.AccountGroup.Expense))
                                         ||
                                       (ledger.Status == Constant.GeneralLedgerStatus.Credit &&
                                        (account.Group == Constant.AccountGroup.Liability ||
                                         account.Group == Constant.AccountGroup.Equity ||
                                         account.Group == Constant.AccountGroup.Revenue)))
                                    {
                                        closingReport.AmountIDR += ledger.Amount;
                                    }
                                    else
                                    {
                                        closingReport.AmountIDR += ledger.Amount;
                                    }
                                    if (lastClosing != null)
                                    {
                                        closingReport.AmountIDR += _closingReportService.GetQueryable().Where(x => x.ClosingId == lastClosing.Id && x.ContactId == contactId).FirstOrDefault().AmountIDR;
                                    }

                                    _closingReportService.UpdateObject(closingReport);
                                }
                            }
                         }
                    }
                    totalAmountInLedgers += totalAmountLast;
                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(leaf.Id, closing.Id);
                    validComb.Amount = totalAmountInLedgers;
                    ClosingReport closingReport2 = new ClosingReport();
                    closingReport2.ClosingId = closing.Id;
                    closingReport2.AccountId = leaf.Id;
                    closingReport2.ContactId = null;
                    closingReport2.AccountParentId = leaf.ParentId;
                    closingReport2.AmountIDR = totalAmountInLedgers;
                    closingReport2.TransactionDate = closing.EndDatePeriod;
                    closingReport2.AmountCurrency = 0;
                    closingReport2.CurrencyId = _currencyService.GetQueryable().Where(x => x.IsBase == true).FirstOrDefault().Id;
                    _closingReportService.CreateObject(closingReport2, _accountService);
                    _validCombService.UpdateObject(validComb, _accountService, this);
                }
                #endregion

                #region Count Exchange Gain and Loss for non base currency
                var ledgerNonBaseCurrency = (from model in _gLNonBaseCurrencyService.GetQueryable().Where(x =>
                                                           x.GeneralLedgerJournal.TransactionDate >= closing.BeginningPeriod &&
                                                           x.GeneralLedgerJournal.TransactionDate < EndDate)
                                             select model.GeneralLedgerJournalId).ToList();
                var nonBaseCurrencyAccountIds = _generalLedgerJournalService.GetQueryable().Where(x => ledgerNonBaseCurrency.Contains(x.Id)).GroupBy(x => new { x.AccountId })
                                                                                         .Select(x => x.Key.AccountId).ToList();

                foreach (var nonBaseCurrencyAccountId in nonBaseCurrencyAccountIds)
                {
                    Account nonBaseAccount = _accountService.GetObjectById(nonBaseCurrencyAccountId);
                    IList<GLNonBaseCurrency> glNonBaseJournals = _gLNonBaseCurrencyService.GetQueryable().Where(x => x.GeneralLedgerJournal.AccountId == nonBaseAccount.Id &&
                                                                                                                     x.GeneralLedgerJournal.TransactionDate >= closing.BeginningPeriod &&
                                                                                                                     x.GeneralLedgerJournal.TransactionDate < EndDate).ToList();
                    decimal totalCurrencyAmountInLedger = 0;
                    decimal totalAmountForVCnonBAse = 0;

                    if (nonBaseAccount.IsCashBankAccount)
                    {
                        #region Cash And Bank                     
                        int cashBankId = int.Parse(nonBaseAccount.LegacyCode.Substring(Constant.AccountLegacyCode.CashBank.Length));
                        CashBank cashBank = _cashBankService.GetQueryable().Where(x => x.Id == cashBankId).FirstOrDefault();
                        ExchangeRateClosing exRate = _exchangeRateClosingService.GetQueryable().Where(x => x.ClosingId == closing.Id && x.CurrencyId == cashBank.CurrencyId).FirstOrDefault();
                        if (cashBank != null && exRate != null)
                        {
                            if (lastClosing != null)
                            {
                                var lastValid = _vCNonBaseCurrencyService.GetQueryable()
                                    .Where(x => x.ValidComb.AccountId == nonBaseAccount.Id && x.ValidComb.ClosingId == lastClosing.Id)
                                    .FirstOrDefault();
                                if (lastValid != null)
                                {
                                    totalCurrencyAmountInLedger += lastValid.Amount;
                                    totalAmountForVCnonBAse += lastValid.Amount;
                                }
                            }

                            foreach(var glNonBaseJournal in glNonBaseJournals)
                            {
                                GeneralLedgerJournal glj = _generalLedgerJournalService.GetObjectById(glNonBaseJournal.GeneralLedgerJournalId);
                                if (glj.Status == Constant.GeneralLedgerStatus.Debit &&
                                    nonBaseAccount.Group == Constant.AccountGroup.Asset)
                                {
                                    totalCurrencyAmountInLedger += glNonBaseJournal.Amount;
                                    totalAmountForVCnonBAse += glNonBaseJournal.Amount;
                                }
                                else if (glj.Status == Constant.GeneralLedgerStatus.Credit &&
                                    nonBaseAccount.Group == Constant.AccountGroup.Asset)
                                {
                                    totalCurrencyAmountInLedger -= glNonBaseJournal.Amount;
                                    totalAmountForVCnonBAse -= glNonBaseJournal.Amount;
                                }
                                else
                                {
                                    var a = glj.Status;
                                    var b = nonBaseAccount.Group;
                                    var c = glNonBaseJournal.Amount;
                                }
                            }
                            totalCurrencyAmountInLedger = Math.Round(Math.Round(totalCurrencyAmountInLedger, 2) * exRate.Rate, 2);
                            ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(nonBaseAccount.Id, closing.Id);
                            VCNonBaseCurrency vcNonBaseCurrency = _vCNonBaseCurrencyService.CreateObject(new VCNonBaseCurrency() { ValidCombId = validComb.Id, Amount = totalAmountForVCnonBAse, CreatedAt = DateTime.Now }, _accountService, this);

                            #region Cash Bank Exchange Gain / Loss
                            if (validComb.Amount > totalCurrencyAmountInLedger)
                            {
                                #region Cash & Bank Loss
                                GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                                {
                                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                                    SourceDocument = Constant.GeneralLedgerSource.Closing,
                                    SourceDocumentId = closing.Id,
                                    TransactionDate = (DateTime)closing.EndDatePeriod,
                                    Status = Constant.GeneralLedgerStatus.Debit, //
                                    Amount = validComb.Amount - totalCurrencyAmountInLedger
                                };
                                debitExchangeLoss = _generalLedgerJournalService.CreateObject(debitExchangeLoss, _accountService);
                                //ValidComb vcExchangeLoss = _validCombService.FindOrCreateObjectByAccountAndClosing(debitExchangeLoss.AccountId, closing.Id);
                                //vcExchangeLoss.Amount += debitExchangeLoss.Amount;
                                //_validCombService.UpdateObject(vcExchangeLoss, _accountService, this);

                                GeneralLedgerJournal creditCashBank = new GeneralLedgerJournal()
                                {
                                    AccountId = nonBaseAccount.Id,
                                    SourceDocument = Constant.GeneralLedgerSource.Closing,
                                    SourceDocumentId = closing.Id,
                                    TransactionDate = (DateTime)closing.EndDatePeriod,
                                    Status = Constant.GeneralLedgerStatus.Credit, //
                                    Amount = validComb.Amount - totalCurrencyAmountInLedger
                                };
                                creditCashBank = _generalLedgerJournalService.CreateObject(creditCashBank, _accountService);
                                //ValidComb vcCashBank = _validCombService.FindOrCreateObjectByAccountAndClosing(creditCashBank.AccountId, closing.Id);
                                //vcCashBank.Amount -= creditCashBank.Amount;
                                //_validCombService.UpdateObject(vcCashBank, _accountService, this);
                                #endregion
                            }
                            else if (totalCurrencyAmountInLedger > validComb.Amount)
                            {
                                #region Cash & Bank Gain
                                GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                                {
                                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                                    SourceDocument = Constant.GeneralLedgerSource.Closing,
                                    SourceDocumentId = closing.Id,
                                    TransactionDate = (DateTime)closing.EndDatePeriod,
                                    Status = Constant.GeneralLedgerStatus.Credit, //
                                    Amount = totalCurrencyAmountInLedger - validComb.Amount
                                };
                                creditExchangeGain = _generalLedgerJournalService.CreateObject(creditExchangeGain, _accountService);
                                //ValidComb vcExchangeGain = _validCombService.FindOrCreateObjectByAccountAndClosing(creditExchangeGain.AccountId, closing.Id);
                                //vcExchangeGain.Amount -= creditExchangeGain.Amount;
                                //_validCombService.UpdateObject(vcExchangeGain, _accountService, this);

                                GeneralLedgerJournal debitCashBank = new GeneralLedgerJournal()
                                {
                                    AccountId = nonBaseAccount.Id,
                                    SourceDocument = Constant.GeneralLedgerSource.Closing,
                                    SourceDocumentId = closing.Id,
                                    TransactionDate = (DateTime)closing.EndDatePeriod,
                                    Status = Constant.GeneralLedgerStatus.Debit, //
                                    Amount = totalCurrencyAmountInLedger - validComb.Amount
                                };
                                debitCashBank = _generalLedgerJournalService.CreateObject(debitCashBank, _accountService);
                                //ValidComb vcCashBank = _validCombService.FindOrCreateObjectByAccountAndClosing(debitCashBank.AccountId, closing.Id);
                                //vcCashBank.Amount += debitCashBank.Amount;
                                //_validCombService.UpdateObject(vcCashBank, _accountService, this);
                                #endregion
                            }
                            else
                            {
                                // No Gain No Loss
                                var a = exRate.Rate;
                                var b = totalCurrencyAmountInLedger;
                                var c = validComb.Amount;
                            }
                            #endregion
                        }
                        else {
                            closing.Errors.Add("Generic", "Cash Bank " + nonBaseAccount.Name + " is not found");
                        }
                        #endregion
                    }
                    else if (nonBaseAccount.IsPayableReceivable && closing.IsYear)
                    {
                        #region Payable And Receivable
                        string accountBaseLegacyCode = (nonBaseAccount.LegacyCode.Contains(Constant.AccountLegacyCode.AccountPayable)) ? Constant.AccountLegacyCode.AccountPayable :
                                          (nonBaseAccount.LegacyCode.Contains(Constant.AccountLegacyCode.GBCHPayable)) ? Constant.AccountLegacyCode.GBCHPayable :
                                          (nonBaseAccount.LegacyCode.Contains(Constant.AccountLegacyCode.AccountReceivable)) ? Constant.AccountLegacyCode.AccountReceivable :
                                          (nonBaseAccount.LegacyCode.Contains(Constant.AccountLegacyCode.GBCHReceivable)) ? Constant.AccountLegacyCode.GBCHReceivable : "";
                        int currencyId = glNonBaseJournals.ElementAtOrDefault(0).CurrencyId;
                        ExchangeRateClosing exRate = _exchangeRateClosingService.GetQueryable().Where(x => x.ClosingId == closing.Id && x.CurrencyId == currencyId).FirstOrDefault();
                        
                        if (nonBaseAccount.LegacyCode.Equals(accountBaseLegacyCode + exRate.CurrencyId.ToString()))
                        {
                            if (lastClosing != null)
                            {
                                var lastValid = _vCNonBaseCurrencyService.GetQueryable()
                                    .Where(x => x.ValidComb.AccountId == nonBaseAccount.Id && x.ValidComb.ClosingId == lastClosing.Id)
                                    .FirstOrDefault();
                                if (lastValid != null)
                                {
                                    totalCurrencyAmountInLedger += lastValid.Amount;
                                    totalAmountForVCnonBAse += lastValid.Amount;
                                }
                            }

                            foreach(var glNonBaseJournal in glNonBaseJournals)
                            {
                                if ((glNonBaseJournal.GeneralLedgerJournal.Status == Constant.GeneralLedgerStatus.Debit &&
                                       (nonBaseAccount.Group == Constant.AccountGroup.Asset ||
                                        nonBaseAccount.Group == Constant.AccountGroup.Expense)) ||
                                    (glNonBaseJournal.GeneralLedgerJournal.Status == Constant.GeneralLedgerStatus.Credit &&
                                       (nonBaseAccount.Group == Constant.AccountGroup.Liability ||
                                        nonBaseAccount.Group == Constant.AccountGroup.Equity ||
                                        nonBaseAccount.Group == Constant.AccountGroup.Revenue)))
                                {
                                    totalCurrencyAmountInLedger += glNonBaseJournal.Amount;
                                    totalAmountForVCnonBAse += glNonBaseJournal.Amount;
                                }
                                else
                                {
                                    totalCurrencyAmountInLedger -= glNonBaseJournal.Amount;
                                    totalAmountForVCnonBAse -= glNonBaseJournal.Amount;
                                }
                            }
                            totalCurrencyAmountInLedger = Math.Round(Math.Round(totalCurrencyAmountInLedger, 2) * exRate.Rate, 2);
                            ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(nonBaseAccount.Id, closing.Id);
                            VCNonBaseCurrency vcNonBaseCurrency = _vCNonBaseCurrencyService.CreateObject(new VCNonBaseCurrency() { ValidCombId = validComb.Id, Amount = totalAmountForVCnonBAse, CreatedAt = DateTime.Now }, _accountService, this);

                            #region AP / AR Exchange Gain Loss
                            if (validComb.Amount > totalCurrencyAmountInLedger)
                            {
                                // AP Gain 
                                if (accountBaseLegacyCode.Equals(Constant.AccountLegacyCode.AccountPayable) ||
                                    accountBaseLegacyCode.Equals(Constant.AccountLegacyCode.GBCHPayable))
                                {
                                    #region AP Gain
                                    GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                                    { 
                                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                                        SourceDocument = Constant.GeneralLedgerSource.Closing,
                                        SourceDocumentId = closing.Id,
                                        TransactionDate = (DateTime)closing.EndDatePeriod,
                                        Status = Constant.GeneralLedgerStatus.Credit,
                                        Amount = validComb.Amount - totalCurrencyAmountInLedger
                                    };
                                    creditExchangeGain = _generalLedgerJournalService.CreateObject(creditExchangeGain, _accountService);

                                    GeneralLedgerJournal debitAccountPayable = new GeneralLedgerJournal()
                                    {
                                        AccountId = nonBaseAccount.Id,
                                        SourceDocument = Constant.GeneralLedgerSource.Closing,
                                        SourceDocumentId = closing.Id,
                                        TransactionDate = (DateTime)closing.EndDatePeriod,
                                        Status = Constant.GeneralLedgerStatus.Debit,
                                        Amount = validComb.Amount - totalCurrencyAmountInLedger
                                    };
                                    debitAccountPayable = _generalLedgerJournalService.CreateObject(debitAccountPayable, _accountService);
                                    #endregion
                                }
                                // AR Loss
                                else if (accountBaseLegacyCode.Equals(Constant.AccountLegacyCode.AccountReceivable) ||
                                         accountBaseLegacyCode.Equals(Constant.AccountLegacyCode.GBCHReceivable))
                                {
                                    #region AR Loss
                                    GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                                    {
                                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                                        SourceDocument = Constant.GeneralLedgerSource.Closing,
                                        SourceDocumentId = closing.Id,
                                        TransactionDate = (DateTime)closing.EndDatePeriod,
                                        Status = Constant.GeneralLedgerStatus.Debit,
                                        Amount = validComb.Amount - totalCurrencyAmountInLedger
                                    };
                                    debitExchangeLoss = _generalLedgerJournalService.CreateObject(debitExchangeLoss, _accountService);

                                    GeneralLedgerJournal creditAccountReceivable = new GeneralLedgerJournal()
                                    {
                                        AccountId = nonBaseAccount.Id,
                                        SourceDocument = Constant.GeneralLedgerSource.Closing,
                                        SourceDocumentId = closing.Id,
                                        TransactionDate = (DateTime)closing.EndDatePeriod,
                                        Status = Constant.GeneralLedgerStatus.Credit,
                                        Amount = validComb.Amount - totalCurrencyAmountInLedger
                                    };
                                    creditAccountReceivable = _generalLedgerJournalService.CreateObject(creditAccountReceivable, _accountService);
                                    #endregion
                                }
                                else
                                {
                                    // No Gain No Loss
                                    var a = nonBaseAccount.Name;
                                    var b = validComb.Amount;
                                    var c = totalCurrencyAmountInLedger;
                                }
                            }
                            else if (validComb.Amount < totalCurrencyAmountInLedger)
                            {
                                // AP Loss
                                if (accountBaseLegacyCode.Equals(Constant.AccountLegacyCode.AccountPayable) ||
                                    accountBaseLegacyCode.Equals(Constant.AccountLegacyCode.GBCHPayable))
                                {
                                    #region AP Loss
                                    GeneralLedgerJournal debitExchangeLoss = new GeneralLedgerJournal()
                                    { 
                                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeLoss).Id,
                                        SourceDocument = Constant.GeneralLedgerSource.Closing,
                                        SourceDocumentId = closing.Id,
                                        TransactionDate = (DateTime)closing.EndDatePeriod,
                                        Status = Constant.GeneralLedgerStatus.Debit,
                                        Amount = totalCurrencyAmountInLedger - validComb.Amount
                                    };
                                    debitExchangeLoss = _generalLedgerJournalService.CreateObject(debitExchangeLoss, _accountService);

                                    GeneralLedgerJournal creditAccountPayable = new GeneralLedgerJournal()
                                    {
                                        AccountId = nonBaseAccount.Id,
                                        SourceDocument = Constant.GeneralLedgerSource.Closing,
                                        SourceDocumentId = closing.Id,
                                        TransactionDate = (DateTime)closing.EndDatePeriod,
                                        Status = Constant.GeneralLedgerStatus.Credit,
                                        Amount = totalCurrencyAmountInLedger - validComb.Amount
                                    };
                                    creditAccountPayable = _generalLedgerJournalService.CreateObject(creditAccountPayable, _accountService);
                                    #endregion
                                }
                                // AR Gain
                                else if (accountBaseLegacyCode.Equals(Constant.AccountLegacyCode.AccountReceivable) ||
                                         accountBaseLegacyCode.Equals(Constant.AccountLegacyCode.GBCHReceivable))
                                {
                                    #region AR Gain
                                    GeneralLedgerJournal creditExchangeGain = new GeneralLedgerJournal()
                                    {
                                        AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ExchangeGain).Id,
                                        SourceDocument = Constant.GeneralLedgerSource.Closing,
                                        SourceDocumentId = closing.Id,
                                        TransactionDate = (DateTime)closing.EndDatePeriod,
                                        Status = Constant.GeneralLedgerStatus.Credit,
                                        Amount = totalCurrencyAmountInLedger - validComb.Amount
                                    };
                                    creditExchangeGain = _generalLedgerJournalService.CreateObject(creditExchangeGain, _accountService);

                                    GeneralLedgerJournal debitAccountReceivable = new GeneralLedgerJournal()
                                    {
                                        AccountId = nonBaseAccount.Id,
                                        SourceDocument = Constant.GeneralLedgerSource.Closing,
                                        SourceDocumentId = closing.Id,
                                        TransactionDate = (DateTime)closing.EndDatePeriod,
                                        Status = Constant.GeneralLedgerStatus.Debit,
                                        Amount = totalCurrencyAmountInLedger - validComb.Amount
                                    };
                                    debitAccountReceivable = _generalLedgerJournalService.CreateObject(debitAccountReceivable, _accountService);
                                    #endregion
                                }
                                else
                                {
                                    // No Gain No Loss
                                }
                            }
                            #endregion
                        }
                        else {
                            closing.Errors.Add("Generic", nonBaseAccount.Name + " is not recognized as payable / receivable. Legacy Code: " + nonBaseAccount.LegacyCode);
                        }
                        #endregion
                    }
                    else
                    {
                        #region Non Cash Bank / Non AP / Non AR / AP & non Closing Year / AR & non Closing Year
                        var a = nonBaseAccount.Name;
                        #endregion
                    }
                }
                #endregion

                #region Recount Valid Comb for nonbase currency
                IList<Account> changedAccounts = _accountService.GetQueryable().Where(x => x.LegacyCode == Constant.AccountLegacyCode.ExchangeGain || 
                                                                                           x.LegacyCode == Constant.AccountLegacyCode.ExchangeLoss ||
                                                                                           x.IsCashBankAccount ||
                                                                                           (x.IsPayableReceivable && closing.IsYear)).ToList();
                foreach(var changedAccount in changedAccounts)
                {
                    IList<GeneralLedgerJournal> ledgers = _generalLedgerJournalService.GetQueryable()
                                                            .Where(x => x.AccountId == changedAccount.Id &&
                                                                    x.TransactionDate >= closing.BeginningPeriod &&
                                                                    x.TransactionDate < EndDate &&
                                                                    x.SourceDocument == Constant.GeneralLedgerSource.Closing)
                                                            .ToList();

                    decimal totalAmountInLedgers = 0;
                    foreach (var ledger in ledgers)
                    {
                        if ((ledger.Status == Constant.GeneralLedgerStatus.Debit &&
                            (changedAccount.Group == Constant.AccountGroup.Asset ||
                                changedAccount.Group == Constant.AccountGroup.Expense)) ||
                            (ledger.Status == Constant.GeneralLedgerStatus.Credit &&
                            (changedAccount.Group == Constant.AccountGroup.Liability ||
                                changedAccount.Group == Constant.AccountGroup.Equity ||
                                changedAccount.Group == Constant.AccountGroup.Revenue)))
                        {
                            totalAmountInLedgers += ledger.Amount;
                        }
                        else
                        {
                            totalAmountInLedgers -= ledger.Amount;
                        }
                    }
                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(changedAccount.Id, closing.Id);
                    validComb.Amount += totalAmountInLedgers;
                    _validCombService.UpdateObject(validComb, _accountService, this);
                }
                #endregion

                #region ClosingEntries: Balancing Cents
                // Perbedaan Cent dianggap Biaya Pembulatan
                decimal diff = _generalLedgerJournalService.GetQueryable().Include("Account").Where(x => !x.IsDeleted && EntityFunctions.TruncateTime(x.TransactionDate) <= closing.EndDatePeriod && EntityFunctions.TruncateTime(x.TransactionDate) >= closing.BeginningPeriod).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;
                if (diff != 0 && Math.Abs(diff) < 1.0m)
                {
                    Account pembulatan = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.BiayaPembulatan);
                    GeneralLedgerJournal debitPembulatan = new GeneralLedgerJournal()
                    {
                        AccountId = pembulatan.Id,
                        SourceDocument = Constant.GeneralLedgerSource.Closing,
                        SourceDocumentId = closing.Id,
                        TransactionDate = closing.EndDatePeriod, //(DateTime)EndDate,
                        Status = diff < 0 ? Constant.GeneralLedgerStatus.Debit : Constant.GeneralLedgerStatus.Credit,
                        Amount = Math.Abs(diff)
                    };
                    debitPembulatan = _generalLedgerJournalService.CreateObject(debitPembulatan, _accountService);
                    ValidComb vcPembulatan = _validCombService.FindOrCreateObjectByAccountAndClosing(pembulatan.Id, closing.Id);
                    vcPembulatan.Amount -= diff;
                    _validCombService.UpdateObject(vcPembulatan, _accountService, this);
                }
                #endregion

                #region ClosingEntries: Net Earning
                IList<Account> IncomeStatementAccounts = _accountService.GetQueryable().Where(x => (x.Group == Constant.AccountGroup.Revenue || x.Group == Constant.AccountGroup.Expense) && x.IsLeaf && !x.IsDeleted).ToList();
                decimal creditNetEarning = 0;
                // Calculate Retained Earning = Revenue - Expense, and then contra all Revenues & Expenses to be 0
                foreach (var account in IncomeStatementAccounts)
                {
                    ValidComb vcClosingEntries = _validCombService.FindOrCreateObjectByAccountAndClosing(account.Id, closing.Id);
                    ValidCombIncomeStatement validCombIncomeStatement = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(account.Id, closing.Id);
                    if (account.Group == Constant.AccountGroup.Expense && vcClosingEntries.Amount != 0)
                    {
                        GeneralLedgerJournal journal = new GeneralLedgerJournal()
                        {
                            AccountId = account.Id,
                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                            SourceDocumentId = closing.Id,
                            TransactionDate = closing.EndDatePeriod, //(DateTime)EndDate,
                            Status = vcClosingEntries.Amount < 0 ? Constant.GeneralLedgerStatus.Debit : Constant.GeneralLedgerStatus.Credit,
                            Amount = Math.Round(Math.Abs(vcClosingEntries.Amount), 2)
                        };
                        journal = _generalLedgerJournalService.CreateObject(journal, _accountService);
                        creditNetEarning -= vcClosingEntries.Amount; //vcClosingEntries.Amount > 0 ? creditNetEarning - vcClosingEntries.Amount : creditNetEarning + vcClosingEntries.Amount;
                        //creditNetEarning -= journal.Amount * (journal.Status == Constant.GeneralLedgerStatus.Credit ? 1 : -1);
                        validCombIncomeStatement.Amount = vcClosingEntries.Amount;
                        _validCombIncomeStatementService.UpdateObject(validCombIncomeStatement, _accountService, this);
                        vcClosingEntries.Amount = 0;
                        _validCombService.UpdateObject(vcClosingEntries, _accountService, this);
                    }
                    else if (account.Group == Constant.AccountGroup.Revenue && vcClosingEntries.Amount != 0)
                    {
                        GeneralLedgerJournal journal = new GeneralLedgerJournal()
                        {
                            AccountId = account.Id,
                            SourceDocument = Constant.GeneralLedgerSource.Closing,
                            SourceDocumentId = closing.Id,
                            TransactionDate = closing.EndDatePeriod, //(DateTime)EndDate,
                            Status = vcClosingEntries.Amount < 0 ? Constant.GeneralLedgerStatus.Credit : Constant.GeneralLedgerStatus.Debit,
                            Amount = Math.Round(Math.Abs(vcClosingEntries.Amount), 2)
                        };
                        journal = _generalLedgerJournalService.CreateObject(journal, _accountService);
                        creditNetEarning += vcClosingEntries.Amount;
                        //creditNetEarning += journal.Amount * (journal.Status == Constant.GeneralLedgerStatus.Credit ? -1 : 1);
                        validCombIncomeStatement.Amount = vcClosingEntries.Amount;
                        _validCombIncomeStatementService.UpdateObject(validCombIncomeStatement, _accountService, this);
                        vcClosingEntries.Amount = 0;
                        _validCombService.UpdateObject(vcClosingEntries, _accountService, this);
                    }
                }

                Account netEarningAccount = _accountService.GetQueryable().Where(x => x.LegacyCode == Constant.AccountLegacyCode.NetEarning && !x.IsDeleted).FirstOrDefault();
                GeneralLedgerJournal netEarningJournal = new GeneralLedgerJournal()
                {
                    AccountId = netEarningAccount.Id,
                    SourceDocument = Constant.GeneralLedgerSource.Closing,
                    SourceDocumentId = closing.Id,
                    TransactionDate = closing.EndDatePeriod, //(DateTime)EndDate,
                    Status = creditNetEarning < 0 ? Constant.GeneralLedgerStatus.Debit : Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Round(Math.Abs(creditNetEarning), 2)
                };
                netEarningJournal = _generalLedgerJournalService.CreateObject(netEarningJournal, _accountService);
                ValidComb vcNetEarning = _validCombService.FindOrCreateObjectByAccountAndClosing(netEarningAccount.Id, closing.Id);
                vcNetEarning.Amount += creditNetEarning;
                _validCombService.UpdateObject(vcNetEarning, _accountService, this);
                #endregion

                #region Fill Valid Comb Non Leaves
                var groupNodeAccounts = _accountService.GetQueryable().Where(x => !x.IsLeaf && !x.IsDeleted).OrderByDescending(x => x.Level).ToList();
                foreach (var groupNode in groupNodeAccounts)
                {
                    FillValidComb(groupNode, closing, _accountService, _validCombService);
                }
                #endregion

                #region Fill Valid Comb Income Statement Non Leaves
                var groupNodeAccountIncomeStatement = _accountService.GetQueryable().Where(x => !x.IsLeaf && !x.IsDeleted && (x.Group == Constant.AccountGroup.Revenue || x.Group == Constant.AccountGroup.Expense)).OrderByDescending(x => x.Level).ToList();
                foreach (var groupNode in groupNodeAccountIncomeStatement)
                {
                    FillValidCombIncomeStatement(groupNode, closing, _accountService, _validCombIncomeStatementService);
                }
                #endregion

                _repository.CloseObject(closing);
            }
            return closing;
        }

        private void FillValidComb(Account nodeAccount, Closing closing, IAccountService _accountService, IValidCombService _validCombService)
        {
            ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(nodeAccount.Id, closing.Id);
            _validCombService.CalculateTotalAmount(validComb, _accountService, this);
        }

        private void FillValidCombIncomeStatement(Account nodeAccount, Closing closing, IAccountService _accountService, IValidCombIncomeStatementService _validCombIncomeStatementService)
        {
            ValidCombIncomeStatement validCombIncomeStatement = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(nodeAccount.Id, closing.Id);
            _validCombIncomeStatementService.CalculateTotalAmount(validCombIncomeStatement, _accountService, this);
        }

        public Closing OpenObject(Closing closing, IAccountService _accountService, IValidCombService _validCombService, IValidCombIncomeStatementService _validCombIncomeStatementService,
                                  IVCNonBaseCurrencyService _vCNonBaseCurrencyService,IGeneralLedgerJournalService _generalLedgerJournalService,
                                  IExchangeRateClosingService _exchangeRateClosingService,IClosingReportService _closingReportService)
        {
            if (_validator.ValidOpenObject(closing))
            {
                IList<Account> allAccounts = _accountService.GetAll();
                foreach (var account in allAccounts)
                {
                    if (account.Group == Constant.AccountGroup.Revenue || account.Group == Constant.AccountGroup.Expense)
                    {
                        ValidCombIncomeStatement validCombIncomeStatement = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(account.Id, closing.Id);
                        validCombIncomeStatement.Amount = 0;
                        _validCombIncomeStatementService.UpdateObject(validCombIncomeStatement, _accountService, this);
                    }
                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(account.Id, closing.Id);
                    validComb.Amount = 0;
                    _validCombService.UpdateObject(validComb, _accountService, this);
                    IList<VCNonBaseCurrency> VCNonBaseCurrencies = _vCNonBaseCurrencyService.GetQueryable().Where(x => x.ValidCombId == validComb.Id).ToList();
                    if (VCNonBaseCurrencies != null)
                    {
                        foreach (var VCNonBaseCurrency in VCNonBaseCurrencies)
                        {
                            _vCNonBaseCurrencyService.DeleteObject(VCNonBaseCurrency.Id);
                        }
                    }
                    IList<ClosingReport> ClosingReports = _closingReportService.GetQueryable().Where(x => x.ClosingId == validComb.ClosingId).ToList();
                    if (ClosingReports != null)
                    {
                        foreach (var ClosingReport in ClosingReports)
                        {
                            _closingReportService.DeleteObject(ClosingReport.Id);
                        }
                    }
                }

                var closingAccountIds = _generalLedgerJournalService.GetQueryable().Where(x => x.SourceDocument == Constant.GeneralLedgerSource.Closing && x.SourceDocumentId == closing.Id)
                                                    .GroupBy(x => new { x.AccountId }).Select(x => x.Key.AccountId).ToList();

                foreach (var closingAccount in closingAccountIds)
                {
                    Account acc = _accountService.GetObjectById(closingAccount);
                    List<GeneralLedgerJournal> GL = new List<GeneralLedgerJournal>();
                    GL = _generalLedgerJournalService.GetQueryable()
                        .Where(x => x.SourceDocument == Constant.GeneralLedgerSource.Closing
                            && x.SourceDocumentId == closing.Id && x.AccountId == acc.Id).ToList();

                    if (acc != null && !acc.IsDeleted)
                    {
                        decimal amountDebit = 0;
                        decimal amountCredit = 0;
                        foreach (var ledger in GL)
                        {
                            amountDebit += ledger.Status == Constant.GeneralLedgerStatus.Debit ? ledger.Amount : 0;
                            amountCredit += ledger.Status == Constant.GeneralLedgerStatus.Credit ? ledger.Amount : 0;
                        }

                        if (amountDebit != amountCredit)
                        {
                            GeneralLedgerJournal journal = new GeneralLedgerJournal()
                            {
                                AccountId = acc.Id,
                                SourceDocument = Constant.GeneralLedgerSource.Closing,
                                SourceDocumentId = closing.Id,
                                TransactionDate = (DateTime)closing.EndDatePeriod,
                                Status = amountDebit > amountCredit ? Constant.GeneralLedgerStatus.Credit : Constant.GeneralLedgerStatus.Debit,
                                Amount = amountDebit > amountCredit ? amountDebit - amountCredit : amountCredit - amountDebit
                            };
                            journal = _generalLedgerJournalService.CreateObject(journal, _accountService);
                        }
                    }
                }
            }
            return _repository.OpenObject(closing);
        }

        public Closing DeleteObject(Closing closing, IAccountService _accountService, IValidCombService _validCombService, IValidCombIncomeStatementService _validCombIncomeStatementService, IVCNonBaseCurrencyService _vCNonBaseCurrencyService, IGeneralLedgerJournalService _generalLedgerJournalService)
        {
            if (_validator.ValidDeleteObject(closing))
            {
                IList<Account> allAccounts = _accountService.GetAll();
                foreach (var account in allAccounts)
                {
                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(account.Id, closing.Id);
                    foreach (var vcnonbase in _vCNonBaseCurrencyService.GetQueryable().Where(x => x.ValidCombId == validComb.Id).ToList())
                    {
                        _vCNonBaseCurrencyService.DeleteObject(vcnonbase.Id);
                    }
                    ValidCombIncomeStatement validCombIncomeStatement = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(account.Id, closing.Id);
                    _validCombIncomeStatementService.DeleteObject(validCombIncomeStatement.Id);
                    _validCombService.DeleteObject(validComb.Id);
                }
                _repository.DeleteObject(closing.Id);

                IList<GeneralLedgerJournal> allClosingLedgers = _generalLedgerJournalService.GetQueryable().Where(x => x.SourceDocument == Constant.GeneralLedgerSource.Closing && x.SourceDocumentId == closing.Id).ToList();
                foreach (var closingLedger in allClosingLedgers)
                {
                    _generalLedgerJournalService.DeleteObject(closingLedger.Id);
                }
            }
            return closing;
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
