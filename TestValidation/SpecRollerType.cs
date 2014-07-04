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

    public class SpecRollerType: nspec
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

        ItemType typeAccessory;
        ItemType typeBearing;
        ItemType typeBlanket;
        ItemType typeCore;
        ItemType typeCompound;
        ItemType typeChemical;
        ItemType typeConsumable;
        ItemType typeGlue;
        ItemType typeUnderpacking;
        ItemType typeRoller;
        Item item;
        RollerType typeDamp;
        RollerType typeFoundDT;
        RollerType typeInkFormX;
        RollerType typeInkDistD;
        RollerType typeInkDistM;
        RollerType typeInkDistE;
        RollerType typeInkDuctB;
        RollerType typeInkDistH;
        RollerType typeInkFormW;
        RollerType typeInkDistHQ;
        RollerType typeDampFormDQ;
        RollerType typeInkFormY;
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

                typeAccessory = _itemTypeService.CreateObject("Accessory", "Accessory");
                typeBearing = _itemTypeService.CreateObject("Bearing", "Bearing");
                typeBlanket = _itemTypeService.CreateObject("Blanket", "Blanket");
                typeChemical = _itemTypeService.CreateObject("Chemical", "Chemical");
                typeCompound = _itemTypeService.CreateObject("Compound", "Compound");
                typeConsumable = _itemTypeService.CreateObject("Consumable", "Consumable");
                typeCore = _itemTypeService.CreateObject("Core", "Core");
                typeGlue = _itemTypeService.CreateObject("Glue", "Glue");
                typeUnderpacking = _itemTypeService.CreateObject("Underpacking", "Underpacking");
                typeRoller = _itemTypeService.CreateObject("Roller", "Roller");

                typeDamp = _rollerTypeService.CreateObject("Damp", "Damp");
                typeFoundDT = _rollerTypeService.CreateObject("Found DT", "Found DT");
                typeInkFormX = _rollerTypeService.CreateObject("Ink Form X", "Ink Form X");
                typeInkDistD = _rollerTypeService.CreateObject("Ink Dist D", "Ink Dist D");
                typeInkDistM = _rollerTypeService.CreateObject("Ink Dist M", "Ink Dist M");
                typeInkDistE = _rollerTypeService.CreateObject("Ink Dist E", "Ink Dist E");
                typeInkDuctB = _rollerTypeService.CreateObject("Ink Duct B", "Ink Duct B");
                typeInkDistH = _rollerTypeService.CreateObject("Ink Dist H", "Ink Dist H");
                typeInkFormW = _rollerTypeService.CreateObject("Ink Form W", "Ink Form W");
                typeInkDistHQ = _rollerTypeService.CreateObject("Ink Dist HQ", "Ink Dist HQ");
                typeDampFormDQ = _rollerTypeService.CreateObject("Damp Form DQ", "Damp Form DQ");
                typeInkFormY = _rollerTypeService.CreateObject("Ink Form Y", "Ink Form Y");

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
            }
        }

        void rollertype_validation()
        {
        
            it["validates_rollertypes"] = () =>
            {
                typeDamp.Errors.Count().should_be(0);
                typeFoundDT.Errors.Count().should_be(0);
                typeInkFormX.Errors.Count().should_be(0);
                typeInkDistD.Errors.Count().should_be(0);
                typeInkDistM.Errors.Count().should_be(0);
                typeInkDistE.Errors.Count().should_be(0);
                typeInkDuctB.Errors.Count().should_be(0);
                typeInkDistH.Errors.Count().should_be(0);
                typeInkFormW.Errors.Count().should_be(0);
                typeInkDistHQ.Errors.Count().should_be(0);
                typeDampFormDQ.Errors.Count().should_be(0);
                typeInkFormY.Errors.Count().should_be(0);
            };

            it["rollertype_with_existingname"] = () =>
            {
                RollerType typecopyDamp = _rollerTypeService.CreateObject("Damp", "Damp");
                typecopyDamp.Errors.Count().should_not_be(0);
            };

            it["delete_rollertype"] = () =>
            {
                typeDamp = _rollerTypeService.SoftDeleteObject(typeDamp, _rollerBuilderService, _coreIdentificationDetailService);
                typeDamp.Errors.Count().should_be(0);
            };

            context["when_deleting_rollertype"] = () =>
            {
                before = () =>
                {
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
                };

                it["delete_rollertype_with_coreidentificationdetail"] = () =>
                {
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
                    typeFoundDT = _rollerTypeService.SoftDeleteObject(typeFoundDT, _rollerBuilderService, _coreIdentificationDetailService);
                    typeFoundDT.Errors.Count().should_not_be(0);
                };

                it["delete_rollertype_with_rollerbuilder"] = () =>
                {
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
                    typeDamp = _rollerTypeService.SoftDeleteObject(typeDamp, _rollerBuilderService, _coreIdentificationDetailService);
                    typeDamp.Errors.Count().should_not_be(0);
                };
            };
        }
    }
}