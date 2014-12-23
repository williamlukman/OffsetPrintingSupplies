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
    public class MstItemTypeController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ItemTypeController");
        private IItemTypeService _itemTypeService;
        private IItemService _itemService;
        private IRollerBuilderService _rollerBuilderService;
        private ICoreBuilderService _coreBuilderService;

        public MstItemTypeController()
        {
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(),new ItemTypeValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
        }


        public ActionResult Index()
        {
            return View();
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _itemTypeService.GetQueryable().Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Name,
                             model.Description,
                             model.CreatedAt,
                             model.UpdatedAt,
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
                            model.Id,
                            model.Name,
                            model.Description,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            ItemType model = new ItemType();
            try
            {
                model = _itemTypeService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Id,
                model.Name,
                model.Description,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoByName(string itemType)
        {
            ItemType model = new ItemType();
            string SKU="";
            int SkuCount;
            try
            {
                model = _itemTypeService.GetObjectByName(itemType);
                if (model.Name == "Roller")
                {
                    SkuCount = _rollerBuilderService.GetAll().Count() + 1;
                    while (_rollerBuilderService.GetQueryable().Where(x => x.BaseSku == model.Description + SkuCount).Count() > 0)
                    {
                        SkuCount++;
                    }
                }
                else if (model.Name == "Core")
                {
                    SkuCount = _coreBuilderService.GetAll().Count() + 1;
                    while (_coreBuilderService.GetQueryable().Where(x => x.BaseSku == model.Description + SkuCount).Count() > 0)
                    {
                        SkuCount++;
                    }
                }
                else
                {
                    SkuCount = _itemService.GetQueryable().Where(x => x.ItemTypeId == model.Id).Count() + 1;
                    while (_itemService.GetObjectBySku(model.Description + SkuCount) != null)
                    {
                        SkuCount++;
                    }
                }
                SKU = model.Description + SkuCount;
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Id,
                model.Name,
                model.Description,
                SKU,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(ItemType model)
        {
            try
            {
                model = _itemTypeService.CreateObject(model);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Insert Failed" +  ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(ItemType model)
        {
            try
            {
                var data = _itemTypeService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.Description = model.Description;
                model = _itemTypeService.UpdateObject(data);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Update Failed" + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(ItemType model)
        {
            try
            {
                var data = _itemTypeService.GetObjectById(model.Id);
                model = _itemTypeService.SoftDeleteObject(data,_itemService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Delete Failed" + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}
