using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IContactService
    {
        IContactValidator GetValidator();
        IList<Contact> GetAll();
        Contact GetObjectById(int Id);
        Contact GetObjectByName(string Name);
        Contact FindOrCreateBaseContact(IContactGroupService _contactGroupService);
        Contact CreateObject(Contact contact, IContactGroupService _contactGroupService);
        Contact CreateObject(string Name, string Address, string ContactNo, string PIC, string PICContactNo, string Email, IContactGroupService _contactGroupService);
        Contact UpdateObject(Contact contact, IContactGroupService _contactGroupService);
        Contact SoftDeleteObject(Contact contact, ICoreIdentificationService _coreIdentificationService, IBarringService _barringService,
                                 IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(Contact contact);
    }
}