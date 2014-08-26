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
            var query = _salesInvoiceService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<SalesInvoice>;

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
                            _deliveryOrderService.GetObjectById(model.DeliveryOrderId).Code,
                            model.Description,
                            model.Discount,
                            model.IsTaxable,
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

            // Get Data
            var query = _salesInvoiceDetailService.GetObjectsBySalesInvoiceId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<SalesInvoiceDetail>;

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
                            model.Code,
                            model.DeliveryOrderDetailId,
                            _deliveryOrderDetailService.GetObjectById(model.DeliveryOrderDetailId).Code,
                            _deliveryOrderDetailService.GetObjectById(model.DeliveryOrderDetailId).ItemId,
                            _itemService.GetObjectById( _deliveryOrderDetailService.GetObjectById(model.DeliveryOrderDetailId).ItemId).Sku,
                            _itemService.GetObjectById( _deliveryOrderDetailService.GetObjectById(model.DeliveryOrderDetailId).ItemId).Name,
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
                model.IsTaxable,
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
                data.IsTaxable = model.IsTaxable;
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
            model = _salesInvoiceService.ConfirmObject(data, model.ConfirmationDate.Value, _salesInvoiceDetailService, _salesOrderService, _deliveryOrderService, _deliveryOrderDetailService, _receivableService);
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
                model = _salesInvoiceService.UnconfirmObject(data, _salesInvoiceDetailService, _deliveryOrderService, _deliveryOrderDetailService, _receiptVoucherDetailService, _receivableService);
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

