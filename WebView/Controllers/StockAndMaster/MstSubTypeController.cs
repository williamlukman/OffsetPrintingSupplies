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
    public class MstSubTypeController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SubTypeController");
        private ISubTypeService _subTypeService;
        private IItemService _itemService;
        private IItemTypeService _itemTypeService;

        public MstSubTypeController()
        {
            _subTypeService = new SubTypeService(new SubTypeRepository(),new SubTypeValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
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
            var q = _subTypeService.GetQueryable().Where(x => !x.IsDeleted).Include("ItemType");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Name,
                             model.ItemTypeId,
                             ItemType = model.ItemType.Name,
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
                            model.ItemTypeId,
                            model.ItemType,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            SubType model = new SubType();
            try
            {
                model = _subTypeService.GetObjectById(Id);
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
                model.ItemTypeId,
                ItemType = _itemTypeService.GetObjectById(model.ItemTypeId).Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(SubType model)
        {
            try
            {
                model = _subTypeService.CreateObject(model, _itemTypeService);
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
        public dynamic Update(SubType model)
        {
            try
            {
                var data = _subTypeService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.ItemTypeId = model.ItemTypeId;
                model = _subTypeService.UpdateObject(data, _itemTypeService);
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
        public dynamic Delete(SubType model)
        {
            try
            {
                var data = _subTypeService.GetObjectById(model.Id);
                model = _subTypeService.SoftDeleteObject(data,_itemService);
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
