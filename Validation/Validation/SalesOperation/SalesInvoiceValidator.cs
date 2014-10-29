﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class SalesInvoiceValidator : ISalesInvoiceValidator
    {
        public SalesInvoice VHasUniqueNomorSurat(SalesInvoice salesInvoice, ISalesInvoiceService _salesInvoiceService)
        {
            IList<SalesInvoice> duplicates = _salesInvoiceService.GetQueryable().Where(x => x.NomorSurat == salesInvoice.NomorSurat && x.Id != salesInvoice.Id).ToList();
            if (duplicates.Any())
            {
                salesInvoice.Errors.Add("NomorSurat", "Tidak boleh merupakan duplikasi");
            }
            return salesInvoice;
        }

        public SalesInvoice VHasDeliveryOrder(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService)
        {
            DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(salesInvoice.DeliveryOrderId);
            if (deliveryOrder == null)
            {
                salesInvoice.Errors.Add("DeliveryOrderId", "Tidak terasosiasi dengan Sales Receival");
            }
            return salesInvoice;
        }

        public SalesInvoice VHasNoSalesInvoiceDetails(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            IList<SalesInvoiceDetail> details = _salesInvoiceDetailService.GetObjectsBySalesInvoiceId(salesInvoice.Id);
            if (details.Any())
            {
                salesInvoice.Errors.Add("Generic", "Tidak boleh memiliki Sales Invoice Details");
            }
            return salesInvoice;
        }

        public SalesInvoice VHasSalesInvoiceDetails(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            IList<SalesInvoiceDetail> details = _salesInvoiceDetailService.GetObjectsBySalesInvoiceId(salesInvoice.Id);
            if (!details.Any())
            {
                salesInvoice.Errors.Add("Generic", "Tidak memiliki Sales Invoice Details");
            }
            return salesInvoice;
        }

        public SalesInvoice VDeliveryOrderHasNotBeenInvoiceCompleted(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService)
        {
            DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(salesInvoice.DeliveryOrderId);
            if (deliveryOrder.IsInvoiceCompleted)
            {
                salesInvoice.Errors.Add("Generic", "Tidak boleh memilih Sales Receival dengan invoice yang sudah terbayar");
            }
            return salesInvoice;
        }

        public SalesInvoice VHasInvoiceDate(SalesInvoice salesInvoice)
        {
            if (salesInvoice.InvoiceDate == null)
            {
                salesInvoice.Errors.Add("InvoiceDate", "Tidak boleh kosong");
            }
            return salesInvoice;
        }

        public SalesInvoice VHasDueDate(SalesInvoice salesInvoice)
        {
            if (salesInvoice.DueDate == null)
            {
                salesInvoice.Errors.Add("DueDate", "Tidak boleh kosong");
            }
            return salesInvoice;
        }

        public SalesInvoice VHasDiscountBetweenZeroAndHundred(SalesInvoice salesInvoice)
        {
            if (salesInvoice.Discount < 0 || salesInvoice.Discount > 100)
            {
                salesInvoice.Errors.Add("Discount", "Harus antara 0 dan 100");
            }
            return salesInvoice;
        }

        public SalesInvoice VHasReceiptVoucherDetails(SalesInvoice salesInvoice, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            Receivable receivable = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice.Id);
            IList<ReceiptVoucherDetail> receiptVoucherDetails = _receiptVoucherDetailService.GetObjectsByReceivableId(receivable.Id);
            if (receiptVoucherDetails.Any())
            {
                salesInvoice.Errors.Add("Generic", "Tidak boleh sudah ada proses pembayaran");
            }
            return salesInvoice;
        }

        public SalesInvoice VHasBeenConfirmed(SalesInvoice salesInvoice)
        {
            if (!salesInvoice.IsConfirmed)
            {
                salesInvoice.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return salesInvoice;
        }

        public SalesInvoice VHasNotBeenConfirmed(SalesInvoice salesInvoice)
        {
            if (salesInvoice.IsConfirmed)
            {
                salesInvoice.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return salesInvoice;
        }

        public SalesInvoice VHasNotBeenDeleted(SalesInvoice salesInvoice)
        {
            if (salesInvoice.IsDeleted)
            {
                salesInvoice.Errors.Add("Generic", "Sudah dihapus");
            }
            return salesInvoice;
        }

        public SalesInvoice VAllSalesInvoiceDetailsAreConfirmable(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                                                  ISalesOrderDetailService _salesOrderDetailService, IServiceCostService _serviceCostService)
        {
            IList<SalesInvoiceDetail> details = _salesInvoiceDetailService.GetObjectsBySalesInvoiceId(salesInvoice.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                detail.ConfirmationDate = salesInvoice.ConfirmationDate;
                _salesInvoiceDetailService.GetValidator().VConfirmObject(detail, _salesInvoiceDetailService,
                                _deliveryOrderDetailService, _salesOrderDetailService, _serviceCostService);
                foreach (var error in detail.Errors)
                {
                    salesInvoice.Errors.Add("Generic", error.Value);
                }
                if (!isValid(salesInvoice)) { return salesInvoice; }
            }
            return salesInvoice;
        }

        public SalesInvoice VAllSalesInvoiceDetailsAreUnconfirmable(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService)
        {
            IList<SalesInvoiceDetail> details = _salesInvoiceDetailService.GetObjectsBySalesInvoiceId(salesInvoice.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                if (!_salesInvoiceDetailService.GetValidator().ValidUnconfirmObject(detail))
                {
                    foreach (var error in detail.Errors)
                    {
                        salesInvoice.Errors.Add("Generic", error.Value);
                    }
                    if (!isValid(salesInvoice)) { return salesInvoice; }
                }
            }
            return salesInvoice;
        }

        public SalesInvoice VReceivableHasNoOtherAssociation(SalesInvoice salesInvoice, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            Receivable receivable = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice.Id);
            IList<ReceiptVoucherDetail> receiptVoucherDetails = _receiptVoucherDetailService.GetObjectsByReceivableId(receivable.Id);
            if (receiptVoucherDetails.Any())
            {
                salesInvoice.Errors.Add("Generic", "Receivable memiliki asosiasi dengan receipt voucher detail");
                return salesInvoice;
            }
            return salesInvoice;
        }

        public SalesInvoice VGeneralLedgerPostingHasNotBeenClosed(SalesInvoice salesInvoice, IClosingService _closingService, int CaseConfirmUnconfirm)
        {
            switch (CaseConfirmUnconfirm)
            {
                case (1): // Confirm
                {
                    if (_closingService.IsDateClosed(salesInvoice.ConfirmationDate.GetValueOrDefault()))
                    {
                        salesInvoice.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
                case (2): // Unconfirm
                {
                    if (_closingService.IsDateClosed(DateTime.Now))
                    {
                        salesInvoice.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
            }
            return salesInvoice;
        }

        public SalesInvoice VCreateObject(SalesInvoice salesInvoice, ISalesInvoiceService _salesInvoiceService, IDeliveryOrderService _deliveryOrderService)
        {
            VHasUniqueNomorSurat(salesInvoice, _salesInvoiceService);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasDeliveryOrder(salesInvoice, _deliveryOrderService);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VDeliveryOrderHasNotBeenInvoiceCompleted(salesInvoice, _deliveryOrderService);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasInvoiceDate(salesInvoice);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasDueDate(salesInvoice);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasDiscountBetweenZeroAndHundred(salesInvoice);
            return salesInvoice;
        }

        public SalesInvoice VUpdateObject(SalesInvoice salesInvoice, ISalesInvoiceService _salesInvoiceService, IDeliveryOrderService _deliveryOrderService, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            VCreateObject(salesInvoice, _salesInvoiceService, _deliveryOrderService);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasNoSalesInvoiceDetails(salesInvoice, _salesInvoiceDetailService);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasNotBeenConfirmed(salesInvoice);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasNotBeenDeleted(salesInvoice);
            return salesInvoice;
        }

        public SalesInvoice VDeleteObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            VHasNotBeenConfirmed(salesInvoice);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasNotBeenDeleted(salesInvoice);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasNoSalesInvoiceDetails(salesInvoice, _salesInvoiceDetailService);
            return salesInvoice;
        }

        public SalesInvoice VHasConfirmationDate(SalesInvoice obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public SalesInvoice VConfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                           IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                           ISalesOrderDetailService _salesOrderDetailService, IServiceCostService _serviceCostService,
                                           IClosingService _closingService)
        {
            VHasConfirmationDate(salesInvoice);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VDeliveryOrderHasNotBeenInvoiceCompleted(salesInvoice, _deliveryOrderService);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasNotBeenDeleted(salesInvoice);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasNotBeenConfirmed(salesInvoice);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasSalesInvoiceDetails(salesInvoice, _salesInvoiceDetailService);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VAllSalesInvoiceDetailsAreConfirmable(salesInvoice, _salesInvoiceDetailService, _deliveryOrderDetailService,
                                                  _salesOrderDetailService, _serviceCostService);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VGeneralLedgerPostingHasNotBeenClosed(salesInvoice, _closingService, 1);
            return salesInvoice;
        }

        public SalesInvoice VUnconfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                             IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                             IClosingService _closingService)
        {
            VHasBeenConfirmed(salesInvoice);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VHasNotBeenDeleted(salesInvoice);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VAllSalesInvoiceDetailsAreUnconfirmable(salesInvoice, _salesInvoiceDetailService, _receiptVoucherDetailService, _receivableService);
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VReceivableHasNoOtherAssociation(salesInvoice, _receivableService, _receiptVoucherDetailService); // _salesAllowanceAllocationDetailService
            if (!isValid(salesInvoice)) { return salesInvoice; }
            VGeneralLedgerPostingHasNotBeenClosed(salesInvoice, _closingService, 2);
            return salesInvoice;
        }

        public bool ValidCreateObject(SalesInvoice salesInvoice, ISalesInvoiceService _salesInvoiceService, IDeliveryOrderService _deliveryOrderService)
        {
            VCreateObject(salesInvoice, _salesInvoiceService, _deliveryOrderService);
            return isValid(salesInvoice);
        }

        public bool ValidUpdateObject(SalesInvoice salesInvoice, ISalesInvoiceService _salesInvoiceService, IDeliveryOrderService _deliveryOrderService, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            salesInvoice.Errors.Clear();
            VUpdateObject(salesInvoice, _salesInvoiceService, _deliveryOrderService, _salesInvoiceDetailService);
            return isValid(salesInvoice);
        }

        public bool ValidDeleteObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            salesInvoice.Errors.Clear();
            VDeleteObject(salesInvoice, _salesInvoiceDetailService);
            return isValid(salesInvoice);
        }

        public bool ValidConfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                       IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                       ISalesOrderDetailService _salesOrderDetailService, IServiceCostService _serviceCostService,
                                       IClosingService _closingService)
        {
            salesInvoice.Errors.Clear();
            VConfirmObject(salesInvoice, _salesInvoiceDetailService, _deliveryOrderService, _deliveryOrderDetailService,
                           _salesOrderDetailService, _serviceCostService, _closingService);
            return isValid(salesInvoice);
        }

        public bool ValidUnconfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                         IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, IClosingService _closingService)
        {
            salesInvoice.Errors.Clear();
            VUnconfirmObject(salesInvoice, _salesInvoiceDetailService, _receiptVoucherDetailService, _receivableService, _closingService);
            return isValid(salesInvoice);
        }

        public bool isValid(SalesInvoice obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesInvoice obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}