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
    public class BlanketWorkProcessController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BlanketWorkProcessController");
        private IItemService _itemService;
        private IItemTypeService _itemTypeService;
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

        public BlanketWorkProcessController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
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
            var q = _blanketOrderDetailService.GetQueryable().Include("Blanket").Include("BlanketOrder")
                                              .Include("Item").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.BlanketOrderId,
                             BlanketOrderCode = model.BlanketOrder.Code,
                             model.BlanketId,
                             BlanketSku = model.Blanket.Sku,
                             Blanket = model.Blanket.Name,
                             RollBlanketItemSku = model.Blanket.RollBlanketItem.Sku,
                             RollBlanketItem = model.Blanket.RollBlanketItem.Name,
                             LeftBarItemSku = model.Blanket.LeftBarItem.Sku,
                             LeftBarItem = model.Blanket.LeftBarItem.Name,
                             RightBarItemSku = model.Blanket.RightBarItem.Sku,
                             RightBarItem = model.Blanket.RightBarItem.Name,
                             model.IsCut,
                             model.IsSideSealed,
                             model.IsBarPrepared,
                             model.IsAdhesiveTapeApplied,
                             model.AdhesiveUsage,
                             model.Adhesive2Usage,
                             model.IsBarMounted,
                             model.IsBarHeatPressed,
                             model.IsBarPullOffTested,
                             model.IsQCAndMarked,
                             model.IsPackaged,
                             model.IsRejected,
                             model.RejectedDate,
                             model.IsFinished,
                             model.FinishedDate,
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
                            model.BlanketOrderId,
                            model.BlanketOrderCode,
                            model.BlanketId,
                            model.BlanketSku,
                            model.Blanket,
                            model.RollBlanketItemSku,
                            model.RollBlanketItem,
                            model.LeftBarItemSku,
                            model.LeftBarItem,
                            model.RightBarItemSku,
                            model.RightBarItem,
                            model.IsCut,
                            model.IsSideSealed,
                            model.IsBarPrepared,
                            model.IsAdhesiveTapeApplied,
                            model.AdhesiveUsage,
                            model.Adhesive2Usage,
                            model.IsBarMounted,
                            model.IsBarHeatPressed,
                            model.IsBarPullOffTested,
                            model.IsQCAndMarked,
                            model.IsPackaged,
                            model.IsRejected,
                            model.RejectedDate,
                            model.IsFinished,
                            model.FinishedDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
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
                model.BlanketOrderId,
                _blanketOrderService.GetObjectById(model.BlanketOrderId).Code,
                model.BlanketId,
                BlanketSku = _blanketService.GetObjectById(model.BlanketId).Sku,
                Blanket =_blanketService.GetObjectById(model.BlanketId).Name,
                RollBlanketSku = _blanketService.GetRollBlanketItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                RollBlanket = _blanketService.GetRollBlanketItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                LeftBarSku = _blanketService.GetObjectById(model.BlanketId).HasLeftBar ?
                             _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku : "",
                LeftBar = _blanketService.GetObjectById(model.BlanketId).HasLeftBar ?
                          _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Name : "",
                RightBarSku = _blanketService.GetObjectById(model.BlanketId).HasRightBar ?
                              _blanketService.GetRightBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku : "",
                RightBar = _blanketService.GetObjectById(model.BlanketId).HasRightBar ?
                          _blanketService.GetRightBarItem(_blanketService.GetObjectById(model.BlanketId)).Name : "",
                model.IsCut,
                model.IsSideSealed,
                model.IsBarPrepared,
                model.IsAdhesiveTapeApplied,
                model.AdhesiveUsage,
                model.Adhesive2Usage,
                model.IsBarMounted,
                model.IsBarHeatPressed,
                model.IsBarPullOffTested,
                model.IsQCAndMarked,
                model.IsPackaged,
                model.IsRejected,
                model.RejectedDate,
                model.IsFinished,
                model.FinishedDate,
                HasBar = _blanketService.GetObjectById(model.BlanketId).HasRightBar || 
                         _blanketService.GetObjectById(model.BlanketId).HasLeftBar,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic ProgressDetail(BlanketOrderDetail model)
        {
            var models = new BlanketOrderDetail();
            models.Errors = new Dictionary<string, string>();
            decimal usage = model.AdhesiveUsage;
            decimal usage2 = model.Adhesive2Usage;
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                if (model.IsCut && !data.IsCut) { models = _blanketOrderDetailService.CutObject(data, _blanketOrderService); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsSideSealed && !data.IsSideSealed) { models = _blanketOrderDetailService.SideSealObject(data); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsBarPrepared && !data.IsBarPrepared) { models = _blanketOrderDetailService.PrepareObject(data, _blanketService); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsAdhesiveTapeApplied && !data.IsAdhesiveTapeApplied) { models = _blanketOrderDetailService.ApplyTapeAdhesiveToObject(data, model.AdhesiveUsage, model.Adhesive2Usage, _blanketService); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsBarMounted && !data.IsBarMounted) { models = _blanketOrderDetailService.MountObject(data, _blanketService); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsBarHeatPressed && !data.IsBarHeatPressed) { models = _blanketOrderDetailService.HeatPressObject(data, _blanketService); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsBarPullOffTested && !data.IsBarPullOffTested) { models = _blanketOrderDetailService.PullOffTestObject(data, _blanketService); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsQCAndMarked && !data.IsQCAndMarked) { models = _blanketOrderDetailService.QCAndMarkObject(data, _blanketService); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsPackaged  && !data.IsPackaged) { models = _blanketOrderDetailService.PackageObject(data); }
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                models.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                models.Errors
            });
        }


       

        [HttpPost]
        public dynamic Finish(BlanketOrderDetail model)
        {
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                model = _blanketOrderDetailService.FinishObject(data, model.FinishedDate.Value, _blanketOrderService, _stockMutationService
                    , _blanketService, _itemService, _itemTypeService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
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
                    , _blanketService, _itemService, _itemTypeService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
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
                    ,_blanketService,_itemService, _itemTypeService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
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
                    ,_blanketService,_itemService, _itemTypeService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
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