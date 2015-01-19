using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class Contact
    {
        public int Id { get; set; }
        public Nullable<int> ContactGroupId { get; set; }

        public string Name { get; set; }
        public string NamaFakturPajak { get; set; }

        public string Address { get; set; }
        public string DeliveryAddress { get; set; }
        public string NPWP { get; set; }
        public string Description { get; set; }
    
        public string ContactNo { get; set; }
        public string PIC { get; set; }
        public string PICContactNo { get; set; }
        public string Email { get; set; }
        public bool IsTaxable { get; set; }
        public string TaxCode { get; set; }
        public string ContactType { get; set; } // Customer/Supplier
        public int DefaultPaymentTerm { get; set; } // duedate days

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public ICollection<Blanket> Blankets { get; set; }
        public ICollection<CoreIdentification> CoreIdentifications { get; set; }
        public ICollection<BlanketOrder> BlanketOrders { get; set; }
        public ICollection<SalesOrder> SalesOrders { get; set; }
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public ICollection<VirtualOrder> VirtualOrders { get; set; }
        public ICollection<SalesQuotation> SalesQuotations { get; set; }
        public ICollection<ContactDetail> ContactDetails { get; set; }
        public ContactGroup ContactGroup { get; set; }
    }
}