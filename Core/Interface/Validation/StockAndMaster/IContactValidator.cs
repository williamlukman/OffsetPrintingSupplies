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
        Contact VHasUniqueName(Contact contact, IContactService _contactService);
        Contact VHasAddress(Contact contact);
        Contact VHasContactNo(Contact contact);
        Contact VHasPIC(Contact contact);
        Contact VHasPICContactNo(Contact contact);
        Contact VHasEmail(Contact contact);
        Contact VHasCoreIdentification(Contact contact, ICoreIdentificationService _coreIdentificationService);
        Contact VHasBlanket(Contact contact, IBlanketService _blanketService);
        Contact VHasPurchaseOrder(Contact contact, IPurchaseOrderService _purchaseOrderService);
        Contact VHasSalesOrder(Contact contact, ISalesOrderService _salesOrderService);
        Contact VHasSalesQuotation(Contact contact, ISalesQuotationService _salesQuotationService);
        Contact VHasVirtualOrder(Contact contact, IVirtualOrderService _virtualOrderService);
        Contact VCreateObject(Contact contact, IContactService _contactService);
        Contact VUpdateObject(Contact contact, IContactService _contactService);
        Contact VDeleteObject(Contact contact, ICoreIdentificationService _coreIdentificationService, IBlanketService _blanketService,
                              IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService,
                              ISalesQuotationService _salesQuotationService, IVirtualOrderService _virtualOrderService);
        bool ValidCreateObject(Contact contact, IContactService _contactService);
        bool ValidUpdateObject(Contact contact, IContactService _contactService);
        bool ValidDeleteObject(Contact contact, ICoreIdentificationService _coreIdentificationService, IBlanketService _blanketService,
                               IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService,
                               ISalesQuotationService _salesQuotationService, IVirtualOrderService _virtualOrderService);
        bool isValid(Contact contact);
        string PrintError(Contact contact);
    }
}