using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface IGroupItemPriceRepository : IRepository<GroupItemPrice>
    {
        IQueryable<GroupItemPrice> GetQueryable();
        IList<GroupItemPrice> GetAll();
        GroupItemPrice GetObjectById(int Id);
        GroupItemPrice CreateObject(GroupItemPrice groupItemPrice);
        GroupItemPrice UpdateObject(GroupItemPrice groupItemPrice);
        GroupItemPrice SoftDeleteObject(GroupItemPrice groupItemPrice);
        bool DeleteObject(int Id);
    }
}
