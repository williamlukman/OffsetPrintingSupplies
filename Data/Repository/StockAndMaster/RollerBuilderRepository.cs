﻿using Core.DomainModel;
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
    public class RollerBuilderRepository : EfRepository<RollerBuilder>, IRollerBuilderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public RollerBuilderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<RollerBuilder> GetQueryable()
        {
            return FindAll();
        }

        public IList<RollerBuilder> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<RollerBuilder> GetObjectsByCompoundId(int compoundId)
        {
            return FindAll(x => x.CompoundId == compoundId && !x.IsDeleted).ToList();
        }

        public IList<RollerBuilder> GetObjectsByCoreBuilderId(int CoreBuilderId)
        {
            return FindAll(x => x.CoreBuilderId == CoreBuilderId && !x.IsDeleted).ToList();
        }

        public IList<RollerBuilder> GetObjectsByItemId(int ItemId)
        {
            return FindAll(x => !x.IsDeleted && (x.CompoundId == ItemId || x.RollerUsedCoreItemId == ItemId || x.RollerNewCoreItemId == ItemId)).ToList();
        }

        public IList<RollerBuilder> GetObjectsByMachineId(int machineId)
        {
            return FindAll(x => x.MachineId == machineId && !x.IsDeleted).ToList();
        }

        public IList<RollerBuilder> GetObjectsByRollerTypeId(int rollerTypeId)
        {
            return FindAll(x => x.RollerTypeId == rollerTypeId && !x.IsDeleted).ToList();
        }

        public Item GetRollerUsedCore(int id)
        {
            using (var db = GetContext())
            {
                RollerBuilder rollerBuilder = GetObjectById(id);
                Item item = (from obj in db.Items
                             where obj.Id == rollerBuilder.RollerUsedCoreItemId
                             select obj).FirstOrDefault();
                if (item != null) { item.Errors = new Dictionary<string, string>(); }
                return item;
            }
        }

        public Item GetRollerNewCore(int id)
        {
            using (var db = GetContext())
            {
                RollerBuilder rollerBuilder = GetObjectById(id);
                Item item = (from obj in db.Items
                             where obj.Id == rollerBuilder.RollerNewCoreItemId
                             select obj).FirstOrDefault();
                if (item != null) { item.Errors = new Dictionary<string, string>(); }
                return item;
            }
        }

        public RollerBuilder GetObjectById(int Id)
        {
            RollerBuilder rollerBuilder = Find(x => x.Id == Id && !x.IsDeleted);
            if (rollerBuilder != null) { rollerBuilder.Errors = new Dictionary<string, string>(); }
            return rollerBuilder;
        }

        public RollerBuilder CreateObject(RollerBuilder rollerBuilder)
        {
            rollerBuilder.Errors = new Dictionary<string, string>();
            rollerBuilder.IsDeleted = false;
            rollerBuilder.CreatedAt = DateTime.Now;
            return Create(rollerBuilder);
        }

        public RollerBuilder UpdateObject(RollerBuilder rollerBuilder)
        {
            rollerBuilder.UpdatedAt = DateTime.Now;
            Update(rollerBuilder);
            return rollerBuilder;
        }

        public RollerBuilder SoftDeleteObject(RollerBuilder rollerBuilder)
        {
            rollerBuilder.IsDeleted = true;
            rollerBuilder.DeletedAt = DateTime.Now;
            Update(rollerBuilder);
            return rollerBuilder;
        }

        public bool DeleteObject(int Id)
        {
            RollerBuilder rollerBuilder = Find(x => x.Id == Id);
            return (Delete(rollerBuilder) == 1) ? true : false;
        }
    }
}