using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesInvoiceService
    {
        ISalesInvoiceValidator GetValidator();
        IQueryable<SalesInvoice> GetQueryable();
        IList<SalesInvoice> GetAll();
        SalesInvoice GetObjectById(int Id);
        IList<SalesInvoice> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        SalesInvoice CreateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService);
        SalesInvoice UpdateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService, ISalesInvoiceDetailService _salesInvoiceDetailService);
        SalesInvoice SoftDeleteObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService);
        bool DeleteObject(int Id);
        SalesInvoice ConfirmObject(SalesInvoice salesInvoice, DateTime ConfirmationDate, ISalesInvoiceDetailService _salesInvoiceDetailService, ISalesOrderService _salesOrderService,
                                   ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                   IReceivableService _receivableService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                   IServiceCostService _serviceCostService, IRollerBuilderService _rollerBuilderService, IItemService _itemService, IItemTypeService _itemTypeService, 
                                   IContactService _contactService, IExchangeRateService _exchangeRateService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        SalesInvoice UnconfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderService _deliveryOrderService,
                                     IDeliveryOrderDetailService _deliveryOrderDetailService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                     IReceivableService _receivableService, ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, IContactService _contactService,
                                     IItemService _itemService, IItemTypeService _itemTypeService, IRollerBuilderService _rollerBuilderService, IServiceCostService _serviceCostService,
                                     IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                     IExchangeRateService _exchangeRateService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        SalesInvoice CalculateAmountReceivable(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService);
    }
}