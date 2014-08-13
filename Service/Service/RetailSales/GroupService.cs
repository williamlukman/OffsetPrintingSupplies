using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Service.Service
{
    public class GroupService : IGroupService
    {
        private IGroupRepository _repository;
        private IGroupValidator _validator;
        public GroupService(IGroupRepository _groupRepository, IGroupValidator _groupValidator)
        {
            _repository = _groupRepository;
            _validator = _groupValidator;
        }

        public IGroupValidator GetValidator()
        {
            return _validator;
        }

        public IList<Group> GetAll()
        {
            return _repository.GetAll();
        }

        public Group GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Group GetObjectByIsLegacy(bool IsLegacy)
        {
            return _repository.GetObjectByIsLegacy(IsLegacy);
        }

        public Group GetObjectByName(string name)
        {
            return _repository.GetObjectByName(name);
        }

        public Group CreateObject(string Name, string Description)
        {
            Group group = new Group
            {
                Name = Name,
                Description = Description
            };
            return this.CreateObject(group);
        }

        public Group CreateObject(Group group)
        {
            group.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(group, this) ? _repository.CreateObject(group) : group);
        }

        public Group UpdateObject(Group group)
        {
            return (group = _validator.ValidUpdateObject(group, this) ? _repository.UpdateObject(group) : group);
        }

        public Group SoftDeleteObject(Group group)
        {
            return (group = _validator.ValidDeleteObject(group) ?
                    _repository.SoftDeleteObject(group) : group);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(Group group)
        {
            IQueryable<Group> groups = _repository.FindAll(x => x.Name == group.Name && !x.IsDeleted && x.Id != group.Id);
            return (groups.Count() > 0 ? true : false);
        }
    }
}
