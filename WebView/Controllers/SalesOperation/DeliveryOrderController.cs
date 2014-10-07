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
    public class DeliveryOrderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("DeliveryOrderController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private ISalesOrderService _salesOrderService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private IDeliveryOrderDetailService _deliveryOrderDetailService;
        private IDeliveryOrderService _deliveryOrderService;
        private ISalesInvoiceService _salesInvoiceService;
        private ISalesInvoiceDetailService _salesInvoiceDetailService;
        private IWarehouseService _warehouseService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private IServiceCostService _serviceCostService;

        public DeliveryOrderController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _salesInvoiceDetailService = new SalesInvoiceDetailService(new SalesInvoiceDetailRepository(), new SalesInvoiceDetailValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _serviceCostService = new ServiceCostService(new ServiceCostRepository(), new ServiceCostValidator());
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
            var q = _deliveryOrderService.GetQueryable().Include("SalesOrder").Include("Warehouse").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.SalesOrderId,
                             SalesOrderCode = model.SalesOrder.Code,
                             model.WarehouseId,
                             Warehouse = model.Warehouse.Name,
                             model.DeliveryDate,
                             model.IsConfirmed,
                             model.ConfirmationDate,
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
                            model.Code,
                            model.SalesOrderId,
                            model.SalesOrderCode,
                            model.WarehouseId,
                            model.Warehouse,
                            model.DeliveryDate,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListConfirmed(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _deliveryOrderService.GetQueryable().Include("SalesOrder").Include("Warehouse").Where(x => !x.IsDeleted && x.IsConfirmed);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.SalesOrderId,
                             SalesOrderCode = model.SalesOrder.Code,
                             model.WarehouseId,
                             Warehouse = model.Warehouse.Name,
                             model.DeliveryDate,
                             model.IsConfirmed,
                             model.ConfirmationDate,
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
                            model.Code,
                            model.SalesOrderId,
                            model.SalesOrderCode,
                            model.DeliveryDate,
                            model.WarehouseId,
                            model.Warehouse,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _deliveryOrderDetailService.GetQueryable().Include("SalesOrderDetail").Include("Item").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.SalesOrderDetailId,
                             SalesOrderDetailCode = model.SalesOrderDetail.Code,
                             model.ItemId,
                             ItemSku = model.Item.Sku,
                             Item = model.Item.Name,
                             model.Quantity,
                             Price = model.SalesOrderDetail.Price,
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
                            model.SalesOrderDetailId,
                            model.SalesOrderDetailCode,
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                            model.Quantity,
                            model.Price,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            DeliveryOrder model = new DeliveryOrder();
            try
            {
                model = _deliveryOrderService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.SalesOrderId,
                SalesOrder = _salesOrderService.GetObjectById(model.SalesOrderId).Code,
                model.DeliveryDate,
                model.WarehouseId,
                Warehouse = _warehouseService.GetObjectById(model.WarehouseId).Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            DeliveryOrderDetail model = new DeliveryOrderDetail();
            try
            {
                model = _deliveryOrderDetailService.GetObjectById(Id);

            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.SalesOrderDetailId,
                SalesOrderDetail = _salesOrderDetailService.GetObjectById(model.SalesOrderDetailId).Code,
                model.ItemId,
                ItemSku = _itemService.GetObjectById(model.ItemId).Sku,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                model.Quantity,
                Price = _salesOrderDetailService.GetObjectById(model.SalesOrderDetailId).Price,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(DeliveryOrder model)
        {
            try
            {

                model = _deliveryOrderService.CreateObject(model, _salesOrderService, _warehouseService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic InsertDetail(DeliveryOrderDetail model)
        {
            try
            {
                model = _deliveryOrderDetailService.CreateObject(model, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(DeliveryOrder model)
        {
            try
            {
                var data = _deliveryOrderService.GetObjectById(model.Id);
                data.SalesOrderId = model.SalesOrderId;
                data.DeliveryDate = model.DeliveryDate;
                data.WarehouseId = model.WarehouseId;
                model = _deliveryOrderService.UpdateObject(data, _salesOrderService, _warehouseService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(DeliveryOrder model)
        {
            try
            {
                var data = _deliveryOrderService.GetObjectById(model.Id);
                model = _deliveryOrderService.SoftDeleteObject(data, _deliveryOrderDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic DeleteDetail(DeliveryOrderDetail model)
        {
            try
            {
                var data = _deliveryOrderDetailService.GetObjectById(model.Id);
                model = _deliveryOrderDetailService.SoftDeleteObject(data);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(DeliveryOrderDetail model)
        {
            try
            {
                var data = _deliveryOrderDetailService.GetObjectById(model.Id);
                data.SalesOrderDetailId = model.SalesOrderDetailId;
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                model = _deliveryOrderDetailService.UpdateObject(data, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }


        [HttpPost]
        public dynamic Confirm(DeliveryOrder model)
        {
            try
            {
                var data = _deliveryOrderService.GetObjectById(model.Id);
                model = _deliveryOrderService.ConfirmObject(data, model.ConfirmationDate.Value, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService,
                        _stockMutationService, _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnConfirm(DeliveryOrder model)
        {
            try
            {

                var data = _deliveryOrderService.GetObjectById(model.Id);
                model = _deliveryOrderService.UnconfirmObject(data, _deliveryOrderDetailService, _salesInvoiceService, _salesInvoiceDetailService,
                        _salesOrderService, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService,
                        _accountService, _generalLedgerJournalService, _closingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unconfirm Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }


    }
}