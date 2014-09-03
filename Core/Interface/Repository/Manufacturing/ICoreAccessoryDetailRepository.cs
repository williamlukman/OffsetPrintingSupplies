using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICoreAccessoryDetailRepository : IRepository<CoreAccessoryDetail>
    {
        IQueryable<CoreAccessoryDetail> GetQueryable(); 
        IList<CoreAccessoryDetail> GetAll();
        IList<CoreAccessoryDetail> GetObjectsByCoreIdentificationDetailId(int CoreIdentificationDetailId);
        IList<CoreAccessoryDetail> GetObjectsByItemId(int ItemId);
        CoreAccessoryDetail GetObjectById(int Id);
        CoreAccessoryDetail CreateObject(CoreAccessoryDetail CoreAccessoryDetail);
        CoreAccessoryDetail UpdateObject(CoreAccessoryDetail CoreAccessoryDetail);
        CoreAccessoryDetail SoftDeleteObject(CoreAccessoryDetail CoreAccessoryDetail);
        bool DeleteObject(int Id);
    }
}