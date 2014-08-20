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
    public class MachineRepository : EfRepository<Machine>, IMachineRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public MachineRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Machine> GetQueryable()
        {
            return FindAll();
        }

        public IList<Machine> GetAll()
        {
            return FindAll().ToList();
        }

        public Machine GetObjectById(int Id)
        {
            Machine machine = Find(x => x.Id == Id && !x.IsDeleted);
            if (machine != null) { machine.Errors = new Dictionary<string, string>(); }
            return machine;
        }

        public Machine CreateObject(Machine machine)
        {
            machine.IsDeleted = false;
            machine.CreatedAt = DateTime.Now;
            return Create(machine);
        }

        public Machine UpdateObject(Machine machine)
        {
            machine.UpdatedAt = DateTime.Now;
            Update(machine);
            return machine;
        }

        public Machine SoftDeleteObject(Machine machine)
        {
            machine.IsDeleted = true;
            machine.DeletedAt = DateTime.Now;
            Update(machine);
            return machine;
        }

        public bool DeleteObject(int Id)
        {
            Machine machine = Find(x => x.Id == Id);
            return (Delete(machine) == 1) ? true : false;
        }
    }
}