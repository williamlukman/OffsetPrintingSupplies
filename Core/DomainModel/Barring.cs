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
        public int AC { get; set; }
        public int AR { get; set; }
        public int thickness { get; set; }
        public int KS { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual ICollection<BarringOrderDetail> BarringOrderDetails { get; set; }
    }
}