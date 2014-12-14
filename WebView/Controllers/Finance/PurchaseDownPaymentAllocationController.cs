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
    public class PurchaseDownPaymentAllocationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PurchaseDownPaymentAllocationController");
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IDeliveryOrderService _deliveryOrderService;
        private IDeliveryOrderDetailService _deliveryOrderDetailService;
        private IPurchaseDownPaymentService _purchaseDownPaymentService;
        private IPayableService _payableService;
        private IItemService _itemService;
        private IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService;
        private IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService;
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        private IContactService _contactService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private IReceivableService _receivableService;
        private IExchangeRateService _exchangeRateService;

        public PurchaseDownPaymentAllocationController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _purchaseDownPaymentService = new PurchaseDownPaymentService(new PurchaseDownPaymentRepository(), new PurchaseDownPaymentValidator());
            _purchaseDownPaymentAllocationService = new PurchaseDownPaymentAllocationService(new PurchaseDownPaymentAllocationRepository(), new PurchaseDownPaymentAllocationValidator());
            _purchaseDownPaymentAllocationDetailService = new PurchaseDownPaymentAllocationDetailService(new PurchaseDownPaymentAllocationDetailRepository(), new PurchaseDownPaymentAllocationDetailValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _purchaseDownPaymentAllocationService = new PurchaseDownPaymentAllocationService(new PurchaseDownPaymentAllocationRepository(), new PurchaseDownPaymentAllocationValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidator());
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
            var q = _purchaseDownPaymentAllocationService.GetQueryable().Include("PurchaseDownPayment")
                                                                        .Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.ReceivableId,
                             Receivable = model.Receivable.Code,
                             model.AllocationDate,
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
                            model.ReceivableId,
                            model.Receivable,
                            model.AllocationDate,
                            model.TotalAmount,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListPayable(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _payableService.GetQueryable().Include("Contact").Where(x => !x.IsDeleted && x.RemainingAmount > 0);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.PayableSource,
                             model.PayableSourceId,
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
                            model.PayableSource,
                            model.PayableSourceId,
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

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _purchaseDownPaymentAllocationDetailService.GetQueryable().Include("PurchaseDownPaymentAllocation").Include("Payable")
                    .Where(x => x.PurchaseDownPaymentAllocationId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.PayableId,
                             Payable = model.Payable.Code,
                             model.Amount,
                             model.Description,
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
                            model.PayableId,
                            model.Payable,
                            model.Amount,
                            model.Description,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            PurchaseDownPaymentAllocation model = new PurchaseDownPaymentAllocation();
            try
            {
                model = _purchaseDownPaymentAllocationService.GetObjectById(Id);
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
                model.ReceivableId,
                Receivable = model.Receivable.Code,
                currency = model.Receivable.Currency.Name,
                model.AllocationDate,
                model.TotalAmount,
                model.RateToIDR,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            PurchaseDownPaymentAllocationDetail model = new PurchaseDownPaymentAllocationDetail();
            try
            {
                model = _purchaseDownPaymentAllocationDetailService.GetObjectById(Id);
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
                model.PayableId,
                Payable = _payableService.GetObjectById(model.PayableId).Code,
                model.Amount,
                model.Description,
                model.Rate,
                model.AmountPaid,
                Remaining = model.Payable.Amount,
                currency = model.Payable.Currency.Name,

                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(PurchaseDownPaymentAllocation model)
        {
            try
            {

                model = _purchaseDownPaymentAllocationService.CreateObject(model, _purchaseDownPaymentService, _purchaseDownPaymentAllocationDetailService, _contactService, _receivableService);
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
        public dynamic InsertDetail(PurchaseDownPaymentAllocationDetail model)
        {
            decimal totalAmount = 0;
            try
            {
                model = _purchaseDownPaymentAllocationDetailService.CreateObject(model, _purchaseDownPaymentAllocationService, _purchaseDownPaymentService,
                                                                                 _payableService, _receivableService);
                totalAmount = _purchaseDownPaymentAllocationService.GetObjectById(model.PurchaseDownPaymentAllocationId).TotalAmount;
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);

            }

            return Json(new
            {
                model.Errors,
                totalAmount
            });
        }

        [HttpPost]
        public dynamic Update(PurchaseDownPaymentAllocation model)
        {
            try
            {
                var data = _purchaseDownPaymentAllocationService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.ReceivableId = model.ReceivableId;
                data.AllocationDate = model.AllocationDate;
                data.RateToIDR = model.RateToIDR;
                model = _purchaseDownPaymentAllocationService.UpdateObject(data, _purchaseDownPaymentService, _purchaseDownPaymentAllocationDetailService, _contactService, _receivableService);
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
        public dynamic Delete(PurchaseDownPaymentAllocation model)
        {
            try
            {
                var data = _purchaseDownPaymentAllocationService.GetObjectById(model.Id);
                model = _purchaseDownPaymentAllocationService.SoftDeleteObject(data, _purchaseDownPaymentAllocationDetailService);
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
        public dynamic DeleteDetail(PurchaseDownPaymentAllocationDetail model)
        {
            decimal totalAmount = 0;
            try
            {
                var data = _purchaseDownPaymentAllocationDetailService.GetObjectById(model.Id);
                model = _purchaseDownPaymentAllocationDetailService.SoftDeleteObject(data, _purchaseDownPaymentAllocationService);
                totalAmount = _purchaseDownPaymentAllocationService.GetObjectById(model.PurchaseDownPaymentAllocationId).TotalAmount;
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                totalAmount
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(PurchaseDownPaymentAllocationDetail model)
        {
            decimal totalAmount = 0;
            try
            {
                var data = _purchaseDownPaymentAllocationDetailService.GetObjectById(model.Id);
                data.PayableId = model.PayableId;
                data.AmountPaid = model.AmountPaid;
                data.Description = model.Description;
                model = _purchaseDownPaymentAllocationDetailService.UpdateObject(data, _purchaseDownPaymentAllocationService, _purchaseDownPaymentService,
                                                                                 _payableService, _receivableService);
                totalAmount = _purchaseDownPaymentAllocationService.GetObjectById(model.PurchaseDownPaymentAllocationId).TotalAmount;
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                totalAmount

            });
        }


        [HttpPost]
        public dynamic Confirm(PurchaseDownPaymentAllocation model)
        {
            try
            {
                var data = _purchaseDownPaymentAllocationService.GetObjectById(model.Id);
                model = _purchaseDownPaymentAllocationService.ConfirmObject(data, model.ConfirmationDate.Value,
                        _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _payableService, _receivableService,
                        _accountService, _generalLedgerJournalService, _closingService);
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
        public dynamic UnConfirm(PurchaseDownPaymentAllocation model)
        {
            try
            {

                var data = _purchaseDownPaymentAllocationService.GetObjectById(model.Id);
                model = _purchaseDownPaymentAllocationService.UnconfirmObject(data, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService,
                    _payableService, _receivableService, _accountService, _generalLedgerJournalService, _closingService);
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


