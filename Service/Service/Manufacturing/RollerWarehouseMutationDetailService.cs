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
    public class RollerWarehouseMutationDetailService : IRollerWarehouseMutationDetailService
    {

        private IRollerWarehouseMutationDetailRepository _repository;
        private IRollerWarehouseMutationDetailValidator _validator;

        public RollerWarehouseMutationDetailService(IRollerWarehouseMutationDetailRepository _rollerWarehouseMutationDetailRepository, IRollerWarehouseMutationDetailValidator _rollerWarehouseMutationDetailValidator)
        {
            _repository = _rollerWarehouseMutationDetailRepository;
            _validator = _rollerWarehouseMutationDetailValidator;
        }

        public IRollerWarehouseMutationDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<RollerWarehouseMutationDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<RollerWarehouseMutationDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RollerWarehouseMutationDetail> GetObjectsByRollerWarehouseMutationId(int rollerWarehouseMutationId)
        {
            return _repository.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutationId);
        }

        public RollerWarehouseMutationDetail GetObjectByRecoveryOrderDetailId(int recoveryOrderDetailId)
        {
            return _repository.GetObjectByRecoveryOrderDetailId(recoveryOrderDetailId);
        }

        public RollerWarehouseMutationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RollerWarehouseMutationDetail CreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                          IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            rollerWarehouseMutationDetail.Errors = new Dictionary<String, String>();
            return (rollerWarehouseMutationDetail = _validator.ValidCreateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService,
                                                    _recoveryOrderDetailService, _coreIdentificationDetailService, this, _itemService, _warehouseItemService) ?
                                                    _repository.CreateObject(rollerWarehouseMutationDetail) : rollerWarehouseMutationDetail);
        }

        public RollerWarehouseMutationDetail CreateObject(int rollerWarehouseMutationId, int itemId, int quantity, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                          IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            RollerWarehouseMutationDetail rollerWarehouseMutationDetail = new RollerWarehouseMutationDetail
            {
                RollerWarehouseMutationId = rollerWarehouseMutationId,
                ItemId = itemId,
                // Price = price
            };
            return this.CreateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _recoveryOrderDetailService,
                                     _coreIdentificationDetailService, _itemService, _warehouseItemService);
        }

        public RollerWarehouseMutationDetail UpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                          IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            return (rollerWarehouseMutationDetail = _validator.ValidUpdateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService,
                                                    _recoveryOrderDetailService, _coreIdentificationDetailService, this, _itemService, _warehouseItemService) ?
                                                   _repository.UpdateObject(rollerWarehouseMutationDetail) : rollerWarehouseMutationDetail);
        }

        public RollerWarehouseMutationDetail SoftDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, IWarehouseItemService _warehouseItemService)
        {
            return (rollerWarehouseMutationDetail = _validator.ValidDeleteObject(rollerWarehouseMutationDetail) ? _repository.SoftDeleteObject(rollerWarehouseMutationDetail) : rollerWarehouseMutationDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public RollerWarehouseMutationDetail ConfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, DateTime ConfirmationDate, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                         IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                         IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                         ICoreIdentificationService _coreIdentificationService, ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService)
        {
            rollerWarehouseMutationDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(rollerWarehouseMutationDetail, _recoveryOrderService, _coreIdentificationService, _rollerWarehouseMutationService,
                                              _itemService, _blanketService, _warehouseItemService, _customerItemService))
            {
                _repository.ConfirmObject(rollerWarehouseMutationDetail);
                
                // Set IsDelivered = true
                RecoveryOrderDetail recoveryOrderDetail = _recoveryOrderDetailService.GetObjectById(rollerWarehouseMutationDetail.RecoveryOrderDetailId);
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
                _coreIdentificationDetailService.DeliverObject(coreIdentificationDetail, _coreIdentificationService, this);

                // reduce warehouseFromItem
                // add warehouseToItem

                RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseFromId, rollerWarehouseMutationDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseToId, rollerWarehouseMutationDetail.ItemId);
                if (coreIdentification.IsInHouse)
                {
                    IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForRollerWarehouseMutation(rollerWarehouseMutationDetail, warehouseItemFrom, warehouseItemTo);
                    foreach (var stockMutation in stockMutations)
                    {
                        _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    }
                }
                else
                {
                    CustomerItem customerItemFrom = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), warehouseItemFrom.Id);
                    CustomerItem customerItemTo = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), warehouseItemTo.Id);
                    IList<CustomerStockMutation> customerStockMutations = _customerStockMutationService.CreateCustomerStockMutationForRollerWarehouseMutation(rollerWarehouseMutationDetail, customerItemFrom, customerItemTo, rollerWarehouseMutationDetail.ItemId);
                    foreach (var customerStockMutation in customerStockMutations)
                    {
                        _customerStockMutationService.StockMutateObject(customerStockMutation, coreIdentification.IsInHouse, _itemService, _customerItemService, _warehouseItemService);
                    }
                }
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail UnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                            IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                            IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                            ICoreIdentificationService _coreIdentificationService, ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService)
        {
            if (_validator.ValidUnconfirmObject(rollerWarehouseMutationDetail, _recoveryOrderService, _coreIdentificationService, _rollerWarehouseMutationService,
                                                _itemService, _blanketService, _warehouseItemService, _customerItemService))
            {
                _repository.UnconfirmObject(rollerWarehouseMutationDetail);

                // Set IsDelivered = false
                RecoveryOrderDetail recoveryOrderDetail = _recoveryOrderDetailService.GetObjectById(rollerWarehouseMutationDetail.RecoveryOrderDetailId);
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.UndoDeliverObject(coreIdentificationDetail, _coreIdentificationService, this);

                // reverse stock mutate warehouseFromItem and warehouseToItem
                RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseFromId, rollerWarehouseMutationDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseToId, rollerWarehouseMutationDetail.ItemId);

                if (rollerWarehouseMutation.RecoveryOrder.CoreIdentification.IsInHouse)
                {
                    IList<StockMutation> stockMutations = new List<StockMutation>();
                    IList<StockMutation> stockMutationFrom = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemFrom.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
                    stockMutationFrom.ToList().ForEach(x => stockMutations.Add(x));
                    IList<StockMutation> stockMutationTo = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemTo.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
                    stockMutationTo.ToList().ForEach(x => stockMutations.Add(x));
                    foreach (var stockMutation in stockMutations)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    }
                    _stockMutationService.DeleteStockMutations(stockMutations);
                }
                else
                {
                    CustomerItem customerItemFrom = _customerItemService.FindOrCreateObject(rollerWarehouseMutation.RecoveryOrder.CoreIdentification.ContactId.GetValueOrDefault(), warehouseItemFrom.Id);
                    CustomerItem customerItemTo = _customerItemService.FindOrCreateObject(rollerWarehouseMutation.RecoveryOrder.CoreIdentification.ContactId.GetValueOrDefault(), warehouseItemTo.Id);
                    IList<CustomerStockMutation> customerStockMutations = new List<CustomerStockMutation>();
                    IList<CustomerStockMutation> customerStockMutationFrom = _customerStockMutationService.GetObjectsBySourceDocumentDetailForCustomerItem(customerItemFrom.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
                    customerStockMutationFrom.ToList().ForEach(x => customerStockMutations.Add(x));
                    IList<CustomerStockMutation> customerStockMutationTo = _customerStockMutationService.GetObjectsBySourceDocumentDetailForCustomerItem(customerItemTo.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
                    customerStockMutationTo.ToList().ForEach(x => customerStockMutations.Add(x));
                    foreach (var customerStockMutation in customerStockMutations)
                    {
                        _customerStockMutationService.ReverseStockMutateObject(customerStockMutation, rollerWarehouseMutation.RecoveryOrder.CoreIdentification.IsInHouse, _itemService, _customerItemService, _warehouseItemService);
                    }
                    _customerStockMutationService.DeleteCustomerStockMutations(customerStockMutations);
                }
            }
            return rollerWarehouseMutationDetail;
        }
    }
}