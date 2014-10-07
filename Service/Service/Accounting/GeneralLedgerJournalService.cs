using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            #region Credit CashBank, Debit AccountPayable
            GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = (DateTime)paymentVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = paymentVoucher.TotalAmount
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);

            GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = (DateTime)paymentVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = paymentVoucher.TotalAmount
            };
            creditcashbank = CreateObject(creditcashbank, _accountService);

            journals.Add(debitaccountpayable);
            journals.Add(creditcashbank);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService)
        {
            #region Debit CashBank, Credit AccountPayable, Debit AccountPayable, Credit GoodsPendingClearance
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = paymentVoucher.TotalAmount
            };
            creditaccountpayable = CreateObject(creditaccountpayable, _accountService);

            GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = paymentVoucher.TotalAmount
            };
            debitcashbank = CreateObject(debitcashbank, _accountService);

            GeneralLedgerJournal creditGoodsPendingClearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = (DateTime)paymentVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = paymentVoucher.TotalAmount
            };
            creditGoodsPendingClearance = CreateObject(creditGoodsPendingClearance, _accountService);

            GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = (DateTime)paymentVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = paymentVoucher.TotalAmount
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);

            journals.Add(creditaccountpayable);
            journals.Add(debitcashbank);

            journals.Add(creditGoodsPendingClearance);
            journals.Add(debitaccountpayable);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService)
        {
            #region Debit CashBank, Credit AccountReceivable, Debit AccountReceivable, Credit Revenue
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = (DateTime)receiptVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = receiptVoucher.TotalAmount
            };
            debitcashbank = CreateObject(debitcashbank, _accountService);

            GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = (DateTime)receiptVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = receiptVoucher.TotalAmount
            };
            creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);

            GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = (DateTime) receiptVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = receiptVoucher.TotalAmount
            };
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            GeneralLedgerJournal creditrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = (DateTime) receiptVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = receiptVoucher.TotalAmount
            };
            creditrevenue = CreateObject(creditrevenue, _accountService);

            journals.Add(debitcashbank);
            journals.Add(creditaccountreceivable);
            journals.Add(debitaccountreceivable);
            journals.Add(creditrevenue);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService)
        {
            #region Credit CashBank, Debit AccountReceivable, Credit AccountReceivable, Debit Revenue
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = receiptVoucher.TotalAmount
            };
            creditcashbank = CreateObject(creditcashbank, _accountService);

            GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = receiptVoucher.TotalAmount
            };
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = (DateTime)receiptVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = receiptVoucher.TotalAmount
            };
            creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);

            GeneralLedgerJournal debitrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = (DateTime)receiptVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = receiptVoucher.TotalAmount
            };
            debitrevenue = CreateObject(debitrevenue, _accountService);

            journals.Add(creditcashbank);
            journals.Add(debitaccountreceivable);
            journals.Add(creditaccountreceivable);
            journals.Add(debitrevenue);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService)
        {
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
                    TransactionDate = (DateTime)cashBankAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = cashBankAdjustment.Amount
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                GeneralLedgerJournal creditcashbankequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = cashBankAdjustment.Amount
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
                    TransactionDate = (DateTime)cashBankAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Abs(cashBankAdjustment.Amount)
                };
                debitcashbankadjustmentexpense = CreateObject(debitcashbankadjustmentexpense, _accountService);

                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Abs(cashBankAdjustment.Amount)
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
            #region if (Amount >= 0) then Credit CashBank, Debit CashBankEquityAdjustment
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            if (cashBankAdjustment.Amount >= 0)
            {
                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = cashBankAdjustment.Amount
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                GeneralLedgerJournal debitcashbankequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = cashBankAdjustment.GetType().ToString(),
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = cashBankAdjustment.Amount
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
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Abs(cashBankAdjustment.Amount)
                };
                creditcashbankadjustmentexpense = CreateObject(creditcashbankadjustmentexpense, _accountService);

                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Abs(cashBankAdjustment.Amount)
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
            #region Debit TargetCashBank, Credit SourceCashBank
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debittargetcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + targetCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = (DateTime)cashBankMutation.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = cashBankMutation.Amount
            };
            debittargetcashbank = CreateObject(debittargetcashbank, _accountService);

            GeneralLedgerJournal creditsourcecashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + sourceCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = (DateTime)cashBankMutation.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = cashBankMutation.Amount
            };
            creditsourcecashbank = CreateObject(creditsourcecashbank, _accountService);

            journals.Add(debittargetcashbank);
            journals.Add(creditsourcecashbank);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService)
        {
            #region Debit SourceCashBank, Credit TargetCashBank
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal credittargetcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + targetCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = cashBankMutation.Amount
            };
            credittargetcashbank = CreateObject(credittargetcashbank, _accountService);

            GeneralLedgerJournal debitsourcecashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + sourceCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = cashBankMutation.Amount
            };
            debitsourcecashbank = CreateObject(debitsourcecashbank, _accountService);

            journals.Add(credittargetcashbank);
            journals.Add(debitsourcecashbank);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService)
        {
            #region if (stockAdjustmentTotal >= 0) then Debit Raw, Credit StockEquityAdjustment
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            if (stockAdjustment.Total >= 0)
            {
                GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = stockAdjustment.Total
                };
                debitraw = CreateObject(debitraw, _accountService);

                GeneralLedgerJournal creditstockequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.ConfirmationDate,
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
                    TransactionDate = (DateTime)stockAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Abs(stockAdjustment.Total)
                };
                debitstockadjustmentexpense = CreateObject(debitstockadjustmentexpense, _accountService);

                GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.ConfirmationDate,
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
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = stockAdjustment.Total
                };
                creditraw = CreateObject(creditraw, _accountService);

                GeneralLedgerJournal debitstockequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
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
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Abs(stockAdjustment.Total)
                };
                creditstockadjustmentexpense = CreateObject(creditstockadjustmentexpense, _accountService);

                GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
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

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForDeliveryOrder(DeliveryOrder deliveryOrder, IAccountService _accountService)
        {
            #region Debit COGS, Credit Raw
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitcogs = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGS).Id,
                SourceDocument = Constant.GeneralLedgerSource.DeliveryOrder,
                SourceDocumentId = deliveryOrder.Id,
                TransactionDate = (DateTime) deliveryOrder.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = deliveryOrder.TotalCOGS
            };
            debitcogs = CreateObject(debitcogs, _accountService);

            GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.DeliveryOrder,
                SourceDocumentId = deliveryOrder.Id,
                TransactionDate = (DateTime) deliveryOrder.ConfirmationDate,
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
            #region Credit COGS, Debit Raw
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditcogs = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGS).Id,
                SourceDocument = Constant.GeneralLedgerSource.DeliveryOrder,
                SourceDocumentId = deliveryOrder.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = deliveryOrder.TotalCOGS
            };
            creditcogs = CreateObject(creditcogs, _accountService);

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.DeliveryOrder,
                SourceDocumentId = deliveryOrder.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = deliveryOrder.TotalCOGS
            };
            debitraw = CreateObject(debitraw, _accountService);

            journals.Add(creditcogs);
            journals.Add(debitraw);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesInvoice(SalesInvoice salesInvoice, IAccountService _accountService)
        {
            #region Debit AccountReceivable, Debit Discount, Debit TaxExpense, Credit Revenue

            decimal Tax = salesInvoice.AmountReceivable * salesInvoice.Tax / (100 - salesInvoice.Tax);
            decimal PreTax = salesInvoice.AmountReceivable * 100 / (100 - salesInvoice.Tax);
            decimal Discount = PreTax * salesInvoice.Discount / (100 - salesInvoice.Discount);

            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = (DateTime)salesInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = salesInvoice.AmountReceivable
            };
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            GeneralLedgerJournal debitdiscount = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Discount).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = (DateTime)salesInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Discount
            };
            debitdiscount = CreateObject(debitdiscount, _accountService);

            GeneralLedgerJournal debittaxexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.TaxExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = (DateTime)salesInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = Tax
            };
            debittaxexpense = CreateObject(debittaxexpense, _accountService);

            GeneralLedgerJournal creditrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = (DateTime)salesInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = salesInvoice.AmountReceivable + Tax + Discount
            };
            creditrevenue = CreateObject(creditrevenue, _accountService);

            journals.Add(debitaccountreceivable);
            journals.Add(debitdiscount);
            journals.Add(debittaxexpense);
            journals.Add(creditrevenue);
            #endregion
            #region Debit COS, Credit FinishedGoods
            if (salesInvoice.TotalCOS > 0)
            {
                GeneralLedgerJournal debitCOS = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COS).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = (DateTime)salesInvoice.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = salesInvoice.TotalCOS
                };
                debitCOS = CreateObject(debitCOS, _accountService);

                GeneralLedgerJournal creditFinishedGoods = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = (DateTime)salesInvoice.ConfirmationDate,
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

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesInvoice(SalesInvoice salesInvoice, IAccountService _accountService)
        {
            #region Credit AccountReceivable, Credit Discount, Credit TaxExpense, Debit Revenue
            DateTime UnconfirmationDate = DateTime.Now;

            decimal Tax = salesInvoice.AmountReceivable * salesInvoice.Tax / (100 - salesInvoice.Tax);
            decimal PreTax = salesInvoice.AmountReceivable * 100 / (100 - salesInvoice.Tax);
            decimal Discount = PreTax * salesInvoice.Discount / (100 - salesInvoice.Discount);

            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = salesInvoice.AmountReceivable
            };
            creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);

            GeneralLedgerJournal creditdiscount = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Discount).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Discount
            };
            creditdiscount = CreateObject(creditdiscount, _accountService);

            GeneralLedgerJournal credittaxexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.TaxExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = Tax
            };
            credittaxexpense = CreateObject(credittaxexpense, _accountService);

            GeneralLedgerJournal debitrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                SourceDocumentId = salesInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = salesInvoice.AmountReceivable + Tax + Discount
            };
            debitrevenue = CreateObject(debitrevenue, _accountService);

            journals.Add(creditaccountreceivable);
            journals.Add(creditdiscount);
            journals.Add(credittaxexpense);
            journals.Add(debitrevenue);
            #endregion
            #region Credit COS, Debit FinishedGoods
            if (salesInvoice.TotalCOS > 0)
            {
                GeneralLedgerJournal creditCOS = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COS).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = salesInvoice.TotalCOS
                };
                creditCOS = CreateObject(creditCOS, _accountService);

                GeneralLedgerJournal debitFinishedGoods = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                    SourceDocument = Constant.GeneralLedgerSource.SalesInvoice,
                    SourceDocumentId = salesInvoice.Id,
                    TransactionDate = UnconfirmationDate,
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

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseReceival(PurchaseReceival purchaseReceival, IAccountService _accountService)
        {
            #region Debit Raw, Credit GoodsPendingClearance
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = (DateTime)purchaseReceival.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseReceival.TotalCOGS
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditgoodsPendingclearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = (DateTime)purchaseReceival.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseReceival.TotalCOGS
            };
            creditgoodsPendingclearance = CreateObject(creditgoodsPendingclearance, _accountService);

            journals.Add(debitraw);
            journals.Add(creditgoodsPendingclearance);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseReceival(PurchaseReceival purchaseReceival, IAccountService _accountService)
        {
            #region Credit Raw, Debit GoodsPendingClearance
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            DateTime UnconfirmationDate = DateTime.Now;
            GeneralLedgerJournal creditraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseReceival.TotalCOGS
            };
            creditraw = CreateObject(creditraw, _accountService);

            GeneralLedgerJournal debitgoodsPendingclearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseReceival,
                SourceDocumentId = purchaseReceival.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseReceival.TotalCOGS
            };
            debitgoodsPendingclearance = CreateObject(debitgoodsPendingclearance, _accountService);

            journals.Add(creditraw);
            journals.Add(debitgoodsPendingclearance);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseInvoice(PurchaseInvoice purchaseInvoice, IAccountService _accountService)
        {
            #region Debit GoodsPendingClearance, Credit AccountPayable
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitGoodsPendingClearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = (DateTime)purchaseInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseInvoice.AmountPayable
            };
            debitGoodsPendingClearance = CreateObject(debitGoodsPendingClearance, _accountService);

            GeneralLedgerJournal creditaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = (DateTime)purchaseInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseInvoice.AmountPayable
            };
            creditaccountpayable = CreateObject(creditaccountpayable, _accountService);

            journals.Add(debitGoodsPendingClearance);
            journals.Add(creditaccountpayable);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseInvoice(PurchaseInvoice purchaseInvoice, IAccountService _accountService)
        {
            #region Credit GoodsPendingClearance, Debit AccountPayable
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditGoodsPendingClearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = purchaseInvoice.AmountPayable
            };
            creditGoodsPendingClearance = CreateObject(creditGoodsPendingClearance, _accountService);

            GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.PurchaseInvoice,
                SourceDocumentId = purchaseInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = purchaseInvoice.AmountPayable
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);

            journals.Add(creditGoodsPendingClearance);
            journals.Add(debitaccountpayable);

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateFinishedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService)
        {
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
            #region Debit Raw (Core, Compound, Accessories), Credit FinishedGoods (Roller)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnfinishedDate = DateTime.Now;

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = UnfinishedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = recoveryOrderDetail.TotalCost
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = UnfinishedDate,
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
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.RecoveryExpense).Id,
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
            #region Debit Raw (Core, Compound, Accessories), Credit RecoveryExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UndoRejectDate = DateTime.Now;

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = UndoRejectDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = recoveryOrderDetail.TotalCost
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditrecoveryexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.RecoveryExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.RecoveryOrderDetail,
                SourceDocumentId = recoveryOrderDetail.Id,
                TransactionDate = UndoRejectDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = recoveryOrderDetail.TotalCost
            };
            creditrecoveryexpense = CreateObject(creditrecoveryexpense, _accountService);

            journals.Add(debitraw);
            journals.Add(creditrecoveryexpense);
            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateFinishedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService)
        {
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
            #region Debit Raw (RollBlanket, Bars, Adhesive), Credit FinishedGoods (Blanket)
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnfinishedDate = DateTime.Now;

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = UnfinishedDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = blanketOrderDetail.TotalCost
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditfinishedgoods = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.FinishedGoods).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = UnfinishedDate,
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
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ConversionExpense).Id,
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
            #region Debit Raw (RollBlanket, Bars, Adhesive), Credit ConversionExpense
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UndoRejectDate = DateTime.Now;

            GeneralLedgerJournal debitraw = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Raw).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = UndoRejectDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = blanketOrderDetail.TotalCost
            };
            debitraw = CreateObject(debitraw, _accountService);

            GeneralLedgerJournal creditconversionexpense = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.ConversionExpense).Id,
                SourceDocument = Constant.GeneralLedgerSource.BlanketOrderDetail,
                SourceDocumentId = blanketOrderDetail.Id,
                TransactionDate = UndoRejectDate,
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
