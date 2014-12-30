using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;
using System.Data.Entity.Migrations;
using Core.DomainModel;
using Core.Interface.Service;

namespace Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Core.DomainModel;

    internal sealed class Configuration : DbMigrationsConfiguration<OffsetPrintingSuppliesEntities>
    {
        private IExchangeRateService _exchangeRateService;
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Data.Context.OffsetPrintingSuppliesEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );

            int? exrate = null; 
            ExchangeRate exrates = context.ExchangeRates.FirstOrDefault();
            if (exrates != null) { exrate = exrates.Id; }
            foreach (var x in context.SalesInvoices.Where(x => x.ExchangeRateId == null))
            {
                x.ExchangeRateId = exrate;
            }

            int? whid = null;
            WarehouseItem warehouseItem = context.WarehouseItems.FirstOrDefault();
            if (warehouseItem != null) { whid = warehouseItem.Id; }
            foreach (var customerItem in context.CustomerItems.Where(x => x.WarehouseItemId == null))
            {
                customerItem.WarehouseItemId = whid;
            }

            int? accountid = null;
            Account accounts = context.Accounts.FirstOrDefault();
            if (accounts != null) { accountid = accounts.Id; }
            foreach (var x in context.ItemTypes.Where(x => x.AccountId == null))
            {
                x.AccountId = accountid;
            }

            //if (context.Currencys.FirstOrDefault() == null)
            //{
            //    context.Currencys.Add(
            //     new Currency
            //     {
            //         CreatedAt = DateTime.Now,
            //         IsBase = true,
            //         Name = "IDR",
            //     }
            //     );
            //    context.SaveChanges();
            //}
           
            //Currency cr = context.Currencys.FirstOrDefault();
            //if (context.ExchangeRates.FirstOrDefault() == null)
            //{
            //  context.ExchangeRates.Add(
            //   new ExchangeRate
            //   {
            //       CreatedAt = DateTime.Now,
            //       ExRateDate = DateTime.Now,
            //       CurrencyId = cr.Id,
            //       Rate = 1
            //   }
            //   );
            //}
           
            //context.SaveChanges();

            //ExchangeRate exrates = context.ExchangeRates.FirstOrDefault();
            //foreach (var x in context.SalesInvoices.Where(x => x.ExchangeRateId == null))
            //{
            //    x.ExchangeRateId = exrates.Id;
            //}

        }
    }
}
