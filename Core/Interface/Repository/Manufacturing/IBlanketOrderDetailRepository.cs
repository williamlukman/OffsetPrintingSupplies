using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBlanketOrderDetailRepository : IRepository<BlanketOrderDetail>
    {
        IQueryable<BlanketOrderDetail> GetQueryable();
        IList<BlanketOrderDetail> GetAll();
        IList<BlanketOrderDetail> GetObjectsByBlanketOrderId(int blanketOrderId);
        IList<BlanketOrderDetail> GetObjectsByBlanketId(int blanketId);
        BlanketOrderDetail GetObjectById(int Id);
        BlanketOrderDetail CreateObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail UpdateObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail SoftDeleteObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail SetJobScheduled(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail UnsetJobScheduled(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail CutObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail SideSealObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail PrepareObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail ApplyTapeAdhesiveToObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail MountObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail HeatPressObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail PullOffTestObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail QCAndMarkObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail PackageObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail RejectObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail UndoRejectObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail FinishObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail UnfinishObject(BlanketOrderDetail blanketOrderDetail);
        bool DeleteObject(int Id);
    }
}