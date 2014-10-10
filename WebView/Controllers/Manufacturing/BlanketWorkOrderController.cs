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
    public class BlanketWorkOrderController : Controller
    {
      private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BlanketWorkOrderController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IWarehouseService _warehouseService;
        private IBlanketOrderService _blanketOrderService;
        private IBlanketOrderDetailService _blanketOrderDetailService;
        private IContactService _contactService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;

        public BlanketWorkOrderController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _blanketOrderService = new BlanketOrderService(new BlanketOrderRepository(), new BlanketOrderValidator());
            _blanketOrderDetailService = new BlanketOrderDetailService(new BlanketOrderDetailRepository(), new BlanketOrderDetailValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
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
            var q = _blanketOrderService.GetQueryable().Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             Contact = model.Contact.Name,
                             Warehouse = model.Warehouse.Name,
                             model.QuantityReceived,
                             model.QuantityFinal,
                             model.QuantityRejected,
                             model.DueDate,
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
                            model.Contact,
                            model.Warehouse,
                            model.QuantityReceived,
                            model.QuantityFinal,
                            model.QuantityRejected,
                            model.DueDate,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListIncomplete(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _blanketOrderService.GetQueryable().Where(x => !x.IsDeleted && !x.IsCompleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             Contact = model.Contact.Name,
                             Warehouse = model.Warehouse.Name,
                             model.QuantityReceived,
                             model.QuantityFinal,
                             model.QuantityRejected,
                             model.DueDate,
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
                            model.Contact,
                            model.Warehouse,
                            model.QuantityReceived,
                            model.QuantityFinal,
                            model.QuantityRejected,
                            model.DueDate,
                            model.ConfirmationDate,
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
            var q = _blanketOrderDetailService.GetQueryable().Include("Blanket").Include("BlanketOrder")
                                              .Include("Item").Where(x => x.BlanketOrderId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.BlanketId,
                             BlanketSku = model.Blanket.Sku,
                             Blanket = model.Blanket.Name,
                             model.Blanket.RollBlanketItemId,
                             RollBlanketItemSku = model.Blanket.RollBlanketItem.Sku,
                             RollBlanketItem = model.Blanket.RollBlanketItem.Name,
                             model.Blanket.LeftBarItemId,
                             LeftBarItemSku = model.Blanket.LeftBarItem.Sku,
                             LeftBarItem = model.Blanket.LeftBarItem.Name,
                             model.Blanket.RightBarItemId,
                             RightBarItemSku = model.Blanket.RightBarItem.Sku,
                             RightBarItem = model.Blanket.RightBarItem.Name,
                             model.RejectedDate,
                             model.FinishedDate
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
                             model.BlanketSku,
                             model.Blanket,
                             model.RollBlanketItemSku,
                             model.RollBlanketItem,
                             model.LeftBarItemSku,
                             model.LeftBarItem,
                             model.RightBarItemSku,
                             model.RightBarItem,
                             model.RejectedDate,
                             model.FinishedDate
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            BlanketOrder model = new BlanketOrder();
            try
            {
                model = _blanketOrderService.GetObjectById(Id);
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
                model.WarehouseId,
                WarehouseCode = _warehouseService.GetObjectById(model.WarehouseId).Code,
                Warehouse = _warehouseService.GetObjectById(model.WarehouseId).Name,
                model.QuantityReceived,
                model.HasDueDate,
                model.DueDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            BlanketOrderDetail model = new BlanketOrderDetail();
            try
            {
                model = _blanketOrderDetailService.GetObjectById(Id);
            
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                BlanketSku = _blanketService.GetObjectById(model.BlanketId).Sku,
                Blanket = _blanketService.GetObjectById(model.BlanketId).Name,
                RollBlanketSku = _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                RollBlanket = _blanketService.GetRollBlanketItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                BlanketLeftBarSku = _blanketService.GetObjectById(model.BlanketId).HasLeftBar ?
                                    _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku : "",
                BlanketLeftBar = _blanketService.GetObjectById(model.BlanketId).HasLeftBar ?
                                 _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Name : "",
                BlanketRightBarSku = _blanketService.GetObjectById(model.BlanketId).HasRightBar ?
                                     _blanketService.GetRightBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku : "",
                BlanketRightBar = _blanketService.GetObjectById(model.BlanketId).HasRightBar ?
                                  _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Name : "",                                  
                model.RejectedDate,
                model.FinishedDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(BlanketOrder model)
        {
            try
            {
                model = _blanketOrderService.CreateObject(model);
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
        public dynamic InsertDetail(BlanketOrderDetail model)
        {
            try
            {
                model = _blanketOrderDetailService.CreateObject(model,_blanketOrderService,_blanketService);
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
        public dynamic Update(BlanketOrder model)
        {
            try
            {
                var data = _blanketOrderService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.WarehouseId = model.WarehouseId;
                data.Code = model.Code;
                data.QuantityReceived = model.QuantityReceived;
                model = _blanketOrderService.UpdateObject(data,_blanketOrderDetailService);
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
        public dynamic Delete(BlanketOrder model)
        {
            try
            {
                var data = _blanketOrderService.GetObjectById(model.Id);
                model = _blanketOrderService.SoftDeleteObject(model, _blanketOrderDetailService);
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
        public dynamic DeleteDetail(BlanketOrderDetail model)
        {
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                model = _blanketOrderDetailService.SoftDeleteObject(data,_blanketOrderService);
                 
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
        public dynamic UpdateDetail(BlanketOrderDetail model)
        {
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                data.BlanketId = model.BlanketId;
                model = _blanketOrderDetailService.UpdateObject(data,_blanketOrderService,_blanketService);
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
        public dynamic Confirm(BlanketOrder model)
        {
            try
            {
                var data = _blanketOrderService.GetObjectById(model.Id);
                model = _blanketOrderService.ConfirmObject(data,model.ConfirmationDate.Value
                   ,_blanketOrderDetailService,_blanketService,_itemService,_warehouseItemService);
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
        public dynamic UnConfirm(BlanketOrder model)
        {
            try
            {

                var data = _blanketOrderService.GetObjectById(model.Id);
                model = _blanketOrderService.UnconfirmObject(data,_blanketOrderDetailService,_blanketService,
                    _itemService,_warehouseItemService);
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
        public dynamic Finish(BlanketOrderDetail model)
        {
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                model = _blanketOrderDetailService.FinishObject(data, model.FinishedDate.Value, _blanketOrderService, _stockMutationService
                    , _blanketService, _itemService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Finish Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnFinish(BlanketOrderDetail model)
        {
            try
            {

                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                model = _blanketOrderDetailService.UnfinishObject(data, _blanketOrderService, _stockMutationService
                    , _blanketService, _itemService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unfinish Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Reject(BlanketOrderDetail model)
        {
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                model = _blanketOrderDetailService.RejectObject(data,model.RejectedDate.Value,_blanketOrderService,_stockMutationService
                    ,_blanketService,_itemService,_warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Reject Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnReject(BlanketOrderDetail model)
        {
            try
            {

                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                model = _blanketOrderDetailService.UndoRejectObject(data,_blanketOrderService,_stockMutationService
                    ,_blanketService,_itemService,_warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unreject Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

    }
}