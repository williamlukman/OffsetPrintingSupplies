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
    public class BarringRepository : EfRepository<Barring>, IBarringRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BarringRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Barring> GetQueryable()
        {
            return FindAll();
        }

        public IList<Barring> GetAll()
        {
            return (from x in Context.Items.OfType<Barring>() where !x.IsDeleted select x).ToList();
        }

        public IList<Barring> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return (from x in Context.Items.OfType<Barring>() where x.ItemTypeId == ItemTypeId && !x.IsDeleted select x).ToList();
        }

        public IList<Barring> GetObjectsByUoMId(int UoMId)
        {
            return (from x in Context.Items.OfType<Barring>() where x.UoMId == UoMId && !x.IsDeleted select x).ToList();
        }

        public IList<Barring> GetObjectsByContactId(int ContactId)
        {
            return (from x in Context.Items.OfType<Barring>() where x.ContactId == ContactId && !x.IsDeleted select x).ToList();
        }

        public IList<Barring> GetObjectsByMachineId(int MachineId)
        {
            return (from x in Context.Items.OfType<Barring>() where x.MachineId == MachineId && !x.IsDeleted select x).ToList();
        }

        public IList<Barring> GetObjectsByBlanketItemId(int blanketItemId)
        {
            return (from x in Context.Items.OfType<Barring>() where x.BlanketItemId == blanketItemId && !x.IsDeleted select x).ToList();
        }

        public IList<Barring> GetObjectsByLeftBarItemId(int leftBarItemId)
        {
            return (from x in Context.Items.OfType<Barring>() where x.LeftBarItemId == leftBarItemId && !x.IsDeleted select x).ToList();
        }

        public IList<Barring> GetObjectsByRightBarItemId(int rightBarItemId)
        {
            return (from x in Context.Items.OfType<Barring>() where x.RightBarItemId == rightBarItemId && !x.IsDeleted select x).ToList();
        }

        public Item GetBlanketItem(Barring barring)
        {
            using (var db = GetContext())
            {
                Item blanket =
                    (from obj in db.Items
                     where obj.Id == barring.BlanketItemId && !obj.IsDeleted
                     select obj).First();
                if (blanket != null) { blanket.Errors = new Dictionary<string, string>(); }
                return blanket;
            }
        }

        public Item GetLeftBarItem(Barring barring)
        {
            using (var db = GetContext())
            {
                Item leftbar =
                    (from obj in db.Items
                     where obj.Id == barring.LeftBarItemId && !obj.IsDeleted
                     select obj).First();
                if (leftbar != null) { leftbar.Errors = new Dictionary<string, string>(); }
                return leftbar;
            }
        }

        public Item GetRightBarItem(Barring barring)
        {
            using (var db = GetContext())
            {
                Item rightbar =
                    (from obj in db.Items
                     where obj.Id == barring.RightBarItemId && !obj.IsDeleted
                     select obj).First();
                if (rightbar != null) { rightbar.Errors = new Dictionary<string, string>(); }
                return rightbar;
            }
        }

        public Barring GetObjectById(int Id)
        {
            Barring barring = (from x in Context.Items.OfType<Barring>() where x.Id == Id && !x.IsDeleted select x).FirstOrDefault();
            if (barring != null) { barring.Errors = new Dictionary<string, string>(); }
            return barring;
        }

        public Barring CreateObject(Barring barring)
        {
            barring.Quantity = 0;
            barring.IsDeleted = false;
            barring.CreatedAt = DateTime.Now;
            return Create(barring);
        }

        public Barring UpdateObject(Barring barring)
        {
            barring.UpdatedAt = DateTime.Now;
            Update(barring);
            return barring;
        }

        public Barring SoftDeleteObject(Barring barring)
        {
            barring.IsDeleted = true;
            barring.DeletedAt = DateTime.Now;
            Update(barring);
            return barring;
        }

        public bool DeleteObject(int Id)
        {
            Barring barring = (from x in Context.Items.OfType<Barring>() where x.Id == Id select x).FirstOrDefault();
            return (Delete(barring) == 1) ? true : false;
        }

        public Barring GetObjectBySku(string Sku)
        {
            return (from x in Context.Items.OfType<Barring>() where x.Sku == Sku && !x.IsDeleted select x).FirstOrDefault();
        }

        public bool IsSkuDuplicated(Barring barring)
        {
            IQueryable<Barring> items = (from x in Context.Items.OfType<Barring>() where x.Sku == barring.Sku && !x.IsDeleted && x.Id != barring.Id select x);
            return (items.Count() > 0 ? true : false);
        }
    }
}