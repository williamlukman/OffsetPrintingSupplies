using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class ValidCombIncomeStatement
    {
        // This ValidCombIncomeStatement is used to save data when Closing does its calculation on Net Earning and set all the 
        // revenue and expense valid comb to 0.
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ClosingId { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual Account Account { get; set; }
        public virtual Closing Closing { get; set; }
    }
}
