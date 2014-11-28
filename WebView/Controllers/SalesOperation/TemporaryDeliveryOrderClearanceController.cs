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
    public class TemporaryDeliveryOrderClearanceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("TemporaryDeliveryOrderClearanceController");
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
        private ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService;
        private ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService;
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
        private ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService;
        private ITemporaryDeliveryOrderService _temporaryDeliveryOrderService;

        public TemporaryDeliveryOrderClearanceController()
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
            _temporaryDeliveryOrderClearanceService = new TemporaryDeliveryOrderClearanceService(new TemporaryDeliveryOrderClearanceRepository(), new TemporaryDeliveryOrderClearanceValidator());
            _temporaryDeliveryOrderClearanceDetailService = new TemporaryDeliveryOrderClearanceDetailService(new TemporaryDeliveryOrderClearanceDetailRepository(), new TemporaryDeliveryOrderClearanceDetailValidator());
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
            _temporaryDeliveryOrderService = new TemporaryDeliveryOrderService(new TemporaryDeliveryOrderRepository(), new TemporaryDeliveryOrderValidator());
            _temporaryDeliveryOrderDetailService = new TemporaryDeliveryOrderDetailService(new TemporaryDeliveryOrderDetailRepository(), new TemporaryDeliveryOrderDetailValidator());
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
            var q = _temporaryDeliveryOrderClearanceService.GetQueryable().Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.TemporaryDeliveryOrderId,
                             TemporaryDeliveryOrder = model.TemporaryDeliveryOrder.Code,
                             model.IsWasted,
                             model.TotalWastedCoGS,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.ClearanceDate,
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
                            model.TemporaryDeliveryOrderId,
                            model.TemporaryDeliveryOrder,
                            model.IsWasted,
                            model.TotalWastedCoGS,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.ClearanceDate,
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
            var q = _temporaryDeliveryOrderClearanceService.GetQueryable().Where(x => !x.IsDeleted && x.IsConfirmed);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.TemporaryDeliveryOrderId,
                             TemporaryDeliveryOrder = model.TemporaryDeliveryOrder.Code,
                             model.IsWasted,
                             model.TotalWastedCoGS,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.ClearanceDate,
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
                            model.TemporaryDeliveryOrderId,
                            model.TemporaryDeliveryOrder,
                            model.IsWasted,
                            model.TotalWastedCoGS,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.ClearanceDate,
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
            var q = _temporaryDeliveryOrderClearanceDetailService.GetQueryable().Where(x => !x.IsDeleted && x.TemporaryDeliveryOrderClearanceId == id);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.TemporaryDeliveryOrderDetailId,
                             TemporaryDeliveryOrderDetail = model.TemporaryDeliveryOrderDetail.Code,
                             model.TemporaryDeliveryOrderDetail.ItemId,
                             ItemSku = model.TemporaryDeliveryOrderDetail.Item.Sku,
                             Item = model.TemporaryDeliveryOrderDetail.Item.Name,
                             model.Quantity,
                             model.WastedCoGS,
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
                            model.TemporaryDeliveryOrderDetailId,
                            model.TemporaryDeliveryOrderDetail,
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                            model.Quantity,
                            model.WastedCoGS,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            TemporaryDeliveryOrderClearance model = new TemporaryDeliveryOrderClearance();
            try
            {
                model = _temporaryDeliveryOrderClearanceService.GetObjectById(Id);
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
                model.TemporaryDeliveryOrderId,
                TemporaryDeliveryOrder = model.TemporaryDeliveryOrder.Code,
                model.IsWasted,
                model.TotalWastedCoGS,
                model.IsConfirmed,
                model.ConfirmationDate,
                model.ClearanceDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            TemporaryDeliveryOrderClearanceDetail model = new TemporaryDeliveryOrderClearanceDetail();
            try
            {
                model = _temporaryDeliveryOrderClearanceDetailService.GetObjectById(Id);

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
                model.TemporaryDeliveryOrderDetailId,
                TemporaryDeliveryOrderDetail = model.TemporaryDeliveryOrderDetail.Code,
                model.TemporaryDeliveryOrderDetail.ItemId,
                ItemSku = model.TemporaryDeliveryOrderDetail.Item.Sku,
                Item = model.TemporaryDeliveryOrderDetail.Item.Name,
                model.Quantity,
                model.WastedCoGS,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(TemporaryDeliveryOrderClearance model)
        {
            try
            {
                model = _temporaryDeliveryOrderClearanceService.CreateObject(model, _temporaryDeliveryOrderService);
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
        public dynamic InsertDetail(TemporaryDeliveryOrderClearanceDetail model)
        {
            try
            {
                model = _temporaryDeliveryOrderClearanceDetailService.CreateObject(model, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);
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
        public dynamic Update(TemporaryDeliveryOrderClearance model)
        {
            try
            {
                var data = _temporaryDeliveryOrderClearanceService.GetObjectById(model.Id);
                data.TemporaryDeliveryOrderId = model.TemporaryDeliveryOrderId;
                data.ClearanceDate = model.ClearanceDate;
                data.IsWasted = model.IsWasted;
                model = _temporaryDeliveryOrderClearanceService.UpdateObject(data, _temporaryDeliveryOrderService);
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
        public dynamic Delete(TemporaryDeliveryOrderClearance model)
        {
            try
            {
                var data = _temporaryDeliveryOrderClearanceService.GetObjectById(model.Id);
                model = _temporaryDeliveryOrderClearanceService.SoftDeleteObject(data, _temporaryDeliveryOrderClearanceDetailService);
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
        public dynamic DeleteDetail(TemporaryDeliveryOrderClearanceDetail model)
        {
            try
            {
                var data = _temporaryDeliveryOrderClearanceDetailService.GetObjectById(model.Id);
                model = _temporaryDeliveryOrderClearanceDetailService.SoftDeleteObject(data);
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
        public dynamic UpdateDetail(TemporaryDeliveryOrderClearanceDetail model)
        {
            try
            {
                var data = _temporaryDeliveryOrderClearanceDetailService.GetObjectById(model.Id);
                data.TemporaryDeliveryOrderDetailId = model.TemporaryDeliveryOrderDetailId;
                data.Quantity = model.Quantity;
                model = _temporaryDeliveryOrderClearanceDetailService.UpdateObject(data, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);
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
        public dynamic ProcessDetail(TemporaryDeliveryOrderClearanceDetail model)
        {
            try
            {
                var data = _temporaryDeliveryOrderClearanceDetailService.GetObjectById(model.Id);
                data.Quantity = model.Quantity;
                model = _temporaryDeliveryOrderClearanceDetailService.ProcessObject(data);
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
        public dynamic Confirm(TemporaryDeliveryOrderClearance model)
        {
            try
            {
                var data = _temporaryDeliveryOrderClearanceService.GetObjectById(model.Id);
                model = _temporaryDeliveryOrderClearanceService.ConfirmObject(data, model.ConfirmationDate.GetValueOrDefault(), _temporaryDeliveryOrderClearanceDetailService,
                        _stockMutationService, _itemService, _blanketService, _warehouseItemService, _temporaryDeliveryOrderDetailService, 
                        _generalLedgerJournalService, _accountService, _closingService);
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
        public dynamic UnConfirm(TemporaryDeliveryOrderClearance model)
        {
            try
            {

                var data = _temporaryDeliveryOrderClearanceService.GetObjectById(model.Id);
                model = _temporaryDeliveryOrderClearanceService.UnconfirmObject(data, _temporaryDeliveryOrderClearanceDetailService, _stockMutationService, _itemService,
                        _blanketService, _warehouseItemService, _temporaryDeliveryOrderDetailService, _generalLedgerJournalService, _accountService, _closingService);
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
