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
    public class BlanketOrderDetailRepository : EfRepository<BlanketOrderDetail>, IBlanketOrderDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BlanketOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<BlanketOrderDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<BlanketOrderDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<BlanketOrderDetail> GetObjectsByBlanketOrderId(int blanketOrderId)
        {
            return FindAll(x => x.BlanketOrderId == blanketOrderId && !x.IsDeleted).ToList();
        }

        public IList<BlanketOrderDetail> GetObjectsByBlanketId(int blanketId)
        {
            return FindAll(x => x.BlanketId == blanketId && !x.IsDeleted).ToList();
        }

        public BlanketOrderDetail GetObjectById(int Id)
        {
            BlanketOrderDetail blanketOrderDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (blanketOrderDetail != null) { blanketOrderDetail.Errors = new Dictionary<string, string>(); }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail CreateObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsCut = false;
            blanketOrderDetail.IsSideSealed = false;
            blanketOrderDetail.IsBarPrepared = false;
            blanketOrderDetail.IsAdhesiveTapeApplied = false;
            blanketOrderDetail.IsBarMounted = false;
            blanketOrderDetail.IsBarHeatPressed = false;
            blanketOrderDetail.IsBarPullOffTested = false;
            blanketOrderDetail.IsQCAndMarked = false;
            blanketOrderDetail.IsPackaged = false;
            blanketOrderDetail.IsRejected = false;
            blanketOrderDetail.IsDeleted = false;
            blanketOrderDetail.CreatedAt = DateTime.Now;
            return Create(blanketOrderDetail);
        }

        public BlanketOrderDetail UpdateObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail SoftDeleteObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsDeleted = true;
            blanketOrderDetail.DeletedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail SetJobScheduled(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsJobScheduled = true;
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail UnsetJobScheduled(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsJobScheduled = false;
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail CutObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsCut = true;
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail SideSealObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsSideSealed = true;
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail PrepareObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsBarPrepared = true;
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail ApplyTapeAdhesiveToObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsAdhesiveTapeApplied = true;
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail MountObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsBarMounted = true;
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail HeatPressObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsBarHeatPressed = true;
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }
        
        public BlanketOrderDetail PullOffTestObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsBarPullOffTested = true;
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail QCAndMarkObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsQCAndMarked = true;
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail PackageObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsPackaged = true;
            blanketOrderDetail.UpdatedAt = DateTime.Now;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail RejectObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsRejected = true;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail UndoRejectObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsRejected = false;
            blanketOrderDetail.RejectedDate = null;
            UpdateObject(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail FinishObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsFinished = true;
            Update(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail UnfinishObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.IsFinished = false;
            blanketOrderDetail.FinishedDate = null;
            UpdateObject(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            BlanketOrderDetail blanketOrderDetail = Find(x => x.Id == Id);
            return (Delete(blanketOrderDetail) == 1) ? true : false;
        }
    }
}