using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class SalesDownPaymentAllocationDetailService : ISalesDownPaymentAllocationDetailService
    {
        private ISalesDownPaymentAllocationDetailRepository _repository;
        private ISalesDownPaymentAllocationDetailValidator _validator;

        public SalesDownPaymentAllocationDetailService(ISalesDownPaymentAllocationDetailRepository _salesDownPaymentAllocationDetailRepository, ISalesDownPaymentAllocationDetailValidator _salesDownPaymentAllocationDetailValidator)
        {
            _repository = _salesDownPaymentAllocationDetailRepository;
            _validator = _salesDownPaymentAllocationDetailValidator;
        }

        public ISalesDownPaymentAllocationDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalesDownPaymentAllocationDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalesDownPaymentAllocationDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<SalesDownPaymentAllocationDetail> GetObjectsBySalesDownPaymentAllocationId(int salesDownPaymentAllocationId)
        {
            return _repository.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocationId);
        }

        public IList<SalesDownPaymentAllocationDetail> GetObjectsByReceivableId(int receivableId)
        {
            return _repository.GetObjectsByReceivableId(receivableId);
        }

        public SalesDownPaymentAllocationDetail GetObjectByReceiptVoucherDetailId(int receiptVoucherDetailId)
        {
            return _repository.GetObjectByReceiptVoucherDetailId(receiptVoucherDetailId);
        }

        public SalesDownPaymentAllocationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalesDownPaymentAllocationDetail CreateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                                ISalesDownPaymentService _salesDownPaymentService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                                                IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService)
        {
            salesDownPaymentAllocationDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(salesDownPaymentAllocationDetail, _salesDownPaymentAllocationService, this, _salesDownPaymentService, _receiptVoucherDetailService, _receivableService))
            {
                _repository.CreateObject(salesDownPaymentAllocationDetail);
                SalesDownPaymentAllocation salesDownPaymentAllocation = _salesDownPaymentAllocationService.GetObjectById(salesDownPaymentAllocationDetail.SalesDownPaymentAllocationId);
                SalesDownPayment salesDownPayment = _salesDownPaymentService.GetObjectById(salesDownPaymentAllocation.SalesDownPaymentId);
                ReceiptVoucherDetail receiptVoucherDetail = new ReceiptVoucherDetail()
                {
                    Amount = salesDownPaymentAllocationDetail.Amount,
                    ReceivableId = salesDownPaymentAllocationDetail.ReceivableId,
                    ReceiptVoucherId = salesDownPayment.ReceiptVoucherId
                };
                _receiptVoucherDetailService.CreateObject(receiptVoucherDetail, _receiptVoucherService, _cashBankService, _receivableService);
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail UpdateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                                ISalesDownPaymentService _salesDownPaymentService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                                                IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService)
        {
            return (_validator.ValidUpdateObject(salesDownPaymentAllocationDetail, _salesDownPaymentAllocationService, this, _salesDownPaymentService, _receiptVoucherDetailService, _receivableService) ?
                     _repository.UpdateObject(salesDownPaymentAllocationDetail) : salesDownPaymentAllocationDetail);
        }

        public SalesDownPaymentAllocationDetail SoftDeleteObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            return (_validator.ValidDeleteObject(salesDownPaymentAllocationDetail) ? _repository.SoftDeleteObject(salesDownPaymentAllocationDetail) : salesDownPaymentAllocationDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesDownPaymentAllocationDetail ConfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, DateTime ConfirmationDate,
                                                                 ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentService _salesDownPaymentService,
                                                                 IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService)
        {
            salesDownPaymentAllocationDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesDownPaymentAllocationDetail, _receiptVoucherDetailService, _receivableService))
            {
                SalesDownPaymentAllocation salesDownPaymentAllocation = _salesDownPaymentAllocationService.GetObjectById(salesDownPaymentAllocationDetail.SalesDownPaymentAllocationId);
                SalesDownPayment salesDownPayment = _salesDownPaymentService.GetObjectById(salesDownPaymentAllocation.SalesDownPaymentId);
                Receivable receivable = _receivableService.GetObjectById(salesDownPaymentAllocationDetail.ReceivableId);

                if (salesDownPayment.IsGBCH) { receivable.PendingClearanceAmount += salesDownPaymentAllocationDetail.Amount; }
                receivable.RemainingAmount -= salesDownPaymentAllocationDetail.Amount;
                if (receivable.RemainingAmount == 0 && receivable.PendingClearanceAmount == 0)
                {
                    receivable.IsCompleted = true;
                    receivable.CompletionDate = DateTime.Now;
                }
                _receivableService.UpdateObject(receivable);

                salesDownPaymentAllocationDetail = _repository.ConfirmObject(salesDownPaymentAllocationDetail);
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail UnconfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                                                   ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentService _salesDownPaymentService,
                                                                   IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService)
        {
            if (_validator.ValidUnconfirmObject(salesDownPaymentAllocationDetail, _receiptVoucherDetailService, _receivableService))
            {
                SalesDownPaymentAllocation salesDownPaymentAllocation = _salesDownPaymentAllocationService.GetObjectById(salesDownPaymentAllocationDetail.SalesDownPaymentAllocationId);
                SalesDownPayment salesDownPayment = _salesDownPaymentService.GetObjectById(salesDownPaymentAllocation.SalesDownPaymentId);
                Receivable receivable = _receivableService.GetObjectById(salesDownPaymentAllocationDetail.ReceivableId);

                if (salesDownPayment.IsGBCH) { receivable.PendingClearanceAmount -= salesDownPaymentAllocationDetail.Amount; }
                receivable.RemainingAmount += salesDownPaymentAllocationDetail.Amount;
                if (receivable.RemainingAmount != 0 || receivable.PendingClearanceAmount != 0)
                {
                    receivable.IsCompleted = false;
                    receivable.CompletionDate = null;
                }
                _receivableService.UpdateObject(receivable);

                salesDownPaymentAllocationDetail = _repository.UnconfirmObject(salesDownPaymentAllocationDetail);
            }
            return salesDownPaymentAllocationDetail;
        }
    }
}