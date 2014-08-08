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
        ReceiptVoucher VHasContact(ReceiptVoucher receiptVoucher, IContactService _contactService);
        ReceiptVoucher VHasCashBank(ReceiptVoucher receiptVoucher, ICashBankService _cashBankService);
        ReceiptVoucher VHasReceiptDate(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VIfGBCHThenIsBank(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VIfGBCHThenHasDueDate(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasNoReceiptVoucherDetail(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VHasReceiptVoucherDetails(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VTotalAmountIsNotZero(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VHasNotBeenDeleted(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasBeenConfirmed(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasNotBeenConfirmed(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VAllReceiptVoucherDetailsAreConfirmable(ReceiptVoucher receiptVoucher, IReceiptVoucherService _paymetnVoucherService,
                                                               IReceiptVoucherDetailService receiptVoucherDetailService, ICashBankService _cashBankService,
                                                               IReceivableService _receivableService);
        ReceiptVoucher VCashBankHasMoreAmountReceiptVoucherDetails(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                                                   ICashBankService _cashBankService);
        ReceiptVoucher VHasBeenReconciled(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasNotBeenReconciled(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasReconciliationDate(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VCreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                     IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        ReceiptVoucher VUpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                     IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        ReceiptVoucher VDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VHasConfirmationDate(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VConfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService,
                                       IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                       IReceivableService _receivableService);
        ReceiptVoucher VUnconfirmObject(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VReconcileObject(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VUnreconcileObject(ReceiptVoucher receiptVoucher);
        bool ValidCreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                               IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidUpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                               IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool ValidConfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService,
                                IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                IReceivableService _receivableService);
        bool ValidUnconfirmObject(ReceiptVoucher receiptVoucher);
        bool ValidReconcileObject(ReceiptVoucher receiptVoucher);
        bool ValidUnreconcileObject(ReceiptVoucher receiptVoucher);
        bool isValid(ReceiptVoucher receiptVoucher);
        string PrintError(ReceiptVoucher receiptVoucher);
    }
}
