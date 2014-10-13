using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class Compound : Item
    {
        public string CompoundType { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}