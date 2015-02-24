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
using Core.Constants;
using System.Web.Script.Serialization;
using System.Data.Objects;
using Data.Context;

namespace WebView.Controllers
{
    public class MstItemController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ItemController");
        private IItemService _itemService;
        private IItemTypeService _itemTypeService;
        private ISubTypeService _subTypeService;
        private IUoMService _uoMService;
        private IWarehouseItemService _warehouseItemService;
        private IWarehouseService _warehouseService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IContactService _contactService;
        private IPriceMutationService _priceMutationService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private IMachineService _machineService;
        public ICurrencyService _currencyService;

        public MstItemController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(),new ItemTypeValidator());
            _subTypeService = new SubTypeService(new SubTypeRepository(), new SubTypeValidator());
            _uoMService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(),new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(),new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(),new SalesOrderDetailValidator());
            _machineService = new MachineService(new MachineRepository(),new MachineValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
        }

        public ActionResult Index()
        {
            return View(this);
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _itemService.GetQueryable().Include("ItemType").Include("UoM").Include("Currency").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Sku,
                             model.Name,
                             model.Quantity,
                             model.PendingReceival,
                             model.PendingDelivery,
                             model.MinimumQuantity,
                             model.Virtual,
                             model.CustomerQuantity,
                             model.UoMId,
                             UoM = model.UoM.Name,
                             model.SellingPrice,
                             model.CurrencyId,
                             Currency = model.Currency.Name,
                             model.PriceList,
                             model.AvgPrice,
                             model.CustomerAvgPrice,
                             model.Description,
                             model.ItemTypeId,
                             ItemType = model.ItemType.Name,
                             model.SubTypeId,
                             SubType = model.SubTypeId != null ? model.SubType.Name : "",
                             model.IsTradeable,
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
                            model.Sku,
                            model.Name,
                            model.Quantity,
                            model.PendingReceival,
                            model.PendingDelivery,
                            model.MinimumQuantity,
                            model.Virtual,
                            model.CustomerQuantity,
                            model.UoM,
                            model.SellingPrice,
                            model.CurrencyId,
                            model.Currency,
                            model.PriceList,
                            model.AvgPrice,
                            model.CustomerAvgPrice,
                            model.Description,
                            model.ItemType,
                            model.SubType,
                            model.IsTradeable,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListChemical(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _itemService.GetQueryable().Where(x => !x.IsDeleted && x.ItemType.Name == Constant.ItemTypeCase.Chemical);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Sku,
                             model.Name,
                             model.Quantity,
                             model.PendingReceival,
                             model.PendingDelivery,
                             model.MinimumQuantity,
                             model.Virtual,
                             model.CustomerQuantity,
                             model.UoMId,
                             UoM = model.UoM.Name,
                             model.SellingPrice,
                             model.CurrencyId,
                             Currency = model.Currency.Name,
                             model.PriceList,
                             model.AvgPrice,
                             model.CustomerAvgPrice,
                             model.Description,
                             model.ItemTypeId,
                             ItemType = model.ItemType.Name,
                             model.SubTypeId,
                             SubType = model.SubTypeId != null ? model.SubType.Name : "",
                             model.IsTradeable,
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
                            model.Sku,
                            model.Name,
                            model.Quantity,
                            model.PendingReceival,
                            model.PendingDelivery,
                            model.MinimumQuantity,
                            model.Virtual,
                            model.CustomerQuantity,
                            model.UoM,
                            model.SellingPrice,
                            model.CurrencyId,
                            model.Currency,
                            model.PriceList,
                            model.AvgPrice,
                            model.CustomerAvgPrice,
                            model.Description,
                            model.ItemType,
                            model.SubType,
                            model.IsTradeable,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListTradeable(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _itemService.GetQueryable().Include("ItemType").Include("UoM").Where(x => x.IsTradeable && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Sku,
                             model.Name,
                             model.Quantity,
                             model.PendingReceival,
                             model.PendingDelivery,
                             model.MinimumQuantity,
                             model.Virtual,
                             model.UoMId,
                             UoM = model.UoM.Name,
                             model.SellingPrice,
                             model.CurrencyId,
                             Currency = model.Currency.Name,
                             model.PriceList,
                             model.AvgPrice,
                             model.Description,
                             model.ItemTypeId,
                             ItemType = model.ItemType.Name,
                             model.SubTypeId,
                             SubType = model.SubTypeId != null ? model.SubType.Name : "",
                             model.IsTradeable,
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
                            model.Sku,
                            model.Name,
                            model.Quantity,
                            model.PendingReceival,
                            model.PendingDelivery,
                            model.MinimumQuantity,
                            model.Virtual,
                            model.UoM,
                            model.SellingPrice,
                            model.CurrencyId,
                            model.Currency,
                            model.PriceList,
                            model.AvgPrice,
                            model.Description,
                            model.ItemType,
                            model.SubType,
                            model.IsTradeable,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetLookUpUsedRoller(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            int ItemTypeRollerId = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Roller).Id;
            var q = _itemService.GetQueryable().Include("ItemType").Include("UoM")
                    .Where(x => x.ItemTypeId == ItemTypeRollerId && !x.IsDeleted && x.Sku.EndsWith("U")); 

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Sku,
                             model.Name,
                             model.Quantity,
                             model.PendingReceival,
                             model.PendingDelivery,
                             model.MinimumQuantity,
                             model.Virtual,
                             model.UoMId,
                             UoM = model.UoM.Name,
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
                            model.Sku,
                            model.Name,
                            model.Quantity,
                            model.PendingReceival,
                            model.PendingDelivery,
                            model.MinimumQuantity,
                            model.Virtual,
                            model.UoM,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListAccessory(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _itemService.GetQueryable().Include("ItemType").Include("UoM")
                                .Where(x => x.ItemType.Name == Core.Constants.Constant.ItemTypeCase.Accessory && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Sku,
                             model.Name,
                             model.Quantity,
                             model.PendingReceival,
                             model.PendingDelivery,
                             model.MinimumQuantity,
                             model.Virtual,
                             UoM = model.UoM.Name,
                             model.SellingPrice,
                             model.CurrencyId,
                             Currency = model.Currency.Name,
                             model.PriceList,
                             model.AvgPrice,
                             model.Description,
                             ItemType = model.ItemType.Name,
                             SubType = model.SubTypeId != null ? model.SubType.Name : "",
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
                            model.Sku,
                            model.Name,
                            model.Quantity,
                            model.PendingReceival,
                            model.PendingDelivery,
                            model.MinimumQuantity,
                            model.UoM,
                            model.SellingPrice,
                            model.CurrencyId,
                            model.Currency,
                            model.PriceList,
                            model.AvgPrice,
                            model.Description,
                            model.ItemType,
                            model.SubType,
                            model.CreatedAt,
                            model.UpdatedAt,
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
                model.SubTypeId,
                SubType = model.SubTypeId != null ? _subTypeService.GetObjectById(model.SubTypeId.GetValueOrDefault()).Name : "",
                model.Quantity,
                model.SellingPrice,
                model.CurrencyId,
                Currency = model.CurrencyId == null ? "" : model.Currency.Name,
                model.PriceList,
                model.PendingDelivery,
                model.PendingReceival,
                model.MinimumQuantity,
                model.Virtual,
                model.Description,
                model.IsTradeable,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(Item model)
        {
            try
            {

                model = _itemService.CreateObject(model,_uoMService,_itemTypeService,_warehouseItemService,
                    _warehouseService,_priceMutationService);
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
                data.Description = model.Description;
                data.UoMId = model.UoMId;
                data.ItemTypeId = model.ItemTypeId;
                data.SubTypeId = model.SubTypeId;
                data.SellingPrice = model.SellingPrice;
                data.CurrencyId = model.CurrencyId;
                data.PriceList = model.PriceList;
                data.MinimumQuantity = model.MinimumQuantity;
                data.IsTradeable = model.IsTradeable;
                model = _itemService.UpdateObject(data,_uoMService,_itemTypeService,_priceMutationService);
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
        public dynamic UpdatePrice(Item model)
        {
            try
            {
                var data = _itemService.GetObjectById(model.Id);
                data.SellingPrice = model.SellingPrice;
                data.PriceList = model.PriceList;
                data.MinimumQuantity = model.MinimumQuantity;
                data.CurrencyId = model.CurrencyId;
                model = _itemService.UpdateLegacyObject(data, _uoMService, _itemTypeService, _warehouseItemService, _warehouseService, _blanketService, _contactService, _machineService, _priceMutationService);
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
                model = _itemService.SoftDeleteObject(data,_stockMutationService,_itemTypeService,
                    _warehouseItemService,_blanketService,_purchaseOrderDetailService,_stockAdjustmentDetailService,
                    _salesOrderDetailService,_priceMutationService);
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