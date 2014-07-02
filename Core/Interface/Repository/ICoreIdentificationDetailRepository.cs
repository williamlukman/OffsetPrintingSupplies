using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface ICoreIdentificationDetailRepository : IRepository<CoreIdentificationDetail>
    {
        IList<CoreIdentificationDetail> GetAll();
        IList<CoreIdentificationDetail> GetObjectsByCoreIdentificationId(int CoreIdentificationId);
        IList<CoreIdentificationDetail> GetObjectsByCoreBuilderId(int CoreBuilderId);
        CoreIdentificationDetail GetObjectById(int Id);
        CoreIdentificationDetail CreateObject(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail UpdateObject(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail SoftDeleteObject(CoreIdentificationDetail coreIdentificationDetail);
        bool DeleteObject(int Id);
    }
}