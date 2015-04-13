using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IBlanketWarehouseMutationDetailValidator
    {
        BlanketWarehouseMutationDetail VHasBlanketOrderDetail(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderDetailService _blanketOrderDetailService);
        //BlanketWarehouseMutationDetail VCoreIdentificationDetailHasNotBeenDelivered(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderDetailService _blanketOrderDetailService,
        //                                                                           ICoreIdentificationDetailService _coreIdentificationDetailService);
        BlanketWarehouseMutationDetail VBlanketOrderDetailHasBeenFinished(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketWarehouseMutationDetail VHasBlanketWarehouseMutation(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService);
        BlanketWarehouseMutationDetail VHasWarehouseItemFrom(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService, IWarehouseItemService _warehouseItemService);
        BlanketWarehouseMutationDetail VHasWarehouseItemTo(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService, IWarehouseItemService _warehouseItemService);
        BlanketWarehouseMutationDetail VUniqueBlanketOrderDetail(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService);
        BlanketWarehouseMutationDetail VNonNegativeStockQuantity(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                                                IBlanketWarehouseMutationService _blanketWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                                IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService, bool CaseConfirm);
        BlanketWarehouseMutationDetail VBlanketWarehouseMutationHasBeenConfirmed(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService);
        BlanketWarehouseMutationDetail VBlanketWarehouseMutationHasNotBeenConfirmed(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService);
        BlanketWarehouseMutationDetail VHasNotBeenConfirmed(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
        BlanketWarehouseMutationDetail VHasBeenConfirmed(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
        BlanketWarehouseMutationDetail VCreateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                    IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                    IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BlanketWarehouseMutationDetail VUpdateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                    IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                    IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BlanketWarehouseMutationDetail VDeleteObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
        BlanketWarehouseMutationDetail VHasConfirmationDate(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
        BlanketWarehouseMutationDetail VConfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService, 
                                                     IBlanketWarehouseMutationService _blanketWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                     IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        BlanketWarehouseMutationDetail VUnconfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                                       IBlanketWarehouseMutationService _blanketWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                       IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        bool ValidCreateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                               IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                               IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                               IBlanketOrderDetailService _blanketOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                               IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidConfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                IBlanketWarehouseMutationService _blanketWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        bool ValidUnconfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                  IBlanketWarehouseMutationService _blanketWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                  IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        bool ValidDeleteObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
        bool isValid(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
        string PrintError(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
    }
}