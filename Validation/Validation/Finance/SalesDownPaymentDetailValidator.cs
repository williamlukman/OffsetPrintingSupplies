using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class SalesDownPaymentDetailValidator : ISalesDownPaymentDetailValidator
    {
        public SalesDownPaymentDetail VHasSalesDownPayment(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService)
        {
            SalesDownPayment salesDownPayment = _salesDownPaymentService.GetObjectById(salesDownPaymentDetail.SalesDownPaymentId);
            if (salesDownPayment == null)
            {
                salesDownPaymentDetail.Errors.Add("SalesDownPaymentId", "Tidak boleh tidak ada");
            }
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VHasReceivable(SalesDownPaymentDetail salesDownPaymentDetail, IReceivableService _receivableService)
        {
            Receivable receivable = _receivableService.GetObjectById(salesDownPaymentDetail.ReceivableId);
            if (receivable == null)
            {
                salesDownPaymentDetail.Errors.Add("ReceivableId", "Tidak boleh tidak ada");
            }
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VHasNotBeenConfirmed(SalesDownPaymentDetail salesDownPaymentDetail)
        {
            if (salesDownPaymentDetail.IsConfirmed)
            {
                salesDownPaymentDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VHasBeenConfirmed(SalesDownPaymentDetail salesDownPaymentDetail)
        {
            if (!salesDownPaymentDetail.IsConfirmed)
            {
                salesDownPaymentDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VHasNotBeenDeleted(SalesDownPaymentDetail salesDownPaymentDetail)
        {
            if (salesDownPaymentDetail.IsDeleted)
            {
                salesDownPaymentDetail.Errors.Add("Generic", "Sudah didelete");
            }
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VReceivableHasNotBeenCompleted(SalesDownPaymentDetail salesDownPaymentDetail, IReceivableService _receivableService)
        {
            Receivable receivable = _receivableService.GetObjectById(salesDownPaymentDetail.ReceivableId);
            if (receivable.IsCompleted)
            {
                salesDownPaymentDetail.Errors.Add("Generic", "Receivable sudah complete");
            }
            return salesDownPaymentDetail;
        }
        
        public SalesDownPaymentDetail VNonNegativeAmount(SalesDownPaymentDetail salesDownPaymentDetail)
        {
            if (salesDownPaymentDetail.Amount < 0)
            {
                salesDownPaymentDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VAmountLessOrEqualReceivable(SalesDownPaymentDetail salesDownPaymentDetail, IReceivableService _receivableService)
        {
            Receivable receivable = _receivableService.GetObjectById(salesDownPaymentDetail.ReceivableId);
            if (salesDownPaymentDetail.Amount > receivable.Amount)
            {
                salesDownPaymentDetail.Errors.Add("Amount", "Tidak boleh lebih dari receivable");
            }
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VUniqueReceivableId(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                                    IReceivableService _receivableService)
        {
            IList<SalesDownPaymentDetail> salesDownPaymentDetails = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPaymentDetail.SalesDownPaymentId);
            Receivable receivable = _receivableService.GetObjectById(salesDownPaymentDetail.ReceivableId);
            foreach (var detail in salesDownPaymentDetails)
            {
                if (detail.ReceivableId == salesDownPaymentDetail.ReceivableId && detail.Id != salesDownPaymentDetail.Id)
                {
                    salesDownPaymentDetail.Errors.Add("Generic", "ReceivableId harus unique dibandingkan receipt voucher details di dalam satu receipt voucher");
                    return salesDownPaymentDetail;
                }
            }
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VDetailsAmountLessOrEqualSalesDownPaymentTotal(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService,
                                                                                 ISalesDownPaymentDetailService _salesDownPaymentDetailService)
        {
            IList<SalesDownPaymentDetail> salesDownPaymentDetails = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPaymentDetail.SalesDownPaymentId);
            decimal TotalSalesDownPaymentDetails = 0;
            foreach (var detail in salesDownPaymentDetails)
            {
                TotalSalesDownPaymentDetails += detail.Amount;
            }
            SalesDownPayment salesDownPayment = _salesDownPaymentService.GetObjectById(salesDownPaymentDetail.SalesDownPaymentId);
            if (salesDownPayment.TotalAmount < TotalSalesDownPaymentDetails)
            {
                decimal sisa = salesDownPayment.TotalAmount - TotalSalesDownPaymentDetails + salesDownPaymentDetail.Amount;
                salesDownPaymentDetail.Errors.Add("Generic", "Receipt Voucher hanya menyediakan sisa dana sebesar " + sisa);
            }
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VCreateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService,
                                                  ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VHasSalesDownPayment(salesDownPaymentDetail, _salesDownPaymentService);
            if (!isValid(salesDownPaymentDetail)) { return salesDownPaymentDetail; }
            VHasNotBeenConfirmed(salesDownPaymentDetail);
            if (!isValid(salesDownPaymentDetail)) { return salesDownPaymentDetail; }
            VHasNotBeenDeleted(salesDownPaymentDetail);
            if (!isValid(salesDownPaymentDetail)) { return salesDownPaymentDetail; }
            VHasReceivable(salesDownPaymentDetail, _receivableService);
            if (!isValid(salesDownPaymentDetail)) { return salesDownPaymentDetail; }
            VReceivableHasNotBeenCompleted(salesDownPaymentDetail, _receivableService);
            if (!isValid(salesDownPaymentDetail)) { return salesDownPaymentDetail; }
            VAmountLessOrEqualReceivable(salesDownPaymentDetail, _receivableService);
            if (!isValid(salesDownPaymentDetail)) { return salesDownPaymentDetail; }
            VUniqueReceivableId(salesDownPaymentDetail, _salesDownPaymentDetailService, _receivableService);
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VUpdateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService,
                                                  ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VHasNotBeenConfirmed(salesDownPaymentDetail);
            if (!isValid(salesDownPaymentDetail)) { return salesDownPaymentDetail; }
            VCreateObject(salesDownPaymentDetail, _salesDownPaymentService, _salesDownPaymentDetailService, _cashBankService, _receivableService);
            return salesDownPaymentDetail;    
        }

        public SalesDownPaymentDetail VDeleteObject(SalesDownPaymentDetail salesDownPaymentDetail)
        {
            VHasNotBeenConfirmed(salesDownPaymentDetail);
            if (!isValid(salesDownPaymentDetail)) { return salesDownPaymentDetail; }
            VHasNotBeenDeleted(salesDownPaymentDetail);
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VHasConfirmationDate(SalesDownPaymentDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public SalesDownPaymentDetail VConfirmObject(SalesDownPaymentDetail salesDownPaymentDetail, IReceivableService _receivableService)
        {
            VHasConfirmationDate(salesDownPaymentDetail);
            if (!isValid(salesDownPaymentDetail)) { return salesDownPaymentDetail; }
            VAmountLessOrEqualReceivable(salesDownPaymentDetail, _receivableService);
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail VUnconfirmObject(SalesDownPaymentDetail salesDownPaymentDetail)
        {
            VHasBeenConfirmed(salesDownPaymentDetail);
            return salesDownPaymentDetail;
        }

        public bool ValidCreateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VCreateObject(salesDownPaymentDetail, _salesDownPaymentService, _salesDownPaymentDetailService, _cashBankService, _receivableService);
            return isValid(salesDownPaymentDetail);
        }

        public bool ValidUpdateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VUpdateObject(salesDownPaymentDetail, _salesDownPaymentService, _salesDownPaymentDetailService, _cashBankService, _receivableService);
            return isValid(salesDownPaymentDetail);
        }

        public bool ValidDeleteObject(SalesDownPaymentDetail salesDownPaymentDetail)
        {
            VDeleteObject(salesDownPaymentDetail);
            return isValid(salesDownPaymentDetail);
        }

        public bool ValidConfirmObject(SalesDownPaymentDetail salesDownPaymentDetail, IReceivableService _receivableService)
        {
            VConfirmObject(salesDownPaymentDetail, _receivableService);
            return isValid(salesDownPaymentDetail);
        }

        public bool ValidUnconfirmObject(SalesDownPaymentDetail salesDownPaymentDetail)
        {
            VUnconfirmObject(salesDownPaymentDetail);
            return isValid(salesDownPaymentDetail);
        }

        public bool isValid(SalesDownPaymentDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesDownPaymentDetail obj)
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