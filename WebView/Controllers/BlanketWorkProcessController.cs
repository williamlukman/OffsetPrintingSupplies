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
    public class BlanketWorkProcessController : Controller
    {
      private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BlanketWorkProcessController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IWarehouseService _warehouseService;
        private IBlanketOrderService _blanketOrderService;
        private IBlanketOrderDetailService _blanketOrderDetailService;
        private IContactService _contactService;

        public BlanketWorkProcessController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _blanketOrderService = new BlanketOrderService(new BlanketOrderRepository(), new BlanketOrderValidator());
            _blanketOrderDetailService = new BlanketOrderDetailService(new BlanketOrderDetailRepository(), new BlanketOrderDetailValidator());
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

            var query = _blanketOrderDetailService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<BlanketOrderDetail>;

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
                            _blanketOrderService.GetObjectById(model.BlanketOrderId).Code,
                            model.BlanketId,
                            _blanketService.GetObjectById(model.BlanketId).Sku,
                            _blanketService.GetObjectById(model.BlanketId).Name,
                            _blanketService.GetRollBlanketItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                            _blanketService.GetRollBlanketItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                            _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                            _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                            _blanketService.GetRightBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                            _blanketService.GetRightBarItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                            model.IsCut,
                            model.IsSideSealed,
                            model.IsBarPrepared,
                            model.IsAdhesiveTapeApplied,
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
                LeftBarSku = _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                LeftBar = _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                RightBarSku = _blanketService.GetRightBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                RightBar = _blanketService.GetRightBarItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                model.IsCut,
                model.IsSideSealed,
                model.IsBarPrepared,
                model.IsAdhesiveTapeApplied,
                model.IsBarMounted,
                model.IsBarHeatPressed,
                model.IsBarPullOffTested,
                model.IsQCAndMarked,
                model.IsPackaged,
                model.IsRejected,
                model.RejectedDate,
                model.IsFinished,
                model.FinishedDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

     


        [HttpPost]
        public dynamic ProgressDetail(BlanketOrderDetail model)
        {
            var models = new BlanketOrderDetail();
            models.Errors = new Dictionary<string, string>();
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                if (model.IsCut && !data.IsCut) { models = _blanketOrderDetailService.CutObject(data, _blanketOrderService); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsSideSealed && !data.IsSideSealed) { models = _blanketOrderDetailService.SideSealObject(data); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsBarPrepared && !data.IsBarPrepared) { models = _blanketOrderDetailService.PrepareObject(data); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsAdhesiveTapeApplied && !data.IsAdhesiveTapeApplied) { models = _blanketOrderDetailService.ApplyTapeAdhesiveToObject(data); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsBarMounted && !data.IsBarMounted) { models = _blanketOrderDetailService.MountObject(data); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsBarHeatPressed && !data.IsBarHeatPressed) { models = _blanketOrderDetailService.HeatPressObject(data); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsBarPullOffTested && !data.IsBarPullOffTested) { models = _blanketOrderDetailService.PullOffTestObject(data); }
                if (models.Errors.Any()) { return Json(new { models.Errors }); }
                if (model.IsQCAndMarked && !data.IsQCAndMarked) { models = _blanketOrderDetailService.QCAndMarkObject(data); }
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
                    , _blanketService, _itemService, _warehouseItemService);
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
                    , _blanketService, _itemService, _warehouseItemService);
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
                    ,_blanketService,_itemService,_warehouseItemService);
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
                    ,_blanketService,_itemService,_warehouseItemService);
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