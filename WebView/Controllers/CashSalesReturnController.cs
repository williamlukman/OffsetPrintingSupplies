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
    public class CashSalesReturnController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CashSalesReturnController");
        private IItemService _itemService;
        private IContactService _contactService;
        private IItemTypeService _itemTypeService;
        private IUoMService _uoMService;
        private IWarehouseItemService _warehouseItemService;
        private IWarehouseService _warehouseService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IPriceMutationService _priceMutationService;
        private IContactGroupService _contactGroupService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        private ICashSalesReturnService _cashSalesReturnService;
        private ICashSalesReturnDetailService _cashSalesReturnDetailService;
        private ICashSalesInvoiceService _cashSalesInvoiceService;
        private ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService;
        private IQuantityPricingService _quantityPricingService;
        private IPayableService _payableService;
        private IPaymentVoucherService _paymentVoucherService;
        private IPaymentVoucherDetailService _paymentVoucherDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        public CashSalesReturnController()
        {
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(),new ItemTypeValidator());
            _uoMService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(),new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(),new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _cashSalesInvoiceService = new CashSalesInvoiceService(new CashSalesInvoiceRepository(), new CashSalesInvoiceValidator());
            _cashSalesReturnDetailService = new CashSalesReturnDetailService(new CashSalesReturnDetailRepository(), new CashSalesReturnDetailValidator());
            _cashSalesInvoiceDetailService = new CashSalesInvoiceDetailService(new CashSalesInvoiceDetailRepository(), new CashSalesInvoiceDetailValidator());
            _cashSalesReturnService = new CashSalesReturnService(new CashSalesReturnRepository(), new CashSalesReturnValidator());
            _quantityPricingService = new QuantityPricingService(new QuantityPricingRepository(), new QuantityPricingValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
            _paymentVoucherDetailService = new PaymentVoucherDetailService(new PaymentVoucherDetailRepository(), new PaymentVoucherDetailValidator());
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
            var query = _cashSalesReturnService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<CashSalesReturn>;

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
                    from cashSalesReturn in list
                    select new
                    {
                        id = cashSalesReturn.Id,
                        cell = new object[] {
                            cashSalesReturn.Id,
                            cashSalesReturn.Code,
                            cashSalesReturn.Description,
                            cashSalesReturn.ReturnDate,
                            cashSalesReturn.CashSalesInvoiceId,
                            _cashSalesInvoiceService.GetObjectById((int)cashSalesReturn.CashSalesInvoiceId).Code,
                            cashSalesReturn.Allowance,
                            cashSalesReturn.IsConfirmed,
                            cashSalesReturn.ConfirmationDate,
                            cashSalesReturn.Total,
                            cashSalesReturn.CashBankId,
                            _cashBankService.GetObjectById((int)cashSalesReturn.CashBankId).Name,
                            _cashBankService.GetObjectById((int)cashSalesReturn.CashBankId).IsBank,
                            cashSalesReturn.IsPaid,
                            cashSalesReturn.CreatedAt,
                            cashSalesReturn.UpdatedAt,
                            _cashSalesReturnDetailService.GetObjectsByCashSalesReturnId(cashSalesReturn.Id).Count(),
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _cashSalesReturnDetailService.GetObjectsByCashSalesReturnId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<CashSalesReturnDetail>;

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
                            model.CashSalesReturnId,
                            _cashSalesReturnService.GetObjectById(model.CashSalesReturnId).Code,
                            model.CashSalesInvoiceDetailId,
                            _cashSalesInvoiceDetailService.GetObjectById(model.CashSalesInvoiceDetailId).Code,
                            model.Quantity,
                            model.TotalPrice,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            CashSalesReturn model = new CashSalesReturn();
            try
            {
                model = _cashSalesReturnService.GetObjectById(Id);
          
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.Description,
                model.ReturnDate,
                model.Allowance,
                model.Total,
                model.CashBankId,
                CashBank = _cashBankService.GetObjectById((int)model.CashBankId).Name,
                model.CashSalesInvoiceId,
                CashSalesInvoice = _cashSalesInvoiceService.GetObjectById((int)model.CashSalesInvoiceId).Code,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            CashSalesReturnDetail model = new CashSalesReturnDetail();
            try
            {
                model = _cashSalesReturnDetailService.GetObjectById(Id);
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
                model.CashSalesReturnId,
                CashSalesReturn = _cashSalesReturnService.GetObjectById(model.CashSalesReturnId).Code,
                model.CashSalesInvoiceDetailId,
                CashSalesInvoiceDetail = _cashSalesInvoiceDetailService.GetObjectById(model.CashSalesInvoiceDetailId).Code,
                _cashSalesInvoiceDetailService.GetObjectById(model.CashSalesInvoiceDetailId).ItemId,
                _itemService.GetObjectById(_cashSalesInvoiceDetailService.GetObjectById(model.CashSalesInvoiceDetailId).ItemId).Name,
                model.Quantity,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CashSalesReturn model)
        {
            try
            {
                model = _cashSalesReturnService.CreateObject(model, _cashSalesInvoiceService, _cashBankService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic InsertDetail(CashSalesReturnDetail model)
        {
            decimal total = 0;
            try
            {
                model = _cashSalesReturnDetailService.CreateObject(model, _cashSalesReturnService, _cashSalesInvoiceDetailService);
                total = _cashSalesReturnService.GetObjectById(model.CashSalesReturnId).Total;
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);

            }

            return Json(new
            {
                model.Errors,
                Total = total
            });
        }

        [HttpPost]
        public dynamic Update(CashSalesReturn model)
        {
            try
            {
                var data = _cashSalesReturnService.GetObjectById(model.Id);
                data.Description = model.Description;
                data.ReturnDate = model.ReturnDate;
                data.Allowance = model.Allowance;
                data.CashBankId = model.CashBankId;
                model = _cashSalesReturnService.UpdateObject(data, _cashSalesReturnDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(CashSalesReturnDetail model)
        {
            decimal total = 0;
            try
            {
                var data = _cashSalesReturnDetailService.GetObjectById(model.Id);
                data.CashSalesInvoiceDetailId = model.CashSalesInvoiceDetailId;
                data.Quantity = model.Quantity;
                data.CashSalesReturnId = model.CashSalesReturnId;
                model = _cashSalesReturnDetailService.UpdateObject(data, _cashSalesReturnService, _cashSalesInvoiceDetailService);
                total = _cashSalesReturnService.GetObjectById(model.CashSalesReturnId).Total;
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                Total = total
            });
        }

        [HttpPost]
        public dynamic Confirm(CashSalesReturn model)
        {
            try
            {
                var data = _cashSalesReturnService.GetObjectById(model.Id);
                model = _cashSalesReturnService.ConfirmObject(data, model.ConfirmationDate.Value, model.Allowance, _cashSalesReturnDetailService, 
                                                    _contactService, _cashSalesInvoiceService, _cashSalesInvoiceDetailService, _priceMutationService, _payableService, 
                                                    _cashSalesReturnService, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService);
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
        public dynamic UnConfirm(CashSalesReturn model)
        {
            try
            {

                var data = _cashSalesReturnService.GetObjectById(model.Id);
                model = _cashSalesReturnService.UnconfirmObject(data, _cashSalesReturnDetailService, _cashSalesInvoiceDetailService, _payableService, _paymentVoucherDetailService,
                                                   _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService);

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
        public dynamic Paid(CashSalesReturn model)
        {
            try
            {
                var data = _cashSalesReturnService.GetObjectById(model.Id);
                data.Allowance = model.Allowance;
                model = _cashSalesReturnService.PaidObject(data, _cashBankService, _payableService, _paymentVoucherService, _paymentVoucherDetailService, 
                                                    _contactService, _cashMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Paid Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnPaid(CashSalesReturn model)
        {
            try
            {

                var data = _cashSalesReturnService.GetObjectById(model.Id);
                model = _cashSalesReturnService.UnpaidObject(data, _paymentVoucherService, _paymentVoucherDetailService, _cashBankService,
                                                   _payableService, _cashMutationService);

            }
            catch (Exception ex)
            {
                LOG.Error("Unpaid Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(CashSalesReturn model)
        {
            try
            {
                var data = _cashSalesReturnService.GetObjectById(model.Id);
                model = _cashSalesReturnService.SoftDeleteObject(data, _cashSalesReturnDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic DeleteDetail(CashSalesReturnDetail model)
        {
            decimal total = 0;
            try
            {
                var data = _cashSalesReturnDetailService.GetObjectById(model.Id);
                model = _cashSalesReturnDetailService.SoftDeleteObject(data, _cashSalesReturnService);
                total = _cashSalesReturnService.GetObjectById(model.CashSalesReturnId).Total;
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                Total = total
            });
        }
    }
}