using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class BarringOrderDetailRepository : EfRepository<BarringOrderDetail>, IBarringOrderDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BarringOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<BarringOrderDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<BarringOrderDetail> GetObjectsByBarringOrderId(int barringOrderId)
        {
            return FindAll(x => x.BarringOrderId == barringOrderId && !x.IsDeleted).ToList();
        }

        public IList<BarringOrderDetail> GetObjectsByBarringId(int barringId)
        {
            return FindAll(x => x.BarringId == barringId && !x.IsDeleted).ToList();
        }

        public BarringOrderDetail GetObjectById(int Id)
        {
            BarringOrderDetail barringOrderDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (barringOrderDetail != null) { barringOrderDetail.Errors = new Dictionary<string, string>(); }
            return barringOrderDetail;
        }

        public BarringOrderDetail CreateObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.HasLeftBar = false;
            barringOrderDetail.HasRightBar = false;
            barringOrderDetail.IsCut = false;
            barringOrderDetail.IsSideSealed = false;
            barringOrderDetail.IsBarPrepared = false;
            barringOrderDetail.IsAdhesiveTapeApplied = false;
            barringOrderDetail.IsBarMounted = false;
            barringOrderDetail.IsBarHeatPressed = false;
            barringOrderDetail.IsBarPullOffTested = false;
            barringOrderDetail.IsQCAndMarked = false;
            barringOrderDetail.IsPackaged = false;
            barringOrderDetail.IsRejected = false;
            barringOrderDetail.IsDeleted = false;
            barringOrderDetail.CreatedAt = DateTime.Now;
            return Create(barringOrderDetail);
        }

        public BarringOrderDetail UpdateObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail SoftDeleteObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsDeleted = true;
            barringOrderDetail.DeletedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail SetJobScheduled(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsJobScheduled = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail UnsetJobScheduled(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsJobScheduled = false;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail AddRightBar(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.HasRightBar = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail RemoveRightBar(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.HasRightBar = false;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail AddLeftBar(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.HasLeftBar = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail RemoveLeftBar(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.HasLeftBar = false;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail CutObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsCut = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail SideSealObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsSideSealed = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail PrepareObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsBarPrepared = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail ApplyTapeAdhesiveToObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsAdhesiveTapeApplied = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail MountObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsBarMounted = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail HeatPressObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsBarHeatPressed = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }
        
        public BarringOrderDetail PullOffTestObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsBarPullOffTested = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail QCAndMarkObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsQCAndMarked = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail PackageObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsPackaged = true;
            barringOrderDetail.UpdatedAt = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail RejectObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsRejected = true;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail UndoRejectObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsRejected = false;
            barringOrderDetail.RejectedDate = null;
            UpdateObject(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail FinishObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsFinished = true;
            barringOrderDetail.FinishedDate = DateTime.Now;
            Update(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail UnfinishObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.IsFinished = false;
            barringOrderDetail.FinishedDate = null;
            UpdateObject(barringOrderDetail);
            return barringOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            BarringOrderDetail barringOrderDetail = Find(x => x.Id == Id);
            return (Delete(barringOrderDetail) == 1) ? true : false;
        }
    }
}