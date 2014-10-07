using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;

namespace Data.Repository
{
    public class ServiceCostRepository : EfRepository<ServiceCost>, IServiceCostRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ServiceCostRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ServiceCost> GetQueryable()
        {
            return FindAll();
        }

        public IList<ServiceCost> GetAll()
        {
            return FindAll().ToList();
        }

        public ServiceCost FindOrCreateObject(int RollerBuilderId)
        {
            using (var db = GetContext())
            {
                ServiceCost serviceCost = Find(x => x.RollerBuilderId == RollerBuilderId);
                if (serviceCost == null)
                {
                    RollerBuilder rollerBuilder = (from obj in db.RollerBuilders
                                                   where obj.Id == RollerBuilderId && !obj.IsDeleted
                                                   select obj).FirstOrDefault();
                    Item item = (from obj in db.Items
                                 where obj.Id == rollerBuilder.RollerUsedCoreItemId && !obj.IsDeleted
                                 select obj).FirstOrDefault();
                    if (item != null) { item.Errors = new Dictionary<string, string>(); }

                    serviceCost = new ServiceCost()
                    {
                        RollerBuilderId = RollerBuilderId,
                        ItemId = item.Id,
                        Quantity = 0,
                        AvgPrice = 0
                    };
                    serviceCost = CreateObject(serviceCost);
                }
                serviceCost.Errors = new Dictionary<string, string>();
                return serviceCost;
            }
        }

        public ServiceCost GetObjectById(int Id)
        {
            ServiceCost serviceCost = Find(x => x.Id == Id);
            if (serviceCost != null) { serviceCost.Errors = new Dictionary<string, string>(); }
            return serviceCost;
        }

        public ServiceCost GetObjectByItemId(int ItemId)
        {
            ServiceCost serviceCost = Find(x => x.ItemId == ItemId);
            if (serviceCost != null) { serviceCost.Errors = new Dictionary<string, string>(); }
            return serviceCost;
        }

        public ServiceCost GetObjectByRollerBuilderId(int RollerBuilderId)
        {
            ServiceCost serviceCost = Find(x => x.RollerBuilderId == RollerBuilderId);
            if (serviceCost != null) { serviceCost.Errors = new Dictionary<string, string>(); }
            return serviceCost;
        }

        public ServiceCost CreateObject(ServiceCost serviceCost)
        {
            serviceCost.CreatedAt = DateTime.Now;
            return Create(serviceCost);
        }

        public ServiceCost UpdateObject(ServiceCost serviceCost)
        {
            serviceCost.UpdatedAt = DateTime.Now;
            Update(serviceCost);
            return serviceCost;
        }
    }
}