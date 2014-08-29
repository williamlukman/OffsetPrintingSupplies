using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IClosingValidator
    {
        //Closing VIsValidPeriod(Closing closing);
        //Closing VIsValidYearPeriod(Closing closing);
        Closing VHasBeginningPeriod(Closing closing);
        Closing VHasEndDatePeriod(Closing closing);

        Closing VCreateObject(Closing closing);
        Closing VCloseObject(Closing closing);
        Closing VDeleteObject(Closing closing);
        bool ValidCreateObject(Closing closing);
        bool ValidCloseObject(Closing closing);
        bool ValidDeleteObject(Closing closing);
        bool isValid(Closing closing);
        string PrintError(Closing closing);
    }
}
