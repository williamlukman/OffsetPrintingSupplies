using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using NSpec;
using Service.Service;
using Core.Interface.Service;
using Data.Context;
using System.Data.Entity;
using Data.Repository;
using Validation.Validation;

namespace TestValidation
{

    public class SpecRollerBuilder: nspec
    {
        ICoreBuilderService _coreBuilderService;
        ICoreIdentificationService _coreIdentificationService;
        ICoreIdentificationDetailService _coreIdentificationDetailService;
        ICustomerService _customerService;
        IItemService _itemService;
        IItemTypeService _itemTypeService;
        IMachineService _machineService;
        IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        IRecoveryOrderDetailService _recoveryOrderDetailService;
        IRecoveryOrderService _recoveryOrderService;
        IRollerBuilderService _rollerBuilderService;
        IRollerTypeService _rollerTypeService;

        Item item;
        Item itemCompound;
        Customer customer;
        Machine machine;
        CoreBuilder coreBuilder;
        CoreIdentification coreIdentification;
        CoreIdentificationDetail coreIdentificationDetail;
        RollerBuilder rollerBuilder;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
                _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
                _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
                _customerService = new CustomerService(new CustomerRepository(), new CustomerValidator());
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
                _machineService = new MachineService(new MachineRepository(), new MachineValidator());
                _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
                _recoveryOrderService = new RecoveryOrderService(new RecoveryOrderRepository(), new RecoveryOrderValidator());
                _recoveryAccessoryDetailService = new RecoveryAccessoryDetailService(new RecoveryAccessoryDetailRepository(), new RecoveryAccessoryDetailValidator());
                _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
                _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());

                _itemTypeService.CreateObject("Accessory", "Accessory");
                _itemTypeService.CreateObject("Bearing", "Bearing");
                _itemTypeService.CreateObject("Blanket", "Blanket");
                _itemTypeService.CreateObject("Chemical", "Chemical");
                _itemTypeService.CreateObject("Compound", "Compound");
                _itemTypeService.CreateObject("Consumable", "Consumable");
                _itemTypeService.CreateObject("Core", "Core");
                _itemTypeService.CreateObject("Glue", "Glue");
                _itemTypeService.CreateObject("Underpacking", "Underpacking");
                _itemTypeService.CreateObject("Roller", "Roller");

                _rollerTypeService.CreateObject("Damp", "Damp");
                _rollerTypeService.CreateObject("Found DT", "Found DT");
                _rollerTypeService.CreateObject("Ink Form X", "Ink Form X");
                _rollerTypeService.CreateObject("Ink Dist D", "Ink Dist D");
                _rollerTypeService.CreateObject("Ink Dist M", "Ink Dist M");
                _rollerTypeService.CreateObject("Ink Dist E", "Ink Dist E");
                _rollerTypeService.CreateObject("Ink Duct B", "Ink Duct B");
                _rollerTypeService.CreateObject("Ink Dist H", "Ink Dist H");
                _rollerTypeService.CreateObject("Ink Form W", "Ink Form W");
                _rollerTypeService.CreateObject("Ink Dist HQ", "Ink Dist HQ");
                _rollerTypeService.CreateObject("Damp Form DQ", "Damp Form DQ");
                _rollerTypeService.CreateObject("Ink Form Y", "Ink Form Y");

