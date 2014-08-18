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
    public class ReceiptVoucherController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ReceiptVoucherController");
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPaymentVoucherDetailService _paymentVoucherDetailService;
        private IReceivableService _receivableService;
        private IItemService _itemService;
        private IPaymentVoucherService _paymentVoucherService;
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        private IContactService _contactService;
        private IReceiptVoucherService _receiptVoucherService;
        private IReceiptVoucherDetailService _receiptVoucherDetailService;

        public ReceiptVoucherController()
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
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
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
            var query =  _receiptVoucherService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<ReceiptVoucher>;

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
                            model.ReceiptDate,
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
            var query = _receivableService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<Receivable>;

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

            // Get Data
            var query =  _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<ReceiptVoucherDetail>;

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
                            _receivableService.GetObjectById(model.ReceivableId).Code,
                            model.Amount,
                            model.Description,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

      
        public dynamic GetInfo(int Id)
        {
            ReceiptVoucher model = new ReceiptVoucher();
            try
            {
                model = _receiptVoucherService.GetObjectById(Id);
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
                model.ReceiptDate,
                model.IsGBCH,
                model.DueDate,
                model.IsBank,
                model.TotalAmount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            ReceiptVoucherDetail model = new ReceiptVoucherDetail();
            try
            {
                model = _receiptVoucherDetailService.GetObjectById(Id);
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
        public dynamic Insert(ReceiptVoucher model)
        {
            try
            {
             
                model = _receiptVoucherService.CreateObject(model,_receiptVoucherDetailService,_receivableService
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
        public dynamic InsertDetail(ReceiptVoucherDetail model)
        {
            decimal amount = 0;
            try
            {
                model = _receiptVoucherDetailService.CreateObject(model,_receiptVoucherService,_cashBankService,_receivableService
                   );
                amount = _receiptVoucherService.GetObjectById(model.ReceiptVoucherId).TotalAmount;
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
        public dynamic Update(ReceiptVoucher model)
        {
            try
            {
                var data = _receiptVoucherService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.CashBankId = model.CashBankId;
                data.ReceiptDate = model.ReceiptDate;
                data.IsGBCH = model.IsGBCH;
                data.DueDate = model.DueDate;
                data.IsBank = model.IsBank;
                data.TotalAmount = model.TotalAmount;
                model = _receiptVoucherService.UpdateObject(data,_receiptVoucherDetailService,_receivableService,
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
        public dynamic Delete(ReceiptVoucher model)
        {
            try
            {
                var data = _receiptVoucherService.GetObjectById(model.Id);
                model = _receiptVoucherService.SoftDeleteObject(data,_receiptVoucherDetailService);
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
        public dynamic DeleteDetail(ReceiptVoucherDetail model)
        {
            decimal amount = 0;
            try
            {
                var data = _receiptVoucherDetailService.GetObjectById(model.Id);
                model = _receiptVoucherDetailService.SoftDeleteObject(data);
                amount = _receiptVoucherService.GetObjectById(model.ReceiptVoucherId).TotalAmount;
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
        public dynamic UpdateDetail(ReceiptVoucherDetail model)
        {
            decimal amount = 0;
            try
            {
                var data = _receiptVoucherDetailService.GetObjectById(model.Id);
                data.ReceivableId = model.ReceivableId;
                data.Amount = model.Amount;
                data.Description = model.Description;
                model = _receiptVoucherDetailService.UpdateObject(data,_receiptVoucherService,_cashBankService,_receivableService);
                amount = _receiptVoucherService.GetObjectById(model.ReceiptVoucherId).TotalAmount;
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
        public dynamic Confirm(ReceiptVoucher model)
        {
            try
            {
                var data = _receiptVoucherService.GetObjectById(model.Id);
                model = _receiptVoucherService.ConfirmObject(data,model.ConfirmationDate.Value,
                    _receiptVoucherDetailService,_cashBankService,_receivableService,_cashMutationService);
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
        public dynamic UnConfirm(ReceiptVoucher model)
        {
            try
            {

                var data = _receiptVoucherService.GetObjectById(model.Id);
                model = _receiptVoucherService.UnconfirmObject(data,_receiptVoucherDetailService,_cashBankService,
                    _receivableService,_cashMutationService);
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
        public dynamic Reconcile(ReceiptVoucher model)
        {
            try
            {
                var data = _receiptVoucherService.GetObjectById(model.Id);
                model = _receiptVoucherService.ReconcileObject(data,model.ReconciliationDate.Value,_receiptVoucherDetailService,
                    _cashMutationService, _cashBankService, _receivableService);
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
        public dynamic UnReconcile(ReceiptVoucher model)
        {
            try
            {
                var data = _receiptVoucherService.GetObjectById(model.Id);
                model = _receiptVoucherService.UnreconcileObject(data,_receiptVoucherDetailService,_cashMutationService,
                    _cashBankService, _receivableService
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


