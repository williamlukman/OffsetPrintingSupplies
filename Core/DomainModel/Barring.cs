using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class Barring : Item
    {
        public string RollNo { get; set; }
        public int CustomerId { get; set; }
        public int MachineId { get; set; }
        public int BlanketItemId { get; set; }
        public Nullable<int> LeftBarItemId { get; set; }
        public Nullable<int> RightBarItemId { get; set; }
        public decimal AC { get; set; }
        public decimal AR { get; set; }
        public decimal thickness { get; set; }
        public decimal KS { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual ICollection<BarringOrderDetail> BarringOrderDetails { get; set; }
    }
}