using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class BankAdministrationDetailService : IBankAdministrationDetailService
    {
        private IBankAdministrationDetailRepository _repository;
        private IBankAdministrationDetailValidator _validator;

        public BankAdministrationDetailService(IBankAdministrationDetailRepository _bankAdministrationDetailRepository, IBankAdministrationDetailValidator _bankAdministrationDetailValidator)
        {
            _repository = _bankAdministrationDetailRepository;
            _validator = _bankAdministrationDetailValidator;
        }

        public IBankAdministrationDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<BankAdministrationDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<BankAdministrationDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BankAdministrationDetail> GetObjectsByBankAdministrationId(int bankAdministrationId)
        {
            return _repository.GetObjectsByBankAdministrationId(bankAdministrationId);
        }

        public IList<BankAdministrationDetail> GetNonLegacyObjectsByBankAdministrationId(int bankAdministrationId)
        {
            return _repository.GetNonLegacyObjectsByBankAdministrationId(bankAdministrationId);
        }

        public BankAdministrationDetail GetLegacyObjectByBankAdministrationId(int bankAdministrationId)
        {
            return _repository.GetLegacyObjectByBankAdministrationId(bankAdministrationId);
        }
        
        public BankAdministrationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BankAdministrationDetail CreateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IAccountService _accountService)
        {
            bankAdministrationDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(bankAdministrationDetail, _bankAdministrationService, this, _accountService))
            {
                bankAdministrationDetail =  _repository.CreateObject(bankAdministrationDetail);
                BankAdministration bankAdministration = _bankAdministrationService.GetObjectById(bankAdministrationDetail.BankAdministrationId);
                _bankAdministrationService.CalculateTotalAmount(bankAdministration, this);
            }
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail UpdateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IAccountService _accountService)
        {
            if (_validator.ValidUpdateObject(bankAdministrationDetail, _bankAdministrationService, this, _accountService))
            {
                bankAdministrationDetail = _repository.UpdateObject(bankAdministrationDetail);
                BankAdministration bankAdministration = _bankAdministrationService.GetObjectById(bankAdministrationDetail.BankAdministrationId);
                _bankAdministrationService.CalculateTotalAmount(bankAdministration, this);
            }
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail CreateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IAccountService _accountService)
        {
            bankAdministrationDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateLegacyObject(bankAdministrationDetail, _bankAdministrationService, this, _accountService) ?
                    _repository.CreateObject(bankAdministrationDetail) : bankAdministrationDetail);
        }

        public BankAdministrationDetail UpdateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IAccountService _accountService)
        {
            return (_validator.ValidUpdateLegacyObject(bankAdministrationDetail, _bankAdministrationService, this, _accountService) ?
                     _repository.UpdateObject(bankAdministrationDetail) : bankAdministrationDetail);
        }

        public BankAdministrationDetail SoftDeleteObject(BankAdministrationDetail bankAdministrationDetail,IBankAdministrationService _bankAdministrationService)
        {
            if (_validator.ValidDeleteObject(bankAdministrationDetail, _bankAdministrationService))
            {
                bankAdministrationDetail = _repository.SoftDeleteObject(bankAdministrationDetail);
                BankAdministration bankAdministration = _bankAdministrationService.GetObjectById(bankAdministrationDetail.BankAdministrationId);
                _bankAdministrationService.CalculateTotalAmount(bankAdministration, this);
            }

            return bankAdministrationDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public BankAdministrationDetail ConfirmObject(BankAdministrationDetail bankAdministrationDetail, DateTime ConfirmationDate, IBankAdministrationService _bankAdministrationService)
        {
            bankAdministrationDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(bankAdministrationDetail, _bankAdministrationService))
            {
                bankAdministrationDetail = _repository.ConfirmObject(bankAdministrationDetail);
            }
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail UnconfirmObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService)
        {
            if (_validator.ValidUnconfirmObject(bankAdministrationDetail, _bankAdministrationService))
            {
                bankAdministrationDetail = _repository.UnconfirmObject(bankAdministrationDetail);
            }
            return bankAdministrationDetail;
        }
    }
}