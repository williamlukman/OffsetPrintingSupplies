using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class RecoveryOrderDetailRepository : EfRepository<RecoveryOrderDetail>, IRecoveryOrderDetailRepository
    {

        private OffsetPrintingSuppliesEntities entities;
        public RecoveryOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<RecoveryOrderDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<RecoveryOrderDetail> GetObjectsByRecoveryOrderId(int recoveryOrderId)
        {
            return FindAll(x => x.RecoveryOrderId == recoveryOrderId && !x.IsDeleted).ToList();
        }

        public RecoveryOrderDetail GetObjectById(int Id)
        {
            RecoveryOrderDetail recoveryOrderDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (recoveryOrderDetail != null) { recoveryOrderDetail.Errors = new Dictionary<string, string>(); }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail CreateObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.HasAccessory = false;
            recoveryOrderDetail.IsDisassembled = false;
            recoveryOrderDetail.IsStrippedAndGlued = false;
            recoveryOrderDetail.IsWrapped = false;
            recoveryOrderDetail.IsVulcanized = false;
            recoveryOrderDetail.IsFacedOff = false;
            recoveryOrderDetail.IsConventionalGrinded = false;
            recoveryOrderDetail.IsCWCGrinded = false;
            recoveryOrderDetail.IsPolishedAndQC = false;
            recoveryOrderDetail.IsPackaged = false;
            recoveryOrderDetail.IsRejected = false;
            recoveryOrderDetail.IsDeleted = false;
            recoveryOrderDetail.CreatedAt = DateTime.Now;
            return Create(recoveryOrderDetail);
        }

        public RecoveryOrderDetail UpdateObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail SoftDeleteObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.IsDeleted = true;
            recoveryOrderDetail.DeletedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail AddAccessory(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.HasAccessory = true;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail DisassembleObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.IsDisassembled = true;
            recoveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }
        
        public RecoveryOrderDetail StripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.IsStrippedAndGlued = true;
            recoveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }
        
        public RecoveryOrderDetail WrapObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.IsWrapped = true;
            recoveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }
        
        public RecoveryOrderDetail VulcanizeObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.IsVulcanized = true;
            recoveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }
        
        public RecoveryOrderDetail FaceOffObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.IsFacedOff = true;
            recoveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }
        
        public RecoveryOrderDetail ConventionalGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.IsConventionalGrinded = true;
            recoveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }
        
        public RecoveryOrderDetail CWCGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.IsCWCGrinded = true;
            recoveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }
        
        public RecoveryOrderDetail PolishAndQCObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.IsPolishedAndQC = true;
            recoveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail PackageObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.IsPackaged = true;
            recoveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail RejectObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.IsRejected = true;
            recoveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            RecoveryOrderDetail recoveryOrderDetail = Find(x => x.Id == Id);
            return (Delete(recoveryOrderDetail) == 1) ? true : false;
        }
    }
}