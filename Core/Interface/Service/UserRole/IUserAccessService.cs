using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IUserAccessService
    {
        IQueryable<UserAccess> GetQueryable();
        IUserAccessValidator GetValidator();
        IList<UserAccess> GetAll();
        UserAccess GetObjectById(int Id);
        UserAccess CreateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService);
        UserAccess UpdateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService);
        UserAccess SoftDeleteObject(UserAccess userAccess);
        bool DeleteObject(int Id);
    }
}