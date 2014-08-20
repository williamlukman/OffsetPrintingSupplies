﻿using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IRollerWarehouseMutationRepository : IRepository<RollerWarehouseMutation>
    {
        IQueryable<RollerWarehouseMutation> GetQueryable();
        IList<RollerWarehouseMutation> GetAll();
        IList<RollerWarehouseMutation> GetAllByMonthCreated();
        IList<RollerWarehouseMutation> GetObjectsByCoreIdentificationId(int coreIdentificationId);
        RollerWarehouseMutation GetObjectById(int Id);
        Warehouse GetWarehouseFrom(RollerWarehouseMutation rollerWarehouseMutation);
        Warehouse GetWarehouseTo(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation CreateObject(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation UpdateObject(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation SoftDeleteObject(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation ConfirmObject(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation UnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation);
        bool DeleteObject(int Id);
        string SetObjectCode();
    }
}