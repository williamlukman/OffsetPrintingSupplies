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
    public class MstItemController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ItemController");
        private IItemService _itemService;
        private IItemTypeService _itemTypeService;
        private IUoMService _uoMService;
        private IWarehouseItemService _warehouseItemService;
        private IWarehouseService _warehouseService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        public MstItemController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(),new ItemTypeValidator());
            _uoMService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(),new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(),new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
        }

        public ActionResult Index()
        {
            return View();
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _itemService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<Item>;

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
                    from item in list
                    select new
                    {
                        id = item.Id,
                        cell = new object[] {
                            item.Id,
                            item.Name,
                            item.ItemTypeId,
                            _itemTypeService.GetObjectById(item.ItemTypeId).Name,
                            item.Sku,
                            item.Category,
                            item.UoMId,
                            _uoMService.GetObjectById(item.UoMId).Name,
                            item.Quantity,
                            item.PendingReceival,
                            item.PendingDelivery,
                            item.CreatedAt,
                            item.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            Item model = new Item();
            try
            {
             model = _itemService.GetObjectById(Id);
          
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Id,
                model.Sku,
                model.Name,
                model.UoMId,
                UoM = _uoMService.GetObjectById(model.UoMId).Name,
                model.ItemTypeId,
                ItemType = _itemTypeService.GetObjectById(model.ItemTypeId).Name,
                model.Quantity,
                model.PendingDelivery,
                model.PendingReceival,
                model.Category,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(Item model)
        {
            try
            {
                model = _itemService.CreateObject(model,_uoMService,_itemTypeService,_warehouseItemService,_warehouseService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(Item model)
        {
            try
            {
                var data = _itemService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.Sku = model.Sku;
                data.Category = model.Category;
                data.UoMId = model.UoMId;
                data.ItemTypeId = model.ItemTypeId;
                model = _itemService.UpdateObject(data,_uoMService,_itemTypeService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(Item model)
        {
            try
            {
                var data = _itemService.GetObjectById(model.Id);
                model = _itemService.SoftDeleteObject(data,_stockMutationService,_itemTypeService,_warehouseItemService,_barringService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}