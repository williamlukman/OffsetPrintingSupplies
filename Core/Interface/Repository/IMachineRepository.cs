﻿using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IMachineRepository : IRepository<Machine>
    {
        IList<Machine> GetAll();
        Machine GetObjectById(int Id);
        Machine CreateObject(Machine machine);
        Machine UpdateObject(Machine machine);
        Machine SoftDeleteObject(Machine machine);
        bool DeleteObject(int Id);
    }
}