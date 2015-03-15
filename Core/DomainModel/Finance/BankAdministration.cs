using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class BankAdministration
    {
        public int Id { get; set; }
        public int CashBankId { get; set; }

        public DateTime AdministrationDate { get; set; }
        //public decimal PendapatanJasaAmount { get; set; }
        //public decimal PendapatanBungaAmount { get; set; }
        ////public decimal PajakPendapatanBungaAmount { get; set; }
        //public decimal BiayaBungaAmount { get; set; }
        //public decimal BiayaAdminAmount { get; set; }
        //public decimal PengembalianPiutangAmount { get; set; }
        public string Code { get; set; }
        public string NoBukti { get; set; }
        public decimal Amount { get; set; } //Total
        public decimal ExchangeRateAmount { get; set; }
        public Nullable<int> ExchangeRateId { get; set; }
        //public bool IsExpense { get; set; }
        public string Description { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual CashBank CashBank { get; set; }
        public virtual ICollection<BankAdministrationDetail> BankAdministrationDetails { get; set; }
        public virtual ExchangeRate ExchangeRate { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}