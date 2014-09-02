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
    public class MstBlanketController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BlanketController");
        private IItemService _itemService;
        private IUoMService _uomService;
        private IWarehouseService _warehouseService;
        private IItemTypeService _itemTypeService;
        private IMachineService _machineService;
        private IWarehouseItemService _warehouseItemService;
        private IBlanketService _blanketService;
        private IContactService _contactService;
        private IContactGroupService _contactGroupService;
        private IPriceMutationService _priceMutationService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private IStockMutationService _stockMutationService;
        private IBlanketOrderDetailService _blanketOrderDetailService;

        public MstBlanketController()
        {
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
             _machineService = new MachineService(new MachineRepository(), new MachineValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _contactService = new ContactService(new ContactRepository(),new ContactValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
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
            var query = _blanketService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<Blanket>;

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
                            model.Sku,
                            model.Name,
                            model.RollNo,
                            model.Quantity,
                            _uomService.GetObjectById(model.UoMId).Name,
                            model.AC,
                            model.AR,
                            model.thickness,
                            model.KS,
                            model.Category,
                            model.Description,
                            _itemTypeService.GetObjectById(model.ItemTypeId).Name,
                            _machineService.GetObjectById(model.MachineId).Name,
                            _itemService.GetObjectById(model.AdhesiveId).Name,
                            _itemService.GetObjectById(model.RollBlanketItemId).Name,
                            model.HasLeftBar ? _itemService.GetObjectById(model.LeftBarItemId.Value).Name : "",
                            model.HasRightBar ? _itemService.GetObjectById(model.RightBarItemId.Value).Name : "",
                            _contactService.GetObjectById(model.ContactId).Name,
                            model.ApplicationCase,
                            model.CroppingType,
                            model.LeftOverAC,
                            model.LeftOverAR,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListLookUpItems(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _blanketService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<Blanket>;

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
                            model.Sku,
                            model.Name,
                            _itemService.GetObjectById(model.RollBlanketItemId).Sku,
                            _itemService.GetObjectById(model.RollBlanketItemId).Name,
                            model.HasLeftBar ? _itemService.GetObjectById(model.LeftBarItemId.Value).Sku : "",
                            model.HasLeftBar ? _itemService.GetObjectById(model.LeftBarItemId.Value).Name : "",
                            model.HasRightBar ? _itemService.GetObjectById(model.RightBarItemId.Value).Sku : "",
                            model.HasRightBar ? _itemService.GetObjectById(model.RightBarItemId.Value).Name : "",
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListRollBlanket(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            int itemtypeId = _itemTypeService.GetObjectByName("RollBlanket").Id;
            var query = _itemService.GetAll().Where(d => d.IsDeleted == false && d.ItemTypeId == itemtypeId);

            var list = query as IEnumerable<Item>;

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
                            model.Name,
                            model.Sku,
                            model.Category,
                            model.Description,
                            model.UoMId,
                            _uomService.GetObjectById(model.UoMId).Name,
                            model.Quantity,
                            model.PendingReceival,
                            model.PendingDelivery,
                            model.CreatedAt,
                            model.UpdatedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListBar(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            int itemtypeId = _itemTypeService.GetObjectByName("Bar").Id;
            var query = _itemService.GetAll().Where(d => d.IsDeleted == false && d.ItemTypeId == itemtypeId);

            var list = query as IEnumerable<Item>;

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
                            model.Name,
                            model.Sku,
                            model.Category,
                            model.Description,
                            model.UoMId,
                            _uomService.GetObjectById(model.UoMId).Name,
                            model.Quantity,
                            model.PendingReceival,
                            model.PendingDelivery,
                            model.CreatedAt,
                            model.UpdatedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListAdhesive(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            int itemtypeId = _itemTypeService.GetObjectByName("Adhesive").Id;
            var query = _itemService.GetAll().Where(d => d.IsDeleted == false && d.ItemTypeId == itemtypeId);

            var list = query as IEnumerable<Item>;

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
                            model.Name,
                            model.Sku,
                            model.Category,
                            model.Description,
                            model.Quantity,
                            model.PendingReceival,
                            model.PendingDelivery,
                            model.UoMId,
                            _uomService.GetObjectById(model.UoMId).Name,
                            model.CreatedAt,
                            model.UpdatedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            Blanket model = new Blanket();
            try
            {
                model = _blanketService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Name,
                model.Category,
                model.Description,
                model.ItemTypeId,
                ItemType = _itemTypeService.GetObjectById(model.ItemTypeId).Name,
                model.UoMId,
                UoM = _uomService.GetObjectById(model.UoMId).Name,
                model.Sku,
                model.RollNo,
                model.ContactId,
                Contact =_contactService.GetObjectById(model.ContactId).Name,
                model.MachineId,
                Machine = _machineService.GetObjectById(model.MachineId).Name,
                model.AdhesiveId,
                Adhesive = _itemService.GetObjectById(model.AdhesiveId).Name,
                model.RollBlanketItemId,
                RollBlanketItem = _itemService.GetObjectById(model.RollBlanketItemId).Name,
                model.IsBarRequired,
                model.HasLeftBar,
                model.HasRightBar,
                LeftBarItemId = model.LeftBarItemId.HasValue ? model.LeftBarItemId.Value : model.LeftBarItemId = null,
                LeftBarItem = model.LeftBarItemId.HasValue ? _itemService.GetObjectById(model.LeftBarItemId.Value).Name : "",
                RightBarItemId = model.RightBarItemId.HasValue ? model.RightBarItemId.Value : model.RightBarItemId = null,
                RightBarItem = model.RightBarItemId.HasValue ? _itemService.GetObjectById(model.RightBarItemId.Value).Name : "",
                model.AC,
                model.AR, 
                model.thickness,
                model.KS,
                model.Quantity,
                model.ApplicationCase,
                model.CroppingType,
                model.LeftOverAC,
                model.LeftOverAR,
                model.CreatedAt,
                model.UpdatedAt,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(Blanket model)
        {
            try
            {
                model = _blanketService.CreateObject(model,_blanketService,_uomService,
                    _itemService,_itemTypeService,_contactService,_machineService,_warehouseItemService,
                    _warehouseService,_priceMutationService,_contactGroupService);
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
        public dynamic Update(Blanket model)
        {
            try
            {
                var data = _blanketService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.RollNo = model.RollNo;
                data.UoMId = model.UoMId;
                data.AC = model.AC;
                data.AR = model.AR;
                data.thickness = model.thickness;
                data.KS = model.KS;
                data.Description = model.Description;
                data.MachineId = model.MachineId;
                data.AdhesiveId = model.AdhesiveId;
                data.RollBlanketItemId = model.RollBlanketItemId;
                data.IsBarRequired = model.IsBarRequired;
                data.HasLeftBar = model.HasLeftBar;
                data.LeftBarItemId = model.LeftBarItemId;
                data.HasRightBar = model.HasRightBar;
                data.ContactId = model.ContactId;
                data.ApplicationCase = model.ApplicationCase;
                data.CroppingType = model.CroppingType;
                data.LeftOverAC = model.LeftOverAC;
                data.LeftOverAR = model.LeftOverAR;
                model = _blanketService.UpdateObject(data,_blanketService,_uomService,_itemService,_itemTypeService,_contactService,_machineService,_warehouseItemService,_warehouseService,_contactGroupService,_priceMutationService);
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
        public dynamic Delete(Blanket model)
        {
            try
            {
                var data = _blanketService.GetObjectById(model.Id);
                model = _blanketService.SoftDeleteObject(data,_itemTypeService,_warehouseItemService,_priceMutationService, _purchaseOrderDetailService,
                                                        _stockAdjustmentDetailService, _salesOrderDetailService,_stockMutationService, _blanketOrderDetailService);
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
    }
}
