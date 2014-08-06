using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IReceiptVoucherValidator
    {
        ReceiptVoucher VHasContact(ReceiptVoucher receiptVoucher, ICustomerService _customerService);
        ReceiptVoucher VHasCashBank(ReceiptVoucher receiptVoucher, ICashBankService _cashBankService);
        ReceiptVoucher VHasReceiptDate(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasReceiptVoucherDetails(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VRemainingCashBankAmount(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService);
        ReceiptVoucher VRemainingAmountDetails(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService);
        ReceiptVoucher VUnconfirmableDetails(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                            ICashBankService _cashBankService, IReceivableService _receivableService);
        ReceiptVoucher VUpdateContact(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VClearanceDate(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasBeenConfirmed(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasNotBeenConfirmed(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VIsBank(ReceiptVoucher receiptVoucher, ICashBankService _cashBankService);
        ReceiptVoucher VHasBeenReconciled(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasNotBeenReconciled(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VCreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, ICustomerService _customerService, ICashBankService _cashBankService);
        ReceiptVoucher VUpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, ICustomerService _customerService, ICashBankService _cashBankService);
        ReceiptVoucher VDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VConfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        ReceiptVoucher VUnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        ReceiptVoucher VReconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        ReceiptVoucher VUnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        bool ValidCreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, ICustomerService _customerService, ICashBankService _cashBankService);
        bool ValidUpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, ICustomerService _customerService, ICashBankService _cashBankService);
        bool ValidDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool ValidConfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        bool ValidUnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        bool ValidReconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        bool ValidUnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        bool isValid(ReceiptVoucher receiptVoucher);
        string PrintError(ReceiptVoucher receiptVoucher);
    }
}
