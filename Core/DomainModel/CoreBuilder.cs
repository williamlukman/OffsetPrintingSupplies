using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class CoreBuilder
    {
        public int Id { get; set; }
        public string BaseSku { get; set; }
        public string SkuUsedCore { get; set; }
        public string SkuNewCore { get; set; }
        public int UsedCoreItemId { get; set; }
        public int NewCoreItemId { get; set; }
        
        public string Name { get; set; }
        public string Category { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public ICollection<CoreIdentificationDetail> CoreIdentificationDetails { get; set; }
    }
}
