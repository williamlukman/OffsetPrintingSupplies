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
    public class UserAccessService : IUserAccessService
    {
        private IUserAccessRepository _repository;
        private IUserAccessValidator _validator;

        public UserAccessService(IUserAccessRepository _userAccessRepository, IUserAccessValidator _userAccessValidator)
        {
            _repository = _userAccessRepository;
            _validator = _userAccessValidator;
        }

        public IUserAccessValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<UserAccess> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<UserAccess> GetAll()
        {
            return _repository.GetAll();
        }

        public UserAccess GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public UserAccess CreateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService)
        {
            userAccess.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(userAccess, _userAccountService, _userMenuService) ? _repository.CreateObject(userAccess) : userAccess);
        }

        public UserAccess UpdateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService)
        {
            return (_validator.ValidUpdateObject(userAccess, _userAccountService, _userMenuService) ? _repository.UpdateObject(userAccess) : userAccess);
        }

        public UserAccess SoftDeleteObject(UserAccess userAccess)
        {
            return (_validator.ValidDeleteObject(userAccess) ? _repository.SoftDeleteObject(userAccess) : userAccess);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
