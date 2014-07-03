﻿using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class CoreIdentificationDetailRepository : EfRepository<CoreIdentificationDetail>, ICoreIdentificationDetailRepository
    {

        private OffsetPrintingSuppliesEntities entities;
        public CoreIdentificationDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<CoreIdentificationDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<CoreIdentificationDetail> GetObjectsByCoreIdentificationId(int CoreIdentificationId)
        {
            return FindAll(x => x.CoreIdentificationId == CoreIdentificationId && !x.IsDeleted).ToList();
        }

        public IList<CoreIdentificationDetail> GetObjectsByCoreBuilderId(int CoreBuilderId)
        {
            return FindAll(x => x.CoreBuilderId == CoreBuilderId && !x.IsDeleted).ToList();
        }

        public IList<CoreIdentificationDetail> GetObjectsByRollerTypeId(int rollerTypeId)
        {
            return FindAll(x => x.RollerTypeId == rollerTypeId && !x.IsDeleted).ToList();
        }

        public IList<CoreIdentificationDetail> GetObjectsByMachineId(int machineId)
        {
            return FindAll(x => x.MachineId == machineId && !x.IsDeleted).ToList();
        }

        public CoreIdentificationDetail GetObjectById(int Id)
        {
            CoreIdentificationDetail coreIdentificationDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (coreIdentificationDetail != null) { coreIdentificationDetail.Errors = new Dictionary<string, string>(); }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail CreateObject(CoreIdentificationDetail coreIdentificationDetail)
        {
            coreIdentificationDetail.IsDeleted = false;
            coreIdentificationDetail.CreatedAt = DateTime.Now;
            return Create(coreIdentificationDetail);
        }

        public CoreIdentificationDetail UpdateObject(CoreIdentificationDetail coreIdentificationDetail)
        {
            coreIdentificationDetail.UpdatedAt = DateTime.Now;
            Update(coreIdentificationDetail);
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail SoftDeleteObject(CoreIdentificationDetail coreIdentificationDetail)
        {
            coreIdentificationDetail.IsDeleted = true;
            coreIdentificationDetail.DeletedAt = DateTime.Now;
            Update(coreIdentificationDetail);
            return coreIdentificationDetail;
        }

        public bool DeleteObject(int Id)
        {
            CoreIdentificationDetail coreIdentificationDetail = Find(x => x.Id == Id);
            return (Delete(coreIdentificationDetail) == 1) ? true : false;
        }
    }
}