using Core.DomainModel;
using Core.Constants;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;

namespace Service.Service
{
    public class ExchangeRateService : IExchangeRateService
    {
        private IExchangeRateRepository _repository;
        private IExchangeRateValidator _validator;
        public ExchangeRateService(IExchangeRateRepository _exchangeRateRepository, IExchangeRateValidator _exchangeRateValidator)
        {
            _repository = _exchangeRateRepository;
            _validator = _exchangeRateValidator;
        }

        public IExchangeRateValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ExchangeRate> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ExchangeRate> GetAll()
        {
            return _repository.GetAll();
        }

        public ExchangeRate GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ExchangeRate CreateObject(ExchangeRate exchangeRate)
        {
            exchangeRate.Errors = new Dictionary<string, string>();
            if (_validator.ValidCreateObject(exchangeRate, this))
            {
                ExchangeRate newExchangeRate = new ExchangeRate()
                {
                    ExRateDate = exchangeRate.ExRateDate,
                    CurrencyId = exchangeRate.CurrencyId,
                    Rate = exchangeRate.Rate,
                };
                _repository.CreateObject(exchangeRate);
            }
            return exchangeRate;
        }

        public ExchangeRate UpdateObject(ExchangeRate exchangeRate)
        {
            return (exchangeRate = _validator.ValidUpdateObject(exchangeRate, this) ? _repository.UpdateObject(exchangeRate) : exchangeRate);
        }

        public ExchangeRate SoftDeleteObject(ExchangeRate exchangeRate)
        {
            return (exchangeRate = _validator.ValidDeleteObject(exchangeRate,this) ? _repository.SoftDeleteObject(exchangeRate) : exchangeRate);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
         
        public bool IsExchangeRateDateDuplicated(ExchangeRate exchangeRate)
        {
            IQueryable<ExchangeRate> exchangeRates = _repository.FindAll(cb => cb.ExRateDate.Date == exchangeRate.ExRateDate.Date && !cb.IsDeleted && cb.CurrencyId != exchangeRate.CurrencyId);
            return (exchangeRates.Count() > 0 ? true : false);
        }

      
    }
}