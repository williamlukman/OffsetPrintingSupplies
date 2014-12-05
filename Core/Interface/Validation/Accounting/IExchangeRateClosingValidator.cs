using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IExchangeRateClosingValidator
    {
        ExchangeRateClosing VCreateObject(ExchangeRateClosing exchangeRateClosing, IExchangeRateClosingService _exchangeRateClosingService);
        ExchangeRateClosing VDeleteObject(ExchangeRateClosing exchangeRateClosing);
        bool ValidCreateObject(ExchangeRateClosing exchangeRateClosing, IExchangeRateClosingService _exchangeRateClosingService);
        bool ValidDeleteObject(ExchangeRateClosing exchangeRateClosing);
        bool isValid(ExchangeRateClosing exchangeRateClosing);
        string PrintError(ExchangeRateClosing exchangeRateClosing);
    }
}
