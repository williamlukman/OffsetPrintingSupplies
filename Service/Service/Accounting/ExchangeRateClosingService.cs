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
    public class ExchangeRateClosingService : IExchangeRateClosingService
    {
        private IExchangeRateClosingRepository _repository;
        private IExchangeRateClosingValidator _validator;

        public ExchangeRateClosingService(IExchangeRateClosingRepository _exchangeRateExchangeRateClosingRepository, IExchangeRateClosingValidator _exchangeRateExchangeRateClosingValidator)
        {
            _repository = _exchangeRateExchangeRateClosingRepository;
            _validator = _exchangeRateExchangeRateClosingValidator;
        }

        public IExchangeRateClosingValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ExchangeRateClosing> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ExchangeRateClosing> GetAll()
        {
            return _repository.GetAll();
        }

        public ExchangeRateClosing GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }


        public ExchangeRateClosing CreateObject(ExchangeRateClosing exchangeRateExchangeRateClosing)
        {
            exchangeRateExchangeRateClosing.Errors = new Dictionary<String, String>();
            // Create all ValidComb
             
           _repository.CreateObject(exchangeRateExchangeRateClosing);
            return exchangeRateExchangeRateClosing;
        }


        public bool DeleteObject(int Id, IAccountService _accountService, IValidCombService _validCombService)
        {
            ExchangeRateClosing exchangeRateExchangeRateClosing = GetObjectById(Id);
            return _repository.DeleteObject(Id);
        }

    }
}
