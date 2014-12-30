using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IDeliveryOrderService
    {
        IDeliveryOrderValidator GetValidator();
        IQueryable<DeliveryOrder> GetQueryable();
        IList<DeliveryOrder> GetAll();
        DeliveryOrder GetObjectById(int Id);
        IList<DeliveryOrder> GetObjectsBySalesOrderId(int salesOrderId);
        IList<DeliveryOrder> GetConfirmedObjects();
        DeliveryOrder CreateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IWarehouseService _warehouseService);
        DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IWarehouseService _warehouseService);
        DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool DeleteObject(int Id);
        DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder, DateTime ConfirmationDate, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                    ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService,
                                    IItemService _itemService, IItemTypeService _itemTypeService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                    IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                    IServiceCostService _serviceCostService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                    ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService,
                                    ICurrencyService _currencyService, IExchangeRateService _exchangeRateService);
        DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesInvoiceService _salesInvoiceService,
                                      ISalesInvoiceDetailService _salesInvoiceDetailService, ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                      IStockMutationService _stockMutationService, IItemService _itemService, IItemTypeService _itemTypeService, IBlanketService _blanketService,
                                      IWarehouseItemService _warehouseItemService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
                                      IClosingService _closingService, ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService);
       DeliveryOrder CheckAndSetInvoiceComplete(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
       DeliveryOrder UnsetInvoiceComplete(DeliveryOrder deliveryOrder);
    }
}