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
    public class PayableService : IPayableService
    {
        private IPayableRepository _repository;
        private IPayableValidator _validator;

        public PayableService(IPayableRepository _payableRepository, IPayableValidator _payableValidator)
        {
            _repository = _payableRepository;
            _validator = _payableValidator;
        }

        public IPayableValidator GetValidator()
        {
            return _validator;
        }

        public IList<Payable> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Payable> GetObjectsByCustomerId(int customerId)
        {
            return _repository.GetObjectsByCustomerId(customerId);
        }

        public Payable GetObjectBySource(string PayableSource, int PayableSourceId)
        {
            return _repository.GetObjectBySource(PayableSource, PayableSourceId);
        }

        public Payable GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Payable CreateObject(Payable payable)
        {
            payable.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(payable, this) ? _repository.CreateObject(payable) : payable);
        }

        public Payable CreateObject(int customerId, string payableSource, int payableSourceId, decimal amount, DateTime dueDate)
        {
            Payable payable = new Payable
            {
                CustomerId = customerId,
                PayableSource = payableSource,
                PayableSourceId = payableSourceId,
                Amount = amount,
                RemainingAmount = amount,
                DueDate = dueDate
            };
            return this.CreateObject(payable);
        }

        public Payable UpdateObject(Payable payable)
        {
            return (_validator.ValidUpdateObject(payable, this) ? _repository.UpdateObject(payable) : payable);
        }

        public Payable SoftDeleteObject(Payable payable)
        {
            return (_validator.ValidDeleteObject(payable) ? _repository.SoftDeleteObject(payable) : payable);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}