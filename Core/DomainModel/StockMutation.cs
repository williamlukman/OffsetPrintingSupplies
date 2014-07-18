using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class StockMutation
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int WarehouseItemId { get; set; }
        public int ItemCase { get; set; }
        public int Status { get; set; }

        public string SourceDocumentType { get; set; }
        public string SourceDocumentDetailType { get; set; }
        public int SourceDocumentId { get; set; }
        public int SourceDocumentDetailId { get; set; }

        public int Quantity { get; set; }

        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }

        public virtual WarehouseItem WarehouseItem { get; set;}
        public Dictionary<String, String> Errors { get; set; }

    }
}