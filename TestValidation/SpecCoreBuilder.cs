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

    public class SpecCoreBuilder: nspec
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

        Customer customer;
        CoreBuilder coreBuilder;
        Item item;

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
                customer = _customerService.CreateObject("Abbey", "1 Abbey St", "001234567", "Daddy", "001234888", "abbey@abbeyst.com");

                coreBuilder = new CoreBuilder()
                {
                    BaseSku = "CORE1001",
                    SkuNewCore = "CORE1001N",
                    SkuUsedCore = "CORE1001U",
                    Name = "Core X 1001",
                    Category = "X 1001"
                };
                coreBuilder = _coreBuilderService.CreateObject(coreBuilder, _itemService, _itemTypeService);
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
        void corebuilder_validation()
        {
        
            it["validates_corebuilder"] = () =>
            {
                coreBuilder.Errors.Count().should_be(0);
            };

            it["corebuilder_with_samesku"] = () =>
            {
                CoreBuilder coreBuilderCopySku = new CoreBuilder()
                {
                    BaseSku = "CORE1001",
                    SkuNewCore = "123N",
                    SkuUsedCore = "123U",
                    Name = "Core X 1001",
                    Category = "X 1001"
                };
                coreBuilderCopySku = _coreBuilderService.CreateObject(coreBuilderCopySku, _itemService, _itemTypeService);
                coreBuilderCopySku.Errors.Count().should_not_be(0);
            };

            it["corebuilder_with_sameskuitemnew"] = () =>
            {
                CoreBuilder coreBuilderCopySkuNew = new CoreBuilder()
                {
                    BaseSku = "CORE1002",
                    SkuNewCore = "CORE1001N",
                    SkuUsedCore = "123U",
                    Name = "Core X 1001",
                    Category = "X 1001"
                };
                coreBuilderCopySkuNew = _coreBuilderService.CreateObject(coreBuilderCopySkuNew, _itemService, _itemTypeService);
                coreBuilderCopySkuNew.Errors.Count().should_not_be(0);
            };


            it["corebuilder_with_sameskuitemused"] = () =>
            {
                CoreBuilder coreBuilderCopySkuUsed = new CoreBuilder()
                {
                    BaseSku = "CORE1002",
                    SkuNewCore = "123N",
                    SkuUsedCore = "CORE1001U",
                    Name = "Core X 1001",
                    Category = "X 1001"
                };
                coreBuilderCopySkuUsed = _coreBuilderService.CreateObject(coreBuilderCopySkuUsed, _itemService, _itemTypeService);
                coreBuilderCopySkuUsed.Errors.Count().should_not_be(0);
            };

            it["delete_corebuilder"] = () =>
            {
                coreBuilder = _coreBuilderService.SoftDeleteObject(coreBuilder, _itemService, _rollerBuilderService, _coreIdentificationDetailService, _recoveryOrderDetailService, _recoveryAccessoryDetailService);
                coreBuilder.Errors.Count().should_be(0);
            };

            it["delete_corebuilder_with_coreidentificationdetail"] = () =>
            {
                Machine machine = new Machine()
                {
                    Code = "M00001",
                    Name = "Machine 00001",
                    Description = "Machine"
                };
                machine = _machineService.CreateObject(machine);
                CoreIdentification coreIdentification = new CoreIdentification()
                {
                    CustomerId = customer.Id,
                    Code = "CI0001",
                    Quantity = 1,
                    IdentifiedDate = DateTime.Now
                };
                coreIdentification = _coreIdentificationService.CreateObject(coreIdentification, _customerService);
                CoreIdentificationDetail coreIdentificationDetail = new CoreIdentificationDetail()
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

                coreBuilder = _coreBuilderService.SoftDeleteObject(coreBuilder, _itemService, _rollerBuilderService, _coreIdentificationDetailService, _recoveryOrderDetailService, _recoveryAccessoryDetailService);
                coreBuilder.Errors.Count().should_not_be(0);
            };
        }
    }
}