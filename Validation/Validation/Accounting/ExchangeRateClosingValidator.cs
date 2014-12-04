
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class ExchangeRateClosingValidator : IExchangeRateClosingValidator
    {


        public ExchangeRateClosing VCreateObject(ExchangeRateClosing exchangeRateClosing, IExchangeRateClosingService _exchangeRateClosingService)
        {
            return exchangeRateClosing;
        }


        public ExchangeRateClosing VDeleteObject(ExchangeRateClosing exchangeRateClosing)
        {
            return exchangeRateClosing;
        }

        public bool ValidCreateObject(ExchangeRateClosing exchangeRateClosing, IExchangeRateClosingService _exchangeRateClosingService)
        {
            VCreateObject(exchangeRateClosing, _exchangeRateClosingService);
            return isValid(exchangeRateClosing);
        }


        public bool ValidDeleteObject(ExchangeRateClosing exchangeRateClosing)
        {
            exchangeRateClosing.Errors.Clear();
            VDeleteObject(exchangeRateClosing);
            return isValid(exchangeRateClosing);
        }

        public bool isValid(ExchangeRateClosing obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ExchangeRateClosing obj)
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
