using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IRetailPurchaseInvoiceDetailService
    {
        IQueryable<RetailPurchaseInvoiceDetail> GetQueryable();
        IRetailPurchaseInvoiceDetailValidator GetValidator();
        IList<RetailPurchaseInvoiceDetail> GetAll();
        IList<RetailPurchaseInvoiceDetail> GetObjectsByRetailPurchaseInvoiceId(int retailPurchaseInvoiceId);
        RetailPurchaseInvoiceDetail GetObjectById(int Id);
        RetailPurchaseInvoiceDetail CreateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService,
                                              IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService);
        RetailPurchaseInvoiceDetail UpdateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService,
                                              IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService);
        RetailPurchaseInvoiceDetail SoftDeleteObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService);
        RetailPurchaseInvoiceDetail ConfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService,
                                               IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        RetailPurchaseInvoiceDetail UnconfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IWarehouseItemService _warehouseItemService,
                                                 IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        bool DeleteObject(int Id);
        decimal CalculateTotal(int RetailPurchaseInvoiceId);
    }
}
