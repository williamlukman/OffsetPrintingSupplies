using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IReceiptVoucherDetailValidator
    {
        ReceiptVoucherDetail VHasReceiptVoucher(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService);
        ReceiptVoucherDetail VHasReceivable(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService);
        ReceiptVoucherDetail VReceivableContactIsTheSame(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceivableService _receivableService);
        ReceiptVoucherDetail VAmountLessThanCashBank(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService);
        ReceiptVoucherDetail VCorrectInstantClearance(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService);
        ReceiptVoucherDetail VUniqueCashBankId(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService);
        ReceiptVoucherDetail VNonNegativeOrZeroAmount(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail VIsConfirmed(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail VUpdateContactOrReceivable(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucherDetail VRemainingAmount(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService);

        ReceiptVoucherDetail VCreateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        ReceiptVoucherDetail VUpdateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        ReceiptVoucherDetail VDeleteObject(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail VConfirmObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        ReceiptVoucherDetail VUnconfirmObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        bool ValidCreateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        bool ValidUpdateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        bool ValidDeleteObject(ReceiptVoucherDetail receiptVoucherDetail);
        bool ValidConfirmObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        bool ValidUnconfirmObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        bool isValid(ReceiptVoucherDetail receiptVoucherDetail);
        string PrintError(ReceiptVoucherDetail receiptVoucherDetail);
    }
}
