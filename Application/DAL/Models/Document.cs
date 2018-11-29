 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Document : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int UnitsInStock { get; set; }
        public bool IsActive { get; set; }
        public bool IsDiscontinued { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }


        public int? ParentId { get; set; }
        public Document Parent { get; set; }

        public int DocumentCategoryId { get; set; }
        public DocumentCategory DocumentCategory { get; set; }

        public ICollection<Document> Children { get; set; }
        public ICollection<EvictionDetail> EvictionDetails { get; set; }
    }
}
