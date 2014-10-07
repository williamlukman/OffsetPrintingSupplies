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
    public class SalesInvoiceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SalesInvoiceController");
        private ISalesOrderService _salesOrderService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private ISalesInvoiceService _salesInvoiceService;
        private ISalesInvoiceDetailService _salesInvoiceDetailService;
        private IDeliveryOrderService _deliveryOrderService;
        private IDeliveryOrderDetailService _deliveryOrderDetailService;
        private IReceiptVoucherDetailService _receiptVoucherDetailService;
        private IReceivableService _receivableService;
        private IItemService _itemService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private IServiceCostService _serviceCostService;
        private IRollerBuilderService _rollerBuilderService;

        public SalesInvoiceController()
        {
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _salesInvoiceDetailService = new SalesInvoiceDetailService(new SalesInvoiceDetailRepository(), new SalesInvoiceDetailValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _serviceCostService = new ServiceCostService(new ServiceCostRepository(), new ServiceCostValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
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
            var q = _salesInvoiceService.GetQueryable().Include("DeliveryOrder").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.DeliveryOrderId,
                             DeliveryOrderCode = model.DeliveryOrder.Code,
                             model.Description,
                             model.Discount,
                             model.Tax,
                             model.InvoiceDate,
                             model.DueDate,
                             model.AmountReceivable,
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
                            model.DeliveryOrderId,
                            model.DeliveryOrderCode,
                            model.Description,
                            model.Discount,
                            model.Tax,
                            model.InvoiceDate,
                            model.DueDate,
                            model.AmountReceivable,
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
            var q = _salesInvoiceDetailService.GetQueryable().Include("DeliveryOrderDetail").Include("Item")
                                              .Where(x => x.SalesInvoiceId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.DeliveryOrderDetailId,
                             DeliveryOrderDetailCode = model.DeliveryOrderDetail.Code,
                             model.DeliveryOrderDetail.ItemId,
                             ItemSku = model.DeliveryOrderDetail.Item.Sku,
                             Item = model.DeliveryOrderDetail.Item.Name,
                             model.Quantity,
                             model.Amount,
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
                            model.DeliveryOrderDetailId,
                            model.DeliveryOrderDetailCode,
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                            model.Quantity,
                            model.Amount,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            SalesInvoice model = new SalesInvoice();
            try
            {
                model = _salesInvoiceService.GetObjectById(Id);
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
                model.DeliveryOrderId,
                DeliveryOrder = _deliveryOrderService.GetObjectById(model.DeliveryOrderId).Code,
                model.Discount,
                model.Description,
                model.Tax,
                model.InvoiceDate,
                model.DueDate,
                model.AmountReceivable,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            SalesInvoiceDetail model = new SalesInvoiceDetail();
            try
            {
                model = _salesInvoiceDetailService.GetObjectById(Id);
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
                model.DeliveryOrderDetailId,
                DeliveryOrder = _deliveryOrderDetailService.GetObjectById(model.DeliveryOrderDetailId).Code,
                ItemId = _deliveryOrderDetailService.GetObjectById(model.DeliveryOrderDetailId).ItemId,
                Item = _itemService.GetObjectById(_deliveryOrderDetailService.GetObjectById(model.DeliveryOrderDetailId).ItemId).Name,
                model.Quantity,
                model.Amount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(SalesInvoice model)
        {
            try
            {

                model = _salesInvoiceService.CreateObject(model, _deliveryOrderService);
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
        public dynamic InsertDetail(SalesInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                model = _salesInvoiceDetailService.CreateObject(model, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);
                amount = _salesInvoiceService.GetObjectById(model.SalesInvoiceId).AmountReceivable;
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);

            }

            return Json(new
            {
                model.Errors,
                AmountReceivable = amount
            });
        }

        [HttpPost]
        public dynamic Update(SalesInvoice model)
        {
            try
            {
                var data = _salesInvoiceService.GetObjectById(model.Id);
                data.DeliveryOrderId = model.DeliveryOrderId;
                data.Description = model.Description;
                data.Discount = model.Discount;
                data.Tax = model.Tax;
                data.InvoiceDate = model.InvoiceDate;
                data.DueDate = model.DueDate;
                model = _salesInvoiceService.UpdateObject(data, _deliveryOrderService, _salesInvoiceDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                model.AmountReceivable
            });
        }

        [HttpPost]
        public dynamic Delete(SalesInvoice model)
        {
            try
            {
                var data = _salesInvoiceService.GetObjectById(model.Id);
                model = _salesInvoiceService.SoftDeleteObject(data, _salesInvoiceDetailService);
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
        public dynamic DeleteDetail(SalesInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                var data = _salesInvoiceDetailService.GetObjectById(model.Id);
                model = _salesInvoiceDetailService.SoftDeleteObject(data, _salesInvoiceService);
                amount = _salesInvoiceService.GetObjectById(model.SalesInvoiceId).AmountReceivable;
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                AmountReceivable = amount
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(SalesInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                var data = _salesInvoiceDetailService.GetObjectById(model.Id);
                data.DeliveryOrderDetailId = model.DeliveryOrderDetailId;
                data.Quantity = model.Quantity;
                model = _salesInvoiceDetailService.UpdateObject(data, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);
                amount = _salesInvoiceService.GetObjectById(model.SalesInvoiceId).AmountReceivable;
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                AmountReceivable = amount
            });
        }


        [HttpPost]
        public dynamic Confirm(SalesInvoice model)
        {
            //try
            //{
            var data = _salesInvoiceService.GetObjectById(model.Id);
            model = _salesInvoiceService.ConfirmObject(data, model.ConfirmationDate.Value, _salesInvoiceDetailService, _salesOrderService,
                    _salesOrderDetailService, _deliveryOrderService, _deliveryOrderDetailService, _receivableService, _accountService,
                    _generalLedgerJournalService, _closingService, _serviceCostService, _rollerBuilderService, _itemService);
            //}
            //catch (Exception ex)
            //{
            //    LOG.Error("Confirm Failed", ex);
            //    model.Errors.Add("Generic", "Error : " + ex);
            //}

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnConfirm(SalesInvoice model)
        {
            try
            {

                var data = _salesInvoiceService.GetObjectById(model.Id);
                model = _salesInvoiceService.UnconfirmObject(data, _salesInvoiceDetailService, _deliveryOrderService,
                        _deliveryOrderDetailService, _receiptVoucherDetailService, _receivableService, _accountService,
                        _generalLedgerJournalService, _closingService);
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

