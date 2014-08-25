using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IPriceMutationService
    {
        IPriceMutationValidator GetValidator();
        IPriceMutationRepository GetRepository();
        IList<PriceMutation> GetAll();
        IList<PriceMutation> GetObjectsByIsActive(bool IsActive, int ItemId/*, int ContactGroupId*/, int ExcludePriceMutationId);
        IList<PriceMutation> GetActiveObjectsByItemId(int ItemId);
        PriceMutation GetObjectById(int Id);
        PriceMutation DeactivateObject(PriceMutation priceMutation, Nullable<DateTime> DeactivationDate);
        PriceMutation CreateObject(int ItemId, /*int ContactGroupId,*/ decimal Price, DateTime CreationDate);
        PriceMutation CreateObject(Item item, /*ContactGroup contactGroup,*/ DateTime CreationDate);
        PriceMutation CreateObject(PriceMutation priceMutation);
        bool DeleteObject(int Id);
    }
}
