using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IRollerBuilderRepository : IRepository<RollerBuilder>
    {
        IQueryable<RollerBuilder> GetQueryable();
        IList<RollerBuilder> GetAll();
        IList<RollerBuilder> GetObjectsByCompoundId(int compoundId);
        IList<RollerBuilder> GetObjectsByCoreBuilderId(int coreBuilderId);
        IList<RollerBuilder> GetObjectsByItemId(int ItemId);
        IList<RollerBuilder> GetObjectsByMachineId(int machineId);
        IList<RollerBuilder> GetObjectsByRollerTypeId(int rollerTypeId);
        RollerBuilder GetObjectById(int Id);
        Item GetRollerUsedCore(int Id);
        Item GetRollerNewCore(int Id);
        RollerBuilder CreateObject(RollerBuilder rollerBuilder);
        RollerBuilder UpdateObject(RollerBuilder rollerBuilder);
        RollerBuilder SoftDeleteObject(RollerBuilder rollerBuilder);
        bool DeleteObject(int Id);
    }
}