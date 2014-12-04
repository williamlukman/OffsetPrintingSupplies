using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace WebView.Controllers
{
    public class CustomerItemController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CustomerItemController");
        private ICustomerItemService _customerItemService;
        private IWarehouseItemService _warehouseItemService;
        private IWarehouseService _warehouseService;
        private IItemService _itemService;
        private IItemTypeService _itemTypeService;
        private IUoMService _uoMService;

        public CustomerItemController()
        {
            _customerItemService = new CustomerItemService(new CustomerItemRepository(),new CustomerItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _uoMService = new UoMService(new UoMRepository(), new UoMValidator());
        }

        public ActionResult Index()
        {
            return View();
        }

        // warehouseitem
        public dynamic GetListItem(string _search, long nd, int rows, int? page, string sidx, string sord, int id, int cid, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _customerItemService.GetQueryable().Where(x => !x.IsDeleted && x.ContactId == cid && x.WarehouseItem.WarehouseId == id);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.WarehouseItem.ItemId,
                             ItemSku = model.WarehouseItem.Item.Sku,
                             Item = model.WarehouseItem.Item.Name,
                             model.Quantity,
                             UoM = model.WarehouseItem.Item.UoM.Name,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                            model.Quantity,
                            model.UoM
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListWarehouse(string _search, long nd, int rows, int? page, string sidx, string sord, int id, int cid, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _customerItemService.GetQueryable().Where(x => !x.IsDeleted && x.ContactId == cid && x.WarehouseItem.ItemId == id);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.WarehouseItem.WarehouseId,
                             WarehouseCode = model.WarehouseItem.Warehouse.Code,
                             Warehouse = model.WarehouseItem.Warehouse.Name,
                             model.Quantity,
                             UoM = model.WarehouseItem.Item.UoM.Name,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                           model.WarehouseId,
                           model.WarehouseCode,
                           model.Warehouse,
                           model.Quantity,
                           model.UoM,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
