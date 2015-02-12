using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;

namespace Service.Service
{
    public class ValidCombIncomeStatementService : IValidCombIncomeStatementService
    {
        private IValidCombIncomeStatementRepository _repository;
        private IValidCombIncomeStatementValidator _validator;

        public ValidCombIncomeStatementService(IValidCombIncomeStatementRepository _validCombIncomeStatementRepository, IValidCombIncomeStatementValidator _validCombIncomeStatementValidator)
        {
            _repository = _validCombIncomeStatementRepository;
            _validator = _validCombIncomeStatementValidator;
        }

        public IValidCombIncomeStatementValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ValidCombIncomeStatement> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ValidCombIncomeStatement> GetAll()
        {
            return _repository.GetAll();
        }

        public ValidCombIncomeStatement GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ValidCombIncomeStatement FindOrCreateObjectByAccountAndClosing(int accountId, int closingId)
        {
            return _repository.FindOrCreateObjectByAccountAndClosing(accountId, closingId);
        }

        public ValidCombIncomeStatement CreateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService)
        {
            validCombIncomeStatement.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(validCombIncomeStatement, _accountService, _closingService) ? _repository.CreateObject(validCombIncomeStatement) : validCombIncomeStatement);
        }

        public ValidCombIncomeStatement UpdateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService)
        {
            return (_validator.ValidUpdateObject(validCombIncomeStatement, _accountService, _closingService) ? _repository.UpdateObject(validCombIncomeStatement) : validCombIncomeStatement);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public ValidCombIncomeStatement CalculateTotalAmount(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService)
        {
            decimal totalAmount = 0;

            IList<Account> subnodeaccounts = _accountService.GetQueryable().Where(x => x.ParentId == validCombIncomeStatement.AccountId && !x.IsDeleted).ToList();
            if (subnodeaccounts.Any())
            {
                foreach (var subnode in subnodeaccounts)
                {
                    totalAmount += FindOrCreateObjectByAccountAndClosing(subnode.Id, validCombIncomeStatement.ClosingId).Amount;
                }
                validCombIncomeStatement.Amount = totalAmount;
                UpdateObject(validCombIncomeStatement, _accountService, _closingService);
            }
            return validCombIncomeStatement;
        }
    }
}
