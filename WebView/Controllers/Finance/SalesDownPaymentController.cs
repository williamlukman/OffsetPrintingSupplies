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
    public class SalesDownPaymentController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SalesDownPaymentController");
        private ISalesOrderService _salesOrderService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private ISalesInvoiceService _salesInvoiceService;
        private ISalesInvoiceDetailService _salesInvoiceDetailService;
        private IDeliveryOrderService _deliveryOrderService;
        private IDeliveryOrderDetailService _deliveryOrderDetailService;
        private ISalesDownPaymentAllocationService _salesDownPaymentAllocationService;
        private ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService;
        private IReceiptVoucherService _receiptVoucherService;
        private IReceiptVoucherDetailService _receiptVoucherDetailService;
        private IReceivableService _receivableService;
        private IItemService _itemService;
        private ISalesDownPaymentService _salesDownPaymentService;
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        private IContactService _contactService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;

        public SalesDownPaymentController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _cashBankService = new CashBankService(new CashBankRepository(),new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _salesInvoiceDetailService = new SalesInvoiceDetailService(new SalesInvoiceDetailRepository(), new SalesInvoiceDetailValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _salesDownPaymentAllocationService = new SalesDownPaymentAllocationService(new SalesDownPaymentAllocationRepository(), new SalesDownPaymentAllocationValidator());
            _salesDownPaymentAllocationDetailService = new SalesDownPaymentAllocationDetailService(new SalesDownPaymentAllocationDetailRepository(), new SalesDownPaymentAllocationDetailValidator());
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _salesDownPaymentService = new SalesDownPaymentService(new SalesDownPaymentRepository(), new SalesDownPaymentValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
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
            var q = _salesDownPaymentService.GetQueryable().Include("CashBank").Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.CashBankId,
                             CashBank = model.CashBank.Name,
                             model.DownPaymentDate,
                             model.IsGBCH,
                             model.DueDate,
                             model.TotalAmount,
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
                            model.ContactId,
                            model.Contact,
                            model.CashBankId,
                            model.CashBank,
                            model.DownPaymentDate,
                            model.IsGBCH,
                            model.DueDate,
                            model.TotalAmount,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListReceivable(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _receivableService.GetQueryable().Include("Contact").Where(x => !x.IsDeleted && x.RemainingAmount > 0);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.ReceivableSource,
                             model.ReceivableSourceId,
                             model.DueDate,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
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
                            model.Code,
                            model.ContactId,
                            model.Contact,
                            model.ReceivableSource,
                            model.ReceivableSourceId,
                            model.DueDate,
                            model.Amount,
                            model.RemainingAmount,
                            model.PendingClearanceAmount,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            SalesDownPayment model = new SalesDownPayment();
            try
            {
                model = _salesDownPaymentService.GetObjectById(Id);
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
                model.ContactId,
                Contact = _contactService.GetObjectById(model.ContactId).Name,
                model.CashBankId,
                CashBank = _cashBankService.GetObjectById(model.CashBankId).Name,
                model.DownPaymentDate,
                model.IsGBCH,
                model.DueDate,
                model.TotalAmount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(SalesDownPayment model)
        {
            try
            {
             
                model = _salesDownPaymentService.CreateObject(model, _contactService, _receiptVoucherDetailService,_receivableService,
                                                                 _cashBankService,_receiptVoucherService);
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
        public dynamic Update(SalesDownPayment model)
        {
            try
            {
                var data = _salesDownPaymentService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.CashBankId = model.CashBankId;
                data.DownPaymentDate = model.DownPaymentDate;
                data.IsGBCH = model.IsGBCH;
                data.DueDate = model.DueDate;
                data.TotalAmount = model.TotalAmount;
                model = _salesDownPaymentService.UpdateObject(data, _contactService, _receiptVoucherDetailService, _receivableService, _cashBankService, _receiptVoucherService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
            });
        }

        [HttpPost]
        public dynamic Delete(SalesDownPayment model)
        {
            try
            {
                var data = _salesDownPaymentService.GetObjectById(model.Id);
                model = _salesDownPaymentService.SoftDeleteObject(data, _salesDownPaymentAllocationService, _receiptVoucherDetailService, _receiptVoucherService);
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
        public dynamic Confirm(SalesDownPayment model)
        {
            try
            {
                var data = _salesDownPaymentService.GetObjectById(model.Id);
                model = _salesDownPaymentService.ConfirmObject(data,model.ConfirmationDate.Value,
                    _cashBankService, _receiptVoucherService, _receiptVoucherDetailService, _receivableService, _contactService,
                    _accountService,_generalLedgerJournalService, _closingService);
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
        public dynamic UnConfirm(SalesDownPayment model)
        {
            try
            {

                var data = _salesDownPaymentService.GetObjectById(model.Id);
                model = _salesDownPaymentService.UnconfirmObject(data, _cashBankService,
                    _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService, _receiptVoucherService,
                    _receiptVoucherDetailService, _receivableService, _contactService,_accountService, _generalLedgerJournalService, _closingService);
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


