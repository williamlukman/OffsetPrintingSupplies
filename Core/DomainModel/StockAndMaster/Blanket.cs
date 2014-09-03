using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class Blanket : Item
    {
        public string RollNo { get; set; }
        public int ContactId { get; set; }
        public int MachineId { get; set; }
        public int AdhesiveId { get; set; }
        public int RollBlanketItemId { get; set; }
        public Nullable<int> LeftBarItemId { get; set; }
        public Nullable<int> RightBarItemId { get; set; }
        public decimal AC { get; set; }
        public decimal AR { get; set; }
        public decimal thickness { get; set; }
        public decimal KS { get; set; }

        public bool IsBarRequired { get; set; }
        public bool HasLeftBar { get; set; }
        public bool HasRightBar { get; set; }

        public string CroppingType { get; set; }
        public decimal LeftOverAC { get; set; }
        public decimal LeftOverAR { get; set; }
        public string ApplicationCase { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual Item Adhesive { get; set; }
        public virtual ICollection<BlanketOrderDetail> BlanketOrderDetails { get; set; }
        public virtual Item RollBlanketItem { get; set; }
        public virtual Item LeftBarItem { get; set; }
        public virtual Item RightBarItem { get; set; }
    }
}