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
    public class TemporaryDeliveryOrderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("TemporaryDeliveryOrderController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private ISalesOrderService _salesOrderService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private IDeliveryOrderService _deliveryOrderService;
        private IDeliveryOrderDetailService _deliveryOrderDetailService;
        private IVirtualOrderDetailService _virtualOrderDetailService;
        private IVirtualOrderService _virtualOrderService;
        private ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService;
        private ITemporaryDeliveryOrderService _temporaryDeliveryOrderService;
        private ISalesInvoiceService _salesInvoiceService;
        private ISalesInvoiceDetailService _salesInvoiceDetailService;
        private IWarehouseService _warehouseService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private IServiceCostService _serviceCostService;
        private IContactService _contactService;
        private ISalesQuotationDetailService _salesQuotationDetailService;
        private ISalesQuotationService _salesQuotationService;
        private ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService;
        public ICurrencyService _currencyService;

        public TemporaryDeliveryOrderController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _virtualOrderDetailService = new VirtualOrderDetailService(new VirtualOrderDetailRepository(), new VirtualOrderDetailValidator());
            _virtualOrderService = new VirtualOrderService(new VirtualOrderRepository(), new VirtualOrderValidator());
            _temporaryDeliveryOrderService = new TemporaryDeliveryOrderService(new TemporaryDeliveryOrderRepository(), new TemporaryDeliveryOrderValidator());
            _temporaryDeliveryOrderDetailService = new TemporaryDeliveryOrderDetailService(new TemporaryDeliveryOrderDetailRepository(), new TemporaryDeliveryOrderDetailValidator());
            _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _salesInvoiceDetailService = new SalesInvoiceDetailService(new SalesInvoiceDetailRepository(), new SalesInvoiceDetailValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _serviceCostService = new ServiceCostService(new ServiceCostRepository(), new ServiceCostValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _salesQuotationService = new SalesQuotationService(new SalesQuotationRepository(), new SalesQuotationValidator());
            _salesQuotationDetailService = new SalesQuotationDetailService(new SalesQuotationDetailRepository(), new SalesQuotationDetailValidator());
            _temporaryDeliveryOrderClearanceService = new TemporaryDeliveryOrderClearanceService(new TemporaryDeliveryOrderClearanceRepository(), new TemporaryDeliveryOrderClearanceValidator());
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
            var q = _temporaryDeliveryOrderService.GetQueryable().Include("VirtualOrder").Include("Warehouse")
                                                                 .Include("DeliveryOrder").Include("SalesOrder").Include("Contact")
                                                                 .Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             Contact = (model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.DeliveryOrder.SalesOrder.Contact.Name : model.VirtualOrder.Contact.Name,
                             model.NomorSurat,
                             model.OrderType,
                             model.VirtualOrderId,
                             VirtualOrderCode = model.VirtualOrder.Code,
                             model.DeliveryOrderId,
                             DeliveryOrderCode = model.DeliveryOrder.Code,
                             OrderId = (model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.DeliveryOrderId : model.VirtualOrderId,
                             OrderCode = (model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.DeliveryOrder.Code : model.VirtualOrder.Code,
                             model.WarehouseId,
                             Warehouse = model.Warehouse.Name,
                             model.DeliveryDate,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.IsReconciled,
                             model.IsPushed,
                             model.PushDate,
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
                            model.Contact,
                            model.NomorSurat,
                            model.OrderType,
                            model.OrderId,
                            model.OrderCode,
                            model.WarehouseId,
                            model.Warehouse,
                            model.DeliveryDate,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.IsReconciled,
                            model.IsPushed,
                            model.PushDate,
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
            var q = _temporaryDeliveryOrderService.GetQueryable().Include("SalesOrder").Include("Warehouse")
                                                  .Include("VirtualOrder").Include("Contact").Include("DeliveryOrder")
                                                  .Where(x => !x.IsDeleted && x.IsConfirmed);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             Contact = (model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.DeliveryOrder.SalesOrder.Contact.Name : model.VirtualOrder.Contact.Name,
                             model.NomorSurat,
                             model.OrderType,
                             model.VirtualOrderId,
                             VirtualOrderCode = model.VirtualOrder.Code,
                             model.DeliveryOrderId,
                             DeliveryOrderCode = model.DeliveryOrder.Code,
                             OrderId = (model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.DeliveryOrderId : model.VirtualOrderId,
                             OrderCode = (model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.DeliveryOrder.Code : model.VirtualOrder.Code,
                             model.WarehouseId,
                             Warehouse = model.Warehouse.Name,
                             model.DeliveryDate,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.IsReconciled,
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
                            model.Contact,
                            model.NomorSurat,
                            model.OrderType,
                            model.OrderId,
                            model.OrderCode,
                            model.WarehouseId,
                            model.Warehouse,
                            model.DeliveryDate,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.IsReconciled,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListConfirmedForNonPartDeliveryOrder(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _temporaryDeliveryOrderService.GetQueryable().Include("SalesOrder").Include("Warehouse")
                                                  .Include("VirtualOrder").Include("Contact").Include("DeliveryOrder")
                                                  .Where(x => !x.IsDeleted && x.IsConfirmed && x.OrderType != Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             Contact = (model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.DeliveryOrder.SalesOrder.Contact.Name : model.VirtualOrder.Contact.Name,
                             model.NomorSurat,
                             model.OrderType,
                             model.VirtualOrderId,
                             VirtualOrderCode = model.VirtualOrder.Code,
                             model.DeliveryOrderId,
                             DeliveryOrderCode = model.DeliveryOrder.Code,
                             OrderId = (model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.DeliveryOrderId : model.VirtualOrderId,
                             OrderCode = (model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.DeliveryOrder.Code : model.VirtualOrder.Code,
                             model.WarehouseId,
                             Warehouse = model.Warehouse.Name,
                             model.DeliveryDate,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.IsReconciled,
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
                            model.NomorSurat,
                            model.OrderType,
                            model.OrderId,
                            model.OrderCode,
                            model.WarehouseId,
                            model.Warehouse,
                            model.DeliveryDate,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.IsReconciled,
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
            var q = _temporaryDeliveryOrderDetailService.GetQueryable().Include("TemporaryDeliveryOrder").Include("VirtualOrderDetail")
                                                                       .Include("SalesOrderDetail").Include("Item").Where(x => !x.IsDeleted && x.TemporaryDeliveryOrderId == id);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.TemporaryDeliveryOrder.OrderType,
                             model.VirtualOrderDetailId,
                             VirtualOrderDetailCode = model.VirtualOrderDetail.Code,
                             model.SalesOrderDetailId,
                             SalesOrderDetailCode = model.SalesOrderDetail.Code,
                             OrderDetailId = (model.TemporaryDeliveryOrder.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.SalesOrderDetailId : model.VirtualOrderDetailId,
                             OrderDetailCode = (model.TemporaryDeliveryOrder.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.SalesOrderDetail.Code : model.VirtualOrderDetail.Code,
                             model.ItemId,
                             ItemSku = model.Item.Sku,
                             Item = model.Item.Name,
                             model.Quantity,
                             model.RestockQuantity,
                             model.WasteQuantity,
                             Price = (model.TemporaryDeliveryOrder.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.SalesOrderDetail.Price : model.SellingPrice,
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
                            model.OrderDetailId,
                            model.OrderDetailCode,
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                            model.Quantity,
                            model.RestockQuantity,
                            model.WasteQuantity,
                            model.Price,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            TemporaryDeliveryOrder model = new TemporaryDeliveryOrder();
            try
            {
                model = _temporaryDeliveryOrderService.GetObjectById(Id);
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
                Contact = (model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder) ? model.DeliveryOrder.SalesOrder.Contact.Name : model.VirtualOrder.Contact.Name,
                model.NomorSurat,
                model.OrderType,
                OrderId = model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder ?
                          model.DeliveryOrderId : model.VirtualOrderId,
                OrderCode = model.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder ?
                          _deliveryOrderService.GetObjectById((int) model.DeliveryOrderId).Code : _virtualOrderService.GetObjectById((int) model.VirtualOrderId).Code,
                model.DeliveryDate,
                model.WarehouseId,
                Warehouse = _warehouseService.GetObjectById(model.WarehouseId).Name,
                ConfirmationDate = model.ConfirmationDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            TemporaryDeliveryOrderDetail model = new TemporaryDeliveryOrderDetail();
            try
            {
                model = _temporaryDeliveryOrderDetailService.GetObjectById(Id);

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
                OrderDetailId = _temporaryDeliveryOrderService.GetObjectById(model.TemporaryDeliveryOrderId).OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder ?
                                model.SalesOrderDetailId : model.VirtualOrderDetailId,
                OrderDetailCode = _temporaryDeliveryOrderService.GetObjectById(model.TemporaryDeliveryOrderId).OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder ?
                                _salesOrderDetailService.GetObjectById((int) model.SalesOrderDetailId).Code : _virtualOrderDetailService.GetObjectById((int) model.VirtualOrderDetailId).Code,
                model.ItemId,
                ItemSku = _itemService.GetObjectById(model.ItemId).Sku,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                model.Quantity,
                model.RestockQuantity,
                model.WasteQuantity,
                Price = _temporaryDeliveryOrderService.GetObjectById(model.TemporaryDeliveryOrderId).OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder ?
                        _salesOrderDetailService.GetObjectById((int) model.SalesOrderDetailId).Price : model.SellingPrice,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(TemporaryDeliveryOrder model)
        {
            try
            {
                model = _temporaryDeliveryOrderService.CreateObject(model, _virtualOrderService, _deliveryOrderService, _warehouseService);
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
        public dynamic InsertDetail(TemporaryDeliveryOrderDetail model)
        {
            try
            {
                model = _temporaryDeliveryOrderDetailService.CreateObject(model, _temporaryDeliveryOrderService, _virtualOrderDetailService, _salesOrderDetailService, _deliveryOrderService, _itemService);
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
        public dynamic Update(TemporaryDeliveryOrder model)
        {
            try
            {
                var data = _temporaryDeliveryOrderService.GetObjectById(model.Id);
                data.VirtualOrderId = model.VirtualOrderId;
                data.DeliveryOrderId = model.DeliveryOrderId;
                data.DeliveryDate = model.DeliveryDate;
                data.WarehouseId = model.WarehouseId;
                data.NomorSurat = model.NomorSurat;
                model = _temporaryDeliveryOrderService.UpdateObject(data, _virtualOrderService, _deliveryOrderService, _warehouseService);
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
        public dynamic Delete(TemporaryDeliveryOrder model)
        {
            try
            {
                var data = _temporaryDeliveryOrderService.GetObjectById(model.Id);
                model = _temporaryDeliveryOrderService.SoftDeleteObject(data, _temporaryDeliveryOrderDetailService);
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
        public dynamic DeleteDetail(TemporaryDeliveryOrderDetail model)
        {
            try
            {
                var data = _temporaryDeliveryOrderDetailService.GetObjectById(model.Id);
                model = _temporaryDeliveryOrderDetailService.SoftDeleteObject(data);
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
        public dynamic UpdateDetail(TemporaryDeliveryOrderDetail model)
        {
            try
            {
                var data = _temporaryDeliveryOrderDetailService.GetObjectById(model.Id);
                data.SalesOrderDetailId = model.SalesOrderDetailId;
                data.VirtualOrderDetailId = model.VirtualOrderDetailId;
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                data.WasteQuantity = model.WasteQuantity;
                data.RestockQuantity = model.RestockQuantity;
                model = _temporaryDeliveryOrderDetailService.UpdateObject(data, _temporaryDeliveryOrderService, _virtualOrderDetailService, 
                                                                          _salesOrderDetailService, _deliveryOrderService, _itemService);
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
        public dynamic ProcessDetail(TemporaryDeliveryOrderDetail model)
        {
            try
            {
                var data = _temporaryDeliveryOrderDetailService.GetObjectById(model.Id);
                data.WasteQuantity = model.WasteQuantity;
                data.RestockQuantity = model.RestockQuantity;
                model = _temporaryDeliveryOrderDetailService.ProcessObject(data);
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
        public dynamic Confirm(TemporaryDeliveryOrder model)
        {
            try
            {
                var data = _temporaryDeliveryOrderService.GetObjectById(model.Id);
                model = _temporaryDeliveryOrderService.ConfirmObject(data, model.ConfirmationDate.Value, _temporaryDeliveryOrderDetailService,
                        _virtualOrderService, _virtualOrderDetailService, _deliveryOrderService, _deliveryOrderDetailService, _salesOrderDetailService,
                        _stockMutationService, _itemService, _blanketService, _warehouseItemService);
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
        public dynamic UnConfirm(TemporaryDeliveryOrder model)
        {
            try
            {

                var data = _temporaryDeliveryOrderService.GetObjectById(model.Id);
                model = _temporaryDeliveryOrderService.UnconfirmObject(data, _temporaryDeliveryOrderDetailService, _virtualOrderService, _virtualOrderDetailService,
                        _deliveryOrderService, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService, _itemService,
                        _blanketService, _warehouseItemService, _temporaryDeliveryOrderClearanceService);
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

        [HttpPost]
        public dynamic Push(TemporaryDeliveryOrder model)
        {
            try
            {
                var data = _temporaryDeliveryOrderService.GetObjectById(model.Id);
                model = _temporaryDeliveryOrderService.PushObject(data, model.ConfirmationDate.Value, _temporaryDeliveryOrderDetailService, _virtualOrderService, _virtualOrderDetailService,
                        _salesOrderService, _salesOrderDetailService, _deliveryOrderService, _deliveryOrderDetailService, _itemService, _stockMutationService, _contactService, _blanketService,
                        _warehouseService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService, _salesQuotationService, _salesQuotationDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Push to DO Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}