                item = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Category = "ABC123",
                    UoM = "Pcs",
                    Quantity = 0
                };
                item = _itemService.CreateObject(item, _itemTypeService);
                itemCompound = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                    Sku = "Cmp10001",
                    Name = "Cmp 10001",
                    Category = "cmp",
                    UoM = "Pcs",
                    Quantity = 2
                };
                itemCompound = _itemService.CreateObject(itemCompound, _itemTypeService);

                customer = _customerService.CreateObject("Abbey", "1 Abbey St", "001234567", "Daddy", "001234888", "abbey@abbeyst.com");

                machine = new Machine()
                {
                    Code = "M00001",
                    Name = "Machine 00001",
                    Description = "Machine"
                };
                machine = _machineService.CreateObject(machine);
                coreBuilder = new CoreBuilder()
                {
                    BaseSku = "CB00001",
                    SkuNewCore = "CB00001N",
                    SkuUsedCore = "CB00001U",
                    Name = "CoreBuilder00001",
                    Category = "X"
                };
                coreBuilder = _coreBuilderService.CreateObject(coreBuilder, _itemService, _itemTypeService);
                coreIdentification = new CoreIdentification()
                {
                    CustomerId = customer.Id,
                    Code = "CI0001",
                    Quantity = 1,
                    IdentifiedDate = DateTime.Now
                };
                coreIdentification = _coreIdentificationService.CreateObject(coreIdentification, _customerService);
                coreIdentificationDetail = new CoreIdentificationDetail()
                {
                    CoreIdentificationId = coreIdentification.Id,
                    DetailId = 1,
                    MaterialCase = 2,
                    CoreBuilderId = coreBuilder.Id,
                    RollerTypeId = _rollerTypeService.GetObjectByName("Found DT").Id,
                    MachineId = machine.Id,
                    RD = 12,
                    CD = 12,
                    RL = 12,
                    WL = 12,
                    TL = 12
                };
                coreIdentificationDetail = _coreIdentificationDetailService.CreateObject(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

                rollerBuilder = new RollerBuilder()
                {
                    BaseSku = "RB0001",
                    SkuNewRoller = "RB0001N",
                    SkuUsedRoller = "RB0001U",
                    Name = "Roller 0001",
                    Category = "0001",
                    RD = 12,
                    CD = 13,
                    RL = 14,
                    TL = 15,
                    WL = 16,
                    CompoundId = itemCompound.Id,
                    CoreBuilderId = coreBuilder.Id,
                    MachineId = machine.Id,
                    RollerTypeId = _rollerTypeService.GetObjectByName("Damp").Id
                };
                rollerBuilder = _rollerBuilderService.CreateObject(rollerBuilder, _machineService, _itemService, _itemTypeService, _coreBuilderService, _rollerTypeService);
            }
        }

        /*
         * STEPS:
         * 1. Create valid item
         * 2. Create invalid item with no name
         * 3. Create invalid items with same SKU
         * 4a. Delete item
         * 4b. Delete item with stock mutations
         */
        void rollerbuilder_validation()
        {
        
            it["validates_rollerbuilder"] = () =>
            {
                item.Errors.Count().should_be(0);
                itemCompound.Errors.Count().should_be(0);
                machine.Errors.Count().should_be(0);
                coreBuilder.Errors.Count().should_be(0);
                coreIdentification.Errors.Count().should_be(0);
                coreIdentificationDetail.Errors.Count().should_be(0);
                rollerBuilder.Errors.Count().should_be(0);
            };

            it["delete_rollerbuilder"] = () =>
            {
                rollerBuilder = _rollerBuilderService.SoftDeleteObject(rollerBuilder, _itemService, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _coreBuilderService);
                rollerBuilder.Errors.Count().should_be(0);
            };

            it["delete_rollerbuilder_with_recoveryorderdetail"] = () =>
            {
                _itemService.AdjustQuantity(_coreBuilderService.GetNewCore(coreBuilder.Id), 1);
                _itemService.AdjustQuantity(_coreBuilderService.GetUsedCore(coreBuilder.Id), 1);

                coreIdentification = _coreIdentificationService.ConfirmObject(coreIdentification, _coreIdentificationDetailService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService);
                coreIdentification.Errors.Count().should_be(0);

                RecoveryOrder recoveryOrder = new RecoveryOrder()
                {
                    Code = "RO0001",
                    CoreIdentificationId = coreIdentification.Id,
                    QuantityReceived = 1,
                };
                recoveryOrder = _recoveryOrderService.CreateObject(recoveryOrder, _coreIdentificationService);
                recoveryOrder.Errors.Count().should_be(0);

                RecoveryOrderDetail recoveryOrderDetail = new RecoveryOrderDetail()
                {
                    RecoveryOrderId = recoveryOrder.Id,
                    CoreIdentificationDetailId = coreIdentificationDetail.Id,
                    RollerBuilderId = rollerBuilder.Id,
                    CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                    RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                    Acc = "Y",
                };
                recoveryOrderDetail = _recoveryOrderDetailService.CreateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);
                recoveryOrderDetail.Errors.Count().should_be(0);

                rollerBuilder = _rollerBuilderService.SoftDeleteObject(rollerBuilder, _itemService, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _coreBuilderService);
                rollerBuilder.Errors.Count().should_not_be(0);
            };
        }
    }
}