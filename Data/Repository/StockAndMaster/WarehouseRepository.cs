using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class WarehouseRepository : EfRepository<Warehouse>, IWarehouseRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public WarehouseRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<Warehouse> GetAll()
        {
            return FindAll().ToList();
        }

        public Warehouse GetObjectById(int Id)
        {
            Warehouse warehouse = Find(x => x.Id == Id && !x.IsDeleted);
            if (warehouse != null) { warehouse.Errors = new Dictionary<string, string>(); }
            return warehouse;
        }

        public Warehouse GetObjectByCode(string Code)
        {
            Warehouse warehouse = Find(x => x.Code == Code && !x.IsDeleted);
            if (warehouse != null) { warehouse.Errors = new Dictionary<string, string>(); }
            return warehouse;
        }

        public Warehouse GetObjectByName(string Name)
        {
            Warehouse warehouse = Find(x => x.Name == Name && !x.IsDeleted);
            if (warehouse != null) { warehouse.Errors = new Dictionary<string, string>(); }
            return warehouse;
        }

        public Warehouse CreateObject(Warehouse warehouse)
        {
            warehouse.IsDeleted = false;
            warehouse.CreatedAt = DateTime.Now;
            return Create(warehouse);
        }

        public Warehouse UpdateObject(Warehouse warehouse)
        {
            warehouse.UpdatedAt = DateTime.Now;
            Update(warehouse);
            return warehouse;
        }

        public Warehouse SoftDeleteObject(Warehouse warehouse)
        {
            warehouse.IsDeleted = true;
            warehouse.DeletedAt = DateTime.Now;
            Update(warehouse);
            return warehouse;
        }

        public bool DeleteObject(int Id)
        {
            Warehouse warehouse =  Find(x => x.Id == Id);
            return (Delete(warehouse) == 1) ? true : false;
        }

        public bool IsCodeDuplicated(Warehouse warehouse)
        {
            IList<Warehouse> warehouses = FindAll(x => x.Code == warehouse.Code && x.Id != warehouse.Id).ToList();
            return (warehouses.Count() > 0 ? true : false);
        }
    }
}