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
    public class MstCashSalesInvoiceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CashSalesInvoiceController");
        private IItemService _itemService;
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
        private ICashSalesInvoiceService _cashSalesInvoiceService;
        private ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService;
        private ICashSalesReturnService _cashSalesReturnService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        public MstCashSalesInvoiceController()
        {
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
            _cashSalesInvoiceService = new CashSalesInvoiceService(new CashSalesInvoiceRepository(), new CashSalesInvoiceValidator());
            _cashSalesInvoiceDetailService = new CashSalesInvoiceDetailService(new CashSalesInvoiceDetailRepository(), new CashSalesInvoiceDetailValidator());
            _cashSalesReturnService = new CashSalesReturnService(new CashSalesReturnRepository(), new CashSalesReturnValidator());
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
            var query = _cashSalesInvoiceService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<CashSalesInvoice>;

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
                    from cashSalesInvoice in list
                    select new
                    {
                        id = cashSalesInvoice.Id,
                        cell = new object[] {
                            cashSalesInvoice.Id,
                            cashSalesInvoice.Code,
                            cashSalesInvoice.Description,
                            cashSalesInvoice.SalesDate,
                            cashSalesInvoice.DueDate,
                            cashSalesInvoice.Discount,
                            cashSalesInvoice.Tax,
                            cashSalesInvoice.Allowance,
                            cashSalesInvoice.IsConfirmed,
                            cashSalesInvoice.ConfirmationDate,
                            cashSalesInvoice.AmountPaid,
                            cashSalesInvoice.CashBankId,
                            _cashBankService.GetObjectById((int)cashSalesInvoice.CashBankId).Name,
                            cashSalesInvoice.IsBank,
                            cashSalesInvoice.IsPaid,
                            cashSalesInvoice.IsFullPayment,
                            cashSalesInvoice.Total,
                            cashSalesInvoice.CoGS,
                            cashSalesInvoice.WarehouseId,
                            _warehouseService.GetObjectById(cashSalesInvoice.WarehouseId).Name,
                            cashSalesInvoice.IsDeleted,
                            cashSalesInvoice.DeletedAt,
                            cashSalesInvoice.CreatedAt,
                            cashSalesInvoice.UpdatedAt,
                            _cashSalesInvoiceDetailService.GetObjectsByCashSalesInvoiceId(cashSalesInvoice.Id).Count(),
                            _cashSalesReturnService.GetObjectsByCashSalesInvoiceId(cashSalesInvoice.Id).Count()
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            CashSalesInvoice model = new CashSalesInvoice();
            try
            {
                model = _cashSalesInvoiceService.GetObjectById(Id);
          
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Id,
                model.Description,
                model.SalesDate,
                model.DueDate,
                model.Discount,
                model.Tax,
                model.Allowance,
                model.CashBankId,
                CashBank = _cashBankService.GetObjectById((int)model.CashBankId).Name,
                model.WarehouseId,
                Warehouse = _warehouseService.GetObjectById(model.WarehouseId).Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CashSalesInvoice model)
        {
            try
            {
                model = _cashSalesInvoiceService.CreateObject(model, _warehouseService);
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
        public dynamic Update(CashSalesInvoice model)
        {
            try
            {
                var data = _cashSalesInvoiceService.GetObjectById(model.Id);
                data.Description = model.Description;
                data.SalesDate = model.SalesDate;
                data.DueDate = model.DueDate;
                data.Discount = model.Discount;
                data.Tax = model.Tax;
                data.Allowance = model.Allowance;
                data.CashBankId = model.CashBankId;
                data.WarehouseId = model.WarehouseId;
                model = _cashSalesInvoiceService.UpdateObject(data, _cashSalesInvoiceDetailService);
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
        public dynamic Delete(CashSalesInvoice model)
        {
            try
            {
                var data = _cashSalesInvoiceService.GetObjectById(model.Id);
                model = _cashSalesInvoiceService.SoftDeleteObject(data, _cashSalesInvoiceDetailService);
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
    }
}