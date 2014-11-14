using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICurrencyValidator
    {
        Currency VCreateObject(Currency currency, ICurrencyService _currencyService);
        Currency VUpdateObject(Currency currency, ICurrencyService _currencyService);
        Currency VDeleteObject(Currency currency, ICurrencyService _currencyService);
        bool ValidCreateObject(Currency currency, ICurrencyService _currencyService);
        bool ValidUpdateObject(Currency currency, ICurrencyService _currencyService);
        bool ValidDeleteObject(Currency currency, ICurrencyService _currencyService);
        bool isValid(Currency currency);
        string PrintError(Currency currency);
    }
}
