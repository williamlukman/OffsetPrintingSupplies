using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public class SpecRetailSalesInvoice : nspec
    {
        public RetailSalesBuilder sb;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                sb = new RetailSalesBuilder();
                sb.PopulateData();
            }
        }

        void retailsalesinvoice_validation()
        {
            it["validates_all_variables"] = () =>
            {
                sb.localWarehouse.Errors.Count().should_be(0);
                sb.contact.Errors.Count().should_be(0);
                sb.blanket1.Errors.Count().should_be(0);
                sb.blanket2.Errors.Count().should_be(0);
                sb.blanket3.Errors.Count().should_be(0);
                sb.cashBank.Errors.Count().should_be(0);
                sb.rsi1.Errors.Count().should_be(0);
                sb.rsi2.Errors.Count().should_be(0);
                sb.rsi3.Errors.Count().should_be(0);
                sb.rsid1.Errors.Count().should_be(0);
                sb.rsid2.Errors.Count().should_be(0);
                sb.rsid3.Errors.Count().should_be(0);
            };

            it["validates_confirmed_retailsalesinvoice"] = () =>
            {
                sb.rsi1.IsConfirmed.should_be_true();
                sb.rsi2.IsConfirmed.should_be_true();
                sb.rsi3.IsConfirmed.should_be_true();

            };

            it["validates_paid_retailsalesinvoice"] = () =>
            {
                sb.rsi1.IsPaid.should_be_true();
                sb.rsi2.IsPaid.should_be_true();
                sb.rsi3.IsPaid.should_be_true();
                
            };

            it["validates_receivables"] = () =>
            {
                foreach (var receivable in sb._receivableService.GetAll())
                {
                    receivable.RemainingAmount.should_be(0);
                }
            };

            context["when_unpaid_retailsalesinvoice"] = () =>
            {
                before = () =>
                {
                    sb._retailSalesInvoiceService.UnpaidObject(sb.rsi1, sb._receiptVoucherService, sb._receiptVoucherDetailService, sb._cashBankService, sb._receivableService, sb._cashMutationService);
                    sb._retailSalesInvoiceService.UnpaidObject(sb.rsi2, sb._receiptVoucherService, sb._receiptVoucherDetailService, sb._cashBankService, sb._receivableService, sb._cashMutationService);
                    sb._retailSalesInvoiceService.UnpaidObject(sb.rsi3, sb._receiptVoucherService, sb._receiptVoucherDetailService, sb._cashBankService, sb._receivableService, sb._cashMutationService);
                    sb.rsi1.Errors.Count().should_be(0);
                    sb.rsi2.Errors.Count().should_be(0);
                    sb.rsi3.Errors.Count().should_be(0);
                    sb.rsid1.Errors.Count().should_be(0);
                    sb.rsid2.Errors.Count().should_be(0);
                    sb.rsid3.Errors.Count().should_be(0);
                };

                it["validates_unpaid_retailsalesinvoice"] = () =>
                {
                    sb.rsi1.IsPaid.should_be_false();
                    sb.rsi2.IsPaid.should_be_false();
                    sb.rsi3.IsPaid.should_be_false();
                };

                context["when_unconfirm_retailsalesinvoice"] = () =>
                {
                    before = () =>
                    {
                        sb._retailSalesInvoiceService.UnconfirmObject(sb.rsi1, sb._retailSalesInvoiceDetailService, sb._receivableService, sb._receiptVoucherDetailService, sb._warehouseItemService, sb._warehouseService, sb._itemService, sb._barringService, sb._stockMutationService);
                        sb._retailSalesInvoiceService.UnconfirmObject(sb.rsi2, sb._retailSalesInvoiceDetailService, sb._receivableService, sb._receiptVoucherDetailService, sb._warehouseItemService, sb._warehouseService, sb._itemService, sb._barringService, sb._stockMutationService);
                        sb._retailSalesInvoiceService.UnconfirmObject(sb.rsi3, sb._retailSalesInvoiceDetailService, sb._receivableService, sb._receiptVoucherDetailService, sb._warehouseItemService, sb._warehouseService, sb._itemService, sb._barringService, sb._stockMutationService);
                        sb.rsi1.Errors.Count().should_be(0);
                        sb.rsi2.Errors.Count().should_be(0);
                        sb.rsi3.Errors.Count().should_be(0);
                        sb.rsid1.Errors.Count().should_be(0);
                        sb.rsid2.Errors.Count().should_be(0);
                        sb.rsid3.Errors.Count().should_be(0);
                    };

                    it["validates_unconfirmed_retailsalesinvoice"] = () =>
                    {
                        sb.rsi1.IsConfirmed.should_be_false();
                        sb.rsi2.IsConfirmed.should_be_false();
                        sb.rsi3.IsConfirmed.should_be_false();
                    };

                };

            };
        }
    }
}