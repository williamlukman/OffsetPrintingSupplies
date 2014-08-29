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
    public class ClosingValidator : IClosingValidator
    {

        public Closing VIsValidPeriod(Closing closing)
        {
            if (closing.Period < 1 || closing.Period > 12)
            {
                closing.Errors.Add("Period", "Harus antara 1 sampai 12");
            }
            return closing;
        }

        /*public Closing VIsValidYearPeriod(Closing closing)
        {
            if (closing.YearPeriod > DateTime.Now.Year)
            {
                closing.Errors.Add("YearPeriod", "Tidak boleh lebih besar dari tahun ini");
            }
            return closing;
        }*/

        public Closing VHasBeginningPeriod(Closing closing)
        {
            if (closing.BeginningPeriod == null || closing.BeginningPeriod.Equals(DateTime.FromBinary(0)))
            {
                closing.Errors.Add("BeginningPeriod", "Tidak valid");
            }
            return closing;
        }

        public Closing VHasEndDatePeriod(Closing closing)
        {
            if (closing.EndDatePeriod == null || closing.EndDatePeriod.Equals(DateTime.FromBinary(0)))
            {
                closing.Errors.Add("EndDatePeriod", "Tidak valid");
            }
            return closing;
        }

        public Closing VCreateObject(Closing closing)
        {
            VHasBeginningPeriod(closing);
            return closing;
        }

        public Closing VCloseObject(Closing closing)
        {
            VHasEndDatePeriod(closing);
            return closing;
        }

        public Closing VDeleteObject(Closing closing)
        {
            return closing;
        }

        public bool ValidCreateObject(Closing closing)
        {
            VCreateObject(closing);
            return isValid(closing);
        }

        public bool ValidCloseObject(Closing closing)
        {
            VCloseObject(closing);
            return isValid(closing);
        }

        public bool ValidDeleteObject(Closing closing)
        {
            closing.Errors.Clear();
            VDeleteObject(closing);
            return isValid(closing);
        }

        public bool isValid(Closing obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Closing obj)
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
