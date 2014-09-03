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
    public class BlanketRepository : EfRepository<Blanket>, IBlanketRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BlanketRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Blanket> GetQueryable()
        {
            return FindAll();
        }

        public IList<Blanket> GetAll()
        {
            return (from x in Context.Items.OfType<Blanket>() where !x.IsDeleted select x).ToList();
        }

        public IList<Blanket> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return (from x in Context.Items.OfType<Blanket>() where x.ItemTypeId == ItemTypeId && !x.IsDeleted select x).ToList();
        }

        public IList<Blanket> GetObjectsByUoMId(int UoMId)
        {
            return (from x in Context.Items.OfType<Blanket>() where x.UoMId == UoMId && !x.IsDeleted select x).ToList();
        }

        public IList<Blanket> GetObjectsByContactId(int ContactId)
        {
            return (from x in Context.Items.OfType<Blanket>() where x.ContactId == ContactId && !x.IsDeleted select x).ToList();
        }

        public IList<Blanket> GetObjectsByMachineId(int MachineId)
        {
            return (from x in Context.Items.OfType<Blanket>() where x.MachineId == MachineId && !x.IsDeleted select x).ToList();
        }

        public IList<Blanket> GetObjectsByRollBlanketItemId(int rollBlanketItemId)
        {
            return (from x in Context.Items.OfType<Blanket>() where x.RollBlanketItemId == rollBlanketItemId && !x.IsDeleted select x).ToList();
        }

        public IList<Blanket> GetObjectsByLeftBarItemId(int leftBarItemId)
        {
            return (from x in Context.Items.OfType<Blanket>() where x.LeftBarItemId == leftBarItemId && !x.IsDeleted select x).ToList();
        }

        public IList<Blanket> GetObjectsByRightBarItemId(int rightBarItemId)
        {
            return (from x in Context.Items.OfType<Blanket>() where x.RightBarItemId == rightBarItemId && !x.IsDeleted select x).ToList();
        }

        public Item GetRollBlanketItem(Blanket blanket)
        {
            using (var db = GetContext())
            {
                Item rollBlanket =
                    (from obj in db.Items
                     where obj.Id == blanket.RollBlanketItemId && !obj.IsDeleted
                     select obj).First();
                if (rollBlanket != null) { rollBlanket.Errors = new Dictionary<string, string>(); }
                return rollBlanket;
            }
        }

        public Item GetLeftBarItem(Blanket blanket)
        {
            using (var db = GetContext())
            {
                Item leftbar =
                    (from obj in db.Items
                     where obj.Id == blanket.LeftBarItemId && !obj.IsDeleted
                     select obj).First();
                if (leftbar != null) { leftbar.Errors = new Dictionary<string, string>(); }
                return leftbar;
            }
        }

        public Item GetRightBarItem(Blanket blanket)
        {
            using (var db = GetContext())
            {
                Item rightbar =
                    (from obj in db.Items
                     where obj.Id == blanket.RightBarItemId && !obj.IsDeleted
                     select obj).First();
                if (rightbar != null) { rightbar.Errors = new Dictionary<string, string>(); }
                return rightbar;
            }
        }

        public Blanket GetObjectById(int Id)
        {
            Blanket blanket = (from x in Context.Items.OfType<Blanket>() where x.Id == Id && !x.IsDeleted select x).FirstOrDefault();
            if (blanket != null) { blanket.Errors = new Dictionary<string, string>(); }
            return blanket;
        }

        public Blanket CreateObject(Blanket blanket)
        {
            blanket.Quantity = 0;
            blanket.IsDeleted = false;
            blanket.CreatedAt = DateTime.Now;
            return Create(blanket);
        }

        public Blanket UpdateObject(Blanket blanket)
        {
            blanket.UpdatedAt = DateTime.Now;
            Update(blanket);
            return blanket;
        }

        public Blanket SoftDeleteObject(Blanket blanket)
        {
            blanket.IsDeleted = true;
            blanket.DeletedAt = DateTime.Now;
            Update(blanket);
            return blanket;
        }

        public bool DeleteObject(int Id)
        {
            Blanket blanket = (from x in Context.Items.OfType<Blanket>() where x.Id == Id select x).FirstOrDefault();
            return (Delete(blanket) == 1) ? true : false;
        }

        public Blanket GetObjectBySku(string Sku)
        {
            return (from x in Context.Items.OfType<Blanket>() where x.Sku == Sku && !x.IsDeleted select x).FirstOrDefault();
        }

        public bool IsSkuDuplicated(Blanket blanket)
        {
            IQueryable<Blanket> items = (from x in Context.Items.OfType<Blanket>() where x.Sku == blanket.Sku && !x.IsDeleted && x.Id != blanket.Id select x);
            return (items.Count() > 0 ? true : false);
        }
    }
}