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

namespace WebView.Controllers
{
    public class PaymentVoucherController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PaymentVoucherController");
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPaymentVoucherDetailService _paymentVoucherDetailService;
        private IPayableService _payableService;
        private IItemService _itemService;
        private IPaymentVoucherService _paymentVoucherService;
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        private IContactService _contactService;

        public PaymentVoucherController()
        {
            _cashBankService = new CashBankService(new CashBankRepository(),new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _paymentVoucherDetailService = new PaymentVoucherDetailService(new PaymentVoucherDetailRepository(), new PaymentVoucherDetailValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
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
            var query =  _paymentVoucherService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<PaymentVoucher>;

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
                            _contactService.GetObjectById(model.ContactId).Name,
                            model.CashBankId,
                            _cashBankService.GetObjectById(model.CashBankId).Name,
                            model.PaymentDate,
                            model.IsGBCH,
                            model.DueDate,
                            model.IsBank,
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

            // Get Data
            var query = _payableService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<Payable>;

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
                            _contactService.GetObjectById(model.ContactId).Name,
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

            // Get Data
            var query =  _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<PaymentVoucherDetail>;

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
                            _payableService.GetObjectById(model.PayableId).Code,
                            model.Amount,
                            model.Description,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

      
        public dynamic GetInfo(int Id)
        {
            PaymentVoucher model = new PaymentVoucher();
            try
            {
                model = _paymentVoucherService.GetObjectById(Id);
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
                model.PaymentDate,
                model.IsGBCH,
                model.DueDate,
                model.IsBank,
                model.TotalAmount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            PaymentVoucherDetail model = new PaymentVoucherDetail();
            try
            {
                model = _paymentVoucherDetailService.GetObjectById(Id);
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
        public dynamic Insert(PaymentVoucher model)
        {
            try
            {
             
                model = _paymentVoucherService.CreateObject(model,_paymentVoucherDetailService,_payableService
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
        public dynamic InsertDetail(PaymentVoucherDetail model)
        {
            decimal amount = 0;
            try
            {
                model = _paymentVoucherDetailService.CreateObject(model,_paymentVoucherService,_cashBankService,_payableService
                   );
                amount = _paymentVoucherService.GetObjectById(model.PaymentVoucherId).TotalAmount;
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
               
            }

            return Json(new
            {
                model.Errors,
                AmountPayable = amount
            });
        }

        [HttpPost]
        public dynamic Update(PaymentVoucher model)
        {
            try
            {
                var data = _paymentVoucherService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.CashBankId = model.CashBankId;
                data.PaymentDate = model.PaymentDate;
                data.IsGBCH = model.IsGBCH;
                data.DueDate = model.DueDate;
                data.IsBank = model.IsBank;
                data.TotalAmount = model.TotalAmount;
                model = _paymentVoucherService.UpdateObject(data,_paymentVoucherDetailService,_payableService,
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
                model.TotalAmount
            });
        }

        [HttpPost]
        public dynamic Delete(PaymentVoucher model)
        {
            try
            {
                var data = _paymentVoucherService.GetObjectById(model.Id);
                model = _paymentVoucherService.SoftDeleteObject(data,_paymentVoucherDetailService);
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
        public dynamic DeleteDetail(PaymentVoucherDetail model)
        {
            decimal amount = 0;
            try
            {
                var data = _paymentVoucherDetailService.GetObjectById(model.Id);
                model = _paymentVoucherDetailService.SoftDeleteObject(data);
                amount = _paymentVoucherService.GetObjectById(model.PaymentVoucherId).TotalAmount;
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                AmountPayable = amount
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(PaymentVoucherDetail model)
        {
            decimal amount = 0;
            try
            {
                var data = _paymentVoucherDetailService.GetObjectById(model.Id);
                data.PayableId = model.PayableId;
                data.Amount = model.Amount;
                data.Description = model.Description;
                model = _paymentVoucherDetailService.UpdateObject(data,_paymentVoucherService,_cashBankService,_payableService);
                amount = _paymentVoucherService.GetObjectById(model.PaymentVoucherId).TotalAmount;
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                AmountPayable = amount
            });
        }


        [HttpPost]
        public dynamic Confirm(PaymentVoucher model)
        {
            try
            {
                var data = _paymentVoucherService.GetObjectById(model.Id);
                model = _paymentVoucherService.ConfirmObject(data,model.ConfirmationDate.Value,
                    _paymentVoucherDetailService,_cashBankService,_payableService,_cashMutationService);
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
        public dynamic UnConfirm(PaymentVoucher model)
        {
            try
            {

                var data = _paymentVoucherService.GetObjectById(model.Id);
                model = _paymentVoucherService.UnconfirmObject(data,_paymentVoucherDetailService,_cashBankService,
                    _payableService,_cashMutationService);
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
        public dynamic Reconcile(PaymentVoucher model)
        {
            try
            {
                var data = _paymentVoucherService.GetObjectById(model.Id);
                model = _paymentVoucherService.ReconcileObject(data,model.ReconciliationDate.Value,_paymentVoucherDetailService,
                    _cashMutationService,_cashBankService,_payableService);
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
        public dynamic UnReconcile(PaymentVoucher model)
        {
            try
            {
                var data = _paymentVoucherService.GetObjectById(model.Id);
                model = _paymentVoucherService.UnreconcileObject(data,_paymentVoucherDetailService,_cashMutationService,
                    _cashBankService,_payableService
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


