using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class BlendingRecipeDetail
    {
        public int Id { get; set; }
        public int BlendingRecipeId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }

        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual Item Item { get; set; }
        public virtual BlendingRecipe BlendingRecipe { get; set; }
    }
}