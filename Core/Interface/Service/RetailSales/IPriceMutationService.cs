using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IPriceMutationService
    {
        IPriceMutationValidator GetValidator();
        IList<PriceMutation> GetAll();
        IList<PriceMutation> GetObjectsByIsActive(bool IsActive, int ItemId, int GroupId, int ExcludePriceMutationId);
        IList<PriceMutation> GetActiveObjectsByItemId(int ItemId);
        PriceMutation GetObjectById(int Id);
        PriceMutation DeactivateObject(PriceMutation priceMutation, Nullable<DateTime> DeactivationDate);
        PriceMutation CreateObject(int ItemId, int GroupId, decimal Price, DateTime CreationDate);
        PriceMutation CreateObject(Item item, Group group, DateTime CreationDate);
        PriceMutation CreateObject(PriceMutation priceMutation);
        bool DeleteObject(int Id);
    }
}
