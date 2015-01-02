using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    /*
     * Created using Table per Type
     * http://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-2-table-per-type-tpt
     */

    public partial class Item
    {
        public int Id { get; set; }
        public int ItemTypeId { get; set; }
        public string Sku { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsTradeable { get; set; }

        public int UoMId { get; set; }
        public decimal Quantity { get; set; }
        public decimal PendingDelivery { get; set; }
        public decimal PendingReceival { get; set; }
        public decimal Virtual { get; set; }
        public decimal MinimumQuantity { get; set; }

        public decimal CustomerQuantity { get; set; }
        public decimal CustomerVirtual { get; set; } // unused ?

        public decimal SellingPrice { get; set; }
        public Nullable<int> CurrencyId { get; set; }
        public int PriceMutationId { get; set; }
        public decimal AvgPrice { get; set; }
        public decimal CustomerAvgPrice { get; set; }

        public virtual ICollection<StockMutation> StockMutations { get; set; }
        public virtual ICollection<PriceMutation> PriceMutations { get; set; }
        public virtual Currency Currency { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual UoM UoM { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual ICollection<RecoveryAccessoryDetail> RecoveryAccessoryDetails { get; set; }
        public virtual ICollection<WarehouseItem> WarehouseItems { get; set; }
        //public virtual ICollection<CustomerItem> CustomerItems { get; set; }
    }
}
