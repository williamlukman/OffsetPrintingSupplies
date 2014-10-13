﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Repository;

namespace Core.Interface.Validation
{
    public interface IReceivableValidator
    {
        Receivable VCreateObject(Receivable receivable, IReceivableService _receivableService);
        Receivable VUpdateObject(Receivable receivable, IReceivableService _receivableService);
        Receivable VDeleteObject(Receivable receivable);
        bool ValidCreateObject(Receivable receivable, IReceivableService _receivableService);
        bool ValidUpdateObject(Receivable receivable, IReceivableService _receivableService);
        bool ValidDeleteObject(Receivable receivable);
        bool isValid(Receivable receivable);
        string PrintError(Receivable receivable);
    }
}
