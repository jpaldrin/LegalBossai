 
using System;
using System.Linq;


namespace Application.ViewModels
{
    public class DocumentViewModel
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
        public string DocumentCategoryName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
