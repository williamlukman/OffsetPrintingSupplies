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

    public class SpecVirtualOrder: nspec
    {

        DataBuilder d;
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();

                d.PopulateUserRole();
                d.PopulateWarehouse();
                d.PopulateItem();
                d.PopulateSingles();
                d.PopulateCashBank();

                d.PopulateVirtualOrder();
            }
        }

        void virtualorder_validation()
        {
        
            it["validates_virtualorder"] = () =>
            {
                d.virtualOrder1.IsConfirmed.should_be_true();
                d.virtualOrder2.IsConfirmed.should_be_true();
                d.temporaryDeliveryOrder1.IsConfirmed.should_be_true();
                d.temporaryDeliveryOrder2.IsConfirmed.should_be_true();
                d.tdoc1.IsConfirmed.should_be_true();
                d.tdoc2.IsConfirmed.should_be_true();
                d.tdoc3.IsConfirmed.should_be_true();
                d.tdoc4.IsConfirmed.should_be_true();
                d.tdoc5.IsConfirmed.should_be_true();
                d.sales1.IsConfirmed.should_be_true();
                d.sales2.IsConfirmed.should_be_true();
            };

            it["validates_partdeliveryorder"] = () =>
            {
                d.GramDeliveryOrder1.IsConfirmed.should_be_true();
            };
        }
    }
}