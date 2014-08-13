using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class RetailSalesInvoiceDetail
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Nullable<int> PriceMutationId { get; set; }
        public int RetailSalesInvoiceId { get; set; }
        
        public int Quantity { get; set; }
        public Nullable<decimal> CoGS { get; set; }
        //public decimal AssignedPrice { get; set; }
        //public bool IsManualPriceAssignment { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
            
        public Dictionary<string, string> Errors { get; set; }

        public virtual Item Item { get; set; }
        public virtual PriceMutation PriceMutation { get; set; }
        public virtual RetailSalesInvoice RetailSalesInvoice { get; set; }
    }
}
