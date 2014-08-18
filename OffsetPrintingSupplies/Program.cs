using Core.DomainModel;
using Core.Interface.Service;
using Data.Context;
using Data.Repository;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestValidation;
using Validation.Validation;

namespace OffsetPrintingSupplies
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                //DataBuilder d = new DataBuilder();
                //PurchaseBuilder p = new PurchaseBuilder();
                //SalesBuilder s = new SalesBuilder();
                RetailPurchaseBuilder rpb = new RetailPurchaseBuilder();
                //RetailSalesBuilder rsb = new RetailSalesBuilder();

                //DataFunction(d);
                //PurchaseFunction(p);
                //SalesFunction(s);
                RetailPurchaseFunction(rpb);
                //RetailSalesFunction(rsb);
            }
        }

        public static void PurchaseFunction(PurchaseBuilder p)
        {
            p.PopulateData();

            //---

        }

        public static void SalesFunction(SalesBuilder s)
        {
            s.PopulateData();
        }

        public static void RetailSalesFunction(RetailSalesBuilder rsb)
        {
            rsb.PopulateData();

            // ---
            Receivable receivables1 = rsb._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, rsb.rsi1.Id);
            Receivable receivables2 = rsb._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, rsb.rsi2.Id);
            Receivable receivables3 = rsb._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, rsb.rsi3.Id);

            IList<ReceiptVoucherDetail> receiptVoucherDetails1 = rsb._receiptVoucherDetailService.GetObjectsByReceivableId(receivables1.Id);
            IList<ReceiptVoucherDetail> receiptVoucherDetails2 = rsb._receiptVoucherDetailService.GetObjectsByReceivableId(receivables2.Id);
            IList<ReceiptVoucherDetail> receiptVoucherDetails3 = rsb._receiptVoucherDetailService.GetObjectsByReceivableId(receivables3.Id);

            foreach (var receiptVoucherDetail in receiptVoucherDetails1)
            {
                if (!receiptVoucherDetail.IsConfirmed) Console.WriteLine("1:FALSE");
            }

            foreach (var receiptVoucherDetail in receiptVoucherDetails2)
            {
                if (!receiptVoucherDetail.IsConfirmed) Console.WriteLine("2:FALSE");
            }

            foreach (var receiptVoucherDetail in receiptVoucherDetails3)
            {
                if (!receiptVoucherDetail.IsConfirmed) Console.WriteLine("3:FALSE");
            }
            // ---
            rsb._retailSalesInvoiceService.UnpaidObject(rsb.rsi1, rsb._receiptVoucherService, rsb._receiptVoucherDetailService, 
                                                        rsb._cashBankService, rsb._receivableService, rsb._cashMutationService);
            rsb._retailSalesInvoiceService.UnpaidObject(rsb.rsi2, rsb._receiptVoucherService, rsb._receiptVoucherDetailService,
                                                        rsb._cashBankService, rsb._receivableService, rsb._cashMutationService);
            rsb._retailSalesInvoiceService.UnpaidObject(rsb.rsi3, rsb._receiptVoucherService, rsb._receiptVoucherDetailService,
                                                        rsb._cashBankService, rsb._receivableService, rsb._cashMutationService);

            rsb._retailSalesInvoiceService.UnconfirmObject(rsb.rsi1, rsb._retailSalesInvoiceDetailService, rsb._receivableService, 
                                                           rsb._receiptVoucherDetailService, rsb._warehouseItemService, rsb._warehouseService, 
                                                           rsb._itemService, rsb._barringService, rsb._stockMutationService);
            rsb._retailSalesInvoiceService.UnconfirmObject(rsb.rsi2, rsb._retailSalesInvoiceDetailService, rsb._receivableService,
                                                           rsb._receiptVoucherDetailService, rsb._warehouseItemService, rsb._warehouseService,
                                                           rsb._itemService, rsb._barringService, rsb._stockMutationService);
            rsb._retailSalesInvoiceService.UnconfirmObject(rsb.rsi3, rsb._retailSalesInvoiceDetailService, rsb._receivableService,
                                                           rsb._receiptVoucherDetailService, rsb._warehouseItemService, rsb._warehouseService,
                                                           rsb._itemService, rsb._barringService, rsb._stockMutationService);
        }

        public static void RetailPurchaseFunction(RetailPurchaseBuilder rpb)
        {
            rpb.PopulateData();

            // ---
            Payable payables1 = rpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, rpb.rpi1.Id);
            Payable payables2 = rpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, rpb.rpi2.Id);
            Payable payables3 = rpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, rpb.rpi3.Id);

            IList<PaymentVoucherDetail> paymentVoucherDetails1 = rpb._paymentVoucherDetailService.GetObjectsByPayableId(payables1.Id);
            IList<PaymentVoucherDetail> paymentVoucherDetails2 = rpb._paymentVoucherDetailService.GetObjectsByPayableId(payables2.Id);
            IList<PaymentVoucherDetail> paymentVoucherDetails3 = rpb._paymentVoucherDetailService.GetObjectsByPayableId(payables3.Id);

            foreach (var paymentVoucherDetail in paymentVoucherDetails1)
            {
                if (!paymentVoucherDetail.IsConfirmed) Console.WriteLine("1:FALSE");
            }

            foreach (var paymentVoucherDetail in paymentVoucherDetails2)
            {
                if (!paymentVoucherDetail.IsConfirmed) Console.WriteLine("2:FALSE");
            }

            foreach (var paymentVoucherDetail in paymentVoucherDetails3)
            {
                if (!paymentVoucherDetail.IsConfirmed) Console.WriteLine("3:FALSE");
            }
            // ---
            rpb._retailPurchaseInvoiceService.UnpaidObject(rpb.rpi1, rpb._paymentVoucherService, rpb._paymentVoucherDetailService,
                                                        rpb._cashBankService, rpb._payableService, rpb._cashMutationService);
            rpb._retailPurchaseInvoiceService.UnpaidObject(rpb.rpi2, rpb._paymentVoucherService, rpb._paymentVoucherDetailService,
                                                        rpb._cashBankService, rpb._payableService, rpb._cashMutationService);
            rpb._retailPurchaseInvoiceService.UnpaidObject(rpb.rpi3, rpb._paymentVoucherService, rpb._paymentVoucherDetailService,
                                                        rpb._cashBankService, rpb._payableService, rpb._cashMutationService);

            rpb._retailPurchaseInvoiceService.UnconfirmObject(rpb.rpi1, rpb._retailPurchaseInvoiceDetailService, rpb._payableService,
                                                           rpb._paymentVoucherDetailService, rpb._warehouseItemService, rpb._warehouseService,
                                                           rpb._itemService, rpb._barringService, rpb._stockMutationService);
            rpb._retailPurchaseInvoiceService.UnconfirmObject(rpb.rpi2, rpb._retailPurchaseInvoiceDetailService, rpb._payableService,
                                                           rpb._paymentVoucherDetailService, rpb._warehouseItemService, rpb._warehouseService,
                                                           rpb._itemService, rpb._barringService, rpb._stockMutationService);
            rpb._retailPurchaseInvoiceService.UnconfirmObject(rpb.rpi3, rpb._retailPurchaseInvoiceDetailService, rpb._payableService,
                                                           rpb._paymentVoucherDetailService, rpb._warehouseItemService, rpb._warehouseService,
                                                           rpb._itemService, rpb._barringService, rpb._stockMutationService);
        }

        public static void DataFunction(DataBuilder d)
        {
            d.PopulateData();

            d.item = new Item()
            {
                ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                Sku = "ABC1001",
                Name = "ABC",
                Category = "ABC123",
                UoMId = d.Pcs.Id
            };
            d.item = d._itemService.CreateObject(d.item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

            d.localWarehouse = new Warehouse()
            {
                Name = "Sentral Solusi Data",
                Description = "Kali Besar Jakarta",
                Code = "LCL"
            };
            d.localWarehouse = d._warehouseService.CreateObject(d.localWarehouse, d._warehouseItemService, d._itemService);

            d.contact = d._contactService.CreateObject("Abbey", "1 Abbey St", "001234567", "Daddy", "001234888", "abbey@abbeyst.com", d._contactGroupService);

            d.coreBuilder = new CoreBuilder()
            {
                BaseSku = "CORE1001",
                SkuNewCore = "CORE1001N",
                SkuUsedCore = "CORE1001U",
                Name = "Core X 1001",
                Category = "X 1001",
                UoMId = d.Pcs.Id
            };
            d.coreBuilder = d._coreBuilderService.CreateObject(d.coreBuilder, d._uomService, d._itemService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

            d.machine = new Machine()
            {
                Code = "M00001",
                Name = "Machine 00001",
                Description = "Machine"
            };
            d.machine = d._machineService.CreateObject(d.machine);
            d.coreIdentification = new CoreIdentification()
            {
                ContactId = d.contact.Id,
                Code = "CI0001",
                Quantity = 1,
                IdentifiedDate = DateTime.Now,
                WarehouseId = d.localWarehouse.Id
            };
            d.coreIdentification = d._coreIdentificationService.CreateObject(d.coreIdentification, d._contactService);
            d.coreIdentificationDetail = new CoreIdentificationDetail()
            {
                CoreIdentificationId = d.coreIdentification.Id,
                DetailId = 1,
                MaterialCase = 2,
                CoreBuilderId = d.coreBuilder.Id,
                RollerTypeId = d._rollerTypeService.GetObjectByName("Found DT").Id,
                MachineId = d.machine.Id,
                RD = 12,
                CD = 12,
                RL = 12,
                WL = 12,
                TL = 12
            };
            d.coreIdentificationDetail = d._coreIdentificationDetailService.CreateObject(d.coreIdentificationDetail, d._coreIdentificationService, d._coreBuilderService, d._rollerTypeService, d._machineService);

            d.coreBuilder = d._coreBuilderService.SoftDeleteObject(d.coreBuilder, d._itemService, d._rollerBuilderService, d._coreIdentificationDetailService, d._recoveryOrderDetailService, d._recoveryAccessoryDetailService,
                                                                   d._warehouseItemService, d._stockMutationService, d._itemTypeService, d._barringService, d._purchaseOrderDetailService,
                                                                   d._stockAdjustmentDetailService, d._salesOrderDetailService, d._priceMutationService);

            // Begin Test
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();

                d.Pcs = new UoM()
                {
                    Name = "Pcs"
                };
                d._uomService.CreateObject(d.Pcs);

                d.Boxes = new UoM()
                {
                    Name = "Boxes"
                };
                d._uomService.CreateObject(d.Boxes);

                d.Tubs = new UoM()
                {
                    Name = "Tubs"
                };
                d._uomService.CreateObject(d.Tubs);

                d.item = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Category = "ABC123",
                    UoMId = d.Pcs.Id
                };
                d.item = d._itemService.CreateObject(d.item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                d.groupItemPrice1 = new GroupItemPrice()
                {
                    Price = 1000,
                    ItemId = d.item.Id,
                    ContactGroupId = d.baseGroup.Id
                };
                d.groupItemPrice1 = d._groupItemPriceService.CreateObject(d.groupItemPrice1, d._contactGroupService, d._itemService, d._priceMutationService);

                GroupItemPrice groupitemprice2 = new GroupItemPrice()
                {
                    Price = 2000,
                    ItemId = d.item.Id,
                    ContactGroupId = d.baseGroup.Id
                };
                groupitemprice2 = d._groupItemPriceService.CreateObject(groupitemprice2, d._contactGroupService, d._itemService, d._priceMutationService);

                Item item = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1002",
                    Name = "CBA",
                    Category = "ABC123",
                    UoMId = d.Pcs.Id
                };
                item = d._itemService.CreateObject(item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                int oldItemId = d.groupItemPrice1.ItemId;
                int oldGroupId = d.groupItemPrice1.ContactGroupId;
                d.groupItemPrice1.ItemId = item.Id;
                GroupItemPrice oldone = d._groupItemPriceService.GetObjectById(d.groupItemPrice1.Id); // TODO : Check why oldone.ItemId have the same new value with d.groupItemPrice1.ItemId
                d.groupItemPrice1 = d._groupItemPriceService.UpdateObject(d.groupItemPrice1, oldGroupId, oldItemId, d._itemService, d._priceMutationService);

                Console.WriteLine("{0}", d.groupItemPrice1.Errors.FirstOrDefault());
            }    

            // End of Test
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}
