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
    public class WarehouseMutationRepository : EfRepository<WarehouseMutation>, IWarehouseMutationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public WarehouseMutationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<WarehouseMutation> GetQueryable()
        {
            return FindAll();
        }

        public IList<WarehouseMutation> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<WarehouseMutation> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public WarehouseMutation GetObjectById(int Id)
        {
            WarehouseMutation warehouseMutation = Find(x => x.Id == Id && !x.IsDeleted);
            if (warehouseMutation != null) { warehouseMutation.Errors = new Dictionary<string, string>(); }
            return warehouseMutation;
        }

        public Warehouse GetWarehouseFrom(WarehouseMutation warehouseMutation)
        {
            using (var db = GetContext())
            {
                Warehouse warehouseFrom =
                    (from obj in db.Warehouses
                     where obj.Id == warehouseMutation.WarehouseFromId &&
                           !obj.IsDeleted
                     select obj).First();
                return warehouseFrom;
            }
        }

        public Warehouse GetWarehouseTo(WarehouseMutation warehouseMutation)
        {
            using (var db = GetContext())
            {
                Warehouse warehouseTo =
                    (from obj in db.Warehouses
                     where obj.Id == warehouseMutation.WarehouseToId &&
                           !obj.IsDeleted
                     select obj).First();
                return warehouseTo;
            }
        }

        public WarehouseMutation CreateObject(WarehouseMutation warehouseMutation)
        {
            warehouseMutation.Code = SetObjectCode();
            warehouseMutation.IsConfirmed = false;
            warehouseMutation.IsDeleted = false;
            warehouseMutation.CreatedAt = DateTime.Now;
            return Create(warehouseMutation);
        }

        public WarehouseMutation UpdateObject(WarehouseMutation warehouseMutation)
        {
            warehouseMutation.UpdatedAt = DateTime.Now;
            Update(warehouseMutation);
            return warehouseMutation;
        }

        public WarehouseMutation SoftDeleteObject(WarehouseMutation warehouseMutation)
        {
            warehouseMutation.IsDeleted = true;
            warehouseMutation.DeletedAt = DateTime.Now;
            Update(warehouseMutation);
            return warehouseMutation;
        }

        public WarehouseMutation ConfirmObject(WarehouseMutation warehouseMutation)
        {
            warehouseMutation.IsConfirmed = true;
            Update(warehouseMutation);
            return warehouseMutation;
        }

        public WarehouseMutation UnconfirmObject(WarehouseMutation warehouseMutation)
        {
            warehouseMutation.IsConfirmed = false;
            warehouseMutation.ConfirmationDate = null;
            Update(warehouseMutation);
            return warehouseMutation;
        }

        public bool DeleteObject(int Id)
        {
            WarehouseMutation warehouseMutation =  Find(x => x.Id == Id);
            return (Delete(warehouseMutation) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}