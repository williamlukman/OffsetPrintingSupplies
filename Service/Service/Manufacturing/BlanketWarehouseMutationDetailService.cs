using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class BlanketWarehouseMutationDetailService : IBlanketWarehouseMutationDetailService
    {

        private IBlanketWarehouseMutationDetailRepository _repository;
        private IBlanketWarehouseMutationDetailValidator _validator;

        public BlanketWarehouseMutationDetailService(IBlanketWarehouseMutationDetailRepository _blanketWarehouseMutationDetailRepository, IBlanketWarehouseMutationDetailValidator _blanketWarehouseMutationDetailValidator)
        {
            _repository = _blanketWarehouseMutationDetailRepository;
            _validator = _blanketWarehouseMutationDetailValidator;
        }

        public IBlanketWarehouseMutationDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<BlanketWarehouseMutationDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<BlanketWarehouseMutationDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BlanketWarehouseMutationDetail> GetObjectsByBlanketWarehouseMutationId(int blanketWarehouseMutationId)
        {
            return _repository.GetObjectsByBlanketWarehouseMutationId(blanketWarehouseMutationId);
        }

        public BlanketWarehouseMutationDetail GetObjectByBlanketOrderDetailId(int blanketOrderDetailId)
        {
            return _repository.GetObjectByBlanketOrderDetailId(blanketOrderDetailId);
        }

        public BlanketWarehouseMutationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BlanketWarehouseMutationDetail CreateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                          IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            blanketWarehouseMutationDetail.Errors = new Dictionary<String, String>();
            return (blanketWarehouseMutationDetail = _validator.ValidCreateObject(blanketWarehouseMutationDetail, _blanketWarehouseMutationService,
                                                    _blanketOrderDetailService, _coreIdentificationDetailService, this, _itemService, _warehouseItemService) ?
                                                    _repository.CreateObject(blanketWarehouseMutationDetail) : blanketWarehouseMutationDetail);
        }

        public BlanketWarehouseMutationDetail CreateObject(int blanketWarehouseMutationId, int itemId, int quantity, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                          IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            BlanketWarehouseMutationDetail blanketWarehouseMutationDetail = new BlanketWarehouseMutationDetail
            {
                BlanketWarehouseMutationId = blanketWarehouseMutationId,
                ItemId = itemId,
                // Price = price
            };
            return this.CreateObject(blanketWarehouseMutationDetail, _blanketWarehouseMutationService, _blanketOrderDetailService,
                                     _coreIdentificationDetailService, _itemService, _warehouseItemService);
        }

        public BlanketWarehouseMutationDetail UpdateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                          IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            return (blanketWarehouseMutationDetail = _validator.ValidUpdateObject(blanketWarehouseMutationDetail, _blanketWarehouseMutationService,
                                                    _blanketOrderDetailService, _coreIdentificationDetailService, this, _itemService, _warehouseItemService) ?
                                                   _repository.UpdateObject(blanketWarehouseMutationDetail) : blanketWarehouseMutationDetail);
        }

        public BlanketWarehouseMutationDetail SoftDeleteObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService, IWarehouseItemService _warehouseItemService)
        {
            return (blanketWarehouseMutationDetail = _validator.ValidDeleteObject(blanketWarehouseMutationDetail) ? _repository.SoftDeleteObject(blanketWarehouseMutationDetail) : blanketWarehouseMutationDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public BlanketWarehouseMutationDetail ConfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, DateTime ConfirmationDate, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                         IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                         IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                         ICoreIdentificationService _coreIdentificationService, ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService)
        {
            blanketWarehouseMutationDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(blanketWarehouseMutationDetail, _blanketOrderService, _coreIdentificationService, _blanketWarehouseMutationService,
                                              _itemService, _blanketService, _warehouseItemService, _customerItemService))
            {
                _repository.ConfirmObject(blanketWarehouseMutationDetail);
                
                // Set IsDelivered = true
                BlanketOrderDetail blanketOrderDetail = _blanketOrderDetailService.GetObjectById(blanketWarehouseMutationDetail.BlanketOrderDetailId);
                //CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(blanketOrderDetail.CoreIdentificationDetailId);
                //CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
                //_coreIdentificationDetailService.DeliverObject(coreIdentificationDetail, _coreIdentificationService, this);

                // reduce warehouseFromItem
                // add warehouseToItem

                BlanketWarehouseMutation blanketWarehouseMutation = _blanketWarehouseMutationService.GetObjectById(blanketWarehouseMutationDetail.BlanketWarehouseMutationId);
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(blanketWarehouseMutation.WarehouseFromId, blanketWarehouseMutationDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(blanketWarehouseMutation.WarehouseToId, blanketWarehouseMutationDetail.ItemId);
                //if (coreIdentification.IsInHouse)
                {
                    IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForBlanketWarehouseMutation(blanketWarehouseMutationDetail, warehouseItemFrom, warehouseItemTo);
                    foreach (var stockMutation in stockMutations)
                    {
                        _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    }
                }
                //else
                //{
                //    CustomerItem customerItemFrom = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), warehouseItemFrom.Id);
                //    CustomerItem customerItemTo = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), warehouseItemTo.Id);
                //    IList<CustomerStockMutation> customerStockMutations = _customerStockMutationService.CreateCustomerStockMutationForBlanketWarehouseMutation(blanketWarehouseMutationDetail, customerItemFrom, customerItemTo, blanketWarehouseMutationDetail.ItemId);
                //    foreach (var customerStockMutation in customerStockMutations)
                //    {
                //        _customerStockMutationService.StockMutateObject(customerStockMutation, coreIdentification.IsInHouse, _itemService, _customerItemService, _warehouseItemService);
                //    }
                //}
            }
            return blanketWarehouseMutationDetail;
        }

        public BlanketWarehouseMutationDetail UnconfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                            IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                            IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                            ICoreIdentificationService _coreIdentificationService, ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService)
        {
            if (_validator.ValidUnconfirmObject(blanketWarehouseMutationDetail, _blanketOrderService, _coreIdentificationService, _blanketWarehouseMutationService,
                                                _itemService, _blanketService, _warehouseItemService, _customerItemService))
            {
                _repository.UnconfirmObject(blanketWarehouseMutationDetail);

                // Set IsDelivered = false
                BlanketOrderDetail blanketOrderDetail = _blanketOrderDetailService.GetObjectById(blanketWarehouseMutationDetail.BlanketOrderDetailId);
                //CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(blanketOrderDetail.CoreIdentificationDetailId);
                //_coreIdentificationDetailService.UndoDeliverObject(coreIdentificationDetail, _coreIdentificationService, this);

                // reverse stock mutate warehouseFromItem and warehouseToItem
                BlanketWarehouseMutation blanketWarehouseMutation = _blanketWarehouseMutationService.GetObjectById(blanketWarehouseMutationDetail.BlanketWarehouseMutationId);
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(blanketWarehouseMutation.WarehouseFromId, blanketWarehouseMutationDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(blanketWarehouseMutation.WarehouseToId, blanketWarehouseMutationDetail.ItemId);

                //if (blanketWarehouseMutation.BlanketOrder.CoreIdentification.IsInHouse)
                {
                    IList<StockMutation> stockMutations = new List<StockMutation>();
                    IList<StockMutation> stockMutationFrom = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemFrom.Id, Constant.SourceDocumentDetailType.BlanketWarehouseMutationDetail, blanketWarehouseMutationDetail.Id);
                    stockMutationFrom.ToList().ForEach(x => stockMutations.Add(x));
                    IList<StockMutation> stockMutationTo = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemTo.Id, Constant.SourceDocumentDetailType.BlanketWarehouseMutationDetail, blanketWarehouseMutationDetail.Id);
                    stockMutationTo.ToList().ForEach(x => stockMutations.Add(x));
                    foreach (var stockMutation in stockMutations)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    }
                }
                //else
                //{
                //    CustomerItem customerItemFrom = _customerItemService.FindOrCreateObject(blanketWarehouseMutation.BlanketOrder.CoreIdentification.ContactId.GetValueOrDefault(), warehouseItemFrom.Id);
                //    CustomerItem customerItemTo = _customerItemService.FindOrCreateObject(blanketWarehouseMutation.BlanketOrder.CoreIdentification.ContactId.GetValueOrDefault(), warehouseItemTo.Id);
                //    IList<CustomerStockMutation> customerStockMutations = new List<CustomerStockMutation>();
                //    IList<CustomerStockMutation> customerStockMutationFrom = _customerStockMutationService.GetObjectsBySourceDocumentDetailForCustomerItem(customerItemFrom.Id, Constant.SourceDocumentDetailType.BlanketWarehouseMutationDetail, blanketWarehouseMutationDetail.Id);
                //    customerStockMutationFrom.ToList().ForEach(x => customerStockMutations.Add(x));
                //    IList<CustomerStockMutation> customerStockMutationTo = _customerStockMutationService.GetObjectsBySourceDocumentDetailForCustomerItem(customerItemTo.Id, Constant.SourceDocumentDetailType.BlanketWarehouseMutationDetail, blanketWarehouseMutationDetail.Id);
                //    customerStockMutationTo.ToList().ForEach(x => customerStockMutations.Add(x));
                //    foreach (var customerStockMutation in customerStockMutations)
                //    {
                //        _customerStockMutationService.ReverseStockMutateObject(customerStockMutation, blanketWarehouseMutation.BlanketOrder.CoreIdentification.IsInHouse, _itemService, _customerItemService, _warehouseItemService);
                //    }
                //}
            }
            return blanketWarehouseMutationDetail;
        }
    }
}