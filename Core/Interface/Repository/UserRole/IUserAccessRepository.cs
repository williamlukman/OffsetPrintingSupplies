using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IUserAccessRepository : IRepository<UserAccess>
    {
        IQueryable<UserAccess> GetQueryable();
        IList<UserAccess> GetAll();
        UserAccess GetObjectById(int Id);
        UserAccess CreateObject(UserAccess userAccess);
        UserAccess UpdateObject(UserAccess userAccess);
        UserAccess SoftDeleteObject(UserAccess userAccess);
        bool DeleteObject(int Id);
    }
}