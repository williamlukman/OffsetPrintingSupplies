using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseInvoiceMigrationService
    {
        IQueryable<PurchaseInvoiceMigration> GetQueryable();
        PurchaseInvoiceMigration GetObjectById(int Id);
        PurchaseInvoiceMigration CreateObject(PurchaseInvoiceMigration purchaseInvoice, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, 
                                              IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService, IPayableService _payableService);
    }
}