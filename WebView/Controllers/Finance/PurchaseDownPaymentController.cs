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
    public class PurchaseDownPaymentController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PurchaseDownPaymentController");
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService;
        private IPayableService _payableService;
        private IItemService _itemService;
        private IPurchaseDownPaymentService _purchaseDownPaymentService;
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        private IContactService _contactService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;

        public PurchaseDownPaymentController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _cashBankService = new CashBankService(new CashBankRepository(),new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _purchaseDownPaymentDetailService = new PurchaseDownPaymentDetailService(new PurchaseDownPaymentDetailRepository(), new PurchaseDownPaymentDetailValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _purchaseDownPaymentService = new PurchaseDownPaymentService(new PurchaseDownPaymentRepository(), new PurchaseDownPaymentValidator());
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
            var q = _purchaseDownPaymentService.GetQueryable().Include("CashBank").Include("Contact").Where(x => !x.IsDeleted);

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
                             model.IsReconciled,
                             model.ReconciliationDate,
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
                            model.IsReconciled,
                            model.ReconciliationDate,
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

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id,string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _purchaseDownPaymentDetailService.GetQueryable().Include("PurchaseDownPayment").Include("Payable")
                                                .Where(x => x.PurchaseDownPaymentId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.Code,
                            model.PayableId,
                            PayableCode = model.Payable.Code,
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
                            model.PayableCode,
                            model.Amount,
                            model.Description,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

      
        public dynamic GetInfo(int Id)
        {
            PurchaseDownPayment model = new PurchaseDownPayment();
            try
            {
                model = _purchaseDownPaymentService.GetObjectById(Id);
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

        public dynamic GetInfoDetail(int Id)
        {
            PurchaseDownPaymentDetail model = new PurchaseDownPaymentDetail();
            try
            {
                model = _purchaseDownPaymentDetailService.GetObjectById(Id);
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
                Payable =_payableService.GetObjectById(model.PayableId).Code,
                model.Amount,
                model.Description,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(PurchaseDownPayment model)
        {
            try
            {
             
                model = _purchaseDownPaymentService.CreateObject(model,_purchaseDownPaymentDetailService,_payableService
                    ,_contactService,_cashBankService);
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
        public dynamic InsertDetail(PurchaseDownPaymentDetail model)
        {
            try
            {
                model = _purchaseDownPaymentDetailService.CreateObject(model,_purchaseDownPaymentService,_cashBankService,_payableService
                   );
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
        public dynamic Update(PurchaseDownPayment model)
        {
            try
            {
                var data = _purchaseDownPaymentService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.CashBankId = model.CashBankId;
                data.DownPaymentDate = model.DownPaymentDate;
                data.IsGBCH = model.IsGBCH;
                data.DueDate = model.DueDate;
                data.TotalAmount = model.TotalAmount;
                model = _purchaseDownPaymentService.UpdateObject(data,_purchaseDownPaymentDetailService,_payableService,
                    _contactService,_cashBankService);
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
        public dynamic Delete(PurchaseDownPayment model)
        {
            try
            {
                var data = _purchaseDownPaymentService.GetObjectById(model.Id);
                model = _purchaseDownPaymentService.SoftDeleteObject(data,_purchaseDownPaymentDetailService);
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
        public dynamic DeleteDetail(PurchaseDownPaymentDetail model)
        {
            try
            {
                var data = _purchaseDownPaymentDetailService.GetObjectById(model.Id);
                model = _purchaseDownPaymentDetailService.SoftDeleteObject(data);
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
        public dynamic UpdateDetail(PurchaseDownPaymentDetail model)
        {
            try
            {
                var data = _purchaseDownPaymentDetailService.GetObjectById(model.Id);
                data.PayableId = model.PayableId;
                data.Amount = model.Amount;
                data.Description = model.Description;
                model = _purchaseDownPaymentDetailService.UpdateObject(data,_purchaseDownPaymentService,_cashBankService,_payableService);
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
        public dynamic Confirm(PurchaseDownPayment model)
        {
            try
            {
                var data = _purchaseDownPaymentService.GetObjectById(model.Id);
                model = _purchaseDownPaymentService.ConfirmObject(data,model.ConfirmationDate.Value,
                    _purchaseDownPaymentDetailService,_cashBankService,_payableService,_cashMutationService,
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
        public dynamic UnConfirm(PurchaseDownPayment model)
        {
            try
            {

                var data = _purchaseDownPaymentService.GetObjectById(model.Id);
                model = _purchaseDownPaymentService.UnconfirmObject(data,_purchaseDownPaymentDetailService,_cashBankService,
                    _payableService,_cashMutationService,_accountService, _generalLedgerJournalService, _closingService);
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
        public dynamic Reconcile(PurchaseDownPayment model)
        {
            try
            {
                var data = _purchaseDownPaymentService.GetObjectById(model.Id);
                model = _purchaseDownPaymentService.ReconcileObject(data,model.ReconciliationDate.Value,_purchaseDownPaymentDetailService,
                    _cashMutationService,_cashBankService,_payableService,_accountService, _generalLedgerJournalService, _closingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Reconcile Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnReconcile(PurchaseDownPayment model)
        {
            try
            {
                var data = _purchaseDownPaymentService.GetObjectById(model.Id);
                model = _purchaseDownPaymentService.UnreconcileObject(data,_purchaseDownPaymentDetailService,_cashMutationService,
                    _cashBankService,_payableService, _accountService, _generalLedgerJournalService, _closingService
                    );
            }
            catch (Exception ex)
            {
                LOG.Error("UnReconcile Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}


