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
    public class QuantityPricingController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("QuantityPricingController");
        private IQuantityPricingService _quantityPricingService;
        private IWarehouseService _warehouseService;
        private IItemService _itemService;
        private IItemTypeService _itemTypeService;
        private IUoMService _uoMService;

        public QuantityPricingController()
        {
            _quantityPricingService = new QuantityPricingService(new QuantityPricingRepository(),new QuantityPricingValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _uoMService = new UoMService(new UoMRepository(), new UoMValidator());
        }

        public ActionResult Index()
        {
            return View();
        }

        public dynamic GetInfo(int Id)
        {
            QuantityPricing model = new QuantityPricing();
            try
            {
                model = _quantityPricingService.GetObjectById(Id);

            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Id,
                model.ItemTypeId,
                model.Discount,
                model.MinQuantity,
                model.IsInfiniteMaxQuantity,
                model.MaxQuantity,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _quantityPricingService.GetObjectsByItemTypeId(id);

            var list = query as IEnumerable<QuantityPricing>;

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
                            model.Discount,
                            model.MinQuantity,
                            model.IsInfiniteMaxQuantity,
                            model.MaxQuantity,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(QuantityPricing model)
        {
            try
            {
                model = _quantityPricingService.CreateObject(model, _itemTypeService);
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
        public dynamic Update(QuantityPricing model)
        {
            try
            {
                var data = _quantityPricingService.GetObjectById(model.Id);
                data.Discount = model.Discount;
                data.MinQuantity = model.MinQuantity;
                data.IsInfiniteMaxQuantity = model.IsInfiniteMaxQuantity;
                data.MaxQuantity = model.MaxQuantity;
                model = _quantityPricingService.UpdateObject(data, data.ItemTypeId, _itemTypeService);
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
        public dynamic Delete(QuantityPricing model)
        {
            try
            {
                var data = _quantityPricingService.GetObjectById(model.Id);
                model = _quantityPricingService.SoftDeleteObject(data);
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
