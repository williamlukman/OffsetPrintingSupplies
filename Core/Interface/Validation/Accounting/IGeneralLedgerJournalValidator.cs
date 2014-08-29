using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IGeneralLedgerJournalValidator
    {
        GeneralLedgerJournal VIsValidSourceDocument(GeneralLedgerJournal generalLedgerJournal);
        GeneralLedgerJournal VCreateObject(GeneralLedgerJournal generalLedgerJournal);
        GeneralLedgerJournal VDeleteObject(GeneralLedgerJournal generalLedgerJournal);
        bool ValidCreateObject(GeneralLedgerJournal generalLedgerJournal);
        bool ValidDeleteObject(GeneralLedgerJournal generalLedgerJournal);
        bool isValid(GeneralLedgerJournal generalLedgerJournal);
        string PrintError(GeneralLedgerJournal generalLedgerJournal);
    }
}
