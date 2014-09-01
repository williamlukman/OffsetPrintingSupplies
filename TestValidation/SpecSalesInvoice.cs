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

    public class SpecSalesInvoice : nspec
    {
        public SalesBuilder sb;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                sb = new SalesBuilder();
                sb.PopulateData();
            }
        }

        void deliveryorder_validation()
        {
            it["validates_all_variables"] = () =>
            {
                sb.localWarehouse.Errors.Count().should_be(0);
                sb.contact.Errors.Count().should_be(0);
                sb.rollBlanket1.Errors.Count().should_be(0);
                sb.rollBlanket2.Errors.Count().should_be(0);
                sb.rollBlanket3.Errors.Count().should_be(0);
                sb.cashBank.Errors.Count().should_be(0);
                sb.so1.Errors.Count().should_be(0);
                sb.so1a.Errors.Count().should_be(0);
                sb.so1b.Errors.Count().should_be(0);
                sb.so1c.Errors.Count().should_be(0);
                sb.so2.Errors.Count().should_be(0);
                sb.so2a.Errors.Count().should_be(0);
                sb.so2b.Errors.Count().should_be(0);
                sb.do1.Errors.Count().should_be(0);
                sb.do1a.Errors.Count().should_be(0);
                sb.do1b.Errors.Count().should_be(0);
                sb.do2.Errors.Count().should_be(0);
                sb.do2a.Errors.Count().should_be(0);
                sb.do2b.Errors.Count().should_be(0);
                sb.do3.Errors.Count().should_be(0);
                sb.do1c.Errors.Count().should_be(0);
                sb.do1a2.Errors.Count().should_be(0);
            };

            it["validates_completed_purchaseorder_and_receival"] = () =>
            {
                sb.so1.IsDeliveryCompleted.should_be_true();
                sb.so2.IsDeliveryCompleted.should_be_true();
                sb.do1.IsConfirmed.should_be_true();
                sb.do2.IsConfirmed.should_be_true();
                sb.do3.IsConfirmed.should_be_true();
            };

            it["validates_completed_purchasereceival_and_invoice"] = () =>
            {
                sb.do1.IsInvoiceCompleted.should_be_true();
                sb.do2.IsInvoiceCompleted.should_be_true();
                sb.do3.IsInvoiceCompleted.should_be_true();
                sb.si1.IsConfirmed.should_be_true();
                sb.si2.IsConfirmed.should_be_true();
                sb.si3.IsConfirmed.should_be_true();
            };

            it["validates_complete_payables"] = () =>
            {
                foreach (var receivable in sb._receivableService.GetAll())
                {
                    receivable.IsCompleted.should_be_true();
                }
            };
        }
    }
}