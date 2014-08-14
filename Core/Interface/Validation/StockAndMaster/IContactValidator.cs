using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IContactValidator
    {
        Contact VHasContactGroup(Contact contact, IContactGroupService _contactGroupService);
        Contact VHasUniqueName(Contact contact, IContactService _contactService);
        Contact VHasAddress(Contact contact);
        Contact VHasContactNo(Contact contact);
        Contact VHasPIC(Contact contact);
        Contact VHasPICContactNo(Contact contact);
        Contact VHasEmail(Contact contact);
        Contact VHasCoreIdentification(Contact contact, ICoreIdentificationService _coreIdentificationService);
        Contact VHasBarring(Contact contact, IBarringService _barringService);
        Contact VHasPurchaseOrder(Contact contact, IPurchaseOrderService _purchaseOrderService);
        Contact VHasSalesOrder(Contact contact, ISalesOrderService _salesOrderService);
        Contact VCreateObject(Contact contact, IContactService _contactService, IContactGroupService _contactGroupService);
        Contact VUpdateObject(Contact contact, IContactService _contactService, IContactGroupService _contactGroupService);
        Contact VDeleteObject(Contact contact, ICoreIdentificationService _coreIdentificationService, IBarringService _barringService,
                              IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService);
        bool ValidCreateObject(Contact contact, IContactService _contactService, IContactGroupService _contactGroupService);
        bool ValidUpdateObject(Contact contact, IContactService _contactService, IContactGroupService _contactGroupService);
        bool ValidDeleteObject(Contact contact, ICoreIdentificationService _coreIdentificationService, IBarringService _barringService,
                               IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService);
        bool isValid(Contact contact);
        string PrintError(Contact contact);
    }
}