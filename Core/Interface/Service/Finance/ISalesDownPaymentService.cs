using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesDownPaymentService
    {
        ISalesDownPaymentValidator GetValidator();
        IQueryable<SalesDownPayment> GetQueryable();
        IList<SalesDownPayment> GetAll();
        SalesDownPayment GetObjectById(int Id);
        IList<SalesDownPayment> GetObjectsByCashBankId(int cashBankId);
        IList<SalesDownPayment> GetObjectsByContactId(int contactId);
        SalesDownPayment CreateObject(SalesDownPayment salesDownPayment, IContactService _contactService,
                                      IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _payableService,
                                      ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService);
        SalesDownPayment UpdateObject(SalesDownPayment salesDownPayment, IContactService _contactService,
                                      IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _payableService,
                                      ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService);
        SalesDownPayment SoftDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                          IReceiptVoucherDetailService _receiptVoucherDetailService, IReceiptVoucherService _receiptVoucherService, IReceivableService _receivableService);
        bool DeleteObject(int Id);
        SalesDownPayment ConfirmObject(SalesDownPayment salesDownPayment, DateTime ConfirmationDate, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService,
                                       IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _payableService, IContactService _contactService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesDownPayment UnconfirmObject(SalesDownPayment salesDownPayment, ICashBankService _cashBankService,
                                         ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                         IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _payableService, IContactService _contactService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
    }
}