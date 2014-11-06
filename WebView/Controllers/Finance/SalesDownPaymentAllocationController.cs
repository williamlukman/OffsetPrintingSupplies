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
    public class SalesDownPaymentAllocationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SalesDownPaymentAllocationController");
        private ISalesOrderService _purchaseOrderService;
        private ISalesOrderDetailService _purchaseOrderDetailService;
        private ISalesInvoiceService _purchaseInvoiceService;
        private ISalesInvoiceDetailService _purchaseInvoiceDetailService;
        private IDeliveryOrderService _purchaseReceivalService;
        private IDeliveryOrderDetailService _purchaseReceivalDetailService;
        private ISalesDownPaymentService _purchaseDownPaymentService;
        private IReceivableService _receivableService;
        private IItemService _itemService;
        private ISalesDownPaymentAllocationService _purchaseDownPaymentAllocationService;
        private ISalesDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService;
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        private IContactService _contactService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private IPayableService _payableService;

        public SalesDownPaymentAllocationController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _purchaseOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
            _purchaseOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _purchaseInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _purchaseInvoiceDetailService = new SalesInvoiceDetailService(new SalesInvoiceDetailRepository(), new SalesInvoiceDetailValidator());
            _purchaseReceivalService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _purchaseReceivalDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _purchaseDownPaymentService = new SalesDownPaymentService(new SalesDownPaymentRepository(), new SalesDownPaymentValidator());
            _purchaseDownPaymentAllocationService = new SalesDownPaymentAllocationService(new SalesDownPaymentAllocationRepository(), new SalesDownPaymentAllocationValidator());
            _purchaseDownPaymentAllocationDetailService = new SalesDownPaymentAllocationDetailService(new SalesDownPaymentAllocationDetailRepository(), new SalesDownPaymentAllocationDetailValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _purchaseDownPaymentAllocationService = new SalesDownPaymentAllocationService(new SalesDownPaymentAllocationRepository(), new SalesDownPaymentAllocationValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
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
            var q = _purchaseDownPaymentAllocationService.GetQueryable().Include("SalesDownPayment")
                                                                        .Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.PayableId,
                             Payable = model.Payable.Code,
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
                            model.PayableId,
                            model.Payable,
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

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _purchaseDownPaymentAllocationDetailService.GetQueryable().Include("SalesDownPaymentAllocation").Include("Receivable")
                    .Where(x => x.SalesDownPaymentAllocationId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ReceivableId,
                             Receivable = model.Receivable.Code,
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
                            model.ReceivableId,
                            model.Receivable,
                            model.Amount,
                            model.Description,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            SalesDownPaymentAllocation model = new SalesDownPaymentAllocation();
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
                model.PayableId,
                Payable = model.Payable.Code,
                model.AllocationDate,
                model.TotalAmount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            SalesDownPaymentAllocationDetail model = new SalesDownPaymentAllocationDetail();
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
                model.ReceivableId,
                Receivable = _receivableService.GetObjectById(model.ReceivableId).Code,
                model.Amount,
                model.Description,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(SalesDownPaymentAllocation model)
        {
            try
            {

                model = _purchaseDownPaymentAllocationService.CreateObject(model, _purchaseDownPaymentService, _purchaseDownPaymentAllocationDetailService, _contactService, _payableService);
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
        public dynamic InsertDetail(SalesDownPaymentAllocationDetail model)
        {
            try
            {
                model = _purchaseDownPaymentAllocationDetailService.CreateObject(model, _purchaseDownPaymentAllocationService, _purchaseDownPaymentService,
                                                                                 _receivableService, _payableService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);

            }

            return Json(new
            {
                model.Errors,
            });
        }

        [HttpPost]
        public dynamic Update(SalesDownPaymentAllocation model)
        {
            try
            {
                var data = _purchaseDownPaymentAllocationService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.PayableId = model.PayableId;
                data.AllocationDate = model.AllocationDate;
                data.TotalAmount = model.TotalAmount;
                model = _purchaseDownPaymentAllocationService.UpdateObject(data, _purchaseDownPaymentService, _purchaseDownPaymentAllocationDetailService, _contactService, _payableService);
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
        public dynamic Delete(SalesDownPaymentAllocation model)
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
        public dynamic DeleteDetail(SalesDownPaymentAllocationDetail model)
        {
            try
            {
                var data = _purchaseDownPaymentAllocationDetailService.GetObjectById(model.Id);
                model = _purchaseDownPaymentAllocationDetailService.SoftDeleteObject(data);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(SalesDownPaymentAllocationDetail model)
        {
            try
            {
                var data = _purchaseDownPaymentAllocationDetailService.GetObjectById(model.Id);
                data.ReceivableId = model.ReceivableId;
                data.Amount = model.Amount;
                data.Description = model.Description;
                model = _purchaseDownPaymentAllocationDetailService.UpdateObject(data, _purchaseDownPaymentAllocationService, _purchaseDownPaymentService,
                                                                                 _receivableService, _payableService);
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
        public dynamic Confirm(SalesDownPaymentAllocation model)
        {
            try
            {
                var data = _purchaseDownPaymentAllocationService.GetObjectById(model.Id);
                model = _purchaseDownPaymentAllocationService.ConfirmObject(data, model.ConfirmationDate.Value,
                    _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService, _receivableService, _payableService,
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
        public dynamic UnConfirm(SalesDownPaymentAllocation model)
        {
            try
            {

                var data = _purchaseDownPaymentAllocationService.GetObjectById(model.Id);
                model = _purchaseDownPaymentAllocationService.UnconfirmObject(data, _purchaseDownPaymentAllocationDetailService, _purchaseDownPaymentService,
                    _receivableService, _payableService, _accountService, _generalLedgerJournalService, _closingService);
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


