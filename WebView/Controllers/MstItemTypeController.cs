﻿using System;
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
    public class MstItemTypeController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ItemTypeController");
        private IItemTypeService _itemTypeService;
        private IItemService _itemService;
         
        public MstItemTypeController()
        {
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(),new ItemTypeValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());

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
            var query = _itemTypeService.GetAll().Where(d => d.IsDeleted == false);
            
            var list = query as IEnumerable<ItemType>;

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
                            item.Description,
                            item.CreatedAt,
                            item.UpdatedAt,
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
            try
            {
                model = _itemTypeService.GetObjectByName(itemType);
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
