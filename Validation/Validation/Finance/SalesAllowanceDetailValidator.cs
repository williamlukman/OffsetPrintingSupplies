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
    public class SalesAllowanceDetailValidator : ISalesAllowanceDetailValidator
    {
        public SalesAllowanceDetail VHasSalesAllowance(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService)
        {
            SalesAllowance salesAllowance = _salesAllowanceService.GetObjectById(salesAllowanceDetail.SalesAllowanceId);
            if (salesAllowance == null)
            {
                salesAllowanceDetail.Errors.Add("SalesAllowanceId", "Tidak boleh tidak ada");
            }
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VHasReceivable(SalesAllowanceDetail salesAllowanceDetail, IReceivableService _receivableService)
        {
            Receivable receivable = _receivableService.GetObjectById(salesAllowanceDetail.ReceivableId);
            if (receivable == null)
            {
                salesAllowanceDetail.Errors.Add("ReceivableId", "Tidak boleh tidak ada");
            }
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VHasNotBeenConfirmed(SalesAllowanceDetail salesAllowanceDetail)
        {
            if (salesAllowanceDetail.IsConfirmed)
            {
                salesAllowanceDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VHasBeenConfirmed(SalesAllowanceDetail salesAllowanceDetail)
        {
            if (!salesAllowanceDetail.IsConfirmed)
            {
                salesAllowanceDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VHasNotBeenDeleted(SalesAllowanceDetail salesAllowanceDetail)
        {
            if (salesAllowanceDetail.IsDeleted)
            {
                salesAllowanceDetail.Errors.Add("Generic", "Sudah didelete");
            }
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VReceivableHasNotBeenCompleted(SalesAllowanceDetail salesAllowanceDetail, IReceivableService _receivableService)
        {
            Receivable receivable = _receivableService.GetObjectById(salesAllowanceDetail.ReceivableId);
            if (receivable.IsCompleted)
            {
                salesAllowanceDetail.Errors.Add("Generic", "Receivable sudah complete");
            }
            return salesAllowanceDetail;
        }
        
        public SalesAllowanceDetail VNonNegativeAmount(SalesAllowanceDetail salesAllowanceDetail)
        {
            if (salesAllowanceDetail.Amount < 0)
            {
                salesAllowanceDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VAmountLessOrEqualReceivable(SalesAllowanceDetail salesAllowanceDetail, IReceivableService _receivableService)
        {
            Receivable receivable = _receivableService.GetObjectById(salesAllowanceDetail.ReceivableId);
            if (salesAllowanceDetail.Amount > receivable.Amount)
            {
                salesAllowanceDetail.Errors.Add("Amount", "Tidak boleh lebih dari receivable");
            }
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VUniqueReceivableId(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                                    IReceivableService _receivableService)
        {
            IList<SalesAllowanceDetail> salesAllowanceDetails = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowanceDetail.SalesAllowanceId);
            Receivable receivable = _receivableService.GetObjectById(salesAllowanceDetail.ReceivableId);
            foreach (var detail in salesAllowanceDetails)
            {
                if (detail.ReceivableId == salesAllowanceDetail.ReceivableId && detail.Id != salesAllowanceDetail.Id)
                {
                    salesAllowanceDetail.Errors.Add("Generic", "ReceivableId harus unique dibandingkan receipt voucher details di dalam satu receipt voucher");
                    return salesAllowanceDetail;
                }
            }
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VDetailsAmountLessOrEqualSalesAllowanceTotal(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService,
                                                                                 ISalesAllowanceDetailService _salesAllowanceDetailService)
        {
            IList<SalesAllowanceDetail> salesAllowanceDetails = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowanceDetail.SalesAllowanceId);
            decimal TotalSalesAllowanceDetails = 0;
            foreach (var detail in salesAllowanceDetails)
            {
                TotalSalesAllowanceDetails += detail.Amount;
            }
            SalesAllowance salesAllowance = _salesAllowanceService.GetObjectById(salesAllowanceDetail.SalesAllowanceId);
            if (salesAllowance.TotalAmount < TotalSalesAllowanceDetails)
            {
                decimal sisa = salesAllowance.TotalAmount - TotalSalesAllowanceDetails + salesAllowanceDetail.Amount;
                salesAllowanceDetail.Errors.Add("Generic", "Receipt Voucher hanya menyediakan sisa dana sebesar " + sisa);
            }
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VCreateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService,
                                                  ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VHasSalesAllowance(salesAllowanceDetail, _salesAllowanceService);
            if (!isValid(salesAllowanceDetail)) { return salesAllowanceDetail; }
            VHasNotBeenConfirmed(salesAllowanceDetail);
            if (!isValid(salesAllowanceDetail)) { return salesAllowanceDetail; }
            VHasNotBeenDeleted(salesAllowanceDetail);
            if (!isValid(salesAllowanceDetail)) { return salesAllowanceDetail; }
            VHasReceivable(salesAllowanceDetail, _receivableService);
            if (!isValid(salesAllowanceDetail)) { return salesAllowanceDetail; }
            VReceivableHasNotBeenCompleted(salesAllowanceDetail, _receivableService);
            if (!isValid(salesAllowanceDetail)) { return salesAllowanceDetail; }
            VAmountLessOrEqualReceivable(salesAllowanceDetail, _receivableService);
            if (!isValid(salesAllowanceDetail)) { return salesAllowanceDetail; }
            VUniqueReceivableId(salesAllowanceDetail, _salesAllowanceDetailService, _receivableService);
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VUpdateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService,
                                                  ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VHasNotBeenConfirmed(salesAllowanceDetail);
            if (!isValid(salesAllowanceDetail)) { return salesAllowanceDetail; }
            VCreateObject(salesAllowanceDetail, _salesAllowanceService, _salesAllowanceDetailService, _cashBankService, _receivableService);
            return salesAllowanceDetail;    
        }

        public SalesAllowanceDetail VDeleteObject(SalesAllowanceDetail salesAllowanceDetail)
        {
            VHasNotBeenConfirmed(salesAllowanceDetail);
            if (!isValid(salesAllowanceDetail)) { return salesAllowanceDetail; }
            VHasNotBeenDeleted(salesAllowanceDetail);
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VHasConfirmationDate(SalesAllowanceDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public SalesAllowanceDetail VConfirmObject(SalesAllowanceDetail salesAllowanceDetail, IReceivableService _receivableService)
        {
            VHasConfirmationDate(salesAllowanceDetail);
            if (!isValid(salesAllowanceDetail)) { return salesAllowanceDetail; }
            VAmountLessOrEqualReceivable(salesAllowanceDetail, _receivableService);
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail VUnconfirmObject(SalesAllowanceDetail salesAllowanceDetail)
        {
            VHasBeenConfirmed(salesAllowanceDetail);
            return salesAllowanceDetail;
        }

        public bool ValidCreateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VCreateObject(salesAllowanceDetail, _salesAllowanceService, _salesAllowanceDetailService, _cashBankService, _receivableService);
            return isValid(salesAllowanceDetail);
        }

        public bool ValidUpdateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VUpdateObject(salesAllowanceDetail, _salesAllowanceService, _salesAllowanceDetailService, _cashBankService, _receivableService);
            return isValid(salesAllowanceDetail);
        }

        public bool ValidDeleteObject(SalesAllowanceDetail salesAllowanceDetail)
        {
            VDeleteObject(salesAllowanceDetail);
            return isValid(salesAllowanceDetail);
        }

        public bool ValidConfirmObject(SalesAllowanceDetail salesAllowanceDetail, IReceivableService _receivableService)
        {
            VConfirmObject(salesAllowanceDetail, _receivableService);
            return isValid(salesAllowanceDetail);
        }

        public bool ValidUnconfirmObject(SalesAllowanceDetail salesAllowanceDetail)
        {
            VUnconfirmObject(salesAllowanceDetail);
            return isValid(salesAllowanceDetail);
        }

        public bool isValid(SalesAllowanceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesAllowanceDetail obj)
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