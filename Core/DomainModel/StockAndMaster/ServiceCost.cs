using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class ServiceCost
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int RollerBuilderId { get; set; }
        public int Quantity { get; set; }
        public decimal AvgPrice { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual Item Item { get; set; }
        public virtual RollerBuilder RollerBuilder { get; set; }
    }
}
