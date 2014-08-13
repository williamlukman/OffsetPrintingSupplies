using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class GroupItemPriceRepository : EfRepository<GroupItemPrice>, IGroupItemPriceRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public GroupItemPriceRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<GroupItemPrice> GetAll()
        {
            return FindAll().ToList();
        }

        public GroupItemPrice GetObjectById(int Id)
        {
            GroupItemPrice groupItemPrice = Find(x => x.Id == Id && !x.IsDeleted);
            if (groupItemPrice != null) { groupItemPrice.Errors = new Dictionary<string, string>(); }
            return groupItemPrice;
        }

        public GroupItemPrice CreateObject(GroupItemPrice groupItemPrice)
        {
            groupItemPrice.IsDeleted = false;
            groupItemPrice.CreatedAt = DateTime.Now;
            return Create(groupItemPrice);
        }

        public GroupItemPrice CreateObject(GroupItemPrice groupItemPrice, DateTime CreationDate)
        {
            groupItemPrice.IsDeleted = false;
            groupItemPrice.CreatedAt = CreationDate;
            return Create(groupItemPrice);
        }

        public GroupItemPrice UpdateObject(GroupItemPrice groupItemPrice)
        {
            groupItemPrice.UpdatedAt = DateTime.Now;
            Update(groupItemPrice);
            return groupItemPrice;
        }

        public GroupItemPrice UpdateObject(GroupItemPrice groupItemPrice, Nullable<DateTime> UpdateDate)
        {
            groupItemPrice.UpdatedAt = UpdateDate;
            Update(groupItemPrice);
            return groupItemPrice;
        }

        public GroupItemPrice SoftDeleteObject(GroupItemPrice groupItemPrice)
        {
            groupItemPrice.IsDeleted = true;
            groupItemPrice.DeletedAt = DateTime.Now;
            Update(groupItemPrice);
            return groupItemPrice;
        }

        public bool DeleteObject(int Id)
        {
            GroupItemPrice groupItemPrice = Find(x => x.Id == Id);
            return (Delete(groupItemPrice) == 1) ? true : false;
        }
    }
}
