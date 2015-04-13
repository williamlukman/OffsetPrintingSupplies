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
    public class BlanketWarehouseMutationRepository : EfRepository<BlanketWarehouseMutation>, IBlanketWarehouseMutationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BlanketWarehouseMutationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<BlanketWarehouseMutation> GetQueryable()
        {
            return FindAll();
        }

        public IList<BlanketWarehouseMutation> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<BlanketWarehouseMutation> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<BlanketWarehouseMutation> GetObjectsByBlanketOrderId(int blanketOrderId)
        {
            return FindAll(x => x.BlanketOrderId == blanketOrderId && !x.IsDeleted).ToList();
        }

        public BlanketWarehouseMutation GetObjectById(int Id)
        {
            BlanketWarehouseMutation blanketWarehouseMutation = Find(x => x.Id == Id && !x.IsDeleted);
            if (blanketWarehouseMutation != null) { blanketWarehouseMutation.Errors = new Dictionary<string, string>(); }
            return blanketWarehouseMutation;
        }

        public Warehouse GetWarehouseFrom(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            using (var db = GetContext())
            {
                Warehouse warehouseFrom =
                    (from obj in db.Warehouses
                     where obj.Id == blanketWarehouseMutation.WarehouseFromId &&
                           !obj.IsDeleted
                     select obj).First();
                return warehouseFrom;
            }
        }

        public Warehouse GetWarehouseTo(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            using (var db = GetContext())
            {
                Warehouse warehouseTo =
                    (from obj in db.Warehouses
                     where obj.Id == blanketWarehouseMutation.WarehouseToId &&
                           !obj.IsDeleted
                     select obj).First();
                return warehouseTo;
            }
        }

        public BlanketWarehouseMutation CreateObject(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            blanketWarehouseMutation.Code = SetObjectCode();
            blanketWarehouseMutation.IsConfirmed = false;
            blanketWarehouseMutation.IsDeleted = false;
            blanketWarehouseMutation.CreatedAt = DateTime.Now;
            return Create(blanketWarehouseMutation);
        }

        public BlanketWarehouseMutation UpdateObject(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            blanketWarehouseMutation.UpdatedAt = DateTime.Now;
            Update(blanketWarehouseMutation);
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation SoftDeleteObject(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            blanketWarehouseMutation.IsDeleted = true;
            blanketWarehouseMutation.DeletedAt = DateTime.Now;
            Update(blanketWarehouseMutation);
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation ConfirmObject(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            blanketWarehouseMutation.IsConfirmed = true;
            Update(blanketWarehouseMutation);
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation UnconfirmObject(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            blanketWarehouseMutation.IsConfirmed = false;
            blanketWarehouseMutation.ConfirmationDate = null;
            Update(blanketWarehouseMutation);
            return blanketWarehouseMutation;
        }

        public bool DeleteObject(int Id)
        {
            BlanketWarehouseMutation blanketWarehouseMutation =  Find(x => x.Id == Id);
            return (Delete(blanketWarehouseMutation) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}