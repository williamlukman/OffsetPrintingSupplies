using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class MachineService : IMachineService
    {
        private IMachineRepository _repository;
        private IMachineValidator _validator;
        public MachineService(IMachineRepository _machineRepository, IMachineValidator _machineValidator)
        {
            _repository = _machineRepository;
            _validator = _machineValidator;
        }

        public IMachineValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Machine> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Machine> GetAll()
        {
            return _repository.GetAll();
        }

        public Machine GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Machine GetObjectByCode(string Code)
        {
            return _repository.FindAll(c => c.Code == Code && !c.IsDeleted).FirstOrDefault();
        }

        public Machine GetObjectByName(string name)
        {
            return _repository.FindAll(c => c.Name == name && !c.IsDeleted).FirstOrDefault();
        }

        public Machine CreateObject(string Code, string Name, string Description)
        {
            Machine machine = new Machine
            {
                Code = Code,
                Name = Name,
                Description = Description
            };
            return this.CreateObject(machine);
        }

        public Machine CreateObject(Machine machine)
        {
            machine.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(machine, this) ? _repository.CreateObject(machine) : machine);
        }

        public Machine UpdateObject(Machine machine)
        {
            return (machine = _validator.ValidUpdateObject(machine, this) ? _repository.UpdateObject(machine) : machine);
        }

        public Machine SoftDeleteObject(Machine machine, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService, IBarringService _barringService)
        {
            return (machine = _validator.ValidDeleteObject(machine, _rollerBuilderService, _coreIdentificationDetailService, _barringService) ? _repository.SoftDeleteObject(machine) : machine);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(Machine machine)
        {
            IQueryable<Machine> machines = _repository.FindAll(x => x.Code == machine.Code && !x.IsDeleted && x.Id != machine.Id);
            return (machines.Count() > 0 ? true : false);
        }

    }
}