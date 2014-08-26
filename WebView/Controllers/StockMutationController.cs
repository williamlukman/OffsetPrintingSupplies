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
    public class StockMutationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("StockMutationController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IWarehouseService _warehouseService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IUoMService _uomService;

        public StockMutationController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(),new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(),new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
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
            var query = _stockMutationService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<StockMutation>;

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
                    from stockmutation in list
                    select new
                    {
                        id = stockmutation.Id,
                        cell = new object[] {
                            stockmutation.Id,
                            stockmutation.ItemId,
                            _itemService.GetObjectById(stockmutation.ItemId).Sku,
                            _itemService.GetObjectById(stockmutation.ItemId).Name,
                            stockmutation.WarehouseId,
                            _warehouseService.GetObjectById(stockmutation.WarehouseId).Name,
                            stockmutation.WarehouseItemId,
                            stockmutation.ItemCase == Core.Constants.Constant.ItemCase.Ready ?
                                (stockmutation.Status == Core.Constants.Constant.MutationStatus.Addition ? stockmutation.Quantity : stockmutation.Quantity * (-1)) : 0,
                            stockmutation.ItemCase == Core.Constants.Constant.ItemCase.PendingReceival ?
                                (stockmutation.Status == Core.Constants.Constant.MutationStatus.Addition ? stockmutation.Quantity : stockmutation.Quantity * (-1)) : 0,
                            stockmutation.ItemCase == Core.Constants.Constant.ItemCase.PendingDelivery ?
                                (stockmutation.Status == Core.Constants.Constant.MutationStatus.Addition ? stockmutation.Quantity : stockmutation.Quantity * (-1)) : 0,
                            _uomService.GetObjectById(_itemService.GetObjectById(stockmutation.ItemId).UoMId).Name,
                            stockmutation.SourceDocumentType,
                            stockmutation.SourceDocumentId,
                            stockmutation.SourceDocumentDetailType,
                            stockmutation.SourceDocumentDetailId,
                            stockmutation.CreatedAt,
                            // TODO
                            // stockmutation.MutationDate
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}