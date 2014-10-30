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
        private ISalesOrderService _salesOrderService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private ISalesInvoiceService _salesInvoiceService;
        private ISalesInvoiceDetailService _salesInvoiceDetailService;
        private IDeliveryOrderService _deliveryOrderService;
        private IDeliveryOrderDetailService _deliveryOrderDetailService;
        private ISalesDownPaymentService _salesDownPaymentService;
        private IReceiptVoucherService _receiptVoucherService;
        private IReceiptVoucherDetailService _receiptVoucherDetailService;
        private IReceivableService _receivableService;
        private IItemService _itemService;
        private ISalesDownPaymentAllocationService _salesDownPaymentAllocationService;
        private ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService;
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        private IContactService _contactService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;

        public SalesDownPaymentAllocationController()
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
            _salesDownPaymentService = new SalesDownPaymentService(new SalesDownPaymentRepository(), new SalesDownPaymentValidator());
            _salesDownPaymentAllocationService = new SalesDownPaymentAllocationService(new SalesDownPaymentAllocationRepository(), new SalesDownPaymentAllocationValidator());
            _salesDownPaymentAllocationDetailService = new SalesDownPaymentAllocationDetailService(new SalesDownPaymentAllocationDetailRepository(), new SalesDownPaymentAllocationDetailValidator());
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());          
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _salesDownPaymentAllocationService = new SalesDownPaymentAllocationService(new SalesDownPaymentAllocationRepository(), new SalesDownPaymentAllocationValidator());
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
            var q = _salesDownPaymentAllocationService.GetQueryable().Include("SalesDownPayment")
                                                                        .Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.SalesDownPaymentId,
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
                            model.SalesDownPaymentId,
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

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id,string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _salesDownPaymentAllocationDetailService.GetQueryable().Include("SalesDownPaymentAllocation").Include("Receivable")
                    .Where(x => x.SalesDownPaymentAllocationId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.Code,
                            model.ReceivableId,
                            ReceivableCode = model.Receivable.Code,
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
                            model.ReceivableCode,
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
                model = _salesDownPaymentAllocationService.GetObjectById(Id);
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
                model.SalesDownPaymentId,
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
                model = _salesDownPaymentAllocationDetailService.GetObjectById(Id);
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
                Receivable =_receivableService.GetObjectById(model.ReceivableId).Code,
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

                model = _salesDownPaymentAllocationService.CreateObject(model, _salesDownPaymentService, _salesDownPaymentAllocationDetailService, _contactService);
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
                model = _salesDownPaymentAllocationDetailService.CreateObject(model,_salesDownPaymentAllocationService, _salesDownPaymentService,
                                                                                 _receiptVoucherDetailService, _receivableService, _receiptVoucherService, _cashBankService);
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
                var data = _salesDownPaymentAllocationService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.SalesDownPaymentId = model.SalesDownPaymentId;
                data.AllocationDate = model.AllocationDate;
                data.TotalAmount = model.TotalAmount;
                model = _salesDownPaymentAllocationService.UpdateObject(data, _salesDownPaymentService, _salesDownPaymentAllocationDetailService, _contactService);
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
                var data = _salesDownPaymentAllocationService.GetObjectById(model.Id);
                model = _salesDownPaymentAllocationService.SoftDeleteObject(data,_salesDownPaymentAllocationDetailService);
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
                var data = _salesDownPaymentAllocationDetailService.GetObjectById(model.Id);
                model = _salesDownPaymentAllocationDetailService.SoftDeleteObject(data, _receiptVoucherDetailService);
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
                var data = _salesDownPaymentAllocationDetailService.GetObjectById(model.Id);
                data.ReceivableId = model.ReceivableId;
                data.Amount = model.Amount;
                data.Description = model.Description;
                model = _salesDownPaymentAllocationDetailService.UpdateObject(data, _salesDownPaymentAllocationService, _salesDownPaymentService, _receiptVoucherDetailService,
                                                                                 _receivableService, _receiptVoucherService, _cashBankService);
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
                var data = _salesDownPaymentAllocationService.GetObjectById(model.Id);
                model = _salesDownPaymentAllocationService.ConfirmObject(data,model.ConfirmationDate.Value,
                    _salesDownPaymentAllocationDetailService, _salesDownPaymentService, _receivableService,
                    _receiptVoucherDetailService, _cashBankService, _accountService,_generalLedgerJournalService, _closingService);
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

                var data = _salesDownPaymentAllocationService.GetObjectById(model.Id);
                model = _salesDownPaymentAllocationService.UnconfirmObject(data,_salesDownPaymentAllocationDetailService, _salesDownPaymentService,
                    _receivableService,_receiptVoucherDetailService,_accountService, _generalLedgerJournalService, _closingService);
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


