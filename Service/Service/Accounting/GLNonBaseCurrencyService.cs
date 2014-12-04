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
    public class GLNonBaseCurrencyService : IGLNonBaseCurrencyService
    {
        #region BasicServiceFunctionality
        private IGLNonBaseCurrencyRepository _repository;
        private IGLNonBaseCurrencyValidator _validator;

        public GLNonBaseCurrencyService(IGLNonBaseCurrencyRepository _generalLedgerJournalRepository, IGLNonBaseCurrencyValidator _generalLedgerJournalValidator)
        {
            _repository = _generalLedgerJournalRepository;
            _validator = _generalLedgerJournalValidator;
        }

        public IGLNonBaseCurrencyValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<GLNonBaseCurrency> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<GLNonBaseCurrency> GetAll()
        {
            return _repository.GetAll();
        }

        public GLNonBaseCurrency GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public GLNonBaseCurrency CreateObject(GLNonBaseCurrency generalLedgerJournal, IAccountService _accountService)
        {
            generalLedgerJournal.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(generalLedgerJournal, _accountService) ? _repository.CreateObject(generalLedgerJournal) : generalLedgerJournal);
        }

        public GLNonBaseCurrency SoftDeleteObject(GLNonBaseCurrency generalLedgerJournal)
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
