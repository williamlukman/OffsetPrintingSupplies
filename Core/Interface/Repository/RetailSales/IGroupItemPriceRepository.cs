using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface IGroupItemPriceRepository : IRepository<GroupItemPrice>
    {
        IList<GroupItemPrice> GetAll();
        GroupItemPrice GetObjectById(int Id);
        GroupItemPrice CreateObject(GroupItemPrice groupItemPrice);
        GroupItemPrice CreateObject(GroupItemPrice groupItemPrice, DateTime CreationDate);
        GroupItemPrice UpdateObject(GroupItemPrice groupItemPrice, Nullable<DateTime> UpdateDate);
        GroupItemPrice SoftDeleteObject(GroupItemPrice groupItemPrice);
        bool DeleteObject(int Id);
    }
}
