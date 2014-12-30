using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class BlendingRecipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TargetItemId { get; set; }
        public decimal TargetQuantity { get; set; }

        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual Item TargetItem { get; set; }
        public virtual ICollection<BlendingRecipeDetail> BlendingRecipeDetails { get; set; }
    }
}