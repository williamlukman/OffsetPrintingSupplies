using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel.RetailSales;

namespace Data.Mapping.RetailSales
{
    public class RetailSalesInvoiceDetailMapping : EntityTypeConfiguration<RetailSalesInvoiceDetail>
    {
        public RetailSalesInvoiceDetailMapping()
        {
            HasKey(rsid => rsid.Id);
            HasRequired(rsid => rsid.RetailSalesInvoice)
                .WithMany(rsi => rsi.RetailSalesinvoiceDetails)
                .HasForeignKey(rsid => rsid.RetailSalesInvoiceId);
            HasRequired(rsid => rsid.PriceMutation)
                .WithMany()
                .HasForeignKey(rsid => rsid.PriceMutationId)
                .WillCascadeOnDelete(false);
            Ignore(rsid => rsid.Errors);
        }
    }
}
