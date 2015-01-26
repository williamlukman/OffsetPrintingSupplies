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
    public class SalesQuotationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SalesQuotationController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private ISalesQuotationService _salesQuotationService;
        private ISalesQuotationDetailService _salesQuotationDetailService;
        private IContactService _contactService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private ISalesOrderService _salesOrderService;

        public SalesQuotationController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _salesQuotationService = new SalesQuotationService(new SalesQuotationRepository(), new SalesQuotationValidator());
            _salesQuotationDetailService = new SalesQuotationDetailService(new SalesQuotationDetailRepository(), new SalesQuotationDetailValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
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
            var q = _salesQuotationService.GetQueryable().Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.VersionNo,
                             model.NomorSurat,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.QuotationDate,
                             model.TotalQuotedAmount,
                             model.TotalRRPAmount,
                             model.CostSaved,
                             model.PercentageSaved,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.IsApproved,
                             model.IsRejected,
                             model.Catatan,
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
                            model.VersionNo,
                            model.NomorSurat,
                            model.ContactId,
                            model.Contact,
                            model.QuotationDate,
                            model.TotalQuotedAmount,
                            model.TotalRRPAmount,
                            model.CostSaved,
                            model.PercentageSaved,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.IsApproved,
                            model.IsRejected,
                            model.Catatan,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListApproved(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _salesQuotationService.GetQueryable().Include("Contact")
                                      .Where(x => x.IsApproved && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.VersionNo,
                             model.NomorSurat,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.QuotationDate,
                             model.TotalQuotedAmount,
                             model.TotalRRPAmount,
                             model.CostSaved,
                             model.PercentageSaved,
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
                            model.VersionNo,
                            model.NomorSurat,
                            model.ContactId,
                            model.Contact,
                            model.QuotationDate,
                            model.TotalQuotedAmount,
                            model.TotalRRPAmount,
                            model.CostSaved,
                            model.PercentageSaved,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _salesQuotationDetailService.GetQueryable().Include("Item").Where(x => x.SalesQuotationId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ItemId,
                             ItemSku = model.Item.Sku,
                             Item = model.Item.Name,
                             model.Quantity,
                             model.QuotationPrice,
                             model.RRP
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
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                            model.Quantity,
                            model.QuotationPrice,
                            model.RRP
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            SalesQuotation model = new SalesQuotation();
            try
            {
                model = _salesQuotationService.GetObjectById(Id);
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
                model.VersionNo,
                model.NomorSurat,
                model.ContactId,
                Contact = _contactService.GetObjectById(model.ContactId).Name,
                model.QuotationDate,
                model.TotalQuotedAmount,
                model.TotalRRPAmount,
                model.CostSaved,
                model.PercentageSaved,
                ConfirmationDate = model.ConfirmationDate,
                model.Catatan,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            SalesQuotationDetail model = new SalesQuotationDetail();
            try
            {
                model = _salesQuotationDetailService.GetObjectById(Id);

            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.ItemId,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                model.Quantity,
                model.QuotationPrice,
                model.RRP,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(SalesQuotation model)
        {
            try
            {
                model = _salesQuotationService.CreateObject(model, _contactService);
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
        public dynamic InsertDetail(SalesQuotationDetail model)
        {
            try
            {
                model = _salesQuotationDetailService.CreateObject(model, _salesQuotationService, _itemService);
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
        public dynamic Update(SalesQuotation model)
        {
            try
            {
                var data = _salesQuotationService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.QuotationDate = model.QuotationDate;
                data.VersionNo = model.VersionNo;
                data.NomorSurat = model.NomorSurat;
                data.Catatan = model.Catatan;
                model = _salesQuotationService.UpdateObject(data, _contactService);
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
        public dynamic Delete(SalesQuotation model)
        {
            try
            {
                var data = _salesQuotationService.GetObjectById(model.Id);
                model = _salesQuotationService.SoftDeleteObject(data, _salesQuotationDetailService);
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
        public dynamic DeleteDetail(SalesQuotationDetail model)
        {
            try
            {
                var data = _salesQuotationDetailService.GetObjectById(model.Id);
                model = _salesQuotationDetailService.SoftDeleteObject(data);
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
        public dynamic UpdateDetail(SalesQuotationDetail model)
        {
            try
            {
                var data = _salesQuotationDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                data.QuotationPrice = model.QuotationPrice;
                data.RRP = model.RRP;
                model = _salesQuotationDetailService.UpdateObject(data, _salesQuotationService, _itemService);
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
        public dynamic Confirm(SalesQuotation model)
        {
            try
            {
                var data = _salesQuotationService.GetObjectById(model.Id);
                model = _salesQuotationService.ConfirmObject(data, model.ConfirmationDate.Value, _salesQuotationDetailService, _itemService, _warehouseItemService);
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
        public dynamic UnConfirm(SalesQuotation model)
        {
            try
            {

                var data = _salesQuotationService.GetObjectById(model.Id);
                model = _salesQuotationService.UnconfirmObject(data, _salesQuotationDetailService, _itemService, _warehouseItemService, _salesOrderService, _salesOrderDetailService);
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
        public dynamic Approve(SalesQuotation model)
        {
            try
            {
                var data = _salesQuotationService.GetObjectById(model.Id);
                model = _salesQuotationService.ApproveObject(data);
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
        public dynamic Reject(SalesQuotation model)
        {
            try
            {
                var data = _salesQuotationService.GetObjectById(model.Id);
                model = _salesQuotationService.RejectObject(data);
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
