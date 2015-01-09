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
    public class SalesInvoiceMigrationService : ISalesInvoiceMigrationService
    {
        private ISalesInvoiceMigrationRepository _repository;

        public SalesInvoiceMigrationService(ISalesInvoiceMigrationRepository _salesInvoiceMigrationRepository)
        {
            _repository = _salesInvoiceMigrationRepository;
        }

        public IQueryable<SalesInvoiceMigration> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public SalesInvoiceMigration GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalesInvoiceMigration CreateObject(SalesInvoiceMigration salesInvoiceMigration, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, 
                                                     IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService, IReceivableService _receivableService)
        {
            salesInvoiceMigration.Errors = new Dictionary<String, String>();
            _repository.CreateObject(salesInvoiceMigration);
            _generalLedgerJournalService.CreateJournalForSalesInvoiceMigration(salesInvoiceMigration, _accountService, _currencyService, _gLNonBaseCurrencyService);
            Receivable receivable = _receivableService.CreateObject(salesInvoiceMigration.ContactId, Constant.ReceivableSource.SalesInvoiceMigration, salesInvoiceMigration.Id, salesInvoiceMigration.CurrencyId, salesInvoiceMigration.AmountReceivable, salesInvoiceMigration.Rate);
            Currency baseCurrency = _currencyService.GetQueryable().Where(x => x.IsBase).FirstOrDefault();
            if (salesInvoiceMigration.CurrencyId != baseCurrency.Id && salesInvoiceMigration.Tax > 0)
            {
                Receivable TaxReceivableForNonBaseCurrency = _receivableService.CreateObject(salesInvoiceMigration.ContactId, Constant.ReceivableSource.SalesInvoiceMigration, salesInvoiceMigration.Id, baseCurrency.Id, salesInvoiceMigration.Tax, 1);
            }
            return salesInvoiceMigration;
        }
    }
}