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
    public class PurchaseInvoiceMigrationService : IPurchaseInvoiceMigrationService
    {
        private IPurchaseInvoiceMigrationRepository _repository;

        public PurchaseInvoiceMigrationService(IPurchaseInvoiceMigrationRepository _purchaseInvoiceMigrationRepository)
        {
            _repository = _purchaseInvoiceMigrationRepository;
        }

        public IQueryable<PurchaseInvoiceMigration> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public PurchaseInvoiceMigration GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PurchaseInvoiceMigration CreateObject(PurchaseInvoiceMigration purchaseInvoiceMigration, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, 
                                                     IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService, IPayableService _payableService)
        {
            purchaseInvoiceMigration.Errors = new Dictionary<String, String>();
            _repository.CreateObject(purchaseInvoiceMigration);

            _generalLedgerJournalService.CreateJournalForPurchaseInvoiceMigration(purchaseInvoiceMigration, _accountService, _currencyService, _gLNonBaseCurrencyService);
            Payable payable = _payableService.CreateObject(purchaseInvoiceMigration.ContactId, Constant.PayableSource.PurchaseInvoiceMigration, purchaseInvoiceMigration.Id, purchaseInvoiceMigration.CurrencyId, purchaseInvoiceMigration.AmountPayable, purchaseInvoiceMigration.Rate);
            return purchaseInvoiceMigration;
        }
    }
}