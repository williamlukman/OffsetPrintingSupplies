using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class Repacking
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int BlendingRecipeId { get; set; }
        public string Description { get; set; }
        public DateTime RepackingDate { get; set; }
        public int WarehouseId { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual BlendingRecipe BlendingRecipe { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}