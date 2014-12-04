using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CustomerStockMutation
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Nullable<int> ContactId { get; set; }
        public Nullable<int> CustomerItemId { get; set; }
        public Nullable<int> WarehouseItemId { get; set; }
        public int ItemCase { get; set; }
        public int Status { get; set; }

        public string SourceDocumentType { get; set; }
        public string SourceDocumentDetailType { get; set; }
        public int SourceDocumentId { get; set; }
        public int SourceDocumentDetailId { get; set; }

        public DateTime MutationDate { get; set; }
        public int Quantity { get; set; }
        
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual WarehouseItem WarehouseItem { get; set; }
        public virtual CustomerItem CustomerItem { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Item Item { get; set; }
        public Dictionary<String, String> Errors { get; set; }

    }
}