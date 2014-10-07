using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IServiceCostRepository : IRepository<ServiceCost>
    {
        IQueryable<ServiceCost> GetQueryable();
        IList<ServiceCost> GetAll();
        ServiceCost FindOrCreateObject(int rollerBuilderId);
        ServiceCost GetObjectById(int Id);
        ServiceCost GetObjectByItemId(int ItemId);
        ServiceCost GetObjectByRollerBuilderId(int RollerBuilderId);
        ServiceCost CreateObject(ServiceCost serviceCost);
        ServiceCost UpdateObject(ServiceCost serviceCost);
    }
}