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
using Core.Constants;

namespace WebView.Controllers
{
    public class PurchaseReceivalController : Controller
    {
       private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PurchaseReceivalController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IWarehouseService _warehouseService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private IExchangeRateService _exchangeRateService;

        public PurchaseReceivalController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
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
            var q = _purchaseReceivalService.GetQueryable().Include("PurchaseOrder").Include("Warehouse").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.NomorSurat,
                             model.PurchaseOrderId,
                             PurchaseOrderCode = model.PurchaseOrder.Code,
                             model.WarehouseId,
                             Warehouse = model.Warehouse.Name,
                             model.ReceivalDate,
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
                            model.NomorSurat,
                            model.PurchaseOrderId,
                            model.PurchaseOrderCode,
                            model.WarehouseId,
                            model.Warehouse,
                            model.ReceivalDate,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListConfirmed(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _purchaseReceivalService.GetQueryable().Where(x => x.IsConfirmed && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.NomorSurat,
                             model.PurchaseOrderId,
                             PurchaseOrderCode = model.PurchaseOrder.Code,
                             NomorSuratPO = model.PurchaseOrder.NomorSurat,
                             model.WarehouseId,
                             Warehouse = model.Warehouse.Name,
                             CurrencyId = model.PurchaseOrder.CurrencyId,
                             Currency = model.PurchaseOrder.Currency.Name,
                             model.ReceivalDate,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.CreatedAt,
                             model.UpdatedAt,
                             Tax = (model.PurchaseOrder.Contact.TaxCode == "01") ? Constant.TaxValue.Code01 :
                                   (model.PurchaseOrder.Contact.TaxCode == "02") ? Constant.TaxValue.Code02 :
                                   (model.PurchaseOrder.Contact.TaxCode == "03") ? Constant.TaxValue.Code03 :
                                   (model.PurchaseOrder.Contact.TaxCode == "04") ? Constant.TaxValue.Code04 :
                                   (model.PurchaseOrder.Contact.TaxCode == "05") ? Constant.TaxValue.Code05 :
                                   (model.PurchaseOrder.Contact.TaxCode == "06") ? Constant.TaxValue.Code06 :
                                   (model.PurchaseOrder.Contact.TaxCode == "07") ? Constant.TaxValue.Code07 :
                                   (model.PurchaseOrder.Contact.TaxCode == "08") ? Constant.TaxValue.Code08 :
                                   (model.PurchaseOrder.Contact.TaxCode == "09") ? Constant.TaxValue.Code09 : 0   
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
                            model.NomorSurat,
                            model.PurchaseOrderId,
                            model.PurchaseOrderCode,
                            model.NomorSuratPO,
                            model.WarehouseId,
                            model.Warehouse,
                            model.ReceivalDate,
                            model.CurrencyId,
                            model.Currency,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                            model.Tax
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
            var q = _purchaseReceivalDetailService.GetQueryable().Include("PurchaseOrderDetail").Include("Item")
                                            .Where(x => x.PurchaseReceivalId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.PurchaseOrderDetailId,
                             PurchaseOrderDetailCode = model.PurchaseOrderDetail.Code,
                             model.ItemId,
                             ItemSku = model.Item.Sku,
                             Item = model.Item.Name,
                             model.Quantity,
                             Price = model.PurchaseOrderDetail.Price,
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
                            model.PurchaseOrderDetailId,
                            model.PurchaseOrderDetailCode,
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                            model.Quantity,
                            model.Price,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            PurchaseReceival model = new PurchaseReceival();
            try
            {
                model = _purchaseReceivalService.GetObjectById(Id);
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
                model.NomorSurat,
                model.PurchaseOrderId,
                PurchaseOrder = _purchaseOrderService.GetObjectById(model.PurchaseOrderId).Code,
                model.ReceivalDate, 
                model.WarehouseId, 
                Warehouse =_warehouseService.GetObjectById(model.WarehouseId).Name,
                ConfirmationDate = model.ConfirmationDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            PurchaseReceivalDetail model = new PurchaseReceivalDetail();
            try
            {
                model = _purchaseReceivalDetailService.GetObjectById(Id);
            
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
                model.PurchaseOrderDetailId,
                PurchaseOrderDetail = _purchaseOrderDetailService.GetObjectById(model.PurchaseOrderDetailId).Code,
                model.ItemId,
                ItemSku = _itemService.GetObjectById(model.ItemId).Sku,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                model.Quantity,
                Price = _purchaseOrderDetailService.GetObjectById(model.PurchaseOrderDetailId).Price,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(PurchaseReceival model)
        {
            try
            {             
                model = _purchaseReceivalService.CreateObject(model,_purchaseOrderService,_warehouseService);
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
        public dynamic InsertDetail(PurchaseReceivalDetail model)
        {
            try
            {
                model = _purchaseReceivalDetailService.CreateObject(model,_purchaseReceivalService,_purchaseOrderDetailService,_purchaseOrderService,_itemService);
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
        public dynamic Update(PurchaseReceival model)
        {
            try
            {
                var data = _purchaseReceivalService.GetObjectById(model.Id);
                data.PurchaseOrderId = model.PurchaseOrderId;
                data.ReceivalDate = model.ReceivalDate;
                data.WarehouseId = model.WarehouseId;
                data.NomorSurat = model.NomorSurat;
                model = _purchaseReceivalService.UpdateObject(data,_purchaseOrderService,_warehouseService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(PurchaseReceival model)
        {
            try
            {
                var data = _purchaseReceivalService.GetObjectById(model.Id);
                model = _purchaseReceivalService.SoftDeleteObject(data,_purchaseReceivalDetailService);
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
        public dynamic DeleteDetail(PurchaseReceivalDetail model)
        {
            try
            {
                var data = _purchaseReceivalDetailService.GetObjectById(model.Id);
                model = _purchaseReceivalDetailService.SoftDeleteObject(data);
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
        public dynamic UpdateDetail(PurchaseReceivalDetail model)
        {
            try
            {
                var data = _purchaseReceivalDetailService.GetObjectById(model.Id);
                data.PurchaseOrderDetailId = model.PurchaseOrderDetailId;
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                model = _purchaseReceivalDetailService.UpdateObject(data,_purchaseReceivalService,_purchaseOrderDetailService,_purchaseOrderService,_itemService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }


        [HttpPost]
        public dynamic Confirm(PurchaseReceival model)
        {
            try
            {
                var data = _purchaseReceivalService.GetObjectById(model.Id);
                model = _purchaseReceivalService.ConfirmObject(data,model.ConfirmationDate.Value,_purchaseReceivalDetailService,_purchaseOrderService,
                        _purchaseOrderDetailService,_stockMutationService,_itemService,_blanketService,_warehouseItemService,_accountService, _generalLedgerJournalService, _closingService,_exchangeRateService);
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
        public dynamic UnConfirm(PurchaseReceival model)
        {
            try
            {
                var data = _purchaseReceivalService.GetObjectById(model.Id);
                model = _purchaseReceivalService.UnconfirmObject(data,_purchaseReceivalDetailService,_purchaseInvoiceService,
                        _purchaseInvoiceDetailService,_purchaseOrderService,_purchaseOrderDetailService,_stockMutationService,
                        _itemService,_blanketService,_warehouseItemService,_accountService, _generalLedgerJournalService, _closingService);
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
