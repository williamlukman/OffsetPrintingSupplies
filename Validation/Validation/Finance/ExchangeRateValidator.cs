
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class ExchangeRateValidator : IExchangeRateValidator
    {
        public ExchangeRate VExchangeRateDate(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService)
        {
            if (_exchangeRateService.IsExchangeRateDateDuplicated(exchangeRate))
            {
                exchangeRate.Errors.Add("Generic", "Date Rate tidak boleh ada duplikasi");
            }
            return exchangeRate;
        }

        public ExchangeRate VCurrencyId(ExchangeRate exchangeRate, ICurrencyService _currencyService)
        {
            Currency currency = _currencyService.GetObjectById(exchangeRate.CurrencyId);

            if (currency == null)
            {
                exchangeRate.Errors.Add("CurrencyId", "Invalid Currency");
            }
            else
            {
                if (currency.IsBase == true)
                {
                    exchangeRate.Errors.Add("CurrencyId", "Invalid Currency");
                }
            }
            return exchangeRate;
        }

        public ExchangeRate VCreateObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService)
        {
            VExchangeRateDate(exchangeRate, _exchangeRateService);
            if (!isValid(exchangeRate)) { return exchangeRate; }
            return exchangeRate;
        }

        public ExchangeRate VUpdateObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService)
        {
            VExchangeRateDate(exchangeRate, _exchangeRateService);
            if (!isValid(exchangeRate)) { return exchangeRate; }
            return exchangeRate;
        }

        public ExchangeRate VDeleteObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService)
        {
            return exchangeRate;
        }

        public ExchangeRate VAdjustAmount(ExchangeRate exchangeRate)
        {
            return exchangeRate;
        }

        public bool ValidCreateObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService)
        {
            VCreateObject(exchangeRate, _exchangeRateService);
            return isValid(exchangeRate);
        }

        public bool ValidUpdateObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService)
        {
            exchangeRate.Errors.Clear();
            VUpdateObject(exchangeRate, _exchangeRateService);
            return isValid(exchangeRate);
        }

        public bool ValidDeleteObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService)
        {
            exchangeRate.Errors.Clear();
            VDeleteObject(exchangeRate, _exchangeRateService);
            return isValid(exchangeRate);
        }

        public bool isValid(ExchangeRate obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ExchangeRate obj)
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
