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

        public IQueryable<UserAccess> GetQueryableObjectsByUserAccountId(int UserAccountId)
        {
            return _repository.GetQueryableObjectsByUserAccountId(UserAccountId);
        }

        public IList<UserAccess> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<UserAccess> GetObjectsByUserAccountId(int UserAccountId)
        {
            return _repository.GetObjectsByUserAccountId(UserAccountId);
        }

        public UserAccess GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public UserAccess GetObjectByUserAccountIdAndUserMenuId(int UserAccountId, int UserMenuId)
        {
            return _repository.GetObjectByUserAccountIdAndUserMenuId(UserAccountId, UserMenuId);
        }

        public UserAccess CreateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService)
        {
            userAccess.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(userAccess, _userAccountService, _userMenuService, this) ? _repository.CreateObject(userAccess) : userAccess);
        }

        public UserAccess UpdateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService)
        {
            return (_validator.ValidUpdateObject(userAccess, _userAccountService, _userMenuService, this) ? _repository.UpdateObject(userAccess) : userAccess);
        }

        public UserAccess SoftDeleteObject(UserAccess userAccess)
        {
            return (_validator.ValidDeleteObject(userAccess) ? _repository.SoftDeleteObject(userAccess) : userAccess);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public int CreateDefaultAccess(int UserAccountId, IUserMenuService _userMenuService, IUserAccountService _userAccountService)
        {
            var userMenus = _userMenuService.GetAll();
            int count = 0;
            foreach (var userMenu in userMenus)
            {
                UserAccess userAccess = new UserAccess()
                {
                    UserAccountId = UserAccountId,
                    UserMenuId = userMenu.Id,
                };
                UserAccount userAccount = _userAccountService.GetObjectById(UserAccountId);
                if (userAccount.IsAdmin)
                {
                    userAccess.AllowConfirm = true;
                    userAccess.AllowCreate = true;
                    userAccess.AllowDelete = true;
                    userAccess.AllowEdit = true;
                    userAccess.AllowPaid = true;
                    userAccess.AllowPrint = true;
                    userAccess.AllowReconcile = true;
                    userAccess.AllowUnconfirm = true;
                    userAccess.AllowUndelete = true;
                    userAccess.AllowUnpaid = true;
                    userAccess.AllowUnreconcile = true;
                    userAccess.AllowView = true;
                    userAccess.AllowSpecialPricing = true;
                }
                CreateObject(userAccess, _userAccountService, _userMenuService);
                if (!userAccess.Errors.Any()) count++;
            }
            return count;
        }

    }
}
