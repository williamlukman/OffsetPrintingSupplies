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
    public class ReceivableService : IReceivableService
    {
        private IReceivableRepository _repository;
        private IReceivableValidator _validator;

        public ReceivableService(IReceivableRepository _receivableRepository, IReceivableValidator _receivableValidator)
        {
            _repository = _receivableRepository;
            _validator = _receivableValidator;
        }

        public IReceivableValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Receivable> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Receivable> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Receivable> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public Receivable GetObjectBySource(string ReceivableSource, int ReceivableSourceId)
        {
            return _repository.GetObjectBySource(ReceivableSource, ReceivableSourceId);
        }

        public Receivable GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Receivable CreateObject(Receivable receivable)
        {
            receivable.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(receivable, this) ? _repository.CreateObject(receivable) : receivable);
        }

        public Receivable CreateObject(int contactId, string receivableSource, int receivableSourceId, int currencyId, decimal amount, decimal Rate,DateTime dueDate)
        {
            Receivable receivable = new Receivable
            {
                ContactId = contactId,
                ReceivableSource = receivableSource,
                ReceivableSourceId = receivableSourceId,
                CurrencyId = currencyId,
                Amount = amount,
                RemainingAmount = amount,
                DueDate = dueDate,
                Rate = Rate
            };
            return this.CreateObject(receivable);
        }

        public Receivable CreateObject(int contactId, string receivableSource, int receivableSourceId, int currencyId, decimal amount, decimal Rate)
        {
            Receivable receivable = new Receivable
            {
                ContactId = contactId,
                ReceivableSource = receivableSource,
                ReceivableSourceId = receivableSourceId,
                CurrencyId = currencyId,
                Amount = amount,
                RemainingAmount = amount,
                Rate = Rate
            };
            return this.CreateObject(receivable);
        }

        public Receivable UpdateObject(Receivable receivable)
        {
            return (_validator.ValidUpdateObject(receivable, this) ? _repository.UpdateObject(receivable) : receivable);
        }

        public Receivable SoftDeleteObject(Receivable receivable)
        {
            return (_validator.ValidDeleteObject(receivable) ? _repository.SoftDeleteObject(receivable) : receivable);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}