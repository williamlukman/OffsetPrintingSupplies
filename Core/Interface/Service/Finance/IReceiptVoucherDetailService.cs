using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IReceiptVoucherDetailService
    {
        IReceiptVoucherDetailValidator GetValidator();
        IQueryable<ReceiptVoucherDetail> GetQueryable();
        IList<ReceiptVoucherDetail> GetAll();
        IList<ReceiptVoucherDetail> GetObjectsByReceiptVoucherId(int receiptVoucherId);
        IList<ReceiptVoucherDetail> GetObjectsByReceivableId(int receivableId);
        ReceiptVoucherDetail GetObjectById(int Id);        
        ReceiptVoucherDetail CreateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService,
                                          ICashBankService _cashBankService, IReceivableService _receivableService, ICurrencyService _currencyService);
        ReceiptVoucherDetail CreateObject(int receiptVoucherId, int receivableId, decimal amount, string description,
                                          IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService, IReceivableService _receivableService, ICurrencyService _currencyService);
        ReceiptVoucherDetail UpdateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService,
                                          ICashBankService _cashBankService, IReceivableService _receivableService,ICurrencyService _currencyService);
        ReceiptVoucherDetail SoftDeleteObject(ReceiptVoucherDetail receiptVoucherDetail,IReceiptVoucherService _receiptVoucherService);
        bool DeleteObject(int Id);
        ReceiptVoucherDetail ConfirmObject(ReceiptVoucherDetail receiptVoucherDetail, DateTime ConfirmationDate, IReceiptVoucherService _receiptVoucherService, IReceivableService _receivableService);
        ReceiptVoucherDetail UnconfirmObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceivableService _receivableService);
    }
}