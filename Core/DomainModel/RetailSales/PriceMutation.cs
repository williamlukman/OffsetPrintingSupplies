using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel.RetailSales
{
    public partial class PriceMutation
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int GroupId { get; set; }

        public int Amount { get; set; }
        public bool IsActive { get; set; }

        public Nullable<DateTime> DeactivatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Item Item { get; set; }
        public virtual Group Group { get; set; }
    }
}
