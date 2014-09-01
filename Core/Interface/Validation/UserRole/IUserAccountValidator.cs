using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IUserAccountValidator
    {
        UserAccount VIsValidUsername(UserAccount userAccount);
        UserAccount VIsValidPassword(UserAccount userAccount);
        UserAccount VIsCorrectOldPassword(UserAccount userAccount, string OldPassword, IUserAccountService _userAccountService);
        UserAccount VIsValidDeletedId(UserAccount userAccount, int LoggedId);
        UserAccount VHasUniqueUsername(UserAccount userAccount, IUserAccountService _userAccountService);
        UserAccount VCreateObject(UserAccount userAccount, IUserAccountService _userAccountService);
        UserAccount VUpdateObject(UserAccount userAccount, string OldPassword, IUserAccountService _userAccountService);
        UserAccount VDeleteObject(UserAccount userAccount, int LoggedId);
        bool ValidCreateObject(UserAccount userAccount, IUserAccountService _userAccountService);
        bool ValidUpdateObject(UserAccount userAccount, string OldPassword, IUserAccountService _userAccountService);
        bool ValidDeleteObject(UserAccount userAccount, int LoggedId);
        bool isValid(UserAccount userAccount);
        string PrintError(UserAccount userAccount);
    }
}
