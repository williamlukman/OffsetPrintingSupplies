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
using System.Linq.Dynamic;
using System.Data.Entity;

namespace WebView.Controllers
{
    public class MstWarehouseController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ItemTypeController");
        private IWarehouseService _warehouseService;
        private IWarehouseItemService _warehouseItemService;
        private IItemService _itemService;
        private ICoreIdentificationService _coreIdentificationService;
        private IBlanketOrderService _blanketOrderService;

        public MstWarehouseController()
        {  
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
             _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _blanketOrderService = new BlanketOrderService(new BlanketOrderRepository(), new BlanketOrderValidator());
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
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _warehouseService.GetQueryable().Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.Name,
                             model.Description,
                             model.CreatedAt,
                             model.UpdatedAt
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
                            model.Code,
                            model.Name,
                            model.Description,                           
                            model.CreatedAt,
                            model.UpdatedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

          public dynamic GetInfo(int Id)
          {
              Warehouse model = new Warehouse();
              try
              {
                  model = _warehouseService.GetObjectById(Id);
                  
              }  
              catch (Exception ex)
              {
                  LOG.Error("GetInfo", ex);
                  model.Errors.Add("Generic", "Error" + ex);
              }

              return Json(new
              {
                 model.Id,
                 model.Code,
                 model.Name,
                 model.Description,
                 model.Errors
              }, JsonRequestBehavior.AllowGet);
          }


        [HttpPost]
        public dynamic Insert(Warehouse model)
        {
            try
            {
                model = _warehouseService.CreateObject(model,_warehouseItemService,_itemService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Insert Failed" + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(Warehouse model)
        {
            try
            {
                var data = _warehouseService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.Description = model.Description;
                model = _warehouseService.UpdateObject(data);
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
        public dynamic Delete(Warehouse model)
        {
            try
            {
                var data = _warehouseService.GetObjectById(model.Id);
                model = _warehouseService.SoftDeleteObject(data,_warehouseItemService,_coreIdentificationService,_blanketOrderService);
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

