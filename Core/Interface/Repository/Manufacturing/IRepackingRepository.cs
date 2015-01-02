using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IRepackingRepository : IRepository<Repacking>
    {
        IQueryable<Repacking> GetQueryable();
        IList<Repacking> GetAll();
        IList<Repacking> GetObjectsByBlendingRecipeId(int BlendingRecipeId);
        Repacking GetObjectById(int Id);
        Repacking CreateObject(Repacking repacking);
        Repacking UpdateObject(Repacking repacking);
        Repacking SoftDeleteObject(Repacking repacking);
        Repacking ConfirmObject(Repacking repacking);
        Repacking UnconfirmObject(Repacking repacking);
        Repacking AdjustQuantity(Repacking repacking);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(Repacking repacking);
    }
}