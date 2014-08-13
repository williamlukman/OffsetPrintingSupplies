using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class GroupRepository : EfRepository<Group>, IGroupRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public GroupRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<Group> GetAll()
        {
            return FindAll().ToList();
        }

        public Group GetObjectById(int Id)
        {
            Group group = Find(x => x.Id == Id && !x.IsDeleted);
            if (group != null) { group.Errors = new Dictionary<string, string>(); }
            return group;
        }

        public Group GetObjectByIsLegacy(bool IsLegacy)
        {
            Group group = Find(x => x.IsLegacy == IsLegacy && !x.IsDeleted);
            if (group != null) { group.Errors = new Dictionary<string, string>(); }
            return group;
        }

        public Group GetObjectByName(string Name)
        {
            Group group = FindAll(x => x.Name == Name && !x.IsDeleted).FirstOrDefault();
            if (group != null) { group.Errors = new Dictionary<string, string>(); }
            return group;
        }

        public Group CreateObject(Group group)
        {
            group.IsDeleted = false;
            group.CreatedAt = DateTime.Now;
            return Create(group);
        }

        public Group UpdateObject(Group group)
        {
            group.UpdatedAt = DateTime.Now;
            Update(group);
            return group;
        }

        public Group SoftDeleteObject(Group group)
        {
            group.IsDeleted = true;
            group.DeletedAt = DateTime.Now;
            Update(group);
            return group;
        }

        public bool DeleteObject(int Id)
        {
            Group group = Find(x => x.Id == Id);
            return (Delete(group) == 1) ? true : false;
        }
    }
}
