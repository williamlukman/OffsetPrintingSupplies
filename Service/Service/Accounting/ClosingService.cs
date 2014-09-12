using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;

namespace Service.Service
{
    public class ClosingService : IClosingService
    {
        private IClosingRepository _repository;
        private IClosingValidator _validator;

        public ClosingService(IClosingRepository _closingRepository, IClosingValidator _closingValidator)
        {
            _repository = _closingRepository;
            _validator = _closingValidator;
        }

        public IClosingValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Closing> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Closing> GetAll()
        {
            return _repository.GetAll();
        }

        public Closing GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Closing CreateObject(Closing closing)
        {
            closing.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(closing) ? _repository.CreateObject(closing) : closing);
        }

        public Closing CloseObject(Closing closing)
        {
            closing.Errors = new Dictionary<String, String>();
            return (_validator.ValidCloseObject(closing) ? _repository.CloseObject(closing) : closing);
        }

        /*public Closing SoftDeleteObject(Closing closing)
        {
            return (_validator.ValidDeleteObject(closing) ? _repository.SoftDeleteObject(closing) : closing);
        }*/

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}
