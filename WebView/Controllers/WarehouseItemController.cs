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

namespace WebView.Controllers
{
    public class WarehouseItemController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("WarehouseItemController");
        private IWarehouseItemService _warehouseItemService;
        private IWarehouseService _warehouseService;
        private IItemService _itemService;
        private IItemTypeService _itemTypeService;
        private IUoMService _uoMService;

        public WarehouseItemController()
        {
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(),new WarehouseItemValidator());
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
        public dynamic GetListItem(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _warehouseItemService.GetObjectsByWarehouseId(id);

            var list = query as IEnumerable<WarehouseItem>;

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
                            _itemService.GetObjectById(model.ItemId).Sku,
                            _itemService.GetObjectById(model.ItemId).Name,
                            model.Quantity,
                           _uoMService.GetObjectById(_itemService.GetObjectById(model.ItemId).UoMId).Name,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetListWarehouse(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _warehouseItemService.GetObjectsByItemId(id);

            var list = query as IEnumerable<WarehouseItem>;

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
                           _warehouseService.GetObjectById(model.WarehouseId).Code,
                           _warehouseService.GetObjectById(model.WarehouseId).Name,
                           model.Quantity,
                           _uoMService.GetObjectById(_itemService.GetObjectById(model.ItemId).UoMId).Name,                           
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
