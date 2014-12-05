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
    public class VCNonBaseCurrencyService : IVCNonBaseCurrencyService
    {
        private IVCNonBaseCurrencyRepository _repository;
        private IVCNonBaseCurrencyValidator _validator;

        public VCNonBaseCurrencyService(IVCNonBaseCurrencyRepository _validCombRepository, IVCNonBaseCurrencyValidator _validCombValidator)
        {
            _repository = _validCombRepository;
            _validator = _validCombValidator;
        }

        public IVCNonBaseCurrencyValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<VCNonBaseCurrency> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<VCNonBaseCurrency> GetAll()
        {
            return _repository.GetAll();
        }

        public VCNonBaseCurrency GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public VCNonBaseCurrency CreateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService)
        {
            validComb.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(validComb, _accountService, _closingService) ? _repository.CreateObject(validComb) : validComb);
        }

        public VCNonBaseCurrency UpdateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService)
        {
            return (_validator.ValidUpdateObject(validComb, _accountService, _closingService) ? _repository.UpdateObject(validComb) : validComb);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

    }
}
