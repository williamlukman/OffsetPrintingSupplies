using Core.Constants;
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

        public SalesDownPaymentAllocationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalesDownPaymentAllocationDetail CreateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                                             ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                             ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService,
                                                             IPayableService _payableService)
        {
            salesDownPaymentAllocationDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(salesDownPaymentAllocationDetail, _salesDownPaymentAllocationService, this, _salesDownPaymentService, _receivableService, _payableService))
            {
                _repository.CreateObject(salesDownPaymentAllocationDetail);
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail UpdateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                             ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IPayableService _payableService)
        {
            if (_validator.ValidUpdateObject(salesDownPaymentAllocationDetail, _salesDownPaymentAllocationService, this, _salesDownPaymentService, _receivableService, _payableService))
            {   
                _repository.UpdateObject(salesDownPaymentAllocationDetail);
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail SoftDeleteObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            if (_validator.ValidDeleteObject(salesDownPaymentAllocationDetail))
            {
                _repository.SoftDeleteObject(salesDownPaymentAllocationDetail);
            }
            return salesDownPaymentAllocationDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesDownPaymentAllocationDetail ConfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, DateTime ConfirmationDate,
                                                ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentService _salesDownPaymentService,
                                                IReceivableService _receivableService, IPayableService _payableService)
        {
            salesDownPaymentAllocationDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesDownPaymentAllocationDetail, _receivableService, _payableService))
            {
                SalesDownPaymentAllocation salesDownPaymentAllocation = _salesDownPaymentAllocationService.GetObjectById(salesDownPaymentAllocationDetail.SalesDownPaymentAllocationId);
                
                Payable payable = _payableService.GetObjectById(salesDownPaymentAllocation.PayableId);
                payable.RemainingAmount -= salesDownPaymentAllocationDetail.Amount;
                _payableService.UpdateObject(payable);

                Receivable receivable = _receivableService.GetObjectById(salesDownPaymentAllocationDetail.ReceivableId);
                receivable.RemainingAmount -= salesDownPaymentAllocationDetail.Amount;
                _receivableService.UpdateObject(receivable);

                salesDownPaymentAllocationDetail = _repository.ConfirmObject(salesDownPaymentAllocationDetail);
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail UnconfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                                ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentService _salesDownPaymentService,
                                                IReceivableService _receivableService, IPayableService _payableService)
        {
            if (_validator.ValidUnconfirmObject(salesDownPaymentAllocationDetail, _receivableService, _payableService))
            {
                SalesDownPaymentAllocation salesDownPaymentAllocation = _salesDownPaymentAllocationService.GetObjectById(salesDownPaymentAllocationDetail.SalesDownPaymentAllocationId);

                Payable payable = _payableService.GetObjectById(salesDownPaymentAllocation.PayableId);
                payable.RemainingAmount += salesDownPaymentAllocationDetail.Amount;
                _payableService.UpdateObject(payable);

                Receivable receivable = _receivableService.GetObjectById(salesDownPaymentAllocationDetail.ReceivableId);
                receivable.RemainingAmount += salesDownPaymentAllocationDetail.Amount;
                _receivableService.UpdateObject(receivable);

                salesDownPaymentAllocationDetail = _repository.UnconfirmObject(salesDownPaymentAllocationDetail);
            }
            return salesDownPaymentAllocationDetail;
        }
    }
}