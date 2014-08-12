using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel.RetailSales;

namespace Data.Mapping.RetailSales
{
    public class RetailSalesInvoiceMapping : EntityTypeConfiguration<RetailSalesInvoice>
    {
        public RetailSalesInvoiceMapping()
        {
            HasKey(rsi => rsi.Id);
            HasMany(rsi => rsi.RetailSalesinvoiceDetails)
                .WithRequired(rsid => rsid.RetailSalesInvoice)
                .HasForeignKey(rsid => rsid.RetailSalesInvoiceId);
            Ignore(rsi => rsi.Errors);
        }
    }
}
