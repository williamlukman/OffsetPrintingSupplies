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

    public class SpecPurchaseInvoice : nspec
    {
        public PurchaseBuilder pb;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                pb = new PurchaseBuilder();
                pb.PopulateData();
            }
        }

        void purchasereceival_validation()
        {
            it["validates_all_variables"] = () =>
            {
                pb.localWarehouse.Errors.Count().should_be(0);
                pb.contact.Errors.Count().should_be(0);
                pb.rollBlanket1.Errors.Count().should_be(0);
                pb.rollBlanket2.Errors.Count().should_be(0);
                pb.rollBlanket3.Errors.Count().should_be(0);
                pb.cashBank.Errors.Count().should_be(0);
                pb.po1.Errors.Count().should_be(0);
                pb.po1a.Errors.Count().should_be(0);
                pb.po1b.Errors.Count().should_be(0);
                pb.po1c.Errors.Count().should_be(0);
                pb.po2.Errors.Count().should_be(0);
                pb.po2a.Errors.Count().should_be(0);
                pb.po2b.Errors.Count().should_be(0);
                pb.pr1.Errors.Count().should_be(0);
                pb.pr1a.Errors.Count().should_be(0);
                pb.pr1b.Errors.Count().should_be(0);
                pb.pr2.Errors.Count().should_be(0);
                pb.pr2a.Errors.Count().should_be(0);
                pb.pr2b.Errors.Count().should_be(0);
                pb.pr3.Errors.Count().should_be(0);
                pb.pr1c.Errors.Count().should_be(0);
                pb.pr1a2.Errors.Count().should_be(0);
            };

            it["validates_completed_purchaseorder_and_receival"] = () =>
            {
                pb.po1.IsReceivalCompleted.should_be_true();
                pb.po2.IsReceivalCompleted.should_be_true();
                pb.pr1.IsConfirmed.should_be_true();
                pb.pr2.IsConfirmed.should_be_true();
                pb.pr3.IsConfirmed.should_be_true();
            };

            it["validates_completed_purchasereceival_and_invoice"] = () =>
            {
                pb.pr1.IsInvoiceCompleted.should_be_true();
                pb.pr2.IsInvoiceCompleted.should_be_true();
                pb.pr3.IsInvoiceCompleted.should_be_true();
                pb.pi1.IsConfirmed.should_be_true();
                pb.pi2.IsConfirmed.should_be_true();
                pb.pi3.IsConfirmed.should_be_true();
            };

            it["validates_complete_payables"] = () =>
            {
                foreach (var payable in pb._payableService.GetAll())
                {
                    payable.IsCompleted.should_be_true();
                }
            };
        }
    }
}