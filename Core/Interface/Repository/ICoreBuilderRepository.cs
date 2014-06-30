﻿using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface ICoreBuilderRepository : IRepository<CoreBuilder>
    {
        IList<CoreBuilder> GetAll();
        CoreBuilder GetObjectById(int Id);
        Item GetUsedCore(int id);
        Item GetNewCore(int id);
        CoreBuilder CreateObject(CoreBuilder coreBuilder);
        CoreBuilder UpdateObject(CoreBuilder coreBuilder);
        CoreBuilder SoftDeleteObject(CoreBuilder coreBuilder);
        bool DeleteObject(int Id);
    }
}