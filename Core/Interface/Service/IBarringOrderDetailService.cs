using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IBarringOrderDetailService
    {
        IBarringOrderDetailValidator GetValidator();
        IBarringOrderDetailRepository GetRepository();
        IList<BarringOrderDetail> GetAll();
        IList<BarringOrderDetail> GetObjectsByBarringOrderId(int barringOrderId);
        BarringOrderDetail GetObjectById(int Id);
        Barring GetBarring(BarringOrderDetail barringOrderDetail, IBarringService _barringService);
        BarringOrderDetail CreateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService);
        BarringOrderDetail UpdateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService);
        BarringOrderDetail SoftDeleteObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);
        BarringOrderDetail SetJobScheduled(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail UnsetJobScheduled(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail AddLeftBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService);
        BarringOrderDetail RemoveLeftBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService);
        BarringOrderDetail AddRightBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService);
        BarringOrderDetail RemoveRightBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService);
        BarringOrderDetail CutObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail SideSealObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail PrepareObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail ApplyTapeAdhesiveToObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail MountObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail HeatPressObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail PullOffTestObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail QCAndMarkObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail PackageObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail RejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);
        BarringOrderDetail UndoRejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);
        BarringOrderDetail FinishObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);
        BarringOrderDetail UnfinishObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);
        bool DeleteObject(int Id);
    }
}