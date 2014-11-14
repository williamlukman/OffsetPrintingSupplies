using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IExchangeRateValidator
    {
        ExchangeRate VCreateObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService);
        ExchangeRate VUpdateObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService);
        ExchangeRate VDeleteObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService);
        bool ValidCreateObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService);
        bool ValidUpdateObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService);
        bool ValidDeleteObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService);
        bool isValid(ExchangeRate exchangeRate);
        string PrintError(ExchangeRate exchangeRate);
    }
}
