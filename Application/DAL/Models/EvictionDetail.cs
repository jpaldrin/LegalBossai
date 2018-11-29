 
using System;
using System.Linq;

namespace DAL.Models
{
    public class EvictionDetail : AuditableEntity
    {
        public int Id { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }

        public int DocumentId { get; set; }
        public Document Document { get; set; }

        public int EvictionId { get; set; }
        public Eviction Evctions { get; set; }
    }
}
