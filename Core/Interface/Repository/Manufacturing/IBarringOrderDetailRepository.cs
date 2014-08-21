using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBarringOrderDetailRepository : IRepository<BarringOrderDetail>
    {
        IQueryable<BarringOrderDetail> GetQueryable();
        IList<BarringOrderDetail> GetAll();
        IList<BarringOrderDetail> GetObjectsByBarringOrderId(int barringOrderId);
        IList<BarringOrderDetail> GetObjectsByBarringId(int barringId);
        BarringOrderDetail GetObjectById(int Id);
        BarringOrderDetail CreateObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail UpdateObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail SoftDeleteObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail SetJobScheduled(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail UnsetJobScheduled(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail CutObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail SideSealObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail PrepareObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail ApplyTapeAdhesiveToObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail MountObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail HeatPressObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail PullOffTestObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail QCAndMarkObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail PackageObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail RejectObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail UndoRejectObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail FinishObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail UnfinishObject(BarringOrderDetail barringOrderDetail);
        bool DeleteObject(int Id);
    }
}