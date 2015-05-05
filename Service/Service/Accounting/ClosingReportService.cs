using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using System.Data.Entity;
using Core.Interface.Validation;

namespace Service.Service
{
    public class ClosingReportService : IClosingReportService
    {
        #region BasicServiceFunctionality
        private IClosingReportRepository _repository;
        private IClosingReportValidator _validator;

        public ClosingReportService(IClosingReportRepository _generalLedgerJournalRepository, IClosingReportValidator _generalLedgerJournalValidator)
        {
            _repository = _generalLedgerJournalRepository;
            _validator = _generalLedgerJournalValidator;
        }

        public IClosingReportValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ClosingReport> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ClosingReport> GetAll()
        {
            return _repository.GetAll();
        }

        public ClosingReport GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ClosingReport UpdateObject(ClosingReport closingReport)
        {
            return _repository.UpdateObject(closingReport);
        }


        public ClosingReport CreateObject(ClosingReport generalLedgerJournal, IAccountService _accountService)
        {
            generalLedgerJournal.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(generalLedgerJournal, _accountService) ? _repository.CreateObject(generalLedgerJournal) : generalLedgerJournal);
        }

        public ClosingReport SoftDeleteObject(ClosingReport generalLedgerJournal)
        {
            return (_validator.ValidDeleteObject(generalLedgerJournal) ? _repository.SoftDeleteObject(generalLedgerJournal) : generalLedgerJournal);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
        #endregion
    }
}
