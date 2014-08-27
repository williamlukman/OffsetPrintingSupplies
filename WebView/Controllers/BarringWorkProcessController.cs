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
    public class BarringWorkProcessController : Controller
    {
      private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BarringWorkProcessController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IWarehouseService _warehouseService;
        private IBarringOrderService _barringOrderService;
        private IBarringOrderDetailService _barringOrderDetailService;
        private IContactService _contactService;

        public BarringWorkProcessController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _barringOrderService = new BarringOrderService(new BarringOrderRepository(), new BarringOrderValidator());
            _barringOrderDetailService = new BarringOrderDetailService(new BarringOrderDetailRepository(), new BarringOrderDetailValidator());
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

            var query = _barringOrderDetailService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<BarringOrderDetail>;

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
                            model.BarringOrderId,
                            model.BarringId,
                            _barringService.GetObjectById(model.BarringId).Sku,
                            _barringService.GetObjectById(model.BarringId).Name,
                            _barringService.GetBlanketItem(_barringService.GetObjectById(model.BarringId)).Sku,
                            _barringService.GetBlanketItem(_barringService.GetObjectById(model.BarringId)).Name,
                            _barringService.GetLeftBarItem(_barringService.GetObjectById(model.BarringId)).Sku,
                            _barringService.GetLeftBarItem(_barringService.GetObjectById(model.BarringId)).Name,
                            _barringService.GetRightBarItem(_barringService.GetObjectById(model.BarringId)).Sku,
                            _barringService.GetRightBarItem(_barringService.GetObjectById(model.BarringId)).Name,
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
            BarringOrderDetail model = new BarringOrderDetail();
            try
            {
                model = _barringOrderDetailService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.BarringOrderId,
                model.BarringId,
                BarringSku = _barringService.GetObjectById(model.BarringId).Sku,
                Barring =_barringService.GetObjectById(model.BarringId).Name,
                BlanketSku = _barringService.GetBlanketItem(_barringService.GetObjectById(model.BarringId)).Sku,
                Blanket = _barringService.GetBlanketItem(_barringService.GetObjectById(model.BarringId)).Name,
                LeftBarSku = _barringService.GetLeftBarItem(_barringService.GetObjectById(model.BarringId)).Sku,
                LeftBar = _barringService.GetLeftBarItem(_barringService.GetObjectById(model.BarringId)).Name,
                RightBarSku = _barringService.GetRightBarItem(_barringService.GetObjectById(model.BarringId)).Sku,
                RightBar = _barringService.GetRightBarItem(_barringService.GetObjectById(model.BarringId)).Name,
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
        public dynamic ProgressDetail(BarringOrderDetail model)
        {
            var models = new BarringOrderDetail();
            try
            {
                var data = _barringOrderDetailService.GetObjectById(model.Id);
                if (model.IsCut == true) { models = _barringOrderDetailService.CutObject(data, _barringOrderService); }
                if (model.IsSideSealed == true) { models = _barringOrderDetailService.SideSealObject(data); }
                if (model.IsBarPrepared == true) { models = _barringOrderDetailService.PrepareObject(data); }
                if (model.IsAdhesiveTapeApplied == true) { models = _barringOrderDetailService.ApplyTapeAdhesiveToObject(data); }
                if (model.IsBarMounted == true) { models = _barringOrderDetailService.MountObject(data); }
                if (model.IsBarHeatPressed == true) { models = _barringOrderDetailService.HeatPressObject(data); }
                if (model.IsBarPullOffTested == true) { models = _barringOrderDetailService.PullOffTestObject(data); }
                if (model.IsQCAndMarked == true) { models = _barringOrderDetailService.QCAndMarkObject(data); }
                if (model.IsPackaged == true) { models = _barringOrderDetailService.PackageObject(data); }
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
        public dynamic Finish(BarringOrderDetail model)
        {
            try
            {
                var data = _barringOrderDetailService.GetObjectById(model.Id);
                model = _barringOrderDetailService.FinishObject(data, model.FinishedDate.Value, _barringOrderService, _stockMutationService
                    , _barringService, _itemService, _warehouseItemService);
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
        public dynamic UnFinish(BarringOrderDetail model)
        {
            try
            {

                var data = _barringOrderDetailService.GetObjectById(model.Id);
                model = _barringOrderDetailService.UnfinishObject(data, _barringOrderService, _stockMutationService
                    , _barringService, _itemService, _warehouseItemService);
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
        public dynamic Reject(BarringOrderDetail model)
        {
            try
            {
                var data = _barringOrderDetailService.GetObjectById(model.Id);
                model = _barringOrderDetailService.RejectObject(data,model.RejectedDate.Value,_barringOrderService,_stockMutationService
                    ,_barringService,_itemService,_warehouseItemService);
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
        public dynamic UnReject(BarringOrderDetail model)
        {
            try
            {

                var data = _barringOrderDetailService.GetObjectById(model.Id);
                model = _barringOrderDetailService.UndoRejectObject(data,_barringOrderService,_stockMutationService
                    ,_barringService,_itemService,_warehouseItemService);
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