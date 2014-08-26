using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface IPriceMutationRepository : IRepository<PriceMutation>
    {
        IQueryable<PriceMutation> GetQueryable();
        IList<PriceMutation> GetAll();
        IList<PriceMutation> GetObjectsByIsActive(bool IsActive, int ExcludePriceMutationId, int ItemId/*, int GroupId*/);
        IList<PriceMutation> GetActiveObjectsByItemId(int ItemId);
        PriceMutation GetObjectById(int Id);
        PriceMutation DeactivateObject(PriceMutation priceMutation);
        PriceMutation CreateObject(PriceMutation priceMutation);
        bool DeleteObject(int Id);
    }
}
