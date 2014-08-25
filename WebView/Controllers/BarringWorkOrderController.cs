﻿using System;
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
    public class BarringWorkOrderController : Controller
    {
      private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BarringWorkOrderController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IWarehouseService _warehouseService;
        private IBarringOrderService _barringOrderService;
        private IBarringOrderDetailService _barringOrderDetailService;
        private IContactService _contactService;

        public BarringWorkOrderController()
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
            var query =  _barringOrderService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<BarringOrder>;

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
                           _contactService.GetObjectById(model.ContactId).Name,
                            _warehouseService.GetObjectById(model.WarehouseId).Name,
                            model.QuantityReceived,
                            model.QuantityFinal,
                            model.QuantityRejected,
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

            // Get Data
            var query = _barringOrderDetailService.GetObjectsByBarringOrderId(id).Where(d => d.IsDeleted == false);

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
                            _barringService.GetObjectById(model.BarringId).Sku,
                            _barringService.GetObjectById(model.BarringId).Name,
                            _barringService.GetBlanketItem(_barringService.GetObjectById(model.BarringId)).Sku,
                            _barringService.GetBlanketItem(_barringService.GetObjectById(model.BarringId)).Name,
                            _barringService.GetLeftBarItem(_barringService.GetObjectById(model.BarringId)).Sku,
                            _barringService.GetLeftBarItem(_barringService.GetObjectById(model.BarringId)).Name,
                            _barringService.GetRightBarItem(_barringService.GetObjectById(model.BarringId)).Sku,
                            _barringService.GetRightBarItem(_barringService.GetObjectById(model.BarringId)).Name,
                            model.RejectedDate,
                            model.FinishedDate
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            BarringOrder model = new BarringOrder();
            try
            {
                model = _barringOrderService.GetObjectById(Id);
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
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
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
                BarringSku = _barringService.GetObjectById(model.BarringId).Sku,
                Barring = _barringService.GetObjectById(model.BarringId).Name,
                BlanketSku = _barringService.GetLeftBarItem(_barringService.GetObjectById(model.BarringId)).Sku,
                Blanket = _barringService.GetBlanketItem(_barringService.GetObjectById(model.BarringId)).Name,
                BarringLeftBarSku = _barringService.GetLeftBarItem(_barringService.GetObjectById(model.BarringId)).Sku,
                BarringLeftBar = _barringService.GetLeftBarItem(_barringService.GetObjectById(model.BarringId)).Name,
                BarringRightBarSku = _barringService.GetRightBarItem(_barringService.GetObjectById(model.BarringId)).Sku,
                BarringRightBar = _barringService.GetLeftBarItem(_barringService.GetObjectById(model.BarringId)).Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(BarringOrder model)
        {
            try
            {
                model = _barringOrderService.CreateObject(model);
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
        public dynamic InsertDetail(BarringOrderDetail model)
        {
            try
            {
                model = _barringOrderDetailService.CreateObject(model,_barringOrderService,_barringService);
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
        public dynamic Update(BarringOrder model)
        {
            try
            {
                var data = _barringOrderService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.WarehouseId = model.WarehouseId;
                data.Code = model.Code;
                data.QuantityReceived = model.QuantityReceived;
                model = _barringOrderService.UpdateObject(data,_barringOrderDetailService);
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
        public dynamic Delete(BarringOrder model)
        {
            try
            {
                var data = _barringOrderService.GetObjectById(model.Id);
                model = _barringOrderService.SoftDeleteObject(model, _barringOrderDetailService);
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
        public dynamic DeleteDetail(BarringOrderDetail model)
        {
            try
            {
                var data = _barringOrderDetailService.GetObjectById(model.Id);
                model = _barringOrderDetailService.SoftDeleteObject(data,_barringOrderService);
                 
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
        public dynamic UpdateDetail(BarringOrderDetail model)
        {
            try
            {
                var data = _barringOrderDetailService.GetObjectById(model.Id);
                data.BarringId = model.BarringId;
                model = _barringOrderDetailService.UpdateObject(data,_barringOrderService,_barringService);
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
        public dynamic ProgressDetail(int Id,string Progress)
        {
            var model = new BarringOrderDetail();
            try
            {
                var data = _barringOrderDetailService.GetObjectById(Id);
                if (Progress == "IsCut") { model = _barringOrderDetailService.CutObject(data, _barringOrderService); }
                else if (Progress == "IsSideSealed") { model = _barringOrderDetailService.SideSealObject(data); }
                else if (Progress == "IsBarPrepared") { model = _barringOrderDetailService.PrepareObject(data); }
                else if (Progress == "IsAdhesiveTapeApplied") { model = _barringOrderDetailService.ApplyTapeAdhesiveToObject(data); }
                else if (Progress == "IsBarMounted") { model = _barringOrderDetailService.MountObject(data); }
                else if (Progress == "IsBarHeatPressed") { model = _barringOrderDetailService.HeatPressObject(data); }
                else if (Progress == "IsBarPullOffTested") { model = _barringOrderDetailService.PullOffTestObject(data); }
                else if (Progress == "IsQCAndMarked") { model = _barringOrderDetailService.QCAndMarkObject(data); }
                else if (Progress == "IsPackaged") { model = _barringOrderDetailService.PackageObject(data); }
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
        public dynamic Confirm(BarringOrder model)
        {
            try
            {
                var data = _barringOrderService.GetObjectById(model.Id);
                model = _barringOrderService.ConfirmObject(data,model.ConfirmationDate.Value
                   ,_barringOrderDetailService,_barringService,_itemService,_warehouseItemService);
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
        public dynamic UnConfirm(BarringOrder model)
        {
            try
            {

                var data = _barringOrderService.GetObjectById(model.Id);
                model = _barringOrderService.UnconfirmObject(data,_barringOrderDetailService,_barringService,
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