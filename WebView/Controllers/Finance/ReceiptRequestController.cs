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
    public class ReceiptRequestController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ReceiptRequestController");
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IReceiptRequestService _receiptRequestService;
        private IReceiptRequestDetailService _receiptRequestDetailService;
        private IContactService _contactService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IReceivableService _receivableService;
        private IItemService _itemService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private IExchangeRateService _exchangeRateService;
        private IGLNonBaseCurrencyService _gLNonBaseCurrencyService;
        private ICurrencyService _currencyService;

        public ReceiptRequestController()
        {
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _receiptRequestService = new ReceiptRequestService(new ReceiptRequestRepository(), new ReceiptRequestValidator());
            _receiptRequestDetailService = new ReceiptRequestDetailService(new ReceiptRequestDetailRepository(), new ReceiptRequestDetailValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidator());
            _gLNonBaseCurrencyService = new GLNonBaseCurrencyService(new GLNonBaseCurrencyRepository(), new GLNonBaseCurrencyValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.ReceiptRequest, Core.Constants.Constant.MenuGroupName.Master))
            {
                return Content(Core.Constants.Constant.ControllerOutput.PageViewNotAllowed);
            }

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
            var q = _receiptRequestService.GetQueryable().Include("Contact");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             contact = model.Contact.Name,
                             model.Description,
                             model.Amount,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.RequestedDate,
                             model.DueDate,
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
                            model.contact,
                            model.Description,
                            model.Amount,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.RequestedDate,
                            model.DueDate,
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
            var q = _receiptRequestDetailService.GetQueryable().Include("ReceiptRequest").Include("Account")
                                                .Where(x => x.ReceiptRequestId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.AccountId,
                             AccountCode = model.Account.Code,
                             Account = model.Account.Name,
                             model.Status,
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
                            model.Code,
                            model.AccountId,
                            model.AccountCode,
                            model.Account,
                            model.Status,
                            model.Amount
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            ReceiptRequest model = new ReceiptRequest();
            try
            {
                model = _receiptRequestService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.ContactId,
                Contact = model.Contact.Name,
                currency = model.Currency.Name,
                currencyId = model.CurrencyId,
                model.AccountReceivableId,
                AccountReceivable = model.AccountReceivable.Name,
                model.Description,
                model.RequestedDate,
                model.DueDate,
                model.Amount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            ReceiptRequestDetail model = new ReceiptRequestDetail();
            try
            {
                model = _receiptRequestDetailService.GetObjectById(Id);
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
                model.AccountId,
                AccountCode = model.Account.Code,
                Account = model.Account.Name,
                model.Status,
                model.Amount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(ReceiptRequest model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.ReceiptRequest, Core.Constants.Constant.MenuGroupName.Master))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _receiptRequestService.CreateObject(model, _contactService, _receiptRequestDetailService, _accountService, _generalLedgerJournalService, _closingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic InsertDetail(ReceiptRequestDetail model)
        {
            decimal totalAmount = 0;
            try
            {
               model = _receiptRequestDetailService.CreateObject(model, _receiptRequestService, _accountService);
               totalAmount = _receiptRequestService.GetObjectById(model.ReceiptRequestId).Amount;
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
        public dynamic Update(ReceiptRequest model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.ReceiptRequest, Core.Constants.Constant.MenuGroupName.Master))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _receiptRequestService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.Description = model.Description;
                data.Amount = model.Amount;
                data.RequestedDate = model.RequestedDate;
                data.DueDate = model.DueDate;
                model = _receiptRequestService.UpdateObject(data, _contactService, _receiptRequestDetailService, _accountService, _generalLedgerJournalService, _closingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors,
            });
        }

        [HttpPost]
        public dynamic Delete(ReceiptRequest model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.ReceiptRequest, Core.Constants.Constant.MenuGroupName.Master))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _receiptRequestService.GetObjectById(model.Id);
                model = _receiptRequestService.SoftDeleteObject(data);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }


        [HttpPost]
        public dynamic UpdateDetail(ReceiptRequestDetail model)
        {
            decimal totalAmount = 0;
            try
            {
                var data = _receiptRequestDetailService.GetObjectById(model.Id);
                data.AccountId = model.AccountId;
                data.Status = model.Status;
                data.Amount = model.Amount;
                model = _receiptRequestDetailService.UpdateObject(data, _receiptRequestService, _accountService);
                totalAmount = _receiptRequestService.GetObjectById(model.ReceiptRequestId).Amount;
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
        public dynamic DeleteDetail(ReceiptRequestDetail model)
        {
            decimal totalAmount = 0;
            try
            {
                var data = _receiptRequestDetailService.GetObjectById(model.Id);
                model = _receiptRequestDetailService.SoftDeleteObject(data,_receiptRequestService);
                totalAmount = _receiptRequestService.GetObjectById(data.ReceiptRequestId).Amount;
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
        public dynamic Confirm(ReceiptRequest model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Confirm", Core.Constants.Constant.MenuName.ReceiptRequest, Core.Constants.Constant.MenuGroupName.Master))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _receiptRequestService.GetObjectById(model.Id);
                model = _receiptRequestService.ConfirmObject(data, model.ConfirmationDate.Value, _receivableService, _receiptRequestDetailService,
                                                             _accountService, _generalLedgerJournalService, _closingService,_exchangeRateService,
                                                             _gLNonBaseCurrencyService, _currencyService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnConfirm(ReceiptRequest model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("UnConfirm", Core.Constants.Constant.MenuName.ReceiptRequest, Core.Constants.Constant.MenuGroupName.Master))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _receiptRequestService.GetObjectById(model.Id);
                model = _receiptRequestService.UnconfirmObject(data, _receiptRequestDetailService, _receivableService, _accountService, 
                    _generalLedgerJournalService, _closingService,_gLNonBaseCurrencyService, _currencyService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unconfirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }


    }
}

