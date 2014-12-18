using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesInvoiceMigrationService
    {
        IQueryable<SalesInvoiceMigration> GetQueryable();
        SalesInvoiceMigration GetObjectById(int Id);
        SalesInvoiceMigration CreateObject(SalesInvoiceMigration salesInvoice, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService,
                                                     IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService, IReceivableService _receivableService);
    }
}